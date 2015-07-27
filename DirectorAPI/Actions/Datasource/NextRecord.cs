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

namespace DirectorAPI.Actions.Datasource
{
    public class NextRecord:AAction
    {
        private Automation _automation;

        private string _nextScene;
        private Guid _id;

        CompilerResults _results;

        public void Update()
        {
            var comm = new SqlCommand("UpdateNextRecord", DBHelper.Connection())
            {
                CommandType = CommandType.StoredProcedure
            };

            var actionid = new SqlParameter("@actionid", SqlDbType.UniqueIdentifier)
            {
                Direction = ParameterDirection.Input,
                Value = _id
            };

            var nextscene = new SqlParameter("@nextstep", SqlDbType.VarChar, 255)
            {
                Direction = ParameterDirection.Input,
                Value = _nextScene
            };

            comm.Parameters.Add(actionid);
            comm.Parameters.Add(nextscene);

            comm.ExecuteNonQuery();
        }

        public void LoadFromDb(Guid actionId)
        {
            var comm = new SqlCommand("Select NextStep from NextRecord where ActionID = '" + actionId + "'", DBHelper.Connection());
            var reader = comm.ExecuteReader();
            if (reader.Read())
            {
                _id = ActionId;
                if (!reader.IsDBNull(0))
                {
                    _nextScene = reader.GetString(0);
                }
            }
            reader.Close();
        }

        public NextRecord(Guid id,Automation automation)
        {
            _automation = automation;
            LoadFromDb(id);
        }

        public NextRecord()
        {
            throw new NotImplementedException();
        }

        public void Refresh()
        {
            throw new NotImplementedException();
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
