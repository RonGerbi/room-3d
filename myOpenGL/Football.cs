using OpenGL;

namespace myOpenGL
{
    public class Football : SelectableObject
    {
        private uint? m_FootballTexture;
        private float m_AngleDelta = 0f;
        private bool m_IsRotating = false;

        public Football(uint? i_FootballTexture)
        {
            m_FootballTexture = i_FootballTexture;
        }

        public void Rotate()
        {
            m_IsRotating = true;
        }

        public void Freeze()
        {
            m_IsRotating = false;
        }

        public override void Draw(bool i_IsShadow)
        {
            m_AngleDelta = m_IsRotating ? (m_AngleDelta - 2f) % 360f : m_AngleDelta;
            GLUquadric obj;
            obj = GLU.gluNewQuadric();

            if (!i_IsShadow && m_FootballTexture.HasValue)
            {
                GL.glEnable(GL.GL_TEXTURE_2D);
                GL.glBindTexture(GL.GL_TEXTURE_2D, m_FootballTexture.Value);
                GL.glColor3f(1.0f, 1.0f, 1.0f);
            }

            GLU.gluQuadricTexture(obj, (byte)GL.GL_TRUE);

            GL.glPushMatrix();
            GL.glRotatef(m_AngleDelta, 0.0f, 0.0f, 1.0f);
            GLU.gluSphere(obj, 0.6, 80, 80);
            GL.glPopMatrix();

            if (m_FootballTexture.HasValue)
            {
                GL.glDisable(GL.GL_TEXTURE_2D);
            }

            GLU.gluDeleteQuadric(obj);
        }
    }
}
