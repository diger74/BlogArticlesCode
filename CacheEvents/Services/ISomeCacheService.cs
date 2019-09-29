namespace DancingGoat.Services
{
    public interface ISomeCacheService : IService
    {
        string DataCacheKey { get; }
        string DummyKey { get; }
        string GetSomeCachedData();
    }
}