using Newtonsoft.Json;
using J = Newtonsoft.Json.JsonPropertyAttribute;

namespace Yone.Api
{
    public partial class WeatherApi
    {
        [J("coord")] public Coord Coord { get; set; }

        [J("weather")] public Weather[] Weather { get; set; }

        [J("base")] public string Base { get; set; }

        [J("main")] public Main Main { get; set; }

        [J("visibility")] public long Visibility { get; set; }

        [J("wind")] public Wind Wind { get; set; }

        [J("clouds")] public Clouds Clouds { get; set; }

        [J("dt")] public long Dt { get; set; }

        [J("sys")] public Sys Sys { get; set; }

        [J("id")] public long Id { get; set; }

        [J("name")] public string Name { get; set; }

        [J("cod")] public long Cod { get; set; }
    }

    public class Clouds
    {
        [J("all")] public long All { get; set; }
    }

    public class Coord
    {
        [J("lon")] public double Lon { get; set; }

        [J("lat")] public double Lat { get; set; }
    }

    public class Main
    {
        [J("temp")] public double Temp { get; set; }

        [J("pressure")] public long Pressure { get; set; }

        [J("humidity")] public long Humidity { get; set; }

        [J("temp_min")] public double TempMin { get; set; }

        [J("temp_max")] public double TempMax { get; set; }
    }

    public class Sys
    {
        [J("type")] public long PurpleType { get; set; }

        [J("id")] public long Id { get; set; }

        [J("message")] public double Message { get; set; }

        [J("country")] public string Country { get; set; }

        [J("sunrise")] public long Sunrise { get; set; }

        [J("sunset")] public long Sunset { get; set; }
    }

    public class Weather
    {
        [J("id")] public long Id { get; set; }

        [J("main")] public string Main { get; set; }

        [J("description")] public string Description { get; set; }

        [J("icon")] public string Icon { get; set; }
    }

    public class Wind
    {
        [J("speed")] public double Speed { get; set; }

        [J("deg")] public long Deg { get; set; }
    }

    public partial class WeatherApi
    {
        public static WeatherApi FromJson(string json)
        {
            return JsonConvert.DeserializeObject<WeatherApi>(json, Converter._Settings);
        }
    }

    public static partial class Serialize
    {
        public static string ToJson(this WeatherApi self)
        {
            return JsonConvert.SerializeObject(self, Converter._Settings);
        }
    }

    public static partial class Converter
    {
        public static readonly JsonSerializerSettings _Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None
        };
    }
}