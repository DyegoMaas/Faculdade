using System;
using System.Collections.Generic;

namespace RedeNeural.Core.Classificacao.Entradas
{
    public class ClassificadorAngulos
    {
        private const int NaoClassificado = -1;
        private readonly List<Func<int, int>> funcoesClassificacao = new List<Func<int, int>>();

        private ClassificadorAngulos()
        {
        }

        public static ClassificadorAngulos Abordagem1()
        {
            var classificadorAngulos = new ClassificadorAngulos();
            classificadorAngulos.Ate(angulo: 90, retornar: 5);
            classificadorAngulos.Entre(angulo1: 90, angulo2: 120, retornar: 2);
            classificadorAngulos.Entre(angulo1: 120, angulo2: 150, retornar: 1);
            classificadorAngulos.Apos(angulo: 150, retornar: 0);
            
            return classificadorAngulos;
        }

        public static ClassificadorAngulos Abordagem2()
        {
            var classificadorAngulos = new ClassificadorAngulos();
            classificadorAngulos.Ate(angulo: 90, retornar: 3);
            classificadorAngulos.Entre(angulo1: 90, angulo2: 120, retornar: 2);
            classificadorAngulos.Entre(angulo1: 120, angulo2: 150, retornar: 1);
            classificadorAngulos.Apos(angulo: 150, retornar: 0);

            return classificadorAngulos;
        }

        public static ClassificadorAngulos Abordagem3()
        {
            var classificadorAngulos = new ClassificadorAngulos();
            classificadorAngulos.Ate(angulo: 75, retornar: 2);
            classificadorAngulos.Entre(angulo1: 75, angulo2: 110, retornar: 3);
            classificadorAngulos.Entre(angulo1: 110, angulo2: 145, retornar: 1);
            classificadorAngulos.Apos(angulo: 145, retornar: 0);

            return classificadorAngulos;
        }

        public int Classificar(int angulo)
        {
            foreach (var funcao in funcoesClassificacao)
            {
                var classificacao = funcao.Invoke(angulo);
                if (classificacao != NaoClassificado)
                    return classificacao;
            }
            throw new Exception("Não foi possível classificar o angulo informado");
        }

        internal void Ate(int angulo, int retornar)
        {
            funcoesClassificacao.Add(a =>
            {
                if (a < angulo)
                    return retornar;
                return NaoClassificado;
            });
        }

        internal void Entre(int angulo1, int angulo2, int retornar)
        {
            funcoesClassificacao.Add(a =>
            {
                if (a >= angulo1 && a < angulo2)
                    return retornar;
                return NaoClassificado;
            });
        }

        internal void Apos(int angulo, int retornar)
        {
            funcoesClassificacao.Add(a =>
            {
                if (a >= angulo)
                    return retornar;
                return NaoClassificado;
            });
        }
    }
}