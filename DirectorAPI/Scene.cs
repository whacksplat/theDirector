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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;


namespace DirectorAPI
{
    public class Scene
    {
        public List<Condition> Conditions = new List<Condition>();
        public List<ScreenCondition> ScreenConditions = new List<ScreenCondition>();
 
        private string _name;
        private int _timeOutInterval;
        private string _timeOutScene;
        private SceneType _type;
        private Automation _automation;

        public enum SceneType
        {
            Always = 0,
            Connection,
            Datasource,
            Variable,
            EndAutomation
        }

        [BrowsableAttribute(false)] 
        public Automation automation
        {
            get
            {
                return _automation;
            }
        }
        
        [CategoryAttribute("Scene Properties"),
        DescriptionAttribute("The unique name of the scene. All scene names must be unique in an Automation.")]
        public string Name
        {
            get 
            { 
                return _name; 
            }

            set
            {
                //make sure no duplicate
                SqlCommand comm = new SqlCommand("select count(AutomationID) from Scenes where AutomationID = '" + AutomationId + "' and Name = '" + value + "'");
                comm.Connection = DBHelper.Connection();
                comm.CommandType = CommandType.Text;
                SqlDataReader reccount = comm.ExecuteReader();

                if (reccount.Read())
                {
                    if (reccount.GetInt32(0) == 0)
                    {
                        //update the database with the new name
                        reccount.Close();
                        comm.CommandText = "update Scenes set Name = '" + value + "' where SceneID = '" + SceneId + "'";
                        comm.ExecuteNonQuery();
                        _name = value;

                    }
                    else
                    {
                        reccount.Close();
                        throw new Exception("Cannot have a duplicate scene name.");
                    }
                };

            }
        }

        [BrowsableAttribute(false)] 
        public Guid AutomationId { get; private set; }
        
        [BrowsableAttribute(false)] 
        public Guid SceneId { get; private set; }

        [BrowsableAttribute(false)] 
        public int SortId { get; private set; }

        [CategoryAttribute("Scene Properties"),
        DescriptionAttribute("The name of the Checkpoint that needs to be checked against when a Scene begins.")]
        public string CheckPoint { get; set; }

        [TypeConverter(typeof(DirectorAPI.TypeConverters.SceneNameConverter)), CategoryAttribute("Scene Properties"),
        DescriptionAttribute("The name of the Scene that control will be passed to if the CheckPoint fails.")]
        public string CheckPointFailureScene {get;set;}

        [CategoryAttribute("Scene Properties"),
        DescriptionAttribute("The amount of time in milliseconds for a timeout. If no condition is fulfilled within this time, control will be passed to the TimeOutScene")]
        public int TimeOutInterval { 
            get
            {
                return _timeOutInterval;
            }
            set
            {
                _timeOutInterval = value;

                SqlCommand comm = new SqlCommand("Update Scenes set TimeOutInterval = " + value + " where ID = '" + SceneId.ToString() + "'");
                comm.Connection = DBHelper.Connection();
                comm.ExecuteNonQuery();
            }
        }

        [TypeConverter(typeof(DirectorAPI.TypeConverters.SceneNameConverter)),
        CategoryAttribute("Scene Properties"),
        DescriptionAttribute("The name of the Scene controll will be passed to when a TimeOut has been generated.")]
        public string TimeOutScene 
        {
            get
            {
                return _timeOutScene;
            }
            set
            {
                _timeOutScene = value;

                SqlCommand comm = new SqlCommand("Update Scenes set TimeOutStep = '" + _timeOutScene + "' where id = '" + SceneId.ToString() + "'");
                comm.Connection = DBHelper.Connection();
                comm.ExecuteNonQuery();

            }
        }

        [CategoryAttribute("Scene Properties"),
        DescriptionAttribute("The Type of the Scene.")]
        public SceneType Type
        {
            get
            {
                return _type;
            }

            set
            {
                _type = value;
                int typevalue = 0;

                switch(value.ToString())
                {
                    case "Always":
                        typevalue = 0;
                        break;

                    case "Connection":
                        typevalue = 1;
                        break;

                    case "Datasource":
                        typevalue = 2;
                        break;

                    case "Variable":
                        typevalue = 3;
                        break;
                    case "EndAutomation":
                        typevalue = 4;
                        break;

                    default:
                        break;

                }

                SqlCommand comm = new SqlCommand("Update Scenes set SceneType = " + typevalue + " where SceneID = '" + SceneId + "'");
                comm.Connection = DBHelper.Connection();
                comm.ExecuteNonQuery();

            }
        }

        public SceneType GetSceneType(Int32 intType)
        {
            switch (intType)
            {
                case 0:
                    return SceneType.Always;

                case 1:
                    return SceneType.Connection;

                case 2:
                    return SceneType.Datasource;
                    
                case 3:
                    return SceneType.Variable;

                case 4:
                    return SceneType.EndAutomation;
                    
                default:
                    return SceneType.Always;

            }
        }
        public Scene(string name,Guid automationid,Guid sceneid,int sortid, int timeoutinterval, string timeoutscene, int type,Automation automation)
        {
            _automation = automation;
            _name = name;
            AutomationId=automationid;
            SceneId = sceneid;
            SortId = sortid;
            _timeOutInterval = timeoutinterval;
            _timeOutScene = timeoutscene;
            _type = GetSceneType(type);
//            GetConditions();
        }


        public void GetConditions()
        {
            SqlCommand comm = new SqlCommand("select SceneID,ConditionID,Condition,DisplayCode from Conditions where SceneID = '" + SceneId + "'");
            comm.Connection = DBHelper.Connection();
            comm.CommandType = CommandType.Text;
            SqlDataReader reader = comm.ExecuteReader();
            while (reader.Read())
            {
                Conditions.Add(new Condition(reader.GetGuid(0),reader.GetGuid(1),reader.GetString(2),reader.GetString(3),_automation));
                
            }

            reader.Close();
            foreach (Condition condition in Conditions)
            {
                condition.GetActions();
            }

        }

        public Scene(Guid automationid,Automation automation)
        {
            _automation = automation;
            AutomationId = automationid;

            //find a unique name
            Name = GetNextSceneName(automationid);

            //get the sortid
            SortId = GetNextSortID();

            //add to database
            AddSceneToDB();
        }

        private void AddSceneToDB()
        {
            SqlCommand comm = new SqlCommand();
            comm.Connection = DBHelper.Connection();
            comm.CommandText = "AddScene";
            comm.CommandType=CommandType.StoredProcedure;

            SqlParameter name = new SqlParameter("@name",SqlDbType.VarChar,50);
            name.Direction = ParameterDirection.Input;
            name.Value = Name;

            SqlParameter automationid = new SqlParameter("@automationid",SqlDbType.UniqueIdentifier);
            automationid.Direction = ParameterDirection.Input;
            automationid.Value = AutomationId;

            SqlParameter sortid = new SqlParameter("@sortid",SqlDbType.Int);
            sortid.Direction = ParameterDirection.Input;
            sortid.Value = SortId;

            comm.Parameters.Add(name);
            comm.Parameters.Add(automationid);
            comm.Parameters.Add(sortid);

            SqlDataReader reader = comm.ExecuteReader();
            if (reader.HasRows)
            {
                if (reader.Read())
                {
                    SceneId = reader.GetGuid(0);
                }
            }

            reader.Close();

        }

        //Guid autiomationid
        private int GetNextSortID()
        {
            int result;
            SqlCommand comm = new SqlCommand("select MAX(SortID) from Scenes where AutomationID = '" + AutomationId + "'");
            comm.Connection = DBHelper.Connection();
            comm.CommandType=CommandType.Text;
            SqlDataReader reader = comm.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();
                if (reader.IsDBNull(0))
                {
                    reader.Close();
                    return 0;
                }
                result =reader.GetInt32(0) + 1;
                reader.Close();
                return result;
            }
            else
            {
                reader.Close();
                return 0;
            }

        }

        private string GetNextSceneName(Guid automationid)
        {
            int count = 0;
            bool found = false;
            SqlDataReader reader;
            SqlCommand comm = new SqlCommand();
            comm.CommandType = CommandType.Text;
            
            while (!found)
            {
                count += 1;
                comm.CommandText = "select count(SceneID) from Scenes where Name = 'NewScene" + Convert.ToString(count) + "' and AutomationID = '" + automationid + "'";
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
        public Condition AddCondition(string code, string displayCode)
        {
            Condition cond = new Condition(SceneId, code, displayCode, _automation);
            Conditions.Add(cond);
            return cond;
        }

        public void AddScreenCondition(ScreenCondition condition)
        {
            ScreenConditions.Add(condition);
        }
    }



}
