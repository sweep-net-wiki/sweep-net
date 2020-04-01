using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Threading;
using System.Text;
using System.Windows.Forms;
using DotNetWikiBot;

namespace sweepnet
{
    public partial class Editor : Form
    {
        Page pg;
        CoreOperations aw;
        MainForm f;
        public Element curr;

        public Editor(Page p, MainForm q)
        {
            pg = p;
            aw = q.aw;
            f = q;
            InitializeComponent();
            this.Text = "Editor".T();
            label1.Text = "Line".T();
            go_button.Text = "Go".T();
            minor.Text = "Minor?".T();
            review.Text = "Review it".T();
            savebutton.Text = "Save".T();
            closebutton.Text = "Close".T();
        }

        public int GetPosOfLine(int line, String text)
        {
            int prev = 0;
            int curr = 0;
            while (1 == 1)
            {
                prev = text.IndexOf('\n', prev);
                //if ( >= 0)
                if (prev >= 0)
                {
                    curr++;
                    prev++;
                }
                else { break; }
                if (curr == line) { return prev; }
            }
            return -1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!Tools.IsNumeric(textBox1.Text))
            {
                MessageBox.Show("It must be a number!".T()); return;
            }
            int v = Convert.ToInt32(textBox1.Text);
            if (v < 1 || v > 1000000)
            {
                MessageBox.Show("Out of limits!".T()); return;
            }
            v--;
            int pos = 0;
            if (v != 0)
            {
                pos = GetPosOfLine(v, text.Text);
            }
            if (pos == -1)
            {
                MessageBox.Show("Out of limits!".T()); return;
            }
            //MessageBox.Show(pos + "");
            text.Focus();
            text.Select();
            text.SelectionStart = pos;
            text.ScrollToCaret();
        }

        private void Editor_Load(object sender, EventArgs e)
        {
            this.Top = this.f.Top + this.f.Height / 2 - this.Height / 2;
            this.Left = this.f.Left + this.f.Width / 2 - this.Width / 2;
            try
            {
                pg.Load();
                text.Text = pg.text;
            }
            catch { MessageBox.Show("Error while loading!".T()); this.Close(); }
        }

        public bool reviewed = false;
        public bool saved = false;

        private void button2_Click(object sender, EventArgs e)
        {
            if (comm.TextLength > 100) { MessageBox.Show("Summary is longer than it should be!".T()); return; }
            if (text.Text.Equals("")) { MessageBox.Show("You are not supposed to clear the text!".T()); return; }
            String c = comm.Text;
            String t = text.Text;
            bool m = minor.Checked;
            Thread myThread = new System.Threading.Thread(new System.Threading.ThreadStart(delegate()
            {
                try
                {
                    saved = true;
                    long lrv = Convert.ToInt64(pg.lastRevisionID);
                    long newrev=pg.Save2(t, c, m);
                    //Page p=aw.GetPage(pg.title);
                    if (newrev <= 0)
                    {
                        SweepNetPage p = new SweepNetPage(aw.mysite, pg.title);
                        p.LoadRevision(lrv);
                        newrev = p.maxid;
                        //  MessageBox.Show("rr");
                    }
                    if (newrev > 0)
                        {
                           // aw.UpdateMaxRevid(curr.stable_id, curr.curr_id, p.maxid);
                            curr.curr_id = newrev;
                            //обновить дифф

                            aw.UpdateDiff(curr);

                            if (review.Checked)
                            {

                                aw.Review(newrev);
                                // this.reviewed = true;
                            }
                        }
                }
                catch (Exception et) { MessageBox.Show("Error occured while applying the changes: ".T() + et.Message); }
            }));
            myThread.Start();
            this.reviewed = review.Checked;
         //   myThread.Join();
            this.Close();
         }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13) { button1_Click(sender, e); }
        }

        private void text_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void text_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.LinkText);
        }

        private void text_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Control == true && e.KeyCode == Keys.V)
            {
                // Strip any formatting from the clipboard
                string str = Clipboard.GetText();
                Clipboard.SetText(str);
            }
        }
    }
}
