using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Web;
using System.Drawing;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading;

using System.Xml;
using DotNetWikiBot;
using sweepnet.core;



namespace sweepnet
{
    public partial class MainForm : Form
    {
        public Element current_revision = null;
        public tl curr2a = null, curr2b = null; // выделенные версии проф. режима; одинаковы - выделена одна правка
        public tl curr2ap = null, curr2bp = null;
        public tl curr2wa = null, curr2wb = null;// дифф ожидается для этих границ
        public String waiting_diff = "";
        public DiffDataResult current_diff = null;

        public CoreOperations aw;
        public RevCache rc;
        public DiffEditor de;
        public AuthorsRequests ar;
        public String domain = "";

        public WorkList wl;
        public RevList rl;

        private List<WikiRevision> rw_waiting_list = null;

        public delegate void _tevent();
        _tevent tevent;
        public delegate void _pnp();
        _pnp pnp;

        bool activated = false;
        bool new_mode = false;

        bool list_visible = true;
        bool revs_visible = false;
        bool started = false;

        bool running = false;
        bool startc_showed = false;

        bool prof_group = false;

        int current_direction = 0; //0-next,1-prev
        int clear_stip = 0;
        bool wb_clear = false;
        String diff_wb = "";
        TextInputForm tmp_tif = null;

        public MainForm()
        {
            Version.single = Version.IsSingleInstance();
            if (!Version.single)
            {
                MessageBox.Show("Внимание! Обнаружена другая работающая копия этой программы. Во избежание возможных проблем сохранение настроек будет отключено.");
            }
            GlobalMouseHandler gmh = new GlobalMouseHandler();
            gmh.TheMouseMoved += new MouseMovedEvent(gmh_TheMouseWheelMoved);
            Application.AddMessageFilter(gmh);
            Start();
        }


        public string T(string n)
        {
            if (!DesignMode)
                return i18n.GetXml(n);
            else return n;
        }

        private void gmh_TheMouseWheelMoved()
        {
            if (!started)
                return;

            Point cur_pos = System.Windows.Forms.Cursor.Position;
            if (diff_browser.RectangleToScreen(diff_browser.DisplayRectangle).Contains(cur_pos))
            {
                diff_browser.Document.Focus();
            }
            else if (listbox.RectangleToScreen(diff_browser.DisplayRectangle).Contains(cur_pos))
            {
                listbox.Focus();
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (diff_browser != null && diff_browser.Document != null && diff_browser.Document.ActiveElement != null && diff_browser.Document.ActiveElement.TagName.ToUpper() == "TEXTAREA")
                return base.ProcessCmdKey(ref msg, keyData);

            if (running && current_revision != null)
            {
                if (keyData == AllHotKeys.list["review"].key)
                {
                    DoReviewArticle(true); return true;
                }
                else if (keyData == AllHotKeys.list["next"].key)
                {
                    DoNextArticle(); return true;
                }
                else if (keyData == AllHotKeys.list["revert"].key)
                {
                    DoRevertArticle(); return true;
                }
                else if (keyData == AllHotKeys.list["scroll"].key)
                {
                    diff_browser.Document.Body.ScrollTop += (int)(diff_browser.Height * 0.9);
                    return true;
                }
                else if (keyData == AllHotKeys.list["authors"].key)
                {
                    if (show_user_info_button.Checked)
                    {
                        HideUserInfoBox();
                        show_user_info_button.Checked = false;
                    }
                    else
                    {
                        ShowUserInfoBox();
                        show_user_info_button.Checked = true;
                    }
                    return true;
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            UpdateLanguage();
        }

        // Глобальные функции


        public void Start()
        {
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;

            Options.LoadOptions();
            StreamConfig.LoadOptions();

            Authorize();
            InitForm();
            StartMainThreads();

            started = true;
        }

        public void CheckEventsThread()
        {
            while (true)
            {
                Thread.Sleep(70);
                try
                {
                    Invoke(tevent);
                }
                catch { continue; }
            }
        }

        public void Start_Work()
        {
            try
            {
                ActivateAll();
                aw.updating = false;

                running = true;
                current_revision = null;
                aw.Update();
                SetStatus(null, T("Updating.."));

                SwitchNewMode(StreamConfig.GetMode() == 0);
                SwitchView(0);

                SwitchRevbox(false);
                SwitchListbox(true);
            }
            catch { }
        }

        public void Stop_Work()
        {
            try
            {
                DeactivateAll();
                if (aw.CurrUpdThread != null) { aw.CurrUpdThread.Abort(); }
                aw.Clear();
                wl.Clear();
                label1.Text = "";
                aw.dc.ClearList();
                running = false;
                ClearBrowsers("");
                SetStatus("", "");
                current_revision = null;

                SwitchListbox(false);
                SwitchRevbox(false);
            }
            catch (Exception) { }
        }

        public void NextArticle()
        {
            try
            {
                if (current_revision != null) { aw.dc.Skip(current_revision); }
                if (aw.GetCountOfAlailableData() == 0)
                {
                    return;
                }
                current_revision = null;
                current_revision = aw.GetRevision();

                if (current_revision == null) { current_revision = null; DeactivateAll(); return; }

                current_direction = 0;
                current_revision.showed = true;

                Invoke(this.pnp);
            }
            catch (Exception) { }
        }

        public void PrevArticle()
        {
            try
            {
                if (current_revision != null) { aw.dc.Skip(current_revision); }
                current_revision = null;
                current_revision = aw.GoBack();
                if (current_revision == null) { MessageBox.Show(T("You can't step back!")); return; }//Назад шагнуть нельзя!
                current_direction = 1;
                Invoke(this.pnp);
            }
            catch (Exception) { }
        }

        public void ClosingForm()
        {
            Version.Stop();
            aw.dc.Stop();

            Environment.Exit(-1);
        }

        public void Authorize()
        {
            AuthForm auth_form = new AuthForm();
            aw = new CoreOperations();
            while (true)
            {
                auth_form.ShowDialog();
                if (auth_form.status == AuthForm.AuthFormStatus.Cancelled)
                {
                    Environment.Exit(-1);
                }

                domain = auth_form.domain;
                if (aw.Connect(domain, auth_form.login, auth_form.password))
                {
                    if (!aw.IsEditor())
                    {
                        MessageBox.Show(T("You dont have an 'editor' flag!"));
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    MessageBox.Show(T("Wrong login or password!"));
                }
                auth_form = new AuthForm();
                continue;
            }
        }



        public void InitForm()
        {
            InitializeComponent();
            DeactivateAll();
            if (Options.GetOptionInt("check_updates") == 1) { Version.CheckUpdates(9); }
            wl = new WorkList(listbox);
            rl = new RevList(revbox, aw);
            rl.UpdateEvent += Rev_SelUpdate;

            rc = new RevCache(aw);
            ar = new AuthorsRequests(aw);
            de = new DiffEditor(aw);

            this.Text = Version.GetName() + " " + Version.GetVersion();
        }

        public void StartMainThreads()
        {
            tevent = new _tevent(CheckEvents);
            pnp = new _pnp(ProcessNextPage);

            Thread tev = new Thread(CheckEventsThread);
            tev.IsBackground = true;
            tev.Start();

            Thread t = new System.Threading.Thread(CheckStatusPanel);
            t.IsBackground = true;
            t.Start();
        }

        public void CheckEvents()
        {
            DoUpdateTraffic();
            String ev = aw.GetEvent();
            if (ev == null) { return; }

            switch (ev)
            {
                case "endupdating":
                    SetStatus(null, T("Updated"));//Обновление завершено
                    if (aw.GetCountOfAlailableData() == 0) { SetStatus(null, T("No data")); }//Нет подходящих данных
                    else
                    {
                        if (!activated)
                        {
                            ActivateAll();
                        }

                        if (current_revision == null)
                        {
                            Invoke((MethodInvoker)delegate { wl.UpdateItems(aw); });

                            NextArticle();
                        }
                    }
                    break;
                case "updatingerr":
                    MessageBox.Show(T("An error occured whlie updating the list! Restart it."));//Ошибка обновления списка! Перезапустите
                    break;
                case "updstarted":
                    SetStatus(null, T("Updating.."));//Начало обновления
                    break;
                case "reviewed":
                    UpdateStatistics();
                    break;
                case "upd-ex":
                    MessageBox.Show(T("An error occured whlie updating the list! Restart it."));//Исключение в потоке! Перезапустите его
                    Stop_Work();
                    break;
                case "nodata_fin":
                    if (current_revision == null)
                    {
                        MessageBox.Show(T("We've run out of available articles! It is time to change the stream settings."));//Данные для патрулирования закончились! Измените настройки потока!
                        Stop_Work();
                    }
                    break;
                case "zero_and_started":
                    MessageBox.Show(T("The list is not yet completely fetched! Wait"));//Списки еще не успели загрузиться! Подождите
                    break;
                case "nodata":
                    if (StreamConfig.GetMode() == 0) { MessageBox.Show(T("We've run out of available data! Wait for a while and restart the stream.")); }//Пока нет данных для потока! Подождите некоторое время и перезапустите поток.
                    Stop_Work();
                    break;
                case "reverted":
                    SetStatus(null, T("The changes have been reverted."));//Правки отклонены
                    break;

                default:
                    if (current_revision != null)
                    {
                        if (ev.Equals("diff" + current_revision.rand))
                        {
                            current_diff = aw.dc.GetRevDiff(current_revision);
                            if (current_diff == null) return;
                            String q = current_diff.data;
                            Logging.AddLog("diff for " + current_revision.title + " sent to browser [" + q.Length + "]");
                            diff_browser.DocumentText = current_diff.data; //@"<html><body><span id=""TMP"">A</span></body></html>";

                            Invoke((MethodInvoker)delegate
                            {
                                de.Initialize(current_diff.pieces, current_diff.element, diff_browser);
                            });
                            label1.Text = String.Format(T("Page: {0};      Changes: {1} bytes"), current_revision.title, (current_revision.loaded ? ((current_revision.diff_size > 0 ? "+" : "") + current_revision.diff_size) : "?"));// "Статья: {0};       Изменения: {1} байт";
                            diff_wb = current_diff.data;
                            pro_mode.Enabled = current_revision.page.CheckProfMode();

                        }
                        else if (ev.Equals("skip" + current_revision.rand))
                        {
                            de.Stop();
                            if (current_direction == 0) { NextArticle(); }
                            else { PrevArticle(); }
                        }
                        else if (ev.Equals("unreviewed" + current_revision.curr_id))
                        {
                            review_button.Checked = false; UpdateStatistics();
                            SetStatus(null, T("Unreviewed")); //Отметка снята.
                            review_button.Text = T("Review") + " (" + AllHotKeys.list["review"].name + ")";
                        }
                        else if (ev.Equals("reviewed" + current_revision.curr_id))
                        {
                            review_button.Checked = true;
                            review_button.Text = T("Unreview") + " (" + AllHotKeys.list["review"].name + ")";
                        }
                        else if (curr2a != null && ev.Equals(waiting_diff))
                        {
                            de.Stop();
                            String q = aw.rrc.GetRevDiff(curr2wa, curr2wb);
                            Logging.AddLog("rev diff for " + current_revision.title + " sent to browser [" + q.Length + "]");
                            diff_browser.DocumentText = q;
                            rl.ScrollTo(curr2wb);
                        }
                        else if (curr2a != null && ev.Equals("rev" + curr2a.revision.rand + (curr2b != null ? curr2b.revision.rand + "" : "")))
                        {
                            de.Stop();
                            String q = aw.rrc.GetRevDiff(curr2a, curr2b);
                            Logging.AddLog("rev diff for " + current_revision.title + " sent to browser [" + q.Length + "]");
                            diff_browser.DocumentText = q;
                            rl.ScrollTo(curr2b);
                        }
                        else if (current_revision != null && ev.Equals("AuthorsReady" + current_revision.page.stable.revid + "_" + current_revision.page.curr.revid))
                        {
                            HtmlElement e = diff_browser.Document.GetElementById("authors");
                            if (e != null) e.InnerHtml = AuthorsRequests.MakeUpTable(ar.result);
                            diff_browser.Document.Body.ScrollTop = 0;
                        }
                    }
                    if (ev.StartsWith("notreviewed_"))
                    {
                        ev = ev.Replace("notreviewed_", "");
                        // MessageBox.Show("При попытке отпатрулировать статью \""+ev+"\" возникло исключение.");
                        lttr(ev);
                    }
                    else if (ev.StartsWith("notreviewed-"))
                    {
                        ev = ev.Replace("notreviewed-", "");
                        lttr(ev);
                        // MessageBox.Show("Из-за проблем на сервере на удалось отпатрулировать статью \"" + ev + "\"");
                    }
                    else if (rw_waiting_list != null && ev.StartsWith("reviewed"))
                    {
                        de.Stop();
                        for (int a = 0; a < rw_waiting_list.Count; a++)
                        {
                            if (ev.Equals("reviewed" + rw_waiting_list[a].revid))
                            {
                                rw_waiting_list[a].flagged = true;
                                rw_waiting_list.RemoveAt(a);
                                current_revision.page.UpdateRwStatus();
                                DoProfCheckReviewButton();
                                current_revision.stable_id = current_revision.page.stable.revid;
                                break;
                            }
                        }
                    }
                    else if (rw_waiting_list != null && ev.StartsWith("unreviewed"))
                    {
                        de.Stop();
                        for (int a = 0; a < rw_waiting_list.Count; a++)
                        {
                            if (ev.Equals("unreviewed" + rw_waiting_list[a].revid))
                            {
                                rw_waiting_list[a].flagged = false;
                                rw_waiting_list.RemoveAt(a);
                                current_revision.page.UpdateRwStatus();
                                DoProfCheckReviewButton();
                                current_revision.stable_id = current_revision.page.stable.revid;
                                break;
                            }
                        }
                    }
                    break;

            }
        }

        public void ScrollBrowser()
        {
            HtmlDocument doc = diff_browser.Document;
            HtmlElement elem = doc.GetElementById("first_difference");
            if (elem != null)
            {
                elem.ScrollIntoView(true);
            }
        }

        void HideUserInfoBox()
        {
            if (current_revision != null && current_revision.page != null && current_revision.page.curr != null && current_revision.page.stable != null && current_revision.page.curr.revid > current_revision.page.stable.revid)
            {
                HtmlElement e = diff_browser.Document.GetElementById("authors");
                if (e != null)
                    e.InnerHtml = "";
            }
        }

        void ShowUserInfoBox()
        {
            if (current_revision != null && current_revision.page != null && current_revision.page.curr != null && current_revision.page.stable != null && current_revision.page.curr.revid > current_revision.page.stable.revid)
            {
                ar.RequestAuthors(current_revision.page, current_revision.page.stable.revid, current_revision.page.curr.revid);
            }
        }

        private void SetEnable(bool value)
        {
            activated = value;

            review_button.Enabled = value;
            reject_button.Enabled = value;
            edit_page_button.Enabled = value;
            button_show_stable_version_in_browser.Enabled = value;
            next_article_button.Enabled = value;
            diff_button.Enabled = value;
            cancel_button.Enabled = value;
            button_show_in_browser.Enabled = value;
            show_history_in_browser.Enabled = value;
            back_article_button.Enabled = value;
            edit_inside_wikipedia.Enabled = value;
            d_diff.Enabled = value;
            d_view.Enabled = value;
            splitContainer1.Enabled = value;
            splitContainer2.Enabled = value;
            restart_stream_button.Enabled = value;
            show_user_info_button.Enabled = value;
            existence_button.Enabled = value;
            if (value == false) { pro_mode.Enabled = value; }
        }

        public void DeactivateAll()
        {
            SetEnable(false);
            ClearBrowsers("Sweep-Net");
        }

        public void ActivateAll()
        {
            SetEnable(true);
        }

        public void SetStatus(String s1, String s2)
        {
            if (s1 != null) { toolStripStatusLabel1.Text = s1; }
            if (s2 != null) { toolStripStatusLabel2.Text = s2; }
            clear_stip = 8;
        }

        public void CheckStatusPanel()
        {
            try
            {
                while (true)
                {
                    if (clear_stip == 0)
                    {
                        toolStripStatusLabel2.Text = "";
                        clear_stip--;
                    }
                    if (clear_stip > 0)
                    {
                        clear_stip--;
                    }
                    Thread.Sleep(1000);
                }
            }
            catch { }
        }

        public void ClearBrowsers(String text)
        {
            diff_browser.DocumentText = "<html><body>" + text + "</body></html>";
        }

        public void UpdateStatistics()
        {
            toolStripStatusLabel1.Text = aw.pc + "/" + aw.wc;
        }

        public void ProcessNextPage()
        {
            try
            {
                ClearBrowsers("");
                SwitchView(0);
                pro_mode.Enabled = false;

                if (aw.CanBack()) { back_article_button.Enabled = true; }
                else { back_article_button.Enabled = false; }

                if (current_revision.reviewed) { review_button.Checked = true; review_button.Text = T("Unreview") + " (" + AllHotKeys.list["review"].name + ")"; }//Снять отметку
                else { review_button.Checked = false; review_button.Text = T("Review") + " (" + AllHotKeys.list["review"].name + ")"; }//Отпатрулировать

                if (current_revision.rejected) { reject_button.Enabled = false; cancel_button.Enabled = false; }
                else { reject_button.Enabled = true; cancel_button.Enabled = true; }

                if (current_revision.stable_id == -1) { diff_button.Enabled = false; }
                else { diff_button.Enabled = true; }

                show_user_info_button.Enabled = true;
                show_user_info_button.Checked = false;
                HideUserInfoBox();

                aw.dc.RequestData(current_revision);

                label1.Text = String.Format(T("Page: {0};      Changes: {1} bytes"), current_revision.title, (current_revision.loaded ? ((current_revision.diff_size > 0 ? "+" : "") + current_revision.diff_size) : "?"));// "Статья: {0};       Изменения: {1} байт";

                UpdateStatistics();
                wl.ScrollTo(current_revision);
                wl.UpdateItems(aw, false);

                show_user_info_button.Enabled = current_revision.page.CheckAuthors();
            }
            catch (Exception)
            {
                // MessageBox.Show(e.StackTrace + e.Message);
            }
        }

        public void lttr(String title)
        {
            var result = MessageBox.Show(String.Format(T("Due to an exception the \"{0}\" page has not been reviewed. Dou you want to open it in a web browser?"), title), T("Error"), //Ошибка при патрулировании
                                            MessageBoxButtons.YesNo,
                                            MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Process.Start(domain + aw.mysite.indexPath + "index.php?title=" + HttpUtility.UrlEncode(title.Replace(' ', '_')) + "&redirect=no");
            }
        }

        public void UpdateForm()
        {
            return;
            int w = main_panel.Width - 6;
            if (revbox.Visible) { w -= 6; }
            if (listbox.Visible) { w -= 6; }

            int w_1 = w / 4;
            int w_2 = w / 12 * 7;
            int w_3 = w / 6;

            if (revbox.Visible && listbox.Visible)
            {
                revbox.Left = 3;
                revbox.Width = w_1;
                diff_browser.Left = 3 + w_1 + 6;
                diff_browser.Width = w_2;
                listbox.Left = 3 + w_1 + 6 + w_2 + 6;
                listbox.Width = w_3;
                control_box.Left = listbox.Left;
                return;
            }

            if (revbox.Visible)
            {
                revbox.Left = 3;
                revbox.Width = w_1;
                diff_browser.Left = 3 + w_1 + 6;
                diff_browser.Width = w_2 + w_3;
                control_box.Left = 3 + w_1 + 6 + w_2 + 6;
                return;
            }

            if (listbox.Visible)
            {
                diff_browser.Left = 3;
                diff_browser.Width = w_2 + w_3;
                listbox.Left = 3 + w_2 + w_3 + 6;
                listbox.Width = w_3;
                control_box.Left = listbox.Left;
                return;
            }

            control_box.Left = 3 + w_1 + w_2 + 6;
            diff_browser.Left = 3;
            diff_browser.Width = w;
        }

        public void SwitchNewMode(bool v)
        {
            new_mode = v;
            if (new_mode == true)
            {
                timer2.Enabled = true;
                control_box.Visible = true;
                pause.Enabled = true;
            }
            else
            {
                timer2.Enabled = false;
                control_box.Visible = false;
            }
        }

        public void LoadProfMode()
        {
            if (current_revision == null) { return; }
        }

        public void UpdateCurrentDiff()
        {
            if (aw != null && aw.dc != null && current_revision != null) { aw.dc.AddRev_first(current_revision, true, true); }
        }

        /* Простой (0) / Профессиональный (1) режим */

        public void SwitchView(int prof_mode, bool check = false)
        {
            if (check && prof_mode == 0 && pro_mode.Checked == false) { return; }
            if (check && prof_mode == 1 && pro_mode.Checked == true) { return; }

            if (prof_mode == 0)
            {

                if (revs_tool_panel.Visible == true)
                {
                    revs_tool_panel.Visible = false;
                    main_panel.Top -= revs_tool_panel.Height;
                    panel1.Top -= revs_tool_panel.Height;
                }
                revs_visible = false;
                revbox.Visible = false;
                revbox.Enabled = false;
                splitContainer1.Panel1Collapsed = true;

                rw_waiting_list = null;
                waiting_diff = "";

                rc.ClearList();
                rc.Stop();

                if (check && pro_mode.Checked == true)
                {
                    // aw.dc.Stop();
                    // aw.dc.ClearList();
                    // aw.dc.AddRev_first(curr,true,true);
                    diff_browser.DocumentText = diff_wb;
                }
                pro_mode.Checked = false;
                pro_mode.Text = T("Enable pro mode");
            }
            else
            {
                pro_mode.Checked = true;
                pro_mode.Text = T("Disable pro mode");
                if (revs_tool_panel.Visible == false)
                {
                    revs_tool_panel.Visible = true;
                    main_panel.Top += revs_tool_panel.Height;
                    panel1.Top += revs_tool_panel.Height;
                }
                splitContainer1.Panel1Collapsed = false;
                revs_visible = true;
                revbox.Visible = true;
                revbox.Enabled = true;
                LoadProfMode();

                curr2a = null; curr2ap = null;
                curr2b = null; curr2bp = null;

                rw_waiting_list = null;
                waiting_diff = "";
                prof_group = false;
                p_gmc.Checked = prof_group;

                if (Pro_SetFirstVars()) DoProfNext();
                else DoProfShowSource();
                ProUpdateDiff();
            }
        }

        public bool Pro_SetFirstVars()
        {
            TRevisionSet tmp = current_revision.revisions;
            bool res = false;
            if (current_revision.page.is_flagged)
            {
                // Статья была когда-то отпатрулирована
                // Поэтому даём дифф с первого (всегд отпатрулированного) tl-элементы до последнего элемента первой непатрулированной цепочки правок
                curr2a = tmp[0].children[0];
                curr2b = curr2a;
                res = true;
            }
            else
            {
                // Статья не была отпатрулирована.
                // Поэтому показываем текст ревизии конца первой цепочки. Цепочку открываем.
                //curr2a = tmp[0].last;
                //curr2b = null;
                //tmp[0].hidden = false;
                // curr2a = tmp[0].children[0];

                // tmp[0].hidden = false;
                curr2a = tmp[tmp.root.count - 1].children[0];
                curr2b = curr2a;
            }

            rl.UpdateSelVars(curr2a, curr2b);
            ProUpdateDiff();
            return res;
        }



        public void SwitchRevbox(bool s)
        {
            revs_visible = s;
            splitContainer1.Panel1Collapsed = !s;
            revbox.Visible = s;
        }

        public void SwitchListbox(bool s)
        {
            list_visible = s;
            splitContainer2.Panel2Collapsed = !s;
            listbox.Visible = s;
        }


        public void TranslateControls(params Component[] controls)
        {
            foreach (Component ctl in controls)
            {
                // if (ctl is UserControl)
                //{


                // if (ctl is ToolStripButton) ((ToolStripButton)ctl).Text = T(((ToolStripButton)ctl).Text);
                if (ctl is ToolStripItem) ((ToolStripItem)ctl).Text = T(((ToolStripItem)ctl).Text);
                else if (ctl is ToolStripMenuItem) ((ToolStripMenuItem)ctl).Text = T(((ToolStripMenuItem)ctl).Text);
                //}
            }
        }

        public void UpdateLanguage()
        {
            // Main Menu
            TranslateControls(настройкиToolStripMenuItem, горячиеКлавишиToolStripMenuItem, проверитьОбновленияToolStripMenuItem, оПрограммеToolStripMenuItem);

            // Toolbar
            TranslateControls(toolStripLabel1, toolStripLabel2, stream_config_button);
            TranslateControls(cancel_button, d_diff, d_view, restart_stream_button, edit_page_button);
            TranslateControls(button_show_stable_version_in_browser,
                button_show_in_browser, diff_button, button_show_in_browser,
                show_history_in_browser, edit_inside_wikipedia);

            review_button.Text = T("Back");
            next_article_button.Text = T("Skip") + " (" + AllHotKeys.list["next"].name + ")";
            review_button.Text = T("Review") + " (" + AllHotKeys.list["review"].name + ")";
            reject_button.Text = T("Revert changes") + " (" + AllHotKeys.list["revert"].name + ")";
            show_user_info_button.Text = T("Show information about the editors") + " (" + AllHotKeys.list["authors"].name + ")";

            // (pro)
            TranslateControls(p_back, p_next, p_gmc);
            p_review.Text = T("Review the selected changes");

            // Rev context
            TranslateControls(sourceToolStripMenuItem, ViewToolStripMenuItem, backToolStripMenuItem, restore_old_v_and_review);
        }


        // Шаблоны для событий

        public void DoReviewArticle(bool s)
        {
            RevAssocCache.Add(current_revision.curr_id, current_revision.title);
            if (s)
            {
                if (current_revision == null) { return; }
                aw.Review(current_revision.curr_id);
                NextArticle();
            }
            else
            {
                if (current_revision == null) { return; }
                aw.Unreview(current_revision.curr_id);
            }
        }

        public void DoNextArticle()
        {
            NextArticle();
        }

        public void DoPrevArticle()
        {
            PrevArticle();
        }

        public void DoStreamConf()
        {
            StreamConf sc = new StreamConf();
            sc.main_form = this;
            sc.ShowDialog();
        }

        public void DoCancelInArticle()
        {
            TextInputForm cc = new TextInputForm("Undo the edit".T(), String.Format("Restoring revision {0}:".T(), current_revision.curr_id));
            cc.ShowDialog(this);
            if (cc.successfully)
            {
                aw.Cancel(current_revision.curr_id, current_revision.stable_id, cc.Text);
                NextArticle();
            }
        }

        public void DoListTick()
        {
            if (!list_visible)
            {
                return;
            }
            wl.UpdateItems(aw);
        }

        public void DoRevTick()
        {
            if (!revs_visible)
            {
                return;
            }
            rl.UpdateItems(current_revision);
        }

        public void DoNewModeTick()
        {
            aw.Update();
        }

        public void DoNewStart()
        {
            timer2.Enabled = true;
            pause.Enabled = true;
        }

        public void DoNewPause()
        {
            timer2.Enabled = false;
            pause.Enabled = false;
        }

        public void DoListBoxIndexChanged()
        {
            Element tmp = wl.GetSelected(aw);
            if (tmp == null)
            {
                return;
            }
            if (tmp.rand == current_revision.rand) { return; }
            aw.ScrollTo(tmp);
            NextArticle();
        }

        public void DoRevertArticle()
        {
            RevAssocCache.Add(current_revision.curr_id, current_revision.title);
            aw.Revert(current_revision.curr_id, current_revision.stable_id);
            NextArticle();
        }

        public void DoViewClick()
        {
            if (d_view.Checked)
            {
                d_view.Checked = false;
                d_diff.Checked = true;
                aw.dc.SwitchToDiffs();
                UpdateCurrentDiff();
            }
            else
            {
                d_view.Checked = true;
                d_diff.Checked = false;
                aw.dc.SwitchToView();
                UpdateCurrentDiff();
            }
        }

        public void DoDiffClick()
        {
            if (d_diff.Checked)
            {
                d_view.Checked = true;
                d_diff.Checked = false;
                aw.dc.SwitchToView();
                aw.dc.AddRev_first(current_revision, true, true);
            }
            else
            {
                d_view.Checked = false;
                d_diff.Checked = true;
                aw.dc.SwitchToDiffs();
                aw.dc.AddRev_first(current_revision, true, true);
            }
        }

        public void DoEditArticle()
        {
            RevAssocCache.Add(current_revision.curr_id, current_revision.title);
            Editor ed = new Editor(aw.PageCreate(current_revision.title), this);
            ed.curr = current_revision;
            ed.ShowDialog();
            if (ed.reviewed)
            {
                NextArticle();
            }
        }

        public void DoOpenDiff()
        {
            if (current_revision == null) { return; }
            if (current_revision.stable_id == -1)
            {
                DoOpenPage();
                return;
            }
            Process.Start(domain + aw.mysite.indexPath + "index.php?action=historysubmit&diff=" + current_revision.curr_id + "&oldid=" + current_revision.stable_id);
        }

        public void DoOpenPage()
        {
            if (current_revision == null) { return; }
            Process.Start(domain + aw.mysite.indexPath + "index.php?title=" + HttpUtility.UrlEncode(current_revision.title.Replace(' ', '_')) + "&redirect=no");
        }

        public void DoOpenHistory()
        {
            if (current_revision == null) { return; }
            Process.Start(domain + aw.mysite.indexPath + "index.php?title=" + HttpUtility.UrlEncode(current_revision.title.Replace(' ', '_')) + "&action=history");
        }

        public void DoOpenToEdit()
        {
            if (current_revision == null) { return; }
            Process.Start(domain + aw.mysite.indexPath + "index.php?title=" + HttpUtility.UrlEncode(current_revision.title.Replace(" ", "_")) + "&action=edit");
        }

        public void DoOpenStable()
        {
            if (current_revision == null) { return; }
            Process.Start(domain + aw.mysite.indexPath + "index.php?oldid=" + current_revision.stable_id);
        }

        public void DoOpenSettings()
        {
            SettingsForm sf = new SettingsForm();
            sf.main_form = this;
            sf.ShowDialog();
        }

        public void DoCheckUpdates()
        {
            Version.CheckUpdates(0);
        }

        public void DoOpenAboutForm()
        {
            AboutForm af = new AboutForm();
            af.ShowDialog();
        }

        public void DoProfCheckReviewButton()
        {
            bool reviewed = false;
            if (curr2a == null || curr2b == null) { p_review.Enabled = false; return; }
            if (curr2a.rand == curr2b.rand)
            {
                reviewed = curr2a.revision.flagged;
            }
            else
            {
                tl max = null;
                tl min = null;
                if (curr2a.revision.revid > curr2b.revision.revid) { max = curr2a; min = curr2b; } else { max = curr2b; min = curr2a; }
                long revstart = min.revision.revid + 1, revfin = max.revision.revid;
                WikiRevision tmp = null;
                for (int a = 0; a < current_revision.page.list.Count; a++)
                {
                    tmp = current_revision.page.list[a];
                    if (tmp.revid >= revstart && tmp.revid <= revfin)
                    {
                        if (!tmp.flagged) { reviewed = false; break; }
                        else { reviewed = true; }
                    }
                }
            }
            p_review.Enabled = true;
            p_review.Checked = reviewed;
            p_review.Text = reviewed ? T("Unreview the selected changes") : T("Review the selected changes");
        }

        public void DoProfNext(bool noerror = false)
        {
            if (curr2a == null && curr2b == null)
            {
                Pro_SetFirstVars(); return;
            }

            tl max = null;
            tl min = null;
            if (curr2a == null) { max = curr2b; }
            else if (curr2b == null) { max = curr2a; }
            else if (curr2a.revision.revid > curr2b.revision.revid) { max = curr2a; min = curr2b; } else { max = curr2b; min = curr2a; }

            if (max.next == null && max.parent.next == null) { if (!noerror) MessageBox.Show(T("You can't step forward!")); }
            else
            {
                curr2a = max;
                if (max.next == null) { max = max.parent; }
                curr2b = max.next;
                if (!prof_group)
                {
                    if (curr2b.children.Count == 1) curr2b = curr2b.children[0];
                }
                else
                {
                    if (curr2b.children.Count >= 1)
                    {
                        curr2b = curr2b.children[0];//[curr2b.count-1];
                    }
                }
                p_next.Enabled = (max.next != null || max.parent.next != null);
                p_back.Enabled = (max.prev != null || max.parent.prev != null);
            }
            rl.UpdateSelVars(curr2a, curr2b);
            rl.ScrollTo(curr2b);
            DoProfCheckReviewButton();
        }

        public void DoProfPrev()
        {
            if (curr2a == null && curr2b == null)
            {
                Pro_SetFirstVars(); return;
            }

            tl max = null;
            tl min = null;
            if (curr2a == null) { max = curr2b; }
            else if (curr2b == null) { max = curr2a; }
            else if (curr2a.revision.revid < curr2b.revision.revid) { max = curr2a; min = curr2b; } else { max = curr2b; min = curr2a; }

            if (max.prev == null && max.parent.prev == null) { MessageBox.Show(T("You can't step back!")); }
            else
            {
                curr2b = max;
                if (max.prev == null) { max = max.parent; }
                curr2a = max.prev;
                if (prof_group)
                {
                    if (curr2a.children.Count >= 1)
                    {
                        curr2a = curr2a.children[curr2a.count - 1];
                    }
                }
                else
                {
                    if (curr2a.children.Count == 1)
                    {
                        curr2a = curr2a.children[0];
                    }
                }
                p_next.Enabled = (max.next != null || max.parent.next != null);
                p_back.Enabled = (max.prev != null || max.parent.prev != null);
            }
            rl.UpdateSelVars(curr2a, curr2b);
            rl.ScrollTo(curr2b);
            DoProfCheckReviewButton();
        }

        public void _SetHidden(tl root, bool h)
        {
            tl tmp;
            for (int a = 0; a < root.count; a++)
            {
                tmp = root.children[a];
                if (tmp.type == 0)
                {
                    if (tmp.count > 1)
                    {
                        _SetHidden(tmp, h);
                        tmp.hidden = !h;
                        if (curr2a != null)
                        {
                            if (curr2a.parent.rand == tmp.rand) { tmp.hidden = false; }
                        }
                        if (curr2b != null)
                        {
                            if (curr2b.parent.rand == tmp.rand) { tmp.hidden = false; }
                        }
                    }
                }
            }
        }

        public void DoProfGroupModeChange(bool nmode)
        {
            prof_group = nmode;
            _SetHidden(current_revision.revisions.root, nmode);

        }

        // 0 - diff
        // 1 - source
        // 2 - rendered
        public void ProUpdateDiff(int type = 0)
        {
            waiting_diff = "";
            if (curr2a == null && curr2b == null)
            {

            }
            else if (curr2a != null && curr2b != null)
            {
                if (curr2a.rand == curr2b.rand) //selected
                {
                    if (type == 0) { diff_browser.DocumentText = i18n.GetText("pm_1"); }
                    else if (type == 1)
                    {
                        curr2wa = curr2a; curr2wb = curr2b; waiting_diff = "rev" + curr2wa.revision.rand + curr2wb.revision.rand;
                        aw.rrc.AddRev_last(curr2wa, curr2wb, 1, current_revision);
                    }
                    else if (type == 2)
                    {
                        curr2wa = curr2a; curr2wb = curr2b; waiting_diff = "rev" + curr2wa.revision.rand + curr2wb.revision.rand;
                        aw.rrc.AddRev_last(curr2wa, curr2wb, 2, current_revision);
                    }

                }
                else // diff
                {
                    // waiting_diff = "";
                    //  tl min = (curr2a.revision.revid > curr2b.revision.revid ? curr2b : curr2a);
                    // if (curr2a.count > 1)
                    // {
                    //     curr2wa = curr2a.children[0];
                    // }
                    // else
                    // {
                    curr2wa = curr2a;
                    // }
                    curr2wb = curr2b;
                    waiting_diff = "rev" + curr2wa.revision.rand + curr2wb.revision.rand;
                    aw.rrc.AddRev_last(curr2wa, curr2wb, 0, current_revision);
                }
                //rl.ScrollTo(curr2b);
            }
        }

        public void DoProfCMBack()
        {
            curr2a = curr2ap;
            curr2b = curr2bp;
            rl.UpdateSelVars(curr2a, curr2b);
            ProUpdateDiff();
        }

        public void DoProfShowSource()
        {
            ProUpdateDiff(1);
        }

        public void DoProfShowView()
        {
            ProUpdateDiff(2);
        }

        public void DoProfRollBack()
        {
            if (curr2a == null && curr2b == null)
            {

            }
            else if (curr2a != null && curr2b != null)
            {
                aw.Revert(current_revision.curr_id, curr2b.revision.revid);
                NextArticle();
                //   MessageBox.Show(curr2b.revision.revid.ToString());
            }
        }

        public void DoProfReviewSelected(bool gr)
        {
            // MessageBox.Show(gr ? "PAT" : "NPAT");

            if (curr2a == null || curr2b == null) { return; }

            tl max = null;
            tl min = null;
            if (curr2a.revision.revid > curr2b.revision.revid) { max = curr2a; min = curr2b; } else { max = curr2b; min = curr2a; }
            //if (min.count > 1) { min = min.children[0]; }
            long revstart = min.revision.revid + 1, revfin = max.revision.revid;
            WikiRevision tmp = null; WikiRevision tmps = null;
            List<WikiRevision> rwlist = new List<WikiRevision>();
            for (int a = 0; a < current_revision.page.list.Count; a++)
            {
                tmp = current_revision.page.list[a];
                if (tmp.revid >= revstart && tmp.revid <= revfin)
                {
                    if (gr == false && tmp.flagged != gr) { rwlist.Add(tmp); }
                    else { tmps = tmp; }
                }
            }
            if (gr && tmps != null) { rwlist.Add(tmps); }
            if (rwlist.Count == 0) { return; }

            List<long> revid = new List<long>();
            for (int a = 0; a < rwlist.Count; a++) { revid.Add(rwlist[a].revid); }

            rw_waiting_list = rwlist;

            if (gr) { aw.ReviewL(revid); }
            else { aw.UnreviewL(revid); }
        }

        public void DoUpdateTraffic()
        {
            if (aw == null || aw.mysite == null) { return; }
            String text = Tools.BytesCountToString(aw.mysite.out_traffic) + " / " + Tools.BytesCountToString(aw.mysite.in_traffic);
            traffic_label.Text = text;
        }

        // События

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            ClosingForm();
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            DoStreamConf();
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            if (startc_showed) { return; }
            startc_showed = true;
            SwitchView(0);
            DeactivateAll();
            DoStreamConf();
        }

        private void review_button_Click(object sender, EventArgs e)
        {
            SwitchView(0, true);
            DoReviewArticle(!review_button.Checked);

        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            SwitchView(0, true);
            NextArticle();
        }

        private void toolStripButton11_Click(object sender, EventArgs e)
        {
            SwitchView(0, true);
            PrevArticle();
        }

        private void cancel_button_Click(object sender, EventArgs e)
        {
            SwitchView(0, true);
            DoCancelInArticle();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (listbox.Visible) { control_box.Left = this.Width - listbox.Width; }
            DoListTick();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            DoNewModeTick();
        }

        private void start_Click(object sender, EventArgs e)
        {
            DoNewStart();
        }

        private void pause_Click(object sender, EventArgs e)
        {
            DoNewPause();
        }

        private void reload_Click(object sender, EventArgs e)
        {
            DoNewModeTick();
        }

        private void listbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            DoListBoxIndexChanged();
        }

        private void reject_button_Click(object sender, EventArgs e)
        {
            SwitchView(0, true);
            DoRevertArticle();
        }

        private void d_diff_Click(object sender, EventArgs e)
        {
            SwitchView(0, true);
            DoDiffClick();
        }

        private void d_view_Click(object sender, EventArgs e)
        {
            SwitchView(0, true);
            DoViewClick();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            DoEditArticle();
        }

        private void diff_button_Click(object sender, EventArgs e)
        {
            DoOpenDiff();
        }

        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            DoOpenPage();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            DoOpenStable();
        }

        private void toolStripButton10_Click(object sender, EventArgs e)
        {
            DoOpenHistory();
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            DoOpenToEdit();
        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            Stop_Work();
            Start_Work();
        }

        private void SettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DoOpenSettings();
        }

        private void CheckUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DoCheckUpdates();
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DoOpenAboutForm();
        }

        private void cb_l_CheckedChanged(object sender, EventArgs e)
        {
            listbox.Visible = !cb_l.Checked;
            SwitchListbox(!cb_l.Checked);
        }

        private void KeyPressHandler(object sender, KeyPressEventArgs e)
        {
            if (!running) { return; }
            if (current_revision == null) { return; }

            if (e.KeyChar == 'j' || e.KeyChar == 'о')
            {
                DoReviewArticle(true);
            }
            if (e.KeyChar == 'k' || e.KeyChar == 'л')
            {
                DoNextArticle();
            }
            if (e.KeyChar == 'l' || e.KeyChar == 'д')
            {
                DoRevertArticle();
            }
            if (e.KeyChar == ' ')
            {
                diff_browser.Document.Body.ScrollTop += (int)(diff_browser.Height * 0.9);
            }
        }

        private void webBrowser1_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {

            if (e.Url.ToString().Equals("about:blank")) { return; }
            if (!e.Url.ToString().Contains("about:process")) { e.Cancel = true; return; }
            String tmp = e.Url.ToString().Replace("about:process_id_", "");
            e.Cancel = true;
            WBcallback.ProcessClick(diff_browser, aw, tmp);
        }

        private void pro_mode_Click(object sender, EventArgs e)
        {
            SwitchView(pro_mode.Checked ? 0 : 1, true);
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            DoRevTick();
        }



        /*
         * Обновление выделенных правок (проф. режим)
         * */

        public void Rev_SelUpdate(object o, RevListUpdateArgs e)
        {
            if (e.action == 0) // update
            {
                curr2a = e.a;
                curr2b = e.b;
                ProUpdateDiff();
                DoProfCheckReviewButton();
            }
            else if (e.action == 1) // update diff
            {
                curr2ap = curr2a;
                curr2bp = curr2b;
                curr2a = e.a;
                curr2b = e.b;
                rev_context.Show(System.Windows.Forms.Cursor.Position);
                DoProfCheckReviewButton();
                wb_clear = true;
            }
            else if (e.action == 2) // dont update diff
            {
                curr2a = e.a;
                curr2b = e.b;
            }
        }

        private void p_next_Click(object sender, EventArgs e)
        {
            DoProfNext();
            ProUpdateDiff();
        }

        private void p_back_Click(object sender, EventArgs e)
        {
            DoProfPrev();
            ProUpdateDiff();
        }

        private void p_gmc_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(p_gmc.Checked ? "Y" : "N");
            //p_gmc.Checked = !p_gmc.Checked;
            DoProfGroupModeChange(p_gmc.Checked);
            //p_gmc.Checked = !p_gmc.Checked;
        }


        private void p_review_Click(object sender, EventArgs e)
        {
            DoProfReviewSelected(!p_review.Checked);
            DoProfNext(true);
        }

        private void backToolStripMenuItem_Click(object sender, EventArgs e)
        {
            wb_clear = false; DoProfCMBack();
        }

        private void sourceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            wb_clear = false; DoProfShowSource();
        }

        private void ViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            wb_clear = false; DoProfShowView();
        }

        private void rev_context_Closed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            if (wb_clear) { ProUpdateDiff(); }
            wb_clear = false;
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (Options.GetOptionInt("scroll_diff") == 0)
                return;

            HtmlDocument doc = diff_browser.Document;
            HtmlElement elem = doc.GetElementById("authors");
            if (elem != null && elem.InnerHtml != null && elem.InnerHtml != "") return;
            elem = doc.GetElementById("first_difference");
            if (elem != null)
            {
                elem.ScrollIntoView(true);
                int w = diff_browser.Document.Body.ScrollTop;
                if (w < 35) { diff_browser.Document.Body.ScrollTop = 0; }
                else { diff_browser.Document.Body.ScrollTop = w - 35; }
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(T("Do you really want to restore that revision as current?"), "Warning!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) ==
                DialogResult.Yes)
            {
                DoProfRollBack();
            }
            wb_clear = false;
        }

        private void горячиеКлавишиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new HotKeys().ShowDialog();
        }

        private void show_user_info_button_Click(object sender, EventArgs e)
        {
            if (!show_user_info_button.Checked)
            {
                ShowUserInfoBox();
                show_user_info_button.Checked = true;
            }
            else
            {
                HideUserInfoBox();
                show_user_info_button.Checked = false;
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            //    this.Text = e.KeyCode == Keys.Q ? "pT" : "pF";
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            //  this.Text = e.KeyCode == Keys.Q ? "uT" : "uF";
        }

        private void toolStripButton1_Click_1(object sender, EventArgs e)
        {
            //  HtmlElement el = webBrowser1.Document.GetElementById("TMP");
            //  if( el!=null)
            //  el.InnerHtml = @"<textarea style=""position:absolute;top:0px;bottom:0px;left:0px;right:0px;width:100%;height:100%;"">eeeeAA</textarea>"; //aw.GetDiffHtml();

        }

        private void existence_button_Click(object sender, EventArgs e)
        {
            tmp_tif = new TextInputForm("Check the existence of a file/page", "");
            tmp_tif.ProcessTheInput += ProcessExistence;
            tmp_tif.Show(this);
        }

        public void ProcessExistence(object o, string text)
        {
            text = text.Trim();
            WBcallback.ProcessLinkThreaded(aw, text);
        }

        private void toolStripButton1_Click_2(object sender, EventArgs e)
        {

        }

    }
}
