using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Activities;

namespace TestMessageboxActivity
{
    public sealed class Messagebox : CodeActivity
    {
        public InArgument<string> Text { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            System.Windows.Forms.MessageBox.Show(this.Text.Get(context));
        }
    }
}
