using OpenGL;
using System;
using System.Collections.Generic;
using System.Text;

namespace myOpenGL
{
    public class Mirror
    {
        public void Draw(uint? i_Texture)
        {
            if (i_Texture.HasValue)
            {
                GL.glEnable(GL.GL_TEXTURE_2D);
                GL.glBindTexture(GL.GL_TEXTURE_2D, i_Texture.Value);
            }

            Cube.Draw();

            if (i_Texture.HasValue)
            {
                GL.glDisable(GL.GL_TEXTURE_2D);
            }
        }

        /*private void drawMirror(bool i_DrawWithTexturesAndColors)
        {
            //GL.glEnable(GL.GL_STENCIL_TEST);
            //GL.glClear(GL.GL_STENCIL_BUFFER_BIT);

            // Draw the mirror
            //GL.glStencilFunc(GL.GL_ALWAYS, 1, 1);
            //GL.glStencilOp(GL.GL_KEEP, GL.GL_KEEP, GL.GL_REPLACE);

            GL.glPushMatrix();

            if (i_DrawWithTexturesAndColors)
            {
                GL.glEnable(GL.GL_TEXTURE_2D);
                GL.glBindTexture(GL.GL_TEXTURE_2D, texture[9]);
            }

            GL.glTranslatef(23.47f, 6.2f, 21.7f);
            GL.glRotatef(90.0f, 0.0f, 0.0f, 1.0f);
            GL.glScalef(0.28f, 0.001f, 0.20f);
            drawCube();

            if (i_DrawWithTexturesAndColors)
            {
                GL.glDisable(GL.GL_TEXTURE_2D);
            }

            GL.glPopMatrix();

            // Configure stencil buffer for reflection
            //GL.glStencilFunc(GL.GL_EQUAL, 1, 1);
            //GL.glStencilOp(GL.GL_KEEP, GL.GL_KEEP, GL.GL_KEEP);

            // Draw the reflected object
            //GL.glPushMatrix();
            //GL.glScalef(1.0f, -1.0f, 1.0f); // Flip the object for reflection
            //GL.glTranslatef(0.0f, -12.4f, 0.0f); // Adjust position for reflection
            //drawCube(); // Example of reflected cube, color red
            //GL.glPopMatrix();

            //GL.glDisable(GL.GL_STENCIL_TEST);
        }*/
    }
}
