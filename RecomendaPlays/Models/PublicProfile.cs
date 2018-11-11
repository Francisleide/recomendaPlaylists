using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecomendaPlays.Models
{
    public class PublicProfile
    {
        [JsonProperty("id")]
        public String Id { get; set; }
    }
}