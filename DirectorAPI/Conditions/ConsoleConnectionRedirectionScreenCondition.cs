using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DirectorAPI.Actions.Connection;
using DirectorAPI.Interfaces;

namespace DirectorAPI.Conditions
{
    public class ConsoleConnectionRedirectionScreenCondition :ICondition
    {
        CompilerResults results;
        private List<IAction> _actions = new List<IAction>();

        public void BuildCode()
        {
            string y = "new ScreenCondition(" + Row + "," + Column + "," + "\"" + Text + "\"" + ", new CaretLocation(" + CaretLocation.Row + "," + CaretLocation.Column + ")));";
            results = CodeHelper.CreateConditionCode(null, "return AutomationHelper.automation.Connection.EvalCondition(" + y );
            //ScreenCondition x = new ScreenCondition(Row,Column,Text, new CaretLocation(CaretLocation.Row,CaretLocation.Column));

        }


        public void LoadActions()
        {
            throw new NotImplementedException();
        }

        public string ExecuteActions()
        {
            string retval = "";
            foreach (IAction action in _actions)
            {
                AutomationHelper.automation.IsEventComplete = false;
                retval = action.Execute();
                AutomationHelper.automation.IsEventComplete = true;
                if (!string.IsNullOrEmpty(retval))
                {
                    return retval;
                }
            }
            return retval; //will return blank
        }

        public bool EvaluateCondition()
        {
            var myclass = results.CompiledAssembly.CreateInstance("ConditionCode.Program");
            var t = myclass.GetType();
            var mi = t.GetMethod("TestValue");
            object[] obj = { AutomationHelper.automation };
            var res = mi.Invoke(myclass, obj);
            return (bool)res;

        }

        public string DisplayText()
        {
            return ("Screen Condition (Row:" + Row.ToString() + " Col:" + Column.ToString() + "Text:" + Text);
        }

        [ReadOnly(true)] 
        public Guid SceneId { get; set; }

        [ReadOnly(true)]
        public Guid ConditionId { get; set; }

        public Enumerations.ConditionTypes ConditionType
        {
            get { return Enumerations.ConditionTypes.ConsoleConnectionRedirectionScreenCondition; }
        }

        public List<IAction> GetActions()
        {
            if (_actions.Count == 0)
            {
                _actions = DBHelper.GetActions(ConditionId);    
            }
            return _actions;
        }

        public IAction AddAction(Enumerations.ActionType type)
        {
            IAction retval;

            switch (type)
            {
                case Enumerations.ActionType.SendData:
                    retval = new SendData();
                    break;
                default:
                    throw new NotImplementedException();
            }

            retval.ConditionID = ConditionId;
            retval.ActionId = Guid.NewGuid();
            retval.ActionType = type;

            DBHelper.SaveAction(retval);
            return retval;

        }


        public Int32 Row { get; set; }
        public Int32 Column { get; set; }
        public string Text { get; set; }
        public CaretLocation CaretLocation { get; set; }


    }
}
