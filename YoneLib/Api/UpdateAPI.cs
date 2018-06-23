using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace YoneLib.Api
{
    using J = JsonPropertyAttribute;
    using R = Required;
    using N = NullValueHandling;

    public partial class Testw
    {
        [J("_id")] public long Id { get; set; }
        [J("UpdateText")] public string UpdateText { get; set; }
        [J("UpdateVersion")] public string UpdateVersion { get; set; }
        [J("SubmittedBy")] public string SubmittedBy { get; set; }
    }

    public partial class Testw
    {
        public static Testw FromJson(string json)
        {
            return JsonConvert.DeserializeObject<Testw>(json, Converterw.Settings);
        }
    }

    public static class Serializew
    {
        public static string ToJson(this Testw self)
        {
            return JsonConvert.SerializeObject(self, Converterw.Settings);
        }
    }

    internal static class Converterw
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter {DateTimeStyles = DateTimeStyles.AssumeUniversal}
            }
        };
    }
}