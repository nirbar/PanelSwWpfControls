using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace PanelSW.WPF.Controls.Converters
{
    [ValueConversion(typeof(object), typeof(Visibility))]
    public class IsInCollectionToVisibilityConverter : MarkupExtension, IValueConverter
    {
        public IEnumerable<object> Collection { get; set; }

        public object ConvertBackVisibleValue { get; set; }

        public object ConvertBackCollapsedValue { get; set; }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (Collection == null)
            {
                return Visibility.Collapsed;
            }

            foreach (object o in Collection)
            {
                if ((o == null) && (value == null))
                {
                    return Visibility.Visible;
                }
                if ((o?.Equals(value) ?? value?.Equals(o)) == true)
                {
                    return Visibility.Visible;
                }
            }

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (value is Visibility v) ? (v == Visibility.Visible) ? ConvertBackVisibleValue : ConvertBackCollapsedValue;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
