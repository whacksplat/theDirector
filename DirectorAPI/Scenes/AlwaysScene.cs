using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using DirectorAPI.Actions.Notifications;
using DirectorAPI.Conditions;
using DirectorAPI.Interfaces;

namespace DirectorAPI.Scenes
{
    public class AlwaysScene : IScene
    {
        private List<ICondition> conditions = new List<ICondition>();

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
        public SceneEnums.SceneType Type { get; set; }
        
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
            condition.Type = condition.Type;
            DBHelper.SaveCondition(this,condition);
            conditions.Add(condition);
            return condition;
        }

        public List<ICondition> GetConditions()
        {
            conditions =  DBHelper.GetConditions(this.SceneId);
            return conditions;
        }
    }
}
