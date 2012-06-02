using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NeuronDotNet.Core.Backpropagation;
using Utils.Model;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {

            ReconhecimentoZonaRN.ReconhecimentoZonaRN rn = new ReconhecimentoZonaRN.ReconhecimentoZonaRN();

            List<Agencia> agencias = Agencia.PegarTodas();

            int acertos = 0;
            foreach (Agencia agencia in agencias)
            {
                string zona = rn.IdentificarZona(agencia.Latitude, agencia.Longitude);
                if (zona == agencia.Zona)
                    acertos++;
            }
        }
    }
}
