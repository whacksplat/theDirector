 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using SyslogLogging;
using System.Activities;
using NLog;
using NLog.Layouts;
using NLog.Targets.Syslog.Settings;

namespace SyslogActivities
{
    public sealed class Info:CodeActivity
    {
        //private static readonly NLog.Logger Logger = NLog.LogManager.GetLogger("theDirectorRule");


        [RequiredArgument] public InArgument<string> Message { get; set; } = "My Message";
        [RequiredArgument] public InArgument<string> Hostname { get; set; } = "127.0.0.1";

        protected override void Execute(CodeActivityContext context)
        {
            //var config = new NLog.Config.LoggingConfiguration();

            //MessageBuilderConfig messageCreation = new MessageBuilderConfig();

            //var syslogTarget = new NLog.Targets.Syslog.SyslogTarget() { Name = "theDirectorTarget" };
            //syslogTarget.Layout = new SimpleLayout("@theDirector: {INFO: \"${message}\"}");

            //messageCreation.Facility = Facility.Local4;
            //messageCreation.Rfc = RfcNumber.Rfc5424;
            //messageCreation.Rfc5424.Hostname = "127.0.0.1";
            //messageCreation.Rfc5424.AppName = "DAEMON.theDirector";
            //messageCreation.Rfc5424.ProcId = "${processid}";
            //messageCreation.Rfc5424.MsgId = "${threadid}";
            //messageCreation.Rfc5424.DisableBom = true;

            //config.AddRule(LogLevel.Trace, LogLevel.Fatal, syslogTarget);
            //NLog.LogManager.Configuration = config;
            //NLog.LogManager.GetCurrentClassLogger().Info(context.GetValue(Message));
            SyslogCalls.WriteToLog(context.GetValue(Message), LogLevel.Info, context.GetValue(Hostname));
        }
    }
}
