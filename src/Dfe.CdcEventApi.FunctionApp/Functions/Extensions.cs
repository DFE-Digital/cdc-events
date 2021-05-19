namespace Dfe.CdcEventApi.FunctionApp.Functions
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
        /// Parses a string to an equivalent <see cref="Enum"/> member.
        /// </summary>
        /// <typeparam name="T">The <see cref="Enum"/> <see cref="Type"/>.</typeparam>
        /// <param name="enumString">The subject member value as <see cref="string"/>.</param>
        /// <returns>An <see cref="Enum"/> member corresponding to the value provided.</returns>
        public static T ToEnum<T>(this string enumString)
        {
            return (T)Enum.Parse(typeof(T), enumString);
        }

        /// <summary>
        /// Exposes the <see cref="DescriptionAttribute"/> content of any type.
        /// </summary>
        /// <typeparam name="T">The <see cref="Type"/> of variable to examine.</typeparam>
        /// <param name="source">The source variable instance.</param>
        /// <returns>
        /// A <see cref="string"/> containing the value of the <see cref="DescriptionAttribute"/> on that source variable.
        /// </returns>
        public static string ToDescription<T>(this T source)
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