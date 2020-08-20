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

namespace TestFunction
{
    public static class Function1
    {

        private const string connection_string = "<connection_string>";
        private const string topic_name = "poc_topic";
        private static ITopicClient topicClient;

        [FunctionName("poc")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "pocServiceBusFunction")] HttpRequest req,
            ILogger log)
        {

            string req_xml = await new StreamReader(req.Body).ReadToEndAsync();

            topicClient = new TopicClient(connection_string, topic_name);

            Message m = new Message(Encoding.UTF8.GetBytes(req_xml));

            await topicClient.SendAsync(m);

            return new OkObjectResult("Okay!");
        }
    }
}
