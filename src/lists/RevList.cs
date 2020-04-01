using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Drawing.Design;
using System.Drawing.Text;
using System.Threading;
using System.Timers;


using System.Collections;

namespace sweepnet
{
    public class RevListUpdateArgs : EventArgs
    {
        public readonly tl a,b;
        public readonly int action;// 0 - update, 1 - show context

        public RevListUpdateArgs(tl _a, tl _b, int act=0)
        {
            a = _a;
            b = _b;
            action = act;
        }
    }

    public delegate void RevListUpdateHandler(object o, RevListUpdateArgs e);


    public class MyRevBox : ListBox
    {
        Bitmap rw, rb, nw;
        public Hashtable ht = new Hashtable();
        public TRevisionSet revs;
        public List<RevListElement> items2_list = new List<RevListElement>();

        public MyRevBox()
        {
            this.DrawMode = DrawMode.OwnerDrawVariable; // We're using custom drawing.
            this.ItemHeight = 16; // Set the item height to 40.

            this.rw = Properties.Resources.list_rw;
            this.rb = Properties.Resources.list_rb;
            this.nw = Properties.Resources.list_new;
            /*
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            */
            this.SetStyle(
        ControlStyles.OptimizedDoubleBuffer |
        ControlStyles.ResizeRedraw |
        ControlStyles.AllPaintingInWmPaint |
        ControlStyles.UserPaint,
        true);
            this.DrawMode = DrawMode.OwnerDrawFixed;

            //Enable the OnNotifyMessage event so we get a chance to filter out 
            // Windows messages before they get to the form's WndProc
            this.SetStyle(ControlStyles.EnableNotifyMessage, true);
        }

        protected override void OnNotifyMessage(Message m)
        {
            //Filter out the WM_ERASEBKGND message
            if (m.Msg != 0x14)
            {
                base.OnNotifyMessage(m);
            }
        }

        public void AddAssigned(String key, RevListElement value)
        {
            ht[key] = value;
            this.Items.Add(key);
            this.items2_list.Add(value);
        }

        public void Clear()
        {
            this.Items.Clear();
            ht.Clear();
            items2_list.Clear();
        }

        public String GetItemTime(DateTime dt)
        {
            return dt.ToString("dd/MM/yyyy HH:mm");
        }

        public String GetRevFlags(WikiRevision rev)
        {
            return "["+(rev.diffsize>0?"+":"")+rev.diffsize+"]"+(rev.minor?" [m]":"");
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            // Make sure we're not trying to draw something that isn't there.
            if (e.Index >= this.Items.Count || e.Index <= -1)
                return;

            // Get the item object.
            object item = this.Items[e.Index];
            if (item == null)
                return;

            string key = item.ToString();
            RevListElement tm = (RevListElement)(ht[key]);
            if (tm != null)
            {
                // MessageBox.Show(tm.name);
            }
            else { return; }

            e.DrawBackground();
            String text = ""; SizeF stringSize;

            e.Graphics.FillRectangle(new SolidBrush(Color.White), e.Bounds); // Изначально всё белое
            int x = e.Bounds.X, y = e.Bounds.Y, h = e.Bounds.Height, w = e.Bounds.Width, bw = 11, bh = h * 7 / 10;//bh = 10, bw = 10;
            if (bh % 2 == 0) { bh--; }
            //bw = (bh - 3) - 2 + 3;
            bw = bh + 1;//(int)((double)(3 / 2 * ((double)bh - 3) + 3));
            // if (bw % 2 == 0) { bw++; }
            //MessageBox.Show(bh + " " + bw);
            x += 2; w -= 2;
            Pen blackpen = new Pen(Color.Black);



            if (tm.selected == false && tm.offset != 0)
            {
                x += 5 * (tm.offset == 1 ? -1 : 1);
                w -= 5;
            }

            if (tm.sub == true)
            {
                x += 7;
                w -= 7;
                e.Graphics.DrawLine(blackpen, new Point(x, y), new Point(x, y + h)); //h
                x += 1;
                w -= 1;
            }
            else
            {
                e.Graphics.DrawLine(blackpen, new Point(x, y), new Point(x, y + h));
                x += 2;
                w -= 2;
                //   e.Graphics.FillRectangle(new SolidBrush(Color.Gainsboro), e.Bounds);
                // e.Graphics.FillRectangle(new SolidBrush(Color.White), e.Bounds);
            }
            Rectangle r = new Rectangle();
            r.X = x; r.Y = y; r.Width = w; r.Height = e.Bounds.Height;
            switch (tm.color)
            {
                case 0: e.Graphics.FillRectangle(new SolidBrush(Color.White), e.Bounds); break;
                case 1:
                    if (tm.sub == false) { e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(255, 255, 230, 230)), r); }
                    else { e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(255, 255, 240, 240)), r); }
                    break; //red
                case 2:
                    if (tm.sub == false) { e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(0x7ffffbdb)), r); }
                    else { e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(255, 255, 254, 249)), r); }
                    break; //yellow
                case 3: e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(255, 221, 239, 255) /*Color.FromArgb(0x7ff0f8ff)*/), r); break; //blue
            }

            if (tm.selected)
            {
                r.Height--;
                e.Graphics.DrawRectangle(blackpen, r);
            }
            else
            {
                if (tm.offset == 1)
                {
                    text = "<<";
                    e.Graphics.DrawString(text, this.Font, new SolidBrush(Color.Black), new PointF(w - 10, e.Bounds.Y + (e.Bounds.Height - e.Graphics.MeasureString(text, this.Font).Height) / 2));
                }
                else if (tm.offset == 2)
                {
                    text = ">>";
                    e.Graphics.DrawString(text, this.Font, new SolidBrush(Color.Black), new PointF(w - 10, e.Bounds.Y + (e.Bounds.Height - e.Graphics.MeasureString(text, this.Font).Height) / 2));
                }
            }

            int startx = 0, starty = 0;


            DateTime revtime = DateTime.Parse(tm.main.revision.timestamp);
            int a = 0, b = 0;
            switch (tm.state)
            {
                case 0: //collapsed list



                    startx = x + 1;
                    starty = (h - bh) / 2 + y;//h / 2 - bh / 2 - 1;
                    e.Graphics.DrawLine(blackpen, new Point(startx, starty), new Point(startx + bw, starty));
                    e.Graphics.DrawLine(blackpen, new Point(startx, starty), new Point(startx, starty + bh));
                    e.Graphics.DrawLine(blackpen, new Point(startx + bw, starty), new Point(startx + bw, starty + bh));
                    e.Graphics.DrawLine(blackpen, new Point(startx, starty + bh), new Point(startx + bw, starty + bh));

                    a = (bh - 3) / 2 + 1;
                    e.Graphics.DrawLine(blackpen, new Point(startx, starty + a), new Point(startx + bw, starty + a));

                    b = (bw - 3) / 2 + 2;
                    e.Graphics.DrawLine(blackpen, new Point(startx + b, starty), new Point(startx + b, starty + bh));


                    x += bw + 2;

                    text = /*"[" + GetItemTime(revtime) + "] " +*/ tm.main.user + " (" + tm.main.count + ")";
                    stringSize = e.Graphics.MeasureString(text, this.Font);
                    e.Graphics.DrawString(text, this.Font, new SolidBrush(Color.Black), new PointF(x, e.Bounds.Y + (e.Bounds.Height - stringSize.Height) / 2));
                    break;
                case 1: //opened list

                    e.Graphics.DrawLine(blackpen, new Point(x - 1, y + h - 1), new Point(x + w, y + h - 1));

                    startx = x + 1;
                    starty = (h - bh) / 2 + y;//h / 2 - bh / 2 - 1;
                    e.Graphics.DrawLine(blackpen, new Point(startx, starty), new Point(startx + bw, starty));
                    e.Graphics.DrawLine(blackpen, new Point(startx, starty), new Point(startx, starty + bh));
                    e.Graphics.DrawLine(blackpen, new Point(startx + bw, starty), new Point(startx + bw, starty + bh));
                    e.Graphics.DrawLine(blackpen, new Point(startx, starty + bh), new Point(startx + bw, starty + bh));

                    a = (bh - 3) / 2 + 1;
                    e.Graphics.DrawLine(blackpen, new Point(startx, starty + a), new Point(startx + bw, starty + a));



                    x += bw + 2;



                    text = /*"[" + GetItemTime(revtime) + "] " + */tm.main.user + " (" + tm.main.count + ")";
                    stringSize = e.Graphics.MeasureString(text, this.Font);
                    e.Graphics.DrawString(text, this.Font, new SolidBrush(Color.Black), new PointF(x, e.Bounds.Y + (e.Bounds.Height - stringSize.Height) / 2));
                    break;
                case 2: //once element
                    text = /*"["+GetItemTime(revtime)+"] "+*/tm.main.user + " " + GetRevFlags(tm.main.revision);
                    if (tm.sub)
                    {
                        text = "[" + GetItemTime(revtime) + "] " + GetRevFlags(tm.main.revision);
                        if (tm.last) { e.Graphics.DrawLine(blackpen, new Point(x - 1, y + h - 1), new Point(x + w, y + h - 1)); }
                    }
                    stringSize = e.Graphics.MeasureString(text, this.Font);
                    e.Graphics.DrawString(text, this.Font, new SolidBrush(Color.Black), new PointF(x, e.Bounds.Y + (e.Bounds.Height - stringSize.Height) / 2));
                    break;
            }

            base.OnDrawItem(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Region iRegion = new Region(e.ClipRectangle);
            e.Graphics.FillRegion(new SolidBrush(this.BackColor), iRegion);
            if (this.Items.Count > 0)
            {
                for (int i = 0; i < this.Items.Count; ++i)
                {
                    System.Drawing.Rectangle irect = this.GetItemRectangle(i);
                    if (e.ClipRectangle.IntersectsWith(irect))
                    {
                        if ((this.SelectionMode == SelectionMode.One && this.SelectedIndex == i)
                        || (this.SelectionMode == SelectionMode.MultiSimple && this.SelectedIndices.Contains(i))
                        || (this.SelectionMode == SelectionMode.MultiExtended && this.SelectedIndices.Contains(i)))
                        {
                            OnDrawItem(new DrawItemEventArgs(e.Graphics, this.Font,
                                irect, i,
                                DrawItemState.Selected, this.ForeColor,
                                this.BackColor));
                        }
                        else
                        {
                            OnDrawItem(new DrawItemEventArgs(e.Graphics, this.Font,
                                irect, i,
                                DrawItemState.Default, this.ForeColor,
                                this.BackColor));
                        }
                        iRegion.Complement(irect);
                    }
                }
            }
            base.OnPaint(e);
        }

        /* Events */

        

    }

    public class RevListElement
    {
        public tl main;
        public int state = 0;//0-collapsed list, 1 - opened list, 2 - single element
        public int color = 0;//0-white, 1 - red, 2 - yellow, 3 - blue
        public bool selected = false;
        public int offset = 0; // 1 - left, 2 - right
        public String rand_s = "";
        public double rand = -1;
        public bool sub = false;
        public bool last = false;
    }

    public class RevList
    {
        public MyRevBox control;
        public String tmp_hash = "";
        public CoreOperations aw;
        public Element curr;

        public event RevListUpdateHandler UpdateEvent;

        public RevList(MyRevBox _w, CoreOperations _aw)
        {
            control = _w;
            aw = _aw;

            control.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mousedown);
            control.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mouseup);

            //control.MouseUp += new System.Windows.Forms.MouseEventHandler(this.do_OnMouseDown);
           // control.DoubleClick += new System.EventHandler(this.do_OnMouseDoubleClick);
        }

        private MouseEventArgs last_mva = null;
        private DateTime last_dt = DateTime.Now;

        private void mousedown(object sender, MouseEventArgs e)
        {
            last_mva = e;
            last_dt = DateTime.Now;
        }

        private void mouseup(object sender, MouseEventArgs e)
        {
            // error
            if (last_mva == null || last_dt == null) { return; }
            int z = last_mva.Y - e.Y;
            if (z < 0) { z *= -1; }
            if (z > 50) { return; }
            if (e.Button != last_mva.Button) { return; }
            // error
            
            int button = 0;
            if (e.Button == MouseButtons.Left) { button = 0; }
            if (e.Button == MouseButtons.Right) { button = 1; }

            int type = 0;
            z = e.X - last_mva.X;
            int ofs = 10;
            TimeSpan ts = DateTime.Now - last_dt;
            if (ts.TotalMilliseconds > 200) { ofs = 20; }
            if (z > ofs) { type = 3; }
            else if (z < -1*ofs) { type = 2; }
            else if (ts.TotalMilliseconds > 400) { type = 1; }


            int index = control.IndexFromPoint(last_mva.X, last_mva.Y); 
            if (index == ListBox.NoMatches)
            {
                index = -1;
            }
            
            MouseProcessor(index, button, type, e.X);
        }

        private tl sela=null, selb=null; // << , >>
        private tl psela=null, pselb=null; // previous

        public void UpdateSelVars(tl a, tl b)
        {
            psela = sela;
            pselb = selb;
            sela = a;
            selb = b;
        }

        // index: item id
        // button: 0 - left, 1 - rigth
        // type: 0 - normal, 1 - long, 2 - left dragging, 3 - right dragging
        private void MouseProcessor(int index, int button, int type, int x=0)
        {
            if (index == -1) { return; } 
           // MessageBox.Show("I: "+index+", B: " + button+ ",  T: " + type);
            
            tl item = GetByIndex(index);

            switch (type)
            {
                case 0:
                    if (button == 0) {
                        if (x < 25)
                        {
                            goto lng;
                        }
                        SelUpdate(0, item); 
                    }
                    else { SelUpdate(0, item, false); EventUpdate(1); } // context
                    break;

                case 1:
            lng:
                    if (button == 0)
                    {
                        item.hidden = !item.hidden;
                        if (sela != null && selb != null && sela.rand == selb.rand) { break; }
                        if (item.hidden == false)
                        {
                            if (sela != null && sela.rand == item.rand)
                            {
                                sela = item.children[item.children.Count-1]; EventUpdate(2);
                            }
                            else if (selb != null && selb.rand == item.rand)
                            {
                                selb = item.children[item.children.Count - 1]; EventUpdate(2);
                            }
                        }
                        else if(item.parent.rootflag)
                        {
                            double randa=-100, randb=-100;
                            if (sela != null)
                            {
                                randa=sela.rand;
                            }
                            if (selb != null)
                            {
                                randb = selb.rand;
                            }

                            int le=item.children.Count-1;
                            for (int a = 0; a <= le; a++)
                            {
                                if (item.children[a].rand == randa)
                                {
                                    if (a == le) { sela = item; EventUpdate(2); }
                                    else { item.hidden = !item.hidden; }
                                    break;
                                }
                                if (item.children[a].rand == randb)
                                {
                                    if (a == le) { selb = item; EventUpdate(2); }
                                    else { item.hidden = !item.hidden; }
                                    break;
                                }
                            }
                        }
                    }
                    break;

                case 2:
                    if (button == 0) { SelUpdate(2, item); }
                    break;
                    
                case 3:
                    if (button == 0) { SelUpdate(3, item); }
                    break;
            }

        }

        private System.Timers.Timer diff_timer = null;

        // type: ^
        private void SelUpdate(int type, tl item, bool timer=true)
        {
            if (type == 0)
            {

                sela = item;
                selb = item;
                if (item.type == 0) { item.hidden = true; }
                if (timer) { EventUpdate(); }

            }

            else if (type == 2) // left
            {
                bool changed = false;

                if (sela != null && selb != null && sela.rand == selb.rand && sela.rand == item.rand) { selb = null; changed = true; }
                else if (sela != null && selb != null && sela.rand == selb.rand) { sela = item; selb = null; changed = true; }
                else
                {
                    if (sela != null && sela.rand == item.rand) { return; }
                    if (selb != null && selb.rand == item.rand)
                    {
                        selb = null;
                        changed = true;
                    }

                    if (!changed)
                    {
                        sela = item; changed = true;
                        if (item.type == 0) { item.hidden = true; }
                    }
                }

                if (changed&&timer)
                {
                    _TimerStart();
                }
            }

            else if (type == 3) // right
            {
                bool changed = false;

                if (sela != null && selb != null && sela.rand == selb.rand && sela.rand == item.rand) { sela = null; changed = true; }
                else if (sela != null && selb != null && sela.rand == selb.rand) { selb = item; sela = null; changed = true; }
                else
                {
                    if (selb != null && selb.rand == item.rand) { return; }
                    if (sela != null && sela.rand == item.rand)
                    {
                        sela = null;
                        changed = true;
                    }

                    if (!changed)
                    {
                        selb = item; changed = true;
                        if (item.type == 0) { item.hidden = true; }
                    }
                }

                if (changed&&timer)
                {
                    _TimerStart();
                }
            }

            if (selb != null) { ScrollTo(selb); }
        }

        private void _TimerStart()
        {
            diff_timer = new System.Timers.Timer(Options.GetOptionInt("pd_wait"));
            diff_timer.Elapsed += new ElapsedEventHandler(_TimerEvent);
            diff_timer.Start();
        }

        private void _TimerStop()
        {
            diff_timer.Stop();
        }

        private void _TimerEvent(object sender, ElapsedEventArgs e)
        {
            diff_timer.Stop();
            if (sela == null && psela != null) { EventUpdate(); return; }
            if (selb == null && pselb != null) { EventUpdate(); return; }
            if (sela == null || selb == null) {  return; }
            if(psela!=null&&psela.rand==sela.rand){ // a didnt changed
                if(pselb!=null&&pselb.rand==selb.rand){ // b didnt changed
                    return;
                }
            }
            EventUpdate();
        }

        private void EventUpdate(int action=0)
        {
            psela = sela; pselb = selb;
            UpdateEvent(new object(), new RevListUpdateArgs(sela, selb, action));
        }


        public String GetHash(tl el)
        {
            String ret = "A";
            if (sela == null) { ret += "N"; } else { ret += sela.rand; }
            if (selb == null) { ret += "N"; } else { ret += selb.rand; }

            if (el.type == 1 && el.revision != null)
            {
                ret += el.revision.revid + "" + (el.revision.flagged ? "y" : "n");
            }
            else
            {
                foreach (tl _el in el.children)
                {
                    ret += (_el.hidden ? "y" : "n") + (_el.count) + (_el.diff ? "y" : "n") + (_el.diffto) + GetHash(_el);
                }
            }

            return Tools.GetMD5(curr.rand + (curr.page.is_flagged ? "y" : "n") + ret);
        }

        public void CreateItems(tl el, bool sub=false)
        {
            tl tmp;
            double selrand=-100,lrand=-100,rrand=-100;
            if (sela != null && selb != null && sela.rand == selb.rand) { selrand = sela.rand; }
            if (sela != null) { lrand = sela.rand; }
            if (selb != null) { rrand = selb.rand; }
            RevListElement last = null;
            for (int a = el.count - 1; a >= 0; a--)
            {
                RevListElement rve = new RevListElement(); last = rve;
                tmp = el.children[a];

                rve.rand = tmp.rand;
                rve.rand_s = rve.rand + "";
                rve.main = tmp;
                rve.sub = sub;

                if (!curr.page.is_flagged) { rve.color = 1; }
                else if (tmp.revision.flagged) { rve.color = 3; }
                else { rve.color = 2; }

                if (selrand == rve.rand) { rve.selected = true; }
                if (lrand == rve.rand) { rve.offset = 1; }
                if (rrand == rve.rand) { rve.offset = 2; }

                //if(tmp.revision.flagged

                if (tmp.type == 0 && tmp.count == 1) { CreateItems(tmp); continue; }
                else if (tmp.type == 0)
                {
                    rve.state = 0;
                    control.AddAssigned(rve.rand_s, rve);
                    if (!tmp.hidden) { rve.state = 1; CreateItems(tmp, true); }
                }
                else if (tmp.type == 1)
                {
                    rve.state = 2;
                    control.AddAssigned(rve.rand_s, rve);
                }
            }
            if (last != null) { last.last = true; }
        }

        public void UpdateItems(Element el, bool check_hash = true)
        {
            if (el.revisions == null) { return; }
            try
            {
                String hash = "";
                lock (el.revisions)
                {
                    curr = el;
                    TRevisionSet rs = el.revisions;
                    tl root = rs.root;
                    if (control.items2_list.Count > 0 && check_hash)
                    {
                        hash = GetHash(root);
                        if (tmp_hash.Equals(hash)) { return; }
                    }

                    int top_index = control.TopIndex;

                    String top_rand_s = "";
                    double top_rand = -1;
                    try
                    {
                        top_rand_s = control.Items[top_index].ToString();
                        foreach (RevListElement i in control.items2_list)
                        {
                            if (i.rand_s.Equals(top_rand_s)) { top_rand = i.rand; break; }
                        }
                    }
                    catch { }

                    int new_index = top_index;

                    control.BeginUpdate();
                    control.Clear();

                    CreateItems(root);

                    control.TopIndex = new_index;
                    control.EndUpdate();
                    control.PerformLayout();
                    tmp_hash = hash;
                }
            }
            catch (Exception) { return; }
        }

        public void MakeCenter(int pos)
        {
            int onpage = control.Height / control.ItemHeight;
            if (pos < 0.5 * onpage) { control.TopIndex = 0; return; }
            int total = control.items2_list.Count * control.ItemHeight;
            if (pos > (total - onpage)) { control.TopIndex = total - onpage; return; }
            control.TopIndex = pos - onpage / 2;
        }

        public void ScrollTo(tl aa)
        {
              try
              {
                  foreach (RevListElement e in control.items2_list)
                  {
                      //e.selected = false;
                      if (e.main.rand==aa.rand)
                      {
                          int pos = -1;
                          for (int a = 0; a < control.Items.Count; a++)
                          {
                              if (control.Items[a].Equals(e.rand + "") && pos == -1)
                              {
                                  pos = a; //break;
                              }
                          }
                          if (pos == -1) { return; }
                          //e.selected = true;
                          control.SetSelected(pos, true);
                          MakeCenter(pos);
                          //e.selected = true;
                          control.Update();
                          break;
                      }
                  }
              }
              catch (Exception e)
              {
                  MessageBox.Show("Исключение в движке списка2: " + e.Message);
              }
        }

        public void Select(Element el)
        {
            /*   foreach (TMyListBoxItem e in control.items2_list)
               {
                   e.selected = false;
                   if (e.rand == el.rand)
                   {
                       e.selected = true;
                   }
               }
               control.Update();*/
        }

        public void Clear()
        {
            control.Clear();
        }

        public tl GetByIndex(int index)
        {
            tl tmp = null;
            if (index == -1) { return tmp; }
            RevListElement t = (RevListElement)(control.ht[control.Items[index]]);
            if (t != null)
            {
                tmp = t.main;
            }
            return tmp;
        }

        public Element GetSelected(CoreOperations aw)
        {
            Element tmp = null;
            int s = control.SelectedIndex;
            if (s == -1) { return tmp; }
            TMyListBoxItem t = (TMyListBoxItem)(control.ht[control.Items[s]]);
            double rand = t.rand;
            foreach (Element el in aw.list.list)
            {
                if (el.rand == rand) { tmp = el; break; }
            }
            foreach (Element el in aw.skipped)
            {
                if (el.rand == rand) { tmp = el; break; }
            }

            return tmp;
        }


    }
}

