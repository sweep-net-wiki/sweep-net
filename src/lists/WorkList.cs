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


using System.Collections;

namespace sweepnet
{
        public class MyListBox : ListBox
        {
            Bitmap rw, rb, nw;
            public Hashtable ht = new Hashtable();
            public List<TMyListBoxItem> items2_list = new List<TMyListBoxItem>();

            public MyListBox()
            {
                this.DrawMode = DrawMode.OwnerDrawVariable; // We're using custom drawing.
                this.ItemHeight = 18; // Set the item height to 40.

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

            public void AddAssigned(String key, TMyListBoxItem value)
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
                TMyListBoxItem tm = (TMyListBoxItem)(ht[key]);
                if (tm != null)
                {
                    // MessageBox.Show(tm.name);
                }
                else { return; }

                e.DrawBackground();

                

                switch (tm.state2)
                {
                    case 0:
                        e.Graphics.FillRectangle(new SolidBrush(Color.Gainsboro), e.Bounds);
                        break;
                    case 1:
                        e.Graphics.FillRectangle(new SolidBrush(Color.White), e.Bounds);
                        if (!tm.was_ever_reviewed) { e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(255,240,240)), e.Bounds); }
                        break;
                }

                // Draw the background color depending on 
                // if the item is selected or not.
                //if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                if(tm.selected)
                {
                    // The item is selected.
                    // We want a blue background color.
                    Rectangle tmp = new Rectangle();
            
                    tmp.X = e.Bounds.X;
                    tmp.Y = e.Bounds.Y;
                    tmp.Width = e.Bounds.Width;
                    tmp.Height = e.Bounds.Height;
                    tmp.Height--;
                    e.Graphics.DrawRectangle(new Pen(Color.Blue), tmp);
                }
  
                // Draw the item.
                
                string text = tm.name;

                SizeF stringSize = e.Graphics.MeasureString(text, this.Font);
                e.Graphics.DrawString(text, this.Font, new SolidBrush(Color.Black),
                    new PointF(18, e.Bounds.Y + (e.Bounds.Height - stringSize.Height) / 2));

                switch(tm.state1){
                    case 0:
                        e.Graphics.DrawImage(this.rw, new Point(1, e.Bounds.Y + 1));
                        break;
                    case 1:
                        e.Graphics.DrawImage(this.rb, new Point(1, e.Bounds.Y + 1));
                        break;
                    case 2:
                        e.Graphics.DrawImage(this.nw, new Point(1, e.Bounds.Y + 1));
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
            
           // 
    }

        public class TMyListBoxItem
        {
            public String name;
            public int state1;//0-reviewed,1-rejected,2-new,3-nothing
            public int state2;//0-new,1-loaded
            public bool was_ever_reviewed = true;

            public int pos = -1;
            public double rand = -1;
            public String rand_s = "";

            public bool selected = false;
        }

    public class WorkList
    {
        public MyListBox control;
        public String tmp_hash = "";

        public WorkList(MyListBox _w)
        {
            control = _w;
        }

        public string GetHash(CoreOperations aw)
        {
            String tt="";
            foreach (Element e in aw.list.list)
            {
                //if (e.skip_me) { continue; }
                tt += (1 + e.curr_id + e.rand + e.stable_id + e.size) + (e.loaded ? "y" : "n") + (e.showed ? "y" : "n") + (e.rejected ? "y" : "n") + (e.reviewed ? "y" : "n");
            }
            tt = Tools.GetMD5(tt) + "|";
            foreach (Element e in aw.skipped)
            {
                //if (e.skip_me) { continue; }
                tt += (1 + e.curr_id + e.rand + e.stable_id + e.size) + (e.loaded ? "y" : "n") + (e.showed ? "y" : "n") + (e.rejected ? "y" : "n") + (e.reviewed ? "y" : "n");
            }
            tt = Tools.GetMD5(tt);
            return tt;
        }

        public void UpdateItems(CoreOperations aw, bool check_hash = true)
        {
            lock (control)
            {
                lock (aw.list.list)
                {
                    String hash = "";
                    lock (aw.skipped)
                    {
                        try
                        {
                            //Point p_old = control.AutoScrollOffset;
                            if (control.Visible == false) { return; } // нет смысла
                            if (aw.skipped.Count == 0 && aw.list.Count == 0) { return; }

                            // Сначала проверим, можно ли обойтись без полного обновления

                            control.SuspendLayout();

                            int wc = aw.list.Count;
                            int s1, s2;

                            if (control.items2_list.Count > 0 && check_hash)
                            {
                                hash = GetHash(aw);
                                if (tmp_hash.Equals(hash)) { return; }
                            }

                            Element tmp;
                            double sel_rand = -1;
                            //if (control.TopIndex > 0) { MessageBox.Show(control.TopIndex + ""); }
                            int top_index = control.TopIndex;
                            String top_rand_s = "";
                            double top_rand = -1;
                            try
                            {
                                top_rand_s = control.Items[top_index].ToString();
                                foreach (TMyListBoxItem i in control.items2_list)
                                {
                                    if (i.rand_s.Equals(top_rand_s)) { top_rand = i.rand; break; }
                                }
                            }
                            catch { }

                            int new_index = top_index;

                            foreach (TMyListBoxItem e in control.items2_list)
                            {
                                if (e.selected) { sel_rand = e.rand; break; }
                            }

                            int old_sel_id = control.SelectedIndex;

                            control.BeginUpdate();
                            control.Clear();


                            for (int a = aw.list.list.Count - 1; a >= 0; a--)
                            {
                                tmp = aw.list.list[a];
                                if (tmp.skip_me) { continue; }
                                //s1 = -1; s2 = -1;
                                if (tmp.loaded) { s2 = 1; }
                                else { s2 = 0; }
                                if (tmp.reviewed) { s1 = 0; }
                                else if (tmp.rejected) { s1 = 1; }
                                else if (!tmp.showed) { s1 = 2; }
                                else { s1 = 3; }
                                TMyListBoxItem n = new TMyListBoxItem();
                                n.state1 = s1; n.state2 = s2; n.name = tmp.title; n.pos = a; n.rand = tmp.rand; n.was_ever_reviewed = (tmp.stable_id != -1); n.rand_s = tmp.rand + "";
                                control.AddAssigned(n.rand_s, n);

                                if (top_rand == n.rand) { new_index = control.Items.Count - 1; }
                                if (tmp.rand == sel_rand)
                                {
                                    n.selected = true;
                                }
                            }

                            for (int a = aw.skipped.Count - 1; a >= 0; a--)
                            {
                                if (a < 0) { break; }
                                tmp = aw.skipped.items[a];
                                if (tmp.skip_me) { continue; }
                                //s1 = -1; s2 = -1;
                                if (tmp.reviewed) { s1 = 0; }
                                else if (tmp.rejected) { s1 = 1; }
                                else if (!tmp.showed) { s1 = 2; }
                                else { s1 = 3; }
                                if (tmp.loaded) { s2 = 1; }
                                else { s2 = 0; }
                                TMyListBoxItem n = new TMyListBoxItem();
                                n.state1 = s1; n.state2 = s2; n.name = tmp.title; n.pos = a; n.rand = tmp.rand; n.was_ever_reviewed = (tmp.stable_id != -1); n.rand_s = tmp.rand + "";
                                control.AddAssigned(n.rand_s, n);

                                if (top_rand == n.rand) { new_index = control.Items.Count - 1; }
                                if (tmp.rand == sel_rand)
                                {
                                    n.selected = true;
                                }
                            }


                            control.TopIndex = new_index;
                            control.EndUpdate();
                            // Thread.Sleep(80);
                            control.PerformLayout();
                            tmp_hash = hash;
                            //control.AutoScrollOffset = new Point(p_old.X, p_old.Y);
                            //if (p_old.Y > 0) { MessageBox.Show(p_old.Y+""); }

                        }
                        catch (InvalidOperationException) { }
                        catch (Exception e)
                        {
#if DEBUG
                            MessageBox.Show("Исключение в движке списка: " + e.GetType().ToString() + "|" + e.Message + "|" + e.StackTrace);
#endif
                        }
                    }
                }
            }
        }

        public void MakeCenter(int pos)
        {
            int onpage = control.Height / control.ItemHeight;
            if (pos < 0.5 * onpage) { control.TopIndex=0; return; }
            int total = control.items2_list.Count * control.ItemHeight;
            if (pos > (total - onpage)) { control.TopIndex = total - onpage; return; }
            control.TopIndex = pos - onpage/2;
        }

        public void ScrollTo(Element el)
        {
            try
            {
                foreach (TMyListBoxItem e in control.items2_list)
                {
                    e.selected = false;
                    if (e.rand == el.rand)
                    {
                        int pos = -1;
                        for (int a = 0; a < control.Items.Count; a++)
                        {
                            if (control.Items[a].Equals(e.rand + "")&&pos==-1)
                            {
                                pos = a; //break;
                            }
                        }
                        if (pos == -1) { return; }
                        e.selected = true;
                        control.SetSelected(pos, true);
                        MakeCenter(pos);
                        //e.selected = true;
                        control.Update();

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
            foreach (TMyListBoxItem e in control.items2_list)
            {
                e.selected = false;
                if (e.rand == el.rand) { 
                    e.selected = true; 
                }
            }
            control.Update();
        }

        public void Clear()
        {
            control.Clear();
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
