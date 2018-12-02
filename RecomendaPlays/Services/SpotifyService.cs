using CsvHelper;
using Microsoft.ML.Legacy;
using Microsoft.ML.Legacy.Trainers;
using Microsoft.ML.Legacy.Transforms;
using RecomendaPlays.ML;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
namespace RecomendaPlays.Models
{
    public class SpotifyService
    {
        static PredictionModel<Musica, ClusterPrediction> model;
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
            String nomeArquivo = usuario.UserId + "_" + usuario.DisplayName;
            foreach (var metaAudio in metaAudios)
            {
                TrackML trackML = new TrackML();
                trackML.Danceability = metaAudio.Danceability;
                trackML.Energy = metaAudio.Energy;
                trackML.Id = metaAudio.Id;
                trackML.Instrumentalness = metaAudio.Instrumentalness;
                trackML.Liveness = metaAudio.Liveness;
                trackML.Loudness = metaAudio.Loudness;
                trackML.NomeArtista = metaAudio.FullTrack.Artists.FirstOrDefault().Name;
                trackML.NomeMusica = metaAudio.FullTrack.Name;
                trackML.Speechiness = metaAudio.Speechiness;
                tracksML.Add(trackML);
                using (var sw = new StreamWriter(@"C:\Users\fran\Documents\Docs\" + nomeArquivo + "musicas.csv"))
                {
                    var writer = new CsvWriter(sw);
                    writer.WriteRecords(tracksML);
                }
            }

            return metaAudios;
        }


       
        public Recomendacoes geraplay(string _dataPath, string _dataPath2, PredictionModel<Musica, ClusterPrediction> model, PredictionModel<Musica, ClusterPrediction> model2)
        {
            //await model.WriteAsync(_modelPath);
            //PredictionModel<Musica, ClusterPrediction> model2 = TrainBaseToda(_dataPath, _dataPath2, _modelPath, _modelPath2);
            List<Musica> p1List = new List<Musica>();
            List<Musica> p2List = new List<Musica>();
            List<Musica> p3List = new List<Musica>();
            string[] arq = File.ReadAllLines(_dataPath);
            for (int i = 1; i < arq.Length; i++)
            {
                Musica m1 = new Musica();
                string[] aux = arq[i].Split(',');
                m1.Cantor = aux[0];
                m1.Titulo = aux[9];

                // Console.WriteLine(aux[3]);
                m1.Liveness = float.Parse(aux[3].Replace("\"", "").Replace(".", ","));
                m1.Energy = float.Parse(aux[4].Replace("\"", "").Replace(".", ","));
                m1.Danceability = float.Parse(aux[5].Replace("\"", "").Replace(".", ","));
                m1.Speechiness = float.Parse(aux[2].Replace("\"", "").Replace(".", ","));

                var prediction = model.Predict(m1);
                if (prediction.PredictedClusterId == 1)
                {
                    p1List.Add(m1);
                }
                if (prediction.PredictedClusterId == 2)
                {
                    p2List.Add(m1);
                }
                if (prediction.PredictedClusterId == 3)
                {
                    p3List.Add(m1);
                }

                //Console.WriteLine($"Cluster: {prediction.PredictedClusterId}");
                //Console.WriteLine($"Distances: {string.Join(" ", prediction.Distances)}");

            }
            //////////////////////////////////////////////////////////////////////////////////////


            //dataset grandão agora!
            List<Musica> play1DS = new List<Musica>();
            List<Result> resultados = new List<Result>();
            float somaDist = 0;
            string[] arq2 = File.ReadAllLines(_dataPath2);
            for (int i = 2; i < arq2.Length; i++)
            {
                Musica m1 = new Musica();
                string[] aux = arq2[i].Split(',');
                m1.Cantor = aux[0];
                m1.Titulo = aux[9];
                m1.Liveness = float.Parse(aux[3].Replace("\"", "").Replace(".", ","));
                m1.Energy = float.Parse(aux[4].Replace("\"", "").Replace(".", ","));
                m1.Danceability = float.Parse(aux[5].Replace("\"", "").Replace(".", ","));
                m1.Speechiness = float.Parse(aux[2].Replace("\"", "").Replace(".", ","));

                var prediction = model2.Predict(m1);
                Result result2;
                ClusterPrediction cp;
                for (uint j = 1; j <= 100; j++)
                {
                    if (prediction.PredictedClusterId == j)
                    {
                        result2 = new Result();
                        result2.Musica = m1;
                        cp = new ClusterPrediction();
                        cp.PredictedClusterId = prediction.PredictedClusterId;
                        cp.Distances = prediction.Distances;
                        result2.ClusterPrediction = cp;
                        //  Console.WriteLine("Musica: "+i+" - " + result2.Musica.Titulo);
                        //Console.WriteLine("Cluster: " + result2.ClusterPrediction.PredictedClusterId);
                        resultados.Add(result2);

                    }
                }
                //resultados.OrderBy();
            }
            int qElementos = 0;
            float menor = 0;
            int idClusterMaior = 0;
            List<Musica> playlistRecomendada = new List<Musica>();
            List<Musica> playlistRecomendada2 = new List<Musica>();
            List<Musica> playlistRecomendadaALEATORIA = new List<Musica>();


            //Isto é minha recomendação!
            //----------------------------------------------------------------
            //c é o cluster

            for (int c = 1; c < resultados.Count; c++)
            {
                foreach (var resultado in resultados)
                {
                    if (resultado.ClusterPrediction.PredictedClusterId == c)
                    {

                        var pred = model.Predict(resultado.Musica);
                        //calculei a ditancia de uma musica pra os 3 clusters do usuário
                        somaDist = somaDist + pred.Distances[0];
                        somaDist = somaDist + pred.Distances[1];
                        somaDist = somaDist + pred.Distances[2];
                        qElementos++;
                    }
                }
                var media = somaDist / qElementos;
                if (c == 1)
                {
                    menor = media;
                    idClusterMaior = c;
                }

                if (media < menor)
                {
                    menor = media;
                    idClusterMaior = c;

                }
                somaDist = 0;
                qElementos = 0;

            }
            //  ----------------------------------------------------------------------
            //Segunda playlist
            int qElementos2 = 0;
            float somaDist2 = 0;
            float menor2 = 0;
            int idClusterMaior2 = 0;
            for (int c = 1; c < resultados.Count; c++)
            {
                foreach (var resultado in resultados)
                {
                    if (resultado.ClusterPrediction.PredictedClusterId == c)
                    {

                        var pred = model.Predict(resultado.Musica);
                        //calculei a ditancia de uma musica pra os 3 clusters do usuário
                        somaDist2 = somaDist2 + pred.Distances[0];
                        qElementos2++;
                    }
                }
                var media = somaDist2 / qElementos2;
                if (c == 1)
                {
                    menor2 = media;
                    idClusterMaior2 = c;
                }

                if (media < menor2)
                {
                    menor2 = media;
                    idClusterMaior = c;

                }
                somaDist2 = 0;
                qElementos2 = 0;

            }

            //----------------------------------------------------------------
            //Terceira Playlist
            int aleatorio = 0;
            Random random = new Random();
            for (int i = 0; i < 20; i++)
            {
               // resultados[i].Musica.qualCluster = 3;
                aleatorio = random.Next(0, resultados.Count);
                playlistRecomendadaALEATORIA.Add(resultados[aleatorio].Musica);
                
            }

            //---------------------------------------------------------------------
            foreach (var r in resultados)
            {
                if (r.ClusterPrediction.PredictedClusterId == idClusterMaior)
                {
                    r.Musica.QualCluster = 1;
                    playlistRecomendada2.Add(r.Musica);
                    Console.WriteLine(r.Musica.Titulo + " - " + r.Musica.Cantor);
                }
            }

            //Preenchendo a exibição da segunda

            foreach (var r in resultados)
            {
                if (r.ClusterPrediction.PredictedClusterId == idClusterMaior2)
                {
                    r.Musica.QualCluster = 2;
                    playlistRecomendada.Add(r.Musica);
                    Console.WriteLine(r.Musica.Titulo + " - " + r.Musica.Cantor);
                }
            }

            for (int i = 0; i < playlistRecomendadaALEATORIA.Count ; i++)
            {
                playlistRecomendadaALEATORIA[i].QualCluster = 3;
            }
        
            Recomendacoes recomendacoes = new Recomendacoes();
            recomendacoes.P1 = playlistRecomendada;
            recomendacoes.P2 = playlistRecomendada2;
            recomendacoes.P3 = playlistRecomendadaALEATORIA;
            return recomendacoes;

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