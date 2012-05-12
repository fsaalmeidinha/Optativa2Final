using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AGCaixeiroViajantePMX.Model;

namespace AGCaixeiroViajantePMX
{
    public class AlgGenetico
    {
        private int numeroIndividuos = 100;
        private int numeroGeracoes = 10000;

        public Dictionary<string, int> QuantidadeAgenciasPorBanco { get; set; }
        private List<Agencia> bancosMaisProximos;
        private double latitude, longitude;
        private List<CaixeiroViajante> caixeirosViajante;
        private double[,] matrizDistancias;
        private int quantidadeAgenciasSolicitadas;

        public AlgGenetico(Dictionary<string, int> quantidadeAgenciasPorBanco, double latitude, double longitude)
        {
            this.QuantidadeAgenciasPorBanco = quantidadeAgenciasPorBanco;
            this.latitude = latitude;
            this.longitude = longitude;

            quantidadeAgenciasSolicitadas = quantidadeAgenciasPorBanco.Sum(qa => qa.Value);
            bancosMaisProximos = SelecionarAgenciasMaisProximas(quantidadeAgenciasPorBanco);
            matrizDistancias = new double[quantidadeAgenciasSolicitadas + 1, quantidadeAgenciasSolicitadas + 1];
            //Seta as distancias do individuo à cada uma das agencias
            for (int i = 0; i < quantidadeAgenciasSolicitadas + 1; i++)
            {
                for (int j = 0; j < quantidadeAgenciasSolicitadas + 1; j++)
                {
                    if (i == j)
                        matrizDistancias[i, j] = 0;
                    else if (i == 0 || j == 0)
                        //Distancias entre o individuo e a agencia
                        matrizDistancias[i, j] = bancosMaisProximos[j + i - 1].Distancia;
                    else
                    {
                        //Distancia entre os bancos
                        double lat1 = bancosMaisProximos[i].Latitude;
                        double long1 = bancosMaisProximos[i].Longitude;
                        double lat2 = bancosMaisProximos[j].Latitude;
                        double long2 = bancosMaisProximos[j].Longitude;
                        double distanciaBancos = Distancia.CalcularDistanciaKM(lat1, long1, lat2, long2);
                        matrizDistancias[i, j] = distanciaBancos;
                    }
                }
            }

            caixeirosViajante = new List<CaixeiroViajante>();
            for (int i = 0; i < numeroIndividuos; i++)
            {
                caixeirosViajante.Add(new CaixeiroViajante(matrizDistancias));
            }

            GerarPopulacaoIndividuos();
        }

        private void GerarPopulacaoIndividuos()
        {
            for (int i = 0; i < numeroGeracoes; i++)
            {
                CaixeiroViajante melhor = new CaixeiroViajante(matrizDistancias, MelhorIndividuo().Cromossomos);
                SelecaoPMX();
            }
        }

        private void SelecaoPMX()
        {
            Random rand = new Random();
            int indiceMutacao1 = rand.Next(1, quantidadeAgenciasSolicitadas + 1);
            int indiceMutacao2 = rand.Next(indiceMutacao1, quantidadeAgenciasSolicitadas + 1);

            for (int i = 0; i < numeroIndividuos - 1; i += 2)
            {
                //List<int[]> mutacoes = new List<int[]>();
                //for (int k = indiceMutacao1; k <= indiceMutacao2; k++)
                //{
                //    mutacoes.Add(new int[] { caixeirosViajante[i].Cromossomos[k], caixeirosViajante[i + 1].Cromossomos[k] });
                //    mutacoes.Add(new int[] { caixeirosViajante[i + 1].Cromossomos[k], caixeirosViajante[i].Cromossomos[k] });
                //}
                //for (int j = 1; j <= quantidadeAgenciasSolicitadas; j++)
                //{
                //    int[] mutacao = mutacoes.FirstOrDefault(mut => mut[0] == caixeirosViajante[i].Cromossomos[j]);
                //    if (mutacao != null)
                //    {
                //        caixeirosViajante[i].Cromossomos[j] = mutacao[1];
                //    }

                //    mutacao = mutacoes.FirstOrDefault(mut => mut[0] == caixeirosViajante[i + 1].Cromossomos[j]);
                //    if (mutacao != null)
                //    {
                //        caixeirosViajante[i + 1].Cromossomos[j] = mutacao[1];
                //    }
                //}
                //for (int j = indiceMutacao1; j <= indiceMutacao2; j++)
                //{
                //    mutacoes.Add(caixeirosViajante[)
                //}
                //}
            }

        }

        private List<Agencia> SelecionarAgenciasMaisProximas(Dictionary<string, int> quantidadeAgenciasPorBanco)
        {
            List<Agencia> bancos = Agencia.PegarTodas(latitude, longitude);
            ValidarAgenciasSolicitadas(quantidadeAgenciasPorBanco, bancos);
            List<Agencia> bancosMaisProximos = new List<Agencia>();
            foreach (KeyValuePair<string, int> qtdAgencias in quantidadeAgenciasPorBanco)
            {
                bancosMaisProximos.AddRange(bancos.Where(banco => banco.Nome.ToUpper() == qtdAgencias.Key.ToUpper()).Take(qtdAgencias.Value));
            }
            return bancosMaisProximos;
        }

        private void ValidarAgenciasSolicitadas(Dictionary<string, int> quantidadeAgenciasPorBanco, List<Agencia> bancos)
        {
            if (quantidadeAgenciasPorBanco == null || quantidadeAgenciasPorBanco.Count == 0 || quantidadeAgenciasPorBanco.Any(qb => qb.Value < 0))
                throw new Exception("Agencias informadas invalidamente");

            foreach (KeyValuePair<string, int> quantidadeBanco in quantidadeAgenciasPorBanco)
            {
                //Verifica se existe a quantidade informada de agencias para o banco
                if (bancos.Count(banco => banco.Nome.ToUpper() == quantidadeBanco.Key.ToUpper()) < quantidadeBanco.Value)
                    throw new Exception(String.Format("Não existe a quantidade de agências do banco {0} solicitada.", quantidadeBanco.Key));
            }
        }

        public CaixeiroViajante MelhorIndividuo()
        {
            CaixeiroViajante melhorIndividuo = caixeirosViajante[0];

            for (int i = 1; i < numeroIndividuos; i++)
            {
                if (caixeirosViajante[i].FuncaoObjetivo > melhorIndividuo.FuncaoObjetivo)
                {
                    melhorIndividuo = caixeirosViajante[i];
                }
            }

            return melhorIndividuo;
        }

    }
}
