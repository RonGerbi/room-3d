using OpenGL;

namespace myOpenGL
{
    public class Pillow
    {
        private uint? m_PillowTexture;

        public Pillow(uint? i_PillowTexture)
        {
            m_PillowTexture = i_PillowTexture;
        }

        public void Draw(float x, float y, float z, float rotationAngle, bool i_IsShadow)
        {
            if (!i_IsShadow && m_PillowTexture.HasValue)
            {
                GL.glColor3f(0.627f, 0.322f, 0.176f);
                GL.glEnable(GL.GL_TEXTURE_2D);
                GL.glBindTexture(GL.GL_TEXTURE_2D, m_PillowTexture.Value);
            }

            GL.glPushMatrix();
            GL.glTranslatef(x, y, z);
            GL.glRotatef(rotationAngle, 0f, 0f, 1f);
            GL.glScalef(0.025f, 0.08f, 0.2f);
            Cube.Draw();
            GL.glPopMatrix();

            if (m_PillowTexture.HasValue)
            {
                GL.glDisable(GL.GL_TEXTURE_2D);
            }
        }
    }
}
