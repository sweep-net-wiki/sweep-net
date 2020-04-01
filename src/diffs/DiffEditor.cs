using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Windows.Forms;
using System.Threading;
using DotNetWikiBot;

namespace sweepnet
{

    public delegate void ProceedToNextPageAction(object o);

    public class DiffPiece
    {
        public bool ready { get; set; }
        public int starting { get; set; }
        public string text { get; set; }
        public bool being_edited { get; set; }
        //
        public int real_line { get; set; }
        public string wikitext { get; set; }
    }

    public class DiffEditor
    {
        public List<DiffPiece> pieces { get; set; }
        public Element element { get; set; }
        public Hashtable temp { get; set; }
        public event ProceedToNextPageAction ProceedToNextPage;


        private CoreOperations aw;
        private WebBrowser wb;
        private System.Threading.Timer timer;
        private Thread thread;
        private DiffPiece requested = null;
        private long milliseconds = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

        public DiffEditor(CoreOperations _aw)
        {
            aw = _aw;
            timer = new System.Threading.Timer(new TimerCallback(TimerTick), new AutoResetEvent(false), 1000, 1000);
        }

 
        /* Initializing the class */
        public void Initialize(List<DiffPiece> list, Element el, WebBrowser _wb)
        {
            pieces = list; element=el; wb = _wb;
            if (temp == null) { temp = new Hashtable(); }
            else { temp.Clear(); }
            wb.Document.MouseMove += new HtmlElementEventHandler(MouseMove);
            wb.Document.Click += new HtmlElementEventHandler(MouseClick);
        }

        public void Stop()
        {
            if (thread != null && thread.IsAlive)
            {
                thread.Abort();
                requested = null;
                temp = null;
                wb = null;
            }
        }

        private void MouseClick(object sender, HtmlElementEventArgs e)
        {
            try
            {
                e.ReturnValue = true;

                if (wb == null || temp == null) return;
                HtmlElement p = wb.Document.GetElementFromPoint(e.ClientMousePosition);
                if (p == null) return;
                string j = p.GetAttribute("buttonid");
                if (j == null || j == "")
                {
                    p = p.Parent;
                    j = p.GetAttribute("buttonid");
                }
                if (j == null || j == "")
                {
                    p = p.Parent;
                    j = p.GetAttribute("buttonid");
                }

                if (p != null) p = p.Parent;
                if (p == null || j == "") return;

                int n = int.Parse(j);
                if (n == 0) // SAVE CLICKED
                {
                    if (thread != null && thread.IsAlive)
                    {
                        thread.Abort(); thread.Join();
                    }
                    thread = new Thread(new ThreadStart(ThreadFunction));
                    thread.IsBackground = true;
                    thread.Start();
                    e.ReturnValue = true;
                    return;
                }
                n--;
                if (n >= 0 && n < pieces.Count)
                {
                    if (!pieces[n].being_edited)
                    {
                        p.InnerHtml = @"<textarea id=""textarea_"+(n+1)+@""" style=""position:absolute;top:0px;bottom:0px;left:0px;right:0px;width:100%;height:100%;"">" + Tools.HtmlEncode(pieces[n].text) + "</textarea>";
                        pieces[n].being_edited = true;
                        ShowButtons();
                    }
                    return;
                }
            }catch{
            }
        }

        private void MouseMove(object sender, HtmlElementEventArgs e)
        {
            try
            {
                long newt = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                if (newt - milliseconds < 40) return;
                milliseconds = newt;


                if (wb == null || temp == null || requested!=null) return;
                HtmlElement p = wb.Document.GetElementFromPoint(e.ClientMousePosition);
                if (p == null) return;

                string j = p.GetAttribute("myindex");
                if (j == null || j == "")
                {
                    p = p.Parent;
                    if (p == null) return;
                    j = p.GetAttribute("myindex");
                }
                if (j == null || j == "")
                {
                    p = p.Parent;
                    if (p == null) return;
                    j = p.GetAttribute("myindex");
                }
                if (j != null && j != "" )
                {
                    int q = int.Parse(j)-1;
                    if (pieces[q].being_edited) return;

                    if (!temp.ContainsKey(j)) { temp.Add(j, 0); }
                    HtmlElement m = wb.Document.GetElementById("button_edit_" + j);
                    if (int.Parse(temp[j].ToString()) == 0) { m.Style = @"visibility:visible"; }
                    temp[j] = 2;
                }
            }
            catch (Exception) { }
        }

        /* Applying changes */
        private void ThreadFunction()
        {
            try
            {
                if (aw != null && wb != null)
                {

                    Page p = aw.PageCreate(element.page.title);
                    p.Load();

                    if (p.text != "")
                    {
                        string[] lines = p.text.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
                        foreach (DiffPiece d in pieces)
                        {
                            int from = d.starting;
                            for (int k = (from - 1) >= 0 ? (from - 1) : 0; k < lines.Length; k++)
                            {
                                if (lines[k] == d.text)
                                {
                                    d.real_line = k;
                                    d.ready = true;
                                    break;
                                }
                            }
                        }


                        //  EditAndReview(this, new EditAndReviewActionArguments(element, p.text));
                        bool error = false, done = false;
                        lock (wb)
                        {
                            for (int i = 0; i < pieces.Count; i++)
                            {
                                if (pieces[i].being_edited)
                                {
                                    if (!pieces[i].ready) error = true;
                                    else
                                    {
                                        HtmlElement el = null;
                                        wb.Invoke((MethodInvoker)delegate
                                {
                                    el = wb.Document.GetElementById("textarea_" + (i + 1).ToString());
                                });
                                        if (el == null) error = true;
                                        else
                                        {
                                            lines[pieces[i].real_line] = el.InnerText;
                                            done = true;
                                        }
                                    }
                                }
                            }
                        }

                        if (error)
                        {

                            string errstr = "Due to internal errors the application did not manage to apply the changes you've made.".T();
                            wb.Invoke((MethodInvoker)delegate
                            {
                                MessageBox.Show(errstr, "Error", MessageBoxButtons.OK);
                            });
                            return;
                        }

                        string summary = ""; bool success = false, minor = false;
                        wb.Invoke((MethodInvoker)delegate
                                {
                                    TextInputForm form = new TextInputForm("Enter a summary".T(), "([[Project:Sweep-Net|Sweep-Net]]): ");
                                    form.ShowDialog((IWin32Window)wb.Parent);
                                    if (form.successfully)
                                    {
                                        success = true;
                                        summary = form.text;
                                    }

                                    HtmlElement el = wb.Document.GetElementById("is_minor");
                                    if (el != null) minor = el.GetAttribute("checked").ToLower() == "true";
                                });

                        if (success)
                        {
                            wb.Invoke((MethodInvoker)delegate
                            {
                                SetUpdatingStatus();
                            });
                            //    MessageBox.Show(String.Join("\r\n", lines));
                            p.Save2(String.Join("\r\n", lines), summary, minor);
                            SweepNetPage tp = new SweepNetPage(aw.mysite, p.title);

                            element.page.ReloadRevision();
                            //tp.LoadRevision(Convert.ToInt64(p.lastRevisionID));

                            if (element.page.maxid > 0)
                            {
                                element.curr_id = element.page.curr.revid;

                                //обновить дифф

                                aw.UpdateDiff(element);
                                wb.Invoke((MethodInvoker)delegate
                                {
                                    SetUpdatedStatus();
                                });
                            }
                        }

                    }
                }
            }
            catch (Exception e)
            {
                //    MessageBox.Show(e.Message+e.StackTrace);
            }

        }

        private void TimerTick(Object stateInfo)
        {
            if (wb == null || temp == null) return;
            lock (temp)
            {
                List<object> list = new List<object>();
                foreach (object p in temp.Keys)
                {
                    list.Add(p);
                }
                foreach(object p in list){
                    int q = int.Parse(temp[p].ToString());
                    if (q > 0)
                    {
                        temp[p] = q - 1;
                    }
                    else
                    {
                        wb.Invoke((MethodInvoker)delegate {
                            HtmlElement h = wb.Document.GetElementById("button_edit_" + p.ToString());
                            if (h != null) h.Style = @"visibility:hidden";
                        });
                    }
                }
            }
        }

        private void ShowButtons()
        {
            if (wb != null)
            {
                HtmlElement el = wb.Document.GetElementById("for_buttons");
                if (el != null)
                {
                    el.InnerHtml = @"<br /><input type=""checkbox"" id=""is_minor"" />"+"Minor change?".T()+@"<br /><input type=""button"" buttonid=""0"" class=""mybutton"" value="""+"Apply changes".T()+@"""/>";
                }
            }
        }

        private void SetUpdatingStatus()
        {
            if (wb != null)
            {
                HtmlElement el = wb.Document.GetElementById("for_buttons");
                if (el != null)
                {
                    el.InnerHtml = "<br />"+"Wait.. applying changes..".T();
                }
            }
        }

        private void SetUpdatedStatus()
        {
            if (wb != null)
            {
                HtmlElement el = wb.Document.GetElementById("for_buttons");
                if (el != null)
                {
                    el.InnerHtml = "<br />" + "The changes have been successfully applied! You can now review the page.".T();
                }
            }
        }

        public static List<DiffPiece> SplitIntoPieces(ref string data)
        {
            List<DiffPiece> list = new List<DiffPiece>();

            int current_starting = 0;
            bool booltmp;
            string newdata = data;
            while (data.Contains("<tr>"))
            {

                data = data.Substring(data.IndexOf("<tr>") + 4);
                string tr = data.Substring(0, data.IndexOf("</tr>") + 4), newtr = tr;

                string test = data;

                int p = test.IndexOf("</tr>");
                if (p >= 0 && test.Contains("<td"))
                {
                    test = test.Substring(0, p);
                    bool newstart = test.Contains("class=\"diff-lineno\"");

                    test = test.Substring(test.LastIndexOf("<td") + 3);
                    test = test.Substring(test.IndexOf('>') + 1);

                    if (newstart)
                    {
                        string s = "";
                        for (int i = 0; i < test.Length; i++)
                            if (char.IsNumber(test[i])) s += test[i];
                        if (s != "")
                        {
                            current_starting = int.Parse(s);
                        }
                        newtr = Tools.ReplaceLast(tr, test, test.Replace("</td>", @"<span id=""for_buttons""></span></td>"));
                    }
                    else
                    {
                        string temp = test.Substring(0, test.IndexOf("</td>")).Trim();
                        if (!temp.Equals(""))
                        {
                            DiffPiece dp = new DiffPiece();

                            dp.starting = current_starting;
                            dp.ready = false;
                            dp.text = "";
                            dp.being_edited = false;
                            dp.text = HttpUtility.HtmlDecode(Tools.StripHtmlTags(temp));
                            if (dp.text.Trim() != "")
                            {
                                list.Add(dp);
                                newtr = Tools.ReplaceLast(tr, ">" + test, " myindex=\"" + list.Count + "\">" + @"<a href=""about:edit"" class=""editbutton"" style=""visibility:hidden;"" buttonid="""+list.Count+@""" id=""button_edit_" + list.Count + @"""><img class=""editimg""></a>" + test);
                            }
                        }

                    }


                }
                if (tr != newtr) newdata = Tools.ReplaceFirst(newdata, tr, newtr);

            }
            data = newdata;
            return list;
        }
    }
}
