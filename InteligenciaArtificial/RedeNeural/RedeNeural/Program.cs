using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Encog;
using Encog.Engine.Network.Activation;
using Encog.ML.Data.Basic;
using Encog.Neural.Networks;
using Encog.Neural.Networks.Layers;
using Encog.Neural.Networks.Training.Propagation.Resilient;
using Encog.Neural.Networks.Training.Simple;

namespace RedeNeural
{
    // Referências:
    // bias: http://stackoverflow.com/questions/2480650/role-of-bias-in-neural-networks
    class Program
    {
        public static double[][] XORInput = new[]
        {
            new [] {0d, 0d},
            new [] {1d, 0d},
            new [] {1d, 1d},
            new [] {0d, 1d}
        };

        // resultado esperado
        public static double[][] XORIdeal = new[]
        {
            new [] {0d},
            new [] {1d},
            new [] {1d},
            new [] {0d}
        };

        static void Main(string[] args)
        {
            var rede = new BasicNetwork();
            rede.AddLayer(new BasicLayer(null, hasBias:true, neuronCount:2));
            rede.AddLayer(new BasicLayer(new ActivationLinear(), hasBias:true, neuronCount:3)); //camada oculta
            rede.AddLayer(new BasicLayer(new ActivationLinear(), hasBias:false, neuronCount:1));
            rede.Structure.FinalizeStructure();

            rede.Reset();

            // dataset de treinamento
            var treinamento = new BasicMLDataSet(XORInput, XORIdeal);

            // treinando a rede
            var treino = new ResilientPropagation(rede, treinamento);

            var epoca = 1;
            do
            {
                treino.Iteration();
                Console.WriteLine("Época: {0}, Taxa de Erro: {1}", epoca, treino.Error);

                epoca++;
            } while (treino.Error > 0.01d);
            treino.FinishTraining();

            // testando a rede 
            Console.WriteLine("Resultados:");
            foreach (var par in treinamento)
            {
                var saida = rede.Compute(par.Input);
                Console.WriteLine("Entrada: {0} XOR {1}, Saída: {2}, Ideal: {3}", 
                    par.Input[0], 
                    par.Input[1], 
                    saida, 
                    par.Ideal[0]);
            }

            EncogFramework.Instance.Shutdown();
            Console.Read();
        }
    }
}
