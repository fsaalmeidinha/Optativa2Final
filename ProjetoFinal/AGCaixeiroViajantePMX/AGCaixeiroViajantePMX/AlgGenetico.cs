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
        private int numeroGeracoes = 50000;

        public Dictionary<string, int> QuantidadeAgenciasPorBanco { get; set; }
        private List<Agencia> agenciasMaisProximas;
        private double latitude, longitude;
        private List<CaixeiroViajante> caixeirosViajante;
        private double[,] matrizDistancias;
        private int quantidadeAgenciasSolicitadas;
        Random rand;

        public AlgGenetico(Dictionary<string, int> quantidadeAgenciasPorBanco, double latitude, double longitude)
        {
            rand = new Random();
            this.QuantidadeAgenciasPorBanco = quantidadeAgenciasPorBanco;
            this.latitude = latitude;
            this.longitude = longitude;

            quantidadeAgenciasSolicitadas = quantidadeAgenciasPorBanco.Sum(qa => qa.Value);
            agenciasMaisProximas = SelecionarAgenciasMaisProximas(quantidadeAgenciasPorBanco);
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
                        matrizDistancias[i, j] = agenciasMaisProximas[j + i - 1].Distancia;
                    else
                    {
                        //Distancia entre os bancos
                        double lat1 = agenciasMaisProximas[i].Latitude;
                        double long1 = agenciasMaisProximas[i].Longitude;
                        double lat2 = agenciasMaisProximas[j].Latitude;
                        double long2 = agenciasMaisProximas[j].Longitude;
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

        public List<Agencia> RecuperarRotaAgencias()
        {
            List<Agencia> roteiroAgencias = new List<Agencia>();
            for (int indCromossomo = 0; indCromossomo < quantidadeAgenciasSolicitadas; indCromossomo++)
            {
                //Utilizar a distancia...
                double distancia = matrizDistancias[indCromossomo, indCromossomo + 1];
                roteiroAgencias.Add(agenciasMaisProximas[MelhorIndividuo().Cromossomos[indCromossomo + 1]]);
            }
            return roteiroAgencias;
        }

        #region Métodos Privados

        private void GerarPopulacaoIndividuos()
        {
            int numeroIteracoesSemMudarFitness = 1;
            double melhorFitness = 0;
            for (int i = 0; i < numeroGeracoes; i++)
            {
                //Elitização
                CaixeiroViajante melhor = new CaixeiroViajante(matrizDistancias, MelhorIndividuo().Cromossomos);

                SelecaoPMX();
                Mutacao();

                int indPiorIndividuo = caixeirosViajante.IndexOf(PiorIndividuo());
                caixeirosViajante[indPiorIndividuo] = melhor;
                if (Math.Abs(melhorFitness - melhor.FuncaoObjetivo) > 0.1 * quantidadeAgenciasSolicitadas)
                {
                    melhorFitness = melhor.FuncaoObjetivo;
                    numeroIteracoesSemMudarFitness = 1;
                }
                else
                    numeroIteracoesSemMudarFitness++;
            }
        }

        private void Mutacao()
        {
            //Mutação de cada caixeiro
            for (int indCaixeiro = 0; indCaixeiro < numeroIndividuos; indCaixeiro++)
            {
                //Taxa de não haver mutação
                if (rand.Next(0, 100) == 0)
                {
                    continue;
                }

                int indiceMutacao1 = 0;
                int indiceMutacao2 = 0;
                switch (rand.Next(0, 3))
                {
                    case 0://Mutacao1
                        //Escolhe 2 cromossomos e os troca de posicao um com o outro
                        indiceMutacao1 = rand.Next(1, quantidadeAgenciasSolicitadas + 1);
                        indiceMutacao2 = rand.Next(1, quantidadeAgenciasSolicitadas + 1);
                        while (indiceMutacao2 == indiceMutacao1)
                            indiceMutacao2 = rand.Next(1, quantidadeAgenciasSolicitadas + 1);
                        int cromossomo1Aux = caixeirosViajante[indCaixeiro].Cromossomos[indiceMutacao1];
                        caixeirosViajante[indCaixeiro].Cromossomos[indiceMutacao1] = caixeirosViajante[indCaixeiro].Cromossomos[indiceMutacao2];
                        caixeirosViajante[indCaixeiro].Cromossomos[indiceMutacao2] = cromossomo1Aux;
                        break;

                    case 1:
                        //Inverte um bloco de cromossomos ex: 1,2,|3,4|,5,6 vira 1,2,4,3,5,6
                        indiceMutacao1 = rand.Next(1, quantidadeAgenciasSolicitadas + 1);
                        indiceMutacao2 = rand.Next(indiceMutacao1, quantidadeAgenciasSolicitadas + 1);
                        caixeirosViajante[indCaixeiro].Cromossomos.Reverse(indiceMutacao1, indiceMutacao2 - indiceMutacao1 + 1);
                        //List<int> valoresCromossomos = new List<int>();
                        //for (int i = indiceMutacao1; i <= indiceMutacao2; i++)
                        //{
                        //    valoresCromossomos.Add(caixeirosViajante[indCaixeiro].Cromossomos[i]);
                        //}
                        //int contador = 0;
                        //for (int i = indiceMutacao2; i >= indiceMutacao1; i--)
                        //{
                        //    caixeirosViajante[indCaixeiro].Cromossomos[i] = valoresCromossomos[contador++];
                        //}
                        break;

                    case 2:
                        //Da um shift a esquerda de um bloco de cromossomos ex: 1,2,3,|4,5|,6 vira 1,2,4,5,3,6
                        indiceMutacao1 = rand.Next(2, quantidadeAgenciasSolicitadas + 1);
                        indiceMutacao2 = rand.Next(indiceMutacao1, quantidadeAgenciasSolicitadas + 1);
                        for (int i = indiceMutacao1 - 1; i < indiceMutacao2; i++)
                        {
                            caixeirosViajante[indCaixeiro].Cromossomos.Reverse(i, 2);
                        }
                        break;
                    default:
                        throw new Exception("Mutação inválida");
                }

            }
        }

        private void SelecaoPMX()
        {
            for (int i = 0; i < numeroIndividuos - 1; i += 2)
            {
                int indiceCruzamento1 = rand.Next(1, quantidadeAgenciasSolicitadas + 1);
                int indiceCruzamento2 = rand.Next(indiceCruzamento1, quantidadeAgenciasSolicitadas + 1);

                //Cruzamentos de ida (do primeiro individuo para o segundo)
                List<int[]> cruzamentosIda = new List<int[]>();
                //Cruzamentos de volta (do segundo individuo para o primeiro)
                List<int[]> cruzamentosVolta = new List<int[]>();
                for (int k = indiceCruzamento1; k <= indiceCruzamento2; k++)
                {
                    cruzamentosIda.Add(new int[] { caixeirosViajante[i].Cromossomos[k], caixeirosViajante[i + 1].Cromossomos[k] });
                    cruzamentosVolta.Add(new int[] { caixeirosViajante[i + 1].Cromossomos[k], caixeirosViajante[i].Cromossomos[k] });
                    int cromoAuxPrimeiroIndv = caixeirosViajante[i].Cromossomos[k];
                    //Troca os cromossomos dos individuos
                    caixeirosViajante[i].Cromossomos[k] = caixeirosViajante[i + 1].Cromossomos[k];
                    caixeirosViajante[i + 1].Cromossomos[k] = cromoAuxPrimeiroIndv;
                }

                for (int indCromossomo = 1; indCromossomo <= quantidadeAgenciasSolicitadas; indCromossomo++)
                {
                    if (indCromossomo >= indiceCruzamento1 && indCromossomo <= indiceCruzamento2)
                        continue;

                    if (cruzamentosVolta.Any(cruz => cruz[0] == caixeirosViajante[i].Cromossomos[indCromossomo]))
                    {
                        caixeirosViajante[i].Cromossomos[indCromossomo] = cruzamentosVolta.First(cruz => cruz[0] == caixeirosViajante[i].Cromossomos[indCromossomo])[1];
                    }

                    if (cruzamentosIda.Any(cruz => cruz[0] == caixeirosViajante[i + 1].Cromossomos[indCromossomo]))
                    {
                        caixeirosViajante[i + 1].Cromossomos[indCromossomo] = cruzamentosIda.First(cruz => cruz[0] == caixeirosViajante[i + 1].Cromossomos[indCromossomo])[1];
                    }
                }
            }
        }

        private List<Agencia> SelecionarAgenciasMaisProximas(Dictionary<string, int> quantidadeAgenciasPorBanco)
        {
            List<Agencia> agencias = Agencia.PegarTodas(latitude, longitude);
            ValidarAgenciasSolicitadas(quantidadeAgenciasPorBanco, agencias);
            List<Agencia> agenciasMaisProximas = new List<Agencia>();
            foreach (KeyValuePair<string, int> qtdAgencias in quantidadeAgenciasPorBanco)
            {
                //Ordena as agencias pela distancia e pega a quantidade necessária
                agenciasMaisProximas.AddRange(agencias.Where(agencia => agencia.Nome.ToUpper() == qtdAgencias.Key.ToUpper()).OrderBy(ag => ag.Distancia).Take(qtdAgencias.Value));
            }
            return agenciasMaisProximas;
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

        private CaixeiroViajante MelhorIndividuo()
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

        private CaixeiroViajante PiorIndividuo()
        {
            CaixeiroViajante piorIndividuo = caixeirosViajante[0];

            for (int i = 1; i < numeroIndividuos; i++)
            {
                if (caixeirosViajante[i].FuncaoObjetivo < piorIndividuo.FuncaoObjetivo)
                {
                    piorIndividuo = caixeirosViajante[i];
                }
            }

            return piorIndividuo;
        }

        #endregion Métodos Privados
    }
}
