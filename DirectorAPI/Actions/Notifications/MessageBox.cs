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

namespace DirectorAPI.Actions.Notifications
{
    public class MessageBox:AAction
    {
        private string _message;
        private string _title;
        private string _nextScene;
        //private Guid _actionId;
        //private Action.ActionType _actionType;
        CompilerResults _compilerResults;

        [Category("General"),
        Description("This is the Message that will appear in the body of the MessageBox.")]
        public string Message
        {
            get { return _message;}
            
            set
            {
                _message = value;
            }
        }

        [Category("General"),
        Description("This is what will appear in the title bar of the MessageBox.")]
        public string Title
        {
            get {return _title;}
            set
            {
                _title = value;
            }
        }

        public MessageBox() 
        {
        }

        public MessageBox(Guid conditionId)
        {
            //these fields need to be populated in order to be serialized
            _message = "";
            _title = "";
            ConditionId = conditionId;
            ActionId = Guid.NewGuid();
            NextScene = "";
            CompilerResults _compilerResults = null;
        }


        [ReadOnly(true),
        Category("Action Properties")]
        public override Guid ConditionId { get; set; }


        [ReadOnly(true),
        Category("Action Properties")]
        public override Guid ActionId { get; set; }

        [ReadOnly(true),
        Category("Action Properties")]
        public override Action.ActionType ActionType { get; set; }
        
        [TypeConverter(typeof(TypeConverters.SceneNameConverter)),
        Category("Action Properties"),
        Description("The next Scene to give command to when this action completes successfully.")]
        public override string NextScene { get; set; }


        public override void BuildCode(Automation automation)
        {
            var src = "System.Windows.Forms.MessageBox.Show(" + "\"" + Message + "\" , \"" + Title + "\");";
            _compilerResults = CodeHelper.CreateActionCode(automation, src);
        }

        public override string Execute(Automation automation)
        {
            object[] obj = { automation };
            var myclass = _compilerResults.CompiledAssembly.CreateInstance("ConsoleApplication1.Program");

            if (myclass == null)
            {
                throw new Exception("Unable to find function or assembly in Execute.");
            }

            var t = myclass.GetType();
            var mi = t.GetMethod("Execute");
            mi.Invoke(myclass, obj);
            return _nextScene;
        }

    }

}
