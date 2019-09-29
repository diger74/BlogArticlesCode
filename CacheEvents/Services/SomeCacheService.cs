using CMS.Helpers;
using DancingGoat.Infrastructure;

namespace DancingGoat.Services
{
    public class SomeCacheService : ISomeCacheService
    {
        private const string _dataCacheKey = "custom|somecacheddata";
        private const string _dummyKey = "custom|dummykey";

        private static bool _inited = false;

        private static void Init()
        {
            if (_inited) return;

            CacheExpirationCallbacks.Register(_dataCacheKey, () => CacheHelper.TouchKey(_dummyKey));
            _inited = true;
        }

        public SomeCacheService()
        {
            Init();
        }

        public string DataCacheKey => _dataCacheKey;
        public string DummyKey => _dummyKey;

        public string GetSomeCachedData()
        {
            var data = string.Empty;

            using (var cs = new CachedSection<string>(ref data, 1, true, DataCacheKey))
            {
                if (cs.LoadData)
                {
                    cs.Data = "some data";
                }
            }

            return data;
        }
    }
}