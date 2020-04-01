using System;
using System.Collections.Generic;
using System.Text;
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
    class RevDiffData
    {
        public WikiRevision a, b;
        public SweepNetPage page;
        public Element el;
        public bool send_event = false;//отправить событие по окончании
        public double rand = 0;//случайное число
        public int sent = 0;
        public bool send_wo_req = true;
        public String data = "";
        public int type = 0;//0-diff, 1-text, 2-view
    }


    public class RevCache
    {


        CoreOperations aw = null;
        List<RevDiffData> list = new List<RevDiffData>();
        Thread main = null;
        bool thread_running = false;
        bool stop_thread = false;
        private bool rt = false;

        public bool support_thread = false;

        public RevCache(CoreOperations a)
        {
            aw = a;
            new Thread(SupportingThread).Start();
        }

        public void SupportingThread()
        {
            while (1 == 1)
            {
                Thread.Sleep(400);
                if (support_thread)
                {
                    if (thread_running == false)
                    {
                        RunThread();
                    }
                }
            }
        }

        public void AddRev_last(tl a, tl b, int type, Element el, bool restart = true)
        {
            ClearList();
            if (a == null) { a = b; b = null; }
            RevDiffData dd = new RevDiffData();
            dd.a = a.revision; dd.b = b.revision; dd.type = type; dd.page = el.page; dd.el = el;
            dd.rand = a.rand+(b!=null?b.rand:0);
            list.Add(dd);

            if (restart) { RunThread(); }
        }

        private void RequestRestart()
        {
            if (rt == true && thread_running)
            {
                main.Abort();
                RunThread();
            }
            else
            {
                rt = true;
                thread_restart = true;
            }
        }

        private void CheckAllEvents()
        {
            try
            {
                foreach (RevDiffData dd in list)
                {
                    CheckForEvent(dd);
                }
            }
            catch { }
        }

        bool thread_restart = false;


        private void _thread_function()
        {
        // MessageBox.Show("start_thread");
        st:
            rt = false;
            thread_restart = false;
            try
            {
                thread_running = true;
                stop_thread = false;
                long tmpl;

                int current_max = Options.GetOptionInt("buff3");
                bool skipifnull = Options.GetOptionInt("skip_null") == 1 ? true : false;
                // MessageBox.Show(current_max + "");

                //  DiffData dd;
                CheckAllEvents();
                RevDiffData tmp = new RevDiffData();

                for (int a = 0; a < list.Count; a++)
                {
                    double rand = list[a].rand;
                    CheckAllEvents();
                    if (a == current_max) { thread_running = false; return; }
                    if (stop_thread) { thread_running = false; return; }
                    if (thread_restart) { goto st; }
                    if (list[a].data.Equals(""))
                    {
                        //
                        for (int b = 0; b < (a < 2 ? 2 : 1); b++)
                        {
                            tmp = list[a];
                        
                            CheckAllEvents();

                            if (thread_restart) { goto st; }

                            if (stop_thread) { thread_running = false; return; }

                            switch (tmp.type)
                            {
                                case 0:
                                    tmp.data = DiffBase.__GetDiff(aw, tmp.a, tmp.b, tmp.el, true).data;
                                    break;
                                case 1:
                                    tmp.data = DiffBase.__GetDiff(aw, tmp.a, null, tmp.el, true).data;
                                    break;
                                case 2:
                                    tmp.data = DiffBase.__GetView(aw, tmp.a, tmp.el, true);
                                    break;
                            }


                            tmp.data = tmp.data.Trim();
                            if (tmp.Equals("")) { Logging.AddLog("creating revdiff for \"" + tmp.el.title + "\" failed #" + (b + 1)); continue; }

                            Logging.AddLog("diff for \"" + tmp.el.title + "\" successfully loaded #" + (b + 1) + "  [" + tmp.data.Length + "]");

                            int newid = -1;
                            int fnw = 0;

                            for (newid = 0; newid < list.Count; newid++)
                            {
                                if (list[newid].rand == rand)
                                {
                                    fnw = 1;
                                    list[newid].data = tmp.data;
                                    //list[newid].el.loaded = true;
                                   // list[newid].el.MakeRevisions();

                                    if (list[newid].send_wo_req) { list[newid].send_event = true; CheckForEvent(list[newid]); }

                                    if (tmp.data.Contains("<!--diff_is_empty-->") && skipifnull == true && list[newid].send_wo_req == false)
                                    {
                                      //  list[newid].skip = true;
                                    }
                                    break;
                                }
                            }

                            if (fnw == 0) { thread_running = false; Logging.AddLog(list[a] + " ......... 0"); return; }
                            else { }

                            CheckAllEvents();

                            if (thread_restart) { goto st; }
                            break;
                        }
                    }

                }
                CheckAllEvents();
            }
            catch (Exception)
            {
                /* MessageBox.Show(e.Message+"\r\n\r\n"+e.StackTrace);*/
                CheckAllEvents(); thread_running = false; if (thread_restart) { goto st; } return;
            }
            thread_running = false; return;
        }

        public void RunThread()
        {
            thread_restart = false;
            support_thread = true;
            stop_thread = false;
            //thread_restart = false;
            if (thread_running == true) { /*main.Abort(); */Logging.AddLog("restarted revdiff stream"); thread_restart = true; }
            else
            {
                Logging.AddLog("started revdiff stream");

                main = new Thread(_thread_function);
                main.IsBackground = true;

                thread_running = true;
                stop_thread = false;
                main.Start();
            }
        }

        private void CheckForEvent(RevDiffData dd)
        {
            if (dd.sent == 0 && dd.send_event == true && !dd.data.Equals(""))
            {
                dd.sent = 1;
                aw.events.Push("rev" + dd.a.rand+(dd.b!=null?dd.b.rand+"":"")); 
            }
        }

        public bool RequestData(tl a, tl b)
        {
            foreach (RevDiffData dd in list)
            {
                if (dd.a.rand == a.revision.rand && ((dd.b == null && b == null) || (dd.b != null && b != null && dd.b.rand == b.revision.rand)))
                {
                    dd.send_event = true;
                    CheckForEvent(dd);
                    if (!dd.data.Equals("")) { return true; }
                    break;
                }
            }
            return false;
        }

        public String GetRevDiff(tl a, tl b)
        {
            foreach (RevDiffData dd in list)
            {
                if (dd.a.rand == a.revision.rand && ((dd.b == null && b == null) || (dd.b != null && b != null && dd.b.rand == b.revision.rand)))
                {
                    if (dd.data.Equals("")) { list.Remove(dd); return ""; }
                    String tmp = dd.data + "";
                    return tmp;
                }
            }
            return "";
        }

        public void Skip(Element el)
        {
            foreach (RevDiffData dd in list)
            {
                if (dd.rand == el.rand)
                {
                    list.Remove(dd);
                    RequestRestart();
                    break;
                }
            }
        }

        public void ClearList()
        {
            list.Clear();
            stop_thread = true;
        }

        public void Stop()
        {
            stop_thread = true;
            support_thread = false;
            thread_running = false;
            try
            {
                if (main != null) { main.Abort(); }
            }
            catch { }
        }

        public void Restart()
        {
            if (thread_running == true && main != null)
            {
                RunThread();
            }
        }
    }
}
