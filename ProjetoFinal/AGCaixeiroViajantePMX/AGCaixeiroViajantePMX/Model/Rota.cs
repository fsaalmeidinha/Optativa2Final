using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AGCaixeiroViajantePMX.Model
{
    public class Rota
    {
        public Rota()
        {

        }

        public Rota(double distancia, string destino, double latitude, double longitude)
        {
            this.Distancia = distancia;
            this.Destino = destino;
            this.Latitude = latitude;
            this.Longitude = longitude;
        }

        public double Distancia { get; set; }
        public string Destino { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
