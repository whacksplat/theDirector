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
        void EvaluateCondition();
        string DisplayText();
        List<IAction> Actions { get;}
        Guid SceneId { get; set; }

    }
}
