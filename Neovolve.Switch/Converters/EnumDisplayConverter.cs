namespace Neovolve.Switch.Converters
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Windows.Data;

    /// <summary>
    /// The <see cref="EnumDisplayConverter"/>
    ///   class is used to convert to an enum value to its display value.
    /// </summary>
    public class EnumDisplayConverter : IValueConverter
    {
        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        /// <param name="value">
        /// The value produced by the binding source.
        /// </param>
        /// <param name="targetType">
        /// The type of the binding target property.
        /// </param>
        /// <param name="parameter">
        /// The converter parameter to use.
        /// </param>
        /// <param name="culture">
        /// The culture to use in the converter.
        /// </param>
        public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            Array values = value as Array;

            if (values == null)
            {
                return AddSpaces(value);
            }

            return from Object item in values
                   select AddSpaces(item);
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        /// <param name="value">
        /// The value that is produced by the binding target.
        /// </param>
        /// <param name="targetType">
        /// The type to convert to.
        /// </param>
        /// <param name="parameter">
        /// The converter parameter to use.
        /// </param>
        /// <param name="culture">
        /// The culture to use in the converter.
        /// </param>
        public Object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            Array values = value as Array;

            if (values == null)
            {
                return RemoveSpaces(value, targetType);
            }

            return from Object item in values
                   select RemoveSpaces(item, targetType);
        }

        /// <summary>
        /// Formats the item.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// A <see cref="String"/> instance.
        /// </returns>
        private static String AddSpaces(Object value)
        {
            if (value == null)
            {
                return String.Empty;
            }

            String name;

            Type valueType = value.GetType();

            if (valueType.IsEnum)
            {
                name = Enum.GetName(valueType, value);
            }
            else
            {
                throw new InvalidOperationException("Conversion is only supported for enum values.");
            }

            if (String.IsNullOrWhiteSpace(name))
            {
                return String.Empty;
            }

            String output = String.Empty;

            foreach (Char letter in name)
            {
                if (Char.IsUpper(letter) && output.Length > 0)
                {
                    output += " ";
                }

                output += letter;
            }

            return output;
        }

        /// <summary>
        /// Removes the spaces.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="targetType">
        /// Type of the target.
        /// </param>
        /// <returns>
        /// A <see cref="String"/> instance.
        /// </returns>
        private static Object RemoveSpaces(Object value, Type targetType)
        {
            String valueToConvert = value as String;

            if (String.IsNullOrWhiteSpace(valueToConvert))
            {
                throw new InvalidOperationException("Unable to convert empty string to enum value");
            }

            if (targetType.IsEnum == false)
            {
                throw new InvalidOperationException("Conversion is only supported for enum values.");
            }

            String newValue = valueToConvert.Replace(" ", String.Empty);

            return Enum.Parse(targetType, newValue);
        }
    }
}