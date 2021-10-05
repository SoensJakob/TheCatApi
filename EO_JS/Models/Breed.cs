using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;
using EO_JS.Repositories;

namespace EO_JS.Models
{
    // Class to save basic breed information and image
    public class Breed
    {
        [JsonProperty(PropertyName = "id")]
        public String Id { get; set; }
        [JsonProperty(PropertyName = "name")]
        public String Name { get; set; }
        [JsonProperty(PropertyName = "origin")]
        public String Origin { get; set; }
        [JsonProperty(PropertyName = "description")]
        public String Description { get; set; }
        public ImageCat Image { get; set; }

    }
}
