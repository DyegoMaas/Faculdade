using Exercicio01.EngineGrafica;
using OpenTK;

namespace Exercicio01.Editor
{
    public class InputManager
    {
        private readonly GameWindow gameWindow;

        public InputManager(GameWindow gameWindow)
        {
            this.gameWindow = gameWindow;
        }

        public Ponto4D ObterPosicaoMouseNaTela()
        {
            return new Ponto4D(gameWindow.Mouse.X, gameWindow.Height - gameWindow.Mouse.Y);
        }
    }
}