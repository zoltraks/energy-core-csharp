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

        #endregion

        private int _Repeat = 1;
        /// <summary>Repeat</summary>
        public int Repeat { get { return _Repeat; } set { _Repeat = value; } }

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

        private Configuration configuration;

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
            this.configuration = configuration;
        }

        public bool Active
        {
            get
            {
                if (_Driver == null) return false;
                if (_Driver.State == ConnectionState.Broken || _Driver.State == ConnectionState.Closed) return false;
                return true;
            }
        }

        public bool Busy
        {
            get
            {
                if (_Driver == null) return false;
                if (_Driver.State == ConnectionState.Connecting || _Driver.State == ConnectionState.Executing || _Driver.State == ConnectionState.Fetching) return true;
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
                if (Log != null) Log.Write("Opening connection", Energy.Core.Log.Level.Trace);
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
            catch (Exception x)
            {
                if (Log == null)
                {
                    throw;
                }
                else
                {
                    Log.Write(x);
                    return false;
                }
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
                }
                catch { }
            }
        }

        private void Prepare(DbCommand command)
        {
            command.Connection = _Driver;
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

    public class Connection<T>: Connection
    {
        public Connection()
        {
            Vendor = typeof(T);
        }
    }
}
