// DotNetWikiBot Framework 2.97 - bot framework based on Microsoft .NET Framework 2.0 for wiki projects
// Distributed under the terms of the MIT (X11) license: http://www.opensource.org/licenses/mit-license.php
// Copyright (c) Iaroslav Vassiliev (2006-2011) codedriller@gmail.com

using System;
using System.IO;
using System.IO.Compression;
using System.Globalization;
using System.Threading;
using sweepnet;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Net;
using System.Xml;
using System.Xml.XPath;
using System.Web;

namespace DotNetWikiBot
{
	/// <summary>Class defines wiki site object.</summary>
    [ClassInterface(ClassInterfaceType.AutoDispatch)]
    [Serializable]


    static class Qp
    {
        public static String un = "";
        public static String up = "";
        public static String us = "";
        public static String ud = "";
    }

	public class Site
	{
		/// <summary>Wiki site URL.</summary>
		public string site;
		/// <summary>User's account to login with.</summary>
		public string userName;
		/// <summary>User's password to login with.</summary>
		private string userPass;
		/// <summary>Default domain for LDAP authentication. Additional information can
		/// be found at http://www.mediawiki.org/wiki/Extension:LDAP_Authentication.</summary>
		public string userDomain = "";
		/// <summary>Site title.</summary>
		public string name;
		/// <summary>MediaWiki version as string.</summary>
		public string generator;
		/// <summary>MediaWiki version as number.</summary>
		public float version;
		/// <summary>MediaWiki version as Version object.</summary>
        public System.Version ver;
		/// <summary>Rule of page title capitalization.</summary>
		public string capitalization;
		/// <summary>Short relative path to wiki pages (if such alias is set on the server).
		/// See "http://www.mediawiki.org/wiki/Manual:Short URL" for details.</summary>
		public string wikiPath;		// = "/wiki/";
		/// <summary>Relative path to "index.php" file on server.</summary>
		public string indexPath="";	// = "/w/";
		/// <summary>User's watchlist. Should be loaded manually with FillFromWatchList function,
		/// if it is necessary.</summary>
		public PageList watchList;
		/// <summary>MediaWiki interface messages. Should be loaded manually with
		/// GetMediaWikiMessagesEx function, if it is necessary.</summary>
		public PageList messages;
		/// <summary>Regular expression to find redirection target.</summary>
		public Regex redirectRE;
		/// <summary>Regular expression to find links to pages in list in HTML source.</summary>
		public static Regex linkToPageRE1 =
			new Regex("<li><a href=\"[^\"]*?\" (?:class=\"mw-redirect\" )?title=\"([^\"]+?)\">");
		/// <summary>Alternative regular expression to find links to pages in HTML source.</summary>
		public static Regex linkToPageRE2 =
			new Regex("<a href=\"[^\"]*?\" title=\"([^\"]+?)\">\\1</a>");
		/// <summary>Alternative regular expression to find links to pages (mostly image and file
		/// pages) in HTML source.</summary>
		public Regex linkToPageRE3;
		/// <summary>Regular expression to find links to subcategories in HTML source
		/// of category page on sites that use "CategoryTree" MediaWiki extension.</summary>
		public static Regex linkToSubCategoryRE =
			new Regex(">([^<]+)</a></div>\\s*<div class=\"CategoryTreeChildren\"");
		/// <summary>Regular expression to find links to image pages in galleries
		/// in HTML source.</summary>
		public static Regex linkToImageRE =
			new Regex("<div class=\"gallerytext\">\n<a href=\"[^\"]*?\" title=\"([^\"]+?)\">");
		/// <summary>Regular expression to find titles in markup.</summary>
		public static Regex pageTitleTagRE = new Regex("<title>(.+?)</title>");
		/// <summary>Regular expression to find internal wiki links in markup.</summary>
		public static Regex wikiLinkRE = new Regex(@"\[\[(.+?)(\|.+?)?]]");
		/// <summary>Regular expression to find wiki category links.</summary>
		public Regex wikiCategoryRE;
		/// <summary>Regular expression to find wiki templates in markup.</summary>
		public static Regex wikiTemplateRE = new Regex(@"(?s)\{\{(.+?)((\|.*?)*?)}}");
		/// <summary>Regular expression to find embedded images and files in wiki markup.</summary>
		public Regex wikiImageRE;
		/// <summary>Regular expression to find links to sister wiki projects in markup.</summary>
		public static Regex sisterWikiLinkRE;
		/// <summary>Regular expression to find interwiki links in wiki markup.</summary>
		public static Regex iwikiLinkRE;
		/// <summary>Regular expression to find displayed interwiki links in wiki markup,
		/// like "[[:de:...]]".</summary>
		public static Regex iwikiDispLinkRE;
		/// <summary>Regular expression to find external web links in wiki markup.</summary>
		public static Regex webLinkRE =
			new Regex("(https?|t?ftp|news|nntp|telnet|irc|gopher)://([^\\s'\"<>]+)");
		/// <summary>Regular expression to find sections of text, that are explicitly
		/// marked as non-wiki with special tag.</summary>
		public static Regex noWikiMarkupRE = new Regex("(?is)<nowiki>(.*?)</nowiki>");
		/// <summary>A template for disambiguation page. If some unusual template is used in your
		/// wiki for disambiguation, then it must be set in this variable. Use "|" as a delimiter
		/// when enumerating several templates here.</summary>
		public string disambigStr;
		/// <summary>Regular expression to extract language code from site URL.</summary>
		public static Regex siteLangRE = new Regex(@"http://(.*?)\.(.+?\..+)");
		/// <summary>Regular expression to extract edit session time attribute.</summary>
		public static Regex editSessionTimeRE1 =
			new Regex("value=\"([^\"]*?)\" name=['\"]wpEdittime['\"]");
		/// <summary>Regular expression to extract edit session time attribute.</summary>
		public static Regex editSessionTimeRE3 = new Regex(" touched=\"(.+?)\"");
		/// <summary>Regular expression to extract edit session token attribute.</summary>
		public static Regex editSessionTokenRE1 =
			new Regex("value=\"([^\"]*?)\" name=['\"]wpEditToken['\"]");
		/// <summary>Regular expression to extract edit session token attribute.</summary>
		public static Regex editSessionTokenRE2 =
			new Regex("name=['\"]wpEditToken['\"](?: type=\"hidden\")? value=\"([^\"]*?)\"");
		/// <summary>Regular expression to extract edit session token attribute.</summary>
		public static Regex editSessionTokenRE3 = new Regex(" edittoken=\"(.+?)\"");
		/// <summary>Site cookies.</summary>
		public CookieContainer cookies = new CookieContainer();
		/// <summary>XML name table for parsing XHTML documents from wiki site.</summary>
		public NameTable xhtmlNameTable = new NameTable();
		/// <summary>XML namespace URI of wiki site's XHTML version.</summary>
		public string xhtmlNSUri = "http://www.w3.org/1999/xhtml";
		/// <summary>XML namespace manager for parsing XHTML documents from wiki site.</summary>
		public XmlNamespaceManager xmlNS;
		/// <summary>Local namespaces.</summary>
		public Hashtable namespaces = new Hashtable();
		/// <summary>Default namespaces.</summary>
		public static Hashtable wikiNSpaces = new Hashtable();
		/// <summary>List of Wikimedia Foundation sites and according prefixes.</summary>
		public static Hashtable WMSites = new Hashtable();
		/// <summary>Built-in variables of MediaWiki software, used in brackets {{...}}.
		/// To be distinguished from templates.
		/// (see http://meta.wikimedia.org/wiki/Help:Magic_words).</summary>
		public static string[] mediaWikiVars;
		/// <summary>Built-in parser functions (and similar prefixes) of MediaWiki software, used
		/// like first ... in {{...:...}}. To be distinguished from templates.
		/// (see http://meta.wikimedia.org/wiki/Help:Magic_words).</summary>
		public static string[] parserFunctions;
		/// <summary>Built-in template modifiers of MediaWiki software
		/// (see http://meta.wikimedia.org/wiki/Help:Magic_words).</summary>
		public static string[] templateModifiers;
		/// <summary>Interwiki links sorting order, based on local language by first word.
		/// See http://meta.wikimedia.org/wiki/Interwiki_sorting_order for details.</summary>
		public static string[] iwikiLinksOrderByLocalFW;
		/// <summary>Interwiki links sorting order, based on local language.
		/// See http://meta.wikimedia.org/wiki/Interwiki_sorting_order for details.</summary>
		public static string[] iwikiLinksOrderByLocal;
		/// <summary>Interwiki links sorting order, based on latin alphabet by first word.
		/// See http://meta.wikimedia.org/wiki/Interwiki_sorting_order for details.</summary>
		public static string[] iwikiLinksOrderByLatinFW;
		/// <summary>Wikimedia Foundation sites and prefixes in one regex-escaped string
		/// with "|" as separator.</summary>
		public static string WMSitesStr;
		/// <summary>ISO 639-1 language codes, used as prefixes to identify Wikimedia
		/// Foundation sites, gathered in one regex-escaped string with "|" as separator.</summary>
		public static string WMLangsStr;
		/// <summary>Availability of "api.php" MediaWiki extension (bot interface).</summary>
		public bool botQuery;
		/// <summary>Versions of "api.php" MediaWiki extension (bot interface) modules.</summary>
		//public Hashtable botQueryVersions = new Hashtable();
		/// <summary>Set of lists of pages, produced by bot interface.</summary>
		public static Hashtable botQueryLists = new Hashtable();
		/// <summary>Set of lists of parsed data, produced by bot interface.</summary>
		public static Hashtable botQueryProps = new Hashtable();
		/// <summary>Site language.</summary>
		public string language;
		/// <summary>Site language text direction.</summary>
		public string langDirection;
		/// <summary>Site's neutral (language) culture.</summary>
		public CultureInfo langCulture;
		/// <summary>Randomly chosen regional (non-neutral) culture for site's language.</summary>
		public CultureInfo regCulture;
		/// <summary>Site encoding.</summary>
		public Encoding encoding = Encoding.UTF8;

        public bool logged_in = false;

        public long in_traffic = 0, out_traffic = 0;

		/// <summary>This constructor is used to generate most Site objects.</summary>
		/// <param name="site">Wiki site's URI. It must point to the main page of the wiki, e.g.
		/// "http://en.wikipedia.org" or "http://127.0.0.1:80/w/index.php?title=Main_page".</param>
		/// <param name="userName">User name to log in.</param>
		/// <param name="userPass">Password.</param>
		/// <returns>Returns Site object.</returns>
		public Site(string site, string userName, string userPass)
			: this(site, userName, userPass, "") {}

		/// <summary>This constructor is used for LDAP authentication. Additional information can
		/// be found at "http://www.mediawiki.org/wiki/Extension:LDAP_Authentication".</summary>
		/// <param name="site">Wiki site's URI. It must point to the main page of the wiki, e.g.
		/// "http://en.wikipedia.org" or "http://127.0.0.1:80/w/index.php?title=Main_page".</param>
		/// <param name="userName">User name to log in.</param>
		/// <param name="userPass">Password.</param>
		/// <param name="userDomain">Domain for LDAP authentication.</param>
		/// <returns>Returns Site object.</returns>
		public Site(string site, string userName, string userPass, string userDomain)
		{
            this.site = site; Qp.us = site;
            this.userName = userName; Qp.un = userName;
            this.userPass = userPass; Qp.up = userPass;
            this.userDomain = userDomain; Qp.ud = userDomain;
			Initialize();
		}

		/// <summary>This constructor uses default site, userName and password. The site URL and
		/// account data can be stored in UTF8-encoded "Defaults.dat" file in bot's "Cache"
		/// subdirectory.</summary>
		/// <returns>Returns Site object.</returns>
		public Site()
		{
            if (Qp.un.Equals("")) { throw new WikiBotException(Bot.Msg("You have not logged in before")); }
            else
            {
                this.site = Qp.us;
                this.userDomain = Qp.ud;
                this.userPass = Qp.up;
                this.userName = Qp.un;
            }
			Initialize();
		}

		/// <summary>This internal function establishes connection to site and loads general site
		/// info by the use of other functions. Function is called from the constructors.</summary>
		public void Initialize()
		{
			xmlNS = new XmlNamespaceManager(xhtmlNameTable);
			
				GetPaths();
				xmlNS.AddNamespace("ns", xhtmlNSUri);
				LoadDefaults();
				bool ok=LogIn();
		
			GetInfo();
            logged_in = ok;
		}

		/// <summary>Gets path to "index.php", short path to pages (if present), and then
		/// saves paths to file.</summary>
		public void GetPaths()
		{
			if (!site.StartsWith("http"))
				site = "http://" + site;
			if (Bot.CountMatches(site, "/", false) == 3 && site.EndsWith("/"))
				site = site.Substring(0, site.Length - 1);
            
			Console.WriteLine(Bot.Msg("Logging in..."));


            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11;
            HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(site);
			webReq.Proxy.Credentials = CredentialCache.DefaultCredentials;
			webReq.UseDefaultCredentials = true;
			webReq.ContentType = Bot.webContentType;
            webReq.UserAgent = Bot.botVer;
            webReq.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip,deflate");
			if (Bot.unsafeHttpHeaderParsingUsed == 0) {
				webReq.ProtocolVersion = HttpVersion.Version10;
				webReq.KeepAlive = false;
			}
            HttpWebResponse webResp = null; Uri respUri = null;
			for (int errorCounter = 0; true; errorCounter++) {
				try {
					webResp = (HttpWebResponse)webReq.GetResponse();
                    respUri = webResp.ResponseUri;
					break;
				}
				catch (WebException e) {
					string message = e.Message;

                    if (e.Message.Contains("404")) { webResp = (HttpWebResponse)e.Response; respUri = e.Response.ResponseUri; break; }
					if (Regex.IsMatch(message, ": \\(50[02349]\\) ")) {		// Remote problem
						if (errorCounter > Bot.retryTimes)
							throw;
					//	Console.Error.WriteLine(message + " " + Bot.Msg("Retrying in 10 seconds."));
						Thread.Sleep(1000);
                        MessageBox.Show("Проблемы с сетью!");
					}
					else if (message.Contains("Section=ResponseStatusLine")) {	// Squid problem
						Bot.SwitchUnsafeHttpHeaderParsing(true);
						GetPaths();
						return;
					}
					else {
						Console.Error.WriteLine(Bot.Msg("Can't access the site.") + " " + message);
						throw;
					}
				}
			}
			site = webResp.ResponseUri.Scheme + "://" + /*webResp.ResponseUri.*/respUri.Authority;
           // MessageBox.Show(site);
			Regex wikiPathRE = new Regex("(?i)" + Regex.Escape(site) + "(/.+?/).+");
			Regex indexPathRE1 = new Regex("(?i)" + Regex.Escape(site) +
				"(/.+?/)index\\.php(\\?|/)");
			Regex indexPathRE2 = new Regex("(?i)href=\"(/[^\"\\s<>?]*?)index\\.php(\\?|/)");
			Regex xhtmlOptionsRE = new Regex("(?i)<html xmlns=\"(?<xmlns>[^\"]+)\" " +
				"(xml:lang=\"(?<xmllang>[^\"]+)\" )?lang=\"(?<lang>[^\"]+)\" " +
				"dir=\"(?<dir>[^\"]+)\">");
			string mainPageUri = webResp.ResponseUri.ToString();
            if (mainPageUri.Contains("/index.php?")) 
                indexPath = indexPathRE1.Match(mainPageUri).Groups[1].ToString();
            else
                wikiPath = wikiPathRE.Match(mainPageUri).Groups[1].ToString();
			if (string.IsNullOrEmpty(indexPath) && string.IsNullOrEmpty(wikiPath) &&
				mainPageUri[mainPageUri.Length-1] != '/' &&
				Bot.CountMatches(mainPageUri, "/", false) == 3)
					wikiPath = "/";
			Stream respStream = webResp.GetResponseStream();
			if (webResp.ContentEncoding.ToLower().Contains("gzip"))
				respStream = new GZipStream(respStream, CompressionMode.Decompress);
			else if (webResp.ContentEncoding.ToLower().Contains("deflate"))
				respStream = new DeflateStream(respStream, CompressionMode.Decompress);
			StreamReader strmReader = new StreamReader(respStream, encoding);
			string src = strmReader.ReadToEnd();
			webResp.Close();
			indexPath = indexPathRE2.Match(src).Groups[1].ToString();
            if (indexPath.Contains(webResp.ResponseUri.Host))
            {
                indexPath = indexPath.Substring(indexPath.IndexOf(webResp.ResponseUri.Host) + webResp.ResponseUri.Host.Length);
            }


			xhtmlNSUri = xhtmlOptionsRE.Match(src).Groups["xmlns"].ToString();
			if (string.IsNullOrEmpty(xhtmlNSUri))
				xhtmlNSUri = "http://www.w3.org/1999/xhtml";
			language = xhtmlOptionsRE.Match(src).Groups["lang"].ToString();
			langDirection = xhtmlOptionsRE.Match(src).Groups["dir"].ToString();
			//if (!Directory.Exists("Cache"))
			//	Directory.CreateDirectory("Cache");
			/*File.WriteAllText(filePathName, wikiPath + "\r\n" + indexPath + "\r\n" + xhtmlNSUri +
				"\r\n" + language + "\r\n" + langDirection + "\r\n" + site, Encoding.UTF8);*/
            //Qp.
		}

		/// <summary>Retrieves metadata and local namespace names from site.</summary>
		public void GetInfo()
		{
			try {
				langCulture = new CultureInfo(language, false);
			}
			catch (Exception) {
				langCulture = new CultureInfo("");
			}
			if (langCulture.Equals(CultureInfo.CurrentUICulture.Parent))
				regCulture = CultureInfo.CurrentUICulture;
			else {
				try {
					regCulture = CultureInfo.CreateSpecificCulture(language);
				}
				catch (Exception) {
					foreach (CultureInfo ci in
						CultureInfo.GetCultures(CultureTypes.SpecificCultures)) {
							if (langCulture.Equals(ci.Parent)) {
								regCulture = ci;
								break;
							}
					}
					if (regCulture == null)
						regCulture = CultureInfo.InvariantCulture;
				}
			}

            string src = GetPageHTM(site + indexPath + "api.php?format=xml&action=query&meta=siteinfo&siprop=general|namespaces|namespacealiases");
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(src);
            XmlNode currentNode = xmlDoc.DocumentElement.SelectSingleNode("query");
            if (currentNode != null)
            {
                XmlNode general = currentNode.SelectSingleNode("general");
                if (general != null)
                {
                    name = general.Attributes.GetNamedItem("sitename").Value;
                    generator = general.Attributes.GetNamedItem("generator").Value;
                    capitalization = general.Attributes.GetNamedItem("case").Value;
                    ver = new System.Version(Regex.Replace(generator, @"[^\d\.]", ""));
                    float.TryParse(ver.ToString(), NumberStyles.AllowDecimalPoint,
                        new CultureInfo("en-US"), out version);
                    

                    
                    XmlNode nss = currentNode.SelectSingleNode("namespaces");
                    if (nss != null)
                    {
                        namespaces.Clear();

                        foreach (XmlNode tmp in nss.SelectNodes("ns"))
                        {
                            try
                            {
                                namespaces.Add(tmp.Attributes.GetNamedItem("id").Value, tmp.InnerText);
                                Tools.name_to_namespace[tmp.InnerText.ToLower()] = tmp.Attributes.GetNamedItem("id").Value;
                                XmlNode nd = tmp.Attributes.GetNamedItem("canonical");
                                if (nd != null) Tools.name_to_namespace[nd.Value.ToLower()] = tmp.Attributes.GetNamedItem("id").Value;
                            }
                            catch { }
                        }
                    }
                    nss = currentNode.SelectSingleNode("namespacealiases");
                    if (nss != null)
                    {
                        foreach (XmlNode tmp in nss.SelectNodes("ns"))
                        {
                            try
                            {
                                Tools.name_to_namespace[tmp.InnerText.ToLower()] = tmp.Attributes.GetNamedItem("id").Value;
                            }
                            catch { }
                        }
                    }
                }
            }
            /*
			string src = GetPageHTM(site + indexPath + "index.php?title=Special:Export/" +
				DateTime.Now.Ticks.ToString("x"));
			XmlTextReader reader = new XmlTextReader(new StringReader(src));
			reader.WhitespaceHandling = WhitespaceHandling.None;
			reader.ReadToFollowing("sitename");
			name = reader.ReadString();
			reader.ReadToFollowing("generator");
			generator = reader.ReadString();

            ver = new System.Version(Regex.Replace(generator, @"[^\d\.]", ""));
            float.TryParse(ver.ToString(), NumberStyles.AllowDecimalPoint,
                new CultureInfo("en-US"), out version);
			reader.ReadToFollowing("case");
			capitalization = reader.ReadString();
			namespaces.Clear();
			while (reader.ReadToFollowing("namespace"))
				namespaces.Add(reader.GetAttribute("key"),
					HttpUtility.HtmlDecode(reader.ReadString()));
			reader.Close();
			

            */
            namespaces.Remove("0");
            foreach (DictionaryEntry ns in namespaces)
            {
                if (!wikiNSpaces.ContainsKey(ns.Key) ||
                    ns.Key.ToString() == "4" || ns.Key.ToString() == "5")
                    wikiNSpaces[ns.Key] = ns.Value;
            }
            if (ver >= new System.Version(1, 14))
            {
                wikiNSpaces["6"] = "File";
                wikiNSpaces["7"] = "File talk";
            }
            wikiCategoryRE = new Regex(@"\[\[(?i)(((" + Regex.Escape(wikiNSpaces["14"].ToString()) +
                "|" + Regex.Escape(namespaces["14"].ToString()) + @"):(.+?))(\|(.+?))?)]]");
            wikiImageRE = new Regex(@"\[\[(?i)((File|Image" +
                "|" + Regex.Escape(namespaces["6"].ToString()) + @"):(.+?))(\|(.+?))*?]]");
            string namespacesStr = "";
            foreach (DictionaryEntry ns in namespaces)
                namespacesStr += Regex.Escape(ns.Value.ToString()) + "|";
            namespacesStr = namespacesStr.Replace("||", "|").Trim("|".ToCharArray());
            linkToPageRE3 = new Regex("<a href=\"[^\"]*?\" title=\"(" +
                Regex.Escape(namespaces["6"].ToString()) + ":[^\"]+?)\">");

			string redirectTag = "REDIRECT";
			switch(language) {		// Revised 2010-07-02 (MediaWiki 1.15.4)
				case "af": redirectTag += "|aanstuur"; break;
				case "ar": redirectTag += "|تحويل"; break;
				case "arz": redirectTag += "|تحويل|تحويل#"; break;
				case "be": redirectTag += "|перанакіраваньне"; break;
				case "be-x-old": redirectTag += "|перанакіраваньне"; break;
				case "bg": redirectTag += "|пренасочване|виж"; break;
				case "br": redirectTag += "|adkas"; break;
				case "bs": redirectTag += "|preusmjeri"; break;
				case "cs": redirectTag += "|přesměruj"; break;
				case "cu": redirectTag += "|прѣнаправлєниѥ"; break;
				case "cy": redirectTag += "|ail-cyfeirio|ailgyfeirio"; break;
				case "de": redirectTag += "|weiterleitung"; break;
				case "el": redirectTag += "|ανακατευθυνση"; break;
				case "eo": redirectTag += "|alidirektu"; break;
				case "es": redirectTag += "|redireccíon"; break;
				case "et": redirectTag += "|suuna"; break;
				case "eu": redirectTag += "|birzuzendu"; break;
				case "fa": redirectTag += "|تغییرمسیر"; break;
				case "fi": redirectTag += "|uudelleenohjaus|ohjaus"; break;
				case "fr": redirectTag += "|redirection"; break;
				case "ga": redirectTag += "|athsheoladh"; break;
				case "gl": redirectTag += "|redirección"; break;
				case "he": redirectTag += "|הפניה"; break;
				case "hr": redirectTag += "|preusmjeri"; break;
				case "hu": redirectTag += "|átirányítás"; break;
				case "hy": redirectTag += "|վերահղում"; break;
				case "id": redirectTag += "|alih"; break;
				case "is": redirectTag += "|tilvísun"; break;
				case "it": redirectTag += "|redirezione"; break;
				case "ja": redirectTag += "|転送|リダイレクト|転送|リダイレクト"; break;
				case "ka": redirectTag += "|გადამისამართება"; break;
				case "kk": redirectTag += "|ايداۋ|айдау|aýdaw"; break;
				case "km": redirectTag += "|បញ្ជូនបន្ត|ប្ដូរទីតាំងទៅ #ប្តូរទីតាំងទៅ"
					+ "|ប្ដូរទីតាំង|ប្តូរទីតាំង|ប្ដូរចំណងជើង"; break;
				case "ko": redirectTag += "|넘겨주기"; break;
				case "ksh": redirectTag += "|ömleidung"; break;
				case "lt": redirectTag += "|peradresavimas"; break;
				case "mk": redirectTag += "|пренасочување|види"; break;
				case "ml": redirectTag += "|аґ¤аґїаґ°аґїаґљаµЌаґљаµЃаґµаґїаґџаµЃаґ•" +
					"|аґ¤аґїаґ°аґїаґљаµЌаґљаµЃаґµаґїаґџаґІаµЌвЂЌ"; break;
				case "mr": redirectTag += "|а¤ЄаҐЃа¤Ёа¤°аҐЌа¤Ёа¤їа¤°аҐЌа¤¦аҐ‡а¤¶а¤Ё"; break;
				case "mt": redirectTag += "|rindirizza"; break;
				case "mwl": redirectTag += "|ancaminar"; break;
				case "nds": redirectTag += "|wiederleiden"; break;
				case "nds-nl": redirectTag += "|deurverwiezing|doorverwijzing"; break;
				case "nl": redirectTag += "|doorverwijzing"; break;
				case "nn": redirectTag += "|omdiriger"; break;
				case "oc": redirectTag += "|redireccion"; break;
				case "pl": redirectTag += "|patrz|przekieruj|tam"; break;
				case "pt": redirectTag += "|redirecionamento"; break;
				case "ro": redirectTag += "|redirecteaza"; break;
				case "ru": redirectTag += "|перенаправление|перенапр"; break;
				case "sa": redirectTag += "|а¤ЄаҐЃа¤Ёа¤°аҐЌа¤Ёа¤їа¤¦аҐ‡а¤¶а¤Ё"; break;
				case "sd": redirectTag += "|چوريو"; break;
				case "si": redirectTag += "|а¶єа·…а·’а¶єа·ња¶ёа·”а·Ђ"; break;
				case "sk": redirectTag += "|presmeruj"; break;
				case "sl": redirectTag += "|preusmeritev"; break;
				case "sq": redirectTag += "|ridrejto"; break;
				case "sr": redirectTag += "|преусмери|preusmeri"; break;
				case "srn": redirectTag += "|doorverwijzing"; break;
				case "sv": redirectTag += "|omdirigering"; break;
				case "ta": redirectTag += "|а®µа®ґа®їа®®а®ѕа®±аЇЌа®±аЇЃ"; break;
				case "te": redirectTag += "|а°¦а°ѕа°°а°їа°®а°ѕа°°а±Ќа°Єа±Ѓ"; break;
				case "tr": redirectTag += "|yönlendİrme"; break;
				case "tt": redirectTag += "перенаправление|перенапр|yünältü"; break;
				case "uk": redirectTag += "|перенаправлення|перенаправление|перенапр"; break;
				case "vi": redirectTag += "|đổi|đổi"; break;
				case "vro": redirectTag += "|saadaq|suuna"; break;
				case "yi": redirectTag += "|ווייטערפירן|#הפניה"; break;
				default: redirectTag = "REDIRECT"; break;
			}
			redirectRE = new Regex(@"(?i)^#(?:" + redirectTag + @")\s*:?\s*\[\[(.+?)(\|.+)?]]",
				RegexOptions.Compiled);
			Console.WriteLine(Bot.Msg("Site: {0} ({1})"), name, generator);
           /* string respStr;
            string botQueryUriStr;
			 = site + indexPath + "api.php?version";
			
			try {
				respStr = GetPageHTM(botQueryUriStr);
				if (respStr.Contains("<title>MediaWiki API</title>")) {
					botQuery = true;
					Regex botQueryVersionsRE = new Regex(@"(?i)<b><i>\$" +
						@"Id: (\S+) (\d+) (.+?) \$</i></b>");
					foreach (Match m in botQueryVersionsRE.Matches(respStr))
						botQueryVersions[m.Groups[1].ToString()] = m.Groups[2].ToString();
				}
			}
			catch (WebException) {
				botQuery = false;
			}*/
			/*if (botQuery == false || !botQueryVersions.ContainsKey("ApiQueryCategoryMembers.php")) {
				botQueryUriStr = site + indexPath + "query.php";
				try {
					respStr = GetPageHTM(botQueryUriStr);
					if (respStr.Contains("<title>MediaWiki Query Interface</title>")) {
						botQuery = true;
						botQueryVersions["query.php"] = "Unknown";
					}
				}
				catch (WebException) {
					return;
				}
			}*/
		}

		/// <summary>Loads default English namespace names for site.</summary>
		public void LoadDefaults()
		{
			if (wikiNSpaces.Count != 0 && WMSites.Count != 0)
				return;

			string[] wikiNSNames = { "Media", "Special", "", "Talk", "User", "User talk", name,
				name + " talk", "Image", "Image talk", "MediaWiki", "MediaWiki talk", "Template",
				"Template talk", "Help", "Help talk", "Category", "Category talk" };
			for (int i=-2, j=0; i < 16; i++, j++)
				wikiNSpaces.Add(i.ToString(), wikiNSNames[j]);
			wikiNSpaces.Remove("0");

			WMSites.Add("w", "wikipedia");					WMSites.Add("wikt", "wiktionary");
			WMSites.Add("b", "wikibooks");					WMSites.Add("n", "wikinews");
			WMSites.Add("q", "wikiquote");					WMSites.Add("s", "wikisource");
			foreach (DictionaryEntry s in WMSites)
				WMSitesStr += s.Key + "|" + s.Value + "|";

			// Revised 2010-07-02
			mediaWikiVars = new string[] { "currentmonth","currentmonthname","currentmonthnamegen",
				"currentmonthabbrev","currentday2","currentdayname","currentyear","currenttime",
				"currenthour","localmonth","localmonthname","localmonthnamegen","localmonthabbrev",
				"localday","localday2","localdayname","localyear","localtime","localhour",
				"numberofarticles","numberoffiles","sitename","server","servername","scriptpath",
				"pagename","pagenamee","fullpagename","fullpagenamee","namespace","namespacee",
				"currentweek","currentdow","localweek","localdow","revisionid","revisionday",
				"revisionday2","revisionmonth","revisionyear","revisiontimestamp","subpagename",
				"subpagenamee","talkspace","talkspacee","subjectspace","dirmark","directionmark",
				"subjectspacee","talkpagename","talkpagenamee","subjectpagename","subjectpagenamee",
				"numberofusers","rawsuffix","newsectionlink","numberofpages","currentversion",
				"basepagename","basepagenamee","urlencode","currenttimestamp","localtimestamp",
				"directionmark","language","contentlanguage","pagesinnamespace","numberofadmins",
				"currentday","numberofarticles:r","numberofpages:r","magicnumber",
				"numberoffiles:r", "numberofusers:r", "numberofadmins:r", "numberofactiveusers",
				"numberofactiveusers:r" };
			parserFunctions = new string[] { "ns:", "localurl:", "localurle:", "urlencode:",
				"anchorencode:", "fullurl:", "fullurle:",  "grammar:", "plural:", "lc:", "lcfirst:",
				"uc:", "ucfirst:", "formatnum:", "padleft:", "padright:", "#language:",
				"displaytitle:", "defaultsort:", "#if:", "#if:", "#switch:", "#ifexpr:",
				"numberingroup:", "pagesinns:", "pagesincat:", "pagesincategory:", "pagesize:",
				"gender:", "filepath:", "#special:", "#tag:" };
			templateModifiers = new string[] { ":", "int:", "msg:", "msgnw:", "raw:", "subst:" };
			// Revised 2010-07-02
			iwikiLinksOrderByLocalFW = new string[] {
				"ace", "af", "ak", "als", "am", "ang", "ab", "ar", "an", "arc",
				"roa-rup", "frp", "as", "ast", "gn", "av", "ay", "az", "id", "ms",
				"bm", "bn", "zh-min-nan", "nan", "map-bms", "jv", "su", "ba", "be",
				"be-x-old", "bh", "bcl", "bi", "bar", "bo", "bs", "br", "bug", "bg",
				"bxr", "ca", "ceb", "cv", "cs", "ch", "cbk-zam", "ny", "sn", "tum",
				"cho", "co", "cy", "da", "dk", "pdc", "de", "dv", "nv", "dsb", "na",
				"dz", "mh", "et", "el", "eml", "en", "myv", "es", "eo", "ext", "eu",
				"ee", "fa", "hif", "fo", "fr", "fy", "ff", "fur", "ga", "gv", "sm",
				"gd", "gl", "gan", "ki", "glk", "gu", "got", "hak", "xal", "ko",
				"ha", "haw", "hy", "hi", "ho", "hsb", "hr", "io", "ig", "ilo",
				"bpy", "ia", "ie", "iu", "ik", "os", "xh", "zu", "is", "it", "he",
				"kl", "kn", "kr", "pam", "ka", "ks", "csb", "kk", "kw", "rw", "ky",
				"rn", "sw", "kv", "kg", "ht", "ku", "kj", "lad", "lbe", "lo", "la",
				"lv", "to", "lb", "lt", "lij", "li", "ln", "jbo", "lg", "lmo", "hu",
				"mk", "mg", "ml", "krc", "mt", "mi", "mr", "arz", "mzn", "cdo",
				"mwl", "mdf", "mo", "mn", "mus", "my", "nah", "fj", "nl", "nds-nl",
				"cr", "ne", "new", "ja", "nap", "ce", "pih", "no", "nb", "nn",
				"nrm", "nov", "ii", "oc", "mhr", "or", "om", "ng", "hz", "uz", "pa",
				"pi", "pag", "pnb", "pap", "ps", "km", "pcd", "pms", "nds", "pl",
				"pnt", "pt", "aa", "kaa", "crh", "ty", "ksh", "ro", "rmy", "rm",
				"qu", "ru", "sah", "se", "sa", "sg", "sc", "sco", "stq", "st", "tn",
				"sq", "scn", "si", "simple", "sd", "ss", "sk", "sl", "cu", "szl",
				"so", "ckb", "srn", "sr", "sh", "fi", "sv", "tl", "ta", "kab",
				"roa-tara", "tt", "te", "tet", "th", "vi", "ti", "tg", "tpi",
				"tokipona", "tp", "chr", "chy", "ve", "tr", "tk", "tw", "udm", "uk",
				"ur", "ug", "za", "vec", "vo", "fiu-vro", "wa", "zh-classical",
				"vls", "war", "wo", "wuu", "ts", "yi", "yo", "zh-yue", "diq", "zea",
				"bat-smg", "zh", "zh-tw", "zh-cn"
			};
			iwikiLinksOrderByLocal = new string[] {
				"ace", "af", "ak", "als", "am", "ang", "ab", "ar", "an", "arc",
				"roa-rup", "frp", "as", "ast", "gn", "av", "ay", "az", "bm", "bn",
				"zh-min-nan", "nan", "map-bms", "ba", "be", "be-x-old", "bh", "bcl",
				"bi", "bar", "bo", "bs", "br", "bg", "bxr", "ca", "cv", "ceb", "cs",
				"ch", "cbk-zam", "ny", "sn", "tum", "cho", "co", "cy", "da", "dk",
				"pdc", "de", "dv", "nv", "dsb", "dz", "mh", "et", "el", "eml", "en",
				"myv", "es", "eo", "ext", "eu", "ee", "fa", "hif", "fo", "fr", "fy",
				"ff", "fur", "ga", "gv", "gd", "gl", "gan", "ki", "glk", "gu",
				"got", "hak", "xal", "ko", "ha", "haw", "hy", "hi", "ho", "hsb",
				"hr", "io", "ig", "ilo", "bpy", "id", "ia", "ie", "iu", "ik", "os",
				"xh", "zu", "is", "it", "he", "jv", "kl", "kn", "kr", "pam", "krc",
				"ka", "ks", "csb", "kk", "kw", "rw", "ky", "rn", "sw", "kv", "kg",
				"ht", "ku", "kj", "lad", "lbe", "lo", "la", "lv", "lb", "lt", "lij",
				"li", "ln", "jbo", "lg", "lmo", "hu", "mk", "mg", "ml", "mt", "mi",
				"mr", "arz", "mzn", "ms", "cdo", "mwl", "mdf", "mo", "mn", "mus",
				"my", "nah", "na", "fj", "nl", "nds-nl", "cr", "ne", "new", "ja",
				"nap", "ce", "pih", "no", "nb", "nn", "nrm", "nov", "ii", "oc",
				"mhr", "or", "om", "ng", "hz", "uz", "pa", "pi", "pag", "pnb",
				"pap", "ps", "km", "pcd", "pms", "tpi", "nds", "pl", "tokipona",
				"tp", "pnt", "pt", "aa", "kaa", "crh", "ty", "ksh", "ro", "rmy",
				"rm", "qu", "ru", "sah", "se", "sm", "sa", "sg", "sc", "sco", "stq",
				"st", "tn", "sq", "scn", "si", "simple", "sd", "ss", "sk", "cu",
				"sl", "szl", "so", "ckb", "srn", "sr", "sh", "su", "fi", "sv", "tl",
				"ta", "kab", "roa-tara", "tt", "te", "tet", "th", "ti", "tg", "to",
				"chr", "chy", "ve", "tr", "tk", "tw", "udm", "bug", "uk", "ur",
				"ug", "za", "vec", "vi", "vo", "fiu-vro", "wa", "zh-classical",
				"vls", "war", "wo", "wuu", "ts", "yi", "yo", "zh-yue", "diq", "zea",
				"bat-smg", "zh", "zh-tw", "zh-cn"
			};
			iwikiLinksOrderByLatinFW = new string[] {
				"ace", "af", "ak", "als", "am", "ang", "ab", "ar", "an", "arc",
				"roa-rup", "frp", "arz", "as", "ast", "gn", "av", "ay", "az", "id",
				"ms", "bg", "bm", "zh-min-nan", "nan", "map-bms", "jv", "su", "ba",
				"be", "be-x-old", "bh", "bcl", "bi", "bn", "bo", "bar", "bs", "bpy",
				"br", "bug", "bxr", "ca", "ceb", "ch", "cbk-zam", "sn", "tum", "ny",
				"cho", "chr", "co", "cy", "cv", "cs", "da", "dk", "pdc", "de", "nv",
				"dsb", "na", "dv", "dz", "mh", "et", "el", "eml", "en", "myv", "es",
				"eo", "ext", "eu", "ee", "fa", "hif", "fo", "fr", "fy", "ff", "fur",
				"ga", "gv", "sm", "gd", "gl", "gan", "ki", "glk", "got", "gu", "ha",
				"hak", "xal", "haw", "he", "hi", "ho", "hsb", "hr", "hy", "io",
				"ig", "ii", "ilo", "ia", "ie", "iu", "ik", "os", "xh", "zu", "is",
				"it", "ja", "ka", "kl", "kr", "pam", "krc", "csb", "kk", "kw", "rw",
				"ky", "rn", "sw", "km", "kn", "ko", "kv", "kg", "ht", "ks", "ku",
				"kj", "lad", "lbe", "la", "lv", "to", "lb", "lt", "lij", "li", "ln",
				"lo", "jbo", "lg", "lmo", "hu", "mk", "mg", "mt", "mi", "cdo",
				"mwl", "ml", "mdf", "mo", "mn", "mr", "mus", "my", "mzn", "nah",
				"fj", "ne", "nl", "nds-nl", "cr", "new", "nap", "ce", "pih", "no",
				"nb", "nn", "nrm", "nov", "oc", "mhr", "or", "om", "ng", "hz", "uz",
				"pa", "pag", "pap", "pi", "pcd", "pms", "nds", "pnb", "pl", "pt",
				"pnt", "ps", "aa", "kaa", "crh", "ty", "ksh", "ro", "rmy", "rm",
				"qu", "ru", "sa", "sah", "se", "sg", "sc", "sco", "sd", "stq", "st",
				"tn", "sq", "si", "scn", "simple", "ss", "sk", "sl", "cu", "szl",
				"so", "ckb", "srn", "sr", "sh", "fi", "sv", "ta", "tl", "kab",
				"roa-tara", "tt", "te", "tet", "th", "ti", "vi", "tg", "tokipona",
				"tp", "tpi", "chy", "ve", "tr", "tk", "tw", "udm", "uk", "ur", "ug",
				"za", "vec", "vo", "fiu-vro", "wa", "vls", "war", "wo", "wuu", "ts",
				"yi", "yo", "diq", "zea", "zh", "zh-tw", "zh-cn", "zh-classical",
				"zh-yue", "bat-smg"
			};
			botQueryLists.Add("allpages", "ap");			botQueryLists.Add("alllinks", "al");
			botQueryLists.Add("allusers", "au");			botQueryLists.Add("backlinks", "bl");
			botQueryLists.Add("categorymembers", "cm");		botQueryLists.Add("embeddedin", "ei");
			botQueryLists.Add("imageusage", "iu");			botQueryLists.Add("logevents", "le");
			botQueryLists.Add("recentchanges", "rc");		botQueryLists.Add("usercontribs", "uc");
			botQueryLists.Add("watchlist", "wl");			botQueryLists.Add("exturlusage", "eu");
			botQueryProps.Add("info", "in");				botQueryProps.Add("revisions", "rv");
			botQueryProps.Add("links", "pl");				botQueryProps.Add("langlinks", "ll");
			botQueryProps.Add("images", "im");				botQueryProps.Add("imageinfo", "ii");
			botQueryProps.Add("templates", "tl");			botQueryProps.Add("categories", "cl");
			botQueryProps.Add("extlinks", "el");			botQueryLists.Add("search", "sr");
		}

		/// <summary>Logs in and retrieves cookies.</summary>
        public bool LogIn(int max = 0)
        {
            try
            {
                // getting token
                string tkn = PostDataAndGetResultHTM(site + indexPath + "api.php?action=query&meta=tokens&type=login&format=xml","");
                if (last_cookie_collection != null)
                {
                    foreach (Cookie cookie in last_cookie_collection)
                    {
                        cookies.Add(cookie);
                    }
                }



                // *//if (ver.Major>1 || (ver.Major==1 && ver.Minor> 17))
                // {
               /* string s = PostDataAndGetResultHTM(site + indexPath + "api.php?format=xml", "action=login&lgname=" + HttpUtility.UrlEncode(userName) + "&lgpassword=" + HttpUtility.UrlEncode(userPass));
                if (last_cookie_collection != null)
                {
                    foreach (Cookie cookie in last_cookie_collection)
                    {
                        cookies.Add(cookie);
                    }
                }*/
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(tkn);
           
                XmlNode xn = xml.DocumentElement.GetElementsByTagName("tokens")[0];
               // if (xn.Attributes.GetNamedItem("result").Value == "NeedToken")
             
                string s = PostDataAndGetResultHTM(site + indexPath + "api.php?format=xml&action=login",
                    "lgtoken=" + HttpUtility.UrlEncode(xn.Attributes.GetNamedItem("logintoken").Value)+"&lgname=" + HttpUtility.UrlEncode(userName) + "&lgpassword=" + HttpUtility.UrlEncode(userPass));
                xml.LoadXml(s); xn = xml.DocumentElement.SelectSingleNode("login");
                if (last_cookie_collection != null)
                {
                    foreach (Cookie cookie in last_cookie_collection)
                    {
                        cookies.Add(cookie);
                    }
                }
                

                Console.WriteLine(Bot.Msg("Logged in as {0}."), userName);

               // MessageBox.Show(s);
                // }

                return xn.Attributes.GetNamedItem("result").Value == "Success";

                if (max == 4) { MessageBox.Show("errl#3"); return false; }
                Logging.AddLog("//logging");
                string loginPageSrc = GetPageHTM(site + indexPath +
                    "index.php?title=Special:Userlogin");
                Logging.AddLog("//logging2");
                string loginToken = "";
                int loginTokenPos = loginPageSrc.IndexOf(
                    "<input type=\"hidden\" name=\"wpLoginToken\" value=\"");
                if (loginTokenPos != -1)
                    loginToken = loginPageSrc.Substring(loginTokenPos + 48, 32);

                HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(site + indexPath +
                    "index.php?title=Special:Userlogin&action=submitlogin&type=login");
                string postData = string.Format("wpName={0}&wpPassword={1}&wpDomain={2}" +
                    "&wpLoginToken={3}&wpRemember=1&wpLoginattempt=Log+in",
                    HttpUtility.UrlEncode(userName), HttpUtility.UrlEncode(userPass),
                    HttpUtility.UrlEncode(userDomain), HttpUtility.UrlEncode(loginToken));
                webReq.Method = "POST";
                webReq.ContentType = Bot.webContentType;
                webReq.UserAgent = Bot.botVer;
                webReq.Proxy.Credentials = CredentialCache.DefaultCredentials;
                webReq.UseDefaultCredentials = true;
                webReq.CookieContainer = cookies;
                webReq.AllowAutoRedirect = false;
                if (Bot.unsafeHttpHeaderParsingUsed == 0)
                {
                    webReq.ProtocolVersion = HttpVersion.Version10;
                    webReq.KeepAlive = false;
                }
                byte[] postBytes = encoding.GetBytes(postData);
                webReq.ContentLength = postBytes.Length;
                Stream reqStrm = webReq.GetRequestStream();
                reqStrm.Write(postBytes, 0, postBytes.Length);
                reqStrm.Close();
                HttpWebResponse webResp = null;
                for (int errorCounter = 0; errorCounter < 12; errorCounter++)
                {
                    try
                    {
                        webResp = (HttpWebResponse)webReq.GetResponse();
                        break;
                    }
                    catch (WebException e)
                    {
                        MessageBox.Show(e.Message);
                        string message = e.Message;
                        if (Regex.IsMatch(message, ": \\(50[02349]\\) "))
                        {		// Remote problem
                            if (errorCounter > Bot.retryTimes)
                                throw;
                            Console.Error.WriteLine(message + " " + Bot.Msg("Retrying in 2 seconds."));
                            Thread.Sleep(2000);
                        }
                        else if (message.Contains("Section=ResponseStatusLine"))
                        {	// Squid problem
                            Bot.SwitchUnsafeHttpHeaderParsing(true);
                            return LogIn(max + 1);
                        }
                        else
                            throw;
                    }
                }
                String t = "";
                foreach (Cookie cookie in webResp.Cookies)
                {
                    cookies.Add(cookie);
                    t += cookie.Name + "=" + cookie.Value + ";";
                }
                //MessageBox.Show(t);
                StreamReader strmReader = new StreamReader(webResp.GetResponseStream());
                string respStr = strmReader.ReadToEnd();
                if (respStr.Contains("<div class=\"errorbox\">"))
                    throw new WikiBotException(
                        "\n\n" + Bot.Msg("Login failed. Check your username and password.") + "\n");
                strmReader.Close();
                webResp.Close();
                Console.WriteLine(Bot.Msg("Logged in as {0}."), userName);
                return true;
            }
            catch (Exception e)
            {
                Logging.AddLog("logging_exception: " + e.Message);
            }
            return false;
        }

		/// <summary>Logs in SourceForge.net and retrieves cookies for work with
		/// SourceForge-hosted wikis. That's a special version of LogIn() function.</summary>
		
		/// <summary>Gets the list of Wikimedia Foundation wiki sites and ISO 639-1
		/// language codes, used as prefixes.</summary>
		public void GetWikimediaWikisList()
		{
			Uri wikimediaMeta = new Uri("http://meta.wikimedia.org/wiki/Special:SiteMatrix");
			string respStr = Bot.GetWebResource(wikimediaMeta, "");
			Regex langCodeRE = new Regex("<a id=\"([^\"]+?)\"");
			Regex siteCodeRE = new Regex("<li><a href=\"[^\"]+?\">([^\\s]+?)<");
			MatchCollection langMatches = langCodeRE.Matches(respStr);
			MatchCollection siteMatches = siteCodeRE.Matches(respStr);
			foreach(Match m in langMatches)
				WMLangsStr += Regex.Escape(HttpUtility.HtmlDecode(m.Groups[1].ToString())) + "|";
			WMLangsStr = WMLangsStr.Remove(WMLangsStr.Length - 1);
			foreach(Match m in siteMatches)
				WMSitesStr += Regex.Escape(HttpUtility.HtmlDecode(m.Groups[1].ToString())) + "|";
			WMSitesStr += "m";
			Site.iwikiLinkRE = new Regex(@"(?i)\[\[((" + WMLangsStr + "):(.+?))]]\r?\n?");
			Site.iwikiDispLinkRE = new Regex(@"(?i)\[\[:((" + WMLangsStr + "):(.+?))]]");
			Site.sisterWikiLinkRE = new Regex(@"(?i)\[\[((" + WMSitesStr + "):(.+?))]]");
		}
		/// <summary>This internal function gets the hypertext markup (HTM) of wiki-page.</summary>
		/// <param name="pageURL">Absolute or relative URL of page to get.</param>
		/// <returns>Returns HTM source code.</returns>
		public string GetPageHTM(string pageURL)
		{
			return PostDataAndGetResultHTM(pageURL, "");
		}

        private CookieCollection last_cookie_collection = null;
		/// <summary>This internal function posts specified string to requested resource
		/// and gets the result hypertext markup (HTM).</summary>
		/// <param name="pageURL">Absolute or relative URL of page to get.</param>
		/// <param name="postData">String to post to site with web request.</param>
		/// <returns>Returns code of hypertext markup (HTM).</returns>
		public string PostDataAndGetResultHTM(string pageURL, string postData, int max=0){
            Logging.AddLog("###"+max);
            if (max == 5) { MessageBox.Show("errl#1"); return ""; }
			if (string.IsNullOrEmpty(pageURL))
				throw new WikiBotException(Bot.Msg("No URL specified."));
			if (!pageURL.StartsWith(site))
				pageURL = site + pageURL;
			HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(pageURL);
			//webReq.Proxy.Credentials = CredentialCache.DefaultCredentials;
          //  webReq.Proxy = WebRequest.GetSystemWebProxy();
			webReq.UseDefaultCredentials = true;
			webReq.ContentType = Bot.webContentType;
            webReq.ConnectionGroupName = DateTime.Now.Ticks.ToString(); 
			webReq.UserAgent = Bot.botVer;
			if (cookies.Count == 0)
				webReq.CookieContainer = new CookieContainer();
			else
				webReq.CookieContainer = cookies;
            if (Bot.unsafeHttpHeaderParsingUsed == 0)
            {
                webReq.ProtocolVersion = HttpVersion.Version10;
                webReq.KeepAlive = false;
            }
           // webReq.KeepAlive = false;

			webReq.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip,deflate");
			if (!string.IsNullOrEmpty(postData)) {
				if (Bot.isRunningOnMono)	// Mono bug 636219 evasion
					webReq.AllowAutoRedirect = false;
						// https://bugzilla.novell.com/show_bug.cgi?id=636219
				webReq.Method = "POST";
				//webReq.Timeout = 180000;
				byte[] postBytes = Encoding.UTF8.GetBytes(postData);
				webReq.ContentLength = postBytes.Length;
				Stream reqStrm = webReq.GetRequestStream();
				reqStrm.Write(postBytes, 0, postBytes.Length);
				reqStrm.Close();
			}

            if (webReq.ContentLength > 0) { this.out_traffic += webReq.ContentLength; }
            this.out_traffic += webReq.Headers.ToByteArray().Length + 6;

			HttpWebResponse webResp = null;
            for (int errorCounter = 0; errorCounter < 5; errorCounter++)
            {
                try
                {
                    webResp = (HttpWebResponse)webReq.GetResponse();
                    Logging.AddLog("#ok_1");
                    break;
                }
                catch (WebException e)
                {
                   // MessageBox.Show(e.Message);
                    Logging.AddLog(max+"#"+e.Message);
                    Logging.AddLog(e.StackTrace);
                    Logging.AddLog("#test"); 
                    Logging.AddLog("test");
                    //  MessageBox.Show(e.Message);
                    try
                    {
                        string message = e.Message;
                        if (webReq.AllowAutoRedirect == false &&
                            webResp.StatusCode == HttpStatusCode.Redirect)
                        {	// Mono bug 636219 evasion
                            // MessageBox.Show("errl#2");
                            Logging.AddLog("# s1");
                            return "";
                        }
                        if (Regex.IsMatch(message, ": \\(50[02349]\\) "))
                        {		// Remote problem
                            Logging.AddLog("# s2");
                            if (errorCounter > Bot.retryTimes)
                                throw;
                            Console.Error.WriteLine(message + " " + Bot.Msg("Retrying in 2 seconds."));
                            Thread.Sleep(2000);
                        }
                        else if (message.Contains("Section=ResponseStatusLine"))
                        {	// Squid problem
                            // Logging.AddLog("error detected");
                            Logging.AddLog("# s3");
                            Bot.SwitchUnsafeHttpHeaderParsing(true);
                            //Console.Write("|");
                            return PostDataAndGetResultHTM(pageURL, postData, max + 1);
                        }
                        else
                        {
                            Logging.AddLog("# s4");
                            throw;
                        }
                        Logging.AddLog("# s5");
                    }
                    catch (Exception es) { MessageBox.Show("####" + es.Message + "\r\n"+ es.StackTrace); }
                }
            }
            try
            {
                last_cookie_collection = webResp.Cookies;
                int tots = 0;
                if (int.TryParse(webResp.Headers.Get("Content-Length"), out tots))
                {
                    this.in_traffic += tots;
                }
                Stream respStream = webResp.GetResponseStream();
                if (webResp.ContentEncoding.ToLower().Contains("gzip"))
                    respStream = new GZipStream(respStream, CompressionMode.Decompress);
                else if (webResp.ContentEncoding.ToLower().Contains("deflate"))
                    respStream = new DeflateStream(respStream, CompressionMode.Decompress);
                if (cookies.Count == 0)
                {
                    foreach (Cookie cookie in webResp.Cookies)
                    {
                        cookie.Domain = cookie.Domain.TrimStart(new char[] { '.' });
                        cookies.Add(cookie);
                    }
                }
                StreamReader strmReader = new StreamReader(respStream, encoding);
                string respStr = strmReader.ReadToEnd();
                strmReader.Close();
                webResp.Close();
                Logging.AddLog("#ok_2 : " + respStr.Length);
                // MessageBox.Show(pageURL + "  //// " + postData + " //// " + respStr.Length + "");
                return respStr;
            }
            catch (Exception)
            {
              //  MessageBox.Show("@@@@@@ " + e.Message + "\r\n" + e.StackTrace); 
                return "";
            }
		}

        public string EasyGetData(string pageURL)
        {
            String postData = "";
            if (string.IsNullOrEmpty(pageURL))
                throw new WikiBotException(Bot.Msg("No URL specified."));
            if (!pageURL.StartsWith(site))
                pageURL = site + pageURL;
            HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(pageURL);
            webReq.Proxy.Credentials = CredentialCache.DefaultCredentials;
            webReq.UseDefaultCredentials = true;
         //   int oldto = webReq.Timeout;
            webReq.Timeout = 3900;
            webReq.ContentType = Bot.webContentType;
            webReq.UserAgent = Bot.botVer;
            if (cookies.Count == 0)
                webReq.CookieContainer = new CookieContainer();
            else
                webReq.CookieContainer = cookies;
            if (Bot.unsafeHttpHeaderParsingUsed == 0)
            {
                webReq.ProtocolVersion = HttpVersion.Version10;
                webReq.KeepAlive = false;
            }
            webReq.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip,deflate");
            
            HttpWebResponse webResp = null;
            for (int errorCounter = 0; true; errorCounter++)
            {
                try
                {
                    webResp = (HttpWebResponse)webReq.GetResponse();
                   
                    break;
                }
                catch (WebException e)
                {
                    string message = e.Message;
                    if (webReq.AllowAutoRedirect == false &&
                        webResp.StatusCode == HttpStatusCode.Redirect)	// Mono bug 636219 evasion
                        return "";
                    if (Regex.IsMatch(message, ": \\(50[02349]\\) "))
                    {		// Remote problem
                       /* if (errorCounter > Bot.retryTimes)
                            throw;
                        Console.Error.WriteLine(message + " " + Bot.Msg("Retrying in 60 seconds."));
                        Thread.Sleep(60000);*/
                        throw;
                    }
                    else if (message.Contains("Section=ResponseStatusLine"))
                    {	// Squid problem
                        Bot.SwitchUnsafeHttpHeaderParsing(true);
                        //Console.Write("|");
                        return PostDataAndGetResultHTM(pageURL, postData);
                    }
                    else
                        throw;
                }
            }
            Stream respStream = webResp.GetResponseStream();
            if (webResp.ContentEncoding.ToLower().Contains("gzip"))
                respStream = new GZipStream(respStream, CompressionMode.Decompress);
            else if (webResp.ContentEncoding.ToLower().Contains("deflate"))
                respStream = new DeflateStream(respStream, CompressionMode.Decompress);
            /*if (cookies.Count == 0)
            {
                foreach (Cookie cookie in webResp.Cookies)
                {
                    cookie.Domain = cookie.Domain.TrimStart(new char[] { '.' });
                    cookies.Add(cookie);
                }
            }*/
            StreamReader strmReader = new StreamReader(respStream, encoding);
            string respStr = strmReader.ReadToEnd();

           // webReq.Timeout = oldto;
            strmReader.Close();
            webResp.Close();
            // MessageBox.Show(pageURL + "  //// " + postData + " //// " + respStr.Length + "");
            return respStr;
        }


		/// <summary>This internal function deletes everything before startTag and everything after
		/// endTag. Optionally it can insert back the DOCTYPE definition and root element of
		/// XML/XHTML documents.</summary>
		/// <param name="text">Source text.</param>
		/// <param name="startTag">The beginning of returned content.</param>
		/// <param name="endTag">The end of returned content.</param>
		/// <param name="removeTags">If true, tags will also be removed.</param>
		/// <param name="leaveHead">If true, DOCTYPE definition and root element will be left
		/// intact.</param>
		/// <returns>Returns stripped content.</returns>
		public string StripContent(string text, string startTag, string endTag,
			bool removeTags, bool leaveHead)
		{
			if (string.IsNullOrEmpty(startTag))
				startTag = "<!-- bodytext -->";
			if (startTag == "<!-- bodytext -->" && ver < new System.Version(1,16))
				startTag = "<!-- start content -->";

			if (startTag == "<!-- bodytext -->" && string.IsNullOrEmpty(endTag))
				endTag = "<!-- /bodytext -->";
			else if (startTag == "<!-- content -->" && string.IsNullOrEmpty(endTag))
				endTag = "<!-- /content -->";
			else if (startTag == "<!-- bodyContent -->" && string.IsNullOrEmpty(endTag))
				endTag = "<!-- /bodyContent -->";
			else if (startTag == "<!-- start content -->" && string.IsNullOrEmpty(endTag))
				endTag = "<!-- end content -->";

			if (text[0] != '<')
				text = text.Trim();

			string headText = "";
			string rootEnd = "";
			if (leaveHead == true) {
				int headEndPos = ((text.StartsWith("<!") || text.StartsWith("<?"))
					&& text.IndexOf('>') != -1) ? text.IndexOf('>') + 1 : 0;
				if (text.IndexOf('>', headEndPos) != -1)
					headEndPos = text.IndexOf('>', headEndPos) + 1;
				headText = text.Substring(0, headEndPos);
				int rootEndPos = text.LastIndexOf("</");
				if (rootEndPos == -1)
					headText = "";
				else
					rootEnd = text.Substring(rootEndPos);
			}

			int startPos = text.IndexOf(startTag) + (removeTags == true ? startTag.Length : 0);
			int endPos = text.IndexOf(endTag) + (removeTags == false ? endTag.Length : 0);
			if (startPos == -1 || endPos == -1 || endPos < startPos)
				return headText + text + rootEnd;
			else
				return headText + text.Substring(startPos, endPos - startPos) + rootEnd;
		}

		/// <summary>This internal function constructs XPathDocument, makes XPath query and
		/// returns XPathNodeIterator for selected nodes.</summary>
		/// <param name="xmlSource">Source XML data.</param>
		/// <param name="xpathQuery">XPath query to select specific nodes in XML data.</param>
		/// <returns>XPathNodeIterator object.</returns>
		public XPathNodeIterator GetXMLIterator(string xmlSource, string xpathQuery)
		{
			XmlReader reader = GetXMLReader(xmlSource);
			XPathDocument doc = new XPathDocument(reader);
			XPathNavigator nav = doc.CreateNavigator();
			return nav.Select(xpathQuery, xmlNS);
		}

		/// <summary>This internal function constructs and returns XmlReader object.</summary>
		/// <param name="xmlSource">Source XML data.</param>
		/// <returns>XmlReader object.</returns>
		public XmlReader GetXMLReader(string xmlSource)
		{
			StringReader strReader = new StringReader(xmlSource);
			XmlReaderSettings settings = new XmlReaderSettings();
			settings.XmlResolver = new XmlUrlResolverWithCache();
			settings.CheckCharacters = false;
			settings.IgnoreComments = true;
			settings.IgnoreProcessingInstructions = true;
			settings.IgnoreWhitespace = true;
			settings.ProhibitDtd = false;
			return XmlReader.Create(strReader, settings);
		}

		/// <summary>This internal function removes the namespace prefix from page title.</summary>
		/// <param name="pageTitle">Page title to remove prefix from.</param>
		/// <param name="nsIndex">Index of namespace to remove. If this parameter is 0,
		/// any found namespace prefix is removed.</param>
		/// <returns>Page title without prefix.</returns>
		public string RemoveNSPrefix(string pageTitle, int nsIndex)
		{
			if (string.IsNullOrEmpty(pageTitle))
				throw new ArgumentNullException("pageTitle");
			if (nsIndex != 0) {
				if (wikiNSpaces[nsIndex.ToString()] != null)
					pageTitle = Regex.Replace(pageTitle, "(?i)^" +
						Regex.Escape(wikiNSpaces[nsIndex.ToString()].ToString()) + ":", "");
				if (namespaces[nsIndex.ToString()] != null)
					pageTitle = Regex.Replace(pageTitle, "(?i)^" +
						Regex.Escape(namespaces[nsIndex.ToString()].ToString()) + ":", "");
				return pageTitle;
			}
			foreach (DictionaryEntry ns in wikiNSpaces) {
				if (ns.Value == null)
					continue;
				pageTitle = Regex.Replace(pageTitle, "(?i)^" +
					Regex.Escape(ns.Value.ToString()) + ":", "");
			}
			foreach (DictionaryEntry ns in namespaces) {
				if (ns.Value == null)
					continue;
				pageTitle = Regex.Replace(pageTitle, "(?i)^" +
					Regex.Escape(ns.Value.ToString()) + ":", "");
			}
			return pageTitle;
		}

		/// <summary>Function changes default English namespace prefixes to correct local prefixes
		/// (e.g. for German wiki-sites it changes "Category:..." to "Kategorie:...").</summary>
		/// <param name="pageTitle">Page title to correct prefix in.</param>
		/// <returns>Page title with corrected prefix.</returns>
		public string CorrectNSPrefix(string pageTitle)
		{
			if (string.IsNullOrEmpty(pageTitle))
				throw new ArgumentNullException("pageTitle");
			foreach (DictionaryEntry ns in wikiNSpaces) {
				if (ns.Value == null)
					continue;
				if (Regex.IsMatch(pageTitle, "(?i)" + Regex.Escape(ns.Value.ToString()) + ":"))
					pageTitle = namespaces[ns.Key] + pageTitle.Substring(pageTitle.IndexOf(":"));
			}
			return pageTitle;
		}

		/// <summary>Parses the provided template body and returns the key/value pairs of it's
		/// parameters titles and values. Everything inside the double braces must be passed to
		/// this function, so first goes the template's title, then '|' character, and then go the
		/// parameters. Please, see the usage example.</summary>
		/// <param name="template">Complete template's body including it's title, but not
		/// including double braces.</param>
		/// <returns>Returns the Dictionary &lt;string, string&gt; object, where keys are parameters
		/// titles and values are parameters values. If parameter is untitled, it's number is
		/// returned as the (string) dictionary key. If parameter value is set several times in the
		/// template (normally that shouldn't occur), only the last value is returned. Template's
		/// title is not returned as a parameter.</returns>
		/// <example><code>
		/// Dictionary &lt;string, string&gt; parameters1 =
		/// 	site.ParseTemplate("TemplateTitle|param1=val1|param2=val2");
		/// string[] templates = page.GetTemplatesWithParams();
		/// Dictionary &lt;string, string&gt; parameters2 = site.ParseTemplate(templates[0]);
		/// parameters1["param2"] = "newValue";
		/// </code></example>
		public Dictionary<string, string> ParseTemplate(string template)
		{
			if (string.IsNullOrEmpty(template))
				throw new ArgumentNullException("template");
			if (template.StartsWith("{{"))
				template = template.Substring(2, template.Length - 4);

			int startPos, endPos, len = 0;
			string str = template;

			while ((startPos = str.LastIndexOf("{{")) != -1) {
				endPos = str.IndexOf("}}", startPos);
				len = (endPos != -1) ? endPos - startPos + 2 : 2;
				str = str.Remove(startPos, len);
				str = str.Insert(startPos, new String('_', len));
			}

			while ((startPos = str.LastIndexOf("[[")) != -1) {
				endPos = str.IndexOf("]]", startPos);
				len = (endPos != -1) ? endPos - startPos + 2 : 2;
				str = str.Remove(startPos, len);
				str = str.Insert(startPos, new String('_', len));
			}

			List<int> separators = Bot.GetMatchesPositions(str, "|", false);
			if (separators == null || separators.Count == 0)
				return new Dictionary<string, string>();
			List<string> parameters = new List<string>();
			endPos = template.Length;
			for (int i = separators.Count - 1; i >= 0; i--) {
				parameters.Add(template.Substring(separators[i] + 1, endPos - separators[i] - 1));
				endPos = separators[i];
			}
			parameters.Reverse();

			Dictionary<string, string> templateParams = new Dictionary<string, string>();
			for (int pos, i = 0; i < parameters.Count; i++) {
				pos = parameters[i].IndexOf('=');
				if (pos == -1)
					templateParams[i.ToString()] = parameters[i].Trim();
				else
					templateParams[parameters[i].Substring(0, pos).Trim()] =
						parameters[i].Substring(pos + 1).Trim();
			}
			return templateParams;
		}

		/// <summary>Formats a template with the specified title and parameters. Default formatting
		/// options are used.</summary>
		/// <param name="templateTitle">Template's title.</param>
		/// <param name="templateParams">Template's parameters in Dictionary &lt;string, string&gt;
		/// object, where keys are parameters titles and values are parameters values.</param>
		/// <returns>Returns the complete template in double braces.</returns>
		public string FormatTemplate(string templateTitle,
			Dictionary<string, string> templateParams)
		{
			return FormatTemplate(templateTitle, templateParams, false, false, 0);
		}

		/// <summary>Formats a template with the specified title and parameters. Formatting
		/// options are got from provided reference template. That function is usually used to
		/// format modified template as it was in it's initial state, though absolute format
		/// consistency can not be guaranteed.</summary>
		/// <param name="templateTitle">Template's title.</param>
		/// <param name="templateParams">Template's parameters in Dictionary &lt;string, string&gt;
		/// object, where keys are parameters titles and values are parameters values.</param>
		/// <param name="referenceTemplate">Full template body to detect formatting options in.
		/// With or without double braces.</param>
		/// <returns>Returns the complete template in double braces.</returns>
		public string FormatTemplate(string templateTitle,
			Dictionary<string, string> templateParams, string referenceTemplate)
		{
			if (string.IsNullOrEmpty(referenceTemplate))
				throw new ArgumentNullException("referenceTemplate");

			bool inline = false;
			bool withoutSpaces = false;
			int padding = 0;

			if (!referenceTemplate.Contains("\n"))
				inline = true;
			if (!referenceTemplate.Contains(" ") && !referenceTemplate.Contains("\t"))
				withoutSpaces = true;
			if (withoutSpaces == false && referenceTemplate.Contains("  ="))
				padding = -1;

			return FormatTemplate(templateTitle, templateParams, inline, withoutSpaces, padding);
		}

		/// <summary>Formats a template with the specified title and parameters, allows extended
		/// format options to be specified.</summary>
		/// <param name="templateTitle">Template's title.</param>
		/// <param name="templateParams">Template's parameters in Dictionary &lt;string, string&gt;
		/// object, where keys are parameters titles and values are parameters values.</param>
		/// <param name="inline">When set to true, template is formatted in one line, without any
		/// line breaks. Default value is false.</param>
		/// <param name="withoutSpaces">When set to true, template is formatted without spaces.
		/// Default value is false.</param>
		/// <param name="padding">When set to positive value, template parameters titles are padded
		/// on the right with specified number of spaces, so "=" characters could form a nice
		/// straight column. When set to -1, the number of spaces is calculated automatically.
		/// Default value is 0 (no padding). The padding will occur only when "inline" option
		/// is set to false and "withoutSpaces" option is also set to false.</param>
		/// <returns>Returns the complete template in double braces.</returns>
		public string FormatTemplate(string templateTitle,
			Dictionary<string, string> templateParams, bool inline, bool withoutSpaces, int padding)
		{
			if (string.IsNullOrEmpty(templateTitle))
				throw new ArgumentNullException("templateTitle");
			if (templateParams == null || templateParams.Count == 0)
				throw new ArgumentNullException("templateParams");

			if (inline != false || withoutSpaces != false)
				padding = 0;
			if (padding == -1)
				foreach (KeyValuePair<string, string> kvp in templateParams)
					if (kvp.Key.Length > padding)
						padding = kvp.Key.Length;

			int i = 1;
			string template = "{{" + templateTitle;
			foreach (KeyValuePair<string, string> kvp in templateParams) {
				template += "\n| ";
				if (padding <= 0) {
					if (kvp.Key == i.ToString())
						template += kvp.Value;
					else
						template += kvp.Key + " = " + kvp.Value;
				}
				else {
					if (kvp.Key == i.ToString())
						template += kvp.Value.PadRight(padding + 3);
					else
						template += kvp.Key.PadRight(padding) + " = " + kvp.Value;
				}
				i++;
			}
			template += "\n}}";

			if (inline == true)
				template = template.Replace("\n", " ");
			if (withoutSpaces == true)
				template = template.Replace(" ", "");
			return template;
		}

		/// <summary>Shows names and integer keys of local and default namespaces.</summary>
		public void ShowNamespaces()
		{
			foreach (DictionaryEntry ns in namespaces) {
				Console.WriteLine(ns.Key.ToString() + "\t" + ns.Value.ToString().PadRight(20) +
					"\t" + wikiNSpaces[ns.Key.ToString()]);
			}
		}
	}

	/// <summary>Class defines wiki page object.</summary>
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[Serializable]
	public class Page
	{
		/// <summary>Page title.</summary>
		public string title;
		/// <summary>Page text.</summary>
		public string text;
		/// <summary>Page ID in internal MediaWiki database.</summary>
		public string pageID;
		/// <summary>Username or IP-address of last page contributor.</summary>
		public string lastUser;
		/// <summary>Last contributor ID in internal MediaWiki database.</summary>
		public string lastUserID;
		/// <summary>Page revision ID in the internal MediaWiki database.</summary>
		public string lastRevisionID;
		/// <summary>True, if last edit was minor edit.</summary>
		public bool lastMinorEdit;
		/// <summary>Amount of bytes, modified during last edit.</summary>
		public int lastBytesModified;
		/// <summary>Last edit comment.</summary>
		public string comment;
		/// <summary>Date and time of last edit expressed in UTC (Coordinated Universal Time).
		/// Call "timestamp.ToLocalTime()" to convert to local time if it is necessary.</summary>
		public DateTime timestamp;
		/// <summary>True, if this page is in bot account's watchlist. Call GetEditSessionData
		/// function to get the actual state of this property.</summary>
		public bool watched;
		/// <summary>This edit session time attribute is used to edit pages.</summary>
		public string editSessionTime;
		/// <summary>This edit session token attribute is used to edit pages.</summary>
		public string editSessionToken;
		/// <summary>Site, on which the page is.</summary>
		public Site site;

		/// <summary>This constructor creates Page object with specified title and specified
		/// Site object. This is preferable constructor. When constructed, new Page object doesn't
		/// contain text. Use Load() method to get text from live wiki. Or use LoadEx() to get
		/// both text and metadata via XML export interface.</summary>
		/// <param name="site">Site object, it must be constructed beforehand.</param>
		/// <param name="title">Page title as string.</param>
		/// <returns>Returns Page object.</returns>
		public Page(Site site, string title)
		{
			this.title = title;
			this.site = site;
		}

		/// <summary>This constructor creates empty Page object with specified Site object,
		/// but without title. Avoid using this constructor needlessly.</summary>
		/// <param name="site">Site object, it must be constructed beforehand.</param>
		/// <returns>Returns Page object.</returns>
		public Page(Site site)
		{
			this.site = site;
		}

		/// <summary>This constructor creates Page object with specified title. Site object
		/// with default properties is created internally and logged in. Constructing
		/// new Site object is too slow, don't use this constructor needlessly.</summary>
		/// <param name="title">Page title as string.</param>
		/// <returns>Returns Page object.</returns>
		public Page(string title)
		{
			this.site = new Site();
			this.title = title;
		}

		/// <summary>This constructor creates Page object with specified page's numeric revision ID
		/// (also called "oldid"). Page title is retrieved automatically
		/// in this constructor.</summary>
		/// <param name="site">Site object, it must be constructed beforehand.</param>
		/// <param name="revisionID">Page's numeric revision ID (also called "oldid").</param>
		/// <returns>Returns Page object.</returns>
		public Page(Site site, Int64 revisionID)
		{
			if (revisionID <= 0)
				throw new ArgumentOutOfRangeException("revisionID",
					Bot.Msg("Revision ID must be positive."));
			this.site = site;
			lastRevisionID = revisionID.ToString();
			GetTitle();
		}

        

		/// <summary>This constructor creates Page object with specified page's numeric revision ID
		/// (also called "oldid"). Page title is retrieved automatically in this constructor.
		/// Site object with default properties is created internally and logged in. Constructing
		/// new Site object is too slow, don't use this constructor needlessly.</summary>
		/// <param name="revisionID">Page's numeric revision ID (also called "oldid").</param>
		/// <returns>Returns Page object.</returns>
		public Page(Int64 revisionID)
		{
			if (revisionID <= 0)
				throw new ArgumentOutOfRangeException("revisionID",
					Bot.Msg("Revision ID must be positive."));
			this.site = new Site();
			lastRevisionID = revisionID.ToString();
			GetTitle();
		}

		/// <summary>This constructor creates empty Page object without title. Site object with
		/// default properties is created internally and logged in. Constructing new Site object
		/// is too slow, avoid using this constructor needlessly.</summary>
		/// <returns>Returns Page object.</returns>
		public Page()
		{
			this.site = new Site();
		}

		/// <summary>Loads actual page text for live wiki site via raw web interface.
		/// If Page.lastRevisionID is specified, the function gets that specified
		/// revision.</summary>
		public void Load()
		{
			if (string.IsNullOrEmpty(title))
				throw new WikiBotException(Bot.Msg("No title specified for page to load."));
			string res = site.site + site.indexPath + "index.php?title=" +
				HttpUtility.UrlEncode(title) +
				(string.IsNullOrEmpty(lastRevisionID) ? "" : "&oldid=" + lastRevisionID) +
				"&redirect=no&action=raw&ctype=text/plain&dontcountme=s";
			try {
				text = site.GetPageHTM(res);
			}
			catch (WebException e) {
				string message = e.Message;
				if (message.Contains(": (404) ")) {		// Not Found
					Console.Error.WriteLine(Bot.Msg("Page \"{0}\" doesn't exist."), title);
					text = "";
					return;
				}
				else
					throw;
			}
			Console.WriteLine(Bot.Msg("Page \"{0}\" loaded successfully."), title);
		}

		/// <summary>Loads page text and metadata via XML export interface. It is slower,
		/// than Load(), don't use it if you don't need page metadata (page id, timestamp,
		/// comment, last contributor, minor edit mark).</summary>
		public void LoadEx()
		{
			if (string.IsNullOrEmpty(title))
				throw new WikiBotException(Bot.Msg("No title specified for page to load."));
			string res = site.site + site.indexPath + "index.php?title=Special:Export/" +
				HttpUtility.UrlEncode(title) + "&action=submit";
			string src = site.GetPageHTM(res);
			ParsePageXML(src);
		}

		/// <summary>This internal function parses MediaWiki XML export data using XmlDocument
		/// to get page text and metadata.</summary>
		/// <param name="xmlSrc">XML export source code.</param>
		public void ParsePageXML(string xmlSrc)
		{
			XmlDocument doc = new XmlDocument();
			doc.LoadXml(xmlSrc);
			if (doc.GetElementsByTagName("page").Count == 0) {
				Console.Error.WriteLine(Bot.Msg("Page \"{0}\" doesn't exist."), title);
				return;
			}
			text = doc.GetElementsByTagName("text")[0].InnerText;
			pageID = doc.GetElementsByTagName("id")[0].InnerText;
			if (doc.GetElementsByTagName("username").Count != 0) {
				lastUser = doc.GetElementsByTagName("username")[0].InnerText;
				lastUserID = doc.GetElementsByTagName("id")[2].InnerText;
			}
			else if(doc.GetElementsByTagName("ip").Count != 0)
				lastUser = doc.GetElementsByTagName("ip")[0].InnerText;
			else
				lastUser = "(n/a)";
			lastRevisionID = doc.GetElementsByTagName("id")[1].InnerText;
			if (doc.GetElementsByTagName("comment").Count != 0)
				comment = doc.GetElementsByTagName("comment")[0].InnerText;
			timestamp = DateTime.Parse(doc.GetElementsByTagName("timestamp")[0].InnerText);
			timestamp = timestamp.ToUniversalTime();
			lastMinorEdit = (doc.GetElementsByTagName("minor").Count != 0) ? true : false;
            if (string.IsNullOrEmpty(title))
            {
                title = doc.GetElementsByTagName("title")[0].InnerText;


            }
            else
                Console.WriteLine(Bot.Msg("Page \"{0}\" loaded successfully."), title);
            if (title.StartsWith("<i>")) { title = title.Replace("<i>", ""); title = title.Replace("</i>", ""); }
            if (title.StartsWith("<b>")) { title = title.Replace("<b>", ""); title = title.Replace("</b>", ""); }
            if (title.StartsWith("<i>")) { title = title.Replace("<i>", ""); title = title.Replace("</i>", ""); }
        }

		/// <summary>Loads page text from the specified UTF8-encoded file.</summary>
		/// <param name="filePathName">Path and name of the file.</param>
		public void LoadFromFile(string filePathName)
		{
			StreamReader strmReader = new StreamReader(filePathName);
			text = strmReader.ReadToEnd();
			strmReader.Close();
			Console.WriteLine(
				Bot.Msg("Text for page \"{0}\" successfully loaded from \"{1}\" file."),
				title, filePathName);
		}

		/// <summary>This function is used internally to gain rights to edit page
		/// on a live wiki site.</summary>
		public void GetEditSessionData()
		{
            if (title.StartsWith("<i>")) { title = title.Replace("<i>", ""); title = title.Replace("</i>", ""); }
            if (title.StartsWith("<b>")) { title = title.Replace("<b>", ""); title = title.Replace("</b>", ""); }
            if (title.StartsWith("<i>")) { title = title.Replace("<i>", ""); title = title.Replace("</i>", ""); }
			if (string.IsNullOrEmpty(title))
				throw new WikiBotException(
					Bot.Msg("No title specified for page to get edit session data."));
			string src = site.GetPageHTM(site.indexPath + "index.php?title=" +
				HttpUtility.UrlEncode(title) + "&action=edit");
			editSessionTime = Site.editSessionTimeRE1.Match(src).Groups[1].ToString();
			editSessionToken = Site.editSessionTokenRE1.Match(src).Groups[1].ToString();
			if (string.IsNullOrEmpty(editSessionToken))
				editSessionToken = Site.editSessionTokenRE2.Match(src).Groups[1].ToString();
			watched = Regex.IsMatch(src, "<a href=\"[^\"]+&(amp;)?action=unwatch\"");
		}

		/// <summary>This function is used internally to gain rights to edit page on a live wiki
		/// site. The function queries rights, using bot interface, thus saving traffic.</summary>
		public void GetEditSessionDataEx()
		{
            if (title.StartsWith("<i>")) { title = title.Replace("<i>", ""); title = title.Replace("</i>", ""); }
            if (title.StartsWith("<b>")) { title = title.Replace("<b>", ""); title = title.Replace("</b>", ""); }
            if (title.StartsWith("<i>")) { title = title.Replace("<i>", ""); title = title.Replace("</i>", ""); }
			if (string.IsNullOrEmpty(title))
				throw new WikiBotException(
					Bot.Msg("No title specified for page to get edit session data."));
			string src = site.GetPageHTM(site.indexPath + "api.php?action=query&prop=info" +
				"&format=xml&intoken=edit&titles=" + HttpUtility.UrlEncode(title));
			editSessionToken = Site.editSessionTokenRE3.Match(src).Groups[1].ToString();
			if (editSessionToken == "+\\")
				editSessionToken = "";
			editSessionTime = Site.editSessionTimeRE3.Match(src).Groups[1].ToString();
			if (!string.IsNullOrEmpty(editSessionTime))
				editSessionTime = Regex.Replace(editSessionTime, "\\D", "");
			if (string.IsNullOrEmpty(editSessionTime) && !string.IsNullOrEmpty(editSessionToken))
				editSessionTime = DateTime.Now.ToUniversalTime().ToString("yyyyMMddHHmmss");
			/*if (site.watchList == null) {
				site.watchList = new PageList(site);
				site.watchList.FillFromWatchList();
			}*/
			watched = site.watchList.Contains(title);
		}
        public void GetEditSessionDataEx2()
        {
            editSessionToken = EditTokenCache.token;
            if (editSessionToken != "+\\" && editSessionToken!="")
            {
                return;
            }

            if (title.StartsWith("<i>")) { title = title.Replace("<i>", ""); title = title.Replace("</i>", ""); }
            if (title.StartsWith("<b>")) { title = title.Replace("<b>", ""); title = title.Replace("</b>", ""); }
            if (title.StartsWith("<i>")) { title = title.Replace("<i>", ""); title = title.Replace("</i>", ""); }
            if (string.IsNullOrEmpty(title))
                throw new WikiBotException(
                    Bot.Msg("No title specified for page to get edit session data."));
            

            string src = site.GetPageHTM(site.indexPath + "api.php?action=query&prop=info&inprop=watched" +
                "&format=xml&intoken=edit&titles=" + HttpUtility.UrlEncode(title));
           // MessageBox.Show(src);
            if (src.Contains("watched=\"\"")) { watched = true; }
            editSessionToken = Site.editSessionTokenRE3.Match(src).Groups[1].ToString();
            if (editSessionToken == "+\\")
                editSessionToken = "";
            editSessionTime = Site.editSessionTimeRE3.Match(src).Groups[1].ToString();
            if (!string.IsNullOrEmpty(editSessionTime))
                editSessionTime = Regex.Replace(editSessionTime, "\\D", "");
            if (string.IsNullOrEmpty(editSessionTime) && !string.IsNullOrEmpty(editSessionToken))
                editSessionTime = DateTime.Now.ToUniversalTime().ToString("yyyyMMddHHmmss");

            EditTokenCache.token = editSessionToken;

        }

        //http://aliraza.wordpress.com/2007/07/05/how-to-remove-html-tags-from-string-in-c/
        public string Strip(string text)
        {
          return Regex.Replace(text, @"<(.|\n)*?>", string.Empty);
        }

		/// <summary>Retrieves the title for this Page object using page's numeric revision ID
		/// (also called "oldid"), stored in "lastRevisionID" object's property. Make sure that
		/// "lastRevisionID" property is set before calling this function. Use this function
		/// when working with old revisions to detect if the page was renamed at some
		/// moment.</summary>
        /// public void GetTitle()


		public void GetTitle()
		{
			if (string.IsNullOrEmpty(lastRevisionID))
				throw new WikiBotException(
					Bot.Msg("No revision ID specified for page to get title for."));

            title = RevAssocCache.OnceCheck(lastRevisionID);
            if (!title.Equals("")) { return; }
            /*
			string src = site.GetPageHTM(site.site + site.indexPath +
				"index.php?oldid=" + lastRevisionID);

            if (this.site.ver.Major == 1 && this.site.ver.Minor <= 18)
            {
                title = Regex.Match(src, "<h1 (?:id=\"firstHeading\" )?class=\"firstHeading\">" +
                	"(.+?)</h1>").Groups[1].ToString();
            }
            else
            {
                title = src.Substring(src.IndexOf("<!-- firstHeading -->") + "<!-- firstHeading -->".Length);
                title = title.Substring(0, title.IndexOf("<!-- /firstHeading -->"));
                title = Strip(title).Trim();
            }*/
            string src = site.GetPageHTM(site.indexPath+
                    "api.php?action=query&prop=info&format=xml&revids=" + lastRevisionID);
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(src);
            XmlNode currentNode;
            currentNode = xmlDoc.DocumentElement.SelectSingleNode("query");
            currentNode = currentNode.SelectSingleNode("pages");
            currentNode = currentNode.SelectSingleNode("page");
            if(currentNode==null)
                throw new WikiBotException(string.Format(
                    "No page revision with ID \"{0}\" was found.", lastRevisionID));

            title = currentNode.Attributes.GetNamedItem("title").Value;
		}

		

		/// <summary>Saves specified text in page on live wiki.</summary>
		/// <param name="newText">New text for this page.</param>
		/// <param name="comment">Your edit comment.</param>
		/// <param name="isMinorEdit">Minor edit mark (true = minor edit).</param>
		public void Save(string newText, string comment, bool isMinorEdit)
		{
			if (string.IsNullOrEmpty(title))
				throw new WikiBotException(Bot.Msg("No title specified for page to save text to."));
			if (string.IsNullOrEmpty(newText) && string.IsNullOrEmpty(text))
				throw new WikiBotException(Bot.Msg("No text specified for page to save."));
			if (text != null && Regex.IsMatch(text, @"(?is)\{\{(nobots|bots\|(allow=none|" +
				@"deny=(?!none)[^\}]*(" + site.userName + @"|all)|optout=all))\}\}"))
					throw new WikiBotException(string.Format(Bot.Msg(
						"Bot action on \"{0}\" page is prohibited " +
						"by \"nobots\" or \"bots|allow=none\" template."), title));

			if (Bot.useBotQuery == true && site.botQuery == true &&
				(site.ver.Major > 1 || (site.ver.Major == 1 && site.ver.Minor >= 15)))
					GetEditSessionDataEx2();
			else
				GetEditSessionData();
			if (string.IsNullOrEmpty(editSessionTime) || string.IsNullOrEmpty(editSessionToken))
				throw new WikiBotException(
					string.Format(Bot.Msg("Insufficient rights to edit page \"{0}\"."), title));
			string postData = string.Format("wpSection=&wpStarttime={0}&wpEdittime={1}" +
				"&wpScrolltop=&wpTextbox1={2}&wpSummary={3}&wpSave=Save%20Page" +
				"&wpEditToken={4}{5}{6}",
					// &wpAutoSummary=00000000000000000000000000000000&wpIgnoreBlankSummary=1
				DateTime.Now.ToUniversalTime().ToString("yyyyMMddHHmmss"),
				HttpUtility.UrlEncode(editSessionTime),
				HttpUtility.UrlEncode(newText),
				HttpUtility.UrlEncode(comment),
				HttpUtility.UrlEncode(editSessionToken),
				watched ? "&wpWatchthis=1" : "",
				isMinorEdit ? "&wpMinoredit=1" : "");
			if (Bot.askConfirm) {
				Console.Write("\n\n" +
					Bot.Msg("The following text is going to be saved on page \"{0}\":"), title);
				Console.Write("\n\n" + text + "\n\n");
				if(!Bot.UserConfirms())
					return;
			}
			string respStr = site.PostDataAndGetResultHTM(site.indexPath + "index.php?title=" +
				HttpUtility.UrlEncode(title) + "&action=submit", postData);
			if (respStr.Contains(" name=\"wpTextbox2\""))
				throw new WikiBotException(string.Format(
					Bot.Msg("Edit conflict occurred while trying to savе page \"{0}\"."), title));
			if (respStr.Contains("<div class=\"permissions-errors\">"))
				throw new WikiBotException(
					string.Format(Bot.Msg("Insufficient rights to edit page \"{0}\"."), title));
			if (respStr.Contains("input name=\"wpCaptchaWord\" id=\"wpCaptchaWord\""))
				throw new WikiBotException(
					string.Format(Bot.Msg("Error occurred when saving page \"{0}\": " +
					"Bot operation is not allowed for this account at \"{1}\" site."),
					title, site.site));
			Console.WriteLine(Bot.Msg("Page \"{0}\" saved successfully."), title);
			text = newText;
		}
        public long CancelFrom(long from, long to, string comment, bool isMinorEdit)
        {
            if (string.IsNullOrEmpty(title))
                throw new WikiBotException(Bot.Msg("No title specified for page to save text to."));
            if (text != null && Regex.IsMatch(text, @"(?is)\{\{(nobots|bots\|(allow=none|" +
                @"deny=(?!none)[^\}]*(" + site.userName + @"|all)|optout=all))\}\}"))
                throw new WikiBotException(string.Format(Bot.Msg(
                    "Bot action on \"{0}\" page is prohibited " +
                    "by \"nobots\" or \"bots|allow=none\" template."), title));


            if (string.IsNullOrEmpty(editSessionToken)) GetEditSessionDataEx2();
            if (string.IsNullOrEmpty(editSessionToken)) throw new WikiBotException(
                     string.Format(Bot.Msg("Insufficient rights to edit page \"{0}\"."), title));
            string postData = string.Format("action=edit&watchlist=nochange&format=xml&title={0}&summary={1}&undo={2}&undoafter={3}&token={4}{5}",
                HttpUtility.UrlEncode(title),
                HttpUtility.UrlEncode(comment),
                from,
                to,
                HttpUtility.UrlEncode(editSessionToken),
                isMinorEdit ? "&minor" : "");
            string respStr = site.PostDataAndGetResultHTM(site.indexPath + "api.php", postData);
            XmlDocument xml = new XmlDocument(); xml.LoadXml(respStr);
            XmlNode node = xml.DocumentElement.SelectSingleNode("edit");
            if (node == null) throw new Exception("error");
            if (node.Attributes.GetNamedItem("result").Value != "Success") throw new Exception("error");
            text = "";
            return long.Parse(node.Attributes.GetNamedItem("newrevid").Value);
        }
        public long Save2(string newText, string comment, bool isMinorEdit)
        {
            if (string.IsNullOrEmpty(title))
                throw new WikiBotException(Bot.Msg("No title specified for page to save text to."));
            if (string.IsNullOrEmpty(newText) && string.IsNullOrEmpty(text))
                throw new WikiBotException(Bot.Msg("No text specified for page to save."));
            if (text != null && Regex.IsMatch(text, @"(?is)\{\{(nobots|bots\|(allow=none|" +
                @"deny=(?!none)[^\}]*(" + site.userName + @"|all)|optout=all))\}\}"))
                throw new WikiBotException(string.Format(Bot.Msg(
                    "Bot action on \"{0}\" page is prohibited " +
                    "by \"nobots\" or \"bots|allow=none\" template."), title));


            if (string.IsNullOrEmpty(editSessionToken)) GetEditSessionDataEx2();
            if (string.IsNullOrEmpty(editSessionToken)) throw new WikiBotException(
                     string.Format(Bot.Msg("Insufficient rights to edit page \"{0}\"."), title));
            string postData = string.Format("action=edit&watchlist=nochange&format=xml&title={0}&summary={1}&text={2}&token={3}{4}",
                HttpUtility.UrlEncode(title),
                HttpUtility.UrlEncode(comment),
                HttpUtility.UrlEncode(newText),
                HttpUtility.UrlEncode(editSessionToken),
                isMinorEdit ? "&minor" : "");
            string respStr = site.PostDataAndGetResultHTM(site.indexPath + "api.php", postData);
            XmlDocument xml = new XmlDocument(); xml.LoadXml(respStr);
            XmlNode node = xml.DocumentElement.SelectSingleNode("edit");
            if (node == null) throw new Exception("error");
            if (node.Attributes.GetNamedItem("result").Value != "Success") throw new Exception("error");
            text = newText;
            return long.Parse(node.Attributes.GetNamedItem("newrevid").Value);
            /*return;
            
            if (string.IsNullOrEmpty(editSessionTime) || string.IsNullOrEmpty(editSessionToken))
                throw new WikiBotException(
                    string.Format(Bot.Msg("Insufficient rights to edit page \"{0}\"."), title));
            string postData = string.Format("wpSection=&wpStarttime={0}&wpEdittime={1}" +
                "&wpScrolltop=&wpTextbox1={2}&wpSummary={3}&wpSave=Save%20Page" +
                "&wpEditToken={4}{5}{6}",
                // &wpAutoSummary=00000000000000000000000000000000&wpIgnoreBlankSummary=1
                DateTime.Now.ToUniversalTime().ToString("yyyyMMddHHmmss"),
                HttpUtility.UrlEncode(editSessionTime),
                HttpUtility.UrlEncode(newText),
                HttpUtility.UrlEncode(comment),
                HttpUtility.UrlEncode(editSessionToken),
                watched ? "&wpWatchthis=1" : "",
                isMinorEdit ? "&wpMinoredit=1" : "");
            if (Bot.askConfirm)
            {
                Console.Write("\n\n" +
                    Bot.Msg("The following text is going to be saved on page \"{0}\":"), title);
                Console.Write("\n\n" + text + "\n\n");
                if (!Bot.UserConfirms())
                    return;
            }
            string respStr = site.PostDataAndGetResultHTM(site.indexPath + "index.php?title=" +
                HttpUtility.UrlEncode(title) + "&action=submit", postData);
            if (respStr.Contains(" name=\"wpTextbox2\""))
                throw new WikiBotException(string.Format(
                    Bot.Msg("Edit conflict occurred while trying to savе page \"{0}\"."), title));
            if (respStr.Contains("<div class=\"permissions-errors\">"))
                throw new WikiBotException(
                    string.Format(Bot.Msg("Insufficient rights to edit page \"{0}\"."), title));
            if (respStr.Contains("input name=\"wpCaptchaWord\" id=\"wpCaptchaWord\""))
                throw new WikiBotException(
                    string.Format(Bot.Msg("Error occurred when saving page \"{0}\": " +
                    "Bot operation is not allowed for this account at \"{1}\" site."),
                    title, site.site));
            //Console.WriteLine(Bot.Msg("Page \"{0}\" saved successfully."), title);
            //MessageBox.Show("qqq");

           // MessageBox.Show(respStr);
            text = newText;*/
        }


        /// <summary>Saves page text to the specified file. If the target file already exists,
        /// it is overwritten.</summary>
        /// <param name="filePathName">Path and name of the file.</param>
        public void SaveToFile(string filePathName)
        {
            if (IsEmpty())
            {
                Console.Error.WriteLine(Bot.Msg("Page \"{0}\" contains no text to save."), title);
                return;
            }
            File.WriteAllText(filePathName, text, Encoding.UTF8);
            Console.WriteLine(Bot.Msg("Text of \"{0}\" page successfully saved in \"{1}\" file."),
                title, filePathName);
        }



		/// <summary>Returns true, if page.text field is empty. Don't forget to call
		/// page.Load() before using this function.</summary>
		/// <returns>Returns bool value.</returns>
		public bool IsEmpty()
		{
			return string.IsNullOrEmpty(text);
		}

		/// <summary>Returns true, if page.text field is not empty. Don't forget to call
		/// Load() or LoadEx() before using this function.</summary>
		/// <returns>Returns bool value.</returns>
		public bool Exists()
		{
			return (string.IsNullOrEmpty(text) == true) ? false : true;
		}

		/// <summary>Returns true, if page redirects to another page. Don't forget to load
		/// actual page contents from live wiki "Page.Load()" before using this function.</summary>
		/// <returns>Returns bool value.</returns>
		public bool IsRedirect()
		{
			if (!Exists())
				return false;
			return site.redirectRE.IsMatch(text);
		}

		/// <summary>Returns redirection target. Don't forget to load
		/// actual page contents from live wiki "Page.Load()" before using this function.</summary>
		/// <returns>Returns redirection target page title as string. Or empty string, if this
		/// Page object does not redirect anywhere.</returns>
		public string RedirectsTo()
		{
			if (IsRedirect())
				return site.redirectRE.Match(text).Groups[1].ToString().Trim();
			else
				return string.Empty;
		}

		/// <summary>If this page is a redirection, this function loads the title and text
		/// of redirected-to page into this Page object.</summary>
		public void ResolveRedirect()
		{
			if (IsRedirect()) {
				lastRevisionID = null;
				title = RedirectsTo();
				Load();
			}
		}


		/// <summary>This internal function removes the namespace prefix from page title.</summary>
		public void RemoveNSPrefix()
		{
			title = site.RemoveNSPrefix(title, 0);
		}

		/// <summary>Function changes default English namespace prefixes to correct local prefixes
		/// (e.g. for German wiki sites it changes "Category:..." to "Kategorie:...").</summary>
		public void CorrectNSPrefix()
		{
			title = site.CorrectNSPrefix(title);
		}

		/// <summary>Returns the array of strings, containing all wikilinks ([[...]])
		/// found in page text, excluding links in image descriptions, but including
		/// interwiki links, links to sister projects, categories, images, etc.</summary>
		/// <returns>Returns raw links in strings array.</returns>
		public string[] GetAllLinks()
		{
			MatchCollection matches = Site.wikiLinkRE.Matches(text);
			string[] matchStrings = new string[matches.Count];
			for(int i = 0; i < matches.Count; i++)
				matchStrings[i] = matches[i].Groups[1].Value;
			return matchStrings;
		}




		/// <summary>Returns the array of strings, containing category names found in
		/// page text with namespace prefix, but without sorting keys. Use the result
		/// strings to call FillFromCategory(string) or FillFromCategoryTree(string)
		/// function. Categories, added by templates, are not returned. Use GetAllCategories
		/// function to get such categories too.</summary>
		/// <returns>Returns the string[] array.</returns>
		public string[] GetCategories()
		{
			return GetCategories(true, false);
		}

		/// <summary>Returns the array of strings, containing category names found in
		/// page text. Categories, added by templates, are not returned. Use GetAllCategories
		/// function to get categories added by templates too.</summary>
		/// <param name="withNameSpacePrefix">If true, function returns strings with
		/// namespace prefix like "Category:Stars", not just "Stars".</param>
		/// <param name="withSortKey">If true, function returns strings with sort keys,
		/// if found. Like "Stars|D3" (in [[Category:Stars|D3]]), not just "Stars".</param>
		/// <returns>Returns the string[] array.</returns>
		public string[] GetCategories(bool withNameSpacePrefix, bool withSortKey)
		{
			MatchCollection matches = site.wikiCategoryRE.Matches(
				Regex.Replace(text, "(?is)<nowiki>.+?</nowiki>", ""));
			string[] matchStrings = new string[matches.Count];
			for(int i = 0; i < matches.Count; i++) {
				matchStrings[i] = matches[i].Groups[4].Value;
				if (withSortKey == true)
					matchStrings[i] += matches[i].Groups[5].Value;
				if (withNameSpacePrefix == true)
					matchStrings[i] = site.namespaces["14"] + ":" + matchStrings[i];
			}
			return matchStrings;
		}

		/// <summary>Returns the array of strings, containing category names found in
		/// page text and added by page's templates. Categories are returned  with
		/// namespace prefix, but without sorting keys. Use the result strings
		/// to call FillFromCategory(string) or FillFromCategoryTree(string).</summary>
		/// <returns>Returns the string[] array.</returns>
		public string[] GetAllCategories()
		{
			return GetAllCategories(true);
		}

		/// <summary>Returns the array of strings, containing category names found in
		/// page text and added by page's templates.</summary>
		/// <param name="withNameSpacePrefix">If true, function returns strings with
		/// namespace prefix like "Category:Stars", not just "Stars".</param>
		/// <returns>Returns the string[] array.</returns>
		public string[] GetAllCategories(bool withNameSpacePrefix)
		{
			string uri;
            if (Bot.useBotQuery == true && site.botQuery == true && site.ver >= new System.Version(1, 15))
				uri = site.site + site.indexPath +
					"api.php?action=query&prop=categories" +
					"&clprop=sortkey|hidden&cllimit=5000&format=xml&titles=" +
					HttpUtility.UrlEncode(title);
			else
				uri = site.site + site.indexPath + "index.php?title=" +
					HttpUtility.UrlEncode(title) + "&redirect=no";

			string xpathQuery;
            if (Bot.useBotQuery == true && site.botQuery == true && site.ver >= new System.Version(1, 15))
				xpathQuery = "//categories/cl/@title";
			else if (site.ver >= new System.Version(1,13))
				xpathQuery = "//ns:div[ @id='mw-normal-catlinks' or @id='mw-hidden-catlinks' ]" +
					"/ns:span/ns:a";
			else
				xpathQuery = "//ns:div[ @id='catlinks' ]/ns:p/ns:span/ns:a";

			string src = site.GetPageHTM(uri);
            if (Bot.useBotQuery != true || site.botQuery != true || site.ver < new System.Version(1, 15))
            {
				int startPos = src.IndexOf("<!-- start content -->");
				int endPos = src.IndexOf("<!-- end content -->");
				if (startPos != -1 && endPos != -1 && startPos < endPos)
					src = src.Remove(startPos, endPos - startPos);
				else {
					startPos = src.IndexOf("<!-- bodytext -->");
					endPos = src.IndexOf("<!-- /bodytext -->");
					if (startPos != -1 && endPos != -1 && startPos < endPos)
						src = src.Remove(startPos, endPos - startPos);
				}
			}

			XPathNodeIterator iterator = site.GetXMLIterator(src, xpathQuery);
			string[] matchStrings = new string[iterator.Count];
			iterator.MoveNext();
			for (int i = 0; i < iterator.Count; i++) {
				matchStrings[i] = (withNameSpacePrefix ? site.namespaces["14"] + ":" : "" ) + 
					site.RemoveNSPrefix(HttpUtility.HtmlDecode(iterator.Current.Value), 14);
					iterator.MoveNext();
			}

			return matchStrings;
		}

		

	

		/// <summary>Returns the array of strings, containing titles of templates, found on page.
		/// The "msgnw:" template modifier is not returned.
		/// Links to templates (like [[:Template:...]]) are not returned. Templates,
		/// mentioned inside &lt;nowiki&gt;&lt;/nowiki&gt; tags are also not returned. The
		/// "magic words" (see http://meta.wikimedia.org/wiki/Help:Magic_words) are recognized and
		/// not returned by this function as templates. When using this function on text of the
		/// template, parameters names and numbers (like {{{link}}} and {{{1}}}) are not returned
		/// by this function as templates too.</summary>
		/// <param name="withNameSpacePrefix">If true, function returns strings with
		/// namespace prefix like "Template:SomeTemplate", not just "SomeTemplate".</param>
		/// <returns>Returns the string[] array. Duplicates are possible.</returns>
        public string[] GetTemplates(bool withNameSpacePrefix)
        {
            string str = Site.noWikiMarkupRE.Replace(text, "");
            if (GetNamespace() == 10)
                str = Regex.Replace(str, @"\{\{\{.*?}}}", "");
            MatchCollection matches = Regex.Matches(str, @"(?s)\{\{(.+?)(}}|\|)");
            string[] matchStrings = new string[matches.Count];
            string match = "", matchLowerCase = "";
            int j = 0;
            for (int i = 0; i < matches.Count; i++)
            {
                match = matches[i].Groups[1].Value;
                matchLowerCase = match.ToLower();
                foreach (string mediaWikiVar in Site.mediaWikiVars)
                    if (matchLowerCase == mediaWikiVar)
                    {
                        match = "";
                        break;
                    }
                if (string.IsNullOrEmpty(match))
                    continue;
                foreach (string parserFunction in Site.parserFunctions)
                    if (matchLowerCase.StartsWith(parserFunction))
                    {
                        match = "";
                        break;
                    }
                if (string.IsNullOrEmpty(match))
                    continue;
                if (match.StartsWith("msgnw:") && match.Length > 6)
                    match = match.Substring(6);
                match = site.RemoveNSPrefix(match, 10).Trim();
                if (withNameSpacePrefix)
                    matchStrings[j++] = site.namespaces["10"] + ":" + match;
                else
                    matchStrings[j++] = match;
            }
            Array.Resize(ref matchStrings, j);
            return matchStrings;
        }

        /// <summary>Returns the array of strings, containing templates, found on page
        /// Everything inside braces is returned with all parameters
        /// untouched. Links to templates (like [[:Template:...]]) are not returned. Templates,
        /// mentioned inside &lt;nowiki&gt;&lt;/nowiki&gt; tags are also not returned. The
        /// "magic words" (see http://meta.wikimedia.org/wiki/Help:Magic_words) are recognized and
        /// not returned by this function as templates. When using this function on text of the
        /// template (on [[Template:NNN]] page), parameters names and numbers (like {{{link}}} 
        /// and {{{1}}}) are not returned by this function as templates too.</summary>
        /// <returns>Returns the string[] array.</returns>
        public string[] GetTemplatesWithParams()
        {
            Dictionary<int, int> templPos = new Dictionary<int, int>();
            StringCollection templates = new StringCollection();
            int startPos, endPos, len = 0;
            string str = text;
            while ((startPos = str.LastIndexOf("{{")) != -1)
            {
                endPos = str.IndexOf("}}", startPos);
                len = (endPos != -1) ? endPos - startPos + 2 : 2;
                if (len != 2)
                    templPos.Add(startPos, len);
                str = str.Remove(startPos, len);
                str = str.Insert(startPos, new String('_', len));
            }
            string[] templTitles = GetTemplates(false);
            Array.Reverse(templTitles);
            foreach (KeyValuePair<int, int> pos in templPos)
                templates.Add(text.Substring(pos.Key + 2, pos.Value - 4));
            for (int i = 0; i < templTitles.Length; i++)
                while (i < templates.Count &&
                    !templates[i].StartsWith(templTitles[i]) &&
                    !templates[i].StartsWith(site.namespaces["10"].ToString() + ":" +
                        templTitles[i], true, site.langCulture) &&
                    !templates[i].StartsWith(Site.wikiNSpaces["10"].ToString() + ":" +
                        templTitles[i], true, site.langCulture) &&
                    !templates[i].StartsWith("msgnw:" + templTitles[i]))
                    templates.RemoveAt(i);
            string[] arr = new string[templates.Count];
            templates.CopyTo(arr, 0);
            Array.Reverse(arr);
            return arr;
        }

      

        /// <summary>Removes all instances of a specified template from page text.</summary>
        /// <param name="templateTitle">Title of template to remove.</param>
        public void RemoveTemplate(string templateTitle)
        {
            if (string.IsNullOrEmpty(templateTitle))
                throw new ArgumentNullException("templateTitle");
            templateTitle = Regex.Escape(templateTitle);
            templateTitle = "(" + Char.ToUpper(templateTitle[0]) + "|" +
                Char.ToLower(templateTitle[0]) + ")" +
                (templateTitle.Length > 1 ? templateTitle.Substring(1) : "");
            text = Regex.Replace(text, @"(?s)\{\{\s*" + templateTitle +
                @"(.*?)}}\r?\n?", "");
        }

		/// <summary>Returns the array of strings, containing names of files,
		/// embedded in page, including images in galleries (inside "gallery" tag).
		/// But no links to images and files, like [[:Image:...]] or [[:File:...]] or
		/// [[Media:...]].</summary>
		/// <param name="withNameSpacePrefix">If true, function returns strings with
		/// namespace prefix like "Image:Example.jpg" or "File:Example.jpg",
		/// not just "Example.jpg".</param>
		/// <returns>Returns the string[] array. The array can be empty (of size 0). Strings in
		/// array may recur, indicating that file was mentioned several times on the page.</returns>
		public string[] GetImages(bool withNameSpacePrefix)
		{
			return GetImagesEx(withNameSpacePrefix, false);
		}

		/// <summary>Returns the array of strings, containing names of files,
		/// mentioned on a page.</summary>
		/// <param name="withNameSpacePrefix">If true, function returns strings with
		/// namespace prefix like "Image:Example.jpg" or "File:Example.jpg",
		/// not just "Example.jpg".</param>
		/// <param name="includeFileLinks">If true, function also returns links to images,
		/// like [[:Image:...]] or [[:File:...]] or [[Media:...]]</param>
		/// <returns>Returns the string[] array. The array can be empty (of size 0).Strings in
		/// array may recur, indicating that file was mentioned several times on the page.</returns>
		public string[] GetImagesEx(bool withNameSpacePrefix, bool includeFileLinks)
		{
			if (string.IsNullOrEmpty(text))
				throw new ArgumentNullException("text");
			string nsPrefixes = "File|Image|" + Regex.Escape(site.namespaces["6"].ToString());
			if (includeFileLinks) {
				nsPrefixes += "|" + Regex.Escape(site.namespaces["-2"].ToString()) + "|" +
					Regex.Escape(Site.wikiNSpaces["-2"].ToString());
			}
			MatchCollection matches;
			if (Regex.IsMatch(text, "(?is)<gallery>.*</gallery>"))
				matches = Regex.Matches(text, "(?i)" + (includeFileLinks ? "" : "(?<!:)") +
					"(" + nsPrefixes + ")(:)(.*?)(\\||\r|\n|]])");		// FIXME: inexact matches
			else
				matches = Regex.Matches(text, @"\[\[" + (includeFileLinks ? ":?" : "") +
					"(?i)((" + nsPrefixes + @"):(.+?))(\|(.+?))*?]]");
			string[] matchStrings = new string[matches.Count];
			for(int i = 0; i < matches.Count; i++) {
				if (withNameSpacePrefix == true)
					matchStrings[i] = site.namespaces["6"] + ":" + matches[i].Groups[3].Value;
				else
					matchStrings[i] = matches[i].Groups[3].Value;
			}
			return matchStrings;
		}

		/// <summary>Identifies the namespace of the page.</summary>
		/// <returns>Returns the integer key of the namespace.</returns>
		public int GetNamespace()
		{
			title = title.TrimStart(new char[] {':'});
			foreach (DictionaryEntry ns in site.namespaces) {
				if (title.StartsWith(ns.Value + ":", true, site.langCulture))
					return int.Parse(ns.Key.ToString());
			}
			foreach (DictionaryEntry ns in Site.wikiNSpaces) {
				if (title.StartsWith(ns.Value + ":", true, site.langCulture))
					return int.Parse(ns.Key.ToString());
			}
			return 0;
		}

		/// <summary>Sends page title to console.</summary>
		public void ShowTitle()
		{
			Console.Write("\n" + Bot.Msg("The title of this page is \"{0}\".") + "\n", title);
		}

		/// <summary>Sends page text to console.</summary>
		public void ShowText()
		{
			Console.Write("\n" + Bot.Msg("The text of \"{0}\" page:"), title);
			Console.Write("\n\n" + text + "\n\n");
		}

		/// <summary>Renames the page.</summary>
		/// <param name="newTitle">New title of that page.</param>
		/// <param name="reason">Reason for renaming.</param>
		public void RenameTo(string newTitle, string reason)
		{
			if (string.IsNullOrEmpty(newTitle))
				throw new ArgumentNullException("newTitle");
			if (string.IsNullOrEmpty(title))
				throw new WikiBotException(Bot.Msg("No title specified for page to rename."));
			//Page mp = new Page(site, "Special:Movepage/" + HttpUtility.UrlEncode(title));
			Page mp = new Page(site, "Special:Movepage/" + title);
			mp.GetEditSessionData();
			if (string.IsNullOrEmpty(mp.editSessionToken))
				throw new WikiBotException(string.Format(
					Bot.Msg("Unable to rename page \"{0}\" to \"{1}\"."), title, newTitle));
			if (Bot.askConfirm) {
				Console.Write("\n\n" +
					Bot.Msg("The page \"{0}\" is going to be renamed to \"{1}\".\n"),
					title, newTitle);
				if(!Bot.UserConfirms())
					return;
			}
			string postData = string.Format("wpNewTitle={0}&wpOldTitle={1}&wpEditToken={2}" +
				"&wpReason={3}", HttpUtility.UrlEncode(newTitle), HttpUtility.UrlEncode(title),
				HttpUtility.UrlEncode(mp.editSessionToken), HttpUtility.UrlEncode(reason));
			string respStr = site.PostDataAndGetResultHTM(site.indexPath +
				"index.php?title=Special:Movepage&action=submit", postData);
			if (Site.editSessionTokenRE2.IsMatch(respStr))
				throw new WikiBotException(string.Format(
					Bot.Msg("Failed to rename page \"{0}\" to \"{1}\"."), title, newTitle));
			Console.WriteLine(
				Bot.Msg("Page \"{0}\" was successfully renamed to \"{1}\"."), title, newTitle);
			title = newTitle;
		}

		/// <summary>Deletes the page. Sysop rights are needed to delete page.</summary>
		/// <param name="reason">Reason for deleting.</param>
		public void Delete(string reason)
		{
			if (string.IsNullOrEmpty(title))
				throw new WikiBotException(Bot.Msg("No title specified for page to delete."));
			string respStr1 = site.GetPageHTM(site.indexPath + "index.php?title=" +
				HttpUtility.UrlEncode(title) + "&action=delete");
			editSessionToken = Site.editSessionTokenRE1.Match(respStr1).Groups[1].ToString();
			if (string.IsNullOrEmpty(editSessionToken))
				editSessionToken = Site.editSessionTokenRE2.Match(respStr1).Groups[1].ToString();
			if (string.IsNullOrEmpty(editSessionToken))
				throw new WikiBotException(
					string.Format(Bot.Msg("Unable to delete page \"{0}\"."), title));
			if (Bot.askConfirm) {
				Console.Write("\n\n" + Bot.Msg("The page \"{0}\" is going to be deleted.\n"), title);
				if(!Bot.UserConfirms())
					return;
			}
			string postData = string.Format("wpReason={0}&wpEditToken={1}",
				HttpUtility.UrlEncode(reason), HttpUtility.UrlEncode(editSessionToken));
			string respStr2 = site.PostDataAndGetResultHTM(site.indexPath + "index.php?title=" +
				HttpUtility.UrlEncode(title) + "&action=delete", postData);
			if (Site.editSessionTokenRE2.IsMatch(respStr2))
				throw new WikiBotException(
					string.Format(Bot.Msg("Failed to delete page \"{0}\"."), title));
			Console.WriteLine(Bot.Msg("Page \"{0}\" was successfully deleted."), title);
			title = "";
		}
	}

    /// <summary>Class defines a set of wiki pages (constructed inside as List object).</summary>
    [ClassInterface(ClassInterfaceType.AutoDispatch)]
    [Serializable]
    public class PageList
    {
        /// <summary>Internal generic List, that contains collection of pages.</summary>
        public List<Page> pages = new List<Page>();
        /// <summary>Site, on which the pages are located.</summary>
        public Site site;

        /// <summary>This constructor creates PageList object with specified Site object and fills
        /// it with Page objects with specified titles. When constructed, new Page objects
        /// in PageList don't contain text. Use Load() method to get text from live wiki,
        /// or use LoadEx() to get both text and metadata via XML export interface.</summary>
        /// <param name="site">Site object, it must be constructed beforehand.</param>
        /// <param name="pageNames">Page titles as array of strings.</param>
        /// <returns>Returns the PageList object.</returns>
        public PageList(Site site, string[] pageNames)
        {
            this.site = site;
            foreach (string pageName in pageNames)
                pages.Add(new Page(site, pageName));
            CorrectNSPrefixes();
        }

        /// <summary>This constructor creates PageList object with specified Site object and fills
        /// it with Page objects with specified titles. When constructed, new Page objects
        /// in PageList don't contain text. Use Load() method to get text from live wiki,
        /// or use LoadEx() to get both text and metadata via XML export interface.</summary>
        /// <param name="site">Site object, it must be constructed beforehand.</param>
        /// <param name="pageNames">Page titles as StringCollection object.</param>
        /// <returns>Returns the PageList object.</returns>
        public PageList(Site site, StringCollection pageNames)
        {
            this.site = site;
            foreach (string pageName in pageNames)
                pages.Add(new Page(site, pageName));
            CorrectNSPrefixes();
        }

        /// <summary>This constructor creates empty PageList object with specified
        /// Site object.</summary>
        /// <param name="site">Site object, it must be constructed beforehand.</param>
        /// <returns>Returns the PageList object.</returns>
        public PageList(Site site)
        {
            this.site = site;
        }

        /// <summary>This constructor creates empty PageList object, Site object with default
        /// properties is created internally and logged in. Constructing new Site object
        /// is too slow, don't use this constructor needlessly.</summary>
        /// <returns>Returns the PageList object.</returns>
        public PageList()
        {
            site = new Site();
        }

        /// <summary>This index allows to call pageList[i] instead of pageList.pages[i].</summary>
        /// <param name="index">Zero-based index.</param>
        /// <returns>Returns the Page object.</returns>
        public Page this[int index]
        {
            get { return pages[index]; }
            set { pages[index] = value; }
        }

        /// <summary>This function allows to access individual pages in this PageList.
        /// But it's better to use simple pageList[i] index, when it is possible.</summary>
        /// <param name="index">Zero-based index.</param>
        /// <returns>Returns the Page object.</returns>

        /// <summary>This index allows to call pageList["title"]. Don't forget to use correct
        /// local namespace prefixes. Call CorrectNSPrefixes function to correct namespace
        /// prefixes in a whole PageList at once.</summary>
        /// <param name="index">Title of page to get.</param>
        /// <returns>Returns the Page object, or null if there is no page with the specified
        /// title in this PageList.</returns>
        public Page this[string index]
        {
            get
            {
                foreach (Page p in pages)
                    if (p.title == index)
                        return p;
                return null;
            }
            set
            {
                for (int i = 0; i < pages.Count; i++)
                    if (pages[i].title == index)
                        pages[i] = value;
            }
        }

        /// <summary>This standard internal function allows to directly use PageList objects
        /// in "foreach" statements.</summary>
        /// <returns>Returns IEnumerator object.</returns>
        public IEnumerator GetEnumerator()
        {
            return pages.GetEnumerator();
        }

        /// <summary>This function adds specified page to the end of this PageList.</summary>
        /// <param name="page">Page object to add.</param>
        public void Add(Page page)
        {
            pages.Add(page);
        }

        /// <summary>Inserts an element into this PageList at the specified index.</summary>
        /// <param name="page">Page object to insert.</param>
        /// <param name="index">Zero-based index.</param>
        public void Insert(Page page, int index)
        {
            pages.Insert(index, page);
        }

        /// <summary>This function returns true, if in this PageList there exists a page with
        /// the same title, as a page specified as a parameter.</summary>
        /// <param name="page">.</param>
        /// <returns>Returns bool value.</returns>
        public bool Contains(Page page)
        {
            page.CorrectNSPrefix();
            CorrectNSPrefixes();
            foreach (Page p in pages)
                if (p.title == page.title)
                    return true;
            return false;
        }

        /// <summary>This function returns true, if a page with specified title exists
        /// in this PageList.</summary>
        /// <param name="title">Title of page to check.</param>
        /// <returns>Returns bool value.</returns>
        public bool Contains(string title)
        {
            Page page = new Page(site, title);
            page.CorrectNSPrefix();
            CorrectNSPrefixes();
            foreach (Page p in pages)
                if (p.title == page.title)
                    return true;
            return false;
        }

        /// <summary>This function returns the number of pages in PageList.</summary>
        /// <returns>Number of pages as positive integer value.</returns>
        public int Count()
        {
            return pages.Count;
        }

        /// <summary>Removes page at specified index from PageList.</summary>
        /// <param name="index">Zero-based index.</param>
        public void RemoveAt(int index)
        {
            pages.RemoveAt(index);
        }

        /// <summary>Removes a page with specified title from this PageList.</summary>
        /// <param name="title">Title of page to remove.</param>
        public void Remove(string title)
        {
            for (int i = 0; i < Count(); i++)
                if (pages[i].title == title)
                    pages.RemoveAt(i);
        }

        /// <summary>Gets page titles for this PageList from "Special:Allpages" MediaWiki page.
        /// That means a list of pages in alphabetical order.</summary>
        /// <param name="firstPageTitle">Title of page to start enumerating from. The title
        /// must have no namespace prefix (like "Talk:"), just the page title itself. Or you can
        /// specify just a letter or two instead of full real title. Pass the empty string or null
        /// to start from the very beginning.</param>
        /// <param name="neededNSpace">Integer, presenting the key of namespace to get pages
        /// from. Only one key of one namespace can be specified (zero for default).</param>
        /// <param name="acceptRedirects">Set this to "false" to exclude redirects.</param>
        /// <param name="quantity">Maximum allowed quantity of pages in this PageList.</param>
        public void FillFromAllPages(string firstPageTitle, int neededNSpace, bool acceptRedirects,
            int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentOutOfRangeException("quantity",
                    Bot.Msg("Quantity must be positive."));
            if (Bot.useBotQuery == true && site.botQuery == true)
            {
                FillFromCustomBotQueryList("allpages", "apnamespace=" + neededNSpace +
                (acceptRedirects ? "" : "&apfilterredir=nonredirects") +
                (string.IsNullOrEmpty(firstPageTitle) ? "" : "&apfrom=" +
                HttpUtility.UrlEncode(firstPageTitle)), quantity);
                return;
            }
            Console.WriteLine(
                Bot.Msg("Getting {0} page titles from \"Special:Allpages\" MediaWiki page..."),
                quantity);
            int count = pages.Count;
            quantity += pages.Count;
            Regex linkToPageRE;
            if (acceptRedirects)
                linkToPageRE = new Regex("<td[^>]*>(?:<div class=\"allpagesredirect\">)?" +
                    "<a href=\"[^\"]*?\" title=\"([^\"]*?)\">");
            else
                linkToPageRE = new Regex("<td[^>]*><a href=\"[^\"]*?\" title=\"([^\"]*?)\">");
            MatchCollection matches;
            do
            {
                string res = site.site + site.indexPath +
                    "index.php?title=Special:Allpages&from=" +
                    HttpUtility.UrlEncode(
                        string.IsNullOrEmpty(firstPageTitle) ? "!" : firstPageTitle) +
                    "&namespace=" + neededNSpace.ToString();
                matches = linkToPageRE.Matches(site.GetPageHTM(res));
                if (matches.Count < 2)
                    break;
                for (int i = 1; i < matches.Count; i++)
                    pages.Add(new Page(site, HttpUtility.HtmlDecode(matches[i].Groups[1].Value)));
                firstPageTitle = site.RemoveNSPrefix(pages[pages.Count - 1].title, neededNSpace) +
                    "!";
            }
            while (pages.Count < quantity);
            if (pages.Count > quantity)
                pages.RemoveRange(quantity, pages.Count - quantity);
            Console.WriteLine(Bot.Msg("PageList filled with {0} page titles from " +
                "\"Special:Allpages\" MediaWiki page."), (pages.Count - count).ToString());
        }

        /// <summary>Gets page titles for this PageList from specified list, produced by
        /// bot query interface ("api.php" MediaWiki extension). The function
        /// does not clear the existing PageList, so new titles will be added.</summary>
        /// <param name="listType">Title of list, the following values are supported: 
        /// "allpages", "alllinks", "allusers", "backlinks", "categorymembers",
        /// "embeddedin", "imageusage", "logevents", "recentchanges", 
        /// "usercontribs", "watchlist", "exturlusage". Detailed documentation
        /// can be found at "http://en.wikipedia.org/w/api.php".</param>
        /// <param name="queryParams">Additional query parameters, specific to the
        /// required list, e.g. "cmtitle=Category:Physical%20sciences&amp;cmnamespace=0|2".
        /// Parameter values must be URL-encoded with HttpUtility.UrlEncode function
        /// before calling this function.</param>
        /// <param name="quantity">Maximum number of page titles to get.</param>
        /// <example><code>
        /// pageList.FillFromCustomBotQueryList("categorymembers",
        /// 	"cmcategory=Physical%20sciences&amp;cmnamespace=0|14",
        /// 	int.MaxValue);
        /// </code></example>
        public void FillFromCustomBotQueryList(string listType, string queryParams, int quantity)
        {
            if (!site.botQuery)
                throw new WikiBotException(
                    Bot.Msg("The \"api.php\" MediaWiki extension is not available."));
            if (string.IsNullOrEmpty(listType))
                throw new ArgumentNullException("listType");
            if (!Site.botQueryLists.Contains(listType))
                throw new WikiBotException(
                    string.Format(Bot.Msg("The list \"{0}\" is not supported."), listType));
            if (quantity <= 0)
                throw new ArgumentOutOfRangeException("quantity",
                    Bot.Msg("Quantity must be positive."));
            string prefix = Site.botQueryLists[listType].ToString();
            string continueAttrTag1 = prefix + "from";
            string continueAttrTag2 = prefix + "continue";
            string attrTag = (listType != "allusers") ? "title" : "name";
            string queryUri = site.indexPath + "api.php?action=query&list=" + listType +
                "&format=xml&" + prefix + "limit=" +
                ((quantity > 500) ? "500" : quantity.ToString());
            string src = "", next = "", queryFullUri = "";
            int count = pages.Count;
            if (quantity != int.MaxValue)
                quantity += pages.Count;
            do
            {
                queryFullUri = queryUri;
                if (next != "")
                    queryFullUri += "&" + prefix + "continue=" + HttpUtility.UrlEncode(next);
                src = site.PostDataAndGetResultHTM(queryFullUri, queryParams);
                using (XmlTextReader reader = new XmlTextReader(new StringReader(src)))
                {
                    next = "";
                    while (reader.Read())
                    {
                        if (reader.IsEmptyElement && reader[attrTag] != null)
                            pages.Add(new Page(site, HttpUtility.HtmlDecode(reader[attrTag])));
                        if (reader.IsEmptyElement && reader[continueAttrTag1] != null)
                            next = reader[continueAttrTag1];
                        if (reader.IsEmptyElement && reader[continueAttrTag2] != null)
                            next = reader[continueAttrTag2];
                    }
                }
            }
            while (next != "" && pages.Count < quantity);
            if (pages.Count > quantity)
                pages.RemoveRange(quantity, pages.Count - quantity);
            if (!string.IsNullOrEmpty(Environment.StackTrace) &&
                !Environment.StackTrace.Contains("FillAllFromCategoryEx"))
                Console.WriteLine(Bot.Msg("PageList filled with {0} page titles " +
                    "from \"{1}\" bot interface list."),
                    (pages.Count - count).ToString(), listType);
        }


      

        /// <summary>Loads text for pages in PageList from site via common web interface.
        /// Please, don't use this function when going to edit big amounts of pages on
        /// popular public wikis, as it compromises edit conflict detection. In that case,
        /// each page's text should be loaded individually right before its processing
        /// and saving.</summary>
        public void Load()
        {
            if (IsEmpty())
                throw new WikiBotException(Bot.Msg("The PageList is empty. Nothing to load."));
            foreach (Page page in pages)
                page.Load();
        }

        /// <summary>Loads texts and metadata for pages in PageList via XML export interface.
        /// Non-existent pages will be automatically removed from the PageList.
        /// Please, don't use this function when going to edit big amounts of pages on
        /// popular public wikis, as it compromises edit conflict detection. In that case,
        /// each page's text should be loaded individually right before its processing
        /// and saving.</summary>
        public void LoadEx()
        {
            if (IsEmpty())
                throw new WikiBotException(Bot.Msg("The PageList is empty. Nothing to load."));
            Console.WriteLine(Bot.Msg("Loading {0} pages..."), pages.Count);
            string res = site.site + site.indexPath +
                "index.php?title=Special:Export&action=submit";
            string postData = "curonly=True&pages=";
            foreach (Page page in pages)
                postData += HttpUtility.UrlEncode(page.title) + "\r\n";
            XmlReader reader = XmlReader.Create(
                new StringReader(site.PostDataAndGetResultHTM(res, postData)));
            Clear();
            while (reader.ReadToFollowing("page"))
            {
                Page p = new Page(site, "");
                p.ParsePageXML(reader.ReadOuterXml());
                pages.Add(p);
            }
            reader.Close();
        }

        /// <summary>Loads text and metadata for pages in PageList via XML export interface.
        /// The function uses XPathNavigator and is less efficient than LoadEx().</summary>
        public void LoadEx2()
        {
            if (IsEmpty())
                throw new WikiBotException("The PageList is empty. Nothing to load.");
            Console.WriteLine(Bot.Msg("Loading {0} pages..."), pages.Count);
            string res = site.site + site.indexPath +
                "index.php?title=Special:Export&action=submit";
            string postData = "curonly=True&pages=";
            foreach (Page page in pages)
                postData += HttpUtility.UrlEncode(page.title + "\r\n");
            string src = site.PostDataAndGetResultHTM(res, postData);
            src = Bot.RemoveXMLRootAttributes(src);
            StringReader strReader = new StringReader(src);
            XPathDocument doc = new XPathDocument(strReader);
            strReader.Close();
            XPathNavigator nav = doc.CreateNavigator();
            foreach (Page page in pages)
            {
                if (page.title.Contains("'"))
                {
                    page.LoadEx();
                    continue;
                }
                string query = "//page[title='" + page.title + "']/";
                try
                {
                    page.text =
                        nav.SelectSingleNode(query + "revision/text").InnerXml;
                }
                catch (System.NullReferenceException)
                {
                    continue;
                }
                page.text = HttpUtility.HtmlDecode(page.text);
                page.pageID = nav.SelectSingleNode(query + "id").InnerXml;
                try
                {
                    page.lastUser = nav.SelectSingleNode(query +
                        "revision/contributor/username").InnerXml;
                    page.lastUserID = nav.SelectSingleNode(query +
                        "revision/contributor/id").InnerXml;
                }
                catch (System.NullReferenceException)
                {
                    page.lastUser = nav.SelectSingleNode(query +
                        "revision/contributor/ip").InnerXml;
                }
                page.lastUser = HttpUtility.HtmlDecode(page.lastUser);
                page.lastRevisionID = nav.SelectSingleNode(query + "revision/id").InnerXml;
                page.lastMinorEdit = (nav.SelectSingleNode(query +
                    "revision/minor") == null) ? false : true;
                try
                {
                    page.comment = nav.SelectSingleNode(query + "revision/comment").InnerXml;
                    page.comment = HttpUtility.HtmlDecode(page.comment);
                }
                catch (System.NullReferenceException) { ;}
                page.timestamp = nav.SelectSingleNode(query + "revision/timestamp").ValueAsDateTime;
            }
            Console.WriteLine(Bot.Msg("Pages download completed."));
        }

        /// <summary>Loads text and metadata for pages in PageList via XML export interface.
        /// The function loads pages one by one, it is slightly less efficient
        /// than LoadEx().</summary>
        public void LoadEx3()
        {
            if (IsEmpty())
                throw new WikiBotException("The PageList is empty. Nothing to load.");
            foreach (Page p in pages)
                p.LoadEx();
        }



        /// <summary>Gets page titles and page texts from all ".txt" files in the specified
        /// directory (folder). Each file becomes a page. Page titles are constructed from
        /// file names. Page text is read from file contents. If any Unicode numeric codes
        /// (also known as numeric character references or NCRs) of the forbidden characters
        /// (forbidden in filenames) are recognized in filenames, those codes are converted
        /// to characters (e.g. "&#x7c;" is converted to "|").</summary>
        /// <param name="dirPath">The path and name of a directory (folder)
        /// to load files from.</param>
        public void FillAndLoadFromFiles(string dirPath)
        {
            foreach (string fileName in Directory.GetFiles(dirPath, "*.txt"))
            {
                Page p = new Page(site, Path.GetFileNameWithoutExtension(fileName));
                p.title = p.title.Replace("&#x22;", "\"");
                p.title = p.title.Replace("&#x3c;", "<");
                p.title = p.title.Replace("&#x3e;", ">");
                p.title = p.title.Replace("&#x3f;", "?");
                p.title = p.title.Replace("&#x3a;", ":");
                p.title = p.title.Replace("&#x5c;", "\\");
                p.title = p.title.Replace("&#x2f;", "/");
                p.title = p.title.Replace("&#x2a;", "*");
                p.title = p.title.Replace("&#x7c;", "|");
                p.LoadFromFile(fileName);
                pages.Add(p);
            }
        }

      
 

        /// <summary>Undoes the last edit of every page in this PageList, so every page text reverts
        /// to previous contents. The function doesn't affect other operations
        /// like renaming.</summary>
        /// <param name="comment">Your edit comment.</param>
        /// <param name="isMinorEdit">Minor edit mark (true = minor edit).</param>
        /*public void Revert(string comment, bool isMinorEdit)
        {
            foreach (Page page in pages)
                page.Revert(comment, isMinorEdit);
        }*/

        /// <summary>Saves titles of all pages in PageList to the specified file. Each title
        /// on a new line. If the target file already exists, it is overwritten.</summary>
        /// <param name="filePathName">The path to and name of the target file as string.</param>
        public void SaveTitlesToFile(string filePathName)
        {
            SaveTitlesToFile(filePathName, false);
        }

        /// <summary>Saves titles of all pages in PageList to the specified file. Each title
        /// on a separate line. If the target file already exists, it is overwritten.</summary>
        /// <param name="filePathName">The path to and name of the target file as string.</param>
        /// <param name="useSquareBrackets">If true, each page title is enclosed
        /// in square brackets.</param>
        public void SaveTitlesToFile(string filePathName, bool useSquareBrackets)
        {
            StringBuilder titles = new StringBuilder();
            foreach (Page page in pages)
                titles.Append(useSquareBrackets ?
                    "[[" + page.title + "]]\r\n" : page.title + "\r\n");
            File.WriteAllText(filePathName, titles.ToString().Trim(), Encoding.UTF8);
            Console.WriteLine(Bot.Msg("Titles in PageList saved to \"{0}\" file."), filePathName);
        }


        /// <summary>Loads the contents of all pages in pageList from live site via XML export
        /// and saves the retrieved XML content to the specified file. The functions just dumps
        /// data, it does not load pages in PageList itself, call LoadEx() or
        /// FillAndLoadFromXMLDump() to do that. Note, that on some sites, MediaWiki messages
        /// from standard namespace 8 are not available for export.</summary>
        /// <param name="filePathName">The path to and name of the target file as string.</param>
        public void SaveXMLDumpToFile(string filePathName)
        {
            Console.WriteLine(Bot.Msg("Loading {0} pages for XML dump..."), this.pages.Count);
            string res = site.site + site.indexPath +
                "index.php?title=Special:Export&action=submit";
            string postData = "catname=&curonly=true&action=submit&pages=";
            foreach (Page page in pages)
                postData += HttpUtility.UrlEncode(page.title + "\r\n");
            string rawXML = site.PostDataAndGetResultHTM(res, postData);
            rawXML = Bot.RemoveXMLRootAttributes(rawXML).Replace("\n", "\r\n");
            if (File.Exists(filePathName))
                File.Delete(filePathName);
            FileStream fs = File.Create(filePathName);
            byte[] XMLBytes = new System.Text.UTF8Encoding(true).GetBytes(rawXML);
            fs.Write(XMLBytes, 0, XMLBytes.Length);
            fs.Close();
            Console.WriteLine(
                Bot.Msg("XML dump successfully saved in \"{0}\" file."), filePathName);
        }

        /// <summary>Removes all empty pages from PageList. But firstly don't forget to load
        /// the pages from site using pageList.LoadEx().</summary>
        /*public void RemoveEmpty()
        {
            for (int i=pages.Count-1; i >= 0; i--)
                if (pages[i].IsEmpty())
                    pages.RemoveAt(i);
        }*/

        /// <summary>Removes all recurring pages from PageList. Only one page with some title will
        /// remain in PageList. This makes all page elements in PageList unique.</summary>
        public void RemoveRecurring()
        {
            for (int i = pages.Count - 1; i >= 0; i--)
                for (int j = i - 1; j >= 0; j--)
                    if (pages[i].title == pages[j].title)
                    {
                        pages.RemoveAt(i);
                        break;
                    }
        }

        /// <summary>Removes all redirecting pages from PageList. But firstly don't forget to load
        /// the pages from site using pageList.LoadEx().</summary>
        /*	public void RemoveRedirects()
            {
                for (int i=pages.Count-1; i >= 0; i--)
                    if (pages[i].IsRedirect())
                        pages.RemoveAt(i);
            }*/

        /// <summary>For all redirecting pages in this PageList, this function loads the titles and
        /// texts of redirected-to pages.</summary>
        /*public void ResolveRedirects()
        {
            foreach (Page page in pages) {
                if (page.IsRedirect() == false)
                    continue;
                page.title = page.RedirectsTo();
                page.Load();
            }
        }*/

        /// <summary>Removes all disambiguation pages from PageList. But firstly don't
        /// forget to load the pages from site using pageList.LoadEx().</summary>
        /*public void RemoveDisambigs()
        {
            for (int i=pages.Count-1; i >= 0; i--)
                if (pages[i].IsDisambig())
                    pages.RemoveAt(i);
        }*/


        /// <summary>Removes all pages from PageList.</summary>
        /*public void RemoveAll()
        {
            pages.Clear();
        }*/

        /// <summary>Removes all pages from PageList.</summary>
        public void Clear()
        {
            pages.Clear();
        }

        /// <summary>Function changes default English namespace prefixes to correct local prefixes
        /// (e.g. for German wiki-sites it changes "Category:..." to "Kategorie:...").</summary>
        public void CorrectNSPrefixes()
        {
            foreach (Page p in pages)
                p.CorrectNSPrefix();
        }

        /// <summary>Shows if there are any Page objects in this PageList.</summary>
        /// <returns>Returns bool value.</returns>
        public bool IsEmpty()
        {
            return (pages.Count == 0) ? true : false;
        }

        /// <summary>Sends titles of all contained pages to console.</summary>
        /*public void ShowTitles()
        {
            Console.WriteLine("\n" + Bot.Msg("Pages in this PageList:"));
            foreach (Page p in pages)
                Console.WriteLine(p.title);
            Console.WriteLine("\n");
        }
        */
        /// <summary>Sends texts of all contained pages to console.</summary>
        /*public void ShowTexts()
        {
            Console.WriteLine("\n" + Bot.Msg("Texts of all pages in this PageList:"));
            Console.WriteLine("--------------------------------------------------");
            foreach (Page p in pages) {
                p.ShowText();
                Console.WriteLine("--------------------------------------------------");
            }
            Console.WriteLine("\n");
        }*/
    }

    /// <summary>Class establishes custom application exceptions.</summary>
    [ClassInterface(ClassInterfaceType.AutoDispatch)]
    [Serializable]
    public class WikiBotException : System.Exception
    {
        /// <summary>Just overriding default constructor.</summary>
        /// <returns>Returns Exception object.</returns>
        public WikiBotException() { }
        /// <summary>Just overriding constructor.</summary>
        /// <returns>Returns Exception object.</returns>
        public WikiBotException(string message)
            : base(message) { Console.Beep(); /*Console.ForegroundColor = ConsoleColor.Red;*/ }
        /// <summary>Just overriding constructor.</summary>
        /// <returns>Returns Exception object.</returns>
        public WikiBotException(string message, System.Exception inner)
            : base(message, inner) { }
        /// <summary>Just overriding constructor.</summary>
        /// <returns>Returns Exception object.</returns>
        protected WikiBotException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
        /// <summary>Destructor is invoked automatically when exception object becomes
        /// inaccessible.</summary>
        ~WikiBotException() { }
    }

	/// <summary>Class defines custom XML URL resolver, that has a caching capability. See
	/// http://www.w3.org/blog/systeam/2008/02/08/w3c_s_excessive_dtd_traffic for details.</summary>
	//[PermissionSetAttribute(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	class XmlUrlResolverWithCache : XmlUrlResolver
	{
		/// <summary>List of cached files absolute URIs.</summary>
		static string[] cachedFilesURIs = {
			"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd",
			"http://www.w3.org/TR/xhtml1/DTD/xhtml-lat1.ent",
			"http://www.w3.org/TR/xhtml1/DTD/xhtml-symbol.ent",
			"http://www.w3.org/TR/xhtml1/DTD/xhtml-special.ent"
			//http://www.mediawiki.org/xml/export-0.4/ http://www.mediawiki.org/xml/export-0.4.xsd
		};
		/// <summary>List of cached files names.</summary>
		static string[] cachedFiles = {
			"xhtml1-transitional.dtd",
			"xhtml-lat1.ent",
			"xhtml-symbol.ent",
			"xhtml-special.ent"
		};
		/// <summary>Local cache directory.</summary>
		static string cacheDir = "Cache" + Path.DirectorySeparatorChar;

		/// <summary>Overriding GetEntity() function to implement local cache.</summary>
		/// <param name="absoluteUri">Absolute URI of requested entity.</param>
		/// <param name="role">User's role for accessing specified URI.</param>
		/// <param name="ofObjectToReturn">Type of object to return.</param>
		/// <returns>Returns object or requested type.</returns>
		public override object GetEntity(Uri absoluteUri, string role, Type ofObjectToReturn)
		{
			for (int i = 0; i < XmlUrlResolverWithCache.cachedFilesURIs.Length; i++)
				if (absoluteUri.OriginalString == XmlUrlResolverWithCache.cachedFilesURIs[i])
					return new FileStream(XmlUrlResolverWithCache.cacheDir +
						XmlUrlResolverWithCache.cachedFiles[i],
						FileMode.Open, FileAccess.Read, FileShare.Read);
			return base.GetEntity(absoluteUri, role, ofObjectToReturn);
		}
	}

	/// <summary>Class defines a Bot instance, some configuration settings
	/// and some auxiliary functions.</summary>
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	public class Bot
	{
		/// <summary>Title and description of web agent.</summary>
		public static readonly string botVer = "DotNetWikiBot";
		/// <summary>Version of DotNetWikiBot Framework.</summary>
		public static readonly System.Version version = new System.Version("2.97");
		/// <summary>Desired bot's messages language (ISO 639-1 language code).
		/// If not set explicitly, the language will be detected automatically.</summary>
		/// <example><code>Bot.botMessagesLang = "fr";</code></example>
		public static string botMessagesLang = null;
		/// <summary>Default edit comment. You can set it to what you like.</summary>
		/// <example><code>Bot.editComment = "My default edit comment";</code></example>
		public static string editComment = "Automatic page editing";
		/// <summary>If set to true, all the bot's edits are marked as minor by default.</summary>
		public static bool isMinorEdit = true;
		/// <summary>If true, the bot uses "MediaWiki API" extension
		/// (special MediaWiki bot interface, "api.php"), if it is available.
		/// If false, the bot uses common user interface. True by default.
		/// Set it to false manually, if some problem with bot interface arises on site.</summary>
		/// <example><code>Bot.useBotQuery = false;</code></example>
		public static bool useBotQuery = true;
		/// <summary>Number of times to retry bot action in case of temporary connection failure or
		/// some other common net problems.</summary>
		public static int retryTimes = 3;
		/// <summary>If true, the bot asks user to confirm next Save, RenameTo or Delete operation.
		/// False by default. Set it to true manually, when necessary.</summary>
		/// <example><code>Bot.askConfirm = true;</code></example>
		public static bool askConfirm = false;
		/// <summary>If true, bot only reports errors and warnings. Call EnableSilenceMode
		/// function to enable that mode, don't change this variable's value manually.</summary>
		public static bool silenceMode = false;
		/// <summary>If set to some file name (e.g. "DotNetWikiBot_Report.txt"), the bot
		/// writes all output to that file instead of a console. If no path was specified,
		/// the bot creates that file in it's current directory. File is encoded in UTF-8.
		/// Call EnableLogging function to enable log writing, don't change this variable's
		/// value manually.</summary>
		public static string logFile = null;

		/// <summary>Array, containing localized DotNetWikiBot interface messages.</summary>
		public static SortedDictionary<string, string> messages =
			new SortedDictionary<string, string>();
		/// <summary>Internal web client, that is used to access sites.</summary>
		public static WebClient wc = new WebClient();
		/// <summary>Content type for HTTP header of web client.</summary>
		public static readonly string webContentType = "application/x-www-form-urlencoded";
		/// <summary>If true, assembly is running on Mono framework. If false,
		/// it is running on original Microsoft .NET Framework. This variable is set
		/// automatically, just get it's value, don't change it.</summary>
		public static readonly bool isRunningOnMono = (Type.GetType("Mono.Runtime") != null);
		/// <summary>Initial state of HttpWebRequestElement.UseUnsafeHeaderParsing boolean
		/// configuration setting. 0 means true, 1 means false, 2 means unchanged.</summary>
		public static int unsafeHttpHeaderParsingUsed = 2;

		/// <summary>This constructor is used to generate Bot object.</summary>
		/// <returns>Returns Bot object.</returns>
		static Bot()
		{
			if (botMessagesLang == null)
				botMessagesLang = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
			if (botMessagesLang != "en")
				if (!LoadLocalizedMessages(botMessagesLang))
					botMessagesLang = "en";
			botVer += "/" + version + " (" + Environment.OSVersion.VersionString + "; " +
				".NET CLR " + Environment.Version.ToString() + ")";
			ServicePointManager.Expect100Continue = false;
		}

		/// <summary>The destructor is used to uninitialize Bot objects.</summary>
		~Bot()
		{
			//if (unsafeHttpHeaderParsingUsed != 2)
				//SwitchUnsafeHttpHeaderParsing(unsafeHttpHeaderParsingUsed == 1 ? true : false);
		}

		/// <summary>Call this function to make bot write all output to the specified file
		/// instead of a console. If only error logging is desirable, first call this
		/// function, and after that call EnableSilenceMode function.</summary>
		/// <param name="logFileName">Path and name of a file to write output to.
		/// If no path was specified, the bot creates that file in it's current directory.
		/// File is encoded in UTF-8.</param>
		public static void EnableLogging(string logFileName)
		{
			logFile = logFileName;
			StreamWriter log = File.AppendText(logFile);
			log.AutoFlush = true;
			Console.SetError(log);
			if (!silenceMode)
				Console.SetOut(log);
		}

		/// <summary>Call this function to make bot report only errors and warnings,
		/// no other messages will be displayed or logged.</summary>
		public static void EnableSilenceMode()
		{
			silenceMode = true;
			Console.SetOut(new StringWriter());
		}

		/// <summary>Function loads localized bot interface messages from 
		/// "DotNetWikiBot.i18n.xml" file. Function is called in Bot class constructor, 
		/// but can also be called manually to change interface language at runtime.</summary>
		/// <param name="language">Desired language's ISO 639-1 code.</param>
		/// <returns>Returns false, if messages for specified language were not found.
		/// Returns true on success.</returns>
		public static bool LoadLocalizedMessages(string language)
		{
			if (!File.Exists("DotNetWikiBot.i18n.xml")) {
				Console.Error.WriteLine("Localization file \"DotNetWikiBot.i18n.xml\" is missimg.");
				return false;
			}
			using (XmlReader reader = XmlReader.Create("DotNetWikiBot.i18n.xml")) {
				if (!reader.ReadToFollowing(language)) {
					Console.Error.WriteLine("\nLocalized messages not found for language \"{0}\"." +
						"\nYou can help DotNetWikiBot project by translating the messages in\n" +
						"\"DotNetWikiBot.i18n.xml\" file and sending it to developers for " +
						"distribution.\n", language);
					return false;
				}
				if (!reader.ReadToDescendant("msg"))
					return false;
				else {
					if (messages.Count > 0)
						messages.Clear();
					messages[reader["id"]] = reader.ReadString();
				}
				while (reader.ReadToNextSibling("msg"))
					messages[reader["id"]] = reader.ReadString();
			}
			return true;
		}

		/// <summary>The function gets localized (translated) form of the specified bot
		/// interface message.</summary>
		/// <param name="message">Message itself, placeholders for substituted parameters are
		/// denoted in curly brackets like {0}, {1}, {2} and so on.</param>
		/// <returns>Returns localized form of the specified bot interface message,
		/// or English form if localized form was not found.</returns>
		public static string Msg(string message)
		{
			if (botMessagesLang == "en")
				return message;
			try {
				return messages[message];
			}
			catch (KeyNotFoundException) {
				return message;
			}
		}

		/// <summary>This function asks user to confirm next action. The message
		/// "Would you like to proceed (y/n/a)? " is displayed and user response is
		/// evaluated. Make sure to set "askConfirm" variable to "true" before
		/// calling this function.</summary>
		/// <returns>Returns true, if user has confirmed the action.</returns>
		/// <example><code>
		/// if (Bot.askConfirm) {
		///     Console.Write("Some action on live wiki is going to occur.\n\n");
		///     if(!Bot.UserConfirms())
		///         return;
		/// }
		/// </code></example>
		public static bool UserConfirms()
		{
			if (!askConfirm)
				return true;
			ConsoleKeyInfo k;
			Console.Write(Bot.Msg("Would you like to proceed (y/n/a)?") + " ");
			k = Console.ReadKey();
			Console.Write("\n");
			if (k.KeyChar == 'y')
				return true;
			else if (k.KeyChar == 'a') {
				askConfirm = false;
				return true;
			}
			else
				return false;
		}

		/// <summary>This auxiliary function counts the occurrences of specified string
		/// in specified text. This count is often needed, but strangely there is no
		/// such function in .NET Framework's String class.</summary>
		/// <param name="text">String to look in.</param>
		/// <param name="str">String to look for.</param>
		/// <param name="ignoreCase">Pass "true" if you need case-insensitive search.
		/// But remember that case-sensitive search is faster.</param>
		/// <returns>Returns the number of found occurrences.</returns>
		/// <example><code>int m = CountMatches("Bot Bot bot", "Bot", false); // =2</code></example>
		public static int CountMatches(string text, string str, bool ignoreCase)
		{
			if (string.IsNullOrEmpty(text))
				throw new ArgumentNullException("text");
			if (string.IsNullOrEmpty(str))
				throw new ArgumentNullException("str");
			int matches = 0;
			int position = 0;
			StringComparison rule = ignoreCase
				? StringComparison.OrdinalIgnoreCase
				: StringComparison.Ordinal;
			while ((position = text.IndexOf(str, position, rule)) != -1) {
				matches++;
				position++;
			}
			return matches;
		}

		/// <summary>This auxiliary function returns the zero-based indexes of all occurrences
		/// of specified string in specified text.</summary>
		/// <param name="text">String to look in.</param>
		/// <param name="str">String to look for.</param>
		/// <param name="ignoreCase">Pass "true" if you need case-insensitive search.
		/// But remember that case-sensitive search is faster.</param>
		/// <returns>Returns the List of positions (zero-based integer indexes) of all found
		/// instances, or empty List if nothing was found.</returns>
		public static List<int> GetMatchesPositions(string text, string str, bool ignoreCase)
		{
			if (string.IsNullOrEmpty(text))
				throw new ArgumentNullException("text");
			if (string.IsNullOrEmpty(str))
				throw new ArgumentNullException("str");
			List<int> positions = new List<int>();
			StringComparison rule = ignoreCase
				? StringComparison.OrdinalIgnoreCase
				: StringComparison.Ordinal;
			int position = 0;
			while ((position = text.IndexOf(str, position, rule)) != -1) {
				positions.Add(position);
				position++;
			}
			return positions;
		}

		/// <summary>This auxiliary function makes the first letter in specified string upper-case.
		/// This is often needed, but strangely there is no such function in .NET Framework's
		/// String class.</summary>
		/// <param name="str">String to capitalize.</param>
		/// <returns>Returns capitalized string.</returns>
		public static string Capitalize(string str)
		{
			return char.ToUpper(str[0]) + str.Substring(1);
		}

		/// <summary>This auxiliary function makes the first letter in specified string lower-case.
		/// This is often needed, but strangely there is no such function in .NET Framework's
		/// String class.</summary>
		/// <param name="str">String to uncapitalize.</param>
		/// <returns>Returns uncapitalized string.</returns>
		public static string Uncapitalize(string str)
		{
			return char.ToLower(str[0]) + str.Substring(1);
		}

		/// <summary>Suspends execution for specified number of seconds.</summary>
		/// <param name="seconds">Number of seconds to wait.</param>
		public static void Wait(int seconds)
		{
			Thread.Sleep(seconds * 1000);
		}

        /// <summary>This internal function switches unsafe HTTP headers parsing on or off.
        /// This is needed to ignore unimportant HTTP protocol violations,
        /// committed by misconfigured web servers.</summary>
        public static void SwitchUnsafeHttpHeaderParsing(bool enabled)
        {
            System.Configuration.Configuration config =
                System.Configuration.ConfigurationManager.OpenExeConfiguration(
                    System.Configuration.ConfigurationUserLevel.None);
            System.Net.Configuration.SettingsSection section =
                (System.Net.Configuration.SettingsSection)config.GetSection("system.net/settings");
            if (unsafeHttpHeaderParsingUsed == 2)
                unsafeHttpHeaderParsingUsed = section.HttpWebRequest.UseUnsafeHeaderParsing ? 1 : 0;
            section.HttpWebRequest.UseUnsafeHeaderParsing = enabled;
            config.Save();
            System.Configuration.ConfigurationManager.RefreshSection("system.net/settings");
        }

		/// <summary>This internal function removes all attributes from root XML/XHTML element
		/// (XML namespace declarations, schema links, etc.) for easy processing.</summary>
		/// <returns>Returns document without unnecessary declarations.</returns>
		public static string RemoveXMLRootAttributes(string xmlSource)
		{
			int startPos = ((xmlSource.StartsWith("<!") || xmlSource.StartsWith("<?"))
				&& xmlSource.IndexOf('>') != -1) ? xmlSource.IndexOf('>') + 1 : 0;
			int firstSpacePos = xmlSource.IndexOf(' ', startPos);
			int firstCloseTagPos = xmlSource.IndexOf('>', startPos);
			if (firstSpacePos != -1 && firstCloseTagPos != -1 && firstSpacePos < firstCloseTagPos)
				return xmlSource.Remove(firstSpacePos, firstCloseTagPos - firstSpacePos);
			return xmlSource;
		}

		/// <summary>This internal function initializes web client to get resources
		/// from web.</summary>
		public static void InitWebClient()
		{
			if (!Bot.isRunningOnMono)
				wc.UseDefaultCredentials = true;
			wc.Encoding = Encoding.UTF8;
			wc.Headers.Add("Content-Type", webContentType);
			wc.Headers.Add("User-agent", botVer);
		}

		/// <summary>This internal wrapper function gets web resource in a fault-tolerant manner.
		/// It should be used only in simple cases, because it sends no cookies, it doesn't support
		/// traffic compression and lacks other special features.</summary>
		/// <param name="address">Web resource address.</param>
		/// <param name="postData">Data to post with web request, can be "" or null.</param>
		/// <returns>Returns web resource as text.</returns>
		public static string GetWebResource(Uri address, string postData)
		{
			string webResourceText = null;
			for (int errorCounter = 0; true; errorCounter++) {
				try {
					Bot.InitWebClient();
					if (string.IsNullOrEmpty(postData))
						webResourceText = Bot.wc.DownloadString(address);
					else
						webResourceText = Bot.wc.UploadString(address, postData);
					break;
				}
				catch (WebException e) {
					if (errorCounter > retryTimes)
						throw;
					string message = e.Message;
					if (Regex.IsMatch(message, ": \\(50[02349]\\) ")) {		// Remote problem
						Console.Error.WriteLine(message + " " + Bot.Msg("Retrying in 60 seconds."));
						Thread.Sleep(60000);
					}
					else if (message.Contains("Section=ResponseStatusLine")) {	// Squid problem
						SwitchUnsafeHttpHeaderParsing(true);
						Console.Error.WriteLine(message + " " + Bot.Msg("Retrying in 60 seconds."));
						Thread.Sleep(60000);
					}
					else
						throw;
				}
			}
			return webResourceText;
		}
	}
}