using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
/*
    http://als.wikipedia.org/w/|http://ar.wikipedia.org/w/|http://be.wikipedia.org/w/|http://bs.wikipedia.org/w/|http://de.wikipedia.org/w/|http://de.wikiquote.org/w/|http://eo.wikipedia.org/w/|http://hu.wikipedia.org/w/|http://ia.wikipedia.org/w/|http://id.wikipedia.org/w/|http://mk.wikipedia.org/w/|http://pl.wikipedia.org/w/|http://pl.wikisource.org/w/|http://ru.wikipedia.org/w/|http://ru.wikiquote.org/w/|http://ru.wikisource.org/w/|http://sq.wikipedia.org/w/|http://tr.wikipedia.org/w/|http://zh-classical.wikipedia.org/w/
    als.wiki|ar.wiki|be.wiki|bs.wiki|de.wiki|de.wikiquote.org|eo.wiki|hu.wiki|ia.wiki|id.wiki|mk.wiki|pl.wiki|pl.wikisource.org|ru.wiki|ru.wikiquote.org|ru.wikisource.org|sq.wiki|tr.wiki|zh-classical.wiki
 */
namespace sweepnet
{
    public partial class AuthForm : Form
    {
        public enum AuthFormStatus
        {
            PendingInput,
            Completed,
            Cancelled
        }

        public AuthFormStatus status = AuthFormStatus.PendingInput;
        public String login = "";
        public String password = "";
        public String domain = "";

        public Language[] l_list;

        public AuthForm()
        {
            InitializeComponent();
            this.Text = "About".T();
        }

         private void SetIfSaved()
         {
             if (!Properties.Settings.Default.i1.Equals("")) { 
                 this.login_textbox.Text = Tools.xor(Properties.Settings.Default.i1);
                 this.password_textbox.Text = Tools.xor(Properties.Settings.Default.i2);
             }
         }


        private void Auth_Load(object sender, EventArgs e)
        {
            version_label.Text = Version.GetName() + " " + Version.GetVersion();
            version_label.Left = this.Width / 2 - version_label.Width / 2 - 15;
            save_pass.Checked = Options.GetOptionInt("save_pass") == 1 ? true : false;

            SetIfSaved();

            l_list = i18n.GetLanguages();
            langbox.Items.Clear();
            string qq = Options.GetOption("language"); int i=0;
            foreach(Language l in l_list){
                langbox.Items.Add(l.med_name);
                if (l.short_name == qq) i = langbox.Items.Count - 1;   
            }
            
            i18n.LoadLanguage(Options.GetOption("language"));
            UpdateLanguage();
            langbox.SelectedIndex = i;

            SiteManagerClass.LoadFromSettings();
            foreach (DictionaryEntry dd in SiteManagerClass.table)
            {
                project_combo.Items.Add(dd.Key);
            }

            project_combo.Sorted = true;

            int pos=0;
            string last = Options.GetOption("wiki");
            foreach (String s in project_combo.Items)
            {
                if (s.Equals(last))
                {
                    project_combo.SelectedIndex = pos; break;
                }
                pos++;
            }
            langbox.SelectedItem = i;
        }

        private void authorize_button_click(object sender, EventArgs e)
        {
            login = this.login_textbox.Text;
            password = this.password_textbox.Text;
            bool savePasswordChecked = this.save_pass.Checked;

            if (project_combo.SelectedIndex == -1) { 
                MessageBox.Show(T("Choose project!"));
                return; 
            }

            domain = project_combo.Items[project_combo.SelectedIndex].ToString(); string wikin = domain;
            SiteManagerClass.current = domain;
            foreach (DictionaryEntry dd in SiteManagerClass.table)
            {
                if (dd.Key.Equals(domain)) { domain = dd.Value.ToString(); }
            }

            if (login.Equals("") || password.Equals(""))
            {
                MessageBox.Show(T("Check your login and password!"));
                return;
            } 

            if (Options.GetOptionInt(Options.SAVE_PASS) != (savePasswordChecked ? 1 : 0))
            {
                Options.SetOption(Options.SAVE_PASS, (savePasswordChecked ? 1 : 0) + "");
            }

            if (Version.single)
            {
                if (savePasswordChecked) { Properties.Settings.Default.i1 = Tools.xor(login_textbox.Text); Properties.Settings.Default.i2 = Tools.xor(password_textbox.Text); Properties.Settings.Default.Save(); }
                else
                {
                    Properties.Settings.Default.i1 = Tools.xor(login_textbox.Text);
                    Properties.Settings.Default.i2 = "";
                }
            }
            Options.SetOption("wiki", wikin);
            status = AuthFormStatus.Completed;
            Options.SaveOptions();
            this.Visible = false;
            this.Close();
        }

        private void Auth_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (status != AuthFormStatus.Completed)
            {
                status = AuthFormStatus.Cancelled;
                Application.Exit();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            domain = (String)(project_combo.Items[project_combo.SelectedIndex]);
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13) {
                authorize_button_click(sender, e);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new SiteManager().ShowDialog();
            String selname = "";
            if (project_combo.SelectedIndex != -1)
            {
                if (!project_combo.Items[project_combo.SelectedIndex].Equals("")) { selname = project_combo.Items[project_combo.SelectedIndex].ToString(); }
            }
            
            SiteManagerClass.LoadFromSettings();
            project_combo.Items.Clear();
            foreach (DictionaryEntry dd in SiteManagerClass.table)
            {
                project_combo.Items.Add(dd.Key);
            }

            project_combo.Sorted = true;
            if (!selname.Equals(""))
            {
                int pos = 0;
                foreach (String s in project_combo.Items)
                {
                    if (s.Equals(selname)) { project_combo.SelectedIndex = pos; break; }
                    pos++;
                }
            }
        }

        public void CheckLanguage()
        {
            if (langbox.SelectedIndex != -1)
            {
                String lc = l_list[langbox.SelectedIndex].short_name;
                Options.SetOption("language", lc);
                i18n.LoadLanguage(lc);
            }
        }


        public static string T(string n) { return n.T(); }
        public void UpdateLanguage()
        {
            this.Text = T("Authorization");
            invitation_label.Text = T("Enter your login and password");
            language_label.Text = "Language"+(T("Language")=="Language"?"":(" / "+T("Language")));
            save_pass.Text = T("Remember password");
            authorize_button.Text = T("Authorize");
        }

        private void langbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckLanguage();
            UpdateLanguage();
        }
    }
}
