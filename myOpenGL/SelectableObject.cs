using OpenGL;
using System.Drawing;

namespace myOpenGL
{
    public abstract class SelectableObject
    {
        protected Color RegularColor { get; set; }
        protected Color SelectedColor { get; set; }
        public bool Select { get; set; }

        public SelectableObject()
        {
            RegularColor = Color.White;
            SelectedColor = Color.Yellow;
            Select = false;
        }

        protected void ApplySelectedColor()
        {
            if (Select)
            {
                GL.glColor3f(SelectedColor.R / 255, SelectedColor.G / 255, SelectedColor.B / 255);
            }
            else
            {
                GL.glColor3f(RegularColor.R / 255, RegularColor.G / 255, RegularColor.B / 255);
            }
        }

        public abstract void Draw(bool i_IsShadow);
    }
}
