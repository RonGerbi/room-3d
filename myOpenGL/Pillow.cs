using OpenGL;
using System;
using System.Collections.Generic;
using System.Text;

namespace myOpenGL
{
    public class Pillow
    {
        public Pillow() { }
        public void Draw(float x, float y, float z, float rotationAngle, uint? i_Texture)
        {
            if (i_Texture.HasValue)
            {
                GL.glColor3f(0.627f, 0.322f, 0.176f);
                GL.glEnable(GL.GL_TEXTURE_2D);
                GL.glBindTexture(GL.GL_TEXTURE_2D, i_Texture.Value);
            }

            GL.glPushMatrix();
            GL.glTranslatef(x, y, z);
            GL.glRotatef(rotationAngle, 0f, 0f, 1f);
            GL.glScalef(0.025f, 0.08f, 0.2f);
            Cube.Draw();
            GL.glPopMatrix();

            if (i_Texture.HasValue)
            {
                GL.glDisable(GL.GL_TEXTURE_2D);
            }
        }
    }
}
