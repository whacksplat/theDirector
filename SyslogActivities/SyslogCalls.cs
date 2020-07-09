using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using NLog.Layouts;
using NLog.Targets.Syslog.Settings;
using LogLevel = NLog.LogLevel;

namespace SyslogActivities
{
    static class SyslogCalls
    {
        public static void WriteToLog(string message,NLog.LogLevel level,string hostname)
        {
            var config = new NLog.Config.LoggingConfiguration();

            MessageBuilderConfig messageCreation = new MessageBuilderConfig();

            var syslogTarget = new NLog.Targets.Syslog.SyslogTarget() { Name = "theDirectorTarget" };
            Console.WriteLine(level.ToString());
            switch (level.ToString())
            {
                case "Info":
                    syslogTarget.Layout = new SimpleLayout("@theDirector: {INFO: \"${message}\"}");
                    break;

                case "Warn":
                    syslogTarget.Layout = new SimpleLayout("@theDirector: {WARN: \"${message}\"}");
                    break;
                case "Error":
                    syslogTarget.Layout = new SimpleLayout("@theDirector: {ERROR: \"${message}\"}");
                    break;
                case "Trace":
                    syslogTarget.Layout = new SimpleLayout("@theDirector: {TRACE: \"${message}\"}");
                    break;
                case "Fatal":
                    syslogTarget.Layout = new SimpleLayout("@theDirector: {FATAL: \"${message}\"}");
                    break;
                case "Debug":
                    syslogTarget.Layout = new SimpleLayout("@theDirector: {DEBUG: \"${message}\"}");
                    break;
                default:
                    throw new Exception("LogLevel not found.");
                    break;


        }
            //syslogTarget.Layout = new SimpleLayout("@theDirector: {INFO: \"${message}\"}");

            messageCreation.Facility = Facility.Local4;
            messageCreation.Rfc = RfcNumber.Rfc5424;
            messageCreation.Rfc5424.Hostname = hostname;
            messageCreation.Rfc5424.AppName = "DAEMON.theDirector";
            messageCreation.Rfc5424.ProcId = "${processid}";
            messageCreation.Rfc5424.MsgId = "${threadid}";
            messageCreation.Rfc5424.DisableBom = true;

            config.AddRule(LogLevel.Trace, LogLevel.Fatal, syslogTarget);
            NLog.LogManager.Configuration = config;
            switch (level.ToString())
            {
                case "Info":
                    NLog.LogManager.GetCurrentClassLogger().Info(message);
                    break;
                case "Warn":
                    NLog.LogManager.GetCurrentClassLogger().Warn(message);
                    break;
                case "Error":
                    NLog.LogManager.GetCurrentClassLogger().Error(message);
                    break;
                case "Trace":
                    NLog.LogManager.GetCurrentClassLogger().Trace(message);
                    break;
                case "Fatal":
                    NLog.LogManager.GetCurrentClassLogger().Fatal(message);
                    break;
                case "Debug":
                    NLog.LogManager.GetCurrentClassLogger().Debug(message);
                    break;
                default:
                    throw new Exception("LogLevel not found.");
                    break;
            }
            //NLog.LogManager.GetCurrentClassLogger().Info(message);

        }
    }
}
