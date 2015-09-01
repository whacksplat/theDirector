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

    }
}
