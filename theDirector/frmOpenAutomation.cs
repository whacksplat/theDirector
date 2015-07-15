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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DirectorAPI;

namespace theDirector
{
    public partial class frmOpenAutomation : Form
    {
        private Guid _id;

        public Guid id
        {
            get { return _id; }
        }

        public frmOpenAutomation()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (this.listBox1.SelectedItem == null)
            {
                _id = System.Guid.Empty;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            _id = System.Guid.Empty;
        }

        private void frmOpenAutomation_Load(object sender, EventArgs e)
        {
            //Director.ListAutomations();
            foreach (Automation obj in Director.ListAutomations())
            {
                listBox1.Items.Add(obj.Name);
            }
            Refresh();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex > -1)
            {
                _id = Director.ListAutomations()[listBox1.SelectedIndex].Id;
            }

        }
    }
}
