﻿/*
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using DirectorAPI.Actions;
using DirectorAPI.Actions.Connection;
using DirectorAPI.Actions.Datasource;
using MessageBox = DirectorAPI.Actions.Notifications.MessageBox;
using System.Xml.Serialization;

namespace DirectorAPI
{
    public class Condition
    {
        private string _code;

        public List<object> Actions = new List<object>();

        CompilerResults results;
        private string _displayCode;
        readonly Automation _automation;

        [BrowsableAttribute(false)]
        public Automation automation
        {
            get
            {
                return _automation;
            }
        }

        [ReadOnlyAttribute(true)]
        public string DisplayCode
        {
            get { return _displayCode; }
            set { _displayCode = value; }
        }

        [ReadOnlyAttribute(true)]
        public string Code
        {
            get
            {
                return _code;
            }
            set
            {
                _code = value;
            }

        }
        
        [BrowsableAttribute(false)]
        public Guid SceneID { get; private set; }

        [BrowsableAttribute(false)]
        public Guid ConditionId { get; private set; }

        public Condition(Guid sceneid, string code, string displaycode,Automation automation)
        {
            _automation = automation;

            //fire off a query
            SqlCommand comm = new SqlCommand("AddCondition");
            comm.CommandType = CommandType.StoredProcedure;

            SqlParameter stepguid = new SqlParameter("@sceneid", SqlDbType.UniqueIdentifier);
            stepguid.Value = sceneid;

            SqlParameter condition = new SqlParameter("@condition", SqlDbType.Text);
            condition.Value = code;

            SqlParameter dcode = new SqlParameter("@displaycode", SqlDbType.Text);
            dcode.Value = displaycode;

            comm.Parameters.Add(stepguid);
            comm.Parameters.Add(condition);
            comm.Parameters.Add(dcode);

            comm.Connection = DBHelper.Connection();

            SqlDataReader reader = comm.ExecuteReader();
            if (reader.Read())
            {
                ConditionId = reader.GetSqlGuid(0).Value;
            }
            reader.Close();

            SceneID = sceneid;
            
            _code = code;
            _displayCode = displaycode;

        }


        public Condition(Guid sceneid, Guid id, string code,string displaycode,Automation automation)
        {
            _automation = automation;
            SceneID = sceneid;
            ConditionId = id;
            _code = code;
            _displayCode = displaycode;
        }


        /// <summary>
        /// The AddAction method will create an instance of an IAction,
        /// </summary>
        /// <param name="type">The type of action to create.</param>
        /// <returns></returns>
        public object AddAction(Action.ActionType type)
        {
            //DBHelper.AddAction()
            XmlSerializer serializer;
            StringWriter writer = new StringWriter(CultureInfo.InvariantCulture);

            switch (type)
            {
                case Action.ActionType.MessageBox:
                    MessageBox msgbox = new MessageBox(ConditionId);
                    msgbox.Message = "";
                    msgbox.Title = "";

                    serializer = new XmlSerializer(typeof(MessageBox));
                    serializer.Serialize(writer,msgbox);

                    //shouldn't I just pass in the above object to make life simpler?
                    DBHelper.AddAction(writer.ToString(),msgbox);

                    Actions.Add(msgbox);
                    return msgbox;
                
                case Action.ActionType.OpenDatasource:
                    OpenDataSource ds = new OpenDataSource();
                    //ds.NewOpenDataSource(ID,_automation);
                    //ds.NewAction(ID, _automation);
                    //Actions.Add(ds);
                    return ds;

                case Action.ActionType.NextRecord:
                    NextRecord nr = new NextRecord();
                    //nr.NewNextRecord(ID, _automation);
                    //Actions.Add(nr);
                    return nr;
                case Action.ActionType.EnterData:
                    EnterData ed = new EnterData();
                    //ed.NewEnterData(ID, _automation);
                    //ed.NewAction(ID, _automation);
                    //Actions.Add(ed);
                    return ed;
                case Action.ActionType.ConnectToCmd:
                    ConnectToCmd ctc = new ConnectToCmd();
                    //ctc.NewConnectToCmd(ID, _automation);
                    //ctc.NewAction(ID,_automation);
                    //Actions.Add(ctc);
                    //_automation.Connection.StartProcess(ctc.CommandLine,"");
                    return ctc;

                default:
                    return null;
            }

        }

        /// <summary>
        /// The BuildCode method will create the assembly that will be used in the Eval method.
        /// The assembly is created on demand and will not be stored in the database.
        /// </summary>
        /// <param name="automation"></param>
        public void BuildCode(Automation automation)
        {
            results = CodeHelper.CreateConditionCode(automation, _code);
            foreach (AAction action in Actions)
            {
                action.BuildCode(automation);
            }
        }


        /// <summary>
        /// The eval function uses the assembly created on the BuildCode method and invokes the TestValue method in the assembly.
        /// </summary>
        /// <param name="automation"></param>
        /// <returns>Returns true if the condition is true.</returns>
        public bool Eval(Automation automation)
        {
            //BuildCode(automation);
            var myclass = results.CompiledAssembly.CreateInstance("ConsoleApplication1.Program");
            Type t = myclass.GetType();
            MethodInfo mi = t.GetMethod("TestValue");
            object[] obj = new object[] { automation };
            var res = mi.Invoke(myclass, obj);
            return (bool)res;
        }

        /// <summary>
        /// This method will execute all the actions contained in the condition, and will return the IAction.NextScene in case Automation flow needs to affected.
        /// </summary>
        /// <param name="automation"></param>
        /// <returns>Returns the name of the NextScene to execute</returns>
        public string Execute(Automation automation)
        {
            foreach (AAction action in Actions)
            {
                action.Execute(automation);
                
                System.Threading.Thread.Sleep(50);
                if (!string.IsNullOrEmpty(action.NextScene))
                {
                    return action.NextScene;
                }

            }
            return null;
        }

        public void GetActions()
        {

            Actions.Clear();

            SqlCommand comm = new SqlCommand("select ConditionID,ActionID,ActionType,ObjectXML from Actions where ConditionID = '" + ConditionId + "'");
            comm.Connection = DBHelper.Connection();
            comm.CommandType = CommandType.Text;
            SqlDataReader reader = comm.ExecuteReader();
            
            while (reader.Read())
            {
                switch (reader.GetInt32(2))
                {
                    case 0:     //messagebox
                        //Actions.Add(new MessageBox(reader.GetGuid(1)));
                        XmlSerializer serializer = new XmlSerializer(typeof(MessageBox));
                        MessageBox msgbox = (MessageBox)serializer.Deserialize(reader.GetXmlReader(3));
                        Actions.Add(msgbox);
                        break;
                    
                    case 1:     //datasource
                        Actions.Add(new OpenDataSource(reader.GetGuid(1),_automation));
                        break;
                    
                    case 2:     //nextrecord
                        Actions.Add(new NextRecord(reader.GetGuid(1), _automation));
                        break;

                    case 3:     //enter data
                        Actions.Add(new EnterData(reader.GetGuid(1), _automation));
                        break;

                    case 4: //ConnectToCmd
                        Actions.Add(new ConnectToCmd());
                        break;

                    default:
                        break;

                }
            }
            reader.Close();

        }
    }
}
