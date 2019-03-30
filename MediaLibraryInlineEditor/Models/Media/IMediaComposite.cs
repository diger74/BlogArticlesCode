using System.Collections.Generic;

namespace diger74.Models.Media
{
    public interface IMediaComposite
    {
        IList<MediaFolderTreeItem> Items { get; set; }

        string Path { get; set; }
    }
}