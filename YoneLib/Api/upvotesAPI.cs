using Newtonsoft.Json;
using J = Newtonsoft.Json.JsonPropertyAttribute;

namespace Yone.Api
{
    public class upvotesAPI
    {
        public partial class VotesApi
        {
            [J("username")] public string Username { get; set; }

            [J("discriminator")] public string Discriminator { get; set; }

            [J("id")] public string Id { get; set; }

            [J("avatar")] public string Avatar { get; set; }
        }

        public partial class VotesApi
        {
            public static VotesApi[] FromJson(string json)
            {
                return JsonConvert.DeserializeObject<VotesApi[]>(json, Converter.Settings);
            }
        }

        public static class Serialize
        {
            public static string ToJson(VotesApi[] self)
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