// Write your own bot scripts and functions in this file.
// Run "Compile & Run.bat" file - it will compile this file as executable and launch it.
// Compiled XML autodocumentation for DotNetWikiBot namespace is available as "Documentation.chm".

using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Xml;
using DotNetWikiBot;

class MyBot : Bot
{
	public void MyFunction1()
	{
		// Write your own function here
	}

	/// The entry point function. Start coding here.
	public static void Main()
	{
		// Here are some code examples:

		Site site = new Site("http://en.wikipedia.org", "YourBotLogin", "YourBotPassword");
		//Site site = new Site("http://mywikisite.com", "YourBotLogin", "YourBotPassword");
		//Site site = new Site("http://sourceforge.net/apps/mediawiki/YourProjectName/",
			//"YourSourceForgeLogin", "YourSourceForgePassword");

		/*
		Page p = new Page(site, "Wikipedia:Sandbox");
		p.LoadEx();
		if (p.Exists())
			Console.WriteLine(p.text);
		p.SaveToFile("dir\\file.txt");
		p.LoadFromFile("dir\\file.txt");
		p.ResolveRedirect();
		site.ShowNamespaces();
		Console.WriteLine(p.GetNamespace());
		p.text = "new text";
		Bot.editComment = "saving test";
		Bot.isMinorEdit = true;
		p.Save();
		/**/

		/*
		string[] arr = {"Art", "Poetry", "Cinematography", "Camera", "Image"};
		PageList pl = new PageList(site, arr);
		pl.LoadEx();
		pl.FillFromAllPages("Sw", 0, true, 100);
		pl.FillFromFile("dir\\text.txt");
		pl.FillFromCategory("Category:Cinematography");
		pl.FillFromAllPageLinks("Cinematography");
		pl.FillFromLinksToPage("Cinematography");
		pl.RemoveEmpty();
		pl.RemoveDisambigs();
		pl.ResolveRedirects();
		Console.WriteLine(pl[2].text);
		pl[1].text = "#REDIRECT [[Some Page]]";
		pl.FilterNamespaces(new int[] {0,3});
		pl.RemoveNamespaces(new int[] {2,4});
		pl.SaveTitlesToFile("dir\\text.txt");
		pl.Clear();
		Bot.editComment = "my edit comment";
		isMinorEdit = true;
		pl.Save();
		/**/
	}
}