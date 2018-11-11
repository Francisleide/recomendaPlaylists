using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecomendaPlays.Models
{
    public class Audio
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("speechiness")]
        public double Speechiness { get; set; }
        [JsonProperty("liveness")]
        public double Liveness { get; set; }
        [JsonProperty("energy")]
        public double Energy { get; set; }
        [JsonProperty("danceability")]
        public double Danceability { get; set; }
        [JsonProperty("instrumentalness")]
        public double Instrumentalness { get; set; }
        [JsonProperty("loudness")]
        public double Loudness { get; set; }
        [JsonProperty("tempo")]
        public double Tempo { get; set; }
        public FullTrack FullTrack { get; set; }
    }
}