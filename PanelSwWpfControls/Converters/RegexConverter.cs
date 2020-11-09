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
        private Regex regex_ = null;
        /// <summary>
        /// Regex expression to apply
        /// </summary>
        public string RegexExpression { get => regex_?.ToString(); set => regex_ = new Regex(value, RegexOptions.Compiled); }

        /// <summary>
        /// Regex replacement on all matches
        /// </summary>
        public string Replacement { get; set; }

        private Regex backRegex_ = null;
        /// <summary>
        /// Regex expression to apply when converting back
        /// </summary>
        public string ConvertBackRegexExpression { get => backRegex_?.ToString(); set => backRegex_ = new Regex(value, RegexOptions.Compiled); }

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
            return Replace(value?.ToString(), regex_, Replacement);
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
            return Replace(value?.ToString(), backRegex_, ConvertBackReplacement);
        }

        private object Replace(string input, Regex regex, string replacement)
        {
            if ((input == null) || (regex == null))
            {
                return input;
            }

            input = regex.Replace(input, replacement);
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