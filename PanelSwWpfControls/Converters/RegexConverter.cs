using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Data;
using System.Windows.Markup;

namespace PanelSW.WPF.Controls.Converters
{
    /// <summary>
    /// Apply <see cref="Regex.Replace(string, string)"/> on value
    /// </summary>
    [ValueConversion(typeof(string), typeof(string))]
    public class RegexConverter : MarkupExtension, IValueConverter
    {
        /// <summary>
        /// Regex expression to apply
        /// </summary>
        public string RegexExpression { get; set; }

        /// <summary>
        /// Regex replacement on all matches
        /// </summary>
        public string Replacement { get; set; }
        
        /// <summary>
        /// Regex expression to apply when converting back
        /// </summary>
        public string ConvertBackRegexExpression { get; set; }

        /// <summary>
        /// Regex replacement on all matches when converting back
        /// </summary>
        public string ConvertBackReplacement { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Replace(value?.ToString(), RegexExpression, Replacement);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
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

            input = Regex.Replace(input, regex, replacement);
            return input;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}