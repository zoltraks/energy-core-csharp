using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace AspNetCoreApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (!Initialize())
                return;

            var configuration = new ConfigurationBuilder()
                   .AddCommandLine(args)
                   .Build();

            var hostUrl = configuration["hosturl"];

            if (string.IsNullOrEmpty(hostUrl))
                hostUrl = "http://0.0.0.0:6000";

            var host = new WebHostBuilder()
                .UseKestrel()
                .UseUrls(hostUrl)
                .UseContentRoot(System.IO.Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .UseConfiguration(configuration)
                .Build();

            host.Run();
        }

        private static bool Initialize()
        {
            Static.Configuration = new Class.Configuration
            {
                Master = "666",
                Dummy = "1"
            };

            return true;
        }

        //public static IWebHost BuildWebHost(string[] args, dynamic configuration) =>
        //    WebHost.CreateDefaultBuilder(args)
        //        .UseKestrel()
        //        .UseContentRoot(Directory.GetCurrentDirectory())
        //        .UseIISIntegration()
        //        .UseStartup<Startup>()
        //        .UseConfiguration(configuration)
        //        .Build();
    }
}
