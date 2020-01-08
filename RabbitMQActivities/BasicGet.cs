using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Activities;
using RabbitMQ.Client;

namespace RabbitMQActivities
{
    public sealed class BasicGet : CodeActivity
    {
        [RequiredArgument]
        public InArgument<string> HostName { get; set; } = "localhost";

        [RequiredArgument]
        public InArgument<string> QueueName { get; set; }

        public OutArgument<BasicGetResult> Result { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            var channel = RabbitTools.GetChannel(context.GetValue(HostName));
            BasicGetResult result= channel.BasicGet(context.GetValue(QueueName), false);
            Result.Set(context, result);
        }
    }
}
