using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebAppBackendCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        public class Class
        {
            public class Status
            {
                public class Information
                {
                    public string CurrentDirectory;
                    public string LibraryVersion;

                    public Information()
                    {
                        this.CurrentDirectory = System.IO.Directory.GetCurrentDirectory();
                        this.LibraryVersion = Energy.Core.Version.LibraryVersion;
                    }
                }
            }
        }

        // GET api/status
        [HttpGet]
        public ActionResult<Class.Status.Information> Get()
        {
            return new Class.Status.Information();
        }
    }
}
