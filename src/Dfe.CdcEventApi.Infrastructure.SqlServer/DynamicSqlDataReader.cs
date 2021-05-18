namespace Dfe.CdcEventApi.Infrastructure.SqlServer
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Dynamic;

    /// <summary>
    /// Handles conversion of <see cref="SqlDataReader"/> data streams to dynamic objects via teh <see cref="ExpandoObject"/>.
    /// </summary>
    public static class DynamicSqlDataReader
    {
        /// <summary>
        /// Reads the <see cref="SqlDataReader"/> and converts it to an <see cref="IEnumerable{dynamic}"/>.
        /// </summary>
        /// <param name="reader">the prepared <see cref="SqlDataReader"/>.</param>
        /// <returns>An <see cref="IEnumerable{dynamic}"/>.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "Handled by client for DTO's")]
        public static IEnumerable<dynamic> Read(SqlDataReader reader)
        {
            while (reader.Read())
            {
                yield return ToExpando(reader);
            }
        }

        private static dynamic ToExpando(IDataRecord record)
        {
            var expandoObject = new ExpandoObject() as IDictionary<string, object>;

            for (var i = 0; i < record.FieldCount; i++)
            {
                expandoObject.Add(record.GetName(i), record[i]);
            }

            return expandoObject;
        }
    }
}