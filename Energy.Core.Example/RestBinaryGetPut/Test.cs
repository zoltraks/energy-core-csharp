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
    }
}
