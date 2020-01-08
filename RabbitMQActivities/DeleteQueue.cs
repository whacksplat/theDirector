using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Activities;
using RabbitMQ.Client;


namespace RabbitMQActivities
{
    public sealed class DeleteQueue : CodeActivity
    {
        [RequiredArgument]
        public InArgument<string> HostName { get; set; } = "localhost";

        [RequiredArgument]
        public InArgument<string> QueueName { get; set; }

        public bool IfUnused { get; set; }
        public bool IfEmpty { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            var channel = RabbitTools.GetChannel(context.GetValue(HostName));
            uint retval = channel.QueueDelete(context.GetValue(QueueName), IfUnused, IfEmpty);
        }
    }
}
