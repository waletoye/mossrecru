using System;
using Newtonsoft.Json;

namespace mossrecru.Models
{
    public class TechnologyModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("guid")]
        public Guid TechnologyId { get; set; }
    }
}
