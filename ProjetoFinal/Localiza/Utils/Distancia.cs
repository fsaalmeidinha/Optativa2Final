using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils
{
    public class Distancia
    {
        public static double CalcularDistanciaKM(double lat1, double lon1, double lat2, double lon2)
        {
            double raioTerra = 6378;
            double lat1R = lat1 / 180 * Math.PI;
            double lat2R = lat2 / 180 * Math.PI;
            double lon1R = lon1 / 180 * Math.PI;
            double lon2R = lon2 / 180 * Math.PI;
            double firstBlock = Math.Sin(lat1R) * Math.Sin(lat2R);
            double secondBlock = Math.Cos(lat1R) * Math.Cos(lat2R) * Math.Cos(lon2R - lon1R);
            return raioTerra * (Math.Acos(firstBlock + secondBlock));
        }
    }
}
