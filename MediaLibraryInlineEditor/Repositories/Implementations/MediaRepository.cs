using System;
using System.Collections.Generic;
using System.Linq;
using CMS.Helpers;
using CMS.MediaLibrary;
using CMS.SiteProvider;

namespace diger74.Repositories.Implementations
{
    public class MediaRepository : IMediaRepository
    {
        private readonly string _libraryName = "Default";
        private readonly string _baseCacheKey = "default|mediafile";

        public IEnumerable<MediaFileInfo> GetAllMediaFilesByLibraryName()
        {
            var cacheKey = $"{_baseCacheKey}|{_libraryName}|all";

            var mediaFiles = new List<MediaFileInfo>();

            using (var cs = new CachedSection<List<MediaFileInfo>>(ref mediaFiles, CacheHelper.CacheMinutes(SiteContext.CurrentSiteName), true, cacheKey))
            {
                if (cs.LoadData)
                {
                    var library =
                        MediaLibraryInfoProvider.GetMediaLibraryInfo(_libraryName, SiteContext.CurrentSiteName);

                    mediaFiles = MediaFileInfoProvider.GetMediaFiles()
                        .WhereEquals("FileLibraryID", library?.LibraryID ?? 0)
                        .ToList();

                    var cacheDependencies = new List<string>
                    {
                        $"{MediaFileInfo.OBJECT_TYPE}|all"
                    };

                    cs.Data = mediaFiles;
                    cs.CacheDependency = CacheHelper.GetCacheDependency(cacheDependencies);
                }
            }

            return mediaFiles;
        }

        public MediaFileInfo GetMediaByPath(string path)
        {
            return GetAllMediaFilesByLibraryName().FirstOrDefault(x => x.FilePath.Equals(path,StringComparison.OrdinalIgnoreCase));
        }
    }
}