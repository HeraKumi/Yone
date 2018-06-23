using Newtonsoft.Json;
using J = Newtonsoft.Json.JsonPropertyAttribute;

namespace Yone.Api
{
    public class MainAPI
    {
        public partial class MainApi
        {
            [J("error")] public string Error { get; set; }
        }

        public partial class MainApi
        {
            public static MainApi FromJson(string json)
            {
                return JsonConvert.DeserializeObject<MainApi>(json, Converter.Settings);
            }
        }

        public static class Serialize
        {
            public static string ToJson(MainApi self)
            {
                return JsonConvert.SerializeObject(self, Converter.Settings);
            }
        }

        public class Converter
        {
            public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
            {
                MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
                DateParseHandling = DateParseHandling.None
            };
        }
    }
}