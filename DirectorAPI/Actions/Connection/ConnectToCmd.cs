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
using System.Reflection;
using DirectorAPI.Connections;
using DirectorAPI.Interfaces;

namespace DirectorAPI.Actions.Connection
{
    public class ConnectToCmd : IAction
    {
        CompilerResults _compilerResults;

        public ConnectToCmd()
        {
            //todo check to see if it's already created
            //if (AutomationHelper.automation.CurrentMode != Automation.Mode.Loading)
            //{
            //    ResetConnection();
            //}
        }

        public void ResetConnection()
        {
            AutomationHelper.automation.Connection = new ConsoleConnectionRedirection();
            AutomationHelper.automation.ResetEventSyncs();
            AutomationHelper.automation.Connection.Connect();
        }

        [TypeConverter(typeof(TypeConverters.SceneNameConverter))]
        public string NextScene { get; set; }

        [ReadOnly(true)]
        public Guid ConditionID { get; set; }

        [ReadOnly(true)]
        public Guid ActionId { get; set; }

        [ReadOnly(true)]
        public Enumerations.ActionType ActionType { get; set; }

        public string DisplayText
        {
            get { return "Connect to Console (Redirection)"; }
        }

        public void BuildCode()
        {
            string src = "AutomationHelper.automation.Connection = new ConsoleConnectionRedirection();" + Environment.NewLine;
            src += "AutomationHelper.automation.ResetEventSyncs();" + Environment.NewLine;
            src += "AutomationHelper.automation.Connection.Connect();";

            _compilerResults = CodeHelper.CreateActionCode(src);
        }

        public string Execute()
        {
            object[] obj = new object[] { AutomationHelper.automation };
            object myclass = _compilerResults.CompiledAssembly.CreateInstance("ActionCode.Program");

            if (myclass == null)
            {
                throw new Exception("Unable to find function or assembly in Execute.");
            }

            Type t = myclass.GetType();
            MethodInfo mi = t.GetMethod("Execute");
            mi.Invoke(myclass, obj);
            return NextScene;
        }
    }
}
