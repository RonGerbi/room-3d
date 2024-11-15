using OpenGL;
using System;
using System.Collections.Generic;
using System.Text;

namespace myOpenGL
{
    public class Aquarium
    {
        public Aquarium() 
        {

        }

        public void Draw(uint? i_Texture, uint? i_TankBottomTexture, uint? i_TankLeftTexture, uint? i_TankWaterTexture)
        {
            if (i_Texture.HasValue)
            {
                GL.glEnable(GL.GL_TEXTURE_2D);
                GL.glBindTexture(GL.GL_TEXTURE_2D, i_Texture.Value);
            }
            
            // tank cover
            Cube.DrawScaledCube(2.4f, 6.5f, 21.7f, 0.05f, 0.01f, 0.21f);

            // aqua left leg
            Cube.DrawScaledCube(2.4f, 1.8f, 19.89f, 0.05f, 0.17f, 0.03f);

            // aqua right leg
            Cube.DrawScaledCube(2.4f, 1.8f, 23.5f, 0.05f, 0.17f, 0.03f);

            // aqua plate
            Cube.DrawScaledCube(2.4f, 3.5f, 21.7f, 0.05f, 0.016f, 0.21f);

            GL.glDisable(GL.GL_TEXTURE_2D);

            // tank - bottom
            if (i_TankBottomTexture.HasValue)
            {
                GL.glEnable(GL.GL_TEXTURE_2D);
                GL.glBindTexture(GL.GL_TEXTURE_2D, i_TankBottomTexture.Value);
            }

            Cube.DrawScaledCube(2.4f, 3.7f, 21.7f, 0.05f, 0.003f, 0.21f);

            if (i_TankBottomTexture.HasValue)
            {
                GL.glDisable(GL.GL_TEXTURE_2D);
            }
            
            Fish fish = new Fish();

            fish.Draw();

            // tank - left
            GL.glPushMatrix();
            GL.glEnable(GL.GL_BLEND);
            GL.glBlendFunc(GL.GL_SRC_ALPHA, GL.GL_ONE_MINUS_SRC_ALPHA);
            GL.glColor4f(1.0f, 1.0f, 1.0f, 0.6f);

            if (i_TankLeftTexture.HasValue)
            {
                GL.glEnable(GL.GL_TEXTURE_2D);
                GL.glBindTexture(GL.GL_TEXTURE_2D, i_TankLeftTexture.Value);
            }

            GL.glTranslatef(1.9f, 5.1f, 21.7f);
            GL.glRotatef(90.0f, 0.0f, 0.0f, 1.0f);
            GL.glScalef(0.15f, 0.001f, 0.21f);
            Cube.Draw();
            GL.glPopMatrix();

            if (i_TankLeftTexture.HasValue)
            {
                GL.glDisable(GL.GL_TEXTURE_2D);
                GL.glDisable(GL.GL_BLEND);
            }

            //tank - water 
            GL.glEnable(GL.GL_BLEND);
            GL.glBlendFunc(GL.GL_SRC_ALPHA, GL.GL_ONE_MINUS_SRC_ALPHA);

            if (i_TankWaterTexture.HasValue)
            {
                GL.glEnable(GL.GL_TEXTURE_2D);
                GL.glBindTexture(GL.GL_TEXTURE_2D, i_TankWaterTexture.Value);
            }

            GL.glColor4f(0.0f, 0.8f, 0.8f, 0.4f);
            Cube.DrawScaledCube(2.4f, 4.6f, 21.7f, 0.05f, 0.1f, 0.21f);
            GL.glDisable(GL.GL_BLEND);

            if (i_TankWaterTexture.HasValue)
            {
                GL.glDisable(GL.GL_TEXTURE_2D);
            }

            // tank - right
            GL.glPushMatrix();
            GL.glEnable(GL.GL_BLEND);
            GL.glBlendFunc(GL.GL_SRC_ALPHA, GL.GL_ONE_MINUS_SRC_ALPHA);
            GL.glColor4f(1.0f, 1.0f, 1.0f, 0.3f);
            GL.glTranslatef(2.9f, 5.1f, 21.7f);
            GL.glRotatef(90.0f, 0.0f, 0.0f, 1.0f);
            GL.glScalef(0.15f, 0.001f, 0.21f);
            Cube.Draw();
            GL.glPopMatrix();

            // tank - back
            GL.glColor4f(1.0f, 1.0f, 1.0f, 0.3f);
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
            GL.glDisable(GL.GL_COLOR);
            GL.glDisable(GL.GL_BLEND);
        }
    }
}
