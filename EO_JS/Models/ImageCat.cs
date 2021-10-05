using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;
using EO_JS.Helper;

namespace EO_JS.Models
{
    // Class to save an image in 
    public class ImageCat
    {
        [JsonProperty(PropertyName = "id")]
        public String Id { get; set; }
        [JsonProperty(PropertyName = "url")]
        public String Url { get; set; }

    }
}
