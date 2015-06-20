using Encog;
using Encog.Engine.Network.Activation;
using Encog.ML.Data.Basic;
using Encog.Neural.Networks;
using Encog.Neural.Networks.Layers;
using Encog.Neural.Networks.Training.Propagation.Resilient;
using System;

namespace RedeNeural
{
    public class RedeIdentificadoraFormasGeometricas
    {
        public static double[][] XORInput = new[]
        {
            //retangulos
            new [] {0d,0d,1d,0d,1d,0d,1d},
            new [] {0d,0d,0d,1d,1d,0d,1d},
            new [] {0d,1d,0d,0d,1d,1d,0d},
            new [] {0d,0d,0d,0d,1d,0d,1d},
            
            //triangulos
            new [] {1d,1d,0d,1d,0d,0d,1d},

            //elipses

        };

        // resultado esperado
        public static double[][] XORIdeal = new[]
        {
            //retangulos
            new [] {0d, 1d, 0d},
            new [] {0d, 1d, 0d},
            new [] {0d, 1d, 0d},
            new [] {0d, 1d, 0d},
            
            //triangulos
            new [] {0d, 0d, 1d},
        };

        public void Processar()
        {
            var rede = new BasicNetwork();
            rede.AddLayer(new BasicLayer(null, hasBias: true, neuronCount: 7));
            rede.AddLayer(new BasicLayer(new ActivationLinear(), hasBias: true, neuronCount: 10)); //camada oculta
            rede.AddLayer(new BasicLayer(new ActivationLinear(), hasBias: false, neuronCount: 3));
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
            } while (treino.Error > 0.5d);
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
