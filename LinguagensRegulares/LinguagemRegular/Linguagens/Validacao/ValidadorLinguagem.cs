using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Analisador.Linguagens.Atributos;
using Analisador.Linguagens.Excecoes;

namespace Analisador.Linguagens.Validacao
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
            this.metodos = linguagem.GetType().GetMethods();

            this.ValidarInicios = true;
            this.ValidarFinais = true;
        }

        public void Validar()
        {
            if (ValidarInicios)
                VerificarInicios();
            
            if (ValidarFinais)
                VerificarFinais();
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
    }
}
