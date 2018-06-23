using Newtonsoft.Json;
using J = Newtonsoft.Json.JsonPropertyAttribute;

namespace Yone.Api
{
    public class YoneUserAPI
    {
        public partial class BlacklistAPi
        {
            [J("_id")] public long Id { get; set; }
            [J("Blocked")] public bool Blocked { get; set; }
        }

        public partial class BlacklistAPi
        {
            public static BlacklistAPi FromJson(string json)
            {
                return JsonConvert.DeserializeObject<BlacklistAPi>(json, Converter.Settings);
            }
        }

        public static class Serialize
        {
            public static string ToJson(BlacklistAPi self)
            {
                return JsonConvert.SerializeObject(self, Converter.Settings);
            }
        }

        private class Converter
        {
            public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
            {
                MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
                DateParseHandling = DateParseHandling.None
            };
        }
    }
}