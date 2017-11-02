using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading;

namespace Energy.Source
{
    #region Connection

    /// <summary>
    /// Database connection interface for application.
    /// To create new connection you have to specify vendor class which implements
    /// DbConnection compatible with ADO.NET.
    /// </summary>
    [Energy.Attribute.Markdown.Text(@"

Connections may be closed automatically when connection timeout is set.

That requires system timer to be set and reset every operation.

Even though ""static"" connections are not recommended, connection timeout might be a *must*
in a situation when multiple instances of software may be run at the same time to avoid
SQL server to run out of free connections.

    ")]
    public partial class Connection : IDisposable, ICloneable
    //, Energy.Interface.IConnection
    //, Energy.Interface.ICopy<Connection>
    {
        #region Constructor

        public Connection() { }

        public Connection(Type vendor)
        {
            this.Vendor = vendor;
        }

        public Connection(Type vendor, string connectionString)
            : this(vendor)
        {
            this.ConnectionString = connectionString;
        }

        public Connection(Type vendor, Configuration configuration)
            : this(vendor)
        {
            this.ConnectionString = configuration.ConnectionString;
        }

        public Connection(DbConnection connection)
        {
            this.Vendor = connection.GetType();
            this.ConnectionString = connection.ConnectionString;
            this.Timeout = connection.ConnectionTimeout;
        }

        public Connection(Configuration configuration)
        {
            this.Configuration = configuration;
        }

        #endregion

        #region Lock

        private readonly Energy.Base.Lock _PropertyLock = new Energy.Base.Lock();

        private readonly Energy.Base.Lock _DriverLock = new Energy.Base.Lock();

        #endregion

        #region Property

        private string _ConnectionString;

        /// <summary>
        /// Connection string used for opening SQL connection.
        /// Connection should be closed on change.
        /// </summary>
        public string ConnectionString
        {
            get
            {
                return _ConnectionString;
            }
            set
            {
                _ConnectionString = value;
            }
        }

        private System.Type _Vendor;

        /// <summary>
        /// DbConnection vendor class for SQL connection.
        /// </summary>
        public System.Type Vendor
        {
            get
            {
                return _Vendor;
            }
            set
            {
                if (value == Vendor)
                    return;
                SetVendor(value);
            }
        }

        private Energy.Source.Configuration _Configuration;

        /// <summary>Configuration</summary>
        public Energy.Source.Configuration Configuration
        {
            get
            {
                return _Configuration;
            }
            private set
            {
                if (_Configuration == value)
                    return;
                _Configuration = value;
            }
        }

        private int _Repeat = 1;
        /// <summary>Repeat operation after recoverable error</summary>
        public int Repeat
        {
            get
            {
                return _Repeat;
            }
            set
            {
                if (_Repeat == value)
                    return;
                _Repeat = value;
            }
        }

        private int _Timeout = 30;
        /// <summary>Time limit in seconds for SQL operations</summary>
        public int Timeout { get { lock (_PropertyLock) return _Timeout; } set { lock (_PropertyLock) _Timeout = value; } }

        private Exception _ErrorException;
        /// <summary>Repeat operation after recoverable error</summary>
        public Exception ErrorException
        {
            get
            {
                lock (_PropertyLock)
                    return _ErrorException;
            }
            private set
            {
                lock (_PropertyLock)
                    _ErrorException = value;
            }
        }

        #endregion

        public void SetVendor(Type vendor)
        {
            if (vendor == null)
                throw new ArgumentNullException();
            if (!vendor.IsSubclassOf(typeof(DbConnection)))
                throw new Exception("Vendor must derrive from DbConnection class");
            lock (_PropertyLock)
            {
                _Vendor = vendor;
            }
        }

        /// <summary>
        /// Log
        /// </summary>
        public Energy.Core.Log Log { get; set; }

        private int _ErrorNumber = 0;
        /// <summary>ErrorNumber</summary>
        public int ErrorNumber { get { lock (_PropertyLock) return _ErrorNumber; } private set { lock (_PropertyLock) _ErrorNumber = value; } }

        private string _ErrorStatus = "";
        /// <summary>ErrorStatus</summary>
        public string ErrorStatus { get { lock (_PropertyLock) return _ErrorStatus; } private set { lock (_PropertyLock) _ErrorStatus = value; } }

        /// <summary>
        /// Create connection object of vendor class.
        /// Used by Open() to create DbConnection object.
        /// </summary>
        /// <returns></returns>
        public DbConnection Create()
        {
            Type vendor = Vendor;
            DbConnection _ = null;
            try
            {
                _ = (DbConnection)Activator.CreateInstance(vendor);
                _.ConnectionString = ConnectionString;
            }
            catch (Exception x)
            {
                if (!LogWrite(x))
                    throw;
            }
            finally
            {
            }
            return _;
        }

        public DbConnection Open(DbConnection connection)
        {
            if (connection == null)
                return null;

            int timeout = Timeout;
            if (timeout < 1 && connection.ConnectionTimeout > 0)
                timeout = connection.ConnectionTimeout;
            if (timeout < 1)
                timeout = 5;

            bool success = false;

            // Open connection in separate thread
            Thread thread = new Thread(delegate ()
            {
                try
                {
                    connection.Open();
                    success = true;
                }
                catch (Exception x)
                {
                    SetError(x);
                }
            })
            {
                // Make sure it's marked as a background thread
                // so it'll get cleaned up automatically
                IsBackground = true,
                CurrentCulture = System.Globalization.CultureInfo.InvariantCulture,
            };
            thread.Start();
            thread.Join(timeout * 1000);

            // If we didn't connect successfully, throw an exception
            if (!success)
            {
                connection = null;
            }

            return connection;
        }

        public IDbConnection Open()
        {
            return Open(Create());
        }

        public static explicit operator Energy.Source.Connection(DbConnection connection)
        {
            Energy.Source.Connection _ = new Energy.Source.Connection(connection);
            return _;
        }

        /// <summary>
        /// Check if database connection is active.
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        public static bool IsActive(DbConnection connection)
        {
            if (connection == null)
                return false;
            switch (connection.State)
            {
                case ConnectionState.Open:
                case ConnectionState.Executing:
                case ConnectionState.Fetching:
                    return true;
                default:
                case ConnectionState.Broken:
                case ConnectionState.Closed:
                    return false;
            }
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
        private bool Catch(Exception exception, DbCommand command)
        {
            CatchResult result = new Energy.Source.Connection.CatchResult();

            string message = exception.Message;

            if (exception is DbException)
            {
                //foreach (SqlError error in ((SqlException)exception).Errors)
                //{
                //}
            }

            int[] error = GetErrorNumber(exception);

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

            // Log
            Core.Log log = Log ?? Core.Log.Default;

            if (Log != null)
            {
                Log.Add(result.ToString(), Enumeration.LogLevel.Trace);
            }

            // Reaction

            if (result.Timeout)
            {
                try
                {
                    command.Cancel();
                }
                catch { }
            }

            if (result.Damage)
            {
                //Kill(command);
            }

            if (Log != null)
                Log.Add(result.ToString(), Enumeration.LogLevel.Error);

            // For timeout and damaged connection result is true.

            return result.Timeout || result.Damage;
        }

        public int[] GetErrorNumber(Exception exception)
        {
            List<int> error = new List<int>();
            Exception e = exception;
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

        //public virtual DataTable Fetch(string query)
        //{
        //    return FetchDataTable(query);
        //}

        //public virtual DataTable FetchDataTable(string query)
        //{
        //    return FetchDataTableRead(query);
        //}

        public virtual DataTable Load(string query)
        {
            ClearError();

            using (IDbConnection connection = Open())
            {
                if (connection == null)
                    return null;
                return Load(connection, query);
            }
        }

        public DataTable Load(IDbConnection connection, string query)
        {
            using (IDbCommand command = Prepare(connection, query))
            {
                return Load(command);
            }
        }

        private DataTable Load(IDbCommand command)
        {
            using (IDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
            {
                DataTable table = new DataTable();
                table.Load(reader);
                reader.Close();
                return table;
            }
        }

        private bool Catch(Exception x)
        {
            return Catch(x, null);
        }

        public virtual DataTable Fetch(string query)
        {
            //ClearError();

            DataTable table = null;

            int repeat = _Repeat;
            while (repeat-- >= 0)
            {
                using (IDbConnection connection = Open())
                {
                    if (connection == null)
                        return null;
                    try
                    {
                        IDbCommand command = Prepare(connection, query);
                        CommandBehavior behaviour = CommandBehavior.CloseConnection;
                        IDataReader reader = command.ExecuteReader(behaviour);

                        DataTable schema = reader.GetSchemaTable();

                        if (schema == null)
                            break;

                        table = new DataTable();

                        List<DataColumn> list = new List<DataColumn>();
                        foreach (DataRow row in schema.Rows)
                        {
                            string columnName = System.Convert.ToString(row["ColumnName"]);
                            DataColumn column = new DataColumn(columnName, (Type)(row["DataType"]));
                            column.Unique = (bool)row["IsUnique"];
                            column.AllowDBNull = (bool)row["AllowDBNull"];
                            column.AutoIncrement = (bool)row["IsAutoIncrement"];
                            list.Add(column);
                            table.Columns.Add(column);
                        }

                        // Read rows from DataReader and populate the DataTable

                        while (reader.Read())
                        {
                            DataRow row = table.NewRow();
                            for (int i = 0; i < list.Count; i++)
                            {
                                row[((DataColumn)list[i])] = reader[i];
                            }
                            table.Rows.Add(row);
                        }

                        command.Cancel();
                        reader.Close();

                        break;
                    }
                    catch (Exception x)
                    {
                        SetError(x);
                        if (Catch(x))
                            continue;
                        else
                        {
                            if (!LogWrite(x))
                                throw;
                            return null;
                        }
                    }
                }
            }
            return table;
        }

        private bool LogWrite(Exception x)
        {
            (Log ?? Energy.Core.Log.Default).Write(x);
            return true;
        }

        private void SetError(Exception x)
        {
            ErrorException = x;
            ErrorStatus = x.Message;

            int[] error = GetErrorNumber(x);
            if (error == null || error.Length == 0)
                ErrorNumber = 0;
            else
            {
                for (int i = 0; i < error.Length; i++)
                {
                    if (error[i] != 0)
                    {
                        ErrorNumber = error[i];
                        break;
                    }
                }
            }
        }

        private void ClearError()
        {
            lock (_PropertyLock)
            {
                _ErrorNumber = 0;
                _ErrorStatus = "";
                _ErrorException = null;
            }
        }

        public virtual object Single(string query)
        {
            return new object();
        }

        public IDbCommand Prepare(IDbConnection connection, string query)
        {
            IDbCommand command = connection.CreateCommand();
            command.CommandTimeout = Timeout;
            command.CommandText = query;
            return command;
        }

        public virtual int Execute(string query)
        {
            try
            {
                using (IDbConnection connection = this.Open())
                {
                    if (connection == null)
                        return GetNegativeErrorNumber();
                    return Execute(Prepare(connection, query));
                }
            }
            catch (Exception x)
            {
                LogWrite(x);
            }
            return -1;
        }

        public virtual int Execute(IDbCommand command)
        {
            return command.ExecuteNonQuery();
        }

        public virtual bool Bool(string query)
        {
            return Energy.Base.Cast.StringToBool((string)Scalar(query));
        }

        private int GetNegativeErrorNumber()
        {
            return ErrorNumber != 0 ? -Math.Abs(ErrorNumber) : -1;
        }

        public virtual string Parse(string query)
        {
            return query;
        }

        public virtual string Parse(string query, Energy.Query.Parameter.List parameters)
        {
            return parameters.Parse(query);
        }

        public void Dispose()
        {
        }

        public virtual object Scalar(IDbCommand command)
        {
            object value = command.ExecuteScalar();
            return value;
        }

        public virtual object Scalar(string query)
        {
            int repeat = _Repeat;
            while (repeat-- >= 0)
            {
                using (IDbConnection connection = Open())
                {
                    if (connection == null)
                        return null;
                    try
                    {
                        return Scalar(connection, query);
                    }
                    catch (Exception x)
                    {
                        if (!Catch(x))
                            return null;
                    }
                }
            }
            return null;
        }

        public virtual T Scalar<T>(string query)
        {
            object value = Scalar(query);
            return Energy.Base.Cast.As<T>(value);
        }

        private object Scalar(IDbConnection connection, string query)
        {
            using (IDbCommand command = Prepare(connection, query))
            {
                return Scalar(command);
            }
        }

        public string GetErrorText()
        {
            List<string> list = new List<string>();
            if (ErrorNumber != 0)
                list.Add(ErrorNumber.ToString());
            if (!string.IsNullOrEmpty(ErrorStatus))
                list.Add(ErrorStatus);
            return string.Join(" ", list.ToArray());
        }

        public Energy.Source.Connection Copy()
        {
            Energy.Source.Connection copy = new Energy.Source.Connection();
            lock (_PropertyLock)
            {
                if (!string.IsNullOrEmpty(_ConnectionString))
                    copy.ConnectionString = _ConnectionString;
                if (_Vendor != null)
                    copy.Vendor = _Vendor;
                if (_Configuration != null)
                    copy.Configuration = _Configuration;
            }
            return copy;
            //    Energy.Source.Connection clone = new Energy.Source.Connection();
            //    clone.ConnectionString = this.ConnectionString;
            //    clone.Vendor = this.Vendor;
            //    clone.Dialect = this.Dialect;
            //    return clone;
        }

        public object Clone()
        {
            return Copy();
        }
    }

    #endregion

    #region Generic

    /// <summary>
    /// Generic method with type of SQL connection driver class
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Connection<T> : Connection
    {
        public Connection()
        {
            this.Vendor = typeof(T);
        }

        public Connection(string connectionString)
            : base(typeof(T), connectionString)
        {
        }
    }

    #endregion

    #region Static

    public partial class Connection
    {
        /// <summary>
        /// Persistent connection to database.
        /// </summary>
        public partial class Static : IDisposable
        {
            #region Lock

            private readonly Energy.Base.Lock _PropertyLock = new Energy.Base.Lock();

            private readonly Energy.Base.Lock _DriverLock = new Energy.Base.Lock();

            #endregion

            #region Property

            private DbConnection _Driver;

            #region IDisposable Support

            private bool disposedValue = false; // To detect redundant calls

            protected virtual void Dispose(bool disposing)
            {
                if (!disposedValue)
                {
                    if (disposing)
                    {
                        // TODO: dispose managed state (managed objects).
                    }

                    // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                    // TODO: set large fields to null.

                    disposedValue = true;
                }
            }

            // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
            // ~Static() {
            //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            //   Dispose(false);
            // }

            // This code added to correctly implement the disposable pattern.
            public void Dispose()
            {
                // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
                Dispose(true);
                // TODO: uncomment the following line if the finalizer is overridden above.
                // GC.SuppressFinalize(this);
            }

            #endregion

            #endregion

            #region Property

            #endregion
        }
    }

    #endregion

    #region Base

    public partial class Connection
    {
        public /*abstract*/ class DatabaseConnectionBase
        {
            private string _ConnectionString;

            /// <summary>
            /// Connection string used for opening SQL connection.
            /// Connection should be closed on change.
            /// </summary>
            public string ConnectionString
            {
                get
                {
                    return _ConnectionString;
                }
                set
                {
                    if (value == _ConnectionString)
                        return;
                    _ConnectionString = value;
                }
            }

            private System.Type _Vendor;

            /// <summary>
            /// DbConnection vendor class for SQL connection.
            /// </summary>
            public System.Type Vendor
            {
                get
                {
                    return _Vendor;
                }
                set
                {
                    if (value == _Vendor)
                        return;
                    _Vendor = value;
                }
            }

            private int _Timeout = 30;
            /// <summary>Time limit in seconds for SQL operations</summary>
            public int Timeout
            {
                get
                {
                    return _Timeout;
                }
                set
                {
                    _Timeout = value;
                }
            }

            /// <summary>
            /// Create connection object of vendor class.
            /// Used internally by Open() to create DbConnection object.
            /// </summary>
            /// <returns></returns>
            public IDbConnection Create()
            {
                Type vendor = Vendor;
                if (vendor == null)
                    return null;
                DbConnection _ = null;
                try
                {
                    _ = (DbConnection)Activator.CreateInstance(vendor);
                    _.ConnectionString = ConnectionString;
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                }
                return (IDbConnection)_;
            }

            public IDbCommand Prepare(IDbConnection connection, string query)
            {
                IDbCommand command = connection.CreateCommand();
                command.CommandTimeout = Timeout;
                command.CommandText = query;
                return command;
            }

            public object Scalar(IDbCommand command)
            {
                object value = null;
                try
                {
                    value = command.ExecuteScalar();
                }
                catch (Exception x)
                {
                    //Energy.Core.Bug.DebugWrite(x);
                    System.Diagnostics.Debug.WriteLine(Energy.Core.Bug.ExceptionMessage(x, true));
                    throw;
                }
                return value;
            }

            public object Scalar(IDbConnection connection, string query)
            {
                return Scalar(Prepare(connection, query));
            }

            public object Scalar(string query)
            {
                using (IDbConnection connection = Create())
                {
                    if (connection == null)
                        return null;
                    try
                    {
                        connection.Open();
                        try
                        {
                            return Scalar(connection, query);
                        }
                        finally
                        {
                            connection.Close();
                        }
                    }
                    catch
                    {
                        throw;
                    }
                }
                //return null;

            }
        }
    }

    #endregion
}

