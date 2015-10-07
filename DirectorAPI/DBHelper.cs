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
using System.Globalization;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using DirectorAPI.Actions;
using DirectorAPI.Actions.Connection;
using DirectorAPI.Actions.Notifications;
using DirectorAPI.Conditions;
using DirectorAPI.Interfaces;
using DirectorAPI.Scenes;

namespace DirectorAPI
{
    public static class DBHelper
    {
        private static SqlConnection _conn;

        public static void SaveScene(IScene scene)
        {

            var comm = new SqlCommand();
            comm.Connection = DBHelper.Connection();
            comm.CommandType = CommandType.StoredProcedure;

            //If SceneId doesn't exist, it's a new scene
            if (!SceneExists(scene.SceneId))
            {
                comm.CommandText = "AddScene";
                comm.Parameters.Add(new SqlParameter("@automationid", SqlDbType.UniqueIdentifier)
                {
                    Value = scene.AutomationId
                });

                comm.Parameters.Add(new SqlParameter("@sceneid", SqlDbType.UniqueIdentifier)
                {
                    Value = scene.SceneId
                });

                comm.Parameters.Add(new SqlParameter("@name", SqlDbType.VarChar, 50)
                {
                    Value = scene.Name
                });

                comm.Parameters.Add(new SqlParameter("@sortid", SqlDbType.Int)
                {
                    Value = scene.SortId
                });

                comm.Parameters.Add(new SqlParameter("@objectxml", SqlDbType.Xml)
                {
                    Value = GetSceneObjectXML(scene)
                });

                comm.Parameters.Add(new SqlParameter("@scenetype", SqlDbType.Int)
                {
                    Value = scene.Type
                });
                comm.ExecuteNonQuery();
            }
            else
            {
                comm.CommandText = "UpdateScene";
                comm.Parameters.Add(new SqlParameter("@sceneid", SqlDbType.UniqueIdentifier)
                {
                    Value = scene.SceneId
                });

                comm.Parameters.Add(new SqlParameter("@name", SqlDbType.VarChar, 50)
                {
                    Value = scene.Name
                });

                comm.Parameters.Add(new SqlParameter("@sortid", SqlDbType.Int)
                {
                    Value = scene.SortId
                });

                comm.Parameters.Add(new SqlParameter("@objectxml", SqlDbType.Xml)
                {
                    Value = GetSceneObjectXML(scene)
                });

                comm.Parameters.Add(new SqlParameter("@scenetype", SqlDbType.Int)
                {
                    Value = scene.Type
                });
                comm.ExecuteNonQuery();
            }
        }
        private static bool SceneExists(Guid sceneGuid)
        {
            var comm = new SqlCommand("select count(SceneId) from Scenes where SceneID = '" + sceneGuid + "'");
            comm.Connection = DBHelper.Connection();
            comm.CommandType = CommandType.Text;
            var reccount = comm.ExecuteReader();

            if (reccount.Read())
            {
                if (reccount.GetInt32(0) == 0)
                {
                    //it's a new Scene
                    return false;
                }
                else
                {
                    return true;
                }
            }
            throw new NullReferenceException("Unable to parse SQL in SceneExists.");
        }
        public static string GetNextSceneName(Guid automationid)
        {
            var count = 0;
            var found = false;
            SqlDataReader reader;
            var comm = new SqlCommand();
            comm.CommandType = CommandType.Text;

            while (!found)
            {
                count += 1;
                comm.CommandText = "select count(SceneID) from Scenes where Name = 'NewScene" + Convert.ToString(count) +
                                   "' and AutomationID = '" + automationid + "'";
                comm.Connection = DBHelper.Connection();
                reader = comm.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    if (reader.IsDBNull(0))
                    {
                        //signals that no match was found, use this name
                        found = true;
                    }

                    if (reader.GetInt32(0) == 0)
                    {
                        found = true;
                    }
                }
                reader.Close();
            }
            return "NewScene" + Convert.ToString(count);
        }
        private static string GetSceneObjectXML(IScene scene)
        {

            XmlSerializer serializer;
            var writer = new StringWriter(CultureInfo.InvariantCulture);
            switch (scene.Type)
            {
                case Enumerations.SceneTypes.Always:
                    serializer = new XmlSerializer(typeof (AlwaysScene));
                    break;
                case Enumerations.SceneTypes.Connection:
                    serializer = new XmlSerializer(typeof(ConnectionScene));
                    break;
                case Enumerations.SceneTypes.Datasource:
                    serializer = new XmlSerializer(typeof(DataSourceScene));
                    break;
                case Enumerations.SceneTypes.EndAutomation:
                    serializer = new XmlSerializer(typeof(EndAutomationScene));
                    break;
                case Enumerations.SceneTypes.Variable:
                    serializer = new XmlSerializer(typeof(VariableScene));
                    break;

                default:
                    throw new NotImplementedException("Unknown scene type.");
                    break;

            }

            serializer.Serialize(writer, scene);
            return writer.ToString();
        }

        
        private static string GetConditionObjectXML(ICondition condition)
        {
            XmlSerializer serializer;
            var writer = new StringWriter(CultureInfo.InvariantCulture);

            switch (condition.ConditionType)
            {
                case Enumerations.ConditionTypes.AlwaysCondition:
                    serializer = new XmlSerializer(typeof (AlwaysCondition));
                    break;
                default:
                    throw new Exception("Unknown condition type.");
                    break;
            }

            serializer.Serialize(writer, condition);
            return writer.ToString();
        }
        public static int GetNextSortID(Guid automationGuid)
        {
            int result;
            var comm = new SqlCommand("select MAX(SortID) from Scenes where AutomationID = '" + automationGuid + "'");
            comm.Connection = DBHelper.Connection();
            comm.CommandType = CommandType.Text;
            var reader = comm.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();
                if (reader.IsDBNull(0))
                {
                    reader.Close();
                    return 0;
                }
                result = reader.GetInt32(0) + 1;
                reader.Close();
                return result;
            }
            reader.Close();
            return 0;
        }
        public static bool IsAutomationNameUnique(string name)
        {
            var sql = "select count(Name) from Automations where Name = '" + name + "';";
            var comm = new SqlCommand(sql);
            comm.Connection = Connection();
            comm.CommandType = CommandType.Text;
            var reader = comm.ExecuteReader();
            reader.Read();
            if (reader.GetInt32(0) == 0)
            {
                reader.Close();
                return true;
            }
            reader.Close();
            return false;
        }



        public static void SaveCondition(IScene scene, ICondition condition)
        {
            var comm = new SqlCommand();
            comm.Connection = DBHelper.Connection();
            comm.CommandType = CommandType.StoredProcedure;

            if (!ConditionExists(condition.ConditionId))
            {
                //Add
                comm.CommandText = "AddCondition";

                comm.Parameters.Add(new SqlParameter("@sceneid", SqlDbType.UniqueIdentifier)
                {
                    Value = condition.SceneId
                });

                comm.Parameters.Add(new SqlParameter("@conditionid", SqlDbType.UniqueIdentifier)
                {
                    Value = condition.ConditionId
                });

                comm.Parameters.Add(new SqlParameter("@objectxml", SqlDbType.Xml)
                {
                    Value = GetConditionObjectXML(condition)
                });

                comm.Parameters.Add(new SqlParameter("@conditiontype", SqlDbType.Int)
                {
                    Value = condition.ConditionType
                });

                comm.ExecuteNonQuery();
            }
            else
            {
                //update
                comm.CommandText = "UpdateCondition";

                comm.Parameters.Add(new SqlParameter("@sceneid", SqlDbType.UniqueIdentifier)
                {
                    Value = condition.SceneId
                });

                comm.Parameters.Add(new SqlParameter("@conditionid", SqlDbType.UniqueIdentifier)
                {
                    Value = condition.ConditionId
                });

                comm.Parameters.Add(new SqlParameter("@objectxml", SqlDbType.Xml)
                {
                    Value = GetSceneObjectXML(scene)
                });

                comm.Parameters.Add(new SqlParameter("@conditiontype", SqlDbType.Int)
                {
                    Value = condition.ConditionType
                });

                comm.ExecuteNonQuery();
            }
        }
        private static bool ConditionExists(Guid conditionGuid)
        {
            var comm =
                new SqlCommand("select count(ConditionId) from Conditions where ConditionID = '" + conditionGuid + "'");
            comm.Connection = DBHelper.Connection();
            comm.CommandType = CommandType.Text;
            var reccount = comm.ExecuteReader();

            if (reccount.Read())
            {
                if (reccount.GetInt32(0) == 0)
                {
                    //it's a new Condition
                    return false;
                }
                else
                {
                    return true;
                }
            }
            throw new NullReferenceException("Unable to parse SQL in SceneExists.");

        }
        public static List<ICondition> GetConditions(Guid sceneId)
        {
            List<ICondition> conditions = new List<ICondition>();
            SqlCommand comm = new SqlCommand()
            {
                CommandText = "GetConditions",
                Connection = DBHelper.Connection(),
                CommandType = CommandType.StoredProcedure
            };

            comm.Parameters.Add(new SqlParameter() { DbType = DbType.Guid, Value = sceneId, ParameterName = "@sceneid" });
            SqlDataReader reader = comm.ExecuteReader();

            if (reader.HasRows)
            {
                if (reader.Read())
                {
                    ICondition condition = CreateConditionFromDb(sceneId,
                        reader.GetGuid(0),
                        reader.GetXmlReader(1),
                        reader.GetInt32(2));

                    condition.GetActions();

                    conditions.Add(condition);

                }
            }

            reader.Close();
            return conditions;
        }
        private static ICondition CreateConditionFromDb(Guid sceneId, Guid conditionId, XmlReader objectXml, Int32 conditionType)
        {
            switch (conditionType)
            {
                case 0:
                    //return new AlwaysCondition(){ConditionId = conditionId,SceneId = sceneId,ConditionType = Enumerations.ConditionTypes.AlwaysCondition};
                    XmlSerializer AlwaysSerializer = new XmlSerializer(typeof(AlwaysCondition));
                    AlwaysCondition alwaysCondition = (AlwaysCondition)AlwaysSerializer.Deserialize(objectXml);
                    return alwaysCondition;

                default:
                    throw new Exception("unknown condition type in CreateConditionFromDb");
            }

        }


        public static SqlConnection Connection()
        {
            if (_conn == null)
            {
                _conn =
                    new SqlConnection(
                        "Data Source=LTPRMAGEAU81\\MSSQLSERVER2014;Initial Catalog=Director;Integrated Security=SSPI;MultipleActiveResultSets=true;");
                _conn.Open();
                return _conn;
            }

            if (_conn.State == ConnectionState.Open)
            {
                return _conn;
            }

            if (_conn.State != ConnectionState.Open)
            {
                _conn.Close();
                _conn.ConnectionString =
                    "Data Source=(local);Initial Catalog=Director;Integrated Security=SSPI;MultipleActiveResultSets=true;";
                _conn.Open();
            }
            return _conn;
        }

        public static void SaveAction(IAction action)
        {
            var comm = new SqlCommand();
            comm.Connection = DBHelper.Connection();
            comm.CommandType = CommandType.StoredProcedure;
            if (!ActionExists(action.ActionId))
            {
                //need to add
                comm.CommandText = "AddAction";

    //            INSERT INTO Actions(ConditionID,ActionID,ActionType,ObjectXML) OUTPUT inserted.ActionID
    //VALUES (@conditionid,@actionid,@actiontype,@objectxml)
                comm.Parameters.Add(new SqlParameter("@conditionid", SqlDbType.UniqueIdentifier)
                {
                    Value = action.ConditionID
                });

                comm.Parameters.Add(new SqlParameter("@actionid", SqlDbType.UniqueIdentifier)
                {
                    Value = action.ActionId
                });

                comm.Parameters.Add(new SqlParameter("@actiontype", SqlDbType.Int)
                {
                    Value = action.ActionType
                });

                comm.Parameters.Add(new SqlParameter("@objectxml", SqlDbType.Xml)
                {
                    Value = GetActionObjectXml(action)
                });


                comm.ExecuteNonQuery();

            }
            else
            {
                //update
            }
        }
        public static void UpdateAction(IAction action)
        {
            var _action = action as IAction;
            var writer = new StringWriter(CultureInfo.InvariantCulture);

            //create the xml
            switch (action.ActionType)
            {
                case Enumerations.ActionType.MessageBox:
                    var serializer = new XmlSerializer(typeof(MessageBox));
                    var messagebox = (MessageBox)action;
                    serializer.Serialize(writer, messagebox);
                    break;

                case Enumerations.ActionType.ConnectToCmd:
                    var ctcSerializer = new XmlSerializer(typeof(ConnectToCmd));
                    var ctc = (ConnectToCmd)action;
                    ctcSerializer.Serialize(writer, ctc);
                    break;

                default:
                    break;
            }

            var comm = new SqlCommand("UpdateAction");
            comm.Connection = Connection();
            comm.CommandType = CommandType.StoredProcedure;

            var conditionID = new SqlParameter("@conditionid", SqlDbType.UniqueIdentifier);
            conditionID.Direction = ParameterDirection.Input;
            conditionID.Value = _action.ConditionID;

            var actionID = new SqlParameter("@actionid", SqlDbType.UniqueIdentifier);
            actionID.Direction = ParameterDirection.Input;
            actionID.Value = _action.ActionId;

            var objectxml = new SqlParameter("@objectxml", SqlDbType.Xml);
            objectxml.Direction = ParameterDirection.Input;
            objectxml.Value = writer.ToString();

            comm.Parameters.Add(conditionID);
            comm.Parameters.Add(actionID);
            comm.Parameters.Add(objectxml);

            var reader = comm.ExecuteReader();
        }
        private static bool ActionExists(Guid actionId)
        {
            var comm = new SqlCommand("select count(ActionId) from Actions where ActionId = '" + actionId + "'");
            comm.Connection = DBHelper.Connection();
            comm.CommandType = CommandType.Text;
            var reccount = comm.ExecuteReader();

            if (reccount.Read())
            {
                if (reccount.GetInt32(0) == 0)
                {
                    //it's a new Condition
                    return false;
                }
                else
                {
                    return true;
                }
            }
            throw new NullReferenceException("Unable to parse SQL in SceneExists.");
        }
        public static List<IAction> GetActions(Guid conditionId)
        {

            List<IAction> actions = new List<IAction>();
            SqlCommand comm = new SqlCommand()
            {
                CommandText = "GetActions",
                Connection = DBHelper.Connection(),
                CommandType = CommandType.StoredProcedure
            };
            comm.Parameters.Add(new SqlParameter() { DbType = DbType.Guid, Value = conditionId, ParameterName = "@conditionid" });
            SqlDataReader reader = comm.ExecuteReader();
            while (reader.Read())
            {
                IAction action = CreateActionFromDb(reader.GetInt32(1), reader.GetXmlReader(2));
                actions.Add(action);
            }
            return actions;
        }        
        private static string GetActionObjectXml(IAction action)
        {
            XmlSerializer serializer;
            var writer = new StringWriter(CultureInfo.InvariantCulture);

            switch (action.ActionType)
            {
                case Enumerations.ActionType.MessageBox:
                    serializer = new XmlSerializer(typeof(MessageBox));
                    break;
                
                case Enumerations.ActionType.ConnectToCmd:
                    serializer = new XmlSerializer(typeof(ConnectToCmd));
                    break;

                default:
                    throw new Exception("Unknown condition type.");
                    break;
            }

            serializer.Serialize(writer, action);
            return writer.ToString();

        }
        private static IAction CreateActionFromDb(int actionType, XmlReader objectXml)
        {
            
            switch (actionType)
            {
                case 0:
                    XmlSerializer messageboxSerializer = new XmlSerializer(typeof(MessageBox));
                    MessageBox msgboxAction = (MessageBox)messageboxSerializer.Deserialize(objectXml);
                    return msgboxAction;
                case 4:
                    XmlSerializer connectToCmdSerializer = new XmlSerializer(typeof(ConnectToCmd));
                    ConnectToCmd connectToCmd = (ConnectToCmd)connectToCmdSerializer.Deserialize(objectXml);
                    return connectToCmd;

                default:
                    throw new NotImplementedException();
            //MessageBox = 0,
            //OpenDatasource,
            //NextRecord,
            //EnterData,
            //ConnectToCmd
            }
        }


        public static Guid? AddAutomation(string name, string description)
        {
            Guid? id = null;

            var comm = new SqlCommand("AddAutomation") { CommandType = CommandType.StoredProcedure };
            comm.Parameters.Add(new SqlParameter("@autname", SqlDbType.VarChar)
            {
                Size = 50,
                Direction = ParameterDirection.Input,
                Value = name
            });

            comm.Parameters.Add(new SqlParameter("@desc", SqlDbType.VarChar)
            {
                Size = 255,
                Direction = ParameterDirection.Input,
                Value = description
            });

            comm.Connection = DBHelper.Connection();

            var reader = comm.ExecuteReader();
            if (reader.Read())
            {
                id = reader.GetSqlGuid(0).Value;
            }
            reader.Close();
            return id;
        }

    }
}