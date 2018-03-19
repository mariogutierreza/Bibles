using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bibles.Models
{
    public class Search
    {
        [JsonProperty("book_id")]
        public long BookId { get; set; }

        [JsonProperty("book_name")]
        public string BookName { get; set; }

        [JsonProperty("book_short")]
        public string BookShort { get; set; }

        [JsonProperty("book_raw")]
        public object BookRaw { get; set; }

        [JsonProperty("chapter_verse")]
        public string ChapterVerse { get; set; }

        [JsonProperty("chapter_verse_raw")]
        public string ChapterVerseRaw { get; set; }
        
        [JsonProperty("verses")]
        public Verses Verses { get; set; }

        [JsonProperty("verses_count")]
        public long VersesCount { get; set; }

        [JsonProperty("single_verse")]
        public bool SingleVerse { get; set; }

        [JsonProperty("nav")]
        public object[] Nav { get; set; }
    }
}
