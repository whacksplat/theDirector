using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using DirectorAPI.Actions.Connection;
using DirectorAPI.Actions.Notifications;
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
            string retval="";
            foreach (IAction action in _actions)
            {
                retval = action.Execute();
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
            return "Always";
        }


        [ReadOnly(true)]
        public Guid SceneId { get; set; }

        [ReadOnly(true)]
        public Guid ConditionId { get; set; }
        
        [ReadOnly(true)]
        public Enumerations.ConditionTypes ConditionType { get; set; }

        public List<IAction> GetActions()
        {
            _actions=DBHelper.GetActions(ConditionId);
            return _actions;
        }

        public IAction AddAction(Enumerations.ActionType type)
        {
            IAction retval = null;

            switch (type)
            {
                case Enumerations.ActionType.ConnectToCmd:
                    retval = new ConnectToCmd();
                    retval.BuildCode();
                    retval.Execute();
                    break;

                case Enumerations.ActionType.EnterData:
                    break;

                case Enumerations.ActionType.MessageBox:
                    retval = new MessageBox();
                    break;

                case Enumerations.ActionType.NextRecord:
                    break;

                case Enumerations.ActionType.OpenDatasource:
                    break;

                default:
                    throw new NotImplementedException("unable to addaction");
            }

            retval.ConditionID = ConditionId;
            retval.ActionId = Guid.NewGuid();
            retval.ActionType = type;

            DBHelper.SaveAction(retval);
            return retval;
        }
    }
}
