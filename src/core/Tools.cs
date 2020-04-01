using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Web;
using System.Xml;

namespace System.Runtime.CompilerServices
{
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class
         | AttributeTargets.Method)]
    public sealed class ExtensionAttribute : Attribute { }
}

namespace sweepnet
{
    public static class Tools
    {
        static System.Random rand_gen = new System.Random();
        static MD5 md5Hasher = MD5.Create();
        static string ipv6_match = "(([0-9a-fA-F]{1,4}:){7,7}[0-9a-fA-F]{1,4}|([0-9a-fA-F]{1,4}:){1,7}:|([0-9a-fA-F]{1,4}:){1,6}:[0-9a-fA-F]{1,4}|([0-9a-fA-F]{1,4}:){1,5}(:[0-9a-fA-F]{1,4}){1,2}|([0-9a-fA-F]{1,4}:){1,4}(:[0-9a-fA-F]{1,4}){1,3}|([0-9a-fA-F]{1,4}:){1,3}(:[0-9a-fA-F]{1,4}){1,4}|([0-9a-fA-F]{1,4}:){1,2}(:[0-9a-fA-F]{1,4}){1,5}|[0-9a-fA-F]{1,4}:((:[0-9a-fA-F]{1,4}){1,6})|:((:[0-9a-fA-F]{1,4}){1,7}|:)|fe80:(:[0-9a-fA-F]{0,4}){0,4}%[0-9a-zA-Z]{1,}|::(ffff(:0{1,4}){0,1}:){0,1}((25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9]).){3,3}(25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])|([0-9a-fA-F]{1,4}:){1,4}:((25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9]).){3,3}(25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9]))";
        static string ipv4_match = @"^([1-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])(\.([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])){3}$";
        static Regex ipv4_reg = new Regex(ipv4_match, RegexOptions.Singleline | RegexOptions.ExplicitCapture);
        static Regex ipv6_reg = new Regex(ipv6_match, RegexOptions.Singleline | RegexOptions.ExplicitCapture);
        static Regex _htmlRegex = new Regex(@"<(.|\n)*?>", RegexOptions.Compiled);
        public static Hashtable name_to_namespace = new Hashtable();

        public static bool IsValidIPAddress(string ipAddr)
        {
            if (ipv4_reg.IsMatch(ipAddr)) return true;
            return ipv6_reg.IsMatch(ipAddr);
        }

        public static string StripHtmlTags(string source)
        {
            return _htmlRegex.Replace(source, string.Empty);
        }

        public static String BytesCountToString(long t)
        {
            if (t < 1024) { return t + " B"; }
            if (t < 1024 * 1024) { return (int)(t / 1024) + " KB"; }
            if (t < 1024 * 1024 * 1024) { return (int)(t / 1024 / 1024) + " MB"; }
            return (int)(t / 1024 / 1024 / 1024) + " GB";
        }

        public static String GetColoredListOfUsers(Hashtable h)
        {
            List<String> list = new List<string>();
            foreach (String k in h.Keys)
            {
                String v = h[k].ToString();
                if (IsValidIPAddress(k))
                {
                    v = "<span style=\"background: #d8f5ff\">" + k + "</span> (" + v + ")";
                }
                else
                {
                    v = "<span style=\"background: #d8fff1\">" + k + "</span> (" + v + ")";
                }
                list.Add(v);
            }
            return String.Join(", ", list.ToArray());
        }

        public static bool IsNumeric(string text)
        {
            if (text == null || text.Equals("")) { return false; }
            Regex pt = new Regex("[^0-9]");
            return !pt.IsMatch(text);
        }

        public static void HashTableToStrings(Hashtable s, String n, String v)
        {
            n = _GetHashTableKeys(s);
            v = _GetHashTableValues(s);
        }

        public static String _GetHashTableKeys(Hashtable s, string joiner = "|")
        {
            List<String> list_n = new List<string>();
            foreach (String j in s.Keys) { list_n.Add(j); }
            String n = String.Join(joiner, list_n.ToArray());
            return n;
        }

        public static String _GetHashTableValues(Hashtable s, string joiner = "|")
        {
            List<String> list_v = new List<string>();
            foreach (String j in s.Values) { list_v.Add(j); }
            String v = String.Join(joiner, list_v.ToArray());
            return v;
        }

        public static void StringsToHashTable(Hashtable h, String n, String v, String spliter = "|")
        {
            spliter = spliter.Replace("|", "" + '\\' + '|');
            String[] names = Regex.Split(n, spliter);
            String[] values = Regex.Split(v, spliter);

            for (int a = 0; a < names.Length; a++)
            {
                h[names[a]] = values[a];
            }
        }

        public static String ConvertTimestampToDate(String d)
        {
            return DateTime.Parse(d).ToString("dd.MM.yyyy HH:mm:ss");
        }

        public static DateTime GetRandomDate(DateTime mind, DateTime maxd)
        {
            DateTime ret = DateTime.MinValue;
            try
            {
                if ((maxd - mind).Days < 4) { return mind; }

                Random rand = new Random();

                return mind.AddDays(1 + rand.NextDouble() * ((maxd - mind).Days - 1));
            }
            catch (Exception) { return ret; }
        }

        public static double RandomDouble()
        {
            return rand_gen.NextDouble();
        }

        public static double FullRandom()
        {
            return RandomDouble() + DateTime.Now.Millisecond + DateTime.Now.Second * 1000;
        }

        public static string GetMD5(string input)
        {
            lock (md5Hasher)
            {
                byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));
                StringBuilder tmp = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    tmp.Append(data[i].ToString("x2"));
                }
                return tmp.ToString();
            }
        }

        public static string xor(string text, int key = 55)
        {
            StringBuilder ins = new StringBuilder(text);
            StringBuilder outs = new StringBuilder(text.Length);
            for (int a = 0; a < ins.Length; a++)
            {
                outs.Append((char)(ins[a] ^ key));
            }
            return outs.ToString();
        }


        public static string ReplaceFirst(string text, string search, string replace, out bool been_replaced)
        {
            int pos = text.IndexOf(search);
            if (pos < 0)
            {
                been_replaced = false;
                return text;
            }
            been_replaced = true;
            return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
        }

        public static string ReplaceFirst(string text, string search, string replace)
        {
            int pos = text.IndexOf(search);
            if (pos < 0)
            {
                return text;
            }
            return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
        }

        public static string ReplaceLast(string text, string search, string replace, out bool been_replaced)
        {
            int pos = text.LastIndexOf(search);
            if (pos < 0)
            {
                been_replaced = false;
                return text;
            }
            been_replaced = true;
            return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
        }

        public static string ReplaceLast(string text, string search, string replace)
        {
            int pos = text.LastIndexOf(search);
            if (pos < 0)
            {
                return text;
            }
            return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
        }

        public static string UrlEncode(string s)
        {
            return HttpUtility.UrlEncode(s);
        }

        public static string HtmlEncode(string s)
        {
            return HttpUtility.HtmlEncode(s);
        }

        public static string ReplaceBetweenStrings(string src, string left, string right, string what)
        {
            int pos = src.IndexOf(left);
            if (pos < 0) return src;
            string tmp = src.Substring(0, pos + left.Length);
            tmp += what;
            pos = src.IndexOf(right);
            if (pos < 0) return src;
            return tmp + src.Substring(pos);
        }


        public static String T(this String str)
        {
            return i18n.GetXml(str);
        }

        public static string HashtableToXml(Hashtable h)
        {
            string data = "";
            foreach (string key in h.Keys)
            {
                data += "<entry><key>" + HtmlEncode(key) + "</key><value>" + HtmlEncode(h[key].ToString()) + "</value></entry>";
            }

            return @"<?xml version=""1.0"" encoding=""utf-8"" ?><root>" + data + "</root>";
        }

        public static Hashtable XmlToHashtable(string xml)
        {
            Hashtable result = new Hashtable();
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);
                foreach (XmlNode node in doc.DocumentElement.SelectNodes("entry"))
                {
                    XmlNode key = node.SelectSingleNode("key");
                    XmlNode value = node.SelectSingleNode("value");
                    if (key != null && value != null)
                    {
                        result[key.InnerText] = value.InnerText;
                    }
                }
            }
            catch { }
            return result;
        }
    }

    public class MyStack<T>
    {
        public List<T> items = new List<T>();

        public void Push(T item)
        {
            items.Add(item);
        }

        public T Pop()
        {
            if (items.Count > 0)
            {
                T temp = items[items.Count - 1];
                items.Remove(temp);
                return temp;
            }
            else
                return default(T);
        }

        public void Remove(int itemAtPosition)
        {
            items.RemoveAt(itemAtPosition);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return (IEnumerator<T>)items.GetEnumerator();
        }

        public int Count
        {
            get
            {
                return items.Count;
            }
        }
    }

    public static class EditTokenCache
    {

        public static string token
        {
            get
            {
                if (DateTime.Now.Ticks / 10000000 - valid > 290)
                {
                    valid = 0;
                    _token = "";
                }
                return _token;
            }
            set
            {
                _token = value;
                valid = DateTime.Now.Ticks / 10000000;
            }
        }

        private static string _token = "";
        private static long valid = DateTime.Now.Ticks / 10000000;
    }

    public static class RevAssocCache
    {
        static Dictionary<long, String> data = new Dictionary<long, string>();
        public static void Add(long revid, String title)
        {
            data[revid] = title;
        }
        public static String OnceCheck(long revid)
        {
            if (data.ContainsKey(revid))
            {
                String r = data[revid];
                data.Remove(revid);
                return r;
            }
            else { return ""; }
        }
        public static String OnceCheck(String revid)
        {
            return OnceCheck(Int64.Parse(revid));
        }
    }
}
