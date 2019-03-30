using System.Collections.Generic;
using CMS.MediaLibrary;

namespace diger74.Repositories
{
    public interface IMediaRepository
    {
        IEnumerable<MediaFileInfo> GetAllMediaFilesByLibraryName();
        MediaFileInfo GetMediaByPath(string path);
    }
}