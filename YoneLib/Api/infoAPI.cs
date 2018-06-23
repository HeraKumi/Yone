using System;
using Newtonsoft.Json;

namespace Yone.Api
{
    using J = JsonPropertyAttribute;

    public class InfoApi
    {
        public class IpApi
        {
            [J("ip")] public string Ip { get; set; }
            [J("countryCode")] public string CountryCode { get; set; }
            [J("countryName")] public string CountryName { get; set; }
            [J("asn")] public long Asn { get; set; }
            [J("isp")] public string Isp { get; set; }
            [J("block")] public long Block { get; set; }
            [J("hostname")] public string Hostname { get; set; }
        }

        public class GeoApi
        {
            [J("ip")] public string Ip { get; set; }
            [J("country_code")] public string CountryCode { get; set; }
            [J("country_name")] public string CountryName { get; set; }
            [J("region_code")] public string RegionCode { get; set; }
            [J("region_name")] public string RegionName { get; set; }
            [J("city")] public string City { get; set; }
            [J("zip_code")] public string ZipCode { get; set; }
            [J("time_zone")] public string TimeZone { get; set; }
            [J("latitude")] public double Latitude { get; set; }
            [J("longitude")] public double Longitude { get; set; }
            [J("metro_code")] public long MetroCode { get; set; }
        }

        public class GithubApi
        {
            [J("avatar_url")] public string AvatarUrl { get; set; }
            [J("gravatar_id")] public string GravatarId { get; set; }
            [J("html_url")] public string HtmlUrl { get; set; }
            [J("name")] public string Name { get; set; }
            [J("blog")] public string Blog { get; set; }
            [J("location")] public string Location { get; set; }
            [J("email")] public string Email { get; set; }
            [J("bio")] public string Bio { get; set; }
            [J("public_repos")] public long PublicRepos { get; set; }
            [J("followers")] public long Followers { get; set; }
            [J("following")] public long Following { get; set; }
            [J("created_at")] public DateTime CreatedAt { get; set; }
            [J("message")] public string Message { get; set; }
        }

        public class DicApi
        {
            [J("type")] public string Type { get; set; }
            [J("definition")] public string Definition { get; set; }
            [J("example")] public string Example { get; set; }
        }

        public partial class Dbapi
        {
            [J("defAvatar")] public string DefAvatar { get; set; }
            [J("invite")] public string Invite { get; set; }
            [J("prefix")] public string Prefix { get; set; }
            [J("lib")] public string Lib { get; set; }
            [J("clientid")] public string Clientid { get; set; }
            [J("avatar")] public string Avatar { get; set; }
            [J("id")] public string Id { get; set; }
            [J("discriminator")] public string Discriminator { get; set; }
            [J("username")] public string Username { get; set; }
            [J("date")] public DateTime Date { get; set; }
            [J("support")] public string Support { get; set; }
            [J("server_count")] public long ServerCount { get; set; }
            [J("shards")] public object[] Shards { get; set; }
            [J("points")] public long Points { get; set; }
            [J("certifiedBot")] public bool CertifiedBot { get; set; }
            [J("owners")] public string[] Owners { get; set; }
            [J("tags")] public object[] Tags { get; set; }
            [J("legacy")] public bool Legacy { get; set; }
        }

        public partial class Dbapi
        {
            public static Dbapi FromJson(string json)
            {
                return JsonConvert.DeserializeObject<Dbapi>(json, Converter.Settings);
            }
        }

        public static class Serialize
        {
            public static string ToJson(Dbapi self)
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