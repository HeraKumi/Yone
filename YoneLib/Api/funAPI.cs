using Newtonsoft.Json;

namespace Yone.Api
{
    using J = JsonPropertyAttribute;

    public class funAPI
    {
        public class UrbanApi
        {
            [J("tags")] public string[] Tags { get; set; }

            [J("result_type")] public string ResultType { get; set; }

            [J("list")] public List[] List { get; set; }

            [J("sounds")] public string[] Sounds { get; set; }
        }

        public class List
        {
            [J("defid")] public long Defid { get; set; }

            [J("word")] public string Word { get; set; }

            [J("author")] public string Author { get; set; }

            [J("permalink")] public string Permalink { get; set; }

            [J("definition")] public string Definition { get; set; }

            [J("example")] public string Example { get; set; }

            [J("thumbs_up")] public long ThumbsUp { get; set; }

            [J("thumbs_down")] public long ThumbsDown { get; set; }

            [J("current_vote")] public string CurrentVote { get; set; }
        }

        public class Comics
        {
            [J("month")] public string Month { get; set; }

            [J("num")] public long Num { get; set; }

            [J("link")] public string Link { get; set; }

            [J("year")] public string Year { get; set; }

            [J("news")] public string News { get; set; }

            [J("safe_title")] public string SafeTitle { get; set; }

            [J("transcript")] public string Transcript { get; set; }

            [J("alt")] public string Alt { get; set; }

            [J("img")] public string Img { get; set; }

            [J("title")] public string Title { get; set; }

            [J("day")] public string Day { get; set; }
        }
    }
}