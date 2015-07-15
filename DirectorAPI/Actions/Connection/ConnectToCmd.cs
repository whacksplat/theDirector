using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Runtime.Serialization;


namespace DirectorAPI.Actions.Connection
{
    public class ConnectToCmd : AAction
    {
        private Automation _automation;
        private CompilerResults _results;

        public string CommandLine { get; set; }

        public ConnectToCmd()
        {
            //create the connection object and store in the singleton automation
            AutomationHelper.automation.Connection.StartProcess("cmd.exe", "");

        }

        public ConnectToCmd(Automation automation)
        {
            _automation = automation;
        }

        [TypeConverter(typeof(TypeConverters.SceneNameConverter)),
        Category("Action Properties"),
        Description("The next Scene to give command to when this action completes successfully.")]
        public override string NextScene { get; set; }

        public override Guid ConditionId { get; set; }
        public override Guid ActionId { get; set; }
        public override Action.ActionType ActionType { get; set; }

        public override void BuildCode(Automation automation)
        {
            throw new NotImplementedException();
        }

        public override string Execute(Automation automation)
        {
            throw new NotImplementedException();
        }
    }
}
