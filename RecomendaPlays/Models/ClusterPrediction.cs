using Microsoft.ML.Runtime.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecomendaPlays.Models
{
    public class ClusterPrediction
    {
        [ColumnName("PredictedLabel")]
        public uint PredictedClusterId;
        [ColumnName("Score")]
        public float[] Distances;
    }
    public class Result
    {
        public ClusterPrediction ClusterPrediction { get; set; }
        public Musica Musica { get; set; }
    }

}