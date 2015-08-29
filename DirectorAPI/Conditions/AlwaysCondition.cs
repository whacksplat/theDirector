using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DirectorAPI.Interfaces;

namespace DirectorAPI.Conditions
{
    public class AlwaysCondition:ICondition
    {
        CompilerResults results;
        private List<IAction> _actions = new List<IAction>();

        public void BuildCode()
        {
            results = CodeHelper.CreateConditionCode(null,"return true;");
            //todo need to build the code for actions
        }

        public void LoadActions()
        {
            throw new NotImplementedException();
        }

        public string ExecuteActions()
        {
            throw new NotImplementedException();
        }

        public bool EvaluateCondition()
        {
            var myclass = results.CompiledAssembly.CreateInstance("ConsoleApplication1.Program");
            var t = myclass.GetType();
            var mi = t.GetMethod("TestValue");
            object[] obj = { AutomationHelper.automation };
            var res = mi.Invoke(myclass, obj);
            return (bool)res;
        }

        public string DisplayText()
        {
            return "Always";
        }


        [ReadOnly(true)]
        public Guid SceneId { get; set; }

        [ReadOnly(true)]
        public Guid ConditionId { get; set; }
        
        [ReadOnly(true)]
        public ConditionEnums.ConditionTypes Type { get; set; }
    }
}
