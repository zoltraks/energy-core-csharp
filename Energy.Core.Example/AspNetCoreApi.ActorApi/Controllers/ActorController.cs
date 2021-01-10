using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AspNetCoreApi.ActorApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ActorController : ControllerBase
    {
        // private readonly ILogger<WeatherForecastController> _logger;

        // public ActorListController(ILogger<WeatherForecastController> logger)
        // {
        //     _logger = logger;
        // }

        [HttpGet]
        public IEnumerable<Model.Actor> Get()
        {
            var db = Global.Static.Database.Source;
            string sql = @"SELECT `first_name` ""first"" , `last_name` ""last"" FROM sakila.actor ;";

            var dt = db.Read(sql);

            var l = new List<Model.Actor>();
            foreach (DataRow row in dt.Rows)
            {
                l.Add(new Model.Actor() {
                    First = row["first"] as string,
                    Last = row["last"] as string,
                });
            }

            return l.ToArray();

            // return new Model.Actor[] {
            //     new Model.Actor() {
            //         Name = "One",
            //     },
            //     new Model.Actor() {
            //         Name = "Two",
            //     },
            // };
        }
    }
}
