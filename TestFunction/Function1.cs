using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Azure.ServiceBus;
using System.Text;
using System.Collections.Generic;

namespace TestFunction
{
    public static class Function1
    {

        private static string connection_string = Environment.GetEnvironmentVariable("connectionString");
        private const string topic_name = "poc_topic";
        private static ITopicClient topicClient;

        [FunctionName("poc")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "pocServiceBusFunction")] HttpRequest req,
            ILogger log)
        {

            Dictionary<string, string> headers_dict = new Dictionary<string, string>();

            foreach (var item in req.Headers)
            {
                headers_dict.Add(item.Key, item.Value);
            }


            string req_xml = await new StreamReader(req.Body).ReadToEndAsync();

            HttpRequestModel httpRequestModel = new HttpRequestModel();
            httpRequestModel.headers = headers_dict;
            httpRequestModel.body = req_xml;

            string final_json = JsonConvert.SerializeObject(httpRequestModel, Formatting.Indented);

            topicClient = new TopicClient(connection_string, topic_name);

            Message m = new Message(Encoding.UTF8.GetBytes(final_json));

            await topicClient.SendAsync(m);

            return new OkObjectResult("Okay!");
        }
    }
}
