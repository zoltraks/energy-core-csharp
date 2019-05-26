using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Energy.Core.Test.Core
{
    [TestClass]
    public class Web
    {
        [TestMethod]
        public void Rest()
        {
            Energy.Core.Web.IgnoreCertificateValidation = true;
            string service = "https://reqres.in";
            string path, url;
            path = "/api/users";
            url = service + path;
            //url = "https://localhost:44318/api/values";
            string body;
            body = @"{
    ""name"": ""morpheus"",
    ""job"": ""leader""
}";
            Energy.Base.Http.Response response;
            response = Energy.Core.Web.Post(url, body, "application/json");
            Assert.IsNotNull(response);
            response = Energy.Core.Web.Patch(url, body, "application/json");
            Assert.IsNotNull(response);
            response = Energy.Core.Web.Put(url, body, "application/json");
            Assert.IsNotNull(response);
        }
    }
}
