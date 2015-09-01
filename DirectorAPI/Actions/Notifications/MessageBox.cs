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
using DirectorAPI.Interfaces;

namespace DirectorAPI.Actions.Notifications
{
    public class MessageBox:IAction
    {
        public string NextScene { get; set; }
        
        [ReadOnly(true)]
        public Guid ConditionID { get; set; }
        
        [ReadOnly(true)]
        public Guid ActionId { get; set; }

        [ReadOnly(true)]
        public Enumerations.ActionType ActionType { get; set; }

        public string DisplayText
        {
            get { return "MessageBox (Message = " + Message + ", Title = " + Title + ")"; }
        }

        public void BuildCode()
        {
            throw new NotImplementedException();
        }

        public string Execute()
        {
            throw new NotImplementedException();
        }

        public string Message { get; set; }
        public string Title { get; set; }

    }

}
