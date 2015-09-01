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
    public class ConnectionScene : IScene
    {
        private List<ICondition> conditions = new List<ICondition>();

        public ConnectionScene()
        {
            Type = Enumerations.SceneTypes.Connection;
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
            //only use Connection based conditions!

            if (!(condition is ConnectionCondition))
            {
                throw new Exception("Cannot add anything but an ConnectionCondition to an ConnectionScene.");
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
