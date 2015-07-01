using JogoLabirinto.Jogo;
using OpenTK;

namespace JogoLabirinto.ObjetosGraficos
{
    public abstract class ComponenteTabuleiro : ObjetoGrafico, IObjetoInteligente
    {
        private Vector3d posicao;
        public Vector3d Posicao
        {
            get { return posicao; }
            set
            {
                posicao = value;
                AtualizarMatrizTranslacao();
            }
        }

        public BoundingBox BoundingBox { get; set; }

        protected ComponenteTabuleiro(Vector3d posicao)
        {
            Posicao = posicao;
            AtualizarMatrizTranslacao();

            AntesDeDesenhar(() =>
            {
                if(BoundingBox != null)
                    BoundingBox.Desenhar();
            });
        }

        private void AtualizarMatrizTranslacao()
        {
            var translacao = Matrix4d.CreateTranslation(Posicao);
            AplicarTransformacao(translacao);
        }

        public abstract void Atualizar();
    }
}