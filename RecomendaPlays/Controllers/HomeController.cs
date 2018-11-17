using Microsoft.ML.Legacy;
using Microsoft.ML.Legacy.Transforms;
using Microsoft.ML.Runtime.Data;
using RecomendaPlays.ML;
using RecomendaPlays.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.ML.Runtime.Api;
using Microsoft.ML.Runtime.Data;
using Microsoft.ML.Runtime.Learners;

namespace RecomendaPlays.Controllers
{
    public class HomeController : Controller
    {
        private readonly SpotifyAuthViewModel _spotifyAuthViewModel;
        private readonly ISpotifyApi _spotifyApi;
        List<PlaylistPronta> playlistProntas = new List<PlaylistPronta>();

        public HomeController(SpotifyAuthViewModel spotifyAuthViewModel, ISpotifyApi spotifyApi)
        {
            _spotifyAuthViewModel = spotifyAuthViewModel;
            _spotifyApi = spotifyApi;
        }

        public ActionResult Index()
        {
            ViewBag.AuthUri = _spotifyAuthViewModel.GetAuthUri();

            return View();
        }

        public ActionResult GenerateNameSortList(string access_token, string error)
        {
            if (error != null || error == "access_denied")
                return View("Error");

            if (string.IsNullOrEmpty(access_token))
                return View();

            try
            {
                _spotifyApi.Token = access_token;
                SpotifyService spotifyService = new SpotifyService(_spotifyApi);
                //Get user_id and user displayName
                SpotifyUser spotifyUser = spotifyService.GetUserProfile();
                ViewBag.UserName = spotifyUser.DisplayName;

                Tracks tocadasRecentemente = spotifyService.GetRecentlyPlayed();
                List<Audio> metaAudios = spotifyService.GetAudioTracks(tocadasRecentemente, spotifyUser);
                
                
                
                
                /*//Usando o framework ML
                string _dataPath = Path.Combine(Environment.CurrentDirectory, "Dados", spotifyUser+"musicas.csv");
                string _modelPath = Path.Combine(@"C:\Users\fran\Documents\Docs\" + spotifyUser.UserId + "musicasSaida.csv");
             
                var env = new LocalEnvironment();
                var reader = new TextLoader(env,
               new TextLoader.Arguments()
               {
                   Separator = ";",
                   HasHeader = true,
                   Column = new[]
                   {
                            new TextLoader.Column("Speechiness", DataKind.R4, 0),
                            new TextLoader.Column("Liveness", DataKind.R4, 1),
                            new TextLoader.Column("Energy", DataKind.R4, 2),
                            new TextLoader.Column("Danceability", DataKind.R4, 3)

                   }
               });
                IDataView trainingDataView = reader.Read(new MultiFileSource(_dataPath));
                var pipeline = new TermEstimator(env, "Label", "Label")
                   .Append(new ConcatEstimator(env, "Speechiness", "Liveness", "Energy", "Danceability", "PetalWidth"))
                   .Append(new SdcaMultiClassTrainer(env, new SdcaMultiClassTrainer.Arguments()))
                   .Append(new KeyToValueEstimator(env, "PredictedLabel"));

                // STEP 4: Train your model based on the data set  
                var model = pipeline.Fit(trainingDataView);

                // var x = spotifyService.MeuK(metaAudios);
                //  metaAudios = spotifyService.calcDistancias(metaAudios);

                //playlistProntas = spotifyService.Knn(metaAudios);

                /* string uriCallback = "http:%2F%2Flocalhost:12029%2FHome%2FPost";
                 string clientId = "215f619c52da4befaa569f12a2108b41";
                 string completo = "https://accounts.spotify.com/en/authorize?client_id=" + clientId +
                      "&response_type=token&redirect_uri=" + uriCallback +
                      "&state=&scope=" + Scope.PLAYLIST_MODIFY_PRIVATE.GetStringAttribute(" ") +
                      "&show_dialog=true";
                 ViewBag.AuthUri = completo;*/

               return RedirectToAction("Create", "Usuarios");
               // Post(access_token, error, playlistProntas);
                //return View("Teste", playlistProntas);
            }
            catch (Exception e)
            {
                return View("Error");
            }

        }

      

        public ActionResult About()
        {
            return View();
        }
        public ActionResult Contact()
        {
            return View();
        }


    }
}