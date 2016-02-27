using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WpfTemplateLib
{
    /// <summary>
    /// Logique d'interaction pour NotificationWindow.xaml
    /// </summary>
    public partial class NotificationWindow : Window
    {
        private bool _isClosed = false;
        private readonly Timer _timer;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mainWindow">Window on which the notification will be opened</param>
        /// <param name="timeOpened">time(ms) before the notification is closed (0 means no automatic close)</param>
        /// <param name="contentTemplate">Template to display</param>
        /// <param name="notificationWindows"></param>
        /// <param name="text"></param>
        public NotificationWindow(Window mainWindow, int timeOpened, DataTemplate contentTemplate, IEnumerable<NotificationWindow> notificationWindows, string text = null)
        {
            InitializeComponent();

            if (null != contentTemplate)
            {
                Control.ContentTemplate = contentTemplate;
            }

            if (null != text)
            {
                Control.Content = text;
            }

            _timer = new Timer(timeOpened);
            _timer.Elapsed += timer_Elapsed;
            Loaded += NotificationWindow_Loaded;

            if (mainWindow != null)
            {
                var point = mainWindow.PointToScreen(new Point(0, 0));
                point.Offset(mainWindow.ActualWidth - 23, mainWindow.ActualHeight - 50);

                this.Left = point.X;
                this.Top = point.Y;
            }

            Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new Action(() =>
            {
                Visibility = Visibility.Visible;


                if (mainWindow != null)
                {

                    var point = mainWindow.PointToScreen(new Point(0, 0));
                    point.Offset(mainWindow.ActualWidth - 23, mainWindow.ActualHeight - 50);

                    this.Left = point.X - this.ActualWidth;
                    this.Top = point.Y - this.ActualHeight;
                }

                foreach (var notificationWindow in notificationWindows)
                {
                    if (notificationWindow != this)
                    {
                        notificationWindow.Top -= this.ActualHeight + 5;
                    }
                }
            }));
        }

        void NotificationWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _timer.Start(); 
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!_isClosed)
            {
                Dispatcher.BeginInvoke(new Action(Close));
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            _isClosed = true;
            base.OnClosed(e);
            _timer.Stop();
        }
    }
}
