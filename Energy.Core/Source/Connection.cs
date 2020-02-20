using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text.RegularExpressions;
using System.Threading;

namespace Energy.Source
{
    #region Template

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

    #region Connection

    /// <summary>
    /// Database connection interface for application.
    /// To create new connection you have to specify vendor class which implements
    /// IDbConnection compatible with ADO.NET.
    /// </summary>
    public class Connection : IDisposable, ICloneable
        , Energy.Interface.ICopy<Connection>
    {
        #region Constructor

        public Connection() { }

        public Connection(Type vendor)
        {
            _Vendor = vendor;
        }

        public Connection(Type vendor, string connectionString)
            : this(vendor)
        {
            _ConnectionString = connectionString;
        }

        public Connection(Type vendor, Configuration configuration)
            : this(vendor)
        {
            _ConnectionString = configuration.ConnectionString;
            _Timeout = configuration.Timeout;
        }

        public Connection(IDbConnection connection)
        {
            _Vendor = connection.GetType();
            _ConnectionString = connection.ConnectionString;
        }

        public static explicit operator Energy.Source.Connection(DbConnection connection)
        {
            Energy.Source.Connection _ = new Energy.Source.Connection(connection);
            return _;
        }

        #endregion

        #region Lock

        private readonly Energy.Base.Lock _Lock = new Energy.Base.Lock();

        #endregion

        #region Empty

        /// <summary>
        /// Represents empty object
        /// </summary>
        public static Connection Empty { get { return GetEmpty(); } }

        private static Connection GetEmpty()
        {
            return new Connection();
        }

        #endregion

        #region Property

        private string _ConnectionString;

        /// <summary>
        /// Connection string used for opening database connection
        /// </summary>
        public string ConnectionString { get { return GetConnectionString(); } set { SetConnectionString(value); } }

        private System.Type _Vendor;

        /// <summary>
        /// Connection vendor class for database connection
        /// </summary>
        public System.Type Vendor { get { return GetVendor(); } set { SetVendor(value); } }

        private bool _Persistent;

        /// <summary>
        /// Persistent connection
        /// </summary>
        public bool Persistent { get { return GetPersistent(); } set { SetPersistent(value); } }

        private int _Repeat = 0;

        /// <summary>
        /// Repeat operation after recoverable error
        /// </summary>
        public int Repeat { get { lock (_Lock) return _Repeat; } set { lock (_Lock) _Repeat = value; } }

        private int _Timeout = 30;

        /// <summary>
        /// Time limit in seconds for SQL operations
        /// </summary>
        public int Timeout { get { lock (_Lock) return _Timeout; } set { lock (_Lock) _Timeout = value; } }

        private Energy.Interface.ILogger _Logger;

        /// <summary>
        /// Log
        /// </summary>
        public Energy.Interface.ILogger Logger { get { lock (_Lock) return _Logger; } set { lock (_Lock) _Logger = value; } }

        private Energy.Interface.IDialect _Dialect;

        /// <summary>
        /// Dialects
        /// </summary>
        public Energy.Interface.IDialect Dialect { get { lock (_Lock) return _Dialect; } set { lock (_Lock) _Dialect = value; } }

        #region Option

        private bool _ThreadOpen;

        /// <summary>
        /// Persistent connection
        /// </summary>
        public bool ThreadOpen { get { return GetThreadOpen(); } set { SetThreadOpen(value); } }

        private bool _ThreadEvent;

        /// <summary>
        /// Persistent connection
        /// </summary>
        public bool ThreadEvent { get { return GetThreadEvent(); } set { SetThreadEvent(value); } }

        #endregion

        private int _ErrorNumber = 0;

        /// <summary>
        /// Error error number
        /// </summary>
        public int ErrorNumber { get { lock (_Lock) return _ErrorNumber; } }

        private string _ErrorMessage = "";

        /// <summary>
        /// Last error status
        /// </summary>
        public string ErrorMessage { get { lock (_Lock) return _ErrorMessage; } }

        private Exception _ErrorException;

        /// <summary>
        /// Last error exception
        /// </summary>
        public Exception ErrorException { get { lock (_Lock) return _ErrorException; } }

        private IDbConnection _Connection;

        /// <summary>
        /// True if connection is active
        /// </summary>
        public bool Active { get { return GetActive(); } }

        private string GetConnectionString()
        {
            lock (_Lock)
            {
                return _ConnectionString;
            }
        }

        private void SetConnectionString(string connectionString)
        {
            lock (_Lock)
            {
                if (0 == string.Compare(connectionString, _ConnectionString, false))
                {
                    return;
                }
                if (0 != string.Compare(connectionString, _ConnectionString, true))
                {
                    Close();
                }
                _ConnectionString = connectionString;
            }
        }

        private Type GetVendor()
        {
            lock (_Lock)
            {
                return _Vendor;
            }
        }

        private void SetVendor(Type vendor)
        {
            if (vendor == null)
            {
                throw new ArgumentNullException();
            }
            if (!typeof(IDbConnection).IsAssignableFrom(vendor))
            {
                throw new Exception("Vendor must implement IDbConnection interface");
            }
            lock (_Lock)
            {
                if (vendor == _Vendor)
                {
                    return;
                }
                Close();
                _Vendor = vendor;
            }
        }

        private bool GetPersistent()
        {
            lock (_Lock)
            {
                return _Persistent;
            }
        }

        private void SetPersistent(bool persistent)
        {
            lock (_Lock)
            {
                if (persistent == _Persistent)
                {
                    return;
                }
                if (!persistent)
                {
                    Close();
                }
                _Persistent = persistent;
            }
        }

        private bool GetThreadOpen()
        {
            lock (_Lock)
            {
                return _ThreadOpen;
            }
        }

        private void SetThreadOpen(bool value)
        {
            _ThreadOpen = value;
        }

        private bool GetActive()
        {
            lock (_Lock)
            {
                if (_Connection == null)
                    return false;
                return IsActive(_Connection);
            }
        }

        private void SetThreadEvent(bool value)
        {
            lock (_Lock)
            {
                _ThreadEvent = value;
            }
        }

        private bool GetThreadEvent()
        {
            lock (_Lock)
            {
                return _ThreadEvent;
            }
        }

        #endregion

        #region Event

        /// <summary>
        /// Event fired when vendor connection object is created by Activator.
        /// </summary>
        public event EventHandler OnCreate;

        /// <summary>
        /// Event fired when vendor connection is open.
        /// </summary>
        public event EventHandler OnOpen;

        /// <summary>
        /// Event fired when vendor connection was closed.
        /// </summary>
        public event EventHandler OnClose;

        #endregion

        #region Fire

        private void FireOnCreate()
        {
            if (null != OnCreate)
            {
                if (ThreadEvent)
                {
                    Thread thread = new Thread(() => OnCreate(this, null))
                    {
                        IsBackground = true,
                        Name = "Energy.Source.Connection.OnCreate",
                    };
                    thread.Start();
                }
                else
                {
                    OnCreate(this, null);
                }
            }
        }

        private void FireOnOpen()
        {
            if (OnOpen != null)
            {
                if (ThreadEvent)
                {
                    Thread thread = new Thread(() => OnOpen(this, null))
                    {
                        IsBackground = true,
                        Name = "Energy.Source.Connection.OnOpen",
                    };
                    thread.Start();
                }
                else
                {
                    OnOpen(this, null);
                }
            }
        }

        private void FireOnClose()
        {
            if (OnClose != null)
            {
                if (ThreadEvent)
                {
                    Thread thread = new Thread(() => OnClose(this, null))
                    {
                        IsBackground = true,
                        Name = "Energy.Source.Connection.OnClose",
                    };
                    thread.Start();
                }
                else
                {
                    OnClose(this, null);
                }
            }
        }

        #endregion

        #region Error

        private string SetError(Exception x)
        {
            int number = 0;
            int[] error = Energy.Source.Error.GetErrorNumber(x);
            if (error.Length > 0)
            {
                number = error[0];
            }
            lock (_Lock)
            {
                _ErrorNumber = number;
                _ErrorMessage = x.Message;
                _ErrorException = x;
                return GetErrorText();
            }
        }

        private string SetError(string error)
        {
            lock (_Lock)
            {
                _ErrorNumber = 0;
                _ErrorMessage = error;
                _ErrorException = null;
            }
            return error;
        }

        private string ClearError()
        {
            lock (_Lock)
            {
                _ErrorNumber = 0;
                _ErrorMessage = "";
                _ErrorException = null;
            }
            return "";
        }

        private int GetNegativeErrorNumber()
        {
            int number = ErrorNumber;
            if (number == 0)
            {
                return -1;
            }
            return number > 0 ? -number : number;
        }

        /// <summary>
        /// Get error text.
        /// </summary>
        /// <returns></returns>
        public string GetErrorText()
        {
            List<string> list = new List<string>();
            lock (_Lock)
            {
                if (_ErrorNumber != 0)
                {
                    list.Add(_ErrorNumber.ToString());
                }
                if (!string.IsNullOrEmpty(_ErrorMessage))
                {
                    list.Add(Regex.Replace(_ErrorMessage, @"(\r\n|\n|\r)+", " "));
                }
            }
            return string.Join(" ", list.ToArray());
        }

        /// <summary>
        /// Get error text or alternative if empty.
        /// </summary>
        /// <returns></returns>
        public string GetErrorText(string alternative)
        {
            string text = GetErrorText();
            if (string.IsNullOrEmpty(text))
            {
                text = alternative;
            }
            return text;
        }

        #endregion

        #region Create

        /// <summary>
        /// Create connection object of vendor class
        /// </summary>
        /// <remarks>
        /// Used by Open() to create DbConnection object.
        /// </remarks>
        /// <returns></returns>
        public IDbConnection Create()
        {
            Type vendor;
            string connectionString;
            lock (_Lock)
            {
                vendor = _Vendor;
                connectionString = _ConnectionString;
            }
            IDbConnection _ = null;
            try
            {
                _ = (IDbConnection)Activator.CreateInstance(vendor);
                _.ConnectionString = connectionString;
            }
            catch (Exception x)
            {
                SetError(x);
                (Logger ?? Energy.Core.Log.Default).Write(x);
            }
            finally
            {
                FireOnCreate();
            }
            return _;
        }

        #endregion

        #region Open

        public IDbConnection Open(IDbConnection connection)
        {
            if (connection == null)
                return null;

            int timeout = Timeout;
            if (timeout < 1 && connection.ConnectionTimeout > 0)
                timeout = connection.ConnectionTimeout;
            if (timeout < 1)
                timeout = 5;

            bool success = false;

            bool openOnBackgroundThread = this.ThreadOpen;

            // Open connection in separate thread
            if (openOnBackgroundThread)
            {
                Thread thread = new Thread(delegate ()
                {
                    try
                    {
                        connection.Open();
                        success = true;
                    }
                    catch (ThreadAbortException)
                    {
                        Energy.Core.Bug.Write("C081", "ThreadAbortException");
                    }
                    catch (Exception x)
                    {
                        SetError(x);
                    }
                })
                {
                    // Make sure it's marked as a background thread
                    // so it'll get cleaned up automatically
                    IsBackground = true, // don't block app
                    CurrentCulture = System.Globalization.CultureInfo.InvariantCulture,
                };
                thread.Start();
                if (!thread.Join(timeout * 1000))
                {
                    thread.Abort();
                }
            }
            else
            // Open connection in main thread
            {
                try
                {
#if NET20
                    connection.Open();
#else
                    //TODO Above NET20 we can use OpenAsync on background thread.
                    //connection.OpenAsync().ConfigureAwait(false);
                    connection.Open();
#endif
                    success = true;
                }
                catch (Exception x)
                {
                    SetError(x);
                }
            }

            if (!success)
                connection = null;

            FireOnOpen();

            return connection;
        }

        public IDbConnection Open()
        {
            lock (_Lock)
            {
                if (_Persistent)
                {
                    if (Active)
                        Close();
                    else
                        Clear();
                    _Connection = Open(Create());
                    return _Connection;
                }
            }

            return Open(Create());
        }

        private void Clear()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Close

        /// <summary>
        /// Close existing connection
        /// </summary>
        public void Close()
        {
            lock (_Lock)
            {
                if (_Connection == null)
                {
                    return;
                }
                try
                {
                    _Connection.Close();
                }
                catch (Exception x)
                {
                    SetError(x);
                    (Logger ?? Energy.Core.Log.Default).Write(x);
                    Energy.Core.Bug.Write("Exception during connection Close...");
                }
                try
                {
                    _Connection.Dispose();
                }
                catch
                {
                    Energy.Core.Bug.Write("Exception during connection Dispose...");
                }
                finally
                {
                    _Connection = null;
                    FireOnClose();
                }
            }
        }

        #endregion

        #region Test

        /// <summary>
        /// Test database connection
        /// </summary>
        /// <returns></returns>
        public bool Test()
        {
            return Scalar<bool>("SELECT 1");
        }

        #endregion

        #region Active

        /// <summary>
        /// Check if database connection is active
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        public static bool IsActive(IDbConnection connection)
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
                    Energy.Core.Bug.Write("C083", "Connection state is Broken");
                    return false;
                case ConnectionState.Closed:
                    return false;
            }
        }

        /// <summary>
        /// Check if database connection is actually working
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        public static bool IsRunning(IDbConnection connection)
        {
            if (connection == null)
                return false;

            switch (connection.State)
            {
                case ConnectionState.Executing:
                case ConnectionState.Fetching:
                    return true;
                default:
                    return false;
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
        private bool Catch(Exception exception, IDbCommand command)
        {
            Energy.Source.Error.Result result = Energy.Source.Error.Catch(exception, command);

            string message = exception.Message;

            (Logger ?? Core.Log.Default).Add(result.ToString(), (int)Energy.Enumeration.LogLevel.Trace);

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

            // For timeout and damaged connection result is true.

            return result.Timeout || result.Damage;
        }

        private bool Catch(Exception x)
        {
            return Catch(x, null);
        }

        #endregion

        #region Fetch

        private Energy.Base.Table Fetch(IDbCommand command)
        {
            IDataReader reader = command.ExecuteReader();

            DataTable schema = reader.GetSchemaTable();

            if (schema == null)
            {
                return null;
            }

            Energy.Base.Table table = new Energy.Base.Table();

            List<string> columnList = new List<string>();
            foreach (DataRow row in schema.Rows)
            {
                string columnName = System.Convert.ToString(row["ColumnName"]);
                columnList.Add(columnName);
            }

            // Read rows from DataReader and populate the DataTable

            while (reader.Read())
            {
                Energy.Base.Record record = table.New();
                for (int i = 0; i < columnList.Count; i++)
                {
                    record[columnList[i]] = reader[i];
                }
            }

            command.Cancel();

            reader.Close();

            return table;
        }

        private Energy.Base.Table Fetch(IDbConnection connection, string query)
        {
            using (IDbCommand command = Prepare(connection, query))
            {
                return Fetch(command);
            }
        }

        /// <summary>
        /// Fetch query results into Energy.Base.Table.
        /// Return null on error.
        /// </summary>
        /// <param name="query">Query text</param>
        /// <param name="error">Error text</param>
        /// <returns></returns>
        public Energy.Base.Table Fetch(string query, out string error)
        {
            if (string.IsNullOrEmpty(query))
            {
                error = SetError("Empty query");
                return null;
            }
            error = ClearError();
            int repeat = Repeat;
            while (repeat-- >= 0)
            {
                if (Persistent)
                {
                    if (!Active && null == Open())
                    {
                        error = GetErrorText("Connection error");
                        return null;
                    }
                    try
                    {
                        lock (_Lock)
                        {
                            return Fetch(_Connection, query);
                        }
                    }
                    catch (Exception x)
                    {
                        error = SetError(x);
                        if (!Catch(x))
                        {
                            return null;
                        }
                    }
                }
                else
                {
                    using (IDbConnection connection = Open())
                    {
                        if (connection == null)
                        {
                            error = GetErrorText("Connection error");
                            return null;
                        }
                        try
                        {
                            return Fetch(connection, query);
                        }
                        catch (Exception x)
                        {
                            error = SetError(x);
                            if (!Catch(x))
                            {
                                return null;
                            }
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Fetch query results into Energy.Base.Table.
        /// Return null on error.
        /// </summary>
        /// <param name="query">Query text</param>
        /// <returns></returns>
        public Energy.Base.Table Fetch(string query)
        {
            return Fetch(query, out _);
        }

        #endregion

        #region Load

        private DataTable Load(IDbConnection connection, string query)
        {
            using (IDbCommand command = Prepare(connection, query))
            {
                return Load(command);
            }
        }

        private DataTable Load(IDbCommand command)
        {
            using (IDataReader reader = command.ExecuteReader())
            {
                DataTable table = new DataTable();
                table.Load(reader);
                reader.Close();
                return table;
            }
        }

        /// <summary>
        /// Load data from query into DataTable.
        /// Return null on error.
        /// </summary>
        /// <param name="query">Query text</param>
        /// <param name="error">Error text</param>
        /// <returns></returns>
        public DataTable Load(string query, out string error)
        {
            if (string.IsNullOrEmpty(query))
            {
                error = SetError("Empty query");
                return null;
            }
            error = ClearError();
            int attempt = Repeat;
            while (attempt-- >= 0)
            {
                if (Persistent)
                {
                    if (!Active && null == Open())
                    {
                        error = GetErrorText("Connection error");
                        return null;
                    }
                    try
                    {
                        lock (_Lock)
                        {
                            return Load(_Connection, query);
                        }
                    }
                    catch (Exception x)
                    {
                        SetError(x);
                        if (!Catch(x))
                        {
                            return null;
                        }
                    }
                }
                else
                {
                    using (IDbConnection connection = Open())
                    {
                        if (connection == null)
                        {
                            return null;
                        }
                        try
                        {
                            return Load(connection, query);
                        }
                        catch (Exception x)
                        {
                            SetError(x);
                            if (!Catch(x))
                            {
                                return null;
                            }
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Load data from query into DataTable.
        /// Return null on error.
        /// </summary>
        /// <param name="query">Query text</param>
        /// <returns></returns>
        public DataTable Load(string query)
        {
            return Load(query, out _);
        }

        #endregion

        #region Read

        private DataTable Read(IDbCommand command)
        {
            using (IDataReader reader = command.ExecuteReader())
            {
                DataTable schema = null;

                try
                {
                    schema = reader.GetSchemaTable();
                }
                catch (Exception x)
                {
                    Energy.Core.Bug.Write(x);
                }

                if (schema == null)
                {
                    return null;
                }

                DataTable table = new DataTable("Table");

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

                return table;
            }
        }

        private DataTable Read(IDbConnection connection, string query)
        {
            using (IDbCommand command = Prepare(connection, query))
            {
                return Read(command);
            }
        }

        /// <summary>
        /// Read query results into DataTable.
        /// Return null on error.
        /// This function will populate values in a loop using IDataReader. 
        /// </summary>
        /// <param name="query">Query text</param>
        /// <param name="error">Error text</param>
        /// <returns></returns>
        public DataTable Read(string query, out string error)
        {
            if (string.IsNullOrEmpty(query))
            {
                error = SetError("Empty query");
                return null;
            }
            error = ClearError();
            int attempt = Repeat;
            int repeat = 0;
            try
            {
                while (attempt-- >= 0)
                {
                    if (Persistent)
                    {
                        if (!Active && null == Open())
                        {
                            error = GetErrorText("Connection error");
                            return null;
                        }
                        try
                        {
                            lock (_Lock)
                            {
                                return Read(_Connection, query);
                            }
                        }
                        catch (Exception x)
                        {
                            error = SetError(x);
                            if (!Catch(x))
                            {
                                return null;
                            }
                        }
                    }
                    else
                    {
                        using (IDbConnection connection = Open())
                        {
                            if (connection == null)
                            {
                                error = GetErrorText("Connection error");
                                return null;
                            }
                            try
                            {
                                return Read(connection, query);
                            }
                            catch (Exception x)
                            {
                                error = SetError(x);
                                if (!Catch(x))
                                {
                                    return null;
                                }
                            }
                        }
                    }
                    repeat++;
                }
            }
            catch (Exception x)
            {
                error = SetError(x);
            }
            finally
            {
                if (repeat > 0)
                {
                    Energy.Core.Bug.Write("Energy.Source.Connection.Read"
                        , string.Format("Command repeat ({0})", repeat)
                        );
                }
            }
            return null;
        }

        /// <summary>
        /// Read query results into DataTable.
        /// Return null on error.
        /// This function will populate values in a loop using IDataReader. 
        /// </summary>
        /// <param name="query">Query string</param>
        /// <returns></returns>
        public DataTable Read(string query)
        {
            return Read(query, out _);
        }

        #endregion

        #region Prepare

        /// <summary>
        /// Prepare IDbCommand object with query string for IDbConnection object.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public IDbCommand Prepare(IDbConnection connection, string query)
        {
            IDbCommand command = connection.CreateCommand();
            command.CommandTimeout = Timeout;
            command.CommandText = query;
            return command;
        }

        #endregion

        #region Execute

        private int Execute(IDbCommand command)
        {
            return command.ExecuteNonQuery();
        }

        private int Execute(IDbConnection connection, string query)
        {
            using (IDbCommand command = Prepare(connection, query))
            {
                return Execute(command);
            }
        }

        /// <summary>
        /// Execute SQL query.
        /// For UPDATE, INSERT, and DELETE statements, the return value is the number of rows affected by the command. 
        /// For all other types of statements, the return value is -1.
        /// On error, return value is -2.
        /// </summary>
        /// <param name="query">Query text</param>
        /// <param name="error">Error text</param>
        /// <returns></returns>
        public int Execute(string query, out string error)
        {
            if (string.IsNullOrEmpty(query))
            {
                error = SetError("Empty query");
                return -2;
            }
            error = ClearError();
            int repeat = Repeat;
            while (repeat-- >= 0)
            {
                if (Persistent)
                {
                    if (!Active && null == Open())
                    {
                        error = GetErrorText("Connection error");
                        return -2;
                    }
                    try
                    {
                        lock (_Lock)
                        {
                            return Execute(_Connection, query);
                        }
                    }
                    catch (Exception x)
                    {
                        error = SetError(x);
                        if (!Catch(x))
                        {
                            return -2;
                        }
                    }
                }
                else
                {
                    using (IDbConnection connection = Open())
                    {
                        if (connection == null)
                        {
                            error = GetErrorText("Connection error");
                            return -2;
                        }
                        try
                        {
                            return Execute(connection, query);
                        }
                        catch (Exception x)
                        {
                            error = SetError(x);
                            if (!Catch(x))
                            {
                                return -2;
                            }
                        }
                    }
                }
            }
            return -2;
        }

        /// <summary>
        /// Execute SQL query.
        /// For UPDATE, INSERT, and DELETE statements, the return value is the number of rows affected by the command. 
        /// For all other types of statements, the return value is -1.
        /// On error, return value is -2.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public int Execute(string query)
        {
            return Execute(query, out _);
        }

        #endregion

        #region Parse

        public string Parse(string query)
        {
            return query;
        }

        public string Parse(string query, Energy.Query.Parameter.List parameters)
        {
            return parameters.Parse(query);
        }

        #endregion

        #region Dispose

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // The bulk of the clean-up code is implemented in Dispose(bool)
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            Close();
        }

        #endregion

        #region Scalar

        private object Scalar(IDbCommand command)
        {
            object value = command.ExecuteScalar();
            return value;
        }

        private object Scalar(IDbConnection connection, string query)
        {
            using (IDbCommand command = Prepare(connection, query))
            {
                return Scalar(command);
            }
        }

        /// <summary>
        /// Execute SQL query and read scalar value as a result.
        /// </summary>
        /// <param name="query">Query text</param>
        /// <param name="error">Error text</param>
        /// <returns></returns>
        public object Scalar(string query, out string error)
        {
            if (string.IsNullOrEmpty(query))
            {
                error = SetError("Empty query");
                return null;
            }
            error = ClearError();
            int attempt = Repeat;
            if (attempt < 0)
            {
                attempt = 0;
            }
            int repeat = 0;
            try
            {
                while (attempt-- >= 0)
                {
                    if (Persistent)
                    {
                        if (!Active && null == Open())
                        {
                            error = GetErrorText("Connection error");
                            return null;
                        }
                        try
                        {
                            lock (_Lock)
                            {
                                return Scalar(_Connection, query);
                            }
                        }
                        catch (Exception x)
                        {
                            SetError(x);
                            if (!Catch(x))
                            {
                                return null;
                            }
                        }
                    }
                    else
                    {
                        using (IDbConnection connection = Open())
                        {
                            if (connection == null)
                            {
                                error = GetErrorText("Connection error");
                                return null;
                            }
                            try
                            {
                                return Scalar(connection, query);
                            }
                            catch (Exception x)
                            {
                                SetError(x);
                                if (!Catch(x))
                                {
                                    return null;
                                }
                            }
                            finally
                            {
                                FireOnClose();
                            }
                        }
                    }
                    repeat++;
                }
            }
            finally
            {
                if (repeat > 0)
                {
                    Energy.Core.Bug.Write("C086", "Command repeat");
                }
            }
            return null;
        }

        /// <summary>
        /// Execute SQL query and read scalar value as a result.
        /// </summary>
        /// <param name="query">Query text</param>
        /// <returns></returns>
        public object Scalar(string query)
        {
            return Scalar(query, out _);
        }

        /// <summary>
        /// Execute SQL query and read scalar value as a result.
        /// </summary>
        /// <param name="query">Query text</param>
        /// <param name="error">Error text</param>
        /// <returns></returns>
        public T Scalar<T>(string query, out string error)
        {
            object value = Scalar(query, out error);
            return Energy.Base.Cast.As<T>(value);
        }

        /// <summary>
        /// Execute SQL query and read scalar value as a result.
        /// </summary>
        /// <param name="query">Query text</param>
        /// <returns></returns>
        public T Scalar<T>(string query)
        {
            object value = Scalar(query, out _);
            return Energy.Base.Cast.As<T>(value);
        }

        #endregion

        #region Copy

        public Energy.Source.Connection Copy()
        {
            Energy.Source.Connection copy = new Energy.Source.Connection();
            lock (_Lock)
            {
                copy.ConnectionString = this._ConnectionString;
                copy.Vendor = this._Vendor;
                copy.Timeout = this._Timeout;
                copy.Repeat = this._Repeat;
                copy.Persistent = this._Persistent;
                copy.Logger = this._Logger;
                copy.Dialect = this._Dialect;
                if (this._ThreadOpen)
                {
                    copy.ThreadOpen = (bool)this._ThreadOpen;
                }
            }
            return copy;
        }

        #endregion

        #region Clone

        /// <summary>
        /// Return copy of object.
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return Copy();
        }

        #endregion

        #region ToString

        public override string ToString()
        {
            return base.ToString();
        }

        #endregion
    }

    #endregion
}
