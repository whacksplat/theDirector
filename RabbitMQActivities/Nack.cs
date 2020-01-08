using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Activities;
using RabbitMQ.Client;


namespace RabbitMQActivities
{

    public sealed class Nack : CodeActivity
    {
        [RequiredArgument]
        public InArgument<string> HostName { get; set; } = "localhost";

        [RequiredArgument]
        public InArgument<string> QueueName { get; set; }

        [RequiredArgument]
        public InArgument<ulong> DeliveryTag { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            var channel = RabbitTools.GetChannel(context.GetValue(HostName));
            channel.BasicNack(context.GetValue(DeliveryTag), false,true);
        }
    }
}