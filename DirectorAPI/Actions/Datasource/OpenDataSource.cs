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
    public class OpenDataSource : AAction
    {
        private string _filename;
        private string _sheetname;

        private readonly Automation _automation;

        private string _nextScene;
        private Guid _id;

        CompilerResults _results;

        public string FileName 
        { 
            get
            {
                return _filename;
            }
            set
            {
                _filename = value;
                Update();
            }
        }

        public string SheetName 
        {
            get
            {
                return _sheetname;
            }
            set
            {
                if(string.IsNullOrEmpty(value))
                {
                    _sheetname = value;
                    return;
                }
                _sheetname = value;
                _automation.Datasource.Open(_filename, _sheetname);
                Update();
            }
        }


        public OpenDataSource(Guid id,Automation automation)
        {
            _automation = automation;
            LoadFromDb(id);
        }

        public OpenDataSource()
        {
            throw new NotImplementedException();
        }

        public void LoadFromDb(Guid actionid)
        {
            var comm = new SqlCommand("Select FileName,Sheetname,NextStep from DataSource where ActionID = '" + actionid + "'", DBHelper.Connection());
            var reader = comm.ExecuteReader();
            if (reader.Read())
            {
                _filename = reader.GetString(0);
                _sheetname = reader.GetString(1);
                _id = actionid;
                if (!reader.IsDBNull(2))
                {
                    _nextScene = reader.GetString(2);
                }
            }
            reader.Close();
        }

        public void Refresh()
        {
            throw new NotImplementedException();

        }

        public void Update()
        {
            var comm = new SqlCommand("UpdateOpenDataSource", DBHelper.Connection())
            {
                CommandType = CommandType.StoredProcedure
            };

            var actionid = new SqlParameter("@actionid", SqlDbType.UniqueIdentifier)
            {
                Direction = ParameterDirection.Input,
                Value = _id
            };

            var filename = new SqlParameter("@filename", SqlDbType.VarChar, 255)
            {
                Direction = ParameterDirection.Input,
                Value = FileName
            };

            var sheetname = new SqlParameter("@sheetname", SqlDbType.VarChar, 255)
            {
                Direction = ParameterDirection.Input,
                Value = SheetName
            };

            var nextstep = new SqlParameter("@nextstep", SqlDbType.VarChar, 255)
            {
                Direction = ParameterDirection.Input,
                Value = _nextScene
            };

            comm.Parameters.Add(actionid);
            comm.Parameters.Add(filename);
            comm.Parameters.Add(sheetname);
            comm.Parameters.Add(nextstep);

            comm.ExecuteNonQuery();
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
