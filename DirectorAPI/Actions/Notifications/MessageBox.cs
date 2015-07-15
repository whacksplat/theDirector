using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Runtime.Serialization;

namespace DirectorAPI.Actions.Notifications
{
    public class MessageBox:AAction
    {
        private string _message;
        private string _title;
        private string _nextScene;
        //private Guid _actionId;
        //private Action.ActionType _actionType;
        CompilerResults _compilerResults;

        [Category("General"),
        Description("This is the Message that will appear in the body of the MessageBox.")]
        public string Message
        {
            get { return _message;}
            
            set
            {
                _message = value;
            }
        }

        [Category("General"),
        Description("This is what will appear in the title bar of the MessageBox.")]
        public string Title
        {
            get {return _title;}
            set
            {
                _title = value;
            }
        }

        public MessageBox() 
        {
        }

        public MessageBox(Guid conditionId)
        {
            //these fields need to be populated in order to be serialized
            _message = "";
            _title = "";
            ConditionId = conditionId;
            ActionId = Guid.NewGuid();
            NextScene = "";
            CompilerResults _compilerResults = null;
        }


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
            string src = "System.Windows.Forms.MessageBox.Show(" + "\"" + Message + "\" , \"" + Title + "\");";
            _compilerResults = CodeHelper.CreateActionCode(automation, src);
        }

        public override string Execute(Automation automation)
        {
            object[] obj = new object[] { automation };
            object myclass = _compilerResults.CompiledAssembly.CreateInstance("ConsoleApplication1.Program");

            if (myclass == null)
            {
                throw new Exception("Unable to find function or assembly in Execute.");
            }

            Type t = myclass.GetType();
            MethodInfo mi = t.GetMethod("Execute");
            mi.Invoke(myclass, obj);
            return _nextScene;
        }

    }

}
