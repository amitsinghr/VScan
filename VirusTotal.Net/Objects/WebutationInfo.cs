using Newtonsoft.Json;

namespace VirusTotalNET.Objects
{
    public class WebutationInfo
    {
        [JsonProperty("Adult content")]
        public string AdultContent { get; set; }

        [JsonProperty("Safety score")]
        public int SafetyScore { get; set; }

        public string Verdict { get; set; }
    }
}