using System;
using System.Collections.Generic;
using System.Text;

namespace RestBinaryGetPut
{
    public class Test
    {
        public static void Make()
        {
            Energy.Core.Web.IgnoreCertificateValidation = true;
            string baseUrl = "http://localhost:16000/api/test/";
            string methodUrl = baseUrl + "echo/";
            methodUrl += "XYZ";

            //Energy.Core.Web.Post(methodUrl, requestBody, responseData);

            var request = new Energy.Base.Http.Request("POST", methodUrl);

            request.Data = new byte[] { (byte)'A', (byte)'B', };
            Console.WriteLine(request.Body);
            request.Body = "ABC";
            Console.WriteLine(request.Data);

            request.Encoding = System.Text.Encoding.Unicode;
            request.Body = "{ \"text\": \"Gęś\" }";
            request.ContentType = "application/javascript";
            request.Headers.Add("X-Path: x-path");

            Console.WriteLine(Energy.Base.Xml.Serialize(request));

            Console.ReadLine();

            request.Method = "GET";
            request.Body = null;

            var response = Energy.Core.Web.Execute(request);

            Console.WriteLine(Energy.Base.Xml.Serialize(response));

            Console.ReadLine();
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
