using OpenTK;

namespace N2_Exercicio07
{
    public struct Quadrado
    {
        public float Largura;

        public Vector2 PontoInferiorEsquerdo;
        public Vector2 PontoSuperiorEsquerdo
        {
            get { return new Vector2(PontoInferiorEsquerdo.X, PontoSuperiorDireito.Y); }
        }

        public Vector2 PontoSuperiorDireito;
        public Vector2 PontoInferiorDireito
        {
            get { return new Vector2(PontoSuperiorDireito.X, PontoInferiorEsquerdo.Y); }
        }

        public Quadrado(Vector2 posicao, float largura)
            : this()
        {
            Largura = largura;
            PontoInferiorEsquerdo = posicao;
            PontoSuperiorDireito = new Vector2(posicao.X + largura, posicao.Y + largura);
        }

        public bool Contem(float x, float y)
        {
            return x > PontoInferiorEsquerdo.X && x < PontoSuperiorDireito.X && 
                   y > PontoInferiorEsquerdo.Y && y < PontoSuperiorDireito.Y;
        }

        public bool Contem(Vector2 posicao)
        {
            return Contem(posicao.X, posicao.Y);
        }
    }
}