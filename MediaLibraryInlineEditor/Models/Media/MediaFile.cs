using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace diger74.Models.Media
{
    public class MediaFile
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public MediaItemType Type { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }
    }
}