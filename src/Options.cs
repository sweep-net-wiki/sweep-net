using System;
using System.Collections;

namespace sweepnet
{
    static class Options
    {

        public static string SAVE_PASS = "save_pass";
        private static Hashtable option_values = new Hashtable();

        public static void FillDefault()
        {
            option_values[SAVE_PASS] = "1";//сохранять пароль
            option_values["buff1"] = "30";//стандартный
            option_values["buff2"] = "15";//свежие
            option_values["buff3"] = "7";//диффы
            option_values["skip_null"] = "0";//пропускать с нулевыми изменениями
            option_values["check_updates"] = "1";//проверять обновления
            option_values["recent_count"] = "30";//сколькими правкам оперировать в режиме свежих
            option_values["check_links"] = "1";//предоставлять интерфейс проверки ссылок на существование
            option_values["rw_comm"] = "";//пользовательский комментарий к патрулированию
            option_values["rj_comm"] = "";//пользовательский комментарий к отклонению
            option_values["pd_wait"] = "2500";//задержка после выбора границ диффа
            option_values["diff_style"] = "";//стиль диффа
            option_values["scroll_diff"] = "1";//прокручивать дифф
            option_values["mode"] = "2";

            option_values["language"] = "ru";
            option_values["wiki"] = "ru.wiki";

            option_values["check_commons"] = "1";// проверять ли файлы на commons
        }

        public static void LoadOptions()
        {
            FillDefault();

            String opt_n = Properties.Settings.Default.options_n;
            String opt_v = Properties.Settings.Default.options_v;

            Tools.StringsToHashTable(option_values, opt_n, opt_v);
        }

        public static void SaveOptions()
        {
            if (!Version.single)
                return;
            String n = Tools._GetHashTableKeys(option_values), v = Tools._GetHashTableValues(option_values);

            Properties.Settings.Default.options_n = n;
            Properties.Settings.Default.options_v = v;
            Properties.Settings.Default.Save();
        }

        public static String GetOption(String name)
        {
            try
            {
                return (String)(option_values[name]);
            }
            catch { return ""; }
        }

        public static int GetOptionInt(String name)
        {
            try
            {
                return Convert.ToInt32((String)(option_values[name]));
            }
            catch { return int.MinValue; }
        }

        public static long GetOptionLong(String name)
        {
            try
            {
                return Convert.ToInt64((String)(option_values[name]));
            }
            catch { return long.MinValue; }
        }

        public static void SetOption(String name, String value)
        {
            option_values[name] = value;
        }
        public static void SetOption(String name, int value)
        {
            option_values[name] = value+"";
        }
     }
}
