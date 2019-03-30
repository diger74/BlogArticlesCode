using Newtonsoft.Json;

namespace diger74.Models.Media
{
    public class MediaFilesSet
    {
        [JsonProperty("hash")]
        public string Hash { get; set; }

        [JsonProperty("items")]
        public MediaFile[] Items { get; set; }
    }
}