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

        /// <summary>
        /// Simplify type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string Simplify(string type)
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

        /// <summary>
        /// Extract type name from type declaration.
        /// For "VARCHAR(50) NOT NULL" function will return "VARCHAR" only.
        /// </summary>
        /// <param name="declaration"></param>
        /// <returns></returns>
        public static string ExtractType(string declaration)
        {
            if (string.IsNullOrEmpty(declaration))
                return declaration;
            Match match = Regex.Match(declaration, Energy.Base.Expression.SqlTypeDeclaration);
            if (!match.Success)
                return declaration;
            string simple = match.Groups["type"].Value;
            return simple;
        }
    }
}
