using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfTemplateBaseLib;

namespace WpfTemplateLib
{
    /// <summary>
    /// Suivez les étapes 1a ou 1b puis 2 pour utiliser ce contrôle personnalisé dans un fichier XAML.
    ///
    /// Étape 1a) Utilisation de ce contrôle personnalisé dans un fichier XAML qui existe dans le projet actif.
    /// Ajoutez cet attribut XmlNamespace à l'élément racine du fichier de balisage où il doit 
    /// être utilisé :
    ///
    ///     xmlns:MyNamespace="clr-namespace:WpfTemplateLib"
    ///
    ///
    /// Étape 1b) Utilisation de ce contrôle personnalisé dans un fichier XAML qui existe dans un autre projet.
    /// Ajoutez cet attribut XmlNamespace à l'élément racine du fichier de balisage où il doit 
    /// être utilisé :
    ///
    ///     xmlns:MyNamespace="clr-namespace:WpfTemplateLib;assembly=WpfTemplateLib"
    ///
    /// Vous devrez également ajouter une référence du projet contenant le fichier XAML
    /// à ce projet et régénérer pour éviter des erreurs de compilation :
    ///
    ///     Cliquez avec le bouton droit sur le projet cible dans l'Explorateur de solutions, puis sur
    ///     "Ajouter une référence"->"Projets"->[Recherchez et sélectionnez ce projet]
    ///
    ///
    /// Étape 2)
    /// Utilisez à présent votre contrôle dans le fichier XAML.
    ///
    ///     <MyNamespace:Notification/>
    ///
    /// </summary>
    public class Notification : Control
    {
        private List<NotificationWindow> _notificationsWindows = new List<NotificationWindow>();

        static Notification()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Notification), new FrameworkPropertyMetadata(typeof(Notification)));
        }

        #region Dependency Property : NotificationTemplate

        /// <summary>
        /// Get or set the NotificationTemplate in the dataContext.
        /// </summary>
        public DataTemplate NotificationTemplate
        {
            get { return (DataTemplate)GetValue(NotificationTemplateProperty); }
            set { SetValue(NotificationTemplateProperty, value); }
        }

        private const string NotificationTemplateName = "NotificationTemplate";

        // Using a DependencyProperty as the backing store for myNamedValueProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NotificationTemplateProperty = DependencyProperty.Register(NotificationTemplateName, typeof(DataTemplate), typeof(Notification),
            new UIPropertyMetadata(null));

        #endregion Dependency Property : NotificationTemplate

        #region Dependency Property : NotificationTime

        /// <summary>
        /// Get or set the NotificationTime in the dataContext.
        /// </summary>
        public int NotificationTime
        {
            get { return (int)GetValue(NotificationTimeProperty); }
            set { SetValue(NotificationTimeProperty, value); }
        }

        private const string NotificationTimeName = "NotificationTime";

        // Using a DependencyProperty as the backing store for myNamedValueProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NotificationTimeProperty = DependencyProperty.Register(NotificationTimeName, typeof(int), typeof(Notification),
            new UIPropertyMetadata(-1));

        #endregion Dependency Property : NotificationTime

        #region Dependency Property : Notifications

        /// <summary>
        /// Get or set the Notifications in the dataContext.
        /// </summary>
        public INotifyCollectionChanged Notifications
        {
            get { return (INotifyCollectionChanged)GetValue(NotificationsProperty); }
            set { SetValue(NotificationsProperty, value); }
        }


        // Using a DependencyProperty as the backing store for myNamedValueProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NotificationsProperty = DependencyProperty.Register("Notifications", typeof(INotifyCollectionChanged), typeof(Notification),
            new UIPropertyMetadata(null, NotificationsChangedCallback));

        private static void NotificationsChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var notification = dependencyObject as Notification;
            if (null != notification)
            {
                notification.NotificationsChangedCallback(dependencyPropertyChangedEventArgs);
            }
        }

        private void NotificationsChangedCallback(DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var old = dependencyPropertyChangedEventArgs.OldValue as INotifyCollectionChanged;
            if (null != old)
            {
                old.CollectionChanged -= Notifications_CollectionChanged;
            }
            var newValue = dependencyPropertyChangedEventArgs.NewValue as INotifyCollectionChanged;
            if (null != newValue)
            {
                newValue.CollectionChanged += Notifications_CollectionChanged;
            }
        }

        void Notifications_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems == null)
            {
                return;
            }
            foreach (var item in e.NewItems)
            {
                // notification windows management
                var nw = new NotificationWindow(this.FindAncestor<Window>(), NotificationTime, NotificationTemplate, _notificationsWindows) { DataContext = item };
                _notificationsWindows.Add(nw);
                nw.Closed += nw_Closed;
                nw.Show();
            }
        }

        void nw_Closed(object sender, EventArgs e)
        {
            var nw = sender as NotificationWindow;
            if (null != nw)
            {
                var list = Notifications as IList;
                if (null != list)
                {
                    list.Remove(nw.DataContext);
                }
                _notificationsWindows.Remove(nw);
            }
        }

        #endregion Dependency Property : Notifications

        public void AddNotification(string text)
        {
            var nw = new NotificationWindow(this.FindAncestor<Window>(), NotificationTime, NotificationTemplate, _notificationsWindows, text);
            _notificationsWindows.Add(nw);
            nw.Closed += nw_Closed;
            nw.Show();
        }

    }
}
