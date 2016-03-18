using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Xml.Serialization;
using System.Threading;

namespace Energy.Source
{
    public class Connection : IDisposable
    {
        #region Constructor

        public Connection()
        {
        }

        #endregion

        /// <summary>
        /// SQL dialect
        /// </summary>
        public Energy.Enumeration.SqlDialect Dialect { get; set; }

        private System.Type vendor;
 
        public System.Type Vendor 
        {
            get
            {
                return vendor;
            }
            set
            {
                if (value != vendor)
                {
                    Close();                    
                    vendor = value;
                    driver = null;
                }
            }
        }

        /// <summary>
        /// Connection string used for opening SQL connection.
        /// </summary>
        public Energy.Base.ConnectionString ConnectionString { get; set; }

        /// <summary>
        /// Log
        /// </summary>
        public Energy.Core.Log Log { get; set; }

        private DbConnection driver;
        /// <summary>
        /// SQL connection driver class.
        /// </summary>
        public DbConnection Driver
        {
            get
            {
                if (driver == null)
                {
                    lock (this)
                    {
                        if (driver == null)
                        {
                            try
                            {
                                driver = (DbConnection)Activator.CreateInstance(vendor);
                            }
                            catch
                            {
                                throw;
                            }
                            driver.ConnectionString = ConnectionString.ToString();
                        }
                    }
                }
                return driver;
            }
            set
            {
                driver = value;
                if (value != null)
                {
                    vendor = value.GetType();
                }
            }
        }

        private Configuration configuration;

        public Connection(Type vendor, string connectionString = "", Energy.Enumeration.SqlDialect dialect = Energy.Enumeration.SqlDialect.None)
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
                if (driver == null) return false;
                if (driver.State == ConnectionState.Broken || driver.State == ConnectionState.Closed) return false;
                return true;
            }
        }

        public bool Busy
        {
            get
            {
                if (driver == null) return false;
                if (driver.State == ConnectionState.Connecting || driver.State == ConnectionState.Executing || driver.State == ConnectionState.Fetching) return true;
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
            if (driver == null) return;
            try
            {
                driver.Close();
            }
            catch (DbException x)
            {
                if (Log == null)
                {
                    throw;
                }
                Log.Add(x);
            }
            driver = null;
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
            if (!Active && !Open())
            {
                return null;
            }
            return null;
        }

        public virtual string Scalar(string query)
        {
            if (!Active && !Open())
            {
                return null;
            }
            DbCommand command = Driver.CreateCommand();
            command.CommandText = Parse(query);
            try
            {
                return Energy.Base.Cast.ObjectToString(command.ExecuteScalar());
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

        public Query.Format Format { get; set; }
    }
}
