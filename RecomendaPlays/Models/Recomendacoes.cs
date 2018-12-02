using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecomendaPlays.Models
{
    public class Recomendacoes
    {
        public int RecomendacoesId { get; set; }
        public List<Musica> P1 { get; set; }
        public List<Musica> P2 { get; set; }
        public List<Musica> P3 { get; set; }
        public string Nome { get; set; }
        public string IdUsuario { get; set; }
    }
}