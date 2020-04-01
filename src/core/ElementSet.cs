using System;
using System.Collections.Generic;
using System.Text;
using DotNetWikiBot;
using System.Threading;

namespace sweepnet
{
    public class ElementSet
    {
        public List<Element> list = new List<Element>(); // впереди
        public List<Element> waslist = new List<Element>(); // уже показанные
        public List<long> waslist_revid = new List<long>();

        public void Add(Element e)
        {
            list.Add(e);
        }

        public void Insert(int pos, Element e)
        {
            list.Insert(pos, e);
        }

        public void Check()
        {
        again:
            foreach (Element el in list)
            {
                if (el.delete_me) { list.Remove(el); goto again; }
            }

        }

        public void Remove(int id)
        {
            waslist.RemoveAt(id);
        }

        public void Remove2(int id)
        {
            list.RemoveAt(id);
        }

        public void ClearAll()
        {
            list.Clear();
            waslist.Clear();
            waslist_revid.Clear();
        }

        public int Count
        {
            get
            {
                return list.Count;
            }
        }

        public Element this[int index]
        {
            set { list[index] = value; }
            get { return list[index]; }
        }
    }

    public class Element
    {
        public String title;
        public SweepNetPage page;
        public long stable_id, curr_id;
        public int diff_size;
        public int page_size;
        public int size;
        public int ns;
        public Thread main_thread = null;
        public bool delete_me = false; //если true, ElementSet удалит элемент при следующей проверке
        public bool skip_me = false;
        public bool check = false; //перепроверить соответствие списку после загрузки списка версий страницы

        public bool reviewed = false, rejected = false, showed = false, loaded = false;
        public bool skipped = false, error = false;

        public double rand = -1;

        public TRevisionSet revisions;

        public Element() { }

        public void FinishStart(Site mysite)
        {
            if (!title.Equals("")) { page = new SweepNetPage(mysite, title); }
            rand = Tools.FullRandom();
        }

        public void Check() // Вызывается при необходимости после загрузки ревизий
        {
            try
            {
                ns = Convert.ToInt32(page.ns);
            }
            catch (Exception) { error = true; return; }
            String allowed_ns = StreamConfig.GetNamespaces();
            if (!allowed_ns.Contains("|" + ns + "|") && !allowed_ns.StartsWith(ns + "|") && !allowed_ns.EndsWith("|" + ns) && !allowed_ns.Equals(page.ns)) { delete_me = true; skip_me = true; return; }
            if (page.flagged_size != -1) { diff_size = page.last_size - page.flagged_size; } else { diff_size = -1; page_size = page.last_size; }
            if (Math.Abs(diff_size) > StreamConfig.GetParamMax()) { delete_me = true; skip_me = true; return; }
            stable_id = page.flagged_rev;
            curr_id = page.maxid;
            if (curr_id == stable_id) { this.reviewed = true; delete_me = true; skip_me = true; return; }

            if (stable_id < 1 && !StreamConfig.IsArticlesEnabled()) { delete_me = true; return; }
            if (stable_id > 0 && !StreamConfig.IsDiffsEnabled()) { delete_me = true; return; }
        }

        public void MakeRevisions()
        {
            revisions = new TRevisionSet(page);
        }
    }
}
