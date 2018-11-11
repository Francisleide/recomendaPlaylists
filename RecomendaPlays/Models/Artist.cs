using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecomendaPlays.Models
{
    public class Artist
    {
            [JsonProperty("name")]
            public String Name { get; set; }
    }
}