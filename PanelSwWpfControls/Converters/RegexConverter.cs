using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Data;
using System.Windows.Markup;

namespace PanelSW.WPF.Controls.Converters
{
    public class RegexConverter : MarkupExtension, IValueConverter
    {
        public string RegexExpression { get; set; }

        public string Replacement { get; set; }
        
        public string ConvertBackRegexExpression { get; set; }

        public string ConvertBackReplacement { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Replace(value?.ToString(), RegexExpression, Replacement);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Replace(value?.ToString(), ConvertBackRegexExpression, ConvertBackReplacement);
        }

        private object Replace(string input, string regex, string replacement)
        {
            if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(regex))
            {
                return input;
            }

            input = Regex.Replace(input, RegexExpression, Replacement);
            return input;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}