using OpenGL;
using System;
using System.Collections.Generic;
using System.Text;

namespace myOpenGL
{
    internal static class Cube
    {
        internal static void Draw()
        {
            GL.glBegin(GL.GL_QUADS);
            //GL.glBindTexture(GL.GL_TEXTURE_2D, texture[1]);
            //front
            GL.glNormal3f(0.0f, 0.0f, 1.0f);
            GL.glTexCoord2f(0.0f, 0.0f);
            GL.glVertex3f(-10.0f, -10.0f, 10.0f);
            GL.glTexCoord2f(1.0f, 0.0f);
            GL.glVertex3f(10.0f, -10.0f, 10.0f);
            GL.glTexCoord2f(1.0f, 1.0f);
            GL.glVertex3f(10.0f, 10.0f, 10.0f);
            GL.glTexCoord2f(0.0f, 1.0f);
            GL.glVertex3f(-10.0f, 10.0f, 10.0f);

            //back
            GL.glNormal3f(0.0f, 0.0f, -1.0f);
            GL.glTexCoord2f(0.0f, 0.0f);
            GL.glVertex3f(-10.0f, -10.0f, -10.0f);
            GL.glTexCoord2f(1.0f, 0.0f);
            GL.glVertex3f(10.0f, -10.0f, -10.0f);
            GL.glTexCoord2f(1.0f, 1.0f);
            GL.glVertex3f(10.0f, 10.0f, -10.0f);
            GL.glTexCoord2f(0.0f, 1.0f);
            GL.glVertex3f(-10.0f, 10.0f, -10.0f);

            //top
            GL.glNormal3f(0.0f, 1.0f, 0.0f);
            GL.glTexCoord2f(0.0f, 0.0f);
            GL.glVertex3f(10.0f, 10.0f, 10.0f);
            GL.glTexCoord2f(1.0f, 0.0f);
            GL.glVertex3f(10.0f, 10.0f, -10.0f);
            GL.glTexCoord2f(1.0f, 1.0f);
            GL.glVertex3f(-10.0f, 10.0f, -10.0f);
            GL.glTexCoord2f(0.0f, 1.0f);
            GL.glVertex3f(-10.0f, 10.0f, 10.0f);

            //bottom
            GL.glNormal3f(0.0f, -1.0f, 0.0f);
            GL.glTexCoord2f(0.0f, 0.0f);
            GL.glVertex3f(10.0f, -10.0f, 10.0f);
            GL.glTexCoord2f(1.0f, 0.0f);
            GL.glVertex3f(10.0f, -10.0f, -10.0f);
            GL.glTexCoord2f(1.0f, 1.0f);
            GL.glVertex3f(-10.0f, -10.0f, -10.0f);
            GL.glTexCoord2f(0.0f, 1.0f);
            GL.glVertex3f(-10.0f, -10.0f, 10.0f);
            //right
            GL.glNormal3f(1.0f, 0.0f, 0.0f);
            GL.glTexCoord2f(0.0f, 0.0f);
            GL.glVertex3f(10.0f, -10.0f, 10.0f);
            GL.glTexCoord2f(1.0f, 0.0f);
            GL.glVertex3f(10.0f, -10.0f, -10.0f);
            GL.glTexCoord2f(1.0f, 1.0f);
            GL.glVertex3f(10.0f, 10.0f, -10.0f);
            GL.glTexCoord2f(0.0f, 1.0f);
            GL.glVertex3f(10.0f, 10.0f, 10.0f);
            //left
            GL.glNormal3f(-1.0f, 0.0f, -1.0f);
            GL.glTexCoord2f(0.0f, 0.0f);
            GL.glVertex3f(-10.0f, -10.0f, 10.0f);
            GL.glTexCoord2f(1.0f, 0.0f);
            GL.glVertex3f(-10.0f, -10.0f, -10.0f);
            GL.glTexCoord2f(1.0f, 1.0f);
            GL.glVertex3f(-10.0f, 10.0f, -10.0f);
            GL.glTexCoord2f(0.0f, 1.0f);
            GL.glVertex3f(-10.0f, 10.0f, 10.0f);
            GL.glEnd();
        }
    }
}
