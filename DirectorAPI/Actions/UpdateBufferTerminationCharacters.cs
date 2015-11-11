using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DirectorAPI.Interfaces;

namespace DirectorAPI.Actions
{
    class UpdateBufferTerminationCharacters:IAction
    {

        CompilerResults _compilerResults;
        string _newBufferTerminationCharacters;

        [TypeConverter(typeof(TypeConverters.SceneNameConverter))]
        public string NextScene { get; set; }

        [ReadOnly(true)]
        public Guid ConditionID { get; set; }

        [ReadOnly(true)]
        public Guid ActionId { get; set; }

        [ReadOnly(true)]
        public Enumerations.ActionType ActionType { get; set; }
        
        public string DisplayText { get { return "UpdateBufferTerminationCharacters"; } }
        public void BuildCode()
        {
            //string src = "AutomationHelper.automation.Connection.Send(" + "\"" + DataToSend + "\"" + ");";
            string src = "AutomationHelper.automation.Connection.BufferTerminationCharacters = " + "\"" + _newBufferTerminationCharacters + "\"" + ");";
            _compilerResults = CodeHelper.CreateActionCode(src);
        }

        public string Execute()
        {
            object[] obj = new object[] { AutomationHelper.automation };
            object myclass = _compilerResults.CompiledAssembly.CreateInstance("ActionCode.Program");

            if (myclass == null)
            {
                throw new Exception("Unable to find function or assembly in Execute.");
            }

            Type t = myclass.GetType();
            MethodInfo mi = t.GetMethod("Execute");
            AutomationHelper.automation.IsEventComplete = false;
            mi.Invoke(myclass, obj);
            AutomationHelper.automation.IsEventComplete = true;
            return NextScene;

        }

        public string NewBufferTerminationCharacters
        {
            get { return _newBufferTerminationCharacters; }
            set
            {
                {
                    _newBufferTerminationCharacters = value;
                    if (AutomationHelper.automation.CurrentMode == Enumerations.Mode.Record)
                    {
                        DBHelper.SaveAction(this);
                    }
                }
            }
        }
    }
}
