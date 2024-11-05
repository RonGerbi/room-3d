using OpenGL;
using System;
using System.Collections.Generic;
using System.Text;

namespace myOpenGL
{
    public class Blind
    {
        public Blind() { }

        public void Draw(float i_ScaleX, float i_ScaleY, float i_ScaleZ)
        {
            GL.glPushMatrix();
            GL.glScalef(i_ScaleX, i_ScaleY, i_ScaleZ);
            Cube.Draw();
            GL.glPopMatrix();
        }
    }
}
