using System.Collections.Generic;
using Newtonsoft.Json;

namespace diger74.Models.Media
{
    public class MediaFolderTreeItem : BaseMedia, IMediaComposite
    {

        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }

        [JsonProperty("items", NullValueHandling = NullValueHandling.Include)]
        public IList<MediaFolderTreeItem> Items { get; set; }
    }
}