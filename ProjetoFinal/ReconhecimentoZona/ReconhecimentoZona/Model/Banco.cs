using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReconhecimentoZonaRN.Model
{
    public class Banco
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Zona { get; set; }

        public static List<Banco> PegarTodos()
        {
            return CsvImport.LerBancos();
        }

    }
}
