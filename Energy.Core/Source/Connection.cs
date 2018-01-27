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

        private Energy.Core.Log _Log;

        /// <summary>
        /// Log
        /// </summary>
        public Energy.Core.Log Log { get { lock (_Lock) return _Log; } set { lock (_Lock) _Log = value; } }

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
                throw new ArgumentNullException();
            if (!typeof(IDbConnection).IsAssignableFrom(vendor))
                throw new Exception("Vendor must implement IDbConnection interface");
            lock (_Lock)
            {
                if (vendor == _Vendor)
                    return;
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
                    return;
                if (!persistent)
                    Close();
                _Persistent = persistent;
            }
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

        #endregion

        #region Event

        public event EventHandler OnCreate;

        public event EventHandler OnOpen;

        public event EventHandler OnClose;

        #endregion

        #region Error

        private void SetError(Exception x)
        {
            int number = 0;
            int[] error = Energy.Source.Error.GetErrorNumber(x);
            if (error.Length > 0)
                number = error[0];
            lock (_Lock)
            {
                _ErrorNumber = number;
                _ErrorMessage = x.Message;
                _ErrorException = x;
            }
        }

        private void ClearError()
        {
            lock (_Lock)
            {
                _ErrorNumber = 0;
                _ErrorMessage = "";
                _ErrorException = null;
            }
        }

        private int GetNegativeErrorNumber()
        {
            int number = ErrorNumber;
            if (number == 0)
                return -1;
            return number > 0 ? -number : number;
        }

        /// <summary>
        /// Get error text
        /// </summary>
        /// <returns></returns>
        public string GetErrorText()
        {
            List<string> list = new List<string>();
            if (ErrorNumber != 0)
                list.Add(ErrorNumber.ToString());
            if (!string.IsNullOrEmpty(ErrorMessage))
                list.Add(Regex.Replace(ErrorMessage, @"(\r\n|\n|\r)+", " "));
            return string.Join(" ", list.ToArray());
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
                (Log ?? Energy.Core.Log.Default).Write(x);
            }
            finally
            {
                if (OnCreate != null)
                    OnCreate(this, null);
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

            if (!success)
                connection = null;

            if (OnOpen != null)
                OnOpen(this, null);

            return connection;
        }

        public IDbConnection Open()
        {
            lock (_Lock)
            {
                if (_Persistent)
                {
                    Close();
                    _Connection = Open(Create());
                    return _Connection;
                }
            }

            return Open(Create());
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
                    return;
                try
                {
                    _Connection.Close();
                }
                catch (Exception x)
                {
                    SetError(x);
                    (Log ?? Energy.Core.Log.Default).Write(x);
                }
                try
                {
                    _Connection.Dispose();
                }
                catch
                {
                    Energy.Core.Bug.Write("Exception during connection dispose...");
                }
                finally
                {
                    _Connection = null;
                    if (OnClose != null)
                        OnClose(this, null);
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
        /// Check if database connection is active.
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
                case ConnectionState.Closed:
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

            (Log ?? Core.Log.Default).Add(result.ToString(), Energy.Enumeration.LogLevel.Trace);

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

        public Energy.Base.Table Fetch(string query)
        {
            ClearError();

            Energy.Base.Table table = null;

            int repeat = Repeat;

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

                        table = new Energy.Base.Table();

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

                        break;
                    }
                    catch (Exception x)
                    {
                        SetError(x);
                        if (Catch(x))
                            continue;
                        else
                        {
                            (Log ?? Energy.Core.Log.Default).Write(x);
                            return null;
                        }
                    }
                }
            }
            return table;
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
        /// Load data from query into DataTable
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public DataTable Load(string query)
        {
            ClearError();
            int repeat = Repeat;
            while (repeat-- >= 0)
            {
                if (Persistent)
                {
                    if (!Active && Open() == null)
                        return null;
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
                            return null;
                    }
                }
                else
                {
                    using (IDbConnection connection = Open())
                    {
                        if (connection == null)
                            return null;
                        try
                        {
                            return Load(connection, query);
                        }
                        catch (Exception x)
                        {
                            SetError(x);
                            if (!Catch(x))
                                return null;
                        }
                    }
                }
            }
            return null;
        }

        #endregion

        #region Read

        private DataTable Read(IDbCommand command)
        {
            using (IDataReader reader = command.ExecuteReader())
            {
                DataTable schema = reader.GetSchemaTable();

                if (schema == null)
                    return null;

                DataTable table = new DataTable();

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
        /// Read query results into DataTable
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public DataTable Read(string query)
        {
            ClearError();
            int repeat = Repeat;
            while (repeat-- >= 0)
            {
                if (Persistent)
                {
                    if (!Active && Open() == null)
                        return null;
                    try
                    {
                        lock (_Lock)
                        {
                            return Read(_Connection, query);
                        }
                    }
                    catch (Exception x)
                    {
                        SetError(x);
                        if (!Catch(x))
                            return null;
                    }
                }
                else
                {
                    using (IDbConnection connection = Open())
                    {
                        if (connection == null)
                            return null;
                        try
                        {
                            return Read(connection, query);
                        }
                        catch (Exception x)
                        {
                            SetError(x);
                            if (!Catch(x))
                                return null;
                        }
                    }
                }
            }
            return null;
        }

        #endregion

        #region Prepare

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

        public int Execute(string query)
        {
            ClearError();
            int repeat = Repeat;
            while (repeat-- >= 0)
            {
                if (Persistent)
                {
                    if (!Active && Open() == null)
                        return -1;
                    try
                    {
                        lock (_Lock)
                        {
                            return Execute(_Connection, query);
                        }
                    }
                    catch (Exception x)
                    {
                        SetError(x);
                        if (!Catch(x))
                            return -1;
                    }
                }
                else
                {
                    using (IDbConnection connection = Open())
                    {
                        if (connection == null)
                            return -1;
                        try
                        {
                            return Execute(connection, query);
                        }
                        catch (Exception x)
                        {
                            SetError(x);
                            if (!Catch(x))
                                return -1;
                        }
                    }
                }
            }
            return -1;
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

        public object Scalar(string query)
        {
            ClearError();
            int repeat = Repeat;
            while (repeat-- >= 0)
            {
                if (Persistent)
                {
                    if (!Active && Open() == null)
                        return null;
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
                            return null;
                    }
                }
                else
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
                            SetError(x);
                            if (!Catch(x))
                                return null;
                        }
                    }
                }
            }
            return null;
        }

        public T Scalar<T>(string query)
        {
            object value = Scalar(query);
            return Energy.Base.Cast.As<T>(value);
        }

        #endregion

        #region Copy

        public Energy.Source.Connection Copy()
        {
            Energy.Source.Connection copy = new Energy.Source.Connection();
            lock (_Lock)
            {
                copy.ConnectionString = _ConnectionString;
                copy.Vendor = _Vendor;
                copy.Timeout = _Timeout;
                copy.Repeat = _Repeat;
                copy.Log = _Log;
                copy.Persistent = _Persistent;
            }
            return copy;
        }

        #endregion

        #region Clone

        public object Clone()
        {
            return Copy();
        }

        #endregion
    }

    #endregion
}
