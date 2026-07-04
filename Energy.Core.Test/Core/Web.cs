using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Energy.Core.Test.Core
{
    [TestClass]
    public class Web
    {
        [TestMethod]
        public void Rest()
        {
            string body = @"{
    ""name"": ""morpheus"",
    ""job"": ""leader""
}";
            byte[] responseBytes = Encoding.UTF8.GetBytes(body);

            int port;
            using (TcpListener probe = new TcpListener(IPAddress.Loopback, 0))
            {
                probe.Start();
                port = ((IPEndPoint)probe.LocalEndpoint).Port;
                probe.Stop();
            }

            string prefix = "http://localhost:" + port + "/";
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add(prefix);
            listener.Start();

            Thread listenerThread = new Thread(() =>
            {
                for (int i = 0; i < 3; i++)
                {
                    try
                    {
                        HttpListenerContext context = listener.GetContext();
                        context.Response.StatusCode = 200;
                        context.Response.ContentType = "application/json";
                        context.Response.ContentLength64 = responseBytes.Length;
                        context.Response.OutputStream.Write(responseBytes, 0, responseBytes.Length);
                        context.Response.Close();
                    }
                    catch (HttpListenerException)
                    {
                        break;
                    }
                }
            });
            listenerThread.Start();

            try
            {
                Energy.Base.Http.Response response;
                response = Energy.Core.Web.Post(prefix, body, "application/json");
                Assert.IsNotNull(response);
                response = Energy.Core.Web.Patch(prefix, body, "application/json");
                Assert.IsNotNull(response);
                response = Energy.Core.Web.Put(prefix, body, "application/json");
                Assert.IsNotNull(response);
            }
            finally
            {
                listener.Stop();
                listenerThread.Join(TimeSpan.FromSeconds(5));
                listener.Close();
            }
        }
    }
}
