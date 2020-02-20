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
            /// Represents simplified type name.
            /// </summary>
            public string Simple;

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

            public string Option;

            public string Value;

            /// <summary>
            /// Size option.
            /// </summary>
            public string Size;

            public string Extra;

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
            string simple = Simplify(type);
            return simple == "NUMBER" || simple == "INTEGER";
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
            return Simplify(type, 0);
        }

        /// <summary>
        /// Simplify type.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static string Simplify(string type, int size)
        {
            if (string.IsNullOrEmpty(type))
                return type;
            string simple = Energy.Base.Text.ReplaceWhite(type, " ").Trim();
            if (string.IsNullOrEmpty(simple))
                return type;
            switch (simple.ToUpper())
            {
                default:
                    return "";

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
                    if (size == 1)
                        return "BOOL";
                    return "INTEGER";
                case "MEDIUMINT":
                    return "INTEGER";
                case "VARCHAR":
                case "NVARCHAR":
                case "CHARACTER VARYING":
                    if (size == 1)
                        return "CHAR";
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
                case "CHAR":
                case "NCHAR":
                case "CHARACTER":
                    return "CHAR";
            }
            //return simple;
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
            return ExtractTypeDefinition(declaration).Type;
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
            return ExtractTypeDefinition(declaration).Null;
        }

        /// <summary>
        /// Extract SQL data type for declaration string like "VARCHAR(50)".
        /// </summary>
        /// <param name="declaration"></param>
        /// <returns></returns>
        public static Definition ExtractTypeDefinition(string declaration)
        {
            string pattern;
            Match match;
            Definition result = default(Definition);

            pattern = Energy.Base.Expression.SqlTypeGenericDefinition;
            match = Regex.Match(declaration, pattern, RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);

            if (match.Success)
            {
                result = new Definition()
                {
                    Type = Energy.Base.Text.ReplaceWhite(Energy.Base.Text.Upper(match.Groups["type"].Value), " "),
                    Simple = Simplify(match.Groups["type"].Value, Energy.Base.Cast.AsInteger(match.Groups["size"].Value)),
                    Null = Energy.Base.Text.Upper(Energy.Base.Text.ReplaceWhite(match.Groups["null"].Value, " ")),
                    Default = match.Groups["default"].Value,
                    Size = match.Groups["size"].Value,
                    Extra = match.Groups["extra"].Value,
                    Option = match.Groups["option"].Value,
                    Value = match.Groups["value"].Value,
                    Parameter = match.Groups["parameter"].Value,
                };

                return result;
            }

            //if (match.Success)
            //{
            //    result = new Definition();
            //}

            //pattern = Energy.Base.Expression.SqlDeclarationToken;
            //match = Regex.Match(declaration, pattern);

            return result;
        }

        #endregion
    }
}
