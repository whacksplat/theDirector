/*
    theDirector - an open source automation solution
    Copyright (C) 2015 Richard Mageau

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;

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
            var comm = new SqlCommand("UpdateEnterData", DBHelper.Connection())
            {
                CommandType = CommandType.StoredProcedure
            };

            var actionid = new SqlParameter("@actionid", SqlDbType.UniqueIdentifier)
            {
                Direction = ParameterDirection.Input,
                Value = _id
            };

            var data = new SqlParameter("@data", SqlDbType.VarChar, 255)
            {
                Direction = ParameterDirection.Input,
                Value = _data
            };

            var nextscene = new SqlParameter("@nextstep", SqlDbType.VarChar, 255)
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
            var comm = new SqlCommand("Select Data,NextStep from EnterData where ActionID = '" + actionid + "'",
                DBHelper.Connection());
            var reader = comm.ExecuteReader();
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
