using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Xml.Serialization;
using System.Threading;

namespace Energy.Source
{
    public class Connection : IDisposable, Energy.Interface.IConnection
    {
        #region Constructor

        public Connection() { }

        public Connection(Type vendor)
        {
            if (vendor == null || !vendor.IsSubclassOf(typeof(DbConnection)))
            {
                throw new Exception("Vendor must derrive from DbConnection class");
            }
            this.Vendor = vendor;
        }

        public Connection(Type vendor, string connectionString, Energy.Enumeration.SqlDialect dialect)
            : this(vendor)
        {
            this.Dialect = dialect;
            this.ConnectionString = connectionString;
        }

        public Connection(Type vendor, Configuration configuration)
            : this(vendor)
        {
            this.Dialect = configuration.Dialect;
            this.ConnectionString = configuration.ConnectionString;
        }

        #endregion

        private int _Repeat = 0;
        /// <summary>Repeat operation after recoverable error</summary>
        public int Repeat { get { return _Repeat; } set { _Repeat = value; } }

        private int _Timeout = 30;
        /// <summary>Time limit in seconds for SQL operations</summary>
        public int Timeout { get { return _Timeout; } set { _Timeout = value; } }

        private bool _Pooling = true;
        private readonly object _PoolingLock = new object();
        /// <summary>Pooling</summary>
        public bool Pooling { get { lock (_PoolingLock) return _Pooling; } set { lock (_PoolingLock) _Pooling = value; } }

        private readonly object _PropertyLock = new object();

        //private Query.Dialect _Query;
        ///// <summary>Query</summary>
        //public Query.Dialect Query
        //{
        //    get
        //    {
        //        lock (_PropertyLock)
        //        {
        //            if (_Query == null && _Dialect != Enumeration.SqlDialect.None)
        //                _Query = new Source.Query.Dialect(_Dialect);
        //            return _Query;
        //        }
        //    }
        //    set
        //    {
        //        lock (_PropertyLock)
        //            _Query = value;
        //    }
        //}

        private Energy.Enumeration.SqlDialect _Dialect;
        private readonly object _DialectLock = new object();
        /// <summary>Dialect</summary>
        public Energy.Enumeration.SqlDialect Dialect
        {
            get
            {
                lock (_PropertyLock)
                    return _Dialect;
            }
            set
            {
                lock (_PropertyLock)
                {
                    _Dialect = value;
                }
            }
        }

        private System.Type _Vendor;
        private readonly object _VendorLock = new object();
        /// <summary>
        /// DbConnection vendor class for SQL connection
        /// </summary>
        public System.Type Vendor
        {
            get
            {
                lock (_VendorLock)
                    return _Vendor;
            }
            set
            {
                if (value != Vendor)
                {
                    Close();
                    Clear();
                }
                lock (_VendorLock)
                    _Vendor = value;
            }
        }

        private void Clear()
        {
            lock (_VendorLock)
                _Vendor = null;
            lock (_DriverLock)
                _Driver = null;
            //lock (_PropertyLock)
            //    _Query = null;
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

        private class LO
        {
            public LO()
            {
                Console.WriteLine("LO");
            }
        }

        private DbConnection _Driver;

        private readonly LO _DriverLock = new LO();

        /// <summary>
        /// SQL connection driver class.
        /// </summary>
        public DbConnection Driver
        {
            get
            {
                if (_Driver == null)
                {
                    lock (_DriverLock)
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

        private int _ErrorNumber = 0;
        /// <summary>ErrorNumber</summary>
        public int ErrorNumber { get { lock (_PropertyLock) return _ErrorNumber; } private set { lock (_PropertyLock) _ErrorNumber = value; } }

        private string _ErrorStatus = "";
        /// <summary>ErrorStatus</summary>
        public string ErrorStatus { get { lock (_PropertyLock) return _ErrorStatus; } private set { lock (_PropertyLock) _ErrorStatus = value; } }

        public bool Active
        {
            get
            {
                lock (_DriverLock)
                {
                    if (_Driver == null)
                        return false;
                    if (_Driver.State == ConnectionState.Broken || _Driver.State == ConnectionState.Closed)
                        return false;
                    return true;
                }
            }
        }

        public bool Busy
        {
            get
            {
                lock (_DriverLock)
                {
                    if (_Driver == null)
                        return false;
                    if (_Driver.State == ConnectionState.Connecting || _Driver.State == ConnectionState.Executing || _Driver.State == ConnectionState.Fetching)
                        return true;
                    return false;
                }
            }
        }

        public bool Open()
        {
            if (Active)
                Close();
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
        public void Close()
        {
            DbException exception = null;
            lock (_DriverLock)
            {
                if (_Driver == null)
                    return;
                try
                {
                    _Driver.Close();
                }
                catch (ObjectDisposedException x)
                {
                    System.Diagnostics.Debug.WriteLine(Energy.Core.Bug.ExceptionMessage(x, true));
                }
                catch (DbException x)
                {
                    if (Log == null)
                        throw;
                    exception = x;
                }
                finally
                {
                    _Driver = null;
                }
            }
            if (exception != null && Log != null)
                Log.Add(exception);
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
        /// <returns>True if operation can be repeated</returns>
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

        private int[] GetErrorNumber(Exception exception)
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

        public virtual DataSet Read(string query)
        {
            bool active = Active;

            if (!active && !Open()) return null;

            if (!active) Close();

            return null;
        }

        public virtual DataTable Fetch(string query)
        {
            return FetchDataTable(query);
        }

        public virtual DataTable FetchDataTable(string query)
        {
            return FetchDataTableRead(query);
        }

        public virtual DataTable FetchDataTableLoad(string query)
        {
            if (!Active && !Open())
                return null;

            ClearError();

            try
            {
                using (DbCommand command = Prepare(query))
                {
                    for (int i = 0; i <= _Repeat; i++)
                    {
                        try
                        {
                            CommandBehavior behaviour = Pooling ? CommandBehavior.CloseConnection : CommandBehavior.Default;
                            using (DbDataReader reader = command.ExecuteReader(behaviour))
                            {
                                DataTable table = new DataTable();
                                table.Load(reader);
                                command.Cancel();
                                //reader.Close(); // Reader will be closed in Dispose() so it is not needed in using section
                                return table;
                            }
                        }
                        catch (Exception x)
                        {
                            command.Cancel();
                            SetError(x);
                            if (Catch(x, command))
                                continue;
                            else
                            {
                                if (Log != null)
                                    Log.Write(x);
                                else
                                    throw;
                                return null;
                            }
                        }
                    }

                    return null;
                }
            }
            finally
            {
                if (Pooling)
                    Close();
            }
        }


        public virtual DataTable FetchDataTableRead(string query)
        {
            ClearError();

            DataTable table = null;

            try
            {
                for (int n = 0; n <= _Repeat; n++)
                {
                    if (!Active && !Open())
                        return null;

                    using (DbCommand command = Prepare(query))
                    {
                        try
                        {
                            CommandBehavior behaviour = GetDefaultCommandBehaviour();
                            DbDataReader reader = command.ExecuteReader(behaviour);

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
                            command.Cancel();
                            SetError(x);
                            if (Catch(x, command))
                                continue;
                            else
                            {
                                if (Log != null)
                                    Log.Write(x);
                                else
                                    throw;
                                return null;
                            }
                        }
                    }
                }
            }
            finally
            {
                if (Pooling)
                    Close();
            }
            return table;
        }

        private CommandBehavior GetDefaultCommandBehaviour(CommandBehavior behaviour)
        {
            if (Pooling)
                return behaviour | CommandBehavior.CloseConnection;
            else
                return behaviour | CommandBehavior.Default;
        }

        private CommandBehavior GetDefaultCommandBehaviour()
        {
            return GetDefaultCommandBehaviour(CommandBehavior.Default);
        }

        private void SetError(Exception x)
        {
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
            ErrorNumber = 0;
            ErrorStatus = "";
        }

        public virtual object Single(string query)
        {
            return new object();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        private DbCommand Prepare(string query)
        {
            DbCommand command = Driver.CreateCommand();
            //command.Connection = _Driver;
            command.CommandTimeout = Timeout;
            command.CommandText = query;
            return command;
        }

        public virtual int Execute(string query)
        {
            if (!Active && !Open())
                return GetNegativeErrorNumber();

            int count = 0;
            using (DbCommand command = Prepare(query))
            {
                try
                {
                    count = command.ExecuteNonQuery();
                    return count;
                }
                catch (DbException x)
                {
                    command.Cancel();
                    SetError(x);
                    return GetNegativeErrorNumber();
                }
                catch (Exception x)
                {
                    command.Cancel();
                    SetError(x);
                    return GetNegativeErrorNumber();
                }
                finally
                {
                    if (Pooling)
                        Close();
                }
            }
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

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Close();
            }
        }

        public virtual Energy.Base.Variant.Value Scalar(DbCommand command)
        {
            ClearError();

            DbException exception = null;

            int repeat = Repeat;
            while (true)
            {
                if (!Active && !Open())
                    return null;

                try
                {
                    object value = command.ExecuteScalar();
                    return new Energy.Base.Variant.Value(value);
                }
                catch (DbException x)
                {
                    command.Cancel();
                    if (repeat-- > 0 && Catch(x, command))
                        continue;
                    SetError(x);
                    if (Log == null)
                        throw;
                    else
                        exception = x;
                    return null;
                }
                finally
                {
                    if (Pooling)
                        Close();
                    if (exception != null)
                        Log.Write(exception);
                }
            }
        }

        public virtual Energy.Base.Variant.Value Scalar(string query)
        {
            using (DbCommand command = Prepare(query))
            {
                return Scalar(command);
            }
        }

        public virtual void Kill()
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
            this.Vendor = typeof(T);
            Dialect = Energy.Query.Dialect.Guess(Vendor.Name, Vendor.FullName);
        }

        public Connection(string connectionString)
            : this()
        {
            this.ConnectionString = connectionString;
        }

        public Connection(Energy.Enumeration.SqlDialect dialect)
        {
            this.Vendor = typeof(T);
            this.Dialect = dialect;
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
    }
}
