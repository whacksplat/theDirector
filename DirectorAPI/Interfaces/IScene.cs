using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        //string GetNextSceneName { get; set; }
        SceneEnums.SceneType Type { get; set; }
        ICondition AddCondition(ICondition condition);
        List<ICondition> GetConditions();
    }

    public class SceneEnums
    {
        public enum SceneType
        {
            Always = 0,
            Connection,
            Datasource,
            Variable,
            EndAutomation
        }

    }
}
