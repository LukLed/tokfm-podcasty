using System.Collections.Generic;
using System.Linq;
using System.Text;
using TokFM.Model;

namespace TokFM.Business
{
	interface IPodcastService
	{
		/// <summary>
		/// Returns podcast list from tok.fm page (http://www.tok.fm/TOKFM/0,94037.html)
		/// </summary>
		/// <param name="page">number of page</param>
		/// <returns></returns>
		IEnumerable<Podcast> GetLatestPodcasts(int page);
	}
}
