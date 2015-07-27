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
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using DirectorAPI.Actions;

namespace DirectorAPI
{
    public class Automation
    {
        public enum Mode
        {
            Record = 0,
            Run
        }

        private readonly Connection _connection = new Connection();  //there will be only one of these at any point in time.
        private readonly DataSource _datasource = new DataSource();  //same thing with datasource, only 1 per automation
        private readonly Guid _id;
        private readonly List<Scene> _scenes = new List<Scene>();

        public string Name { get; set; }
        public string Description { get; private set; }

        public Guid Id
        {
            get { return _id; }
        }

        public Mode CurrentMode { get; set; }

        public DataSource Datasource
        {
            get { return _datasource; }
        }

        public Connection Connection
        {
            get { return _connection; }
        }

        public List<Scene> Scenes
        {
            get { return _scenes; }
        }

        
        public Automation(string name, string description, Guid id)
        {
            Name = name;
            Description = description;
            _id = id;
            AutomationHelper.automation = this;
        }


        /// <summary>
        /// Runs the rules to check to see if we meet all requirements.
        /// </summary>
        /// <returns>Returns true if we meet all the requirements and can run the automation</returns>
        public bool PreCompile()
        {
            //todo make sure at least one action in a scene redirects to another scene, or we will get stuck in a scene
            //todo make sure we have a End Automation scene
            //todo scene must have at least 1 condition

            if (!CheckForEndScene())
            {
                MessageBox.Show(
                    "Each Automation must have a scene designated as the End Scene so the Automation will know when it's done.");
                return false;
            }
            if (!CheckForConditions())
            {
                MessageBox.Show("Each Scene must have at least one condition.");
                return false;
            }

            if (!CheckForRedirects())
            {
                MessageBox.Show("A Scene must have at least one action that redirects to another scene.");
                return false;
            }

            return true;
        }


        /// <summary>
        /// Checks to make sure each scene has at least 1 condition
        /// </summary>
        /// <returns></returns>
        private bool CheckForConditions()
        {
            return _scenes.All(scene => scene.Conditions.Count != 0 || scene.Type == Scene.SceneType.EndAutomation);
        }


        /// <summary>
        /// Returns true if it finds a scene that is marked end automation
        /// </summary>
        /// <returns></returns>
        private bool CheckForEndScene()
        {
            return _scenes.Any(scene => scene.Type == Scene.SceneType.EndAutomation);
        }

        

        /// <summary>
        /// Each scene will need to have at least 1 action that will redirect to another scene. If not, it will be stuck in that scene until the automation ends
        /// </summary>
        /// <returns>Returns false if there is a missing redirect.</returns>
        private bool CheckForRedirects()
        {
            foreach (var scene in _scenes)
            {
                if (scene.Type != Scene.SceneType.EndAutomation)
                {
                    if (!CheckScene(scene))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private bool CheckScene(Scene scene)
        {
            foreach (var condition in scene.Conditions)
            {
                foreach (AAction action in condition.Actions)
                {
                    if (!string.IsNullOrEmpty(action.NextScene))
                    {
                        //we're good for this scene
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        ///     This constructor's purpose is to create an instance of an Automation
        ///     and add it to the local database
        /// </summary>
        /// <param name="name">The name of the Automation.</param>
        /// <param name="description">A brief description of the Automation.</param>
        public Automation(string name, string description)
        {
            //_connection.BufferRefresh += _connection_BufferRefresh;
            Name = name;
            Description = description;
            //_id = new Guid();


            var autname = new SqlParameter("@autname", SqlDbType.VarChar)
            {
                Size = 50,
                Direction = ParameterDirection.Input,
                Value = Name
            };

            var descr = new SqlParameter("@desc", SqlDbType.VarChar)
            {
                Size = 255,
                Direction = ParameterDirection.Input,
                Value = Description
            };

            var comm = new SqlCommand("AddAutomation") {CommandType = CommandType.StoredProcedure};
            comm.Parameters.Add(autname);
            comm.Parameters.Add(descr);
            comm.Connection = DBHelper.Connection();

            var reader = comm.ExecuteReader();
            if (reader.Read())
            {
                Console.WriteLine(reader.GetSqlGuid(0).ToString());
                _id = reader.GetSqlGuid(0).Value;
            }
            reader.Close();
            AutomationHelper.automation = this;
        }

 
        /// <summary>
        /// The purpose of this constructor
        /// </summary>
        /// <param name="id"></param>
        public Automation(Guid id)
        {
            var comm = new SqlCommand("GetAutomationByGUID") {CommandType = CommandType.StoredProcedure};

            var paramid = new SqlParameter("@automationid", SqlDbType.UniqueIdentifier)
            {
                Direction = ParameterDirection.Input,
                Value = id
            };

            comm.Parameters.Add(paramid);
            comm.Connection = DBHelper.Connection();

            var reader = comm.ExecuteReader();
            if (reader.Read())
            {
                Name = reader.GetString(1);
                Description = reader.GetString(2);
                _id = reader.GetGuid(0);
            }
            reader.Close();
            AutomationHelper.automation = this;
            GetScenes();
        }

 private void GetScenes()
        {
            var comm = new SqlCommand("GetScenes") { CommandType = CommandType.StoredProcedure };

            var autoid = new SqlParameter("@automationid", SqlDbType.UniqueIdentifier)
            {
                Direction = ParameterDirection.Input,
                Value = _id
            };

            comm.Parameters.Add(autoid);
            comm.Connection = DBHelper.Connection();

            var reader = comm.ExecuteReader();
            while (reader.Read())
            {
                //SELECT AutomationID, SortID, Name, SceneType, TimeOutInterval, TimeOutStep from Scenes where AutomationID = @automationid order by SortID
                if (reader.IsDBNull(5))
                {
                    _scenes.Add(new Scene(reader.GetString(2), //name
                        _id, //automationid
                        reader.GetGuid(0), //sceneid
                        reader.GetInt32(1), //sortid
                        reader.GetInt32(4), //timeoutinterval
                        null, //timeoutstep
                        reader.GetInt32(3),
                        this)); //type
                }
                else
                {
                    _scenes.Add(new Scene(reader.GetString(2), //name
                        _id, //automationid
                        reader.GetGuid(0), //id
                        reader.GetInt32(1), //sortid
                        reader.GetInt32(4), //timeoutinterval
                        reader.GetString(5), //timeoutstep
                        reader.GetInt32(3),
                        this)); //type
                }
            }

            reader.Close();

            foreach (var scene in _scenes)
            {
                scene.GetConditions();
            }
        }

        public Scene AddScene()
        {
            var scene = new Scene(_id, this);
            _scenes.Add(scene);
            return scene;
        }

        public void MoveScene(Scene moveFrom, Scene moveTo)
        {
            if (moveFrom.SortId < moveTo.SortId)
            {
                string sql;
                sql = "UPDATE Scenes set SortId = SortId - 1 " +
                      "where SortId > " + moveFrom.SortId + " and SortId <= " + moveTo.SortId + " and AutomationId = '" + _id + "'";
                var comm = new SqlCommand(sql);
                comm.CommandType = CommandType.Text;
                comm.Connection = DBHelper.Connection();
                comm.ExecuteNonQuery();

                sql = "Update Scenes set SortId = " + moveTo.SortId + " where ID = '" + moveFrom.SceneId + "'";
                comm.CommandText = sql;
                comm.ExecuteNonQuery();
            }
            else
            {
                string sql;
                sql = "UPDATE Scenes set SortId = SortId + 1 " +
                      "where SortId < " + moveFrom.SortId + " and SortId >= " + moveTo.SortId + " and AutomationId = '" + _id + "'";
                var comm = new SqlCommand(sql);
                comm.CommandType = CommandType.Text;
                comm.Connection = DBHelper.Connection();
                comm.ExecuteNonQuery();

                sql = "Update Scenes set SortId = " + moveTo.SortId + " where SceneID = '" + moveFrom.SceneId + "'";
                comm.CommandText = sql;
                comm.ExecuteNonQuery();
            }
            
            _scenes.Clear();
            GetScenes();
        }


        public void DeleteScene(Scene scene)
        {
            
        }
 
        public void BuildAssemblies()
        {
            foreach (var scene in _scenes)
            {
                foreach (var condition in scene.Conditions)
                {
                    condition.BuildCode(this);
                }
            }
        }
    }
}