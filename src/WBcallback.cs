using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Threading;



namespace sweepnet
{
    static class WBcallback
    {
        static Regex _htmlRegex = new Regex("<.*?>", RegexOptions.Compiled);
        static Regex _linkRegex = new Regex(@"\[\[([^]]*)\]\]", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public static string rfo(string original, string oldValue, string newValue, int occ = 0)
        {
            if (String.IsNullOrEmpty(original))
                return String.Empty;
            if (String.IsNullOrEmpty(oldValue))
                return original;
            if (String.IsNullOrEmpty(newValue))
                newValue = String.Empty;
            int loc = 0;
            for (int a = occ; a >= 0; a--)
            {
                loc = original.IndexOf(oldValue, loc);
            }

            return original.Remove(loc, oldValue.Length).Insert(loc, newValue);
        }

        public static String ProcessPage(String text)
        {
            String ret = text + "";
            Regex r = _linkRegex;

            Match m = r.Match(text);
            int tmp = 1;

            List<String> l = new List<string>();

            while (m.Success)
            {
                String t = m.Groups[0].Value;
                bool cs = t.Contains("|");
                if (cs) { t = t.Substring(0, t.IndexOf('|')); }


                int c = 0;
                for (int a = 0; a < l.Count; a++) { if (l[a].Equals(t)) { c++; } }
                l.Add(t);
                if (!cs && t.IndexOf(':') != -1 && t.IndexOf(':') < 4) { continue; }
                ret = rfo(ret, t, "[[<a id=\"id" + tmp + "\" style=\"text-decoration:none;font-weight:normal;color:#0645AD\" href=\"process_id_" + tmp + "\" title=\"" + "Check the existence".T() + "\">" + t.Replace("[[", "") + "</a>", c);

                m = m.NextMatch();
                tmp++;
            }

            return ret.Replace("]]</a>", "</a>]]");
        }

        public static bool CheckVisbleChanges(String data)
        {
            if (!data.Contains("class=\"diff-context\"")) { return true; }
            if (data.Contains("<span class=\"diffchange\">")) { return true; }
            if (data.Contains("<td class=\"diff-addedline\">")) { return true; }
            if (data.Contains("<td class=\"diff-deletedline\">")) { return true; }
            return false;
        }

        public static string StripTags(string source)
        {
            return _htmlRegex.Replace(source, string.Empty);
        }

        public static void ProcessLink(CoreOperations aw, string title, HtmlElement html_element = null)
        {
            String text = title;//StripTags(he.InnerText).Trim();

            bool file = false;
            if (title.Contains(":"))
            {
                string j = title.Substring(0, title.IndexOf(":")).ToLower();
                if (Tools.name_to_namespace.ContainsKey(j) && Tools.name_to_namespace[j].Equals("6"))
                {
                    file = true;
                }
            }

            String data = "";
            if (file) data = aw.HttpGetApi("api.php?format=xml&action=query&prop=info&prop=imageinfo&titles=" + text.Replace(' ', '_'));
            else data = aw.HttpGetApi("api.php?format=xml&action=query&prop=info&titles=" + text.Replace(' ', '_'));
            bool ok = !data.Contains("imagerepository=\"\"");

            if (html_element != null)
            {
                if (ok) { html_element.Style = "color:green;"; }
                else { html_element.Style = "color:red;"; }
            }
            if (file)
            {
                if (ok) { MessageBox.Show(String.Format(@"file ""{0}"" exists".T(), text), "Result".T()); } //Файл; существует
                else { MessageBox.Show(String.Format(@"file ""{0}"" does not exist".T(), text), "Result".T()); } //Файл; не существует
            }
            else
            {
                if (ok) { MessageBox.Show(String.Format(@"page ""{0}"" exists".T(), text), "Result".T()); } //Страница; существует
                else { MessageBox.Show(String.Format(@"page ""{0}"" does not exist".T(), text), "Result".T()); } //Страница; не существует
            }
        }
        public static void ProcessLinkThreaded(CoreOperations aw, string title, HtmlElement he = null)
        {
            Thread myThread = new System.Threading.Thread(new System.Threading.ThreadStart(delegate ()
            {
                ProcessLink(aw, title, he);
            }));
            myThread.IsBackground = true;
            myThread.Start();
        }

        public static void ProcessClick(WebBrowser wb, CoreOperations aw, String id)
        {
            HtmlElement he = wb.Document.GetElementById("id" + id);
            Thread myThread = new System.Threading.Thread(new System.Threading.ThreadStart(delegate ()
            {
                ProcessLink(aw, StripTags(he.InnerText).Trim(), he);
            }));
            myThread.IsBackground = true;
            myThread.Start();
        }
    }
}
