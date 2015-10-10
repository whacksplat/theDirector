using System;
using System.Collections.Generic;

namespace DirectorAPI.Interfaces
{
    public interface IScene
    {
        string Name { get; set; }
        Guid AutomationId { get; set; }
        Guid SceneId { get; set; }
        int SortId { get; set; }
        string CheckPoint { get; set; }
        string CheckPointFailureStep { get; set; }
        int Timeout { get; set; }
        string TimeoutScene { get; set; }

        Enumerations.SceneTypes Type { get; set; }
        ICondition AddCondition(ICondition condition);
        List<ICondition> GetConditions();
    }
}
