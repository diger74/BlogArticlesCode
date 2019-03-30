using System.Collections.Generic;
using diger74.Models.Media;

namespace diger74.Services
{
    public interface IMediaService
    {
        IEnumerable<ImageModel> GetAllImages();

        MediaFolderTree GetMediaFolderTree();

        MediaFilesSet GetMediaFiles(string path);
    }
}