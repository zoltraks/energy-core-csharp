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

        /// <summary>
        /// Dictionary of basic data types.
        /// </summary>
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

        #region Definition

        /// <summary>
        /// Represents SQL database type definition from a string like "NVARCHAR(20) NOT NULL".
        /// </summary>
        public class Definition
        {
            /// <summary>
            /// Represents type name.
            /// Example "VARCHAR".
            /// </summary>
            public string Type;

            /// <summary>
            /// Represents type parameter string. 
            /// Example: "(9,2)".
            /// </summary>
            public string Parameter;

            /// <summary>
            /// Represents default option.
            /// Example: "DEFAULT ''"
            /// </summary>
            public string Default;

            /// <summary>
            /// Represents nullable.
            /// Example "NOT NULL", "NULL".
            /// </summary>
            public string Null;

            public override string ToString()
            {
                return Energy.Base.Text.Join(" ", new string[]
                {
                    Type, Parameter, Default, Null
                });
            }
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
                    return "BOOL";
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
                case "DATETIMEOFFSET":
                case "SMALLDATETIME":
                    return "STAMP";
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
                case "UNIQUEIDENTIFIER":
                    return "TEXT";
                case "XML":
                    return "TEXT";
                case "MONEY":
                case "SMALLMONEY":
                    return "NUMBER";
                case "SET":
                case "ENUM":
                    return "SET";
            }
            return simple;
        }

        #endregion

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

        public static Definition ExtractTypeDefinition(string declaration)
        {
            string pattern = Energy.Base.Expression.SqlDeclarationToken;
        }

        #endregion
    }
}
