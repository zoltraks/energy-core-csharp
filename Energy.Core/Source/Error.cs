using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Source
{
    /// <summary>
    /// Database exception helper
    /// </summary>
    public class Error
    {
        #region Result

        /// <summary>
        /// Exception catch result
        /// </summary>
        public class Result
        {
            bool access = false;
            /// <summary>
            /// Access error
            /// </summary>
            public bool Access { get { return access; } set { access = value; } }

            bool timeout = false;
            /// <summary>
            /// Timeout
            /// </summary>
            public bool Timeout { get { return timeout; } set { timeout = value; } }

            bool damage = false;
            /// <summary>
            /// Connection damaged
            /// </summary>
            public bool Damage { get { return damage; } set { damage = value; } }

            bool connection = false;
            /// <summary>
            /// Connection error
            /// </summary>
            public bool Connection { get { return connection; } set { connection = value; } }

            bool deadlock = false;
            /// <summary>
            /// Deadlock occured
            /// </summary>
            public bool Deadlock { get { return deadlock; } set { deadlock = value; } }

            bool syntax = false;
            /// <summary>
            /// Syntax error
            /// </summary>
            public bool Syntax { get { return syntax; } set { syntax = value; } }

            bool miss = false;
            /// <summary>
            /// Object missing like not existing column
            /// </summary>
            public bool Miss { get { return miss; } set { miss = value; } }

            bool operation = false;
            /// <summary>
            /// Operation error
            /// </summary>
            public bool Operation { get { return operation; } set { operation = value; } }

            /// <summary>
            /// Represent as text
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                List<string> list = new List<string>();
                if (connection)
                    list.Add("Connection error");
                if (access)
                    list.Add("Access error");
                if (Timeout)
                    list.Add("Timeout");
                if (miss)
                    list.Add("Missing object");
                if (syntax)
                    list.Add("Syntax error");
                if (operation)
                    list.Add("Operation error");
                string status = string.Join(" + ", list.ToArray());
                return status;
            }
        }

        #endregion

        #region Catch

        /// <summary>
        /// Check exception from database connection.
        ///
        /// True for timeout and damaged connections. They may be repeated.
        /// False otherwise which means that operation may not have a chance to succeed.
        ///
        /// If exception may be temporary and can disappear after repeat,
        /// this function results true. If result is false, operation like
        /// syntax error is final and repeating command makes no sense.
        /// </summary>
        /// <remarks>
        /// Returns true if operation can be repeated.
        /// </remarks>
        /// <param name="exception"></param>
        /// <param name="command"></param>
        /// <returns>True if operation can be repeated.</returns>
        public static Result Catch(System.Exception exception, System.Data.IDbCommand command)
        {
            Energy.Source.Error.Result result = new Energy.Source.Error.Result();

            string message = exception.Message;

            int[] error = GetErrorNumber(exception);

            foreach (int number in error)
            {
                switch (number)
                {
                    case 1042:  // Unable to connect to any of the specified MySQL hosts.
                        result.Connection = true;
                        result.Damage = true;
                        break;
                    case 1045:  // Access denied for user 'horizon'@'localhost' (using password: YES)
                        result.Access = true;
                        break;
                    case 53:
                    case 64:    // Wystąpił błąd poziomu transportu podczas odbierania wyników z serwera. (provider: Dostawca TCP, error: 0 - Określona nazwa sieciowa już jest niedostępna.
                    case 10054:
                        result.Damage = true;
                        break;
                    case 1205:  // Deadlock
                        result.Damage = true;
                        result.Deadlock = true;
                        break;
                    case -2:
                        result.Timeout = true;
                        break;
                    case 207:   // Invalid column name
                    case 208:   // Invalid object name
                    case 2812:  // Cannot find stored procedure
                    case 4121:  // Cannot find either column or the user-defined function or aggregate or the name is ambiguous
                        result.Syntax = true;
                        result.Miss = true;
                        break;
                    case 102:   // Incorrect syntax near '@'
                    case 156:   // Incorrect syntax near the keyword 'FROM'
                    case 245:   // Conversion failed when converting the varchar value to data type int
                    case 1038:  // An object or column name is missing or empty
                    case 2705:  // Column names in each table must be unique
                    case 8152:  // String or binary data would be truncated
                        result.Syntax = true;
                        break;
                    case 5074:  // The index is dependent on column
                    case 8115:  // Arithmetic overflow error converting bigint to data type numeric
                    case 15233: // Property cannot be added. Property already exists
                        result.Operation = true;
                        break;
                    case 297:   // The user does not have permission to perform this action
                        result.Access = true;
                        break;
                    case -1:    // Connection error, possibly reconnecting is required
                        result.Damage = true;
                        break;
                    default:
                        break;
                }
            }

            if (exception is InvalidOperationException)
            {
                result.Damage = true;
            }

            return result;
        }

        #endregion

        #region Number

        /// <summary>
        /// Get array of error numbers from database exception
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public static int[] GetErrorNumber(System.Exception exception)
        {
            List<int> error = new List<int>();
            System.Exception e = exception;
            while (e != null)
            {
                foreach (string field in new string[] {
                    "Number",
                    "ErrorCode",
                })
                {
                    object value = Energy.Base.Class.GetFieldOrPropertyValue(e, field, true, false);
                    int number = Energy.Base.Cast.ObjectToInteger(value);
                    if (number != 0 && !error.Contains(number))
                        error.Add(number);
                }
                e = e.InnerException;
            }
            return error.ToArray();
        }

        #endregion
    }
}
