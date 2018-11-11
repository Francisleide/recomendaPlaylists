using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecomendaPlays.Models
{
    public interface ISpotifyApi
    {
        string Token { get; set; }

        T GetSpotifyType<T>(string url);
        T PostSpotifyType<T>(string url, string PToken, string json);
    }
}