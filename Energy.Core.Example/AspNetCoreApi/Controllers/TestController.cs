using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace AspNetCoreApi.Controllers
{
    [Produces("application/json")]
    [Route("api/test")]
    public class TestController : Controller
    {
        /// <summary>
        /// Get version number
        /// </summary>
        /// <returns></returns>
        // GET: api/test/version
        [HttpGet("version")]
        public object GetVersion()
        {
            string version = Energy.Core.Version.GetProduct(System.Reflection.Assembly.GetExecutingAssembly());
            string directory = System.IO.Directory.GetCurrentDirectory();
            string platform = Environment.Version.ToString();
            return new
            {
                version = version,
                directory = directory,
                platform = platform,
            };
        }

        /// <summary>
        /// Get text back from request
        /// </summary>
        /// <param name="echo">Text to echo back</param>
        /// <returns></returns>
        // GET: api/test/echo/Abc123
        [HttpGet("echo/{echo}")]
        public object GetEcho(string echo)
        {
            return echo;
        }

        /// <summary>
        /// Get body back from request
        /// </summary>
        /// <returns></returns>
        // GET: api/test/echo
        [HttpGet("echo")]
        public string GetEchoBody()
        {
            string response = Request.GetRawBodyString();
            return response;
        }

        /// <summary>
        /// Get body back from request
        /// </summary>
        /// <returns></returns>
        // PUT: api/test/echo
        [HttpPut("echo")]
        public string PutEchoBody()
        {
            string response = Request.GetRawBodyString();
            return response;
        }

        /// <summary>
        /// Get body back from request
        /// </summary>
        /// <returns></returns>
        // PUT: api/test/echo
        [HttpPost("echo")]
        public object PostEchoBody()
        {
            string response = Request.GetRawBodyString();
            return response;
        }

        /// <summary>
        /// Perform file creation test
        /// </summary>
        /// <returns></returns>
        // GET: api/test/file
        [HttpGet("file")]
        public object GetFile()
        {
            string hex = Energy.Base.Random.GetRandomHex();
            string name = hex + ".txt";
            string path = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Storage", "Temporary");
            string file = System.IO.Path.Combine(path, name);
            try
            {
                if (!Energy.Base.File.MakeDirectory(path))
                    throw new Exception("Could not create temporary directory");
                System.IO.File.WriteAllText(file, hex);
                long size = new System.IO.FileInfo(file).Length;
                System.IO.File.Delete(file);
                return new
                {
                    file = new
                    {
                        name = name,
                        path = path,
                        size = size,
                    },
                };
            }
            catch (Exception x)
            {
                return new { exception = new { message = x.Message, }, };
            }
        }

        /// <summary>
        /// Return list of headers
        /// </summary>
        /// <returns></returns>
        // GET: api/test/header
        [HttpGet("header")]
        public object GetHeader()
        {
            Energy.Base.Collection.StringDictionary d = new Energy.Base.Collection.StringDictionary();

            foreach (KeyValuePair<string, StringValues> _ in Request.Headers)
            {
                d[_.Key] = _.Value;
            }

            return d;
        }

        /// <summary>
        /// Return value of token passed as standard HTTP Authorization header
        /// </summary>
        /// <returns></returns>
        // GET: api/test/header/token
        [HttpGet("header/token")]
        public object GetHeaderToken()
        {
            string token = Common.GetToken(Request.Headers);
            if (token == null)
                return new
                {
                    exception = new { message = "No authorization header present", },
                };
            return new
            {
                token = token,
            };
        }

        /// <summary>
        /// Return list of headers
        /// </summary>
        /// <returns></returns>
        // PUT: api/test/header
        [HttpPut("header")]
        public object PutHeader()
        {
            return GetHeader();
        }

        /// <summary>
        /// Return list of headers
        /// </summary>
        /// <returns></returns>
        // POST: api/test/header
        [HttpPost("header")]
        public object PostHeader()
        {
            return GetHeader();
        }

        /// <summary>
        /// Return configuration information
        /// </summary>
        /// <returns></returns>
        // GET: api/test/configuration
        [HttpGet("configuration")]
        public object GetConfiguraion()
        {
            try
            {
                string file = Energy.Base.File.Locate("Configuration.xml", new string[] { "..", "." }, null);
                return new
                {
                    file = new
                    {
                        name = file,
                    }
                };
            }
            catch (Exception x)
            {
                return new { exception = new { message = x.Message, }, };
            }
        }
    }
}
