using System;
using System.Collections.Generic;
using System.Text;

namespace sweepnet
{
    public class tl
    {
        public int type = 0;//0-tls,1-revision
        public List<tl> children = new List<tl>();
        public tl last = null;
        public WikiRevision revision = null;

        public bool skip = false;
        public bool hidden = true;
        public int count = 0;

        public String user = "";

        public tl diffto = null; // 2
        public bool diff = false; // 1

        public double rand = -1;


        public tl next = null, prev = null;
        public bool rootflag = false;
        public tl parent = null;
    }

    public class TRevisionSet
    {
        public tl root = new tl();

        public TRevisionSet(SweepNetPage pg)
        {
            List<tl> l = new List<tl>();
            foreach (WikiRevision rev in pg.list)
            {
                tl curr = new tl();
                curr.count = 1;
                curr.type = 1;
                curr.hidden = false;
                curr.revision = rev;
                curr.rand = Tools.FullRandom();
                curr.user = rev.user;
                l.Add(curr);
            }
            RevisionGroup(l);
        }

        private void RevisionGroup(List<tl> list)
        {
            root.children.Clear();
            root.type = 1;
            root.rootflag = true;
            String tmp = "";
            tl curr = new tl();
            curr.type = 0;
            curr.count = 0;

            tl prev_a = null; // group
            tl prev_b = null; // other
            tl prev_prev_a = null;

            foreach (tl t in list)
            {
                if (!t.user.Equals(tmp) || (curr.last != null && t.revision.flagged != curr.last.revision.flagged))
                {
                    if (curr.count > 0) { curr.parent = root; root.children.Add(curr); root.count++; }
                    curr = new tl();
                    curr.type = 0;
                    curr.rand = Tools.FullRandom();
                    curr.count = 0;
                    curr.prev = prev_a;
                    if (prev_a != null) { prev_a.next = curr; }
                    prev_b = null;
                    prev_prev_a = prev_a;
                    prev_a = curr;
                }
                if (prev_b != null) { prev_b.next = t; }
                t.prev = prev_b == null ? prev_prev_a : prev_b;
                prev_b = t;
                t.parent = curr;
                curr.children.Add(t);
                tmp = t.user;
                curr.user = tmp;
                curr.count++;
                curr.revision = t.revision; //последняя ревизия = группа
                curr.last = t;
            }
            if (curr.count > 0) { curr.parent = root; root.children.Add(curr); root.count++; }
        }

        public void AddRevision(WikiRevision rev)
        {
            tl curr = null;
            if (root.children.Count == 0)
            {
                List<tl> l = new List<tl>();
                tl t = new tl();
                t.type = 0;
                t.count = 1;
                t.rand = Tools.FullRandom();
                t.revision = rev;
                l.Add(t);
                RevisionGroup(l);
            }
            else
            {
                curr = root.children[root.children.Count - 1];
                if (!curr.user.Equals(rev.user))
                {
                    curr = new tl();
                    curr.type = 0;
                }
                tl t = new tl();
                t.type = 1;
                t.revision = rev;
                t.rand = Tools.FullRandom();
                curr.children.Add(t);
                curr.count++;
            }

        }

        public tl Get(long revid, tl z = null)
        {
            if (z == null) { z = root; }
            tl r;
            foreach (tl tmp in z.children)
            {
                if (tmp.type == 0) { r = Get(revid, tmp); if (r != null) { return r; } }
                else { if (tmp.revision.revid == revid) { return tmp; } }
            }
            return null;
        }

        public tl this[int key]
        {
            get
            {
                return root.children[key];
            }
            set
            {
                root.children[key] = value;
            }
        }
    }
}
