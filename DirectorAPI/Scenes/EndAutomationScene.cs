using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DirectorAPI.Conditions;
using DirectorAPI.Interfaces;

namespace DirectorAPI.Scenes
{
    public class EndAutomationScene : IScene
    {
        private List<ICondition> conditions = new List<ICondition>();

        public EndAutomationScene()
        {
            Type = Enumerations.SceneTypes.EndAutomation;
        }

        public string Name { get; set; }

        [ReadOnly(true)]
        public Guid AutomationId { get; set; }

        [ReadOnly(true)]
        public Guid SceneId { get; set; }

        [ReadOnly(true)]
        public int SortId { get; set; }
        public string CheckPoint { get; set; }
        public string CheckPointFailureStep { get; set; }
        public int Timeout { get; set; }
        public string TimeoutScene { get; set; }

        [ReadOnly(true)]
        public Enumerations.SceneTypes Type { get; set; }
        
        public ICondition AddCondition(ICondition condition)
        {
            if (!(condition is VariableCondition))
            {
                throw new Exception("Cannot add anything but an DatasourceCondition to an DatasourceScene.");
            }

            condition.ConditionId = Guid.NewGuid();
            condition.SceneId = SceneId;
            condition.ConditionType = condition.ConditionType;
            DBHelper.SaveCondition(this, condition);
            conditions.Add(condition);
            return condition;
        }

        public List<ICondition> GetConditions()
        {
            conditions = DBHelper.GetConditions(this.SceneId);
            return conditions;
        }
    }
}
