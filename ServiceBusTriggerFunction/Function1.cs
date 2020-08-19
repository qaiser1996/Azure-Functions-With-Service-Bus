using System;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace ServiceBusTriggerFunction
{
    public static class Function1
    {
        [FunctionName("SubscriberFunction")]
        public static async void Run([ServiceBusTrigger("poc_topic", "function_subscription", Connection = "connectionString")]string mySbMsg, ILogger log)
        {
            var client = new HttpClient();

            var httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://localhost:44345/shopping_service.asmx"),
                Headers = {
                { HttpRequestHeader.Authorization.ToString(), "Qaiser" },
                { HttpRequestHeader.Accept.ToString(), "application/xml" },
                { "X-Version", "1" },
                 
            },
                Content = new StringContent(mySbMsg)
            };

            HttpResponseMessage response = await client.SendAsync(httpRequestMessage);

            //response.EnsureSuccessStatusCode();
            string responseBody1 = response.Content.ToString();
            string responseBody = await response.Content.ReadAsStringAsync();
            log.LogInformation(responseBody);
        }
    }
}
