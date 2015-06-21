using RedeNeural.Core.Classificacao;
using RedeNeural.Core.Classificacao.Saidas;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace RedeNeural.Core
{
    public class RedeIdentificadoraFormasGeometricas
    {
        //private double[][] inputsParaTreinamento;
        //private double[][] saidasIdeais;
        
        public RedeIdentificadoraFormasGeometricas(DiretorioTreinamento diretorioTreinamento)
        {
            var resultadosIdeais = new List<ResultadoIdeal>();
            using (var stringReader = File.OpenText(diretorioTreinamento.CaminhoArquivoTreinamento))
            {
                while (!stringReader.EndOfStream)
                {
                    var linha = stringReader.ReadLine();
                    if (linha == null) continue;

                    var par = linha.Split('=');
                    var bits = par[0]
                        .Split(',')
                        .Select(v => Convert.ToInt32(v))
                        .ToArray();
                    var classeEsperada = (ClasseGeometrica)Convert.ToInt32(linha[1]);

                    resultadosIdeais.Add(new ResultadoIdeal(bits, classeEsperada));
                }
            }

            resultadosIdeais.Shuffle();


        }

        public void Treinar()
        {
            
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
