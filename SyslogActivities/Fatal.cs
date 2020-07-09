using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Activities;
using NLog;

namespace SyslogActivities
{
    public sealed class Fatal:CodeActivity
    {
        [RequiredArgument] public InArgument<string> Message { get; set; } = "My Message";
        [RequiredArgument] public InArgument<string> Hostname { get; set; } = "127.0.0.1";

        protected override void Execute(CodeActivityContext context)
        {
            SyslogCalls.WriteToLog(context.GetValue(Message), LogLevel.Fatal, context.GetValue(Hostname));
        }
    }
}
