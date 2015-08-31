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

        private static string GetSceneObjectXML(IScene scene)
        {

            XmlSerializer serializer;
            var writer = new StringWriter(CultureInfo.InvariantCulture);
            switch (scene.Type)
            {
                case SceneEnums.SceneType.Always:
                    serializer = new XmlSerializer(typeof (AlwaysScene));
                    break;
                case SceneEnums.SceneType.Connection:
                    throw new NotImplementedException("Unknown scene type.");
                    break;
                case SceneEnums.SceneType.Datasource:
                    throw new NotImplementedException("Unknown scene type.");
                    break;
                case SceneEnums.SceneType.EndAutomation:
                    throw new NotImplementedException("Unknown scene type.");
                    break;
                case SceneEnums.SceneType.Variable:
                    throw new NotImplementedException("Unknown scene type.");
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

            switch (condition.Type)
            {
                case Enumerations.ConditionTypes.Always:
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

        public static void UpdateAction(object action)
        {
            var _action = action as AAction;
            var writer = new StringWriter(CultureInfo.InvariantCulture);

            //create the xml
            switch (_action.ActionType)
            {
                case Action.ActionType.MessageBox:
                    var serializer = new XmlSerializer(typeof (MessageBox));
                    var messagebox = (MessageBox) action;
                    serializer.Serialize(writer, messagebox);
                    break;

                case Action.ActionType.ConnectToCmd:
                    var ctcSerializer = new XmlSerializer(typeof (ConnectToCmd));
                    var ctc = (ConnectToCmd) action;
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
            conditionID.Value = _action.ConditionId;

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

        public static Guid AddAction(string xml, AAction action)
        {
            //Guid conditionId,Action.ActionType type,
            var comm2 = new SqlCommand("AddAction");
            comm2.Connection = Connection();
            comm2.CommandType = CommandType.StoredProcedure;


            var conditionID = new SqlParameter("@conditionid", SqlDbType.UniqueIdentifier);
            conditionID.Direction = ParameterDirection.Input;
            conditionID.Value = action.ConditionId;

            var actionID = new SqlParameter("@actionid", SqlDbType.UniqueIdentifier);
            actionID.Direction = ParameterDirection.Input;
            actionID.Value = action.ActionId;

            var actiontype = new SqlParameter("@actiontype", SqlDbType.Int);
            actiontype.Direction = ParameterDirection.Input;
            actiontype.Value = action.ActionType;

            var objectxml = new SqlParameter("@objectxml", SqlDbType.Xml);
            objectxml.Direction = ParameterDirection.Input;
            objectxml.Value = xml;

            comm2.Parameters.Add(conditionID);
            comm2.Parameters.Add(actionID);
            comm2.Parameters.Add(actiontype);
            comm2.Parameters.Add(objectxml);

            var reader = comm2.ExecuteReader();
            if (reader.HasRows)
            {
                if (reader.Read())
                {
                    var retval = reader.GetGuid(0);
                    reader.Close();
                    reader = null;
                    return retval;
                }
            }
            throw new Exception("Unable to AddAction");
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
                    Value = condition.Type
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
                    Value = condition.Type
                });

                comm.ExecuteNonQuery();
            }
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

            comm.Parameters.Add(new SqlParameter() { DbType = DbType.Guid, Value = sceneId, ParameterName = "@sceneid"});
            SqlDataReader reader = comm.ExecuteReader();

            if (reader.HasRows)
            {
                if (reader.Read())
                {
                    conditions.Add(CreateConditionFromDb(sceneId,
                        reader.GetGuid(0),
                        reader.GetString(1),
                        reader.GetInt32(2)));
                }
            }

            reader.Close();
            return conditions;
        }

        private static ICondition CreateConditionFromDb(Guid sceneId, Guid conditionId, string objectXml, Int32 conditionType)
        {
            switch (conditionType)
            {
                case 0:
                    return new AlwaysCondition(){ConditionId = conditionId,SceneId = sceneId,Type = Enumerations.ConditionTypes.Always};
                default:
                    throw new Exception("unknown condition type in CreateConditionFromDb");
            }
                
        }
    }
}