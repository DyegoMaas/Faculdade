using OpenTK;

namespace Exercicio01
{
    public class Input
    {
        private readonly GameWindow gameWindow;

        public Input(GameWindow gameWindow)
        {
            this.gameWindow = gameWindow;
        }

        public Vector2 ObterPosicaoMouseNaTela()
        {
            return new Vector2(gameWindow.Mouse.X, gameWindow.Height - gameWindow.Mouse.Y);
        }
    }
}