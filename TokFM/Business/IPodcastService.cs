using System.Collections.Generic;
using System.Linq;
using System.Text;
using TokFM.Model;

namespace TokFM.Business
{
	interface IPodcastService
	{
		IList<Podcast> GetLatestPodcasts(int page);
	}
}
