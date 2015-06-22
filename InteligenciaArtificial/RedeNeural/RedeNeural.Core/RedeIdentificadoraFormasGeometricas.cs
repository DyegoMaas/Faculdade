using Encog;
using Encog.Engine.Network.Activation;
using Encog.ML.Data;
using Encog.ML.Data.Basic;
using Encog.Neural.CPN;
using Encog.Neural.CPN.Training;
using Encog.Neural.Networks;
using Encog.Neural.Networks.Training.Propagation.Resilient;
using Encog.Neural.Pattern;
using RedeNeural.Core.Classificacao;
using RedeNeural.Core.Classificacao.Saidas;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace RedeNeural.Core
{
	public class RedeIdentificadoraFormasGeometricas : IDisposable
	{
		private readonly DiretorioTreinamento diretorioTreinamento;
		private readonly IList<ResultadoIdeal> resultadosIdeais = new List<ResultadoIdeal>();
		//private CPNNetwork redeCPN;
		private BasicNetwork rede;

		//https://www.wikiwand.com/en/Counterpropagation_network
		public RedeIdentificadoraFormasGeometricas(DiretorioTreinamento diretorioTreinamento)
		{
			this.diretorioTreinamento = diretorioTreinamento;
			CarregarResultadosIdeais(diretorioTreinamento);
			//redeCPN = new CPNNetwork(theInputCount: 7, theInstarCount: resultadosIdeais.Count, theOutstarCount: 3, theWinnerCount: 1);
			//rede = new BasicNetwork();
			//rede.AddLayer(new BasicLayer(null, true, 7));
			//rede.AddLayer(new BasicLayer(new ActivationSigmoid(), true, 10));
			//rede.AddLayer(new BasicLayer(new ActivationSigmoid(), false, 3));
			//rede.Structure.FinalizeStructure();

			var pattern = new FeedForwardPattern
			{
				InputNeurons = 7, 
				OutputNeurons = 3,
                ActivationFunction = new ActivationSigmoid()//ActivationSteepenedSigmoid() //sigmoed
			};
			pattern.AddHiddenLayer(10);
			rede = (BasicNetwork)pattern.Generate();
			rede.Reset();
		}

		private void CarregarResultadosIdeais(DiretorioTreinamento diretorioTreinamento)
		{
			using (var stringReader = diretorioTreinamento.ArquivoTreinamento.OpenText())
			{
				while (!stringReader.EndOfStream)
				{
					var linha = stringReader.ReadLine();
					if (linha == null) continue;

					var par = linha.Split('=');
					var bits = par[0].Select(c => new string(c, 1))
						.Select(s => Convert.ToInt32(s))
						.ToArray();
					var classeEsperada = (ClasseGeometrica)Convert.ToInt32(par[1]);

					resultadosIdeais.Add(new ResultadoIdeal(bits, classeEsperada));
				}
			}

			resultadosIdeais.Shuffle();
		}

		public void Treinar()
		{
			var inputsParaTreinamento = new double[resultadosIdeais.Count][];
			var saidasIdeais = new double[resultadosIdeais.Count][];

			for (int i = 0; i < resultadosIdeais.Count; i++)
			{
				inputsParaTreinamento[i] = resultadosIdeais[i].Bits.Select(Convert.ToDouble).ToArray();
				saidasIdeais[i] = MontarArrayResultado(resultadosIdeais[i].Classe);
			}

			var treinamento = new BasicMLDataSet(inputsParaTreinamento, saidasIdeais);
			TreinarX(treinamento);
			//TreinarInstar(redeCPN, treinamento);
			//TreinarOutstar(redeCPN, treinamento);

			var persistBasicNetwork = new PersistBasicNetwork();
			using (var stream = File.Create(ObterCaminhoArquivoPersistencia(diretorioTreinamento)))
			{
				persistBasicNetwork.Save(stream, rede);
			}
			//var persistCpn = new PersistCPN();
			//using (var stream = File.Create(ObterCaminhoArquivoPersistencia(diretorioTreinamento)))
			//{
			//    persistCpn.Save(stream, redeCPN);
			//}
		}

		private void TreinarX(BasicMLDataSet treinamento)
		{
			var epoca = 1;
			var treino = new ResilientPropagation(rede, treinamento);
			
			do
			{
				treino.Iteration();
				Console.WriteLine("Treinando, Epoch #" + epoca + ", Error: " + treino.Error);
				epoca++;
			}
			while (epoca < 10000 && treino.Error > 0.001);
		}

		public static ClasseGeometrica Computar(int[] bitsEntrada, DiretorioTreinamento diretorioTreinamento)
		{
			var redeIdentificadoraFormasGeometricas = new RedeIdentificadoraFormasGeometricas(diretorioTreinamento);

			var persistCpn = new PersistBasicNetwork();
			using (var stream = File.Open(ObterCaminhoArquivoPersistencia(diretorioTreinamento), FileMode.Open))
			{
				redeIdentificadoraFormasGeometricas.rede = (BasicNetwork)persistCpn.Read(stream);
			}
			//var persistCpn = new PersistCPN();
			//using (var stream = File.Open(ObterCaminhoArquivoPersistencia(diretorioTreinamento), FileMode.Open))
			//{
			//    redeIdentificadoraFormasGeometricas.redeCPN = (CPNNetwork)persistCpn.Read(stream);
			//}

			var input = new BasicMLData(bitsEntrada.Select(Convert.ToDouble).ToArray());
			var output = redeIdentificadoraFormasGeometricas.rede.Compute(input);
			//var output = redeIdentificadoraFormasGeometricas.redeCPN.Compute(input);

			var classe = redeIdentificadoraFormasGeometricas.ObterClasse(output);
			return classe;
		}

		private static string ObterCaminhoArquivoPersistencia(DiretorioTreinamento diretorioTreinamento)
		{
			return Path.Combine(diretorioTreinamento.DadosTreinamento.FullName, "rede.dat");
		}

		private ClasseGeometrica ObterClasse(IMLData output)
		{
			if (output[0] > output[1] && output[0] > output[2]) return ClasseGeometrica.Elipse;
			if (output[2] > output[0] && output[2] > output[1]) return ClasseGeometrica.Triangulo;
			return ClasseGeometrica.Retangulo;
		}

		private void TreinarInstar(CPNNetwork network, IMLDataSet training) {
			var epoca = 1;
			var treino = new TrainInstar(network, training, 0.1, theInitWeights:true);
			do 
			{
				treino.Iteration();
				Console.WriteLine("Training instar, Epoch #" + epoca + ", Error: "+ treino.Error);
				epoca++;
			}
			while (treino.Error > 0.05);
		}

		//https://www.wikiwand.com/en/Outstar
		private void TreinarOutstar(CPNNetwork network, IMLDataSet training)
		{
			var epoca = 1;
			var treino = new TrainOutstar(network, training, 0.1);
			do
			{
				treino.Iteration();
				Console.WriteLine("Training outstar, Epoch #" + epoca + ", Error: " + treino.Error);
				epoca++;
			}
			while (treino.Error > 0.05);
		}

		private double[] MontarArrayResultado(ClasseGeometrica classe)
		{
			switch (classe)
			{
                //case ClasseGeometrica.Elipse: return new[] { 1d, -1d, -1d };
                //case ClasseGeometrica.Retangulo: return new[] { -1d, 1d, -1d };
                //default: return new[] { -1d, -1d, 1d };
                case ClasseGeometrica.Elipse: return new[] { 1d, 0d, 0d };
                case ClasseGeometrica.Retangulo: return new[] { 0d, 1d, 0d };
                default: return new[] { 0d, 0d, 1d };
			}
		}

		public void Dispose()
		{
			EncogFramework.Instance.Shutdown();
		}
	}

	internal static class ListExtensions
	{
		public static void Shuffle<T>(this IList<T> list)
		{
			var provider = new RNGCryptoServiceProvider();
			int n = list.Count;
			while (n > 1)
			{
				byte[] box = new byte[1];
				do provider.GetBytes(box);
				while (!(box[0] < n * (Byte.MaxValue / n)));
				int k = (box[0] % n);
				n--;
				T value = list[k];
				list[k] = list[n];
				list[n] = value;
			}
		}
	}
}
