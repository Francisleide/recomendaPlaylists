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
        private RecomendaPlaysContext db = new RecomendaPlaysContext();
        private readonly SpotifyAuthViewModel _spotifyAuthViewModel;
        private readonly ISpotifyApi _spotifyApi;
        List<PlaylistPronta> playlistProntas = new List<PlaylistPronta>();
        SpotifyUser spotifyUser;
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

      /*  public ActionResult PlayLists() {
            ViewBag.usuario = spotifyUser.DisplayName;
            return View(db.Musicas.Where(m => m.idUsuario.Equals(spotifyUser.UserId)).ToList());
        }*/

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
                spotifyUser = spotifyService.GetUserProfile();
                ViewBag.UserName = spotifyUser.DisplayName;

                Tracks tocadasRecentemente = spotifyService.GetRecentlyPlayed();
                
                List<Audio> metaAudios = spotifyService.GetAudioTracks(tocadasRecentemente, spotifyUser);
                // GeraPlayLists();
                
                string _dataPath = Path.Combine("C://Users//fran//Documents//Docs//225jyk7jzbguolpbmgsgq6oby_Fran Almeidamusicas.csv");
                string _dataPath2 = Path.Combine("C://Users//fran//Documents//Docs//dataSpotify.csv");
                string _modelPath = Path.Combine("C://Users//fran//Documents//Docs//p1.zip");
                string _modelPath2 = Path.Combine("C://Users//fran//Documents//Docs//p2.zip");
                
                var model1 = PredictionModel.ReadAsync<Musica, ClusterPrediction>(_modelPath).Result;
                var model2 = PredictionModel.ReadAsync<Musica, ClusterPrediction>(_modelPath2).Result;
                var rec = spotifyService.geraplay(_dataPath, _dataPath2, model1, model2);
                rec.Nome = spotifyUser.DisplayName;
                rec.IdUsuario = spotifyUser.UserId;
                
                foreach (var musicasp1 in rec.P1)
                {
                    musicasp1.IdUsuario = spotifyUser.UserId;
                   // db.Musicas.Add(musicasp1);
                }
                foreach (var musicasp2 in rec.P2)
                {
                    musicasp2.IdUsuario = spotifyUser.UserId;
                    //db.Musicas.Add(musicasp2);
                }
                foreach (var musicasp3 in rec.P3)
                {
                    musicasp3.IdUsuario = spotifyUser.UserId;
                    //db.Musicas.Add(musicasp3);
                }

                // db.SaveChanges();
                TempData["rec"] = rec;
                return RedirectToAction("Index","Musicas");
             //   return RedirectToAction("Playlists", rec);
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