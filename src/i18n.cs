using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Xml;


namespace sweepnet
{
    public class Language
    {
        public String short_name { get; set; }
        public String med_name { get; set; }
        public String full_name { get; set; }
        public String translator { get; set; }

        public Language(String s, String m, String f, String t)
        {
            short_name = s;
            med_name = m;
            full_name = f;
            translator = t;
        }
    }

    public static class i18n
    {
        public static Hashtable xml_entries = new Hashtable();
        public static Hashtable xml_table = new Hashtable();
        public static String current_language_code;

        public static bool LoadLanguage(String short_code)
        {
            xml_entries.Clear();
            xml_table.Clear();
            String tmp = "";
            current_language_code = short_code;
            switch (short_code)
            {
                case "ru":
                    tmp = Properties.Resources.ru_i18n;
                    break;
                case "en":
                    tmp = Properties.Resources.en_i18n;
                    break;
                case "uk":
                    tmp = Properties.Resources.uk_i18n;
                    break;
            }
            if (tmp.Equals("")) { return false; }

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(tmp);

            foreach (XmlNode el in xmlDoc.DocumentElement.SelectNodes("entry"))
            {
                XmlNode a = el.SelectSingleNode("name");
                XmlNode b = el.SelectSingleNode("value");
                if (a != null && b != null && a.InnerText != "" && b.InnerText != "")
                {
                    a.InnerText = a.InnerText.Replace("\\n", "\n").Replace("\\r", "\r");
                    if (xml_entries.ContainsKey(a.InnerText)) continue;
                    xml_entries.Add(a.InnerText, b.InnerText.Replace("\\n", "\n").Replace("\\r", "\r"));
                }
            }
            foreach (XmlNode el in xmlDoc.DocumentElement.SelectNodes("phrase"))
            {
                XmlNode a = el.SelectSingleNode("orig");
                XmlNode b = el.SelectSingleNode("new");
                if (a != null && b != null && a.InnerText != "" && b.InnerText != "")
                {
                    a.InnerText = a.InnerText.Replace("\\n", "\n").Replace("\\r", "\r");
                    if (xml_table.ContainsKey(a.InnerText)) continue;
                    xml_table.Add(a.InnerText, b.InnerText.Replace("\\n", "\n").Replace("\\r", "\r"));
                }
            }

            return true;
        }

        public static string GetXml(string n)
        {
            if (xml_table.ContainsKey(n))
            {
                return xml_table[n].ToString();
            }
            return n;
        }

        public static string GetXmlEntry(string name)
        {
            if (xml_entries.ContainsKey(name))
            {
                return xml_entries[name].ToString();
            }
            return "";
        }

        public static Language[] GetLanguages()
        {
            List<Language> tmp = new List<Language>();

            tmp.Add(new Language("ru","Русский","Русский язык","[[ru:Haffman]]"));
            tmp.Add(new Language("en", "English", "English language", "[[ru:Haffman]]"));
            tmp.Add(new Language("uk", "Українська", "Українська мова", "[[ru:Base]]"));

            return tmp.ToArray();
        }

        public static string GetText(String name)
        {
            
            switch (current_language_code)
            {
                case "uk":
                    return Properties.Resources.ResourceManager.GetObject(name + "_uk_i18n").ToString();
                case "en":
                    return Properties.Resources.ResourceManager.GetObject(name + "_en_i18n").ToString();
                case "ru":
                default:
                    return Properties.Resources.ResourceManager.GetObject(name+"_ru_i18n").ToString();
            }
        }
    }
}
