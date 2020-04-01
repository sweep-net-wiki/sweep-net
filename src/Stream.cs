using System;
using System.Collections.Generic;
using System.Globalization;
using System.Collections;


namespace sweepnet
{
    public static class StreamConfig
    {
        private static Hashtable h = new Hashtable();
        public static List<String> list = new List<string>();

        public static void LoadDefault(){
            h["mode"] = "1";//режим

            h["ns"] = "0|6|10|14";
            h["art"] = "1";//статьи
            h["redir"] = "1";//перенаправления
            h["max_diff"] = "0";
            h["dir"] = "";//0 - older, 1 - newer, [2 - auto]
            h["show_articles"] = "1";
            h["show_diffs"] = "1";

            // mode 0


            // mode 1
            h["smode_1"] = "0";//подрежим
            h["start_1"] = "";//стартовый timestamp
            h["category"] = "";//название категории

            //mode 2
            h["smode_2"] = "0";//подрежим
            h["start_2"] = "";//стартовый timestamp

            //mode 3

            //mode 4
            h["user"] = "";
            h["user_time"] = "";
        }

        public static void LoadOptions()
        {
            try
            {
                LoadDefault();
                Tools.StringsToHashTable(h, Properties.Settings.Default.config_n, Properties.Settings.Default.config_v, "|||||||||");
            }
            catch
            {
            }
        }
        public static void SaveOptions()
        {
            try
            {
                if (!Version.single)
                    return;
                String n = Tools._GetHashTableKeys(h, "|||||||||"), v = Tools._GetHashTableValues(h, "|||||||||");

                Properties.Settings.Default.config_n = n;
                Properties.Settings.Default.config_v = v;
                Properties.Settings.Default.Save();
            }
            catch
            {
            }
        }

        public static String GetOption(String name)
        {
            try
            {
                return (String)(h[name]);
            }
            catch { return ""; }
        }

        public static int GetOptionInt(String name)
        {
            try
            {
                return Convert.ToInt32((String)(h[name]));
            }
            catch { return int.MinValue; }
        }

        public static long GetOptionLong(String name)
        {
            try
            {
                return Convert.ToInt64((String)(h[name]));
            }
            catch { return long.MinValue; }
        }

        public static bool IsValidDate(String t)
        {
            try
            {
                DateTime dt = ParseDate(t);
                if (dt.Year < 2000 || dt.Year > DateTime.Now.Year + 1) { return false; }
                return true;
            }
            catch { return false; }
        }

        public static void SetOption(String name, String value)
        {
            h[name] = value;
        }
        public static void SetOption(String name, int value)
        {
            h[name] = value + "";
        }

        /*
         * Функции быстрого доступа
         * к параметрам
         * */

        public static int GetMode()
        {
            return GetOptionInt("mode");
        }

        public static WorkMode GetWMode()
        {
            switch (GetOptionInt("mode"))
            {
                case 0: return WorkMode.RecentChanges;
                case 1: return WorkMode.DatedArticles;
                case 2: return WorkMode.UnreviewedArticles;
                case 3: return WorkMode.List;
                case 4: return WorkMode.Contribution;
                default: return WorkMode.None;
            }
        }

        public static String GetNamespaces()
        {
            return GetOption("ns").Replace('.','|');
        }

        public static String GetParamRedir()
        {
            bool art = GetOptionInt("art")==1 ? true : false;
            bool redir = GetOptionInt("redir") == 1 ? true : false;
            if (art == true && redir == true)
            {
                return "all";
            }
            if (art == true && redir == false)
            {
                return "nonredirects";
            }
            return "redirects";
        }

        public static int GetParamMax()
        {
            int md = GetOptionInt("max_diff");
            if (md == 0) { return 100000000; }
            else { return md; }
        }

        public static string GetDirection()
        {
            return (GetOptionInt("dir") == 0 ? "older" : (GetOptionInt("dir") == 1?"newer":"auto"));
        }

        public static bool IsArticlesEnabled()
        {
            return GetOptionInt("show_articles") == 1;
        }

        public static bool IsDiffsEnabled()
        {
            return GetOptionInt("show_diffs") == 1;
        }

        public static String GetParamCategory()
        {
            return GetOption("category");
        }

        public static string GetStart_1()
        {
            return GetOption("start_1");
        }

        public static string GetStart_2()
        {
            return GetOption("start_2");
        }

        public static string GetStart_4()
        {
            return GetOption("user_time");
        }

        public static int GetSubmode_1()
        {
            return GetOptionInt("smode_1");
        }

        public static int GetSubmode_2()
        {
            return GetOptionInt("smode_2");
        }

        public static int GetSubmode_3()
        {
            return GetOptionInt("smode_3");
        }

        public static int GetSubmode_4()
        {
            return GetOptionInt("smode_4");
        }

        public static DateTime ParseDate(String t)
        {
            CultureInfo provider = CultureInfo.InvariantCulture;
            try
            {
                DateTime dt;
                if (t.Contains(" "))
                {
                    dt = DateTime.ParseExact(t, "dd.MM.yyyy HH:mm", provider);
                }
                else
                {
                    dt = DateTime.ParseExact(t, "dd.MM.yyyy", provider);
                }
                return dt;
            }
            catch { return DateTime.MinValue; }
        }

        public static DateTime GetDateFromMode1()
        {
            return ParseDate(GetOption("start_1"));
        }

        public static String GetUser()
        {
            return GetOption("user");
        }

        public static String GetParamArticle()
        {
            return GetOption("article");
        }

        public static DateTime GetDateFromMode4()
        {
            return ParseDate(GetOption("start_4"));
        }
    }

  
}
