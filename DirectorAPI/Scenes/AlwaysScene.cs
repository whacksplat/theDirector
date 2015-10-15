using System;
using System.Collections.Generic;
using System.ComponentModel;
using DirectorAPI.Conditions;
using DirectorAPI.Interfaces;

namespace DirectorAPI.Scenes
{
    public class AlwaysScene : IScene
    {
        private List<ICondition> conditions;

        public string Name{ get; set; }
        
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
            //Always scene will only have singular Always condition.
            if (conditions.Count > 0)
            {
                throw new Exception("Cannot have more than one condition in an Always scene.");
            }
            if (!(condition is AlwaysCondition))
            {
                throw new Exception("Cannot add anything but an AlwaysCondition to an Always scene.");
            }
            
            condition.ConditionId = Guid.NewGuid();
            condition.SceneId = SceneId;
            //condition.ConditionType = condition.ConditionType;
            DBHelper.SaveCondition(this,condition);
            conditions.Add(condition);
            return condition;
        }

        public List<ICondition> GetConditions()
        {
            if (conditions == null)
            {
                //fresh load from db
                conditions = new List<ICondition>();
                conditions = DBHelper.GetConditions(this.SceneId);
            }
            return conditions;
        }
    }
}
