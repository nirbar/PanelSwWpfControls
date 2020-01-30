using System;
using System.Collections;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace PanelSW.WPF.Controls.Converters
{
    /// <summary>
    /// Compare value to supplied objects. If all objects equal value, return <see cref="EqualValue"/>; Otherwise, return <see cref="InequalValue"/>.
    /// If ConverterParameter is provided it is handles like part of <see cref="Collection"/>
    /// <para>
    /// When converting back, value is compared to <see cref="EqualValue"/>. If they are equal, then <see cref="ConvertBackEqualValue"/> is returned.
    /// Otherwise, value is compared to <see cref="InequalValue"/>. If they are equal, then <see cref="ConvertBackInequalValue"/> is returned.
    /// Last, if neither the above compared equal, then null is returned.
    /// </para>
    /// </summary>
    [ValueConversion(typeof(object), typeof(object))]
    public class EqualityConverter : MarkupExtension, IValueConverter
    {
        /// <summary>
        /// Collection of objects to compare to value
        /// </summary>
        public IEnumerable Collection { get; set; } = new ArrayList();

        /// <summary>
        /// Value to return when converting back <see cref="InequalValue"/>
        /// </summary>
        public object ConvertBackInequalValue { get; set; }

        /// <summary>
        /// Value to return when converting back <see cref="EqualValue"/>
        /// </summary>
        public object ConvertBackEqualValue { get; set; }

        /// <summary>
        /// Value to return when value is equal to all objects in <see cref="Collection"/>. Defaults to true
        /// </summary>
        public object EqualValue { get; set; } = true;

        /// <summary>
        /// Value to return when value is not equal to all objects in <see cref="Collection"/>. Defaults to false
        /// </summary>
        public object InequalValue { get; set; } = false;

        /// <summary>
        /// Convertion method
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter != null)
            {
                if (!parameter.Equals(value))
                {
                    return InequalValue;
                }
            }

            if (Collection == null)
            {
                return (value == null);
            }

            foreach (object o in Collection)
            {
                // If both are null, we'll continue; If just one is null, we'll return false; If neither is null, then actual comparison will be made.
                if ((o?.Equals(value) ?? value?.Equals(o)) == false)
                {
                    return InequalValue;
                }
            }

            return EqualValue;
        }

        /// <summary>
        /// Convert-back method
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((EqualValue?.Equals(value) ?? value?.Equals(EqualValue)) != false) // Equal, or both null
            {
                return ConvertBackEqualValue;
            }
            if ((InequalValue?.Equals(value) ?? value?.Equals(InequalValue)) != false)
            {
                return ConvertBackInequalValue;
            }

            return null;
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