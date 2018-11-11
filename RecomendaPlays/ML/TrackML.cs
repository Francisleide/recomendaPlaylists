using Microsoft.ML.Runtime.Api;
using RecomendaPlays.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecomendaPlays.ML
{
    [Serializable]
    public class TrackML
    {
        
        public string NomeArtista { get; set; }
        public string Id { get; set; }
        [Column("1")]
        public double Speechiness { get; set; }
        [Column("2")]
        public double Liveness { get; set; }
        [Column("3")]
        public double Energy { get; set; }
        [Column("4")]
        public double Danceability { get; set; }
        public double Instrumentalness { get; set; }
        public double Loudness { get; set; }
        public double Tempo { get; set; }
        public string NomeMusica { get; set; }
    }
    public class ClusterPrediction
    {
        [ColumnName("PredictedLabel")]
        public uint PredictedClusterId;

        [ColumnName("Score")]
        public float[] Distances;
    }
}