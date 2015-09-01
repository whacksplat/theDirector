using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DirectorAPI.Interfaces;

namespace DirectorAPI.Conditions
{
    public class VariableCondition : ICondition
    {
        public void BuildCode()
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public string DisplayText()
        {
            throw new NotImplementedException();
        }


        [ReadOnly(true)]
        public Guid SceneId { get; set; }

        [ReadOnly(true)]
        public Guid ConditionId { get; set; }

        [ReadOnly(true)]
        public Enumerations.ConditionTypes ConditionType { get; set; }

        public List<IAction> GetActions()
        {
            throw new NotImplementedException();
        }

        public IAction AddAction(Enumerations.ActionType type)
        {
            throw new NotImplementedException();
        }
    }
}
