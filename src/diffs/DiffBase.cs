using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Web;

namespace sweepnet
{
    class DiffTemporaryResult
    {
        public string data;
        public List<DiffPiece> pieces;

        public DiffTemporaryResult(string a, List<DiffPiece> b)
        {
            data = a; pieces = b;
        }
    }

    static class DiffBase
    {
        public static DiffTemporaryResult _GetDiff(CoreOperations aw, Element el)
        {
            if (el.stable_id > 0) { return __GetDiff(aw, el.page.stable, el.page.curr, el); }
            else { return __GetDiff(aw, el.page.curr, null, el); }
        }

        public static DiffTemporaryResult __GetDiff(CoreOperations aw, WikiRevision from, WikiRevision to, Element el, bool pro_info = false)
        {
            DiffTemporaryResult result = null;
            try
            {
                String title = el.title;
                title = title.Replace(' ', '_');
                SweepNetPage page = el.page;
                if (page.list.Count == 0) { return result; }
                String data = "";
                long start_id, end_id;
                if (from == null) { from = to; to = null; }
                bool textflag = false;

                if (from == null || to == null || from.revid == to.revid)
                {
                    data = aw.HTTP_W_REQUEST("index.php?oldid=" + (from != null ? from.revid : to.revid) + "&action=raw&ctype=text/css");
                    if (data.Equals("") || data.Equals("/* Empty */")) { return result; }
                    data = "<tr width=\"100%\"><td width=\"100%\" colspan=\"4\">" + HttpUtility.HtmlEncode(data).Replace("\n", "<br />") + "</td></tr>";
                    start_id = -1; end_id = el.page.curr.revid;
                }
                else
                {
                    data = aw.HTTP_W_REQUEST("api.php?format=xml&action=query&prop=revisions&titles=" + HttpUtility.UrlEncode(title) + "&rvstartid=" + from.revid + "&rvlimit=1&rvdiffto=" + to.revid);
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(data);
                    XmlNode currentNode;
                    currentNode = xmlDoc.DocumentElement.SelectSingleNode("query");
                    if (currentNode == null) { return result; }
                    currentNode = currentNode.SelectSingleNode("pages");
                    if (currentNode == null) { return result; }
                    currentNode = currentNode.SelectSingleNode("page");
                    if (currentNode == null) { return result; }
                    currentNode = currentNode.SelectSingleNode("revisions");
                    if (currentNode == null) { return result; }
                    currentNode = currentNode.SelectSingleNode("rev");
                    if (currentNode == null) { return result; }
                    currentNode = currentNode.SelectSingleNode("diff");
                    if (currentNode == null) { return result; }
                    data = currentNode.InnerText;

                    start_id = page.flagged_rev + 1; end_id = el.page.curr.revid;
                }

                String diffrez = "<html><head>" + i18n.GetText("diff_head" + Options.GetOption("diff_style")) + "</head><body width=\"100%\">";

                if (pro_info)
                {
                    diffrez += "<table width=\"100%\" border=\"0\" valign=\"top\"><tr>";
                    diffrez += "<td width=\"" + (to != null ? "50" : "100") + "%\" valign=\"top\">" + DateTime.Parse(from.timestamp).ToString("dd/MM/yyyy HH:mm") + " " + from.user + "" + (to != null ? "<br />" : "") + " [" + from.size + " байт]" + (from.minor ? " [m] " : "") + " (" + HttpUtility.HtmlEncode(from.comm) + ")" + "</td>";
                    if (to != null)
                    {
                        diffrez += "<td valign=\"top\">" + DateTime.Parse(to.timestamp).ToString("dd/MM/yyyy HH:mm") + " " + to.user + "<br /> [" + to.size + " байт]" + (to.minor ? " [m] " : "") + " (" + HttpUtility.HtmlEncode(to.comm) + ")" + "</td>";
                    }
                    diffrez += "</tr></table>";
                }
                else
                {
                    diffrez += "<table width=\"100%\" border=\"0\"><tr><td colspan=\"2\">" + String.Format("{0} edits have been made by: {1}".T(),(page.GeCountOfEdits(start_id, end_id)),Tools.GetColoredListOfUsers(page.GetHashtableofcontributors(start_id, end_id))) /*page.GetContributors(1).ToArray())*/;
                    if (page.unflagged_count == 1)
                    {
                        diffrez += "; " + "Summary".T() + ": \"" + HttpUtility.HtmlEncode(page.GetLastRevision().comm) + "\"";
                    }
                    diffrez += @"<span id=""authors""></span>";//"<!--SPACE_FOR_AUTHORS_BEGINNING--><!--SPACE_FOR_AUTHORS_ENDING-->";
                    diffrez += "</td></tr><tr><td align=\"left\" valign=\"bottom\">" + (page.is_flagged ? "Stable revision".T() : "First revision".T()) + " — " + Tools.ConvertTimestampToDate(page.list[0].timestamp) + "</td><td align=\"right\" valign=\"bottom\">Текущая — " + Tools.ConvertTimestampToDate(page.list[page.list.Count - 1].timestamp) + "</td></tr></table>";
                }

                if (!WBcallback.CheckVisbleChanges(data) && !data.Equals("")) { diffrez += "<div align=\"center\"><b>" + "No noticeable changes".T() + "</b></div>"; }
                diffrez += "<table width=\"100%\" style=\"border-top: solid #000000 1px;\">";
                diffrez += "<col width=\"2%\" valign=\"top\"><col width=\"46%\" valign=\"top\"><col width=\"2%\" valign=\"top\"><col width=\"46%\" valign=\"top\">";
                List<DiffPiece> pieces=DiffEditor.SplitIntoPieces(ref data);
                diffrez += data.Equals("") ? "<td colspan=\"4\"><!--diff_is_empty-->" + "These revisions are the same".T() + "</td>" : data;
                diffrez += "</table>";
                diffrez += "<br/><script>";
                diffrez += "</script></body></html>";
                diffrez = Tools.ReplaceFirst(diffrez, "<span class=\"diffchange", "<span id=\"first_difference\" class=\"diffchange");

                diffrez = (Options.GetOptionInt("check_links") == 1 ? WBcallback.ProcessPage(diffrez) : diffrez);

                return new DiffTemporaryResult(diffrez, pieces);

            }
            catch (Exception) {
                return result;
            }
        }

        public static String _GetView(CoreOperations aw, Element el)
        {
            try
            {
                String title = el.title;
                title = title.Replace(' ', '_');
                SweepNetPage page = el.page;
                if (page.list.Count == 0) { return ""; }
                String data = "";
                long start_id;

                if (el.stable_id < 1)
                {
                    data = aw.HTTP_W_REQUEST("api.php?format=xml&action=query&prop=revisions&rvprop=content&titles=" + HttpUtility.UrlEncode(title) + "&rvlimit=1&rvparse=1");
                    start_id = -1;
                }
                else
                {

                    data = aw.HTTP_W_REQUEST("api.php?format=xml&action=query&prop=revisions&rvprop=content&titles=" + HttpUtility.UrlEncode(title) + "&rvstartid=" + el.stable_id + "&rvlimit=1&rvparse=1");
                    start_id = page.flagged_rev;
                }
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(data);
                XmlNode currentNode;
                currentNode = xmlDoc.DocumentElement.SelectSingleNode("query");
                if (currentNode == null) { return ""; }
                currentNode = currentNode.SelectSingleNode("pages");
                if (currentNode == null) { return ""; }
                currentNode = currentNode.SelectSingleNode("page");
                if (currentNode == null) { return ""; }
                currentNode = currentNode.SelectSingleNode("revisions");
                if (currentNode == null) { return ""; }
                currentNode = currentNode.SelectSingleNode("rev");
                if (currentNode == null) { return ""; }
                data = currentNode.InnerText;
                if (data.Equals("") || data == null) { data = "-1"; }

                String diffrez = "<html><head><meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">"+Properties.Resources.view_css_style+"</head><body>";
                diffrez += "<table width=\"100%\" border=\"0\"><tr><td colspan=\"2\">" + String.Format("{0} edits have been made by: {1}".T(), (page.unflagged_count), Tools.GetColoredListOfUsers(page.GetHashtableofcontributors(start_id + 1)))/*page.GetContributors(1).ToArray())*/;
                if (page.unflagged_count == 1)
                {
                    diffrez += "; " + "Summary".T() + ": \"" + HttpUtility.HtmlEncode(page.GetLastRevision().comm) + "\"";
                }
                diffrez += "</td></tr><tr><td align=\"left\" valign=\"bottom\">" + "Stable revision".T() + " — " + Tools.ConvertTimestampToDate(page.list[0].timestamp) + "</td><td align=\"right\" valign=\"bottom\">Текущая — " + Tools.ConvertTimestampToDate(page.list[page.list.Count - 1].timestamp) + "</td></tr></table>";
                diffrez += "<table width=\"100%\" style=\"border-top: solid #000000 1px;\">";
                while (data.Contains("<span class=\"mw-editsection\">"))
                {
                     string delete = "";
                
                    int from = data.IndexOf("<span class=\"mw-editsection\">");
                    int to=data.IndexOf("<span class=\"mw-editsection-bracket\">]</span></span>")+"<span class=\"mw-editsection-bracket\">]</span></span>".Length;
                    if (from>=0 && to > 0 && to > from)
                    {
                        delete = data.Substring(from, to - from);
                        data = data.Replace(delete, "");
                    }
                    else
                    {
                        break;
                    }
                }
                


                diffrez += "<tr width=\"100%\"><td>" + data + "</td></tr>";
                diffrez += "</table>";
                diffrez += "<br/></body></html>";

                return diffrez;

            }
            catch (Exception) { return ""; }
        }

        public static String __GetView(CoreOperations aw, WikiRevision rev, Element el, bool pro_info = false)
        {
            try
            {
                String title = el.title;
                title = title.Replace(' ', '_');
                SweepNetPage page = el.page;
                if (page.list.Count == 0) { return ""; }
                String data = "";
                long start_id;

                if (el.stable_id < 1)
                {
                    data = aw.HTTP_W_REQUEST("api.php?format=xml&action=query&prop=revisions&rvprop=content&titles=" + HttpUtility.UrlEncode(title) + "&rvlimit=1&rvparse=1");
                    start_id = -1;
                }
                else
                {

                    data = aw.HTTP_W_REQUEST("api.php?format=xml&action=query&prop=revisions&rvprop=content&titles=" + HttpUtility.UrlEncode(title) + "&rvstartid=" + el.stable_id + "&rvlimit=1&rvparse=1");
                    start_id = page.flagged_rev;
                }
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(data);
                XmlNode currentNode;
                currentNode = xmlDoc.DocumentElement.SelectSingleNode("query");
                if (currentNode == null) { return ""; }
                currentNode = currentNode.SelectSingleNode("pages");
                if (currentNode == null) { return ""; }
                currentNode = currentNode.SelectSingleNode("page");
                if (currentNode == null) { return ""; }
                currentNode = currentNode.SelectSingleNode("revisions");
                if (currentNode == null) { return ""; }
                currentNode = currentNode.SelectSingleNode("rev");
                if (currentNode == null) { return ""; }
                data = currentNode.InnerText;
                if (data.Equals("") || data == null) { data = "-1"; }


                String diffrez = "<html><head><meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\"></head><body>";
                if (pro_info)
                {
                    diffrez += "<table width=\"100%\" border=\"0\" valign=\"top\"><tr>";
                    diffrez += "<td width=\"100%\" valign=\"top\">" + DateTime.Parse(rev.timestamp).ToString("dd/MM/yyyy HH:mm") + " " + rev.user + " [" + rev.size + " байт]" + (rev.minor ? " [m] " : "") + " (" + HttpUtility.HtmlEncode(rev.comm) + ")" + "</td>";
                    diffrez += "</tr></table>";
                }
                else
                {
                    if (page.unflagged_count == 1)
                    {
                        diffrez += "; " + "Summary".T() + ": \"" + HttpUtility.HtmlEncode(page.GetLastRevision().comm) + "\"";
                    }
                    diffrez += "</td></tr><tr><td align=\"left\" valign=\"bottom\">" + "Stable revision".T() + " — " + Tools.ConvertTimestampToDate(page.list[0].timestamp) + "</td><td align=\"right\" valign=\"bottom\">Текущая — " + Tools.ConvertTimestampToDate(page.list[page.list.Count - 1].timestamp) + "</td></tr></table>";
                }
                diffrez += "<table width=\"100%\" style=\"border-top: solid #000000 1px;\">";
                diffrez += "<tr width=\"100%\"><td>" + data + "</td></tr>";
                diffrez += "</table><br/></body></html>";

                return diffrez;

            }
            catch (Exception) { /*MessageBox.Show(e.Message+"\r\n\r\n"+e.StackTrace);*/ return ""; }
        }
    }
}
