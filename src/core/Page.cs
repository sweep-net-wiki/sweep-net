using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Web;
using DotNetWikiBot;
using System.Collections;
using System.Xml;

namespace sweepnet
{
       public class WikiRevision
        {
            public long revid;
            public String user = "";
            public bool flagged = false;
            public bool minor = false;
            public String comm = "";
            public String text = "";
            public String timestamp;
            public int size = 0;
            public int diffsize = 0; // to previous
            public double rand = -1;

            public WikiRevision()
            {

            }
        }

       class RevLoadOptions
       {
           public bool updated = false;
           public long start;
           public bool withstart = true;
           public bool loadtext = false;
       }

        public class SweepNetPage
        {
            public List<WikiRevision> list;

            public WikiRevision stable=null, curr=null;

            private Site mysite;
            public String title { get; set; }

            public long maxid = 0;
            public bool is_flagged = false;
            public bool last_flagged = false;
            public long flagged_rev = -1;
            public String ns = "";
            public int flagged_size=-1, last_size=-1;
            public int unflagged_count = 0;

            private RevLoadOptions prev_opt=new RevLoadOptions();

            public SweepNetPage(Site s, String pagename)
            {
                title = pagename.Replace(' ', '_');
                mysite = s;
                list = new List<WikiRevision>();
            }
            public String HttpGet(String url)
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.UserAgent = "Opera 9/11 (windows Nt; 9)";//Version.GetName()+" "+Version.GetVersion();
                // request.Timeout = 9;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
                String data = readStream.ReadToEnd();
                response.Close();
                readStream.Close();
                return data;
            }

            public void ReloadRevision()
            {
                if (prev_opt.updated)
                {
                    LoadRevision(prev_opt.start, prev_opt.withstart, prev_opt.loadtext);
                }
            }

            public bool LoadRevision(long start, bool withstart = true, bool loadtext = false)
            {
                prev_opt.updated = true; prev_opt.start = start; prev_opt.withstart= withstart; prev_opt.loadtext = loadtext;

                list.Clear();
                List<WikiRevision> l = new List<WikiRevision>();
                WikiRevision tmp;
                WikiRevision prev=null;
                long first_start = start;
                try
                {
                    long next = -1;
                    start = -1;
                st:

                    String data = mysite.GetPageHTM(mysite.indexPath + "api.php?format=xml&action=query&prop=revisions|flagged&rvdir=older" + (start < 1 ? "" : "&rvstartid=" + start + "") + (first_start<1?"":"&rvendid="+first_start)+"&rvlimit=500&titles=" + HttpUtility.UrlEncode(title) + "&rvprop=ids|timestamp|user|comment|flags|size" + (loadtext ? "|content" : ""));
                    //  Process.Start("http://ru.wikipedia.org/w/" +  "api.php?format=xml&action=query&prop=revisions|flagged&rvdir=older" + (start < 1 ? "" : "&rvstartid=" + start + "") + (first_start<1?"":"&rvendid="+first_start)+"&rvlimit=500&titles=" + HttpUtility.UrlEncode(title) + "&rvprop=ids|timestamp|user|comment|flags|size" + (loadtext ? "|content" : ""));
                    // MessageBox.Show(data);
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(data);
                    XmlNode currentNode;
                    // updating = false;
                    currentNode = xmlDoc.DocumentElement.SelectSingleNode("query");
                    if (currentNode == null) { return false; }

                    next = -1;
                    XmlNode cn = xmlDoc.DocumentElement.SelectSingleNode("query-continue");
                    if (cn != null)
                    {
                        cn = cn.SelectSingleNode("revisions");
                        next = Convert.ToInt64(cn.Attributes.GetNamedItem("rvstartid").Value);
                    }

                    currentNode = currentNode.SelectSingleNode("pages");
                    if (currentNode == null) { return false; }
                    currentNode = currentNode.SelectSingleNode("page");
                    if (currentNode == null) { return false; }

                    XmlNode fr = currentNode.SelectSingleNode("flagged");
                    if(fr !=null){
                    flagged_rev = Convert.ToInt32(fr.Attributes.GetNamedItem("stable_revid").Value); 
                    }
                    if (flagged_rev > 0)
                    {
                        is_flagged = true;
                    }


                    ns = currentNode.Attributes.GetNamedItem("ns").Value;

                    currentNode = currentNode.SelectSingleNode("revisions");
                    if (currentNode == null) { return false; }
                    bool enought = false;
                    foreach (XmlNode el in currentNode.SelectNodes("rev"))
                    {
                        tmp = new WikiRevision();
                        tmp.revid = Convert.ToInt64(el.Attributes.GetNamedItem("revid").Value);
                        if (tmp.revid == start && withstart == false) { continue; }
                        tmp.rand = Tools.FullRandom();
                        if (tmp.revid > maxid) { maxid = tmp.revid; }
                        tmp.comm = el.Attributes.GetNamedItem("comment").Value;
                        tmp.user = el.Attributes.GetNamedItem("user").Value;
                        tmp.timestamp = el.Attributes.GetNamedItem("timestamp").Value;
                        tmp.minor = el.Attributes.GetNamedItem("minor") != null;
                        tmp.size=Convert.ToInt32(el.Attributes.GetNamedItem("size").Value);
                        if (prev != null)
                        {
                            prev.diffsize = prev.size - tmp.size;
                        }
                        prev = tmp;
                        if (loadtext) { tmp.text = el.InnerText; }
                        if (tmp.revid <= flagged_rev) { tmp.flagged = true; }
                        l.Add(tmp);
                        
                        if (tmp.flagged) { enought = true; break; }
                        /* */
                    }
                    if (next != -1 && enought == false)
                    {
                        start = next;
                        goto st;
                    }
                }
                catch (Exception)
                {
                    list.Clear();
                }
                

                for (int a = l.Count - 1; a >= 0; a--)
                {
                    tmp = l[a];
                    
                    if (tmp.revid == flagged_rev)
                    {
                        flagged_size = tmp.size;
                    }

                    last_flagged = tmp.flagged;
                    last_size = tmp.size;
                    if (tmp.flagged == false) { unflagged_count++; }
                    if (tmp.flagged == true && (stable == null || stable.revid < tmp.revid)) { stable = tmp; }
                    if (curr == null || curr.revid < tmp.revid) { curr = tmp; }
                    list.Add(tmp);
                }

                return list.Count > 0;
            }

            public List<String> GetContributors(long start_id)
            {
                List<String> cl = new List<string>();
                for (int a = 0; a < list.Count; a++)
                {
                    if (list[a].revid < start_id) { continue; }
                    if (!cl.Contains(list[a].user)) { cl.Add(list[a].user); }
                }
                return cl;
            }

            public Hashtable GetHashtableofcontributors(long start_id, long end_id=-1)
            {
                Hashtable h = new Hashtable();
                for (int a = 0; a < list.Count; a++)
                {
                    if (list[a].revid < start_id) { continue; }
                    if (end_id != -1 && list[a].revid > end_id) { break; }
                    if (h.Contains(list[a].user))
                    {
                        h[list[a].user] = (Convert.ToInt32(h[list[a].user]) + 1) + "";
                    }
                    else { h[list[a].user] = "1"; }
                }
                return h;
            }

            public int GeCountOfEdits(long start_id, long end_id = -1)
            {
                int res = 0;
                for (int a = 0; a < list.Count; a++)
                {
                    if (list[a].revid < start_id) { continue; }
                    if (list[a].revid > end_id) { break; }
                    res++;
                }
                return res;
            }

            public WikiRevision GetLastRevision()
            {
                return list[list.Count - 1];
            }

            public void SetReviewed(long revid)
            {
                for (int a = 0; a < list.Count; a++)
                {
                    if (list[a].revid == revid)
                    {
                        if (stable != null && stable.revid < revid)
                        {
                            stable = list[a];
                            flagged_rev = revid;
                        }

                        is_flagged = true;
                        list[a].flagged = true;
                        
                        if (a == list.Count - 1) { last_flagged = true; }
                        
                        break;
                    }
                }
            }

            public void UpdateRwStatus()
            {
                stable = null;
                for (int a = 0; a < list.Count; a++)
                {
                    if (list[a].flagged)
                    {
                        if (stable == null) { stable = list[a]; }
                        else if (stable.revid < list[a].revid) { stable = list[a]; }
                    }
                }
                if (stable != null) { flagged_rev = stable.revid; is_flagged = true; }
                else { is_flagged = false; last_flagged = false; }

                if (stable != null)
                {
                    for (int a = 0; a < list.Count; a++)
                    {
                        if (list[a].revid <= stable.revid) { list[a].flagged = true; }
                        else { list[a].flagged = false; }
                    }
                }
            }

            public void AddRevision(WikiRevision rev)
            {
                list.Add(rev);
                curr = rev;
                last_flagged = rev.flagged;
                if (!is_flagged && rev.flagged) { is_flagged = true; }
            }

            public bool CheckAuthors()
            {
                if (list.Count == 0) return true;
                long startid = -1;
                if (stable != null) { startid = stable.revid + 1; }
                for (int a = 0; a < list.Count; a++)
                {
                    if (list[a].revid < startid) { continue; }
                    if (!Tools.IsValidIPAddress(list[a].user)) return true;
                }
                return false;
            }

            public bool CheckProfMode()
            {
                long startid = -1;
                if (stable != null) { startid = stable.revid + 1; }
                int users = 0; String lastuser = "";
                for (int a = 0; a < list.Count; a++)
                {
                    if (list[a].revid < startid) { continue; }
                    if (!list[a].user.Equals(lastuser)) { users++; lastuser = list[a].user; }
                }

                return users > 1;
            }

        }
}
