using Newtonsoft.Json;
using J = Newtonsoft.Json.JsonPropertyAttribute;

namespace Yone.Api
{
    // I know I could've just used one but eeh its what ever :'('''
    public partial class HugApi
    {
        [J("id")] public string Id { get; set; }

        [J("nsfw")] public bool Nsfw { get; set; }

        [J("path")] public string Path { get; set; }

        [J("type")] public string PurpleType { get; set; }
    }

    public partial class HugApi
    {
        public static HugApi FromJson(string json)
        {
            return JsonConvert.DeserializeObject<HugApi>(json, Converter.Settings);
        }
    }

    public partial class CryApi
    {
        [J("id")] public string Id { get; set; }

        [J("nsfw")] public bool Nsfw { get; set; }

        [J("path")] public string Path { get; set; }

        [J("type")] public string PurpleType { get; set; }
    }

    public partial class CryApi
    {
        public static CryApi FromJson(string json)
        {
            return JsonConvert.DeserializeObject<CryApi>(json, Converter.Settings);
        }
    }

    public partial class CuddleApi
    {
        [J("id")] public string Id { get; set; }

        [J("nsfw")] public bool Nsfw { get; set; }

        [J("path")] public string Path { get; set; }

        [J("type")] public string PurpleType { get; set; }
    }

    public partial class CuddleApi
    {
        public static CuddleApi FromJson(string json)
        {
            return JsonConvert.DeserializeObject<CuddleApi>(json, Converter.Settings);
        }
    }

    public partial class KissApi
    {
        [J("id")] public string Id { get; set; }

        [J("nsfw")] public bool Nsfw { get; set; }

        [J("path")] public string Path { get; set; }

        [J("type")] public string PurpleType { get; set; }
    }

    public partial class KissApi
    {
        public static KissApi FromJson(string json)
        {
            return JsonConvert.DeserializeObject<KissApi>(json, Converter.Settings);
        }
    }

    public partial class LewdApi
    {
        [J("id")] public string Id { get; set; }

        [J("nsfw")] public bool Nsfw { get; set; }

        [J("path")] public string Path { get; set; }

        [J("type")] public string PurpleType { get; set; }
    }

    public partial class LewdApi
    {
        public static LewdApi FromJson(string json)
        {
            return JsonConvert.DeserializeObject<LewdApi>(json, Converter.Settings);
        }
    }

    public partial class LickApi
    {
        [J("id")] public string Id { get; set; }

        [J("nsfw")] public bool Nsfw { get; set; }

        [J("path")] public string Path { get; set; }

        [J("type")] public string PurpleType { get; set; }
    }

    public partial class LickApi
    {
        public static LickApi FromJson(string json)
        {
            return JsonConvert.DeserializeObject<LickApi>(json, Converter.Settings);
        }
    }

    public partial class NomApi
    {
        [J("id")] public string Id { get; set; }

        [J("nsfw")] public bool Nsfw { get; set; }

        [J("path")] public string Path { get; set; }

        [J("type")] public string PurpleType { get; set; }
    }

    public partial class NomApi
    {
        public static NomApi FromJson(string json)
        {
            return JsonConvert.DeserializeObject<NomApi>(json, Converter.Settings);
        }
    }

    public partial class NyanApi
    {
        [J("id")] public string Id { get; set; }

        [J("nsfw")] public bool Nsfw { get; set; }

        [J("path")] public string Path { get; set; }

        [J("type")] public string PurpleType { get; set; }
    }

    public partial class NyanApi
    {
        public static NyanApi FromJson(string json)
        {
            return JsonConvert.DeserializeObject<NyanApi>(json, Converter.Settings);
        }
    }

    public partial class OwoApi
    {
        [J("id")] public string Id { get; set; }

        [J("nsfw")] public bool Nsfw { get; set; }

        [J("path")] public string Path { get; set; }

        [J("type")] public string PurpleType { get; set; }
    }

    public partial class OwoApi
    {
        public static OwoApi FromJson(string json)
        {
            return JsonConvert.DeserializeObject<OwoApi>(json, Converter.Settings);
        }
    }

    public partial class PatApi
    {
        [J("id")] public string Id { get; set; }

        [J("nsfw")] public bool Nsfw { get; set; }

        [J("path")] public string Path { get; set; }

        [J("type")] public string PurpleType { get; set; }
    }

    public partial class PatApi
    {
        public static PatApi FromJson(string json)
        {
            return JsonConvert.DeserializeObject<PatApi>(json, Converter.Settings);
        }
    }

    public partial class PoutApi
    {
        [J("id")] public string Id { get; set; }

        [J("nsfw")] public bool Nsfw { get; set; }

        [J("path")] public string Path { get; set; }

        [J("type")] public string PurpleType { get; set; }
    }

    public partial class PoutApi
    {
        public static PoutApi FromJson(string json)
        {
            return JsonConvert.DeserializeObject<PoutApi>(json, Converter.Settings);
        }
    }

    public partial class SlapApi
    {
        [J("id")] public string Id { get; set; }

        [J("nsfw")] public bool Nsfw { get; set; }

        [J("path")] public string Path { get; set; }

        [J("type")] public string PurpleType { get; set; }
    }

    public partial class SlapApi
    {
        public static SlapApi FromJson(string json)
        {
            return JsonConvert.DeserializeObject<SlapApi>(json, Converter.Settings);
        }
    }

    public partial class SmugApi
    {
        [J("id")] public string Id { get; set; }

        [J("nsfw")] public bool Nsfw { get; set; }

        [J("path")] public string Path { get; set; }

        [J("type")] public string PurpleType { get; set; }
    }

    public partial class SmugApi
    {
        public static SmugApi FromJson(string json)
        {
            return JsonConvert.DeserializeObject<SmugApi>(json, Converter.Settings);
        }
    }

    public partial class StareApi
    {
        [J("id")] public string Id { get; set; }

        [J("nsfw")] public bool Nsfw { get; set; }

        [J("path")] public string Path { get; set; }

        [J("type")] public string PurpleType { get; set; }
    }

    public partial class StareApi
    {
        public static StareApi FromJson(string json)
        {
            return JsonConvert.DeserializeObject<StareApi>(json, Converter.Settings);
        }
    }

    public partial class TickleApi
    {
        [J("id")] public string Id { get; set; }

        [J("nsfw")] public bool Nsfw { get; set; }

        [J("path")] public string Path { get; set; }

        [J("type")] public string PurpleType { get; set; }
    }

    public partial class TickleApi
    {
        public static TickleApi FromJson(string json)
        {
            return JsonConvert.DeserializeObject<TickleApi>(json, Converter.Settings);
        }
    }

    public partial class TriggeredApi
    {
        [J("id")] public string Id { get; set; }

        [J("nsfw")] public bool Nsfw { get; set; }

        [J("path")] public string Path { get; set; }

        [J("type")] public string PurpleType { get; set; }
    }

    public partial class TriggeredApi
    {
        public static TriggeredApi FromJson(string json)
        {
            return JsonConvert.DeserializeObject<TriggeredApi>(json, Converter.Settings);
        }
    }

    public partial class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None
        };
    }
}