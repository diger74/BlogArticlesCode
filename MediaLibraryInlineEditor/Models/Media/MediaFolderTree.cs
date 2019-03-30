using System.Collections.Generic;
using Newtonsoft.Json;

namespace diger74.Models.Media
{
    public class MediaFolderTree : BaseMedia, IMediaComposite
    {
        public MediaFolderTree()
        {
            Type = MediaItemType.root;
        }

        [JsonProperty("hash")]
        public string Hash { get; set; }

        [JsonProperty("items", NullValueHandling = NullValueHandling.Include)]
        public IList<MediaFolderTreeItem> Items { get; set; }
    }
}