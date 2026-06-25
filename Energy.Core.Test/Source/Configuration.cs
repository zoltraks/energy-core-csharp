using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Energy.Core.Test.Source
{
    [TestClass]
    public class Configuration
    {
        [TestMethod]
        public void Configuration_DefaultValues()
        {
            var config = new Energy.Source.Configuration();

            Assert.AreEqual(Energy.Enumeration.SqlDialect.ANSI, config.Dialect);
            Assert.AreEqual(0, config.Port);
            Assert.IsFalse(config.Encryption);
            Assert.IsFalse(config.Compression);
            Assert.AreEqual(0, config.Timeout);
            Assert.IsNull(config.Server);
            Assert.IsNull(config.Catalog);
            Assert.IsNull(config.User);
            Assert.IsNull(config.Password);
            Assert.IsNull(config.Charset);
        }

        [TestMethod]
        public void Configuration_PropertyAssignment()
        {
            var config = new Energy.Source.Configuration
            {
                Dialect = Energy.Enumeration.SqlDialect.MYSQL,
                Server = "localhost",
                Port = 3306,
                Catalog = "testdb",
                User = "testuser",
                Password = "testpass",
                Encryption = true,
                Compression = true,
                Timeout = 30,
                Charset = "utf8"
            };

            Assert.AreEqual(Energy.Enumeration.SqlDialect.MYSQL, config.Dialect);
            Assert.AreEqual("localhost", config.Server);
            Assert.AreEqual(3306, config.Port);
            Assert.AreEqual("testdb", config.Catalog);
            Assert.AreEqual("testuser", config.User);
            Assert.AreEqual("testpass", config.Password);
            Assert.IsTrue(config.Encryption);
            Assert.IsTrue(config.Compression);
            Assert.AreEqual(30, config.Timeout);
            Assert.AreEqual("utf8", config.Charset);
        }

        [TestMethod]
        public void Configuration_GetConnectionString_SqlServer_Defaults()
        {
            var config = new Energy.Source.Configuration
            {
                Dialect = Energy.Enumeration.SqlDialect.SQLSERVER
            };

            string connectionString = config.GetConnectionString();
            Assert.Contains("Data Source=.", connectionString);
            Assert.Contains("Integrated Security=Yes", connectionString);
        }

        [TestMethod]
        public void Configuration_GetConnectionString_SqlServer_WithCredentials()
        {
            var config = new Energy.Source.Configuration
            {
                Dialect = Energy.Enumeration.SqlDialect.SQLSERVER,
                Server = "MYSERVER",
                Port = 1433,
                Catalog = "My Database",
                User = "myuser",
                Password = "mypass",
                Timeout = 15
            };

            string connectionString = config.GetConnectionString();
            Assert.Contains("Data Source=MYSERVER,1433", connectionString);
            Assert.Contains("Initial Catalog=My Database", connectionString);
            Assert.Contains("User ID=myuser", connectionString);
            Assert.Contains("Password=mypass", connectionString);
            Assert.Contains("Connect Timeout=15", connectionString);
            Assert.DoesNotContain("Integrated Security", connectionString);
        }

        [TestMethod]
        public void Configuration_GetConnectionString_MySql()
        {
            var config = new Energy.Source.Configuration
            {
                Dialect = Energy.Enumeration.SqlDialect.MYSQL,
                Server = "mysql.example.com",
                Port = 3306,
                Catalog = "mydb",
                User = "mysqluser",
                Password = "mysqlpass",
                Encryption = true
            };

            string connectionString = config.GetConnectionString();
            Assert.Contains("Server=mysql.example.com", connectionString);
            Assert.Contains("Port=3306", connectionString);
            Assert.Contains("Database=mydb", connectionString);
            Assert.Contains("Uid=mysqluser", connectionString);
            Assert.Contains("Pwd=mysqlpass", connectionString);
            Assert.Contains("Encrypt=true", connectionString);
        }

        [TestMethod]
        public void Configuration_GetEntityConnectionString()
        {
            var config = new Energy.Source.Configuration
            {
                Dialect = Energy.Enumeration.SqlDialect.SQLSERVER,
                Server = "testserver",
                Catalog = "testdb"
            };

            string entityConnectionString = config.GetEntityConnectionString("System.Data.SqlClient");
            Assert.Contains("metadata=res://*", entityConnectionString);
            Assert.Contains("provider=System.Data.SqlClient", entityConnectionString);
            Assert.Contains("provider connection string=", entityConnectionString);
            Assert.Contains(config.GetConnectionString(), entityConnectionString);
        }

        [TestMethod]
        public void Configuration_GetDSNPrefix()
        {
            var config = new Energy.Source.Configuration();

            config.Dialect = Energy.Enumeration.SqlDialect.MYSQL;
            Assert.AreEqual("mysql", config.GetDSNPrefix());

            config.Dialect = Energy.Enumeration.SqlDialect.SQLSERVER;
            Assert.AreEqual("sqlsrv", config.GetDSNPrefix());

            config.Dialect = Energy.Enumeration.SqlDialect.POSTGRESQL;
            Assert.AreEqual("pg", config.GetDSNPrefix());

            config.Dialect = Energy.Enumeration.SqlDialect.ORACLE;
            Assert.AreEqual("oracle", config.GetDSNPrefix());

            config.Dialect = Energy.Enumeration.SqlDialect.SQLITE;
            Assert.AreEqual("sqlite", config.GetDSNPrefix());

            config.Dialect = Energy.Enumeration.SqlDialect.FIREBIRD;
            Assert.AreEqual("firebird", config.GetDSNPrefix());
        }

        [TestMethod]
        public void Configuration_ToString_SqlServer()
        {
            var config = new Energy.Source.Configuration
            {
                Dialect = Energy.Enumeration.SqlDialect.SQLSERVER,
                Server = "myserver",
                Port = 1433,
                Catalog = "mydatabase"
            };

            string result = config.ToString();
            Assert.Contains("sqlsrv:", result);
            Assert.Contains("Server=myserver,1433", result);
            Assert.Contains("Database=mydatabase", result);
        }

        [TestMethod]
        public void Configuration_ToString_MySql()
        {
            var config = new Energy.Source.Configuration
            {
                Dialect = Energy.Enumeration.SqlDialect.MYSQL,
                Server = "mysqlhost",
                Port = 3306,
                Catalog = "mysqldb",
                Charset = "utf8mb4"
            };

            string result = config.ToString();
            Assert.Contains("mysql:", result);
            Assert.Contains("host=mysqlhost", result);
            Assert.Contains("port=3306", result);
            Assert.Contains("dbname=mysqldb", result);
            Assert.Contains("charset=utf8mb4", result);
        }

        [TestMethod]
        public void Configuration_ConnectionStringProperty()
        {
            var config = new Energy.Source.Configuration
            {
                Dialect = Energy.Enumeration.SqlDialect.SQLSERVER,
                Server = "testserver"
            };

            Assert.AreEqual(config.GetConnectionString(), config.ConnectionString);
        }

        [TestMethod]
        public void Configuration_EmptyConfiguration()
        {
            var config = new Energy.Source.Configuration();

            // Test with minimal configuration
            config.Dialect = Energy.Enumeration.SqlDialect.SQLITE;
            string connectionString = config.GetConnectionString();
            Assert.AreEqual("", connectionString);

            string dsnPrefix = config.GetDSNPrefix();
            Assert.AreEqual("sqlite", dsnPrefix);
        }
    }
}
