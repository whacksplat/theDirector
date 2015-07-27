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


using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DirectorAPI
{
    public static class Director
    {
        public static Automation AddAutomation(string Name,string Description)
        {
            return new Automation(Name, Description);
        }

        public static List<Automation> ListAutomations()
        {
            var obj = new List<Automation>();
            var comm = new SqlCommand("GetAutomations");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Connection = DBHelper.Connection();
            
            var reader = comm.ExecuteReader();

            while (reader.Read())
            {
                obj.Add(new Automation(reader.GetString(1),reader.GetString(2), reader.GetGuid(0)));
            }

            reader.Close();

            return obj;

        }
    }
}
