using OpenTK;
using System;

namespace JogoLabirinto.Utils
{
    public class MathUtils
    {
        public static double DistanciaEntre(Vector3d pontoA, Vector3d pontoB)
        {
            var CA = pontoA.X - pontoB.X;
            var CY = pontoA.Y - pontoB.Y;
            var CZ = pontoA.Z - pontoB.Z;
            return Math.Sqrt(CA * CA + CY * CY + CZ + CZ);
        }
    }
}