using OpenGL;
using System.Collections.Generic;

namespace myOpenGL
{
    public class Aquarium
    {
        uint? m_StandTexture, m_TankBottomTexture, m_TankBackTexture, m_TankWaterTexture;

        public Aquarium(uint? i_StandTexture, uint? i_TankBottomTexture, uint? i_TankBackTexture, uint? i_TankWaterTexture) 
        {
            m_StandTexture = i_StandTexture;
            m_TankBottomTexture = i_TankBottomTexture;
            m_TankBackTexture = i_TankBackTexture;
            m_TankWaterTexture = i_TankWaterTexture;
        }

        public void Draw(bool i_IsShadow)
        {
            if (!i_IsShadow && m_StandTexture.HasValue)
            {
                GL.glEnable(GL.GL_TEXTURE_2D);
                GL.glBindTexture(GL.GL_TEXTURE_2D, m_StandTexture.Value);
            }
            
            // tank cover
            Cube.DrawScaledCube(2.4f, 6.5f, 21.7f, 0.05f, 0.01f, 0.21f);

            // aqua left leg
            Cube.DrawScaledCube(2.4f, 1.8f, 19.89f, 0.05f, 0.17f, 0.03f);

            // aqua right leg
            Cube.DrawScaledCube(2.4f, 1.8f, 23.5f, 0.05f, 0.17f, 0.03f);

            // aqua plate
            Cube.DrawScaledCube(2.4f, 3.5f, 21.7f, 0.05f, 0.016f, 0.21f);

            if (!i_IsShadow && m_StandTexture.HasValue)
            {
                GL.glDisable(GL.GL_TEXTURE_2D);
            }

            // tank - bottom
            if (!i_IsShadow && m_TankBottomTexture.HasValue)
            {
                GL.glEnable(GL.GL_TEXTURE_2D);
                GL.glBindTexture(GL.GL_TEXTURE_2D, m_TankBottomTexture.Value);
            }

            Cube.DrawScaledCube(2.4f, 3.7f, 21.7f, 0.05f, 0.003f, 0.21f);

            if (!i_IsShadow && m_TankBottomTexture.HasValue)
            {
                GL.glDisable(GL.GL_TEXTURE_2D);
            }
            
            List<Fish> fish = new List<Fish>(3) { new Fish(1.4f, 4.5f, 23.5f), new Fish(1.8f, 5f, 21.5f), new Fish(1.4f, 5.5f, 20.5f) };

            foreach (Fish f in fish)
            {
                f.Draw();
            }

            // tank - left
            GL.glPushMatrix();

            if (!i_IsShadow && m_TankBackTexture.HasValue)
            {
                GL.glEnable(GL.GL_BLEND);
                GL.glBlendFunc(GL.GL_SRC_ALPHA, GL.GL_ONE_MINUS_SRC_ALPHA);
                GL.glColor4f(1.0f, 1.0f, 1.0f, 0.6f);
                GL.glEnable(GL.GL_TEXTURE_2D);
                GL.glBindTexture(GL.GL_TEXTURE_2D, m_TankBackTexture.Value);
            }

            GL.glTranslatef(1.9f, 5.1f, 21.7f);
            GL.glRotatef(90.0f, 0.0f, 0.0f, 1.0f);
            GL.glScalef(0.15f, 0.001f, 0.21f);
            Cube.Draw();
            GL.glPopMatrix();

            if (!i_IsShadow && m_TankBackTexture.HasValue)
            {
                GL.glDisable(GL.GL_TEXTURE_2D);
                GL.glDisable(GL.GL_BLEND);
            }

            //tank - water 
            if (!i_IsShadow && m_TankWaterTexture.HasValue)
            {
                GL.glEnable(GL.GL_BLEND);
                GL.glBlendFunc(GL.GL_SRC_ALPHA, GL.GL_ONE_MINUS_SRC_ALPHA);
                GL.glEnable(GL.GL_TEXTURE_2D);
                GL.glBindTexture(GL.GL_TEXTURE_2D, m_TankWaterTexture.Value);
                GL.glColor4f(0.0f, 0.8f, 0.8f, 0.4f);
            }

            Cube.DrawScaledCube(2.4f, 4.6f, 21.7f, 0.05f, 0.1f, 0.21f);
            

            if (!i_IsShadow && m_TankWaterTexture.HasValue)
            {
                GL.glDisable(GL.GL_BLEND);
                GL.glDisable(GL.GL_TEXTURE_2D);
            }

            // tank - right
            GL.glPushMatrix();

            if (!i_IsShadow)
            {
                GL.glEnable(GL.GL_BLEND);
                GL.glBlendFunc(GL.GL_SRC_ALPHA, GL.GL_ONE_MINUS_SRC_ALPHA);
                GL.glColor4f(1.0f, 1.0f, 1.0f, 0.3f);
            }

            GL.glTranslatef(2.9f, 5.1f, 21.7f);
            GL.glRotatef(90.0f, 0.0f, 0.0f, 1.0f);
            GL.glScalef(0.15f, 0.001f, 0.21f);
            Cube.Draw();
            GL.glPopMatrix();

            // tank - back
            if (!i_IsShadow)
            {
                GL.glColor4f(1.0f, 1.0f, 1.0f, 0.3f);
            }

            GL.glPushMatrix();
            GL.glTranslatef(2.4f, 5.1f, 19.6f);
            GL.glRotatef(90.0f, 1.0f, 0.0f, 0.0f);
            GL.glScalef(0.052f, 0.001f, 0.15f);
            Cube.Draw();
            GL.glPopMatrix();

            //tank - front
            GL.glPushMatrix();
            GL.glTranslatef(2.4f, 5.1f, 23.81f);
            GL.glRotatef(90.0f, 1.0f, 0.0f, 0.0f);
            GL.glScalef(0.052f, 0.001f, 0.15f);
            Cube.Draw();
            GL.glPopMatrix();

            if (!i_IsShadow)
            {
                GL.glDisable(GL.GL_COLOR);
                GL.glDisable(GL.GL_BLEND);
            }
        }
    }
}
