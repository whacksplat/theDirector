using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DirectorAPI.Interfaces;

namespace DirectorAPI.Scenes
{
    public class DataSourceScene : IScene
    {
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
        }

        public List<ICondition> GetConditions()
        {
            throw new NotImplementedException();
        }
    }
}
