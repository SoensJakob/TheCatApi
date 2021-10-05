using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace EO_JS.Models
{
    // Class to save extra breed information with inheritance from breed
    public class BreedInfo : Breed
    {
        
        [JsonProperty(PropertyName = "temperament")]
        public String Temperament { get; set; }
        [JsonProperty(PropertyName = "life_span")]
        public String Lifespan { get; set; }

        [JsonProperty(PropertyName = "affection_level")]
        public int Affection { get; set; }
        [JsonProperty(PropertyName = "energy_level")]
        public int Energy { get; set; }
        [JsonProperty(PropertyName = "intelligence")]
        public int Intelligence { get; set; }
        [JsonProperty(PropertyName = "social_needs")]
        public int Social { get; set; }
        [JsonProperty(PropertyName = "vocalisation")]
        public int Vocalisation { get; set; }

        public String WeightMetric { get; set; }

        [JsonExtensionData]
        private readonly Dictionary<string, JToken> _extraJsonData = new Dictionary<string, JToken>();

        [OnDeserialized]
        private void ProcessExtraJsonData(StreamingContext context)
        {
            JToken weightMetric = (JToken)_extraJsonData["weight"];
            WeightMetric = (string)weightMetric.SelectToken("metric");
        }

    }
}
