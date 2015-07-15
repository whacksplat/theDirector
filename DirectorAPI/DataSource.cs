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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;

namespace DirectorAPI
{
    public class DataSource
    {
        private System.Data.OleDb.OleDbDataReader reader;
        public object[] fields;
        private List<object[]> records = new List<object[]>();
        private int _currentRecord;

        public void Open(String filename, string sheetName)
        {
            System.Data.OleDb.OleDbConnection conn = new System.Data.OleDb.OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filename + ";Extended Properties=\"" + "Excel 12.0 Xml;HDR=YES;IMEX=1;" + "\"");
            conn.Open();
            System.Data.OleDb.OleDbCommand comm = new System.Data.OleDb.OleDbCommand("Select * from [" + sheetName + "]", conn);
            reader = comm.ExecuteReader(CommandBehavior.SingleResult);
            records.Clear();

            while (reader.Read())
            {
                fields = new object[reader.FieldCount];
                int fieldcount = reader.GetValues(fields);
                records.Add(fields);
            }
            //reader.
        }

  
        public void MoveNext()
        {
            if (!(_currentRecord > records.Count()))
            {
                _currentRecord += 1;
            }
        }

        public Boolean EOF()
        {
            return (_currentRecord > records.Count());
        }

    }
}
