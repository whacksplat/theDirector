﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DirectorAPI.Interfaces
{
    public interface IAction
    {
        string NextScene { get; set; }
        Guid ConditionID { get; set; }
        Guid ActionId { get; set; }
        Action.ActionType ActionType { get; set; }
        void BuildCode();
        string Execute();
    }
}