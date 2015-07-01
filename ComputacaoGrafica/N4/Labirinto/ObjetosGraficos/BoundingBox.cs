using JogoLabirinto.Utils;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace JogoLabirinto.ObjetosGraficos
{
    public class BoundingBox : ObjetoGrafico
    {
        Vector3d min = Vector3d.Zero, max = Vector3d.Zero;

        public Vector3d Min
        {
            set { min = value; }
            get { return min; }
        }

        public Vector3d Max
        {
            set { max = value; }
            get { return max; }
        }

        public Vector3d Centro
        {
            set
            {
                var dist = value - Centro;
                min += dist;
                max += dist;
            }
            get { return (min + max) / 2.0f; }
        }

        public Vector3d HalfSize
        {
            set
            {
                Vector3d cent = Centro;
                max = cent + value;
                min = cent - value;
            }
            get { return (max - min) / 2.0f; }
        }

        public void Scale(Vector3d scale)
        {
            var halfSize = HalfSize;

            halfSize.X *= scale.X;
            halfSize.Y *= scale.Y;
            halfSize.Z *= scale.Z;

            HalfSize = halfSize;
        }

        public BoundingBox()
        {
            GraphicUtils.TransformarEmCubo(this);
            Wireframe = true;
            AntesDeDesenhar(() => GL.Color3(Color.DarkKhaki));
        }

        public BoundingBox(BoundingBox box) 
            : this()
        {
            min = box.Min;
            max = box.Max;
        }

        public BoundingBox(Vector3d min, Vector3d max)
            : this()
        {
            this.min = min;
            this.max = max;
        }

        protected override void DesenharObjeto()
        {
            //TODO desenhar
        }
    }
}