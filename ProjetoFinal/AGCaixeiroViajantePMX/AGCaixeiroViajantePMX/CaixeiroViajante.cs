using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AGCaixeiroViajantePMX.Model;

namespace AGCaixeiroViajantePMX
{
    public class CaixeiroViajante
    {
        //private int tamCromossomo { get { return QuantidadeAgenciasPorBanco.Sum(qtd => qtd.Value); } }
        public List<int> Cromossomos { get; set; }
        private double[,] matrizDistancias;
        private int numeroCromossomos;
        public double FuncaoObjetivo { get { return CalcularFuncaoObjetivo(); } }

        public CaixeiroViajante(double[,] matrizDistancias)
        {
            numeroCromossomos = matrizDistancias.GetLength(0);
            this.matrizDistancias = matrizDistancias;
            Cromossomos = InicializarAleatorio();
        }

        public CaixeiroViajante(double[,] matrizDistancias, List<int> cromossomos)
        {
            numeroCromossomos = matrizDistancias.GetLength(0);
            this.matrizDistancias = matrizDistancias;
            Cromossomos = cromossomos;
        }

        private List<int> InicializarAleatorio()
        {
            List<int> cromossomoAux = new List<int>();

            for (int i = 1; i < numeroCromossomos; i++)
            {
                cromossomoAux.Add(i);
            }
            cromossomoAux.Sort();
            cromossomoAux.Insert(0, 0);
            return cromossomoAux;
        }

        private double CalcularFuncaoObjetivo()
        {
            double distancia = 0;
            for (int i = 0; i < numeroCromossomos - 1; i++)
            {
                distancia += matrizDistancias[Cromossomos[i], Cromossomos[i + 1]];
            }
            distancia += matrizDistancias[Cromossomos[numeroCromossomos - 1], 0];
            return distancia;
        }
    }
}
