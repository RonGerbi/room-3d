using OpenGL;

namespace myOpenGL
{
    public class Football : SelectableObject
    {
        private uint? m_FootballTexture;

        public Football(uint? i_FootballTexture)
        {
            m_FootballTexture = i_FootballTexture;
        }

        public override void Draw(bool i_IsShadow)
        {
            GLUquadric obj;
            obj = GLU.gluNewQuadric();

            if (!i_IsShadow && m_FootballTexture.HasValue)
            {
                GL.glEnable(GL.GL_TEXTURE_2D);
                GL.glBindTexture(GL.GL_TEXTURE_2D, m_FootballTexture.Value);
                GL.glColor3f(1.0f, 1.0f, 1.0f);
            }

            GLU.gluQuadricTexture(obj, (byte)GL.GL_TRUE);
            GLU.gluSphere(obj, 0.6, 80, 80);

            if (m_FootballTexture.HasValue)
            {
                GL.glDisable(GL.GL_TEXTURE_2D);
            }

            GLU.gluDeleteQuadric(obj);
        }
    }
}
