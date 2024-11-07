using OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace myOpenGL
{
    public class Bed
    {
        private List<Pillow> m_Pillows;

        public Bed() 
        {
            m_Pillows = new List<Pillow>() { new Pillow(), new Pillow() };
        }
        public void Draw(uint? i_BedTexture, uint? i_PillowTexture, uint? i_BlanketTexture)
        {
            //bed head
            if (i_BedTexture.HasValue)
            {
                GL.glColor3f(0.329f, 0.188f, 0.016f);
            }

            Cube.DrawScaledCube(-4.5f, -0.52f, 6.2f, 0.05f, 0.16f, 0.5f);
            Cube.DrawScaledCube(0f, -1.58f, 6.2f, 0.4f, 0.06f, 0.5f);

            //bed body
            if (i_BedTexture.HasValue)
            {
                GL.glColor3f(0.639f, 0.545f, 0.204f);
                GL.glEnable(GL.GL_TEXTURE_2D);
                GL.glBindTexture(GL.GL_TEXTURE_2D, i_BedTexture.Value);
            }

            Cube.DrawScaledCube(0f, -0.27f, 6.2f, 0.4f, 0.07f, 0.5f);

            if (i_BedTexture.HasValue)
            {
                GL.glDisable(GL.GL_TEXTURE_2D);
            }

            float spaceBetween = 3.8f;

            foreach (Pillow pillow in m_Pillows)
            {
                pillow.Draw(-3.2f, 1f, spaceBetween, 70f, i_PillowTexture);
                spaceBetween += 5f;
            }

            //blanket
            if (i_BlanketTexture.HasValue)
            {
                GL.glColor3f(0.627f, 0.322f, 0.176f);
                GL.glEnable(GL.GL_TEXTURE_2D);
                GL.glBindTexture(GL.GL_TEXTURE_2D, i_BlanketTexture.Value);
            }

            GL.glPushMatrix();
            GL.glTranslatef(1f, 0.45f, 6.15f);
            GL.glScalef(0.3f, 0.005f, 0.51f);
            Cube.Draw();
            GL.glPopMatrix();

            if (i_BedTexture.HasValue)
            {
                GL.glDisable(GL.GL_TEXTURE_2D);
            }
        }
    }
}
