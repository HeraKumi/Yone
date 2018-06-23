using Newtonsoft.Json;

namespace YoneLib.Api
{
    using J = JsonPropertyAttribute;

    public partial class Cat
    {
        [J("_id")] public long Id { get; set; }
        [J("NUm")] public int Num { get; set; }
    }

    public partial class Cat
    {
        public static Cat FromJson(string json)
        {
            return JsonConvert.DeserializeObject<Cat>(json, Converter.Settings);
        }
    }

    public static class Serialize
    {
        public static string ToJson(Cat self)
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