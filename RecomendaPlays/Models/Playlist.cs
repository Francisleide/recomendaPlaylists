﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace RecomendaPlays.Models
{
    public class Playlist
    {
        [JsonProperty("id")]
        public String Id { get; set; }
        [JsonProperty("name")]
        public String Name { get; set; }
        [JsonProperty("owner")]
        public PublicProfile Owner { get; set; }
        [JsonProperty("uris")]
        public List<string> Uris { get; set; }
    }

    public class Playlists
        {
            [JsonProperty("items")]
            public List<Playlist> Items { get; set; }
        }

       
        public class PlaylistPronta
        {
            public List<Audio> audios { get; set; }
            public string Nome { get; set; }

        }
    }