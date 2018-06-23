using System;
using Newtonsoft.Json;
using J = Newtonsoft.Json.JsonPropertyAttribute;

namespace Yone.Api
{
    public partial class MangaApi
    {
        [J("Entries")] public Entry[] Entries { get; set; }
    }

    public partial class Entry
    {
        [J("Chapters")] public long Chapters { get; set; }

        [J("Volumes")] public long Volumes { get; set; }

        [J("Id")] public long Id { get; set; }

        [J("Title")] public string Title { get; set; }

        [J("English")] public string English { get; set; }

        [J("Synonyms")] public string Synonyms { get; set; }

        [J("Score")] public double Score { get; set; }

        [J("Type")] public string PurpleType { get; set; }

        [J("Status")] public string Status { get; set; }

        [J("StartDate")] public DateTime StartDate { get; set; }

        [J("EndDate")] public string EndDate { get; set; }

        [J("Synopsis")] public string Synopsis { get; set; }

        [J("Image")] public string Image { get; set; }
    }

    public partial class AnimeApi
    {
        [J("Entries")] public Entry[] Entries { get; set; }
    }

    public partial class Entry
    {
        [J("Episodes")] public long Episodes { get; set; }

        [J("Id")] public long ID { get; set; }

        [J("Title")] public string title { get; set; }

        [J("English")] public string english { get; set; }

        [J("Synonyms")] public string synonyms { get; set; }

        [J("Score")] public double score { get; set; }

        [J("Type")] public string purpleType { get; set; }

        [J("Status")] public string status { get; set; }

        [J("StartDate")] public DateTime startDate { get; set; }

        [J("EndDate")] public string endDate { get; set; }

        [J("Synopsis")] public string synopsis { get; set; }

        [J("Image")] public string image { get; set; }
    }

    public partial class AnimeApi
    {
        public static AnimeApi FromJson(string json)
        {
            return JsonConvert.DeserializeObject<AnimeApi>(json, Converter.Settings);
        }
    }

    public partial class MangaApi
    {
        public static MangaApi FromJson(string json)
        {
            return JsonConvert.DeserializeObject<MangaApi>(json, Converter.__Settings);
        }
    }

    public static partial class Serialize
    {
        public static string ToJson(this MangaApi self)
        {
            return JsonConvert.SerializeObject(self, Converter.__Settings);
        }
    }

    public partial class Converter
    {
        public static readonly JsonSerializerSettings __Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None
        };
    }
}