using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Activities;
using System.Windows.Forms;
using System.ComponentModel;

namespace DirectorAPI.IO
{
    public sealed class Messagebox : CodeActivity
    {
        public enum MsgBoxBtn
        {
            OkOnly,YesNoCancel
        }

        public InArgument<string> Text { get; set; }
        public InArgument<string> Caption { get; set; }
        public MessageBoxButtons Buttons { get; set; }

        public InArgument<MessageBoxIcon> Icon { get; set; }
        public InArgument<MessageBoxDefaultButton> DefaultButton { get; set; }
        protected override void Execute(CodeActivityContext context)
        {
            MessageBox.Show(this.Text.Get(context), 
                this.Caption.Get(context), 
                MessageBoxButtons.OK,
                this.Icon.Get(context), 
                this.DefaultButton.Get(context));
        }
    }
}
