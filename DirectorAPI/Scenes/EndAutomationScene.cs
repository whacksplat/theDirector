using System;
using System.Collections.Generic;
using System.ComponentModel;
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
            throw new Exception("You cannot add a condition to an EndAutomation Scene.");
        }

        public List<ICondition> GetConditions()
        {
            return conditions;
        }
    }
}
