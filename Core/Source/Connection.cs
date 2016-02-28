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

        //[XmlIgnore]
        //public System.Type Vendor { get; set; }

        //[XmlIgnore]
        //private DbConnection driver;

        //private DbConnection Driver
        //{
        //    get
        //    {
        //        if (driver == null)
        //        {
        //            driver = (DbConnection)Activator.CreateInstance(Vendor);
        //        }
        //        return driver;
        //    }
        //}

        public Base.Enumeration.SQL Dialect { get; set; }

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

        public Energy.Base.ConnectionString ConnectionString { get; set; }

        public Energy.Core.Log Log { get; set; }

        private DbConnection driver;
        //private Type type;

        private DbConnection Driver
        {
            get
            {
                if (driver == null)
                {
                    lock (this)
                    {
                        if (driver == null)
                        {
                            driver = (DbConnection)Activator.CreateInstance(vendor);
                            driver.ConnectionString = ConnectionString.ToString();
                        }
                    }
                }
                return driver;
            }
        }

        //private static readonly object semaphore = new object();

        //public Source(Query.Layer layer, string connectionString = "")
        //    : this(layer, null, connectionString)
        //{
        //}

        public Connection(Type vendor = null, string connectionString = "", Base.Enumeration.SQL layer = Base.Enumeration.SQL.None)
        {
            if (vendor != null && !vendor.IsSubclassOf(typeof(DbConnection)))
            {
                throw new Exception("Vendor must derrive from DbConnection class");
            }            
            this.Dialect = layer;
            this.Vendor = vendor;
            this.ConnectionString = connectionString;
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
