using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Activities;
using RabbitMQ.Client;

namespace RabbitMQActivities
{
    public sealed class CreateConnectionFactory:CodeActivity
    {
        private string hostName;

        public InArgument<string> HostName { get; set; } = "localhost";
        public OutArgument<RabbitMQ.Client.ConnectionFactory> ConnectionFactory { get; set; }

    protected override void Execute(CodeActivityContext context)
        {
            var factory = new RabbitMQ.Client.ConnectionFactory() { HostName = hostName };
            ConnectionFactory.Set(context, factory);
        }
    }
}
