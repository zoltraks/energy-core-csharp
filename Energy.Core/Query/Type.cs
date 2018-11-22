using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Energy.Query
{
    public class Type
    {
        #region Private

        private bool _TreatBitAsBool = false;

        private Dictionary<string, Energy.Enumeration.BasicType> _BasicTypeMap;

        #endregion

        #region Property

        /// <summary>
        /// Treat BIT type the same as BOOL.
        /// </summary>
        public bool TreatBitAsBool
        {
            get
            {
                return _TreatBitAsBool;
            }
            set
            {
                _TreatBitAsBool = value;
            }
        }

        public Dictionary<string, Energy.Enumeration.BasicType> BasicTypeMap
        {
            get
            {
                if (_BasicTypeMap == null)
                    _BasicTypeMap = CreateBasicTypeMap();
                return _BasicTypeMap;
            }
        }

        #endregion

        #region Private

        private Dictionary<string, Energy.Enumeration.BasicType> CreateBasicTypeMap()
        {
            Dictionary<string, Energy.Enumeration.BasicType> d = new Dictionary<string, Energy.Enumeration.BasicType>();
            d["TEXT"] = Enumeration.BasicType.Text;
            d["BOOL"] = Enumeration.BasicType.Bool;
            d["NUMBER"] = Enumeration.BasicType.Number;
            d["INTEGER"] = Enumeration.BasicType.Integer;
            d["TIME"] = Enumeration.BasicType.Time;
            d["DATE"] = Enumeration.BasicType.Date;
            d["DATETIME"] = Enumeration.BasicType.Stamp;
            return d;
        }

        #endregion

        #region IsNumeric

        /// <summary>
        /// Check if SQL type is numeric.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsNumeric(string type)
        {
            type = Simplify(type);
            return type == "NUMBER" || type == "INTEGER";
        }

        #endregion

        #region IsNullable

        /// <summary>
        /// Check if SQL type is numeric.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsNullable(string type)
        {
            string nullable = ExtractTypeNull(type);
            return nullable != "NOT NULL";
        }

        #endregion

        #region Simplify

        /// <summary>
        /// Simplify type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string Simplify(string type)
        {
            string simple = ExtractTypeName(type);
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
                case "NVARCHAR":
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
                case "NCHAR":
                    return "TEXT";
                case "XML":
                    return "TEXT";
                case "SET":
                case "ENUM":
                    return "SET";
            }
            return simple;
        }

        #region Extract

        /// <summary>
        /// Extract type name from type declaration.
        /// For "VARCHAR(50) NOT NULL" function will return "VARCHAR" only.
        /// </summary>
        /// <param name="declaration"></param>
        /// <returns></returns>
        public static string ExtractTypeName(string declaration)
        {
            if (string.IsNullOrEmpty(declaration))
                return declaration;
            Match match = Regex.Match(declaration, Energy.Base.Expression.SqlTypeDeclaration);
            if (!match.Success)
                return declaration;
            string simple = match.Groups["type"].Value;
            return simple;
        }

        #endregion

        /// <summary>
        /// Extract nullability from type declaration.
        /// For "VARCHAR(50) not null" function will return "NOT NULL".
        /// For empty declaration function results empty value.
        /// </summary>
        /// <param name="declaration"></param>
        /// <returns></returns>
        public static string ExtractTypeNull(string declaration)
        {
            if (string.IsNullOrEmpty(declaration))
                return "";
            Match match = Regex.Match(declaration, Energy.Base.Expression.SqlTypeDeclaration);
            if (!match.Success)
                return "";
            string value = match.Groups["null"].Value;
            value = value.ToUpper().StartsWith("NOT") ? "NOT NULL" : "NULL";
            return value;
        }

        #endregion
    }
}
