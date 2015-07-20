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
        CompilerResults _compilerResults;

        public string CommandLine { get; set; }

        public ConnectToCmd()
        {
            //create the connection object and store in the singleton automation
            //AutomationHelper.automation.Connection.StartProcess("cmd.exe", "");
        }


        //public ConnectToCmd(Automation automation)
        //{
        //    _automation = automation;
        //}

        public ConnectToCmd(Guid conditionId)
        {
            ConditionId = conditionId;
            ActionId = Guid.NewGuid();
            NextScene = "";
            ActionType = Action.ActionType.ConnectToCmd;
            CompilerResults _compilerResults = null;
            AutomationHelper.automation.Connection.StartProcess("cmd.exe", "");
        }

        //implemented properties/methods
        [ReadOnly(true),
        Category("Action Properties")]
        public override Guid ConditionId { get; set; }

        [ReadOnly(true),
        Category("Action Properties")]
        public override Guid ActionId { get; set; }

        [ReadOnly(true),
        Category("Action Properties")]
        public override Action.ActionType ActionType { get; set; }


        [TypeConverter(typeof(TypeConverters.SceneNameConverter)),
        Category("Action Properties"),
        Description("The next Scene to give command to when this action completes successfully.")]
        public override string NextScene { get; set; }

        public override void BuildCode(Automation automation)
        {
            //string src = "System.Windows.Forms.MessageBox.Show(" + "\"" + Message + "\" , \"" + Title + "\");";
            //_compilerResults = CodeHelper.CreateActionCode(automation, src);
            string src = @"automation.Connection.StartProcess(""cmd.exe"", """" );";
            _compilerResults = CodeHelper.CreateActionCode(automation, src);
        }

        public override string Execute(Automation automation)
        {
            throw new NotImplementedException();
        }
    }
}
