using System;
using CMS.DocumentEngine.Types.Custom;

namespace DancingGoat.Repositories
{
    public interface IStaticHtmlChunkRepository : IRepository
    {
        StaticHtmlChunk GetByNodeGuid(Guid nodeGuid);
    }
}