using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;

namespace WpfTemplateBaseLib
{
    public class EventBinding : MarkupExtension
    {
        public EventBinding()
        {

        }

        private bool _isInitialized = false;
        private object _source = null;

        private object _targetObject;
        private object _targetProperty;
        private Control _control;

        public string EventName { get; set; }
        public string EventOwner { get; set; }

        public string ElementName { get; set; }

        [DefaultValue(null)]
        public IValueConverter Converter { get; set; }

        public object ConverterParameter { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (!_isInitialized)
            {
                var pvt = serviceProvider as IProvideValueTarget;
                if (pvt == null)
                {
                    return null;
                }

                var frameworkElement = pvt.TargetObject as FrameworkElement;

                _targetObject = frameworkElement;
                _targetProperty = pvt.TargetProperty;
                if (frameworkElement == null)
                {
                    return this;
                }

                _control = frameworkElement.FindAncestor<Control>();
                if (_control == null)
                {
                    return this;
                }

                _control.Loaded += ControlOnLoaded;
            }
            _isInitialized = true;

            return Converter.Convert(_source, typeof(object), ConverterParameter, CultureInfo.CurrentUICulture);
        }

        private void ControlOnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            // event initialization
            var type = _control.GetType();

            _source = type.GetMethod("GetTemplateChild",
                BindingFlags.NonPublic | BindingFlags.Instance).Invoke(_control, new object[] { ElementName });

            object eventOwner;
            if (!string.IsNullOrEmpty(EventOwner))
            {
                eventOwner = type.GetMethod("GetTemplateChild",
                    BindingFlags.NonPublic | BindingFlags.Instance).Invoke(_control, new object[] { EventOwner });
            }
            else
            {
                eventOwner = _control;
            }

            if (!string.IsNullOrEmpty(EventName))
            {
                var evnt = eventOwner.GetType().GetEvent(EventName);
                var delg = Delegate.CreateDelegate(evnt.EventHandlerType, this, "EventPerformed");

                evnt.AddEventHandler(eventOwner, delg);
            }
            EventPerformed(_source, new EventArgs());
        }

        private void EventPerformed(object sender, EventArgs args)
        {
            _targetObject.GetType().GetProperty(((DependencyProperty)_targetProperty).Name).SetValue(_targetObject, ProvideValue(new ServiceProviders()));
        }
    }
}