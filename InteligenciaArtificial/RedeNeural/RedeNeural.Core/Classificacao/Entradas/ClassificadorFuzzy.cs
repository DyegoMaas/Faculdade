using System;
using System.Collections.Generic;

namespace RedeNeural.Core.Classificacao.Entradas
{
    public class ClassificadorFuzzy
    {
        private const int NaoClassificado = -1;
        private readonly List<Func<int, int>> funcoesClassificacao = new List<Func<int, int>>();

        private ClassificadorFuzzy()
        {
        }

        public static ClassificadorFuzzy Abordagem1()
        {
            var classificadorFuzzy = new ClassificadorFuzzy();
            classificadorFuzzy.Ate(valor: 90, retornar: 5);
            classificadorFuzzy.Entre(valor1: 90, valor2: 120, retornar: 2);
            classificadorFuzzy.Entre(valor1: 120, valor2: 150, retornar: 1);
            classificadorFuzzy.Apos(valor: 150, retornar: 0);
            
            return classificadorFuzzy;
        }

        public static ClassificadorFuzzy Abordagem2()
        {
            var classificadorFuzzy = new ClassificadorFuzzy();
            classificadorFuzzy.Ate(valor: 90, retornar: 3);
            classificadorFuzzy.Entre(valor1: 90, valor2: 120, retornar: 2);
            classificadorFuzzy.Entre(valor1: 120, valor2: 150, retornar: 1);
            classificadorFuzzy.Apos(valor: 150, retornar: 0);

            return classificadorFuzzy;
        }

        public static ClassificadorFuzzy Abordagem3()
        {
            var classificadorFuzzy = new ClassificadorFuzzy();
            classificadorFuzzy.Ate(valor: 75, retornar: 2);
            classificadorFuzzy.Entre(valor1: 75, valor2: 110, retornar: 3);
            classificadorFuzzy.Entre(valor1: 110, valor2: 145, retornar: 1);
            classificadorFuzzy.Apos(valor: 145, retornar: 0);

            return classificadorFuzzy;
        }

        public int Classificar(int angulo)
        {
            foreach (var funcao in funcoesClassificacao)
            {
                var classificacao = funcao.Invoke(angulo);
                if (classificacao != NaoClassificado)
                    return classificacao;
            }
            throw new Exception("Não foi possível classificar o valor informado");
        }

        internal void Ate(int valor, int retornar)
        {
            funcoesClassificacao.Add(a =>
            {
                if (a < valor)
                    return retornar;
                return NaoClassificado;
            });
        }

        internal void Entre(int valor1, int valor2, int retornar)
        {
            funcoesClassificacao.Add(a =>
            {
                if (a >= valor1 && a < valor2)
                    return retornar;
                return NaoClassificado;
            });
        }

        internal void Apos(int valor, int retornar)
        {
            funcoesClassificacao.Add(a =>
            {
                if (a >= valor)
                    return retornar;
                return NaoClassificado;
            });
        }
    }
}