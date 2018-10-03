using System;
using System.Collections.Generic;
using System.Text;

namespace RestBinaryGetPut
{
    public class Test
    {
        public static void Make()
        {
            Energy.Core.Tilde.WriteLine("~g~Trying to make a POST request with body to example AspNetCoreApi");
            Energy.Core.Web.IgnoreCertificateValidation = true;
            string baseUrl = "http://localhost:16000/api/test/";
            string methodUrl = baseUrl + "echo/";
           
            //Energy.Core.Web.Post(methodUrl, requestBody, responseData);

            var request = new Energy.Base.Http.Request("POST", methodUrl);

            request.Encoding = System.Text.Encoding.UTF8;
            request.Body = "{ \"text\": \"Gęś\" }";
            request.ContentType = "application/javascript";
            request.Headers.Add("X-Path: x-path");

            Console.WriteLine(Energy.Base.Xml.Serialize(request));

            //Console.ReadLine();

            var response = Energy.Core.Web.Execute(request);

            Console.WriteLine(Energy.Base.Xml.Serialize(response));

            //Console.ReadLine();
        }

        internal static void AspNetCoreApi(string baseUrl)
        {
            string url;
            url = baseUrl + "test/echo";
            Energy.Base.Http.Response response;

            Energy.Core.Tilde.WriteLine($"Testing ~w~GET~0~ on ~g~{url}~0~...");
            response = Energy.Core.Web.Get(url);
            Console.WriteLine(string.Format("{0} {1}"
                , Energy.Base.Http.Response.GetStatusCode(response)
                , Energy.Base.Text.Limit(Energy.Base.Http.Response.GetBody(response), 50, "...")
                ));

            Energy.Core.Tilde.WriteLine($"Testing ~w~POST~0~ on ~g~{url}~0~...");
            response = Energy.Core.Web.Post(url, "{ \"request\": \"value\" }", "application/json");
            Console.WriteLine(string.Format("{0} {1}"
                , Energy.Base.Http.Response.GetStatusCode(response)
                , Energy.Base.Text.Limit(Energy.Base.Http.Response.GetBody(response), 70, "...")
                ));

            Energy.Core.Tilde.WriteLine($"Testing ~w~PUT~0~ on ~g~{url}~0~...");
            response = Energy.Core.Web.Put(url, "{ \"request\": \"value\" }", "application/json");
            Console.WriteLine(string.Format("{0} {1}"
                , Energy.Base.Http.Response.GetStatusCode(response)
                , Energy.Base.Text.Limit(Energy.Base.Http.Response.GetBody(response), 70, "...")
                ));

            Energy.Core.Tilde.WriteLine($"Testing ~w~PATCH~0~ on ~g~{url}~0~...");
            response = Energy.Core.Web.Patch(url, "{ \"request\": \"value\" }", "application/json");
            Console.WriteLine(string.Format("{0} {1}"
                , Energy.Base.Http.Response.GetStatusCode(response)
                , Energy.Base.Text.Limit(Energy.Base.Http.Response.GetBody(response), 70, "...")
                ));

            Energy.Core.Tilde.WriteLine($"Testing ~w~DELETE~0~ on ~g~{url}~0~...");
            response = Energy.Core.Web.Delete(url);
            Console.WriteLine(string.Format("{0} {1}"
                , Energy.Base.Http.Response.GetStatusCode(response)
                , Energy.Base.Text.Limit(Energy.Base.Http.Response.GetBody(response), 70, "...")
                ));

            Energy.Core.Tilde.WriteLine($"Testing ~w~HEAD~0~ on ~g~{url}~0~...");
            response = Energy.Core.Web.Head(url);
            Console.WriteLine(string.Format("{0} {1}"
                , Energy.Base.Http.Response.GetStatusCode(response)
                , Energy.Base.Text.Limit(string.Join(", ", Energy.Base.Http.Response.GetHeaders(response, new string[] { })), 250, "...")
                ));

            Energy.Core.Tilde.WriteLine($"Testing ~w~OPTIONS~0~ on ~g~{url}~0~...");
            response = Energy.Core.Web.Options(url);
            Console.WriteLine(string.Format("{0} {1}"
                , Energy.Base.Http.Response.GetStatusCode(response)
                , Energy.Base.Text.Limit(string.Join(", ", Energy.Base.Http.Response.GetHeaders(response, new string[] { })), 250, "...")
                ));
        }

        internal static void CheckGet(string url)
        {
            using (Energy.Core.Bug.Trap(0.0001, (TimeSpan elapsedTime) => { Console.WriteLine(elapsedTime.ToString() + " Sorry"); }))
            {
                Energy.Core.Web.Get(url);
            }
        }

        internal static void S()
        {
            System.Net.WebHeaderCollection h = new System.Net.WebHeaderCollection();
            h.Add("Accept: application/json");
            h.Add("Client: MyClient");
            foreach (var x in h.GetHeaders())
            {
                Console.WriteLine(x.Key + " = " + x.Value);
            }
        }

        private static void TestPutExe()
        {
            byte[] data = System.IO.File.ReadAllBytes(@"C:\DATA\example.exe");
            string url = "http://localhost:6000/api/storage/xyz/path/to/file.data";
            string json;
            Energy.Base.Http.Response response = Energy.Core.Web.Post(url, data);
            json = response.Body;
            int responseCode = response.StatusCode;
            Console.WriteLine(responseCode);
            Console.WriteLine(json);
            Console.ReadLine();
        }

        private static void TestLog()
        {
            Energy.Core.Log log = Energy.Core.Log.Default;
            Energy.Core.Bug.TraceLogging.On();
            log.Write("1");
            log.Flush();
            log.Exception(new Exception("ERROR"));
            log.Write("2");
            Console.WriteLine(log.ToString());
            log.Destination += new Energy.Core.Log.Target.Console() { Color = false, Immediate = false };
            log.Write("3");
            log.Flush();
            log.Write("4");
            Console.ReadLine();
        }
    }
}
