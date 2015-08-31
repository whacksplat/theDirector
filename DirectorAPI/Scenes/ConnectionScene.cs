using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DirectorAPI.Interfaces;

namespace DirectorAPI.Scenes
{
    public class ConnectionScene : IScene
    {
        private List<ICondition> conditions = new List<ICondition>();

        public string Name { get; set; }
        public Guid AutomationId { get; set; }
        public Guid SceneId { get; set; }
        public int SortId { get; set; }
        public string CheckPoint { get; set; }
        public string CheckPointFailureStep { get; set; }
        public int Timeout { get; set; }
        public string TimeoutScene { get; set; }
        public SceneEnums.SceneType Type { get; set; }
        public ICondition AddCondition(ICondition condition)
        {
            throw new NotImplementedException();

            //only use Connection based conditions!
            condition.ConditionId = Guid.NewGuid();
            condition.SceneId = SceneId;
            condition.Type = condition.Type;
            DBHelper.SaveCondition(this, condition);
            conditions.Add(condition);
            return condition;
        }

        public List<ICondition> GetConditions()
        {
            throw new NotImplementedException();
        }
    }
}
