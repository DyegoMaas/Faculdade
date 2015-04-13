using System;
using System.Collections.Generic;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;

namespace Exercicio01.EngineGrafica
{
    public class Mundo
    {
        public Camera Camera { get; set; }
        public List<ObjetoGrafico> Filhos { get; set; }
    }

    public class Camera
    {
        public int MinX { get; set; }
        public int MaxX { get; set; }
        public int MinY { get; set; }
        public int MaxY { get; set; }

        public void Pan()
        {
            throw new NotImplementedException();
        }

        public void Zoom()
        {
            throw new NotImplementedException();
        }
    }

    public class ObjetoGrafico
    {
        public List<ObjetoGrafico> Filhos { get; set; }
        public Transformacao Transformacao { get; set; }
        public Color Cor { get; set; }
        public PrimitiveType Primitiva { get; set; }

        public Vector4 Posicao { get; set; }

        public void Desenhar()
        {
            throw new NotImplementedException();
        }
    }

    public class Transformacao
    {
        private float[,] matriz = new float[4,4];

        public void AplicarTransformacao(object algo)
        {
            throw new NotImplementedException();
        }
    }

    public class BBox
    {
        public int MinX { get; set; }
        public int MaxX { get; set; }
        public int MinY { get; set; }
        public int MaxY { get; set; }

        public static BBox Calcular(ObjetoGrafico objeto)
        {
            throw new NotImplementedException();
        }
    }
}