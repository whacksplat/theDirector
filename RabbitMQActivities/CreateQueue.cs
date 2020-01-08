using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Activities;
using RabbitMQ.Client;


namespace RabbitMQActivities
{
    public sealed class CreateQueue : CodeActivity
    {
        [RequiredArgument]
        public InArgument<string> HostName { get; set; } = "localhost";

        [RequiredArgument]
        public InArgument<string> QueueName { get; set; }
        
        [RequiredArgument]
        public bool Durable { get; set; }

        [RequiredArgument]
        public bool Exclusive { get; set; }
        
        [RequiredArgument]
        public bool AutoDelete { get; set; }
        
        public InArgument<IDictionary<string, object>> Arguments { get; set; } = null;

        protected override void Execute(CodeActivityContext context)
        {
            var channel = RabbitTools.GetChannel(context.GetValue(HostName));
            var ret = channel.QueueDeclare(context.GetValue(QueueName), Durable, Exclusive, AutoDelete, context.GetValue(Arguments));
            
        }
    }
}
