using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace RecomendaPlays.Models
{
    public class Player
    {
        [JsonProperty("played_at")]
        public string Played_at { get; set; }
        [JsonProperty("items")]
        public List<Track> Items { get; set; }
        [JsonProperty("href")]
        public string Href { get; set; }
    }
}