using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using DirectorAPI.Actions.Notifications;
using DirectorAPI.Interfaces;

namespace DirectorAPI.Scenes
{
    public class AlwaysScene : IScene
    {
        private string _name;

        public string Name
        {
            get { return _name; } 

            set { _name = value; }
        }

        public Guid AutomationId { get; set; }
        public Guid SceneId { get; set; }
        public int SortId { get; set; }
        public string CheckPoint { get; set; }
        public string CheckPointFailureStep { get; set; }
        public int Timeout { get; set; }
        public string TimeoutScene { get; set; }
        public string GetNextSceneName { get; set; }
        public SceneEnums.SceneType Type { get; set; }
    }
}
