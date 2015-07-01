using OpenTK;
using System.Collections.Generic;

namespace JogoLabirinto.ObjetosGraficos
{
    public class Face
    {
        public Vector3d Normal { get; private set; }
        public IList<Vector3d> Vertices { get; private set; }

        public Face(Vector3d normal, IList<Vector3d> vertices)
        {
            Normal = normal;
            Vertices = vertices;
        }
    }
}