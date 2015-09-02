using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace WpfTemplateLib
{
    public class TrackPositionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var track =  value as Track;
            if (null != track)
            {
                var x = (track.PointFromScreen(new Point(0, 0)) - track.Thumb.PointFromScreen(new Point(0, 0))).X;
                return x;
            }
            return 0.0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}