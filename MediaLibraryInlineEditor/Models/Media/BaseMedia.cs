using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace diger74.Models.Media
{
    public class BaseMedia
    {
        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public MediaItemType Type { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("path")]
        public string Path { get; set; }
    }
}