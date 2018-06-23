using Newtonsoft.Json;
using J = Newtonsoft.Json.JsonPropertyAttribute;

namespace YoneLib.Api
{
    public class DefaultAPI
    {
        public partial class DefaultApi
        {
            [J("botToken")] public string botToken { get; set; }
            [J("ipHub")] public string ipToken { get; set; }
            [J("discordBotOrg")] public string dboToken { get; set; }
            [J("kitsuClientId")] public string kcId { get; set; }
            [J("kitsuClientSecret")] public string kcsId { get; set; }
            [J("twitchClientId")] public string tcId { get; set; }
            [J("FirstRun")] public bool firstRun { get; set; }
        }

        public partial class DefaultApi
        {
            public static DefaultApi FromJson(string json)
            {
                return JsonConvert.DeserializeObject<DefaultApi>(json, Converteer.Settings);
            }
        }

        public static class Serialize
        {
            public static string ToJson(DefaultApi self)
            {
                return JsonConvert.SerializeObject(self, Converteer.Settings);
            }
        }

        public class Converteer
        {
            public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
            {
                MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
                DateParseHandling = DateParseHandling.None
            };
        }
    }
}