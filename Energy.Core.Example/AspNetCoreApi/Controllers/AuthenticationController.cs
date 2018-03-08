using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreApi.Controllers
{
    [Produces("application/json")]
    [Route("api/authentication")]
    public class AuthenticationController : Controller
    {
        // GET: api/authentication/Abc123
        [HttpGet("{user}")]
        public string Get(string user)
        {
            return "value";
        }

        // POST: api/authentication/token
        [HttpPost("token")]
        public void PostTokenBody()
        {
        }

        // PUT: api/authentication/token
        [HttpPut("token")]
        public void PutTokenBody()
        {
        }

        /// <summary>
        /// Create new authentication token
        /// </summary>
        /// <param name="token"></param>
        // POST: api/authentication/token
        [HttpPost("token/{token}")]
        public void PostTokenBody(string token)
        {
        }

        /// <summary>
        /// Update existing authentication token
        /// </summary>
        /// <param name="token"></param>
        // PUT: api/authentication/token
        [HttpPut("token/{token}")]
        public void PutTokenBody(string token)
        {
        }

        /// <summary>
        /// Delete existing token
        /// </summary>
        /// <param name="token"></param>
        // DELETE: api/token/Abc123
        [HttpDelete("token/{token}")]
        public void Delete(string token)
        {
        }
    }
}
