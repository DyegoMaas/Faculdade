using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace N2_Exercicio07
{
    [Flags]
    public enum RecursosCamera
    {
        Nenhum = 0,
        Zoom = 1,
        Pan = 2
    }

    public class Camera
    {
        private float ortho2DMinX = -400f;
        private float ortho2DMaxX = 400f;
        private float ortho2DMinY = -400f;
        private float ortho2DMaxY = 400f;

        private RecursosCamera recursosHabilitados = RecursosCamera.Nenhum;
        
        public void Habilitar(RecursosCamera recursosHabilitar)
        {
            recursosHabilitados = recursosHabilitados | recursosHabilitar;
        }
        
        public void Update()
        {
            var teclado = Keyboard.GetState();

            if((recursosHabilitados & RecursosCamera.Pan) == RecursosCamera.Pan)
                Pan(teclado);

            if ((recursosHabilitados & RecursosCamera.Zoom) == RecursosCamera.Zoom)
                Zoom(teclado);
        }

        public void CarregarMatrizOrtografica()
        {
            var matriz = Matrix4.CreateOrthographicOffCenter(ortho2DMinX, ortho2DMaxX, ortho2DMinY, ortho2DMaxY, 0, 1);
            GL.LoadMatrix(ref matriz);
        }

        private void Pan(KeyboardState teclado)
        {
            float panX = 0;
            float panY = 0;

            // pan x
            if (teclado.IsKeyDown(Key.E) || teclado.IsKeyDown(Key.A))
            {
                panX += 1;
            }
            if (teclado.IsKeyDown(Key.D))
            {
                panX -= 1;
            }

            // pan y
            if (teclado.IsKeyDown(Key.C) || teclado.IsKeyDown(Key.W))
            {
                panY -= 1;
            }
            if (teclado.IsKeyDown(Key.B) || teclado.IsKeyDown(Key.S))
            {
                panY += 1;
            }

            ortho2DMinX += panX;
            ortho2DMaxX += panX;
            ortho2DMinY += panY;
            ortho2DMaxY += panY;

            CarregarMatrizOrtografica();
        }

        private void Zoom(KeyboardState teclado)
        {
            float zoom = 0;

            if (teclado.IsKeyDown(Key.I))
            {
                zoom += 1;
            }
            if (teclado.IsKeyDown(Key.O))
            {
                zoom -= 1;
            }

            ortho2DMinX += zoom;
            ortho2DMaxX -= zoom;
            ortho2DMinY += zoom;
            ortho2DMaxY -= zoom;

            CarregarMatrizOrtografica();
        }
    }
}