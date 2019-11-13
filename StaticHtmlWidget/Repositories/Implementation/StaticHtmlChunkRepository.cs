using System;
using System.Linq;
using CMS.DocumentEngine.Types.Custom;
using CMS.SiteProvider;

namespace DancingGoat.Repositories.Implementation
{
    public class StaticHtmlChunkRepository : IStaticHtmlChunkRepository
    {
        private readonly string mCultureName;
        private readonly bool mLatestVersionEnabled;

        public StaticHtmlChunkRepository(string cultureName, bool latestVersionEnabled)
        {
            mCultureName = cultureName;
            mLatestVersionEnabled = latestVersionEnabled;
        }

        public StaticHtmlChunk GetByNodeGuid(Guid nodeGuid)
        {
            return StaticHtmlChunkProvider
                .GetStaticHtmlChunks()
                .LatestVersion(mLatestVersionEnabled)
                .Published(!mLatestVersionEnabled)
                .OnSite(SiteContext.CurrentSiteName)
                .Culture(mCultureName)
                .CombineWithDefaultCulture()
                .WhereEquals("NodeGUID", nodeGuid)
                .TopN(1)
                .FirstOrDefault();
        }
    }
}