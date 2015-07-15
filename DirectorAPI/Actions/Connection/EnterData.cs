using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Runtime.Serialization;

namespace DirectorAPI.Actions.Connection
{
    public class EnterData : AAction
    {
        private string _data;
        private string _nextScene;
        private Automation _automation;
        private Guid _id;

        private CompilerResults _results;

        //TODO should replace this with something agnostic. Key/Enter/Tab

        public string Data
        {
            get { return _data; }
            set
            {
                _data = value;
                Update();
            }
        }

        public void Update()
        {
            SqlCommand comm = new SqlCommand("UpdateEnterData", DBHelper.Connection())
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };

            SqlParameter actionid = new SqlParameter("@actionid", SqlDbType.UniqueIdentifier)
            {
                Direction = System.Data.ParameterDirection.Input,
                Value = _id
            };

            SqlParameter data = new SqlParameter("@data", SqlDbType.VarChar, 255)
            {
                Direction = System.Data.ParameterDirection.Input,
                Value = _data
            };

            SqlParameter nextscene = new SqlParameter("@nextstep", SqlDbType.VarChar, 255)
            {
                Direction = ParameterDirection.Input,
                Value = _nextScene
            };

            comm.Parameters.Add(actionid);
            comm.Parameters.Add(data);
            comm.Parameters.Add(nextscene);

            comm.ExecuteNonQuery();
        }


        public EnterData(Guid id, Automation automation)
        {
            _automation = automation;
            LoadFromDb(id);
        }

        public EnterData()
        {
            throw new NotImplementedException();
        }

        public void LoadFromDb(Guid actionid)
        {
            SqlCommand comm = new SqlCommand("Select Data,NextStep from EnterData where ActionID = '" + actionid + "'",
                DBHelper.Connection());
            SqlDataReader reader = comm.ExecuteReader();
            if (reader.Read())
            {
                if (!reader.IsDBNull(0))
                {
                    _data = reader.GetString(0);
                }
                _id = actionid;
                if (!reader.IsDBNull(1))
                {
                    _nextScene = reader.GetString(1);
                }
            }
            reader.Close();
        }

        public void Refresh()
        {
            LoadFromDb(ActionId);
        }

        [TypeConverter(typeof(TypeConverters.SceneNameConverter)),
        Category("Action Properties"),
        Description("The next Scene to give command to when this action completes successfully.")]
        public override string NextScene { get; set; }
        public override Guid ConditionId { get; set; }
        public override Guid ActionId { get; set; }
        public override Action.ActionType ActionType { get; set; }

        public override void BuildCode(Automation automation)
        {
            throw new NotImplementedException();
        }

        public override string Execute(Automation automation)
        {
            throw new NotImplementedException();
        }
    }
}
