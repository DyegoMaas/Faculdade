﻿using OpenTK.Graphics.OpenGL;
using System.Drawing;
using Tao.FreeGlut;

namespace JogoLabirinto
{
    public class EsferaSolida : CoisaSolida
    {
        private Color cor;

        public EsferaSolida(Color cor)
        {
            this.cor = cor;
        }

        protected void Desenhar()
        {
            GL.Color3(cor);
            Glut.glutSolidSphere(.5, 100, 10);
        } 
    }
}