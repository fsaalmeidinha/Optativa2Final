using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReconhecimentoZonaRN.Model;
using NeuronDotNet.Core.Backpropagation;
using NeuronDotNet.Core;
using NeuronDotNet.Core.Initializers;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace ReconhecimentoZonaRN
{
    public class ReconhecimentoZonaRN
    {
        Network redeNeural = null;

        const double minLatitude = 45;
        const double maxLatitude = 47;
        const double minLongitude = 22;
        const double maxLongitude = 24;
        string nomeRede = "RedeReconhecimentoZona";

        public ReconhecimentoZonaRN()
        {
            redeNeural = RecuperarRede();
            if (redeNeural == null)
            {
                redeNeural = Treinar(4, 0.25, 10000);
            }
        }

        private BackpropagationNetwork Treinar(int numeroNeuronios, double taxaAprendizado, int ciclos)
        {
            List<Banco> bancos = Banco.PegarTodos();

            List<KeyValuePair<double[], double[]>> dadosEntrada = new List<KeyValuePair<double[], double[]>>();
            foreach (Banco banco in bancos)
            {
                KeyValuePair<double[], double[]> kvp = new KeyValuePair<double[], double[]>(new double[] { NormalizarLatitude(banco.Latitude), NormalizarLongitude(banco.Longitude) }, ConverterZonaParaArrayDouble(banco.Zona));
                dadosEntrada.Add(kvp);
            }

            BackpropagationNetwork network;
            //int numeroNeuronios = 4;
            //double taxaAprendizado = 0.25d;

            ActivationLayer inputLayer = new LinearLayer(2);
            ActivationLayer hiddenLayer = new SigmoidLayer(numeroNeuronios);
            ActivationLayer outputLayer = new SigmoidLayer(4);
            new BackpropagationConnector(inputLayer, hiddenLayer).Initializer = new RandomFunction(0d, 0.3d);
            new BackpropagationConnector(hiddenLayer, outputLayer).Initializer = new RandomFunction(0d, 0.3d);
            network = new BackpropagationNetwork(inputLayer, outputLayer);
            network.SetLearningRate(taxaAprendizado);

            TrainingSet trainingSet = new TrainingSet(2, 4);
            foreach (KeyValuePair<double[], double[]> kvp in dadosEntrada)
            {
                trainingSet.Add(new TrainingSample(kvp.Key, kvp.Value));
            }

            network.EndEpochEvent += new TrainingEpochEventHandler(
                delegate(object senderNetwork, TrainingEpochEventArgs argsNw)
                {
                    //trainingProgressBar.Value = (int)(argsNw.TrainingIteration * 100d / cycles);
                    //Application.DoEvents();
                });

            //network.Learn(trainingSet, ciclos);
            int cicloAtual = ciclos / 5;
            int acertos = 0;
            while (cicloAtual <= ciclos && acertos / bancos.Count < 0.99)
            {
                network.Learn(trainingSet, cicloAtual);

                acertos = 0;
                foreach (Banco banco in bancos)
                {
                    double[] input = new double[] { NormalizarLatitude(banco.Latitude), NormalizarLongitude(banco.Longitude) };
                    double[] resultado = network.Run(input);
                    if (ConverterArrayDoubleParaZona(resultado) != banco.Zona)
                        trainingSet.Add(new TrainingSample(input, ConverterZonaParaArrayDouble(banco.Zona)));
                    else
                        acertos++;
                }

                cicloAtual += ciclos / 5;
            }

            //using (Stream stream = File.Open(System.IO.Directory.GetCurrentDirectory() + "\\" + nomeRedeNeural + ".ndn", FileMode.Create))
            using (Stream stream = File.Open("c:\\ReconhecimentoZonaRN\\" + nomeRede + ".ndn", FileMode.Create))
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, network);
            }

            return network;
        }

        public string IdentificarZona(double latitude, double longitude)
        {
            double[] resultado = redeNeural.Run(new double[] { NormalizarLatitude(latitude), NormalizarLongitude(longitude) });
            return ConverterArrayDoubleParaZona(resultado);
        }

        #region Normalização

        private double NormalizarLatitude(double latitude)
        {
            return (Math.Abs(latitude) - minLatitude) / (maxLatitude - minLatitude);
        }

        private double NormalizarLongitude(double longitude)
        {
            return (Math.Abs(longitude) - minLongitude) / (maxLongitude - minLongitude);
        }

        private List<double> NormalizarDadosLatitude(List<double> latitudes)
        {
            return latitudes.ConvertAll(lat => NormalizarLatitude(lat));
        }

        private List<double> NormalizarDadosLongitude(List<double> longitudes)
        {
            return longitudes.ConvertAll(lat => NormalizarLongitude(lat));
        }

        #endregion Normalização

        private double[] ConverterZonaParaArrayDouble(string zona)
        {
            switch (zona)
            {
                case "Zona Norte":
                    return new double[] { 1, 0, 0, 0 };
                case "Centro":
                    return new double[] { 0, 1, 0, 0 };
                case "Zona Leste":
                    return new double[] { 0, 0, 1, 0 };
                case "Zona Sul":
                    return new double[] { 0, 0, 0, 1 };
                default:
                    throw new Exception();
            }
        }

        private string ConverterArrayDoubleParaZona(double[] output)
        {
            int indexMax = output.ToList().IndexOf(output.Max());

            switch (indexMax)
            {
                case 0:
                    return "Zona Norte";
                case 1:
                    return "Centro";
                case 2:
                    return "Zona Leste";
                case 3:
                    return "Zona Sul";
                default:
                    throw new Exception();
            }
        }

        private Network RecuperarRede()
        {
            if (!File.Exists("c:\\ReconhecimentoZonaRN\\" + nomeRede + ".ndn"))
                return null;

            using (Stream stream = File.Open("c:\\ReconhecimentoZonaRN\\" + nomeRede + ".ndn", FileMode.Open))
            {
                IFormatter formatter = new BinaryFormatter();
                return (Network)formatter.Deserialize(stream);
            }
        }
    }
}
