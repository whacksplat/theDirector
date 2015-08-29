using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

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
        ConditionEnums.ConditionTypes Type { get; set; }
    }

    public class ConditionEnums
    {
        public enum ConditionTypes
        {
            Always = 0
        }
    }
}
