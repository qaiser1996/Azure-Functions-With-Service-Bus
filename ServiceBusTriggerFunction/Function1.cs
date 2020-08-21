using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ServiceBusTriggerFunction
{
    public static class Function1
    {
        [FunctionName("SubscriberFunction")]
        public static async void Run([ServiceBusTrigger("poc_topic", "function_subscription", Connection = "connectionString")]string mySbMsg, ILogger log)
        {
            HttpRequestModel httpRequestModel = JsonConvert.DeserializeObject<HttpRequestModel>(mySbMsg);

            var client = new HttpClient();

            var httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://localhost:44345/shopping_service.asmx"),
                Content = new StringContent(httpRequestModel.body)
            };

            httpRequestMessage.Headers.Clear();

            foreach (KeyValuePair<string, string> item in httpRequestModel.headers)
            {
                if(item.Key.Equals("Content-Type") || item.Key.Equals("Content-Length"))
                {
                    httpRequestMessage.Content.Headers.Add(item.Key, item.Value);
                    continue;
                }
                
                httpRequestMessage.Headers.Add(item.Key, item.Value);
                
                
            }

            HttpResponseMessage response = await client.SendAsync(httpRequestMessage);

            //response.EnsureSuccessStatusCode();
            string responseBody1 = response.Content.ToString();
            string responseBody = await response.Content.ReadAsStringAsync();
            log.LogInformation(responseBody);
        }
    }
}
