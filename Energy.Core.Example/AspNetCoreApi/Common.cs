using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace AspNetCoreApi
{
    public static class Common
    {
        public static string GetToken(IHeaderDictionary header)
        {
            if (!header.ContainsKey("Authorization"))
                return null;
            string value = header["Authorization"];
            if (!value.StartsWith("Token ", StringComparison.InvariantCultureIgnoreCase))
                return null;
            string token = value.Substring("Token ".Length);
            return token;
        }
    }
}
