namespace Dfe.CdcEventApi.Domain
{
    using System;
    using System.ComponentModel;
    using System.Reflection;

    /// <summary>
    /// Extensions for <see cref="Enum"/> processing.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Parses a <see cref="int"/> to an equivalent <see cref="Enum"/> member.
        /// </summary>
        /// <typeparam name="T">The <see cref="Enum"/> <see cref="Type"/>.</typeparam>
        /// <param name="source">The subject member value as <see cref="int"/>.</param>
        /// <returns>An <see cref="Enum"/> member corresponding to the value provided.</returns>
        public static T ToEnum<T>(this int source)
        {
            return (T)Enum.Parse(typeof(T), $"{source}");
        }

        /// <summary>
        /// Exposes the <see cref="DescriptionAttribute"/> content of any <see cref="Enum"/> type.
        /// </summary>
        /// <typeparam name="T">The <see cref="Type"/> of variable to examine.</typeparam>
        /// <param name="source">The source variable instance.</param>
        /// <returns>
        /// A <see cref="string"/> containing the value of the <see cref="DescriptionAttribute"/> on that source variable. If no member exists the returned value is the string of the source value.
        /// </returns>
        public static string ToEnumDescription<T>(this T source)
            where T : Enum
        {
            FieldInfo fi = source.GetType().GetField(source.ToString());

            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
            {
                return attributes[0].Description;
            }
            else
            {
                return source.ToString();
            }
        }
    }
}