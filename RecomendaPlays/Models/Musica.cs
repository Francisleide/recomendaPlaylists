using Microsoft.ML.Runtime.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecomendaPlays.Models
{
    public class Musica
    {
        public int MusicaId { get; set; }
        public String IdUsuario { get; set; }
        public int QualCluster { get; set; }
        public float Speechiness { get; set; }
        public float Liveness { get; set; }
        public float Energy { get; set; }
        public float Danceability { get; set; }
        public String Titulo { get; set; }
        public String Cantor { get; set; }
    }
   

    }
