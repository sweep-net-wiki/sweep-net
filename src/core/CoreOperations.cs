using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using DotNetWikiBot;
using System.Xml;
using System.Web;
using System.Diagnostics;
using System.Net;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows.Forms;


namespace sweepnet
{
    public class CoreOperations
    {
        public Site mysite;
        public Stack<String> events = new Stack<string>();
        public ElementSet list;
        public Thread CurrUpdThread;
        private String contin = null; private DateTime mind=DateTime.MinValue, maxd=DateTime.MinValue;//дополнительные временные паременные для режимов, требующих выборку списком
        public bool updating = false;
        public DiffCache dc;
        public RevCache rrc;

        public String tmp_dir = "";

        public int wc=0, pc=0, rc=0;// для статистики

        public MyStack<Element> skipped = new MyStack<Element>();

        public int count_p = 35;

        public String domain = "";

        public CoreOperations()
        {
            list = new ElementSet();
            dc = new DiffCache(this);
            rrc = new RevCache(this);
        }

        public int GetCountOfAlailableData()
        {
            return list.Count;
        }

        public String HttpGet(String url)
        {
            return mysite.GetPageHTM(url);
        }

        public String HttpGetApi(String nonapiurl)
        {
            return HttpGet(domain + mysite.indexPath + nonapiurl);
        }

        public Element GetRevision()
        {
            wc++;

            int m = StreamConfig.GetMode();

            if (list.Count == 0)
            {
                if (m == 0) { }
                else if (m == 3) { NewEvent("nodata_fin"); return null; }
                else { Update(false, true, false); }
            }
            Element tr = null;

            for (int i = 0; i < list.Count; i++)
            {
                tr = list[i];
                list.Remove2(i);
                break;
            }
             
            

            if (list.Count < count_p / 4 && list.Count > 0 && (m==1||m==2||m==4)) { Update(true, false, true); } // true true true


            for (int a = 0; a < skipped.items.Count; a++)
            {
                if (skipped.items[a].title.Equals(tr.title)) { skipped.items.RemoveAt(a); break; }
            }

            if (tr == null && m == 3) { NewEvent("nodata_fin"); }

            skipped.Push(tr);
            return tr;
        }

        public Element GoBack()
        {
            if (skipped.Count < 2) { return null; }

            try
            {
                bool w = false;
                for (int a = skipped.Count-2; a >= 0; a--)
                {
                    if (skipped.items[a].skipped == false) { w = true; break; }
                }
                if (w == false) { return null; }

                Element rev2=null;
                Element rev=null;

                for (int a = skipped.Count - 1; a >= 0; a--)
                {
                    rev2 = skipped.Pop();
                    rev = skipped.Pop();
                    list.Insert(0, rev2);
                    skipped.Push(rev);

                    if (rev.skipped == false) { break; }
                }

                if (rev == null || rev2 == null) { return null; }

                //TElement rev2 = skipped.Pop();
               // TElement rev = skipped.Pop();
                //list.Insert(0, rev2);

               // skipped.Push(rev);

                this.dc.AddRev_first(rev2, false);
                this.dc.AddRev_first(rev, true);

                return rev;
            }
            catch (Exception e) { MessageBox.Show(e.Message); }
            return null;
        }

        public void ScrollTo(Element el)
        {
            lock (list.list)
            {
                foreach (Element e in list.list)
                {
                    if (e.rand == el.rand)
                    {
                        dc.Stop();
                        dc.ClearList();
                    g:
                        for (int a = 0; a < list.list.Count; a++)
                        {
                            if (list[a].rand == el.rand) { break; }
                            skipped.Push(list[a]);
                            list.Remove2(0);
                            goto g;
                        }

                        for (int a = 0; a < list.Count; a++)
                        {
                            dc.AddRev_last(list[a], false);
                        }
                        dc.RunThread();
                        return;
                    }
                }
            }

            lock (skipped)
            {
                foreach (Element e in skipped)
                {
                    if (el.rand == e.rand)
                    {
                        while (skipped.Count > 0)
                        {
                            Element tmp = skipped.Pop();
                            list.Insert(0, tmp);
                            if (tmp.rand == e.rand) { break; }
                        }
                        dc.Stop();
                        dc.ClearList();
                        for (int a = 0; a < list.Count; a++)
                        {
                            dc.AddRev_last(list[a], false);
                        }
                        dc.RunThread();
                        return;
                    }
                }
            }
        }

        public bool CanBack()
        {
            if (skipped.Count < 2) { return false; }
            else { return true; }
        }

        public bool Connect(String site, String u, String p)
        {
            try
            {
                domain = site;
                mysite = new Site(site, u, p);
                return mysite.logged_in;
            }
            catch (Exception e) { MessageBox.Show(e.Message); return false; }
        }

        public void Clear()
        {
            contin = null;
            mind = DateTime.MinValue;
            maxd = DateTime.MinValue;
            list.ClearAll();
            skipped.items.Clear();
        }

        /*
         * Проверка на наличие флага патрулирующего
         * */
        public bool IsEditor()
        {
            try
            {
                String data = "";
                for (int a = 0; a < 5; a++)
                {
                    data = mysite.GetPageHTM(mysite.indexPath + "api.php?format=xml&action=query&list=users&ususers=" + HttpUtility.UrlEncode(mysite.userName) + "&usprop=groups");
                    if (!data.Equals("")) { break; }
                    else { Thread.Sleep(20); }
                }
                if (data != "")
                {
                    return data.Contains("<g>editor</g>");
                }
            }
            catch { }
            return false;
        }

        public void NewEvent(String v)
        {
            events.Push(v);
        }

        public String GetEvent()
        {
            list.Check(); //заодно
            if (events.Count == 0) { return null; }
            return events.Pop();
        }

        public bool CheckElement(Element el)
        {

            return true;
        }

        public DateTime GetMinFromPendingChanges()
        {
            DateTime ret = DateTime.MinValue;
            try
            {
                String data = mysite.GetPageHTM(mysite.indexPath + "api.php?format=xml&action=query&list=oldreviewedpages&ornamespace=" + StreamConfig.GetNamespaces() + "&orlimit=1&ordir=newer&ormaxsize=" + StreamConfig.GetParamMax() + "&orfilterredir=" + StreamConfig.GetParamRedir());
                String min = "";
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(data);
                XmlNode currentNode; XmlNode continueNode;
                currentNode = xmlDoc.DocumentElement.SelectSingleNode("query");
                if (currentNode == null) { return ret; }

                continueNode = xmlDoc.DocumentElement.SelectSingleNode("query-continue");
                if (continueNode == null) { return ret; }
                else
                {
                    continueNode = continueNode.SelectSingleNode("oldreviewedpages");
                    if (continueNode == null) { return ret; }
                    else
                    {
                        min = continueNode.Attributes.GetNamedItem("orstart").Value;
                        //MessageBox.Show(contin);
                    }
                }
                DateTime mind = DateTime.Parse(min);


                return mind;
            }
            catch (Exception e) { MessageBox.Show(e.Message); return ret; }
        }

        public DateTime GetMaxFromPendingChanges()
        {
            DateTime ret = DateTime.MinValue;
            try
            {
                String data = mysite.GetPageHTM(mysite.indexPath + "api.php?format=xml&action=query&list=oldreviewedpages&ornamespace=" + StreamConfig.GetNamespaces() + "&orlimit=1&ordir=older&ormaxsize=" + StreamConfig.GetParamMax() + "&orfilterredir=" + StreamConfig.GetParamRedir());
                String max = "";
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(data);
                XmlNode currentNode; XmlNode continueNode;
                currentNode = xmlDoc.DocumentElement.SelectSingleNode("query");
                if (currentNode == null) { return ret; }

                continueNode = xmlDoc.DocumentElement.SelectSingleNode("query-continue");
                if (continueNode == null) { return ret; }
                else
                {
                    continueNode = continueNode.SelectSingleNode("oldreviewedpages");
                    if (continueNode == null) { return ret; }
                    else
                    {
                        max = continueNode.Attributes.GetNamedItem("orstart").Value;
                        //MessageBox.Show(contin);
                    }
                }
                DateTime maxd = DateTime.Parse(max);


                return maxd;
            }
            catch (Exception e) { MessageBox.Show(e.Message); return ret; }
        }

        private String AutoDirection(DateTime mind, DateTime maxd, DateTime randd)
        {
            int total = (maxd - mind).Days;
            if (total == 0) { return "older"; }//разницы нет
            int min = (randd - mind).Days;
            double coef = (double)(min) / (double)(total);
            if (coef <= 0.5) { return "newer"; }
            else { return "older"; }
        }

        public bool Update(bool backgr=false, bool fin=false, bool thr=true)
        {
            String ns = StreamConfig.GetNamespaces(), max = StreamConfig.GetParamMax() + "", redir = StreamConfig.GetParamRedir(), dir = StreamConfig.GetDirection();
            int sm = 0;
            switch (StreamConfig.GetWMode())
            {
                case WorkMode.RecentChanges:
                    UpdateFromRecentChanges(ns);
                    break;
                case WorkMode.DatedArticles:
                    sm = StreamConfig.GetSubmode_1();

                    if (dir.Equals("auto") && maxd == DateTime.MinValue && (sm == 2 || sm == 4))
                    {
                        maxd = GetMaxFromPendingChanges();
                        mind = GetMinFromPendingChanges();
                    }

                    switch (sm)
                    {
                        case 0:
                            if (contin == null)
                            {
                                contin = "";
                            }
                            UpdateListFromPendingChanges(ns, "older", max, redir, "all", "", backgr, fin, thr);
                            break;
                        case 1:
                            if (contin == null)
                            {
                                contin = "";
                            }
                            UpdateListFromPendingChanges(ns, "newer", max, redir, "all", "", backgr, fin, thr);
                            break;
                        case 2:
                            if (contin == null) { contin = StreamConfig.GetDateFromMode1().ToString("yyyy-MM-ddTHH:mm:ssZ"); }
                            if (dir.Equals("auto"))
                            {
                                dir = AutoDirection(mind, maxd, StreamConfig.GetDateFromMode1());
                            }
                            UpdateListFromPendingChanges(ns, dir, max, redir, "all", "", backgr, fin, thr);
                            break;
                        case 3:
                            if (contin == null)
                            {
                                contin = "";
                            }
                            if (dir.Equals("auto"))
                            {
                                dir = "older";
                            }
                            UpdateListFromPendingChanges(ns, dir, max, redir, "watched", "", backgr, fin, thr);
                            break;
                        case 4:
                            if (contin == null)
                            {
                                DateTime randd = Tools.GetRandomDate(mind, maxd);
                                dir = AutoDirection(mind, maxd, randd);
                                contin = randd.ToString("yyyy-MM-ddT00:00:00Z");
                            }
                            UpdateListFromPendingChanges(ns, dir, max, redir, "all", "", backgr, fin, thr);
                            break;
                        case 5:
                            if (contin == null)
                            {
                                contin = "";
                            }
                            if (dir.Equals("auto"))
                            {
                                dir = "older";
                            }
                            UpdateListFromPendingChanges(ns, dir, max, redir, "all", StreamConfig.GetParamCategory(), backgr, fin, thr);
                            break;
                    }
                    break;
                case WorkMode.UnreviewedArticles:
                    switch (StreamConfig.GetSubmode_2())
                    {
                        case 0:
                            if (contin == null)
                            {
                                contin = "";
                            }
                            break;
                        case 1:
                            if (contin == null)
                            {
                                contin = StreamConfig.GetStart_2();
                            }
                            break;

                    }
                    UpdateListFromUnreviewedPages(ns, redir, backgr, fin, thr);
                    break;
                case WorkMode.List:
                    AddFromList(StreamConfig.list);
                    break;
                case WorkMode.Contribution:
                    sm = StreamConfig.GetSubmode_4();
                    if (contin == null)
                    {
                        contin = "";
                    }
                    if (dir.Equals("auto")) { dir = "older"; }

                    String st = "";
                    if (sm == 1) { st = StreamConfig.GetDateFromMode4().ToString("yyyy-MM-ddTHH:mm:ssZ"); ; }
                    UpdateFromUserContribution(ns, dir, StreamConfig.GetUser(), st, backgr, fin, thr);
                    break;
            }

            return true;
        }

        public void UpdateListFromPendingChanges(String ns, String dir, String max, String redir, String watched, String category, bool backgr = false, bool fin = false, bool thr = true)
        {
            CurrUpdThread = new System.Threading.Thread(new System.Threading.ThreadStart(delegate()
            {
                try
                {                                     
                    updating = true;
                    Logging.AddLog("updating list from pending changes.. ["+backgr+"/"+fin+"/"+thr+"]");
                   // Process.Start("http://ru.wikipedia.org/w/" + "api.php?format=xml&action=query&list=oldreviewedpages" + (contin != null && !contin.Equals("") ? "&orstart=" + contin : "") + "&ornamespace=" + ns + "&orlimit=" + count_p + "&ordir=" + dir + "&ormaxsize=" + max + "&orfilterredir=" + redir + "&orfilterwatched=" + watched + "&orcategory=" + HttpUtility.UrlEncode(category.Trim().Replace(' ', '_')));

                    String cont = "";
                    if (contin != null && !contin.Equals(""))
                    {
                        cont = contin;
                    }
                    else
                    {
                        if (list.Count > 0)
                        {
                            cont = list[list.Count - 1].stable_id + "";
                        }
                    }

                    String data = mysite.GetPageHTM(mysite.indexPath + "api.php?format=xml&action=query&list=oldreviewedpages" + (!cont.Equals("") ? "&orstart=" + cont : "") + "&ornamespace=" + ns + "&orlimit=" + count_p + "&ordir=" + dir + "&ormaxsize=" + max + "&orfilterredir=" + redir + "&orfilterwatched=" + watched + "&orcategory=" + HttpUtility.UrlEncode(category.Trim().Replace(' ', '_')));
                   // MessageBox.Show(mysite.indexPath + "api.php?format=xml&action=query&list=oldreviewedpages" + (!cont.Equals("") ? "&orstart=" + cont : "") + "&ornamespace=" + ns + "&orlimit=" + count_p + "&ordir=" + dir + "&ormaxsize=" + max + "&orfilterredir=" + redir + "&orfilterwatched=" + watched + "&orcategory=" + HttpUtility.UrlEncode(category.Trim().Replace(' ', '_')));
              
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(data);
                    XmlNode currentNode; XmlNode continueNode;
                    updating = false;
                    currentNode = xmlDoc.DocumentElement.SelectSingleNode("query");
                    if (currentNode == null) { if (backgr == false) { events.Push("updatingerr"); } return; }

                    continueNode = xmlDoc.DocumentElement.SelectSingleNode("query-continue");
                    if (continueNode == null) { contin = null; }
                    else
                    {
                        continueNode = continueNode.SelectSingleNode("oldreviewedpages");
                        if (continueNode == null) { contin = null; }
                        else
                        {
                            contin = continueNode.Attributes.GetNamedItem("orstart").Value;
                        }
                    }

                    currentNode = currentNode.SelectSingleNode("oldreviewedpages");
                    if (currentNode == null) { if (backgr == false) { events.Push("updatingerr"); } return; }
                    int added = 0;
                    long tmp = 0;

                    lock (list)
                    {

                        foreach (XmlNode el in currentNode.SelectNodes("p"))
                        {
                            XmlNode attr;

                            attr = el.Attributes.GetNamedItem("revid");
                            tmp = Convert.ToInt64(attr.Value);

                            if (list.waslist_revid.Contains(tmp)) { continue; }
                            Element tr = new Element();
                            tr.curr_id = tmp;

                            added++;
                            attr = el.Attributes.GetNamedItem("title");
                            tr.title = attr.Value;
                            tr.ns = Convert.ToInt32(el.Attributes.GetNamedItem("ns").Value);
                            attr = el.Attributes.GetNamedItem("stable_revid");
                            tr.stable_id = Convert.ToInt64(attr.Value);
                            attr = el.Attributes.GetNamedItem("diff_size");
                            tr.diff_size = Convert.ToInt32(attr.Value);

                            int isok = 1;
                            foreach (Element q in list.list)
                            {
                                if (q.stable_id == tr.stable_id)
                                {
                                    isok = 0;
                                }
                            }

                            if (isok == 0) { continue; }

                            tr.check = false;
                            tr.FinishStart(mysite);
                            list.Add(tr);
                            dc.AddRev_last(tr, false);
                        }
                    }
                   // MessageBox.Show(added + "q");
                    Logging.AddLog("loaded " + added + " changes");
                    dc.RunThread();
  
                    updating = false;
                    if (added == 0) { if (backgr == false || fin == true) { if (fin == true) { events.Push("nodata_fin"); } else { events.Push("nodata"); } } return; }


                    if (backgr == false) { events.Push("endupdating"); }

                }
                catch (Exception) { /*MessageBox.Show(e.Message + " - исключение в потоке"); */events.Push("upd-ex"); }
            }));


            CurrUpdThread.Start();
            if (!thr) { CurrUpdThread.Join(); }
            //  MessageBox.Show(data);
        }

        public void UpdateListFromUnreviewedPages(String ns, String redir, bool backgr = false, bool fin = false, bool thr = true)
        {
            CurrUpdThread = new System.Threading.Thread(new System.Threading.ThreadStart(delegate()
            {
                try
                {                 
                    if (contin == null) { if (backgr == false || fin == true) { if (fin == true) { NewEvent("nodata_fin"); } else { NewEvent("nodata"); } } return; }
                    
                    updating = true;

                    Logging.AddLog("updating list from unreviewed pages.. ["+backgr+"/"+fin+"/"+thr+"]");

                    String cont = "";
                    if (contin != null && !contin.Equals(""))
                    {
                        cont = contin;
                    }
                    else
                    {
                        if (list.Count > 0)
                        {
                            cont = HttpUtility.UrlEncode(list[list.Count - 1].title);
                        }
                    }
                   // MessageBox.Show(mysite.indexPath + "api.php?format=xml&action=query&list=unreviewedpages&urstart=" + cont + "&urnamespace=" + ns + "&urfilterlevel=0&urfilterredir=" + redir + "&urlimit=" + count_p);

                    String data = mysite.GetPageHTM(mysite.indexPath + "api.php?format=xml&action=query&list=unreviewedpages&urstart="+cont+"&urnamespace="+ns+"&urfilterlevel=0&urfilterredir="+redir+"&urlimit="+count_p);
                    
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(data);
                    XmlNode currentNode; XmlNode continueNode;
                    updating = false;
                    currentNode = xmlDoc.DocumentElement.SelectSingleNode("query");
                    if (currentNode == null) { if (backgr == false) { events.Push("updatingerr"); } return; }

                    continueNode = xmlDoc.DocumentElement.SelectSingleNode("query-continue");
                    if (continueNode == null) { contin = null; }
                    else
                    {
                        continueNode = continueNode.SelectSingleNode("unreviewedpages");
                        if (continueNode == null) { contin = null; }
                        else
                        {
                            contin = continueNode.Attributes.GetNamedItem("urstart").Value;
                        }
                    }

                    currentNode = currentNode.SelectSingleNode("unreviewedpages");
                    if (currentNode == null) { if (backgr == false) { events.Push("updatingerr"); } return; }
                    int added = 0;
                    long tmp = 0;

                    foreach (XmlNode el in currentNode.SelectNodes("p"))
                    {
                        XmlNode attr;
                        
                        attr = el.Attributes.GetNamedItem("revid");
                        tmp = Convert.ToInt64(attr.Value);

                        if (list.waslist_revid.Contains(tmp)) { continue; }
                        Element tr = new Element();
                        tr.curr_id = tmp;

                        added++;
                        attr = el.Attributes.GetNamedItem("title");
                        tr.title = attr.Value;
                        tr.ns =  Convert.ToInt32(el.Attributes.GetNamedItem("ns").Value);
                        tr.curr_id=-1;

                        tr.check = true;

                        int ok = 1;
                        for (int aa=0; aa < list.Count; aa++)
                        {
                            if (list[aa].title.Equals(tr.title)) { ok = 0; break; }
                        }
                        if (ok == 0) { continue; }

                        tr.FinishStart(mysite);
                        list.Add(tr);
                        dc.AddRev_last(tr, false);
                    }
                    Logging.AddLog("loaded " + added + " changes");
                    dc.RunThread();
  
                    updating = false;
                    if (added == 0) { if (backgr == false || fin == true) { if (fin == true) { events.Push("nodata_fin"); } else { events.Push("nodata"); } } return; }


                    if (backgr == false) { events.Push("endupdating"); }

                }
                catch (Exception) { /*MessageBox.Show(e.Message + " - исключение в потоке"); */events.Push("upd-ex"); }
            }));


            CurrUpdThread.Start();
            if (!thr) { CurrUpdThread.Join(); }
        }


        public void AddFromList(List<String> pages)
        {
            foreach (String s in pages)
            {
                String j = s.Trim();
                if (j.Equals("")) { continue; }
                Element tr = new Element();
                tr.title=j;
                tr.check = true;
                tr.FinishStart(mysite);
                list.Add(tr);
                dc.AddRev_last(tr, false);
            }
            dc.RunThread();
            events.Push("endupdating");
        }

        public bool UpdateFromRecentChanges(String ns)
        {
            try
            {
                if (updating == true) { return false; }
                updating = true;
                Logging.AddLog("updating list from recent changes.. ");

                String data = HTTP_W_REQUEST("api.php?format=xml&action=query&list=recentchanges&rcnamespace="+ns+"&rcprop=title|ids&rclimit="+Options.GetOption("recent_count"));
                //Process.Start(domain + mysite.indexPath + "api.php?format=xml&action=query&list=recentchanges&rcnamespace=" + ns + "&rcprop=title|ids&rclimit=" + Options.GetOption("recent_count"));
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(data);
                XmlNode currentNode;

                currentNode = xmlDoc.DocumentElement.SelectSingleNode("query");
                if (currentNode == null) { updating = false; return false; }

                currentNode = currentNode.SelectSingleNode("recentchanges");
                if (currentNode == null) { updating = false; return false; }
                int added = 0;
                long tmp = 0;

                List<long> accepted_ids = new List<long>();

                foreach (XmlNode el in currentNode.SelectNodes("rc"))
                {
                    XmlNode attr;

                    attr = el.Attributes.GetNamedItem("revid");
                    tmp = Convert.ToInt64(attr.Value);
                    accepted_ids.Add(tmp);

                    Element tr = new Element();
                    tr.curr_id = tmp;

                    added++;
                    attr = el.Attributes.GetNamedItem("title");
                    tr.title = attr.Value;
                    bool ok = true;
                    for (int a = 0; a < list.Count; a++)
                    {
                        if (list[a].title.Equals(tr.title) && list[a].curr_id < tr.curr_id)
                        {
                            dc.Skip(list[a]);
                            list.Remove2(a);
                            break;
                        }
                        if (list[a].title.Equals(tr.title) && list[a].curr_id >= tr.curr_id)
                        {
                            ok = false; break;
                        }
                    }
                    if (!ok) { continue; }

                    foreach (Element e in skipped)
                    {
                        if (e.curr_id == tr.curr_id)
                        {
                            //skipped.items.Remove(e);
                            ok = false;
                            break;
                        }
                    }

                    if (!ok) { continue; }
                    tr.ns = Convert.ToInt32(el.Attributes.GetNamedItem("ns").Value);
                   // tr.curr_id = -1;

                    tr.check = true;

                    tr.FinishStart(mysite);
                    list.Insert(0, tr);

                    dc.AddRev_first(tr, false);
                }

                again:
                for (int a = 0; a < list.Count; a++)
                {
                    int found = 0;
                    foreach (long l in accepted_ids)
                    {
                        if (list[a].curr_id == l) { found++; break; }
                    }
                    if (found == 0)
                    {
                        dc.Skip(list[a]);
                        list.Remove2(a);
                        goto again;
                    }
                }



                Logging.AddLog("loaded " + added + " changes");
                dc.Stop();
                dc.RunThread();

                updating = false;
                if (added == 0) { return false; }
                else { events.Push("endupdating"); return true; }


            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            updating = false;
            return false;
        }

        public void UpdateFromUserContribution(String ns, String dir, String user, String start, bool backgr = false, bool fin = false, bool thr = true)
        {
            //http://ru.wikipedia.org/w/api.php?action=query&prop=info&list=usercontribs&ucuser=H2Bot&ucdir=newer&ucnamespace=0&ucprop=ids|title&ucshow=!patrolled&uclimit=50
            CurrUpdThread = new System.Threading.Thread(new System.Threading.ThreadStart(delegate()
            {
                try
                {
                    if (contin == null) { if (backgr == false || fin == true) { if (fin == true) { NewEvent("nodata_fin"); } else { NewEvent("nodata"); } } return; }

                    updating = true;

                    Logging.AddLog("updating list from user's contribution.. [" + backgr + "/" + fin + "/" + thr + "]");

                    String data = HTTP_W_REQUEST("api.php?format=xml&action=query&prop=info&list=usercontribs&" + 
                        (start.Equals("") ? "" : "ucstart=" + start) + "&ucuser=" + HttpUtility.UrlEncode(user) + (contin.Equals("") ? "" : "&ucstart=" + contin) +
                        "&ucnamespace=" + ns + "&ucdir=" + dir + "&ucprop=ids|title&uclimit=" + count_p);
                    //MessageBox.Show(mysite.indexPath + "api.php?format=xml&action=query&prop=info&list=usercontribs&" + (start.Equals("") ? "" : "ucstart=" + start) + "&ucuser=" + HttpUtility.UrlEncode(user) + (contin.Equals("") ? "" : "&ucstart=" + contin) + "&ucnamespace=" + ns + "&ucdir=" + dir + "&ucprop=ids|title&ucshow=!patrolled&uclimit=" + count_p);
                    // Process.Start("http://ru.wikipedia.org/w/" + "api.php?format=xml&action=query&prop=info&list=usercontribs&"+(start.Equals("")?"":"ucstart="+start)+"&ucuser="+HttpUtility.UrlEncode(user)+(contin.Equals("")?"":"&ucstart=" + contin) + "&ucnamespace=" + ns + "&ucdir=" + dir + "&ucprop=ids|title&ucshow=!patrolled&uclimit=" + count_p);


                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(data);
                    XmlNode currentNode; XmlNode continueNode;
                    updating = false;
                    currentNode = xmlDoc.DocumentElement.SelectSingleNode("query");
                    if (currentNode == null) { if (backgr == false) { events.Push("updatingerr"); } return; }

                    continueNode = xmlDoc.DocumentElement.SelectSingleNode("query-continue");
                    if (continueNode == null) { contin = null; }
                    else
                    {
                        continueNode = continueNode.SelectSingleNode("usercontribs");
                        if (continueNode == null) { contin = null; }
                        else
                        {
                            contin = continueNode.Attributes.GetNamedItem("ucstart").Value;
                        }
                    }

                    currentNode = currentNode.SelectSingleNode("usercontribs");
                    if (currentNode == null) { if (backgr == false) { events.Push("updatingerr"); } return; }
                    int added = 0;
                    long tmp = 0;

                    foreach (XmlNode el in currentNode.SelectNodes("item"))
                    {
                        XmlNode attr;

                        attr = el.Attributes.GetNamedItem("revid");
                        tmp = Convert.ToInt64(attr.Value);

                        if (list.waslist_revid.Contains(tmp)) { continue; }
                        Element tr = new Element();
                        tr.curr_id = tmp;

                        added++;
                        attr = el.Attributes.GetNamedItem("title");
                        tr.title = attr.Value;

                        bool ok = true;

                        foreach (Element et in list.list)
                        {
                            if (et.title.Equals(tr.title)) { ok = false; break; }
                        }
                        if (!ok) { continue; }
                        foreach (Element et in skipped)
                        {
                            if (et.title.Equals(tr.title)) { ok = false; break; }
                        }
                        if (!ok) { continue; }

                        tr.ns = Convert.ToInt32(el.Attributes.GetNamedItem("ns").Value);
                        tr.curr_id = -1;

                        tr.check = true;

                        tr.FinishStart(mysite);
                        list.Add(tr);
                        dc.AddRev_last(tr, false);
                    }
                    Logging.AddLog("loaded " + added + " changes");
                    dc.RunThread();

                    updating = false;
                    if (added == 0) { if (backgr == false || fin == true) { if (fin == true) { events.Push("nodata_fin"); } else { events.Push("nodata"); } } return; }


                    if (backgr == false) { events.Push("endupdating"); }

                }
                catch (Exception) { /*MessageBox.Show(e.Message + " - исключение в потоке"); */events.Push("upd-ex"); }
            }));


            CurrUpdThread.Start();
            if (!thr) { CurrUpdThread.Join(); }
        
        }

    /*    public void UpdateFromBackLinks(String ns, String redir, String article, bool backgr = false, bool fin = false, bool thr = true)
        {
            //http://ru.wikipedia.org/w/api.php?action=query&list=backlinks&blnamespace=&blfilterredir=&bltitle=Main%20Page&bllimit=50

            CurrUpdThread = new System.Threading.Thread(new System.Threading.ThreadStart(delegate()
        {
            try
            {
                if (contin == null) { if (backgr == false || fin == true) { if (fin == true) { NewEvent("nodata_fin"); } else { NewEvent("nodata"); } } return; }

                updating = true;

                Logging.AddLog("updating list from links.. [" + backgr + "/" + fin + "/" + thr + "]");

                String data = mysite.GetPageHTM(mysite.indexPath + "api.php?format=xml&action=query&prop=info&list=backlinks&blcontinue="+contin+"&blnamespace=" + ns + "blfilterredir=" + redir + "&bltitle="+HttpUtility.UrlEncode(article)+"&uclimit=" + count_p);

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(data);
                XmlNode currentNode; XmlNode continueNode;
                updating = false;
                currentNode = xmlDoc.DocumentElement.SelectSingleNode("query");
                if (currentNode == null) { if (backgr == false) { events.Push("updatingerr"); } return; }

                continueNode = xmlDoc.DocumentElement.SelectSingleNode("query-continue");
                if (continueNode == null) { contin = null; }
                else
                {
                    continueNode = continueNode.SelectSingleNode("backlinks");
                    if (continueNode == null) { contin = null; }
                    else
                    {
                        contin = continueNode.Attributes.GetNamedItem("blcontinue").Value;
                    }
                }

                currentNode = currentNode.SelectSingleNode("backlinks");
                if (currentNode == null) { if (backgr == false) { events.Push("updatingerr"); } return; }
                int added = 0;
                long tmp = 0;

                foreach (XmlNode el in currentNode.SelectNodes("bl"))
                {
                    XmlNode attr;

                    attr = el.Attributes.GetNamedItem("pageid");
                    tmp = Convert.ToInt64(attr.Value);

                    if (list.waslist_revid.Contains(tmp)) { continue; }
                    TElement tr = new TElement();
                    tr.curr_id = tmp;

                    added++;
                    attr = el.Attributes.GetNamedItem("title");
                    tr.title = attr.Value;
                    tr.ns = Convert.ToInt32(el.Attributes.GetNamedItem("ns").Value);
                    tr.curr_id = -1;

                    tr.check = true;

                    tr.FinishStart(mysite);
                    list.Add(tr);
                    dc.AddRev_last(tr, false);
                }
                Logging.AddLog("loaded " + added + " changes");
                dc.RunThread();

                updating = false;
                if (added == 0) { if (backgr == false || fin == true) { if (fin == true) { events.Push("nodata_fin"); } else { events.Push("nodata"); } } return; }


                if (backgr == false) { events.Push("endupdating"); }

            }
            catch (Exception e) { events.Push("upd-ex"); }
        }));


            CurrUpdThread.Start();
            if (!thr) { CurrUpdThread.Join(); }

        }
        */

        

        public SweepNetPage SweepNetPageCreate(String title)
        {
            return new SweepNetPage(mysite, title);
        }

        public Page PageCreate(String title)
        {
            return new Page(mysite, title);
        }

        public void UpdateDiff(Element el)
        {
            dc.AddRev_first(el, true, true);
        }

        /* Запустить патрулирование в фоне */

        public void Review(long rev)
        {
            Thread myThread = new System.Threading.Thread(new System.Threading.ThreadStart(delegate()
            {
                Review_(rev);
            }));

            myThread.Start();
        }

        public void ReviewL(List<long> list)
        {
            Thread myThread = new System.Threading.Thread(new System.Threading.ThreadStart(delegate()
            {
                for (int a = 0; a < list.Count; a++) { Review_(list[a]); }
            }));

            myThread.Start();
        }

        /* Запустить снятие отметки со статьи в фоне */

        public void Unreview(long rev)
        {
            Thread myThread = new System.Threading.Thread(new System.Threading.ThreadStart(delegate()
            {
                Review_(rev, 1);
            }));

            myThread.Start();
        }

        public void UnreviewL(List<long> list)
        {
            Thread myThread = new System.Threading.Thread(new System.Threading.ThreadStart(delegate()
            {
                for (int a = 0; a < list.Count; a++) { Review_(list[a],1); }
            }));

            myThread.Start();
        }

        public void Review_(long rev, int cancel = 0)
        {
            String title = "";
            try
            {
                for (int a = 0; a < 3; a++)
                {
                    try
                    {
                        foreach (Element tr in skipped)
                        {
                            if (tr.curr_id == rev)
                            {
                                title = tr.title;
                                goto fin;//break;
                            }
                        }
                        goto fin;
                    }
                    catch { Thread.Sleep(100); }
                }
            fin:

                int c = 0;
                Page pg = new Page(mysite, rev);
                for (c = 0; c < 3; c++)
                {
                    pg.editSessionToken = "";
                    try
                    {
                        pg.GetEditSessionDataEx2();
                    }
                    catch (Exception) { }
                    if (!pg.editSessionToken.Equals("")) { break; }
                }
                if (pg.editSessionToken.Equals("")) { MessageBox.Show(String.Format("Error occured while reviewing \"{0}\"".T(), title)); return; }
                String data = "";
                for (c = 0; c < 5; c++)
                {
                    try
                    {
                        string summary = "";
                        if (SiteManagerClass.review_summaries.ContainsKey(SiteManagerClass.current) && !(SiteManagerClass.review_summaries[SiteManagerClass.current].ToString().Equals("")))
                        {
                            summary = SiteManagerClass.review_summaries[SiteManagerClass.current].ToString();
                        }
                        else
                        {
                            summary = HttpUtility.UrlEncode(Options.GetOption("rw_comm").Equals("") ? (String.Format(i18n.GetXmlEntry("REVIEW_SUMMARY"), Version.GetName(), Version.GetVersion())) : Options.GetOption("rw_comm"));
                        }
                        
                        data = mysite.PostDataAndGetResultHTM(mysite.indexPath + "api.php?", "format=xml&action=review&revid=" + rev + "&token=" + HttpUtility.UrlEncode(pg.editSessionToken) + "&flag_accuracy=1&comment=" + summary + (cancel == 1 ? "&unapprove=1" : "")); //"отпатрулировано с " + Version.GetName() + " " + Version.GetVersion()
                        if (data != "" && data != null && data.Contains("Success") && !data.Contains("\"badtoken\"")) { break; }
                        EditTokenCache.token = "";
                        pg.GetEditSessionDataEx2();
                    }
                    catch (Exception)
                    {
                        /* MessageBox.Show(e.Message+" - ошибка при патрулировании");*/
                    }
                }
                //MessageBox.Show(data);
                if (data.Contains("Success"))
                {
                    if (cancel == 0)
                    {
                        pc++; events.Push("reviewed"); events.Push("reviewed" + rev);
                        foreach (Element tr in skipped)
                        {
                            if (tr.curr_id == rev)
                            {
                                tr.reviewed = true;
                            }
                        }
                    }
                    else
                    {

                        pc--; events.Push("unreviewed" + rev);
                        foreach (Element tr in skipped)
                        {
                            if (tr.curr_id == rev)
                            {
                                tr.reviewed = false;
                            }
                        }
                    }
                }
                else { /*MessageBox.Show(data);*/ events.Push("notreviewed-" + title); }
                // pc++;
                // return data.Contains("Success");
            }
            catch (Exception e)
            {
                // return false;
                MessageBox.Show(e.Message);
                events.Push("notreviewed_" + title);
            }
        }

        public void Revert_(long curr, long stab, String comm)
        {

            Page p = new Page(mysite, curr);
          //  long newrevid = p.CancelFrom(stab+1, curr+1, comm, false);
            Page p2 = new Page(mysite, stab);
            p2.Load();
            long newrevid=p.Save2(p2.text, comm, false);
            rc++;
            if (newrevid <= 0)
            {
                SweepNetPage tp = this.SweepNetPageCreate(p.title);
                tp.LoadRevision(curr);
                newrevid = tp.maxid;
            }
            Review_(newrevid);


            foreach (Element tr in skipped)
            {
                if (tr.curr_id == curr)
                {
                    tr.rejected = true;
                }
            }
            events.Push("reverted");
        }

        public void Revert(long curr, long stab)
        {
            Thread myThread = new System.Threading.Thread(new System.Threading.ThreadStart(delegate()
            {
                try
                {
                    string summary = "";
                    if (SiteManagerClass.rollback_summaries.ContainsKey(SiteManagerClass.current) && !(SiteManagerClass.rollback_summaries[SiteManagerClass.current].ToString().Equals("")))
                    {
                        summary = SiteManagerClass.rollback_summaries[SiteManagerClass.current].ToString();
                    }
                    else
                    {
                        summary = (Options.GetOption("rj_comm").Equals("") ? i18n.GetXmlEntry("ROLLBACK_SUMMARY") : Options.GetOption("rj_comm"));
                    }
                    Revert_(curr, stab, summary.Replace("{0}", stab + ""));
                }
                catch (Exception e) { MessageBox.Show(e.Message); }
            }));

            myThread.Start();
        }

        public void Cancel(long curr, long stab, String comm)
        {
            Thread myThread = new System.Threading.Thread(new System.Threading.ThreadStart(delegate()
            {
                Revert_(curr, stab, comm);
            }));

            myThread.Start();
        }

        // /w/%url%
        public string HTTP_W_REQUEST(string url)
        {
            return mysite.EasyGetData(mysite.indexPath + url);
        }
    }

  
}

