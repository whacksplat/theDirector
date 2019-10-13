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
using System.Xml.Serialization;
using DirectorAPI.Connections;
using DirectorAPI.Interfaces;
using DirectorAPI.Scenes;

namespace DirectorAPI
{
    public class Automation
    {

        private ConsoleConnectionRedirection _connection;// = new Connection();  //there will be only one of these at any point in time.
        //private readonly DataSource _datasource;// = new DataSource();  //same thing with datasource, only 1 per automation
        private readonly Guid _id;
        private readonly List<IScene> _scenes = new List<IScene>();
        private IScene _currentScene;

        public delegate void BufferRefreshed(object sender);
        public event BufferRefreshed ConnectionBufferRefresh;


        public string Name { get; set; }
        public string Description { get; private set; }

        public bool IsEventComplete { get; set; }

        public void ResetEventSyncs()
        {
            Connection.BufferRefresh += Connection_BufferRefresh;
        }

        /// <summary>
        /// This is the AutomationId for this Automation.
        /// </summary>
        public Guid Id
        {
            get { return _id; }
        }

        public Enumerations.Mode CurrentMode { get; set; }

        //public DataSource Datasource
        //{
        //    get { return _datasource; }
        //}

        public ConsoleConnectionRedirection Connection
        {
            get { return _connection; }
            set { _connection = value; }
        }

        public List<IScene> Scenes
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

        private void Connection_BufferRefresh(object sender)
        {
            if (ConnectionBufferRefresh != null)
            {
                ConnectionBufferRefresh(sender);
                if (_currentScene is ConnectionScene)
                {
                    RunScreenConditions();
                    return;
                }
            }
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
            return _scenes.All(scene => scene.GetConditions().Count != 0 || scene.Type == Enumerations.SceneTypes.EndAutomation);
        }


        /// <summary>
        /// Returns true if it finds a scene that is marked end automation
        /// </summary>
        /// <returns></returns>
        private bool CheckForEndScene()
        {
            return _scenes.Any(scene => scene.Type == Enumerations.SceneTypes.EndAutomation);
        }

        

        /// <summary>
        /// Each scene will need to have at least 1 action that will redirect to another scene. If not, it will be stuck in that scene until the automation ends
        /// </summary>
        /// <returns>Returns false if there is a missing redirect.</returns>
        private bool CheckForRedirects()
        {
            foreach (var scene in _scenes)
            {
                if (scene.Type != Enumerations.SceneTypes.EndAutomation)
                {
                    if (!CheckScene(scene))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private bool CheckScene(IScene scene)
        {
            foreach (ICondition condition in scene.GetConditions())
            {
                foreach (IAction action in condition.GetActions())
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
            Name = name;
            Description = description;
            
            Guid? id = DBHelper.AddAutomation(name, description);
            if (!id.HasValue)
            {
                throw new Exception("Unable to create automation");
            }
            _id = (Guid)id;

            AutomationHelper.automation = this;
        }

 
        /// <summary>
        /// The purpose of this constructor
        /// </summary>
        /// <param name="id"></param>
        public Automation(Guid id)
        {
            CurrentMode = Enumerations.Mode.Loading;
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
            CurrentMode = Enumerations.Mode.Record;
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
                switch (reader.GetInt32(4))
                {
                    case 0: //AlwaysScene
                        XmlSerializer AlwaysSerializer = new XmlSerializer(typeof(AlwaysScene));
                        AlwaysScene alwaysScene = (AlwaysScene)AlwaysSerializer.Deserialize(reader.GetXmlReader(3));
                        _scenes.Add(alwaysScene);
                        alwaysScene.GetConditions();
                        break;

                    case 1: //ConnectionCondition
                        XmlSerializer connectionSerializer = new XmlSerializer(typeof(ConnectionScene));
                        ConnectionScene connectionScene = (ConnectionScene)connectionSerializer.Deserialize(reader.GetXmlReader(3));
                        _scenes.Add(connectionScene);
                        connectionScene.GetConditions();
                        break;

                    case 2: //DatasourceCondition
                        XmlSerializer datasourceSerializer = new XmlSerializer(typeof(DataSourceScene));
                        DataSourceScene datasourceScene = (DataSourceScene)datasourceSerializer.Deserialize(reader.GetXmlReader(3));
                        _scenes.Add(datasourceScene);
                        datasourceScene.GetConditions();
                        break;

                    case 3: //VariableCondition
                        XmlSerializer variableSerializer = new XmlSerializer(typeof(VariableScene));
                        VariableScene variableScene = (VariableScene)variableSerializer.Deserialize(reader.GetXmlReader(3));
                        _scenes.Add(variableScene);
                        variableScene.GetConditions();
                        break;

                    case 4: //EndCondition
                        XmlSerializer endautomationSerializer = new XmlSerializer(typeof(EndAutomationScene));
                        EndAutomationScene endautomationScene = (EndAutomationScene)endautomationSerializer.Deserialize(reader.GetXmlReader(3));
                        _scenes.Add(endautomationScene);    //EndAutomationScene will NOT have conditions
                        break;

                    default:
                        throw new NullReferenceException();
                }
            }

            reader.Close();
        }

        public void AddScene(IScene scene)
        {
            scene.AutomationId = Id;
            scene.SceneId = Guid.NewGuid();
            scene.Name = DBHelper.GetNextSceneName(Id);
            scene.SortId = DBHelper.GetNextSortID(Id);
            _scenes.Add(scene);
            DBHelper.SaveScene(scene);
        }

        public void MoveScene(IScene moveFrom, IScene moveTo)
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

                sql = "Update Scenes set SortId = " + moveTo.SortId + " where SceneId = '" + moveFrom.SceneId + "'";
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


        public void DeleteScene(IScene scene)
        {
            throw new NotImplementedException();
        }
 
        public void BuildAssemblies()
        {
            foreach (IScene scene in _scenes)
            {
                foreach (ICondition condition in scene.GetConditions())
                {
                    condition.BuildCode();
                    foreach (IAction action in condition.GetActions())
                    {
                        action.BuildCode();
                    }
                }
            }
        }

        public void RunAutomation()
        {
            CurrentMode = Enumerations.Mode.Run;

            if (!PreCompile())
            {
                return;
            }

            _currentScene = null;
            BuildAssemblies();

            //reset the data source and connection objects in the automation
            _currentScene = Scenes[0];
            

            //todo reset CurrentMode when scene is end scene
            RunScenes();
        }

        private void RunScenes()
        {
            string retval;

            while (_currentScene != null)
            {
                if (_currentScene is EndAutomationScene)
                {
                    //we're done
                    CurrentMode = Enumerations.Mode.Record;
                    break;
                }

                if (_currentScene is ConnectionScene)
                {
                    //release control and let the buffer take care of it
                    return;
                }

                foreach (ICondition condition in _currentScene.GetConditions())
                {

                    if (condition.EvaluateCondition())
                    {
                        retval = condition.ExecuteActions();
                        //ResetEventSyncs();
                        if (!string.IsNullOrEmpty(retval))
                        {
                            _currentScene = Scenes.Find(x => x.Name == retval);
                        }
                    }
                }
            }
        }

        public void RunScreenConditions()
        {
            foreach (ICondition condition in _currentScene.GetConditions())
            {
                if (condition.EvaluateCondition())
                {
                    //string retval="";

                    foreach (IAction action in condition.GetActions())  //todo check to see if this is doing another database call
                    {
                        if (!string.IsNullOrEmpty(action.NextScene))
                        {
                            _currentScene = Scenes.Find(x => x.Name == action.NextScene);
                        }
                    }
                    condition.ExecuteActions();     //release immediatley
                    return;
                }
            }
        }



    }
}