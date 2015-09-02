using System.Windows;
using System.Windows.Media;

namespace WpfTemplateBaseLib
{
    public static class ExtensionMethod
    {
        public static T FindAncestor<T>(this DependencyObject element)
            where T : class
        {
            var elem = element;
            while (elem != null && !(elem is T))
            {
                elem = VisualTreeHelper.GetParent(elem);
            }
            return elem as T;
        }
    }
}