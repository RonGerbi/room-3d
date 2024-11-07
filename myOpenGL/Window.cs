using OpenGL;
using System;
using System.Collections.Generic;
using System.Text;

namespace myOpenGL
{
    public class Window
    {
        private const int k_BlindsCount = 10;
        List<Blind> m_Blinds;

        public Window() 
        {
            m_Blinds = new List<Blind>(10);

            for (int i = 0; i < k_BlindsCount; i++)
            {
                m_Blinds.Add(new Blind());
            }
        }

        public void Draw()
        {
            float frameScaleX = 0.01f;
            float frameScaleY = 0.3f;
            float frameScaleZ = 0.01f;
            float frameTranslationX = 5f;
            float blindHeight = 0.6f;
            float blindScaleX = 0.02f;
            float blindScaleY = 0.24f;
            float blindScaleZ = 0.01f;
            int numberOfBlinds = 10;



            // left side
            GL.glPushMatrix();
            GL.glScalef(frameScaleX, frameScaleY, frameScaleZ);
            Cube.Draw();
            GL.glPopMatrix();

            // right side
            GL.glPushMatrix();
            GL.glTranslatef(frameTranslationX, 00f, 0f);
            GL.glScalef(frameScaleX, frameScaleY, frameScaleZ);
            Cube.Draw();
            GL.glPopMatrix();

            GL.glPopMatrix();

            GL.glPushMatrix();

            // down side
            GL.glRotatef(90f, 1f, 0f, 0f);
            GL.glTranslatef(0f, 9.5f, -5f);

            GL.glPushMatrix();
            GL.glScalef(blindScaleX, frameScaleY, frameScaleZ);
            Cube.Draw();
            GL.glPopMatrix();

            // upper side
            GL.glPushMatrix();
            GL.glTranslatef(0f, 0f, -6f);
            GL.glScalef(blindScaleX, frameScaleY, frameScaleZ);
            Cube.Draw();
            GL.glPopMatrix();

            foreach (Blind blind in m_Blinds)
            {
                GL.glTranslatef(0f, 0f, -blindHeight);

                blind.Draw(blindScaleX, blindScaleY, blindScaleZ);
            }
        }
    }
}
