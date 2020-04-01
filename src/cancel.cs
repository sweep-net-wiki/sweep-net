using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Threading;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace sweepnet
{
    public partial class cancel : Form
    {
        public Form1 q;
        public long curr, stab;

        private String g(String s) { return i18n.Get(s); }

        public cancel()
        {
            InitializeComponent();
            this.Text = g("0400");
        }
        public void StandartTitle1(String user, String id){
            textBox1.Text = String.Format(g("0401"), user, id);//"Отмена правки "+id+" участника "+user;
        }

        public void StandartTitle2(String id)
        {
            textBox1.Text = String.Format(g("0402"), id);// "Возвращение к версии " + id + ": ";
            textBox1.SelectionStart = textBox1.Text.Length;
        }

        private void cancel_Deactivate(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            q.aw.Cancel(curr, stab, textBox1.Text);
            //q.Next();
            q.NextArticle();
            this.Close();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13) { button1_Click(sender,e); }
        }

        private void cancel_Load(object sender, EventArgs e)
        {
            this.Top = this.q.Top + this.q.Height / 2 - this.Height / 2;
            this.Left = this.q.Left + this.q.Width / 2 - this.Width / 2;
        }
    }
}
