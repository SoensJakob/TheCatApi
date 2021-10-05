using EO_JS.Helper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EO_JS.Models
{
    // Class to save favourite data and image
    public class Favourite
    {
        [JsonProperty("Id")]
        public String FavouriteId { get; set; }
        [JsonProperty("sub_id")]
        public String User { get; set; }
        [JsonProperty(PropertyName = "image")]
        public ImageCat Image { get; set; }
        public String Icon { get; set; } = IconFont.HeartOutline;
        [JsonProperty(PropertyName = "created_at")]
        public DateTime Timestamp { get; set; }
    }
}
