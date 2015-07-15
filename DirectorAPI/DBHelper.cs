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
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml.Serialization;
using DirectorAPI.Actions;
using DirectorAPI.Actions.Connection;
using DirectorAPI.Actions.Datasource;
using DirectorAPI.Actions.Notifications;

namespace DirectorAPI
{
    public static class DBHelper
    {
        private static SqlConnection _conn;


        public static bool IsAutomationNameUnique(string name)
        {
            string sql = "select count(Name) from Automations where Name = '" + name + "';";
            SqlCommand comm = new SqlCommand(sql);
            comm.Connection = Connection();
            comm.CommandType= CommandType.Text;
            SqlDataReader reader = comm.ExecuteReader();
            reader.Read();
            if (reader.GetInt32(0) == 0)
            {
                reader.Close();
                return true;
            }
            else
            {
                reader.Close();
                return false;
            }
        }


        public static SqlConnection Connection()
        {
            if (_conn == null)
            {
                _conn = new SqlConnection("Data Source=(local);Initial Catalog=Director;Integrated Security=SSPI;MultipleActiveResultSets=true;");
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
                _conn.ConnectionString = "Data Source=(local);Initial Catalog=Director;Integrated Security=SSPI;MultipleActiveResultSets=true;";
                _conn.Open();
            }
            return _conn;
        }



        public static void UpdateAction(object action)
        {
            AAction _action = action as AAction;
            StringWriter writer = new StringWriter(CultureInfo.InvariantCulture);

            //create the xml
            switch (_action.ActionType)
            {
                case Action.ActionType.MessageBox:
                    XmlSerializer serializer = new XmlSerializer(typeof(MessageBox));
                    MessageBox messagebox = (MessageBox) action;
                    serializer.Serialize(writer,messagebox);



                    break;
                default:
                    break;
            }

            SqlCommand comm = new SqlCommand("UpdateAction");
            comm.Connection = Connection();
            comm.CommandType = CommandType.StoredProcedure;

            SqlParameter conditionID = new SqlParameter("@conditionid", SqlDbType.UniqueIdentifier);
            conditionID.Direction = ParameterDirection.Input;
            conditionID.Value = _action.ConditionId;

            SqlParameter actionID = new SqlParameter("@actionid", SqlDbType.UniqueIdentifier);
            actionID.Direction = ParameterDirection.Input;
            actionID.Value = _action.ActionId;

            SqlParameter objectxml = new SqlParameter("@objectxml", SqlDbType.Xml);
            objectxml.Direction = ParameterDirection.Input;
            objectxml.Value = writer.ToString();

            comm.Parameters.Add(conditionID);
            comm.Parameters.Add(actionID);
            comm.Parameters.Add(objectxml);

            SqlDataReader reader = comm.ExecuteReader();
        }

        
        public static Guid AddAction(string xml,AAction action)
        {
            //Guid conditionId,Action.ActionType type,
            SqlCommand comm2 = new SqlCommand("AddAction");
            comm2.Connection = Connection();
            comm2.CommandType = CommandType.StoredProcedure;


            SqlParameter conditionID = new SqlParameter("@conditionid", SqlDbType.UniqueIdentifier);
            conditionID.Direction = ParameterDirection.Input;
            conditionID.Value = action.ConditionId;

            SqlParameter actionID = new SqlParameter("@actionid", SqlDbType.UniqueIdentifier);
            actionID.Direction = ParameterDirection.Input;
            actionID.Value = action.ActionId;

            SqlParameter actiontype = new SqlParameter("@actiontype", SqlDbType.Int);
            actiontype.Direction = ParameterDirection.Input;
            actiontype.Value = action.ActionType;

            SqlParameter objectxml = new SqlParameter("@objectxml", SqlDbType.Xml);
            objectxml.Direction = ParameterDirection.Input;
            objectxml.Value = xml;

            comm2.Parameters.Add(conditionID);
            comm2.Parameters.Add(actionID);
            comm2.Parameters.Add(actiontype);
            comm2.Parameters.Add(objectxml);

            SqlDataReader reader = comm2.ExecuteReader();
            if (reader.HasRows)
            {
                if (reader.Read())
                {
                    Guid retval = reader.GetGuid(0);
                    reader.Close();
                    reader = null;
                    return retval;
                }
            }
            throw new Exception("Unable to AddAction");
        }
    }
}
