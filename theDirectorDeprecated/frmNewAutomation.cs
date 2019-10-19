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
using System.Windows.Forms;
using DirectorAPI;

namespace theDirector
{
    public partial class frmNewAutomation : Form
    {
        private string _name;
        private string _description;

        public string automationname
        {
            get {return _name;}
        }

        public string description
        {
            get { return _description; }
        }

        public frmNewAutomation()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void frmNewAutomation_Load(object sender, EventArgs e)
        {

        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("You must provide a name for new Automation.");
                DialogResult = DialogResult.None;
                return;
            }
            if (!DBHelper.IsAutomationNameUnique(txtName.Text))
            {
                MessageBox.Show("Automation name must be unique!");
                DialogResult=DialogResult.None;
                return;
            }
            _name = txtName.Text;
            _description = txtDescription.Text;

        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtName_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //if (!DBHelper.IsAutomationNameUnique(txtName.Text))
            //{
            //    MessageBox.Show("Automation name must be unique!");
            //    e.Cancel = true;
            //}
        }
    }
}
