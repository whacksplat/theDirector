using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMQActivities
{
    static class RabbitTools
    {
        public static IModel GetChannel(string hostname)
        {
            IModel channel;
            var factory = new ConnectionFactory() { HostName = hostname };
            var connection = factory.CreateConnection();
            channel = connection.CreateModel();
            return channel;
        }
     }
}
