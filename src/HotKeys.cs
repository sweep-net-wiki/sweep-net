using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using sweepnet.core;

namespace sweepnet
{
    public partial class HotKeys : Form
    {
        public HotKeys()
        {
            InitializeComponent();
        }

        private void HotKeys_Load(object sender, EventArgs e)
        {
            text.Text += "Review the changes: ".T() + AllHotKeys.list["review"].name + "\r\n";
            text.Text += "Switch to the next page: ".T() + AllHotKeys.list["next"].name + "\r\n";
            text.Text += "Rollback the changes: ".T() + AllHotKeys.list["revert"].name + "\r\n";
            text.Text += "Scroll: ".T() + AllHotKeys.list["scroll"].name + "\r\n";
            text.Text += "Fetch information about the editors: ".T() + AllHotKeys.list["authors"].name + "\r\n";
        }

        private void HotKeys_Deactivate(object sender, EventArgs e)
        {
            this.Close();
        }

        private void text_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
