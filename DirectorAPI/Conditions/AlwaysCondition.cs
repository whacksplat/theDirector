using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
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
        }

        public void LoadActions()
        {
            throw new NotImplementedException();
        }

        public string ExecuteActions()
        {
            throw new NotImplementedException();
        }

        public void EvaluateCondition()
        {
            throw new NotImplementedException();
        }

        public string DisplayText()
        {
            throw new NotImplementedException();
        }

        public List<IAction> Actions
        {
            get { return _actions; }
        }

        public Guid SceneId { get; set; }
    }
}
