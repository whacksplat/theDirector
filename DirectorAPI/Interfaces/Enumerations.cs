
namespace DirectorAPI.Interfaces
{
    public class Enumerations
    {
        public enum ConditionTypes
        {
            AlwaysCondition = 0,
            ConsoleConnectionRedirectionScreenCondition,
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
            SendData,
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
