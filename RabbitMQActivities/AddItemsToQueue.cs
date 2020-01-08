using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Activities;
using RabbitMQ.Client;

namespace RabbitMQActivities
{

    public sealed class AddItemsToQueue : CodeActivity
    {
        [RequiredArgument]
        public InArgument<string> HostName { get; set; } = "localhost";

        [RequiredArgument]
        public InArgument<string> QueueName { get; set; }

        [RequiredArgument]
        public InArgument<string> Body { get; set; }

        public InArgument<string> Exchange { get; set; } = null;

        public InArgument<IBasicProperties> BasicProperties { get; set; } = null;

        protected override void Execute(CodeActivityContext context)
        {
            var channel = RabbitTools.GetChannel(context.GetValue(HostName));
            string exc = context.GetValue(Exchange);
            if (string.IsNullOrWhiteSpace(exc))
            {
                exc = string.Empty;
            }

            channel.BasicPublish(exc, "testing", null, Encoding.UTF8.GetBytes(exc));

        }
    }
}
