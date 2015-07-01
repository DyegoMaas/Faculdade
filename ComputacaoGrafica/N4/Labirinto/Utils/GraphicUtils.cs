using JogoLabirinto.ObjetosGraficos;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using System.Drawing;
using Tao.FreeGlut;

namespace JogoLabirinto.Utils
{
    public static class GraphicUtils
    {
        public static void DesenharCubo(IList<Face> faces, bool adicionarContornos = false)
        {
            foreach (var face in faces)
            {
                DesenharFace(face);
            }

            GL.Color3(Color.Black);
            if (adicionarContornos)
            {
                Glut.glutWireCube(1);
            }
        }

        public static void DesenharBoundingBox(BoundingBox bBox)
        {
            var min = bBox.Min;
            var max = bBox.Max;
            GL.LineWidth(1);
            GL.Color3(Color.BurlyWood);

            GL.Begin(PrimitiveType.Lines);
            {
                //TODO remover arestas duplicadas
                // Front Face
                GL.Vertex3(min.X, min.Y, max.Z);
                GL.Vertex3(max.X, min.Y, max.Z);
                GL.Vertex3(max.X, max.Y, max.Z);
                GL.Vertex3(min.X, max.Y, max.Z);

                // Back Face
                GL.Vertex3(min.X, min.Y, min.Z);
                GL.Vertex3(min.X, max.Y, min.Z);
                GL.Vertex3(max.X, max.Y, min.Z);
                GL.Vertex3(max.X, min.Y, min.Z);

                // Top Face
                GL.Vertex3(min.X, max.Y, min.Z);
                GL.Vertex3(min.X, max.Y, max.Z);
                GL.Vertex3(max.X, max.Y, max.Z);
                GL.Vertex3(max.X, max.Y, min.Z);

                // Bottom Face
                GL.Vertex3(min.X, min.Y, min.Z);
                GL.Vertex3(max.X, min.Y, min.Z);
                GL.Vertex3(max.X, min.Y, max.Z);
                GL.Vertex3(min.X, min.Y, max.Z);

                // Right face
                GL.Vertex3(max.X, min.Y, min.Z);
                GL.Vertex3(max.X, max.Y, min.Z);
                GL.Vertex3(max.X, max.Y, max.Z);
                GL.Vertex3(max.X, min.Y, max.Z);

                // Left Face
                GL.Vertex3(min.X, min.Y, min.Z);
                GL.Vertex3(min.X, min.Y, max.Z);
                GL.Vertex3(min.X, max.Y, max.Z);
                GL.Vertex3(min.X, max.Y, min.Z);
            }
            GL.End();
        }

        public static double Clamp(this double valor, double min, double max)
        {
            if (valor < min) return min;
            if (valor > max) return max;
            return valor;
        }

        public static bool EstaProximo(this Vector3d ponto, Vector3d outroPonto, double tolerancia)
        {
            return outroPonto.X >= ponto.X - tolerancia && outroPonto.X <= ponto.X + tolerancia &&
                   outroPonto.Y >= ponto.Y - tolerancia && outroPonto.Y <= ponto.Y + tolerancia;
        }

        public static Vector3d InverterSinal(this Vector3d ponto)
        {
            return new Vector3d(-ponto.X, - ponto.Y, - ponto.Z);
        }

        public static void DesenharFace(Face face)
        {
            GL.Begin(PrimitiveType.Quads);
            {
                GL.Normal3(face.Normal);
                foreach (var vertice in face.Vertices)
                {
                    GL.Vertex3(vertice);
                }    
            }
            GL.End();
        }

        public static void DesenharWireframe(Face face)
        {
            GL.Begin(PrimitiveType.Lines);
            {
                foreach (var vertice in face.Vertices)
                {
                    GL.Vertex3(vertice);
                }
            }
            GL.End();
        }

        public static void TransformarEmCubo(ObjetoGrafico objetoGrafico, float x = .5f, float y = .5f, float z = .5f)
        {
            // Front Face
            objetoGrafico.Faces.Add(new Face(new Vector3d(0, 0, 1), new[]
            {
                new Vector3d(-x, -y, z),
                new Vector3d(x, -y, z),
                new Vector3d(x, y, z),
                new Vector3d(-x, y, z)    
            }));

            // Back Face
            objetoGrafico.Faces.Add(new Face(new Vector3d(0, 0, -1), new[]
            {
                new Vector3d(-x, -y, -z),
                new Vector3d(-x, y, -z),
                new Vector3d(x, y, -z),
                new Vector3d(x, -y, -z)
            }));

            // Top Face
            objetoGrafico.Faces.Add(new Face(new Vector3d(0, 1, 0), new[]
            {
                new Vector3d(-x, y, -z),
                new Vector3d(-x, y, z),
                new Vector3d(x, y, z),
                new Vector3d(x, y, -z)
            }));

            // Bottom Face
            objetoGrafico.Faces.Add(new Face(new Vector3d(0, -1, 0), new[]
            {
                new Vector3d(-x, -y, -z),
                new Vector3d(x, -y, -z),
                new Vector3d(x, -y, z),
                new Vector3d(-x, -y, z)
            }));

            // Right face
            objetoGrafico.Faces.Add(new Face(new Vector3d(1, 0, 0), new[]
            {
                new Vector3d(x, -y, -z),
                new Vector3d(x, y, -z),
                new Vector3d(x, y, z),
                new Vector3d(x, -y, z)
            }));

            // Left face
            objetoGrafico.Faces.Add(new Face(new Vector3d(1, 0, 0), new[]
            {
                new Vector3d(-x, -y, -z),
                new Vector3d(-x, -y, z),
                new Vector3d(-x, y, z),
                new Vector3d(-x, y, -z)
            }));
        }
    }
}