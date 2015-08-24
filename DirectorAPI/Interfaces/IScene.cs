using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectorAPI.Interfaces
{
    public interface IScene
    {
        //void Save();
        //void AddCondition();
        //void AddScreenCondition();

        string Name { get; set; }
        Guid AutomationId { get; set; }
        Guid SceneId { get; set; }
        int SortId { get; set; }
        string CheckPoint { get; set; }
        string CheckPointFailureStep { get; set; }
        int Timeout { get; set; }
        string TimeoutScene { get; set; }
        string GetNextSceneName { get; set; }
        SceneEnums.SceneType Type { get; set; }
        //public List<Condition> Conditions = new List<Condition>();
        //List<ICondition> Conditions { get; set; } 

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
