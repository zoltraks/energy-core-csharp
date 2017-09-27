using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApplicationAspNetCoreExample.Controllers
{
    [Produces("application/json")]
    [Route("api/configuration")]
    public class ConfigurationController : Controller
    {
        /// <summary>
        /// Get all configuration items
        /// </summary>
        /// <returns></returns>
        // GET: api/configuration
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        /// <summary>
        /// Get configuration item
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        // GET: api/configuration/key
        [HttpGet("{key}")]
        public string Get(string key)
        {
            return "value";
        }

        // POST: api/configuration
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Configuration/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
