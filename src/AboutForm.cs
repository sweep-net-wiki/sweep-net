using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace sweepnet
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
        }

        private void AboutForm_Deactivate(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AboutForm_Load(object sender, EventArgs e)
        {
            label1.Text = Version.GetName() + " " + Version.GetVersion();
            label3.Text = "This program is free software: you can redistribute it and/or modify\r\nit under the terms of the GNU General Public License as published by\r\nthe Free Software Foundation, either version 3 of the License, or\r\n(at your option) any later version.\r\n\r\nThis program is distributed in the hope that it will be useful,\r\nbut WITHOUT ANY WARRANTY; without even the implied warranty of\r\nMERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See\r\nthe GNU General Public License for more details.\r\n\r\nYou should have received a copy of the GNU General Public License\r\nalong with this program.  If not, see <http://www.gnu.org/licenses/>";
            label3.Text += "\r\n\r\n\r\nUses icons from Gnome 2.18 (GNU GPL/LGPL http://gnome.org \r\nCopyright © 2005‒2011 The GNOME Project) and DotNetWikiBot code \r\n(MIT license http://dotnetwikibot.sourceforge.net/ \r\n(c) Copyright © (2006-2011) Iaroslav Vassiliev)\r\n\r\nUkrainian translation: [[:m:User:Base]]";
        }

        private void AboutForm_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
