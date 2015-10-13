using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DirectorAPI.Interfaces;

namespace DirectorAPI.Conditions
{
    public class ConsoleConnectionRedirectionScreenCondition :ICondition
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
            return ("ConsoleConnectionRedirectScreenCondition");
        }

        public Guid SceneId { get; set; }
        public Guid ConditionId { get; set; }

        public Enumerations.ConditionTypes ConditionType
        {
            get { return Enumerations.ConditionTypes.ConsoleConnectionRedirectionScreenCondition; }
        }

        public List<IAction> GetActions()
        {
            throw new NotImplementedException();
        }

        public IAction AddAction(Enumerations.ActionType type)
        {
            throw new NotImplementedException();
        }


        public Int32 Row { get; set; }
        public Int32 Column { get; set; }
        public string Text { get; set; }
        public CaretLocation CaretLocation { get; set; }


    }
}
