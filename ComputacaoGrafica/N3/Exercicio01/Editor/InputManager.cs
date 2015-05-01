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

        public Vector2 ObterPosicaoMouseNaTela()
        {
            return new Vector2(gameWindow.Mouse.X, gameWindow.Height - gameWindow.Mouse.Y);
        }
    }
}