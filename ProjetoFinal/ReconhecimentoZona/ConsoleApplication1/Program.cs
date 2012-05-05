using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReconhecimentoZonaRN;
using NeuronDotNet.Core.Backpropagation;
using ReconhecimentoZonaRN.Model;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {

            ReconhecimentoZonaRN rn = new ReconhecimentoZonaRN();

            List<ReconhecimentoZonaRN.Model.Banco> bancos = ReconhecimentoZonaRN.Model.Banco.PegarTodos();

            int acertos = 0;
            foreach (Banco banco in bancos)
            {
                string zona = rn.IdentificarZona(banco.Latitude, banco.Longitude);
                if (zona == banco.Zona)
                    acertos++;
            }
        }
    }
}
