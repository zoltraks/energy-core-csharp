using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Xml.Serialization;
using System.Threading;
using Energy.Source.Interface;

namespace Energy.Source
{
    public class Connection : IDisposable, Energy.Source.Interface.IConnection
    {
        #region Constructor

        public Connection()
        {
        }

        public Connection(Type vendor, string connectionString, Energy.Enumeration.SqlDialect dialect)
        {
            if (vendor != null && !vendor.IsSubclassOf(typeof(DbConnection)))
            {
                throw new Exception("Vendor must derrive from DbConnection class");
            }
            this.Dialect = dialect;
            this.Vendor = vendor;
            this.ConnectionString = connectionString;
        }

        public Connection(Type vendor, Configuration configuration)
        {
            this.Vendor = vendor;
            this.Dialect = configuration.Dialect;
            this.ConnectionString = configuration.ConnectionString;
        }

        #endregion

        private int _Repeat = 1;
        /// <summary>Repeat operation after recoverable error</summary>
        public int Repeat { get { return _Repeat; } set { _Repeat = value; } }

        private int _Timeout;
        /// <summary>Time limit in seconds for SQL operations</summary>
        public int Timeout { get { return _Timeout; } set { _Timeout = value; } }

        /// <summary>
        /// SQL dialect
        /// </summary>
        public Energy.Enumeration.SqlDialect Dialect { get; set; }

        private System.Type _Vendor;

        /// <summary>
        /// DbConnection vendor class for SQL connection
        /// </summary>
        public System.Type Vendor
        {
            get
            {
                return _Vendor;
            }
            set
            {
                if (value != _Vendor)
                {
                    Close();
                    _Vendor = value;
                    _Driver = null;
                }
            }
        }

        /// <summary>
        /// Connection string used for opening SQL connection.
        /// Connection should be closed on change.
        /// </summary>
        public Energy.Base.ConnectionString ConnectionString { get; set; }

        /// <summary>
        /// Log
        /// </summary>
        public Energy.Core.Log Log { get; set; }

        /// <summary>
        /// Configuration
        /// </summary>
        public Energy.Core.Configuration Configuration { get; set; }

        private DbConnection _Driver;

        /// <summary>
        /// SQL connection driver class.
        /// </summary>
        public DbConnection Driver
        {
            get
            {
                if (_Driver == null)
                {
                    lock (this)
                    {
                        if (_Driver == null)
                        {
                            try
                            {
                                _Driver = (DbConnection)Activator.CreateInstance(_Vendor);
                            }
                            catch
                            {
                                throw;
                            }
                            _Driver.ConnectionString = ConnectionString.ToString();
                        }
                    }
                }
                return _Driver;
            }
            set
            {
                _Driver = value;
                if (value != null)
                {
                    _Vendor = value.GetType();
                }
            }
        }

        public bool Active
        {
            get
            {
                if (_Driver == null)
                    return false;
                if (_Driver.State == ConnectionState.Broken || _Driver.State == ConnectionState.Closed)
                    return false;
                return true;
            }
        }

        public bool Busy
        {
            get
            {
                if (_Driver == null)
                    return false;
                if (_Driver.State == ConnectionState.Connecting || _Driver.State == ConnectionState.Executing || _Driver.State == ConnectionState.Fetching)
                    return true;
                return false;
            }
        }

        public bool Open()
        {
            if (Active)
            {
                Close();
            }
            try
            {
                if (Log != null)
                    Log.Write("Opening connection", Energy.Enumeration.LogLevel.Trace);

                Driver.Open();

                bool first = false;

                while (Busy)
                {
                    if (first)
                    {
                        first = false;
                        Thread.Sleep(0);
                    }
                    else
                    {
                        Thread.Sleep(1);
                    }
                }

                return Active;
            }
            catch (DbException dbException)
            {
                if (Log == null)
                    throw;
                //Log.Write(dbException);
                int code = 0;
                Catch(dbException, null);
                object _ = Energy.Base.Class.GetFieldOrPropertyValue(dbException, "Number", true, false);
                if (_ != null && _ is int)
                    code = (int)_;
                Log.Write(dbException.Message, dbException.Source, code);
                return false;
            }
            catch (Exception x)
            {
                if (Log == null)
                    throw;

                Log.Write(x);
                return false;
            }
        }

        /// <summary>
        /// Close connection
        /// </summary>
        private void Close()
        {
            if (_Driver == null) return;
            try
            {
                _Driver.Close();
            }
            catch (DbException x)
            {
                if (Log == null)
                {
                    throw;
                }
                Log.Add(x);
            }
            _Driver = null;
        }

        #region CatchResult

        public class CatchResult
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
            /// Connection damaged
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

        /// <summary>
        /// Check exception from database connection.
        /// 
        /// True for timeout and damaged connections.
        /// False otherwise which probably may not have a chance to succeed.
        /// 
        /// If exception may be temporary and can disappear after repeat,
        /// this function results true. If result is false, operation like
        /// syntax error is final and exception cannot be "catched" by
        /// repeating command.
        /// </summary>
        /// <remarks>
        /// Returns true if operation can be repeated
        /// </remarks>
        /// <param name="exception"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        private bool Catch(Exception exception, DbCommand command)
        {
            CatchResult result = new Energy.Source.Connection.CatchResult();

            List<int> error = new List<int>();

            string message = exception.Message;

            if (exception is DbException)
            {
                //foreach (SqlError error in ((SqlException)exception).Errors)
                //{
                //}
            }

            Exception e = exception;
            while (e != null)
            {
                int _Number = Energy.Base.Cast.ObjectToInteger(Energy.Base.Class.GetFieldOrPropertyValue(e, "Number", true, false));
                if (_Number > 0 && !error.Contains(_Number))
                    error.Add(_Number);
                e = e.InnerException;
            }

            foreach (int number in error)
            {
                switch (number)
                {
                    case 1042: // Unable to connect to any of the specified MySQL hosts.
                        result.Connection = true;
                        result.Damage = true;
                        break;
                    case 1045: // Access denied for user 'horizon'@'localhost' (using password: YES)
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
                    case -1: // Connection error, possibly reconnecting is required
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

            // Log

            if (Log != null)
                Log.Add(result.ToString(), Enumeration.LogLevel.Trace);

            // Reaction

            if (result.Timeout)
            {
                try
                {
                    command.Cancel();
                }
                catch { }

                if (Configuration != null)
                {
                }
            }

            if (result.Damage)
            {
                //Kill(command);
            }

            if (Log != null)
                Log.Add(result.ToString(), Enumeration.LogLevel.Error);

            // For timeout and damaged connection error result is true. 
            // False otherwise which probably may not have a chance to succeed.

            return result.Timeout || result.Damage;
        }

        public virtual DataSet Read(string query)
        {
            bool active = Active;

            if (!active && !Open()) return null;

            if (!active) Close();

            return null;
        }

        public virtual DataTable Fetch(string query)
        {
            for (int i = 0; i <= _Repeat; i++)
            {
                if (!Active && !Open())
                {
                    return null;
                }
                DbCommand command = Driver.CreateCommand();
                command.CommandText = query;
                DbDataReader reader = null;
                try
                {
                    Prepare(command);
                    reader = command.ExecuteReader();
                    return new DataTable();
                }
                catch { }
            }
            return null;
        }

        private void Prepare(DbCommand command)
        {
            command.Connection = _Driver;
            if (Timeout > 0)
            {
                command.CommandTimeout = Timeout;
            }
        }

        public virtual object Scalar(string query)
        {
            if (!Active && !Open())
            {
                return null;
            }
            DbCommand command = Driver.CreateCommand();
            command.CommandText = Parse(query);
            try
            {
                object value = command.ExecuteScalar();
                return value == DBNull.Value ? null : value;
            }
            catch (DbException x)
            {
                if (Log == null)
                {
                    throw;
                }
                Log.Write(x);
                return null;
            }
        }

        public virtual string Execute(string query)
        {
            if (!Active && !Open())
            {
                return null;
            }
            DbCommand command = Driver.CreateCommand();
            command.CommandText = Parse(query);
            return null;
        }

        public virtual string Parse(string query)
        {
            return query;
        }

        public virtual void Dispose()
        {
            Close();
        }

        public virtual object Scalar(DbCommand command)
        {
            try
            {
                return command.ExecuteScalar();
            }
            catch (Exception x)
            {
                if (Log == null) throw x;
                Log.Add(x);
                return null;
            }
        }

        public void Kill()
        {
            throw new NotImplementedException();
        }

        int IConnection.Execute(string query)
        {
            throw new NotImplementedException();
        }

        void IConnection.Close()
        {
            throw new NotImplementedException();
        }

        object IConnection.Fetch(string query)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Generic method with type of SQL connection driver class
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Connection<T> : Connection
    {
        public Connection()
        {
            Vendor = typeof(T);
        }

        public Connection(Energy.Enumeration.SqlDialect dialect)            
        {
            Vendor = typeof(T);
            Dialect = dialect;
        }
    }
}
