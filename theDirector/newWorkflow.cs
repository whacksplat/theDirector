using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestingHosting
{
    public partial class newWorkflow : Form
    {
        public Int32 WorkFlowType;

        public newWorkflow()
        {
            InitializeComponent();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            WorkFlowType = (Convert.ToInt32(listView2.SelectedItems[0].Tag));
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            WorkFlowType = 0;
        }
    }
}
