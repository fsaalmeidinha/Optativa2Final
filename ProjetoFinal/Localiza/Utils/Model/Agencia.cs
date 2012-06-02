using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils.Model
{
    public class Agencia
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Zona { get; set; }
        public double Distancia { get; set; }

        public static List<Agencia> PegarTodas()
        {
            return CsvImport.LerBancos();
        }

        public static List<Agencia> PegarTodas(double latitude, double longitude)
        {
            List<Agencia> agencias = CsvImport.LerBancos();
            agencias.ForEach(banco => banco.Distancia = Utils.Distancia.CalcularDistanciaKM(latitude, longitude, banco.Latitude, banco.Longitude));
            return agencias;
        }
    }
}
