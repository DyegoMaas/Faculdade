using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Analisador.Linguagens.Atributos;
using Analisador.Linguagens.Excecoes;
using Analisador.Linguagens.Excecoes.Finalizacao;
using Analisador.Linguagens.Validacao;

namespace Analisador.Linguagens
{
    public class Processador
    {
        private string palavra = string.Empty;
        private LinguagemRegular linguagem = null;
        private MethodInfo[] metodos = null;

        public Processador(string palavra, LinguagensSuportadas pLinguagem = LinguagensSuportadas.G1)
        {
            this.palavra = palavra;
            this.linguagem = LinguagemRegular.GetLinguagem(pLinguagem);
            this.metodos = linguagem.GetType().GetMethods();
        }

        public void Processar()
        {
            ProcessarPalavra();
        }

        private void ProcessarPalavra()
        {
            MethodInfo inicio = GetMetodoInicial();
            ProcessarPalavraRecursivo(palavra, inicio, inicio.Name);
        }

        private void ProcessarPalavraRecursivo(string restoPalavra, MethodInfo atual, string reconhecimento)
        {
            if (restoPalavra == string.Empty)
            {
                //verificar se o método atual é final / validar final
                //TODO: concluir análise
            }

            if (!linguagem.CaracteresSuportados.Contains(restoPalavra[0]))
            {
                string sequencia = palavra.Remove(palavra.Length - restoPalavra.Length, restoPalavra.Length);
                if (palavra[0] == restoPalavra[0])
                {
                    throw new InvalidCharacterException(
                        new ResultadoAnalise()
                        {
                            Sequencia = sequencia,
                            Resultado = "erro: símbolo(s) inválido(s)",
                            Reconhecimento = reconhecimento
                        });
                }
                else
                {
                    throw new InvalidWordException(
                        new ResultadoAnalise()
                        {
                            Sequencia = sequencia,
                            Resultado = "erro: palavra inválida",
                            Reconhecimento = reconhecimento
                        });
                }
            }

            WithAttribute[] withs = GetWithAttributesCom(restoPalavra[0], atual);
            if (withs.Length == 0)
            { 
                //TODO: concluir análise
                //não processou a palavra até o final
                throw new InvalidWordException();
            }
            else foreach (WithAttribute with in withs)
            {
                MethodInfo destino = (from metodosDestino in metodos
                                      where metodosDestino.Name.ToUpper() == with.Destino.ToUpper()
                                      select metodosDestino).First();

                StringBuilder recon = new StringBuilder(reconhecimento);
                recon.Append(", ").Append(destino.Name);

                ProcessarPalavraRecursivo(restoPalavra.Substring(1, restoPalavra.Length - 1), destino, recon.ToString());
            }
        }

        private MethodInfo GetMetodoInicial()
        {
            return (from inicio in metodos
                          where (inicio.GetCustomAttributes(typeof(StartAttribute), false).Count() == 1)
                          select inicio).First() as MethodInfo;
        }

        private WithAttribute[] GetWithAttributesCom(char caractere, MethodInfo metodo)
        {
            var withs = from atributo in metodo.GetCustomAttributes(typeof(WithAttribute), false) as WithAttribute[]
                                     where (atributo.Caractere == caractere)
                                     select atributo;

            return withs.ToArray();
        }

    }
}
