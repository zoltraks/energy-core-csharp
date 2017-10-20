using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Energy.Query
{
    public class Type
    {
        public static bool IsNumeric(string type)
        {
            type = Simplify(type);
            return type == "NUMBER" || type == "INTEGER";
        }

        /// <summary>
        /// Simplify type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static string Simplify(string type)
        {
            string simple = ExtractType(type);
            if (string.IsNullOrEmpty(simple))
                return type;
            switch (simple.ToUpper())
            {
                case "BIT":
                    return "INTEGER";
                case "BOOL":
                    return "BOOL";
                case "BINARY":
                case "VARBINARY":
                case "BINARY VARYING":
                    return "TEXT";
                case "INTEGER":
                    return "INTEGER";
                case "SMALLINT":
                    return "INTEGER";
                case "TINYINT":
                    return "INTEGER";
                case "MEDIUMINT":
                    return "INTEGER";
                case "VARCHAR":
                case "NVACHAR":
                case "CHARACTER VARYING":
                    return "TEXT";
                case "TEXT":
                case "MEDIUMTEXT":
                case "LONGTEXT":
                    return "TEXT";
                case "DATETIME":
                case "DATETIME2":
                    return "DATETIME";
                case "DATE":
                    return "DATE";
                case "TIME":
                    return "TIME";
                case "TIMESTAMP":
                    return "TIMESTAMP";
                case "FLOAT":
                case "REAL":
                case "DOUBLE":
                case "DOUBLE PRECISION":
                    return "NUMBER";
                case "NUMERIC":
                case "DECIMAL":
                    return "NUMBER";
                case "CHAR":
                    return "TEXT";
                case "XML":
                    return "TEXT";
                case "SET":
                case "ENUM":
                    return "SET";
            }
            return simple;
        }

        private static string ExtractType(string type)
        {
            if (string.IsNullOrEmpty(type))
                return type;
            Match match = Regex.Match(type, Energy.Base.Expression.SqlColumnTypeSimple);
            if (!match.Success)
                return type;
            string simple = match.Groups["type"].Value;
            return simple;
        }
    }
}
