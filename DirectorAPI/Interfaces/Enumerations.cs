using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectorAPI.Interfaces
{
    public class Enumerations
    {
        public enum ConditionTypes
        {
            AlwaysCondition = 0,
            ConnectionCondition,
            DatasourceCondition,
            VariableCondition,
            EndCondition
        }

        public enum ActionType
        {
            MessageBox = 0,
            OpenDatasource,
            NextRecord,
            EnterData,
            ConnectToCmd
        }

        public enum SceneTypes
        {
            Always = 0,
            Connection,
            Datasource,
            Variable,
            EndAutomation
        }


    }
}
