using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using HtmlAgilityPack;
using TokFM.Model;

namespace TokFM.Business
{
	class PodcastService : IPodcastService
	{
		public IList<Podcast> GetLatestPodcasts(int pageNumber)
		{
			var hw = new HtmlWeb();
			hw.OverrideEncoding = Encoding.GetEncoding("ISO-8859-2");
			var doc = hw.Load("http://www.tok.fm/TOKFM/0,94037.html?str=" + pageNumber.ToString(CultureInfo.InvariantCulture));
			doc.OptionOutputAsXml = true;
			doc.OptionCheckSyntax = true;
			doc.OptionFixNestedTags = true;
			var sb = new StringBuilder();
			var stringWriter = new StringWriter(sb);

			doc.Save(stringWriter);
			var page = sb.ToString();
			var stringReader = new StringReader(page);
			doc.Load(stringReader);
			var result = new List<Podcast>();
			foreach(HtmlNode link in doc.DocumentNode.SelectNodes("//a[@class='tokfm_play']"))
			{
				var imgNode = link.SelectSingleNode("img");
				var imageURL = String.Empty;
				if (imgNode != null)
					imageURL = imgNode.Attributes["src"].Value;
				result.Add(new Podcast { Href = link.Attributes["href"].Value, Title = link.Attributes["title"].Value, ImageURL = imageURL });	  
			}

			return result;
		}
	}
}