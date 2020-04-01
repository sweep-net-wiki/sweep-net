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
    public class DiffDataResult
    {
        public string data { get; set; }
        public List<DiffPiece> pieces { get; set; }
        public Element element { get; set; }

        public DiffDataResult(string a, List<DiffPiece> b, Element c)
        {
            data = a; pieces = b; element = c;
        }
    }

    public class DiffData
    {
        public Element el;
        public bool send_event = false;//отправить событие по окончании
        public double rand = 0;//случайное число
        public int sent = 0;
        public bool skip = false;
        public bool send_wo_req = false;
        public String data = "";
        public String data2 = "";
        public List<DiffPiece> pieces;
    }

    public class DiffCache
    {


        CoreOperations aw = null;
        List<DiffData> list = new List<DiffData>();
        Thread main = null;
        bool thread_running = false;
        bool stop_thread = false;
        private String tmp = "";
        private bool rt = false;

        public int data_type = 0;//0 - diff, 1 - rendered view
        public bool support_thread = false;


        public DiffCache(CoreOperations a)
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
                    if (thread_running == false && list.Count > 0)
                    {
                        int current_max = Options.GetOptionInt("buff3");
                        current_max = list.Count > current_max ? current_max : list.Count;
                        bool need=false;
                        for (int i = 0; i < current_max; i++)
                        {
                            if (list[i].data.Equals("")) { need = true; }
                        }
                        if(need)
                            RunThread();
                    }
                }
            }
        }

        public void AddRev_last(Element el, bool restart = true)
        {
            DiffData dd = new DiffData();
            dd.el = el;
            dd.rand = el.rand;
            CheckDup(el);
            list.Add(dd);

            if (restart) { RunThread(); }
        }

        public void AddRev_first(Element el, bool restart = true, bool sworq = false)
        {
            try
            {
                support_thread = false;
                Stop();
                DiffData dd = new DiffData();
                dd.el = el; dd.send_wo_req = sworq; dd.rand = el.rand;
                CheckDup(el);
                Stop();
                list.Insert(0, dd);
                Logging.AddLog(el.title + " inserted");
            }
            catch (Exception e) { MessageBox.Show(e.Message); }
            if (restart) { support_thread = true; thread_running = false; RunThread(); }
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

        private void CheckDup(Element el)
        {
            //foreach (DiffData dd in list)
            for (int a = 0; a < list.Count; a++)
            {
                if (el.stable_id > 0 && el.curr_id > 0 && list[a].el.stable_id == el.stable_id && list[a].el.curr_id <= el.curr_id)
                {
                    list.RemoveAt(a);
                    break;
                }
            }
        }

        private void CheckAllEvents()
        {
            try
            {
                foreach (DiffData dd in list)
                {
                    CheckForEvent(dd);
                }
            }
            catch{  }
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

                int current_max = Options.GetOptionInt("buff3");
                bool skipifnull = Options.GetOptionInt("skip_null") == 1 ? true : false;
                // MessageBox.Show(current_max + "");

                //  DiffData dd;
                CheckAllEvents();
                DiffData tmp = new DiffData();

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
                            if (tmp.skip == true) { break; }

                            CheckAllEvents();

                            if (thread_restart) { goto st; }


                            if (stop_thread) { thread_running = false; return; }

                            Logging.AddLog("Trying to load diff for \"" + tmp.el.title + "\" #" + (b + 1));

                          //  try
                            if (tmp.el.page.list.Count == 0)
                            {
                                if (tmp.el.check == true)
                                {
                                    tmp.el.page.LoadRevision(-1);
                                    lock (aw.list.list) { tmp.el.Check(); }
                                    if (tmp.el.skip_me)
                                    {
                                        tmp.skip = true; tmp.el.skipped = true;
                                        tmp.data = "В настройках отключено";
                                        break;
                                    }
                                }
                                else
                                {
                                    tmp.el.skipped = false;
                                    tmp.el.page.LoadRevision(tmp.el.stable_id);
                                }
                            }
                            else
                            {
                                if (tmp.el.check == true)
                                {
                                    lock (aw.list.list) { tmp.el.Check(); }
                                    if (tmp.el.skip_me)
                                    {
                                        tmp.skip = true; tmp.el.skipped = true;
                                        tmp.data = "В настройках отключено";
                                        break;
                                    }
                                }
                            }
                            //catch (Exception e) { Logging.AddLog("Exception while loading diff for \"" + tmp.el.title + "\": " + e.Message); continue; }

                          //  try
                            if (tmp.el.page.list.Count == 0 || tmp.el.error)
                            {
                                tmp.data = "API error.";
                            }else
                            {
                                if (data_type == 0)
                                {
                                    DiffTemporaryResult dtr=DiffBase._GetDiff(aw,tmp.el);
                                    tmp.data = dtr.data;
                                    tmp.pieces = dtr.pieces;
                                }
                                else
                                {
                                    tmp.data = DiffBase._GetView(aw,tmp.el);
                                }
                            }

                            tmp.data = tmp.data.Trim();
                            if (tmp.Equals("")) { Logging.AddLog("creating diff for \"" + tmp.el.title + "\" failed #" + (b + 1)); continue; }

                            Logging.AddLog("diff for \"" + tmp.el.title + "\" successfully loaded #" + (b + 1) + "  [" + tmp.data.Length + "]");

                            int newid = -1;
                            int fnw = 0;

                            for (newid = 0; newid < list.Count; newid++)
                            {
                                if (list[newid].rand == rand)
                                {
                                    fnw = 1;
                                    list[newid].data = tmp.data;
                                    list[newid].el.loaded = true;
                                    list[newid].el.MakeRevisions();

                                    if (list[newid].send_wo_req) { CheckForEvent(list[newid]); list[newid].send_event = true; }

                                    if (tmp.data.Contains("<!--diff_is_empty-->") && skipifnull == true && list[newid].send_wo_req == false)
                                    {
                                        list[newid].skip = true;
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
            catch (Exception) { 
             /*   MessageBox.Show(e.Message+"\r\n\r\n"+e.StackTrace); */CheckAllEvents(); thread_running = false; if (thread_restart) { goto st; } return; }
            thread_running = false; return;
        }

        public void RunThread()
        {
            try
            {
                thread_restart = false;
                support_thread = true;
                stop_thread = false;
                //thread_restart = false;
                if (thread_running == true && main != null && main.IsAlive) { /*main.Abort(); */Logging.AddLog("restarted diff stream"); thread_restart = true; }
                else
                {
                    Logging.AddLog("started diff stream");

                    main = new Thread(_thread_function);
                    main.IsBackground = true;

                    thread_running = true;
                    stop_thread = false;

                    main.Start();
                }
                // myThread.Start();
            }
            catch (Exception) { }
        }

        private void CheckForEvent(DiffData dd)
        {
            if (dd.sent < 2 && dd.send_event == true && !dd.data.Equals(""))
            {
                
                // aw.events.Push("diff" + dd.curr + dd.stab);
                if (dd.skip == true) { aw.events.Push("skip" + dd.el.rand); }
                else { aw.events.Push("diff" + dd.el.rand); }
                dd.sent++;
                //  MessageBox.Show("pushed "+dd.title);
            }
        }

        public bool RequestData(Element el)
        {
            foreach (DiffData dd in list)
            {
                if (dd.rand == el.rand)
                {
                    dd.send_event = true;
                    Logging.AddLog("diff for \"" + dd.el.title + "\" requested");
                    CheckForEvent(dd);
                    /*if (dd.sent == 0 || thread_running == false)
                    {
                        RunThread();
                    }*/
                    if (!dd.data.Equals("")) { return true; }
                    //break;
                }
                else
                {
                    dd.send_event = false;
                }
            }
            return false;
        }

        public DiffDataResult GetRevDiff(Element el)
        {
            foreach (DiffData dd in list)
            {
                if (dd.rand==el.rand)
                {
                    if (dd.data.Equals("")) { list.Remove(dd); return null; }
                    dd.sent = 100;
                    Logging.AddLog("diff for \"" + dd.el.title + "\" successfully sent [" + tmp.Length + "]");
                    return new DiffDataResult(dd.data+"", dd.pieces, dd.el);
                }
            }
            return null;
        }

        public void Skip(Element el)
        {
            foreach (DiffData dd in list)
            {
                if (dd.rand==el.rand)
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

        public void FullReset()
        {
            foreach (DiffData dd in list)
            {
                dd.data = "";
                dd.el.loaded = false;
                dd.data2 = "";
            }
            Restart();
        }

        public void Restart()
        {
            if (thread_running == true && main != null)
            {
                RunThread();
            }
        }

        public void ChangeDatas()
        {
            String t = "";
            foreach (DiffData dd in list)
            {
                t = dd.data2;
                dd.data2 = dd.data;
                dd.data = t;
            }
        }

        public void SwitchToDiffs()
        {
            if (data_type == 1) { ChangeDatas(); Stop(); RunThread(); }
            data_type = 0;
        }

        public void SwitchToView()
        {
            if (data_type == 0) { ChangeDatas(); Stop(); RunThread(); }
            data_type = 1;
        }
    }
}