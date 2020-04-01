using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace sweepnet
{
    public partial class SiteManager : Form
    {

        public static string T(string s) { return s.T(); }
        public SiteManager()
        {
            InitializeComponent();

            this.Text = T("List of projects");
            savebutton.Text = T("Save");
            cancelbutton.Text = T("Close");
            this.удалитьВыделеннуюСтрокуToolStripMenuItem.Text = T("Remove the entry");
            this.apiurl.HeaderText = T("URL for API requests (example)");
            this.apiurl.ToolTipText = "http://ru.wikipedia.org/";
            this.sitename.HeaderText = T("Name (to be shown in the list)");
        }

        private void SiteManager_Load(object sender, EventArgs e)
        {
            SiteManagerClass.LoadFromSettings();
            SiteManagerClass.LoadToGrid(dataGridView1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<String> tmp = new List<String>();
            for (int row = 0; row < dataGridView1.Rows.Count; row++)
            {
                try
                {
                    tmp.Add(dataGridView1.Rows[row].Cells[1].Value.ToString());
                }
                catch { }
            }

                 List<string> vals = new List<string>();

                 foreach (string s in tmp)
                 {
                     if (vals.Contains(s))
                     {
                         MessageBox.Show(T("There must be no duplicated names!"));//Названия не должны повторяться!
                         return;
                     }
                     vals.Add(s);
                 }




            SiteManagerClass.LoadFromGrid(dataGridView1);
            SiteManagerClass.SaveToSettings();
            this.Close();
        }

        private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    row.Selected = false;
                }
                dataGridView1.CurrentCell = dataGridView1[e.ColumnIndex, e.RowIndex];
            }
            
        }

        private void удалитьВыделеннуюСтрокуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(T("Do you really want to remove this entry?"), T("Warning"), //Вы действительно хотите удалить эту запись?
                                            MessageBoxButtons.YesNo,
                                            MessageBoxIcon.Question);
             if (result == DialogResult.Yes)
             {
                 dataGridView1.Rows.RemoveAt(dataGridView1.CurrentRow.Index);
             }
        }
    }

    public static class SiteManagerClass
    {
        public static Hashtable table = new Hashtable();

        public static Hashtable review_summaries = new Hashtable();
        public static Hashtable rollback_summaries = new Hashtable();

        public static string current = "";

        public static void LoadFromSettings()
        {
            Tools.StringsToHashTable(table, Properties.Settings.Default.add_names3, Properties.Settings.Default.add_url3);
            review_summaries = Tools.XmlToHashtable(Properties.Settings.Default.add_customreviewsummaries);
            rollback_summaries = Tools.XmlToHashtable(Properties.Settings.Default.add_customrollbacksummaries);
        }

        public static void SaveToSettings()
        {
            if (!Version.single) return;
            String n = Tools._GetHashTableKeys(table), v = Tools._GetHashTableValues(table);
            Properties.Settings.Default.add_names3 = n;
            Properties.Settings.Default.add_url3 = v;
            Properties.Settings.Default.add_customreviewsummaries = Tools.HashtableToXml(review_summaries);
            Properties.Settings.Default.add_customrollbacksummaries = Tools.HashtableToXml(rollback_summaries);
            Properties.Settings.Default.Save();
        }
        
        public static void LoadToGrid(DataGridView d)
        {
            d.Rows.Clear();
            foreach (DictionaryEntry dd in table)
            {
                d.Rows.Add(dd.Value, dd.Key);
            }
        }

        public static void LoadFromGrid(DataGridView d)
        {
            table.Clear();
            foreach (DataGridViewRow dd in d.Rows)
            {
                try
                {
                    table[dd.Cells[1].Value.ToString()] = dd.Cells[0].Value.ToString();
                }
                catch { }
            }
        }
    }
}
