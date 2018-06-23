using Newtonsoft.Json;

namespace YoneLib.Api
{
    public class YoneDatabaseAPI
    {
        public partial class YoneDatabaseApi
        {
            [JsonProperty("_id")] public long Id { get; set; }
            [JsonProperty("GuildOwner")] public string GuildOwner { get; set; }
            [JsonProperty("GuildPrefix")] public string GuildPrefix { get; set; }
            [JsonProperty("AnnouncementChannel")] public string AnnouncementChannel { get; set; }
            [JsonProperty("WelcomeChannel")] public string WelcomeChannel { get; set; }
            [JsonProperty("LeaveChannel")] public string LeaveChannel { get; set; }
            [JsonProperty("ModerationChannel")] public string ModerationChannel { get; set; }
            [JsonProperty("LogChannel")] public string LogChannel { get; set; }
            [JsonProperty("WelcomeMessage")] public string WelcomeMessage { get; set; }
            [JsonProperty("LeaveMessage")] public string LeaveMessage { get; set; }
            [JsonProperty("BotWelcomeMessage")] public string BotWelcomeMessage { get; set; }
            [JsonProperty("BotLeaveMessage")] public string BotLeaveMessage { get; set; }
            [JsonProperty("IPlinkChecker")] public bool IPlinkChecker { get; set; }
            [JsonProperty("SwearChecker")] public bool SwearChecker { get; set; }
            [JsonProperty("nsfwLinkChecker")] public bool NsfwLinkChecker { get; set; }
            [JsonProperty("LinkBlocker")] public bool LinkBlocker { get; set; }
            [JsonProperty("SwearReplacer")] public bool SwearReplacer { get; set; }
            [JsonProperty("ModRole")] public string ModRole { get; set; }
            [JsonProperty("AdminRole")] public string AdminRole { get; set; }
            [JsonProperty("MuteRole")] public string MuteRole { get; set; }
            [JsonProperty("Autorole")] public string Autorole { get; set; }
            [JsonProperty("BotAutoRole")] public string BotAutoRole { get; set; }
        }

        public partial class YoneDatabaseApi
        {
            public static YoneDatabaseApi FromJson(string json)
            {
                return JsonConvert.DeserializeObject<YoneDatabaseApi>(json, Converter.Settings);
            }
        }

        public static class Serialize
        {
            public static string ToJson(YoneDatabaseApi self)
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