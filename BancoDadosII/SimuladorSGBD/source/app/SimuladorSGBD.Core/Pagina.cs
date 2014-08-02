namespace SimuladorSGBD.Core
{
    internal class Pagina : IPagina
    {
        public char[] Dados { get; private set; }
        public bool Sujo { get; private set; }
        public int PinCount { get; private set; }
        public int UltimoAcesso { get; private set; }
    }
}