using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Activities;
using System.Windows.Forms;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DirectorAPI.IO
{
    public sealed class Messagebox : CodeActivity
    {
        public InArgument<string> Text { get; set; }

        //[RequiredArgument] 
        public InArgument<string> Caption { get; set; }= new InArgument<string>("Caption");
        
        public MessageBoxButtons Buttons { get; set; }
        public MessageBoxIcon Icon { get; set; }
        public MessageBoxDefaultButton DefaultButton { get; set; }

        public OutArgument<DialogResult> Result
        {
            get;
            set;
        }

        protected override void Execute(CodeActivityContext context)
        {
            DialogResult localResult = MessageBox.Show(this.Text.Get(context), 
                this.Caption.Get(context), 
                Buttons,
                this.Icon, 
                this.DefaultButton);
            
             Result.Set(context,localResult);
        }
    }
}
