using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace sweepnet
{
    public partial class StreamConf : Form
    {
        public MainForm main_form = null;

        public StreamConf()
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
                else if (ctl is RadioButton) ((RadioButton)ctl).Text = T(((RadioButton)ctl).Text);

            }
        }

        public void UpdateLanguage()
        {
            this.Text = T(this.Text);
            TranslateControls(savebutton, closebutton, to_prof);
            TranslateControls(label2, label5, label13, label14, label19,label10);

            string[] p = T("Main///File///Template///Category").Split(new string[] { "///" }, StringSplitOptions.None);
            ns0.Text = p[0]; ns6.Text = p[1]; ns10.Text = p[2]; ns14.Text = p[3];
            p = T("Articles///Redirects").Split(new string[] { "///" }, StringSplitOptions.None);
            articles.Text = p[0]; redirects.Text = p[1];
            p = T("Pages///Differences").Split(new string[] { "///" }, StringSplitOptions.None);
            show_articles.Text = p[0]; show_diffs.Text = p[1];
            p = T("Older///Newer///Auto").Split(new string[] { "///" }, StringSplitOptions.None);
            dir_old.Text = p[0]; dir_new.Text = p[1]; dir_auto.Text = p[2];


            p = T("Real-time changes///Pending pages///Unreviewed pages///Data from a list///User's contribution").Split(new string[] { "///" }, StringSplitOptions.None);
            label15.Text = p[0]; label7.Text = p[1]; label11.Text = p[2]; label12.Text = p[3]; label16.Text = p[4];

            TranslateControls(label20, mode0, mode2, mode4, mode1, mode3, mode5, label6, label8, label21,label1);
            TranslateControls(label4, m1_m0, m1_m1, label3);
            TranslateControls(label9);
            TranslateControls(label17, label22, m2_m0, m2_m1, label18);
        }


        private void StreamConf_Load(object sender, EventArgs e)
        {

            this.Top = this.main_form.Top + this.main_form.Height / 2 - this.Height / 2;
            this.Left = this.main_form.Left + this.main_form.Width / 2 - this.Width / 2;

            tabControl.SelectedTab=tabPage2;
            LoadConfig();
            SetValuesForMode();
        }



        public void CheckVisiblity()
        {
            if (mode0.Checked) { panel1.Enabled = false; dir_new.Checked = true; }
            else { panel1.Enabled = true; }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (!CheckMode(tabControl.SelectedIndex)) { return; }

                SaveConfig();

                main_form.Stop_Work();
                main_form.Start_Work();

                this.Hide();
            }
            catch (Exception ee) { MessageBox.Show(ee.Message); }
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            mode2.Checked = true;
        }

        private void next_mode_Click(object sender, EventArgs e)
        {
            tabControl.SelectedIndex = (tabControl.TabCount + tabControl.SelectedIndex + 1) % tabControl.TabCount;
        }

        private void prev_mode_Click(object sender, EventArgs e)
        {
            tabControl.SelectedIndex = (tabControl.TabCount + tabControl.SelectedIndex - 1) % tabControl.TabCount;
        }


        /*
         * 
         * 
         * */

        public void SetActivity(int mode)
        {
            switch (mode)
            {
                case 0://Свежие
                    ns_panel.Enabled = true;
                    type_panel.Enabled = false;
                    dsize_panel.Enabled = true;
                    panel_dir.Enabled = false;
                    break;
                case 1://Непроверенные правки
                    ns_panel.Enabled = true;
                    type_panel.Enabled = true;
                    dsize_panel.Enabled = true;
                    panel_dir.Enabled = true;
                    break;
                case 2: //Непроверенные статьи
                    ns_panel.Enabled = true;
                    type_panel.Enabled = true;
                    dsize_panel.Enabled = true;
                    panel_dir.Enabled = true;
                    break;
                case 3: //Список
                    ns_panel.Enabled = false;
                    type_panel.Enabled = false;
                    dsize_panel.Enabled = false;
                    panel_dir.Enabled = false;
                    break;
                case 4:
                    ns_panel.Enabled = true;
                    dsize_panel.Enabled = true;
                    panel_dir.Enabled = true;
                    type_panel.Enabled = false;
                    break;

            }
        }

        public void SetValuesForMode()
        {
            int mode = tabControl.SelectedIndex;
            SetActivity(mode);
            switch (mode)
            {
                case 0:
                    dir_new.Checked = true;
                    show_diffs.Checked = true;
                    show_articles.Checked = true;
                    work_panel.Enabled = true;
                    break;
                case 1:
                    show_diffs.Checked = true;
                    show_articles.Checked = false;
                    work_panel.Enabled = false;
                    if (mode0.Checked)
                    {
                        dir_old.Checked = true;
                        panel_dir.Enabled = false;
                    }
                    if (mode1.Checked)
                    {
                        dir_new.Checked = true;
                        panel_dir.Enabled = false;
                    }
                    if (mode4.Checked)
                    {
                        dir_auto.Checked = true;
                        panel_dir.Enabled = false;
                    }
                    break;
                case 2:
                    show_diffs.Checked = false;
                    show_articles.Checked = true;
                    panel_dir.Enabled = false;
                    work_panel.Enabled = false;
                    break;
                case 3:
                    show_diffs.Checked = true;
                    show_articles.Checked = true;
                    work_panel.Enabled = true;
                    break;
                case 4:
                     articles.Checked = true;
                     redirects.Checked = true;
                     show_articles.Checked = true;
                     show_diffs.Checked = true;
                     work_panel.Enabled = true;
                     dir_old.Checked = true;
                    break;
            }
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetValuesForMode();
        }

        public bool CheckStandart()
        {
            if (!ns0.Checked && !ns6.Checked && !ns10.Checked && !ns14.Checked) { MessageBox.Show(T("Choose at least one namespace!")); return false; } //Выберите хоть одно пространство!
            if (!articles.Checked && !redirects.Checked) { MessageBox.Show(T("Choose at least one page type!")); return false; } //Выберите хоть один тип страниц!
            if (!Tools.IsNumeric(maxdiff.Text) || maxdiff.Text.Equals("") || Convert.ToInt32(maxdiff.Text) < 0 || Convert.ToInt32(maxdiff.Text) > 1024 * 1024 * 256) { MessageBox.Show(T("Check the diff size!")); return false; } //
            if (!show_articles.Checked && !show_diffs.Checked) { MessageBox.Show(T("Choose what you want to work with!")); return false; } //Выберите то, с чем хотите работать!

            return true;
        }

        public bool CheckMode(int mode)
        {
            switch (mode)
            {
                case 0:
                    //return false;
                    return true;
                    break;
                case 1:
                    if (!mode0.Checked && !mode1.Checked && !mode2.Checked && !mode3.Checked && !mode4.Checked && !mode5.Checked) { MessageBox.Show(T("You haven't chosen a mode!")); return false; } //Не выбран ни один режим!

                    if (mode2.Checked && !StreamConfig.IsValidDate(textBox1.Text)) { MessageBox.Show(T("Incorrect date!")); return false; } //Неверно введена дата!
                    if (textBox2.Text.Contains("|")) { MessageBox.Show(T("Incorrect category!")); return false; } //Неверно введена категория!

                    break;
                case 2:
                    if (!m1_m0.Checked && !m1_m1.Checked) { MessageBox.Show(T("You haven't chosen a mode!")); return false; } //Не выбран ни один режим!
                    if (m1_m1.Checked && (textBox3.Text.Equals("") || textBox3.Text.Contains("|"))) { MessageBox.Show(T("Incorrect page name!")); return false; } //Неверно введена статья!

                    break;
                case 3:
                    int c = 0;
                    foreach (String tmp in list.Lines)
                    {
                        if (tmp.Trim().Length > 1) { c++; }
                    }
                    if (c < 1) { MessageBox.Show(T("The list is empty!")); return false; } //Список пуст!
                    break;
                case 4:
                    if (!m2_m0.Checked&&!m2_m1.Checked) { MessageBox.Show(T("You haven't chosen a mode!")); return false; } //Не выбран ни один режим!
                    if (textBox4.Text.Equals("") || textBox4.Text.Contains("|")) { MessageBox.Show(T("Incorrect user!")); return false; } //Неверно введен пользователь!
                    if (m2_m1.Checked && !StreamConfig.IsValidDate(textBox5.Text)) { MessageBox.Show(T("Incorrect date!")); return false; } //Неверно введена дата!
                    break;
            }
            return true;
        }

        public void LoadConfig()
        {
            int mode = StreamConfig.GetOptionInt("mode");

            tabControl.SelectedIndex = mode;
            // общие
            String[] ns;
            String ss = StreamConfig.GetOption("ns");
            ns = ss.Split('|');

            ns0.Checked = false; ns6.Checked = false; ns10.Checked = false; ns14.Checked = false;
            foreach (String s in ns)
            {
                switch (s)
                {
                    case "0": ns0.Checked = true; break;
                    case "6": ns6.Checked = true; break;
                    case "10": ns10.Checked = true; break;
                    case "14": ns14.Checked = true; break;
                }
            }
            articles.Checked = false;
            redirects.Checked = false;

            if (StreamConfig.GetOptionInt("redir") == 1) { redirects.Checked = true; }
            else { redirects.Checked = false; }
            if (StreamConfig.GetOptionInt("art") == 1) { articles.Checked = true; }
            else { redirects.Checked = false; }

            maxdiff.Text = StreamConfig.GetOption("max_diff");


            int dir = StreamConfig.GetOptionInt("dir");
            if (dir == 0) { dir_old.Checked = true; }
            else if (dir == 2) { dir_auto.Checked = true; }
            else { dir_new.Checked = true; }

            show_articles.Checked = StreamConfig.GetOptionInt("show_articles") == 1? true : false;
            show_diffs.Checked = StreamConfig.GetOptionInt("show_diffs") == 1 ? true : false;

            // режим 0


            // режим 1

            mode = StreamConfig.GetOptionInt("smode_1");

            if (mode == 0) { mode0.Checked = true; }
            else if (mode == 1) { mode1.Checked = true; }
            else if (mode == 2) { mode2.Checked = true; }
            else if (mode == 3) { mode3.Checked = true; }
            else if (mode == 4) { mode4.Checked = true; }
            else if (mode == 5) { mode5.Checked = true; }

            textBox1.Text = StreamConfig.GetOption("start_1");
            textBox2.Text = StreamConfig.GetOption("category");


            // режим 2

            mode = StreamConfig.GetOptionInt("smode_2");

            if (mode == 0) { m1_m0.Checked = true; }
            else if (mode == 1) { m1_m1.Checked = true; }

            textBox3.Text = StreamConfig.GetOption("start_2");


            // режим 3


            // режим 4

            mode = StreamConfig.GetOptionInt("smode_4");

            if (mode == 0) { m2_m0.Checked = true; }
            else if (mode == 1) { m2_m1.Checked = true; }

            textBox4.Text = StreamConfig.GetOption("user");
            textBox5.Text = StreamConfig.GetOption("user_time");
        }

        public void SaveConfig()
        {

            int mode = tabControl.SelectedIndex;

            // стандартные
            StreamConfig.SetOption("mode", mode);

            List<String> ns = new List<string>();
            if (ns0.Checked) { ns.Add("0"); }
            if (ns6.Checked) { ns.Add("6"); }
            if (ns10.Checked) { ns.Add("10"); }
            if (ns14.Checked) { ns.Add("14"); }
            StreamConfig.SetOption("ns", String.Join("|", ns.ToArray()));

            if (articles.Checked) { StreamConfig.SetOption("art", 1); }
            else { StreamConfig.SetOption("art", 0); }
            if (articles.Checked) { StreamConfig.SetOption("art", 1); }
            else { StreamConfig.SetOption("art", 0); }

            if (dir_old.Checked == true) { StreamConfig.SetOption("dir", 0); }
            else if (dir_auto.Checked == true) { StreamConfig.SetOption("dir", 2); }
            else { StreamConfig.SetOption("dir", 1); }

            StreamConfig.SetOption("max_diff", maxdiff.Text);

            if (show_articles.Checked) { StreamConfig.SetOption("show_articles", 1); }
            else { StreamConfig.SetOption("show_articles", 0); }

            if (show_diffs.Checked) { StreamConfig.SetOption("show_diffs", 1); }
            else { StreamConfig.SetOption("show_diffs", 0); }

            // сохраняем изменения в только открытом режиме

            switch (mode)
            {
                case 0:
                    break;
                case 1:
                    mode = -1;

                    if (mode0.Checked) { mode = 0; }
                    if (mode1.Checked) { mode = 1; }
                    if (mode2.Checked) { mode = 2; }
                    if (mode3.Checked) { mode = 3; }
                    if (mode4.Checked) { mode = 4; }
                    if (mode5.Checked) { mode = 5; }

                    StreamConfig.SetOption("smode_1", mode);

                    StreamConfig.SetOption("start_1", textBox1.Text);
                    StreamConfig.SetOption("category", textBox2.Text);
                    break;
                case 2:
                    mode = -1;

                    if (m1_m0.Checked) { mode = 0; }
                    if (m1_m1.Checked) { mode = 1; }

                    StreamConfig.SetOption("smode_2", mode);

                    StreamConfig.SetOption("start_2", textBox3.Text);
                    break;
                case 3:
                    StreamConfig.list.Clear();
                    foreach (String tmp in list.Lines)
                    {
                        if (tmp.Trim().Length > 1) { StreamConfig.list.Add(tmp.Trim()); }
                    }
                    break;
                case 4:
                    mode = -1;

                    if (m2_m0.Checked) { mode = 0; }
                    if (m2_m1.Checked) { mode = 1; }

                    StreamConfig.SetOption("smode_4", mode);
                    StreamConfig.SetOption("user", textBox4.Text);
                    StreamConfig.SetOption("user_time", textBox5.Text);
                    break;
            }
            StreamConfig.SaveOptions();

        }

        private void mode0_CheckedChanged(object sender, EventArgs e)
        {
            SetValuesForMode();
        }

        private void mode2_CheckedChanged(object sender, EventArgs e)
        {
            SetValuesForMode();
        }

        private void mode4_CheckedChanged(object sender, EventArgs e)
        {
            SetValuesForMode();
        }

        private void mode1_CheckedChanged(object sender, EventArgs e)
        {
            SetValuesForMode();
        }

        private void mode3_CheckedChanged(object sender, EventArgs e)
        {
            SetValuesForMode();
        }

        private void mode5_CheckedChanged(object sender, EventArgs e)
        {
            SetValuesForMode();
        }

        private void m1_m0_CheckedChanged(object sender, EventArgs e)
        {
            SetValuesForMode();
        }

        private void m1_m2_CheckedChanged(object sender, EventArgs e)
        {
            SetValuesForMode();
        }

        private void m1_m1_CheckedChanged(object sender, EventArgs e)
        {
            SetValuesForMode();
        }

        private void m1_m3_CheckedChanged(object sender, EventArgs e)
        {
            SetValuesForMode();
        }

        private void open_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    list.LoadFile(openFileDialog1.FileName);
                }
                catch { }
            }

        }

        private void save_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    list.SaveFile(saveFileDialog1.FileName);
                }
                catch { }
            }
        }

        private void tabPage5_Click(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void m2_m0_CheckedChanged_1(object sender, EventArgs e)
        {
            SetValuesForMode();
        }

        private void m2_m1_CheckedChanged_1(object sender, EventArgs e)
        {
            SetValuesForMode();
        }

        private void textBox5_TextChanged_1(object sender, EventArgs e)
        {
            m2_m1.Checked = true;
        }

        private void textBox1_Click_1(object sender, EventArgs e)
        {
            mode2.Checked = true;
        }

        private void textBox2_Click(object sender, EventArgs e)
        {
            mode5.Checked = true;
        }

        private void textBox3_Click(object sender, EventArgs e)
        {
            m1_m1.Checked = true;
        }

        private void textBox5_Click(object sender, EventArgs e)
        {
            m2_m1.Checked = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
