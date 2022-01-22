using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Analisador.Atributos;
using Analisador.Excecoes;
using Analisador.Excecoes.Finalizacao;
using Analisador.Linguagens;

namespace Analisador.Validacao
{
    public class ValidadorLinguagem
    {
        private LinguagemRegular linguagem;
        private MethodInfo[] metodos;

        public bool ValidarInicios { get; set; }
        public bool ValidarFinais { get; set; }

        public ValidadorLinguagem(LinguagemRegular linguagem)
        {
            this.linguagem = linguagem;
            this.metodos = linguagem.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);

            this.ValidarInicios = true;
            this.ValidarFinais = true;
        }

        public void Validar()
        {
            if (ValidarInicios)
                VerificarInicios();
            
            if (ValidarFinais)
                VerificarFinais();

            VerificarDestinosInvalidos();
            VerificarValidadeBindings();
        }

        private void VerificarInicios()
        {
            VerificarMultiplosInicios();
            VerificarAusenciaInicio();
        }

        private void VerificarMultiplosInicios()
        {
            var inicios = from metodoInicial in metodos
                          where (metodoInicial.GetCustomAttributes(typeof(StartAttribute), false).Count() > 1)
                          select metodoInicial;

            if (inicios.Count() > 0)
                throw new MultipleStartException();
        }

        private void VerificarAusenciaInicio()
        {
            var inicios = from metodoInicial in metodos
                          where (metodoInicial.GetCustomAttributes(typeof(StartAttribute), false).Count() == 1)
                          select metodoInicial;

            if (inicios.Count() == 0)
                throw new NoStartException();
        }

        private void VerificarFinais()
        {
            var finais = from metodoFinal in metodos
                          where (metodoFinal.GetCustomAttributes(typeof(EndAttribute), false).Count() > 0)
                          select metodoFinal;

            if (finais.Count() == 0)
                throw new NoEndException();
        }

        private void VerificarDestinosInvalidos()
        {
            VerificarDestinosVazios();
            VerificarDestinosInexistentes();
        }

        private void VerificarDestinosVazios()
        {
            foreach (var metodo in metodos)
            {
                foreach (BindAttribute binding in metodo.GetCustomAttributes(typeof(BindAttribute), false))
                {
                    if (binding.Destino.Trim() == string.Empty)
                        throw new InvalidDestinyException();
                }
            }
        }

        private void VerificarDestinosInexistentes()
        {
            foreach (var metodo in metodos)
            {
                foreach (BindAttribute binding in metodo.GetCustomAttributes(typeof(BindAttribute), false))
                {
                    var destinosExistentes = from destino in metodos
                                             where destino.Name.ToUpper() == binding.Destino.ToUpper()
                                             select destino;

                    if (destinosExistentes.Count() == 0)
                        throw new InvalidDestinyException();
                }
            }
        }

        private void VerificarValidadeBindings()
        {
            foreach(var metodo in metodos)
            {
                var bindingsInvalidos = from binding in metodo.GetCustomAttributes(typeof(BindAttribute), false) as BindAttribute[]
                          where !linguagem.CaracteresSuportados.Contains(binding.Caractere)
                          select binding;

                if (bindingsInvalidos.Count() > 0)
                    throw new InvalidCharacterException();
            }
        }
    }
}
