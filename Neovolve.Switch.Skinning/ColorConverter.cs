namespace Neovolve.Switch.Skinning
{
    using System;
    using System.ComponentModel;
    using System.Drawing;

    /// <summary>
    /// The <see cref="ColorConverter"/>
    /// class is used to convert to and from the <see cref="Color"/> type.
    /// </summary>
    public class ColorConverter : TypeConverter
    {
        /// <summary>
        /// Returns whether this converter can convert an object of the given type to the type of this converter, using the specified context.
        /// </summary>
        /// <param name="context">
        /// An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that provides a format context.
        /// </param>
        /// <param name="sourceType">
        /// A <see cref="T:System.Type"/> that represents the type you want to convert from.
        /// </param>
        /// <returns>
        /// True if this converter can perform the conversion; otherwise, false.
        /// </returns>
        public override Boolean CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType.Equals(typeof(String)))
            {
                return true;
            }

            return base.CanConvertFrom(context, sourceType);
        }

        /// <summary>
        /// Returns whether this converter can convert the object to the specified type, using the specified context.
        /// </summary>
        /// <param name="context">
        /// An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that provides a format context.
        /// </param>
        /// <param name="destinationType">
        /// A <see cref="T:System.Type"/> that represents the type you want to convert to.
        /// </param>
        /// <returns>
        /// True if this converter can perform the conversion; otherwise, false.
        /// </returns>
        public override Boolean CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType.Equals(typeof(String)))
            {
                return true;
            }

            return base.CanConvertTo(context, destinationType);
        }

        /// <summary>
        /// Converts the given object to the type of this converter, using the specified context and culture information.
        /// </summary>
        /// <param name="context">
        /// An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that provides a format context.
        /// </param>
        /// <param name="culture">
        /// The <see cref="T:System.Globalization.CultureInfo"/> to use as the current culture.
        /// </param>
        /// <param name="value">
        /// The <see cref="T:System.Object"/> to convert.
        /// </param>
        /// <returns>
        /// An <see cref="T:System.Object"/> that represents the converted value.
        /// </returns>
        /// <exception cref="T:System.NotSupportedException">
        /// The conversion cannot be performed. 
        /// </exception>
        public override Object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, Object value)
        {
            String castValue = value as String;

            if (castValue != null)
            {
                // Check if the value is in the format #XXXXXX
                if (castValue.StartsWith("#", StringComparison.OrdinalIgnoreCase))
                {
                    return ColorTranslator.FromHtml(castValue);
                }

                throw new InvalidCastException("Unable to convert the string '" + castValue + " to " + typeof(Color).FullName + ".");
            }

            return base.ConvertFrom(context, culture, value);
        }

        /// <summary>
        /// Converts the given value object to the specified type, using the specified context and culture information.
        /// </summary>
        /// <param name="context">
        /// An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that provides a format context.
        /// </param>
        /// <param name="culture">
        /// A <see cref="T:System.Globalization.CultureInfo"/>. If null is passed, the current culture is assumed.
        /// </param>
        /// <param name="value">
        /// The <see cref="T:System.Object"/> to convert.
        /// </param>
        /// <param name="destinationType">
        /// The <see cref="T:System.Type"/> to convert the <paramref name="value"/> parameter to.
        /// </param>
        /// <returns>
        /// An <see cref="T:System.Object"/> that represents the converted value.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// The <paramref name="destinationType"/> parameter is null. 
        /// </exception>
        /// <exception cref="T:System.NotSupportedException">
        /// The conversion cannot be performed. 
        /// </exception>
        public override Object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, Object value, Type destinationType)
        {
            Color castValue = (Color)value;

            if (destinationType.Equals(typeof(String)))
            {
                return ColorTranslator.ToHtml(castValue);
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}