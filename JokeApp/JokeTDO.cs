using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace JokeApp
{
    public class JokeTDO
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("joke")]
        public string Joke { get; set; }

        [JsonPropertyName("setup")]
        public string Setup {  get; set; }

        [JsonPropertyName("delivery")]
        public string Delivery { get; set; }

        [JsonPropertyName("error")]
        public bool Error {  get; set; }
    }
}
