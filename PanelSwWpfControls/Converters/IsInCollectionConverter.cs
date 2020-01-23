using System;
using System.Collections.Generic;
using System.Windows.Data;
using System.Windows.Markup;

namespace PanelSW.WPF.Controls.Converters
{
    [ValueConversion(typeof(object), typeof(bool))]
    public class IsInCollectionConverter : MarkupExtension, IValueConverter
    {
        public IEnumerable<object> Collection { get; set; }

        public object ConvertBackTrueValue { get; set; }

        public object ConvertBackFalseValue { get; set; }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (Collection == null)
            {
                return false;
            }

            foreach (object o in Collection)
            {
                if ((o == null) && (value == null))
                {
                    return true;
                }
                if ((o?.Equals(value) ?? value?.Equals(o))==true)
                {
                    return true;
                }
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (value is bool b) ? b ? ConvertBackTrueValue : ConvertBackFalseValue : null;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
