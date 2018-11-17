using CsvHelper;
using RecomendaPlays.ML;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace RecomendaPlays.Models
{
    public class SpotifyService
    {
        private ISpotifyApi _spotifyApi;

        public SpotifyService(ISpotifyApi spotifyApi)
        {
            _spotifyApi = spotifyApi;
        }

        public List<string> GetPlaylistsName(string userId)
        {
            Playlists playLists = GetPlaylists(userId);

            List<string> playlistNames = new List<string>();

            foreach (var playlist in playLists.Items)
            {
                playlistNames.Add(playlist.Name);
            }

            return playlistNames;
        }

        public SpotifyUser GetUserProfile()
        {
            string url = "https://api.spotify.com/v1/me";
            SpotifyUser spotifyUser = _spotifyApi.GetSpotifyType<SpotifyUser>(url);
            return spotifyUser;
        }
        public Tracks GetRecentlyPlayed()
        {
            string url = "https://api.spotify.com/v1/me/player/recently-played?limit=50";
            Tracks tracks = _spotifyApi.GetSpotifyType<Tracks>(url);
            return tracks;
        }

        public Playlists GetPlaylists(string userId)
        {
            string url = string.Format("https://api.spotify.com/v1/users/{0}/playlists?", userId);
            Playlists playlists = _spotifyApi.GetSpotifyType<Playlists>(url);

            return playlists;
        }
        public Audio getAudio(string urlTrack)
        {
            string url = string.Format("https://api.spotify.com/v1/audio-features/{0}", urlTrack);
            Audio audio = _spotifyApi.GetSpotifyType<Audio>(url);
            return audio;
        }
        //em tracks so tem id e name  (vetor)
        public List<Audio> GetAudioTracks(Tracks tracks, SpotifyUser usuario)
        {

            List<Audio> metaAudios = new List<Audio>();

            foreach (var track in tracks.Items)
            {
                if (tracks == null)
                    continue;
                string music = track.FullTrack.Name;
                string urlTrack = track.FullTrack.Id;
                Audio audio = getAudio(urlTrack);
                audio.FullTrack = track.FullTrack;
                metaAudios.Add(audio);

            }
            List<TrackML> tracksML = new List<TrackML>();
            foreach (var metaAudio in metaAudios)
            {
                TrackML trackML = new TrackML();
                trackML.Danceability = metaAudio.Danceability;
                trackML.Danceability = metaAudio.Energy;
                trackML.Id = metaAudio.Id;
                trackML.Instrumentalness = metaAudio.Instrumentalness;
                trackML.Liveness = metaAudio.Liveness;
                trackML.Loudness = metaAudio.Loudness;
                trackML.NomeArtista = metaAudio.FullTrack.Artists.FirstOrDefault().Name;
                trackML.NomeMusica = metaAudio.FullTrack.Name;
                tracksML.Add(trackML);
            }
            //gerar o nome do arquivo
            String nomeArquivo = usuario.UserId + "_"+usuario.DisplayName;
            //salvar o arquivo
            string path = HttpContext.Current.Server.MapPath("~/Content/Dados/");
            using (var sw = new StreamWriter(path + nomeArquivo + "musicas.csv"))
           {
                    var writer = new CsvWriter(sw);
                   writer.WriteRecords(metaAudios);
           }
            return metaAudios;
        }

        

        public Playlist PostPlays(PlaylistPronta playlistPronta, string access_token, string userId)
        {
            Tracks play = new Tracks();
            List<Track> items = new List<Track>();
            string json2 = "{\"uris\":[";
            string pedacoJson2 = "\"spotify:track:";
            string outropedaco = "";
            for (int i = 0; i < playlistPronta.audios.Count(); i++)
            {
                var t = new Track();
                t.FullTrack = playlistPronta.audios[i].FullTrack;
                items.Add(t);

                if (i == playlistPronta.audios.Count() - 1)
                {
                    outropedaco = outropedaco + pedacoJson2 + t.FullTrack.Id;
                }
                else
                {
                    outropedaco = outropedaco + pedacoJson2 + t.FullTrack.Id + "\",";
                }
            }

            play.Items = items;
            string url = "https://api.spotify.com/v1/users/" + userId + "/playlists";
            string json = "{\"name\":\"" + playlistPronta.Nome + "\",\"description\":\"Playlist gerada por algoritmo usando IA\", \"public\": false}";

            Playlist playlist = _spotifyApi.PostSpotifyType<Playlist>(url, access_token, json);
            string url2 = url + "/" + playlist.Id + "/tracks";
            json2 = json2 + outropedaco + "\"]}";
            Tracks tracks = _spotifyApi.PostSpotifyType<Tracks>(url2, access_token, json2);
            return playlist;
        }


    }
}