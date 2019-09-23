using System.Collections.Generic;
using System.Linq;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.Helpers;
using CMS.SiteProvider;

namespace Custom.Domain.Helpers
{
    public class RoutingQueryHelper
    {
        public static MultiDocumentQuery GetNodeBySeoUrlQuery(string url, string[] columns = null)
        {
            return DocumentHelper.GetDocuments()
                .Types(GetPageTypesWithSeoUrlClassNames())
                .Columns(columns ?? new[] {Constants.DynamicRouting.SeoUrlFieldName, "DocumentID"})
                .WhereEquals(Constants.DynamicRouting.SeoUrlFieldName, url);
        }

        private static string[] GetPageTypesWithSeoUrlClassNames()
        {
            const string cacheKey = "custom|pagetypeswithseourlclassnames|all";

            var result = new string[0];
            var siteName = SiteContext.CurrentSiteName;

            using (var cs = new CachedSection<string[]>(ref result, CacheHelper.CacheMinutes(siteName), true, cacheKey))
            {
                if (cs.LoadData)
                {
                    // All page types with SeoUrl column
                    result = DataClassInfoProvider.GetClasses()
                        .Where(dataClass =>
                            dataClass.ClassIsDocumentType &&
                            dataClass.ClassSearchSettingsInfos.Any(x => x.Name.Equals(Constants.DynamicRouting.SeoUrlFieldName)))
                        .Select(x => x.ClassName)
                        .ToArray();

                    var cacheDependencies = new List<string>
                    {
                        "cms.class|all",
                        "cms.documenttype|all"
                    };

                    cs.Data = result;
                    cs.CacheDependency = CacheHelper.GetCacheDependency(cacheDependencies);
                }
            }

            return result;
        }
    }
}