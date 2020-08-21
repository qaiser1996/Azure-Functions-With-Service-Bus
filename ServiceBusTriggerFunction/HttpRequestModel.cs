using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace ServiceBusTriggerFunction
{
    public class HttpRequestModel
    {
        public Dictionary<string, string> headers;
        public string body;
    }
}


