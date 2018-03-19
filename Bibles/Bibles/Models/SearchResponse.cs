using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bibles.Models
{
    class SearchResponse
    {
        [JsonProperty("hash")]
        public string Hash { get; set; }

        [JsonProperty("error_level")]
        public long ErrorLevel { get; set; }

        [JsonProperty("results")]
        public List<Search> Contents { get; set; }
    }
}
