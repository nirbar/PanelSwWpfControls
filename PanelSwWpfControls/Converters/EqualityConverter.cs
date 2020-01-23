using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace PanelSW.WPF.Controls.Converters
{
    [ValueConversion(typeof(object), typeof(bool))]
    public class EqualityConverter : MarkupExtension, IValueConverter
    {
        public IEnumerable<object> ComparedObjects { get; set; }

        public object ConvertBackFalseValue { get; set; }

        public object ConvertBackTrueValue { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (ComparedObjects == null)
            {
                return (value == null);
            }

            foreach (object o in ComparedObjects)
            {
                // If both are null, we'll continue; If just one is null, we'll return false; If neither is null, then actual comparison will be made.
                if ((o?.Equals(value) ?? value?.Equals(o)) == false)
                {
                    return false;
                }
            }

            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool b)
            {
                return b ? ConvertBackTrueValue : ConvertBackFalseValue;
            }

            return null;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}