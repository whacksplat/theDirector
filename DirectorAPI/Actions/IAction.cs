using System;
using System.CodeDom.Compiler;
using System.Runtime.Serialization;

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

