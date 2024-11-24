using OpenGL;
using System;

namespace myOpenGL
{
    public class Football : SelectableObject
    {
        private const float k_BaseLocationDelta = 0f;
        private const float k_MaxLocationDelta = 10f;
        private const float k_MomentumLoss = 0.001f;
        private float m_Location = 0f;
        private float m_Delta = 0f;
        private uint? m_FootballTexture;
        private float m_AngleDelta = 0f;
        private bool m_IsRotating = false;
        private bool m_IsMovingForward = false;
        private bool m_IsMovingBackwards = false;

        public Football(uint? i_FootballTexture)
        {
            m_FootballTexture = i_FootballTexture;
        }

        public void MoveForward()
        {
            m_IsMovingForward = true;
            m_IsMovingBackwards = false;
            m_Delta = 0.17f;
            rotate();
        }

        public void MoveBackwards()
        {
            m_IsMovingForward = false;
            m_IsMovingBackwards = true;
            m_Delta = -0.17f;
            rotate();
        }

        private void rotate()
        {
            m_IsRotating = true;
        }

        private void freeze()
        {
            m_IsRotating = false;
        }

        public override void Draw(bool i_IsShadow)
        {
            if (m_IsMovingForward)
            {
                m_Delta = Math.Max(m_Delta - k_MomentumLoss, 0f);
            }
            else if (m_IsMovingBackwards)
            {
                m_Delta = Math.Min(m_Delta + k_MomentumLoss, 0f);
            }

            if (Math.Abs(m_Delta) <= 0.01f)
            {
                m_Delta = 0f;
                m_IsMovingForward = false;
                m_IsMovingBackwards = false;
                freeze();
            }

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
            m_Location += m_Delta;

            GL.glPushMatrix();
            GL.glTranslatef(m_Location, 0f, 0f);
            GL.glRotatef(m_IsMovingBackwards ? -m_AngleDelta : m_AngleDelta, 0.0f, 0.0f, 1.0f);
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
