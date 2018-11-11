﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecomendaPlays.Models
{
    public class Tracks
    {
        [JsonProperty("items")]
        public List<Track> Items { get; set; }
    }
    public class Track
    {
        [JsonProperty("track")]
        public FullTrack FullTrack { get; set; }

    }

    public class FullTrack
    {
        [JsonProperty("artists")]
        public List<Artist> Artists { get; set; }
        [JsonProperty("name")]
        public String Name { get; set; }
        [JsonProperty("id")]
        public string Id { get; set; }

    }
}