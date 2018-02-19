﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class WebHeaderCollectionExtensions
{
    public static IEnumerable<KeyValuePair<string, string>> GetHeaders(this System.Net.WebHeaderCollection webHeaderCollection)
    {
        string[] keys = webHeaderCollection.AllKeys;
        for (int i = 0; i < keys.Length; i++)
        {
            yield return new KeyValuePair<string, string>(keys[i], webHeaderCollection[keys[i]]);
        }
    }
}

