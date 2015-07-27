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

namespace DirectorAPI.Actions
{
    //TODO need to refactor this class to be 2 classes, data and functional
    //TODO buildcode, execute, update and loadfromdb will need to go in the functional class
    //TODO NextScene, ConditionID, ActionID, actiontype will need to be stored. CompilerResults will not be stored, and will be created dynamically before run.

    [Serializable]
    public abstract class AAction
    {
        //public AAction()
        //{
        //}
        #region Properties

        /// <summary>
        /// The NextScene property holds the name of the Scene that control will get passed to after the Action is executed.
        /// </summary>
        public abstract string NextScene { get; set; }

        /// <summary>
        /// This is the unique GUID of the condition that this Action is a part of.
        /// </summary>
        public abstract Guid ConditionId { get; set; }

        /// <summary>
        /// This is the unique GUID of the Action. No other action will have this GUID.
        /// </summary>
        public abstract Guid ActionId { get; set; }

        /// <summary>
        /// This is the ActionType of the Action.
        /// </summary>
        public abstract Action.ActionType ActionType { get; set; }

        /// <summary>
        /// This holds the compiled assembly.
        /// </summary>
        //public abstract CompilerResults CompilerResults { get; set; }

        //public abstract Automation Automation { get; set; }

        #endregion


        #region Methods
        /// <summary>
        /// The BuildCode routine's responisibility is to generate the Execute function / assembly for execution.
        /// </summary>
        /// <param name="automation">This is the Automation object which will be passed to the Execute function in the assembly that was generated.</param>
        public abstract void BuildCode(Automation automation);

        /// <summary>
        /// The Execute routine executes the Execute method in the assembly generated in the BuildCode method.
        /// </summary>
        /// <param name="automation">This is the Automation object which will be passed to the Execute function in the assembly that was generated.</param>
        /// <returns></returns>
        public abstract String Execute(Automation automation);


        #endregion
    }
}

