﻿using Encog;
using Encog.Engine.Network.Activation;
using Encog.ML.Data;
using Encog.ML.Data.Basic;
using Encog.Neural.Networks;
using Encog.Neural.Networks.Training.Propagation.Resilient;
using Encog.Neural.Pattern;
using RedeNeural.Core.Classificacao;
using RedeNeural.Core.Classificacao.Entradas;
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
		private BasicNetwork rede;

		public RedeIdentificadoraFormasGeometricas(DiretorioTreinamento diretorioTreinamento)
		{
			this.diretorioTreinamento = diretorioTreinamento;
			CarregarResultadosIdeais(diretorioTreinamento);

			var pattern = new FeedForwardPattern
			{
				InputNeurons = GeradorValoresRede.NumeroBits, 
				OutputNeurons = 3,
				ActivationFunction = new ActivationSigmoid()
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
			var treino = new ResilientPropagation(rede, treinamento);

			var epoca = 1;
			do
			{
				treino.Iteration();
				Console.WriteLine("Treinando, Epoch #" + epoca + ", Error: " + treino.Error);
				epoca++;
			}
			while (epoca < 3000 && treino.Error > 0.001);

			var persistBasicNetwork = new PersistBasicNetwork();
			using (var stream = File.Create(ObterCaminhoArquivoPersistencia(diretorioTreinamento)))
			{
				persistBasicNetwork.Save(stream, rede);
			}
		}
		
		public static ClasseGeometrica Computar(int[] bitsEntrada, DiretorioTreinamento diretorioTreinamento)
		{
			var redeIdentificadoraFormasGeometricas = new RedeIdentificadoraFormasGeometricas(diretorioTreinamento);

			var persistCpn = new PersistBasicNetwork();
			using (var stream = File.Open(ObterCaminhoArquivoPersistencia(diretorioTreinamento), FileMode.Open))
			{
				redeIdentificadoraFormasGeometricas.rede = (BasicNetwork)persistCpn.Read(stream);
			}

			var input = new BasicMLData(bitsEntrada.Select(Convert.ToDouble).ToArray());
			var output = redeIdentificadoraFormasGeometricas.rede.Compute(input);

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

		private double[] MontarArrayResultado(ClasseGeometrica classe)
		{
			switch (classe)
			{
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
