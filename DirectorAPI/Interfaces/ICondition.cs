using System;
using System.Collections.Generic;

namespace DirectorAPI.Interfaces
{
    public interface ICondition
    {
        void BuildCode();
        void LoadActions();
        string ExecuteActions();
        bool EvaluateCondition();
        string DisplayText();

        Guid SceneId { get; set; }
        Guid ConditionId { get; set; }
        Enumerations.ConditionTypes ConditionType { get; }

        List<IAction> GetActions();

        IAction AddAction(Enumerations.ActionType type);
    }
}
