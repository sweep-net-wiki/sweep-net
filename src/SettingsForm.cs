using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace sweepnet
{
    public partial class SettingsForm : Form
    {
        public MainForm main_form;
        public SettingsForm()
        {
            InitializeComponent();
            UpdateLanguage();
        }


        public static string T(string s) { return s.T(); }
        public void TranslateControls(params Component[] controls)
        {
            foreach (Component ctl in controls)
            {
                if (ctl is TabPage) ((TabPage)ctl).Text = T(((TabPage)ctl).Text);
                else if (ctl is Button) ((Button)ctl).Text = T(((Button)ctl).Text);
                else if (ctl is Label) ((Label)ctl).Text = T(((Label)ctl).Text);
                else if (ctl is GroupBox) ((GroupBox)ctl).Text = T(((GroupBox)ctl).Text);
                else if (ctl is CheckBox) ((CheckBox)ctl).Text = T(((CheckBox)ctl).Text);
            }
        }

        private void UpdateLanguage()
        {
            this.Text = T("Settings");

            TranslateControls(tabPage_common, tabPage_detailed, tabPage_pro);
            TranslateControls(def, save, closebutton);

            //1
            TranslateControls(groupBox2, groupBox4, groupBox5);
            TranslateControls(check_updates, check_links, auto_scroll);
            TranslateControls(label7, label8, label13);
            string[] list = T("New///Old").Split(new string[] { "///" }, StringSplitOptions.None);
            diff_style.Items.Clear();
            for (int a = 0; a < list.Length; a++)
            {
                diff_style.Items.Add(list[a]);
            }

            //2
            TranslateControls(groupBox1, groupBox3);
            TranslateControls(skip_null, label9, label12, label1, label3, label4);
            TranslateControls(applyonlytotheproject1, applyonlytotheproject2);
       
            label2.Text = T("articles") + "(5-480)";
            label5.Text = T("articles") + "(5-480)";
            label6.Text = T("articles") + "(3-480)";

            //3
            TranslateControls(groupBox6, label10, label11);
        }

        private void LoadSettings()
        {
            //save_password.Checked = (Options.GetOptionInt("save_pass") == 1 ? true : false);
            skip_null.Checked = (Options.GetOptionInt("skip_null") == 1 ? true : false);
            check_updates.Checked = (Options.GetOptionInt("check_updates") == 1 ? true : false);
            check_links.Checked = (Options.GetOptionInt("check_links") == 1 ? true : false);


            buff1.Text = Options.GetOption("buff1");
            buff2.Text = Options.GetOption("buff2");
            buff3.Text = Options.GetOption("buff3");

            recent_count.Text = Options.GetOption("recent_count");
            
            bool tmp = SiteManagerClass.review_summaries.ContainsKey(SiteManagerClass.current) && !(SiteManagerClass.review_summaries[SiteManagerClass.current].ToString().Equals(""));
            rw_comm.Text =  tmp?SiteManagerClass.review_summaries[SiteManagerClass.current].ToString():Options.GetOption("rw_comm");
            applyonlytotheproject1.Checked = tmp;

            tmp = SiteManagerClass.rollback_summaries.ContainsKey(SiteManagerClass.current) && !(SiteManagerClass.rollback_summaries[SiteManagerClass.current].ToString().Equals(""));
            rj_comm.Text = tmp ? SiteManagerClass.rollback_summaries[SiteManagerClass.current].ToString() : Options.GetOption("rj_comm");
            applyonlytotheproject2.Checked = tmp;

            pd_wait.Text = Options.GetOption("pd_wait");
            diff_style.SelectedIndex = Options.GetOption("diff_style").Equals("") ? 0 : 1;
            auto_scroll.Checked = Options.GetOptionInt("scroll_diff") == 1 ? true : false;
        }

        public void _st(int n)
        {
            tabControl1.SelectedIndex = n;
        }

        private bool CheckSettings()
        {
            if (!Tools.IsNumeric(buff1.Text) || Convert.ToInt32(buff1.Text) < 5 || Convert.ToInt32(buff1.Text) > 480) { _st(1); MessageBox.Show(T("Check the preloading data buffer size!")); return false; } //Неверно введен размер буфера для предзагрузки данных!
            if (!Tools.IsNumeric(buff2.Text) || Convert.ToInt32(buff2.Text) < 5 || Convert.ToInt32(buff2.Text) > 480) { _st(1); MessageBox.Show(T("Check the size of real-time changes buffer!")); return false; } //Неверно введен размер буфера для предзагрузки данных для мониторинга свежих!
            if (!Tools.IsNumeric(buff3.Text) || Convert.ToInt32(buff3.Text) < 3 || Convert.ToInt32(buff3.Text) > 480) { _st(1); MessageBox.Show(T("Check the size of diffs buffer!")); return false; } //Неверно введен размер буфера для предзагрузки диффов!
            if (!Tools.IsNumeric(recent_count.Text) || Convert.ToInt32(recent_count.Text) < 2 || Convert.ToInt32(recent_count.Text) > 10000) { _st(0); MessageBox.Show(T("Check the recent changes number!")); return false; } //Неверно введено количество "свежих" правок!
            if (rw_comm.Text.Length > 60) { _st(1); MessageBox.Show(T("Your custom summary is too long!")); return false; }
            if (rj_comm.Text.Length > 60) { _st(1); MessageBox.Show(T("Your custom summary is too long!")); return false; }

            if (!Tools.IsNumeric(pd_wait.Text) || Convert.ToInt32(pd_wait.Text) < 0 || Convert.ToInt32(pd_wait.Text) > 10000) { _st(2); MessageBox.Show(T("Check the delay length number!")); return false; } //Неверно введен размер задержки после выбора правок!
            
            return true;
        }

        public void SaveSettings()
        {
            Options.SetOption("skip_null", skip_null.Checked ? 1 : 0);
            Options.SetOption("check_updates", check_updates.Checked ? 1 : 0);

            Options.SetOption("check_links", check_links.Checked ? 1 : 0);


            Options.SetOption("buff1", buff1.Text);
            Options.SetOption("buff2", buff2.Text);
            Options.SetOption("buff3", buff3.Text);

            Options.SetOption("recent_count", recent_count.Text);

            if (applyonlytotheproject1.Checked)
            {
                SiteManagerClass.review_summaries[SiteManagerClass.current] = rw_comm.Text;
            }
            else
            {
                Options.SetOption("rw_comm", rw_comm.Text);
                SiteManagerClass.review_summaries[SiteManagerClass.current] = "";
            }

            if (applyonlytotheproject2.Checked)
            {
                SiteManagerClass.rollback_summaries[SiteManagerClass.current]=rj_comm.Text;
            }
            else
            {
                Options.SetOption("rj_comm", rj_comm.Text);
                SiteManagerClass.rollback_summaries[SiteManagerClass.current] = "";
            }

            SiteManagerClass.SaveToSettings();

            Options.SetOption("pd_wait", pd_wait.Text);
            Options.SetOption("diff_style", diff_style.SelectedIndex == 0 ? "" : "_old");
            Options.SetOption("scroll_diff", auto_scroll.Checked ? 1 : 0);

            Options.SaveOptions();
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            this.Top = this.main_form.Top + this.main_form.Height / 2 - this.Height / 2;
            this.Left = this.main_form.Left + this.main_form.Width / 2 - this.Width / 2;

            Options.LoadOptions();
            LoadSettings();
        }

        private void def_Click(object sender, EventArgs e)
        {
            Options.FillDefault();
            LoadSettings();
        }

        private void save_Click(object sender, EventArgs e)
        {
            if (CheckSettings() == true)
            {
                SaveSettings();
                this.main_form.UpdateCurrentDiff();
                this.main_form.aw.dc.FullReset();
                this.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
