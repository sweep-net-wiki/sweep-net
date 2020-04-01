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
    public delegate void ProcessTheInputAction(object o, string text);
    public partial class TextInputForm : Form
    {
        public event ProcessTheInputAction ProcessTheInput;
        public string text
        {
            get
            {
                return textBox1 == null ? null : textBox1.Text;
            }
            set
            {
                if (textBox1 != null) textBox1.Text = value;
            }
        }
        public bool successfully { get; set; }

        public TextInputForm(string title, string default_text)
        {
            InitializeComponent();
            this.Text = title;
            textBox1.Text = default_text;
            textBox1.SelectionStart = textBox1.Text.Length;
            successfully = false;
        }
       /* public void StandartTitle1(String user, String id){
            textBox1.Text = String.Format("Undid revision {0} by {1}:".T(), user, id);//"Отмена правки "+id+" участника "+user;
        }*/
        /*
        public void StandartTitle2(String id)
        {
            textBox1.Text = String.Format("Restoring revision {0}:".T(), id);// "Возвращение к версии " + id + ": ";
            textBox1.SelectionStart = textBox1.Text.Length;
        }*/

        private void cancel_Deactivate(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
           /* q.aw.Cancel(curr, stab, textBox1.Text);
            //q.Next();
            q.NextArticle();*/
            if(ProcessTheInput!=null) ProcessTheInput(this, textBox1.Text);
            successfully = true;
            this.Close();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13) { button1_Click(sender,e); }
        }

        private void cancel_Load(object sender, EventArgs e)
        {
            if (Owner != null)
                Location = new Point(Owner.Location.X + Owner.Width / 2 - Width / 2,
                    Owner.Location.Y + Owner.Height / 2 - Height / 2);
           // this.Top = this.q.Top + this.q.Height / 2 - this.Height / 2;
            //this.Left = this.q.Left + this.q.Width / 2 - this.Width / 2;
        }
    }
}
