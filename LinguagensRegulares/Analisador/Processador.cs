using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Analisador.Atributos;
using Analisador.Excecoes;
using Analisador.Excecoes.Finalizacao;
using Analisador.Linguagens;

namespace Analisador
{
    public class Processador
    {
        private string palavra = string.Empty;
        private LinguagemRegular linguagem = null;
        private MethodInfo[] metodos = null;

        public Processador(string palavra, LinguagensSuportadas pLinguagem = LinguagensSuportadas.G1)
        {
            this.palavra = palavra;
            this.linguagem = LinguagemRegularFactory.GetLinguagem(pLinguagem);
            this.metodos = linguagem.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
        }

        public void Processar()
        {
            try
            {
                ProcessarPalavra();
            }
            catch (ProcessingEndException ex)
            {
                if (ex is InvalidCharacterException || ex is InvalidWordException)
                {
                    ex.Resultado.Reconhecimento += ", erro";
                }

                throw ex;
            }
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
                string sequencia = MontarSequencia(restoPalavra);
                
                if (EhMetodoFinal(atual))
                {
                    throw new ValidWordException(
                        new ResultadoAnalise()
                        {
                            Sequencia = sequencia,
                            Resultado = "palavra válida",
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

            if (!linguagem.CaracteresSuportados.Contains(restoPalavra[0]))
            {
                string sequencia = MontarSequencia(restoPalavra);
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

            BindAttribute[] bindings = GetBindAttributesCom(restoPalavra[0], atual);
            if (bindings.Length == 0)
            {
                throw new InvalidWordException(
                        new ResultadoAnalise()
                        {
                            Sequencia = MontarSequencia(restoPalavra),
                            Resultado = "erro: palavra inválida",
                            Reconhecimento = reconhecimento
                        });
            }
            else foreach (BindAttribute binding in bindings) 
            {
                MethodInfo destino = (from metodosDestino in metodos
                                      where metodosDestino.Name.ToUpper() == binding.Destino.ToUpper()
                                      select metodosDestino).First();

                StringBuilder recon = new StringBuilder(reconhecimento);
                recon.Append(", ").Append(destino.Name);

                ProcessarPalavraRecursivo(restoPalavra.Substring(1, restoPalavra.Length - 1), destino, recon.ToString());
            }
        }

        private bool EhMetodoFinal(MethodInfo atual)
        {
            return atual.GetCustomAttributes(typeof(EndAttribute), false).Count() == 1;
        }

        private MethodInfo GetMetodoInicial()
        {
            return (from inicio in metodos
                          where (inicio.GetCustomAttributes(typeof(StartAttribute), false).Count() == 1)
                          select inicio).First() as MethodInfo;
        }

        private BindAttribute[] GetBindAttributesCom(char caractere, MethodInfo metodo)
        {
            var bindings = from atributo in metodo.GetCustomAttributes(typeof(BindAttribute), false) as BindAttribute[]
                                     where (atributo.Caractere == caractere)
                                     select atributo;

            return bindings.ToArray();
        }

        private string MontarSequencia(string restoPalavra)
        {
            return palavra.Remove(palavra.Length - restoPalavra.Length, restoPalavra.Length);
        }

    }
}
