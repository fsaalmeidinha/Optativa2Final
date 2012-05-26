using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils;
using AGCaixeiroViajantePMX.Model;

namespace Localiza
{
    class Program
    {
        static void Main(string[] args)
        {
            Coordenadas cord = GoogleGeoCode.GetCoordenadas("Avenida Maranhão, 235 - Parque Paraíso - Itapecerica da Serra/SP");
            // cord = GoogleGeoCode.GetCoordenadas("Rua Brigadeiro Araujo, 76 - Freguesia do Ó - Sao Paulo/SP");

            Console.WriteLine("Latitude:{0}, Longitude:{1}", cord.Latitude, cord.Longitude);
            //Console.Read();

            Dictionary<string, int> qtdAgenciasPorBanco = new Dictionary<string, int>();
            qtdAgenciasPorBanco.Add("Banco do Brasil", 1);
            qtdAgenciasPorBanco.Add("Caixa Economica Federal", 1);

            AGCaixeiroViajantePMX.AlgGenetico algGenetico = new AGCaixeiroViajantePMX.AlgGenetico(qtdAgenciasPorBanco, (double)cord.Latitude, (double)cord.Longitude);
            List<Rota> rota = algGenetico.RecuperarRotaAgencias();

            StringBuilder roteiro = new StringBuilder("Voce deve:");
            foreach (Rota rotaAtual in rota)
            {
                roteiro.AppendFormat("\n ir a \"{0}\" que fica na latitude: {1} e longitude: {2}, percorrendo {3} KM,", rotaAtual.Destino, rotaAtual.Latitude, rotaAtual.Longitude, rotaAtual.Distancia);
            }

            Console.WriteLine(roteiro.ToString());

        }
    }
}
