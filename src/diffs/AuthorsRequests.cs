using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Threading;
using System.Xml;

namespace sweepnet
{
    public class AuthorsRequestResult
    {
        public List<AuthorsRequestResultItem> items { get; set; }
        public SweepNetPage page { get; set; }
        public long start { get; set; }
        public long end { get; set; }
        public bool ready { get; set; }
        public bool error { get; set; }
    }

    public class AuthorsRequestResultItem
    {
        public string user { get; set; }
        public string timestamp { get; set; }
        public int edits { get; set; }
        public bool banned { get; set; }
        public List<string> groups { get; set; }
    }

    public class AuthorsRequests
    {
        private CoreOperations aw = null;
        public Thread worker = null;
        public AuthorsRequestResult result = null;

        public AuthorsRequests(CoreOperations aw_)
        {
            aw = aw_;
        }

        public void RequestAuthors(SweepNetPage page, long start, long end)
        {
            if (aw == null) return;

            if (result != null && result.ready && result.page.title==page.title && start==result.start && end == result.end)
            {
                aw.events.Push("AuthorsReady"+start+"_"+end);
                return;
            }
            result = null;
            if (worker != null && worker.IsAlive)
            {
                try
                {
                    worker.Abort();
                    worker.Join();
                }
                catch { }
            }

            result = new AuthorsRequestResult();
            result.ready = false;
            result.page = page;
            result.start = start;
            result.end = end;
            result.error = false;

            worker = new Thread(MyThreadFunction);
            worker.IsBackground = true;
            worker.Start();
        }

        public List<AuthorsRequestResultItem> _Work(SweepNetPage page, long start_id, long end_id)
        {
            try
            {
                List<AuthorsRequestResultItem> result = new List<AuthorsRequestResultItem>();

                Hashtable h = page.GetHashtableofcontributors(start_id+1, end_id);
                List<string> names = new List<string>();
                foreach (String k in h.Keys)
                {
                    names.Add(Tools.UrlEncode(k));
                }

                String data = aw.HTTP_W_REQUEST("api.php?format=xml&action=query&list=users&ususers=" + String.Join("|", names.ToArray()) + "&usprop=groups|editcount|registration|blockinfo");

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(data);
                XmlNode currentNode;
                currentNode = xmlDoc.DocumentElement.SelectSingleNode("query");
                if (currentNode == null) { return null; }
                currentNode = currentNode.SelectSingleNode("users");
                if (currentNode == null) { return null; }
                foreach (XmlNode el in currentNode.SelectNodes("user"))
                {
                    AuthorsRequestResultItem tmp = new AuthorsRequestResultItem();
                    tmp.user = el.Attributes.GetNamedItem("name").Value;
                    tmp.edits = Convert.ToInt32(el.Attributes.GetNamedItem("editcount").Value);
                    tmp.timestamp = el.Attributes.GetNamedItem("registration").Value;
                    tmp.banned = el.Attributes.GetNamedItem("blockid") != null;
                    tmp.groups = new List<string>();

                    XmlNode j = el.SelectSingleNode("groups");
                    if (j != null)
                    {
                        foreach (XmlNode v in j.SelectNodes("g"))
                        {
                            if (v.InnerText == "*") continue;
                            tmp.groups.Add(v.InnerText);
                        }
                    }

                    result.Add(tmp);
                }
                return result;

            }
            catch
            {
                return null;
            }
        }

        public void MyThreadFunction()
        {
            if (aw == null) return;
            if (result == null || result.ready) return;

            result.items = _Work(result.page, result.start, result.end);
            if (result.items == null || result.items.Count > 0)
            {
                if (result.items == null) result.error = true;
                result.ready = true;
                aw.events.Push("AuthorsReady" + result.start + "_" + result.end);
            }
        }

        public static string MakeUpTable(AuthorsRequestResult res)
        {
            if (res.error) return "<br />" + ("Error. Check, may be the pending changes have been made only by anonymous editors.").T();
            string tmp = "<table class=\"myt\" style=\"margin-top:10px;margin-bottom:10px;\"><tr><td>User</td><td>Edits</td><td>Registration</td><td>Rights</td><td>Banned</td></tr>";
            foreach (AuthorsRequestResultItem item in res.items)
            {
                tmp += "<tr><td>"+Tools.HtmlEncode(item.user)+"</td><td>"+item.edits+"</td><td>"+item.timestamp.Substring(0,10)+"</td><td>"+
                    string.Join(", ",item.groups.ToArray())+"</td><td>"+(item.banned?"Yes":"No")+"</td></tr>";
            }
            return tmp + "</table>";
        }

    }
}
