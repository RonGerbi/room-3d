using OpenGL;
using System;

namespace myOpenGL
{
    public class Fish
    {
        private const float delta = 0.01f;
        private float m_Angle;
        private float m_XPos;
        private float m_YPos;
        private float m_ZPos;
        private float m_MinXPos;
        private float m_MinYPos;
        private float m_MinZPos;
        private float m_MaxXPos;
        private float m_MaxYPos;
        private float m_MaxZPos;
        private bool m_IsForwardX;
        private bool m_IsForwardY;
        private bool m_IsForwardZ;

        public Fish(float i_XPos, float i_YPos, float i_ZPos,
            float i_MinXPos, float i_MinYPos, float i_MinZPos,
            float i_MaxXPos, float i_MaxYPos, float i_MaxZPos) 
        {
            m_Angle = 90f;
            m_XPos = i_XPos;
            m_YPos = i_YPos;
            m_ZPos = i_ZPos;
            m_MinXPos = i_MinXPos;
            m_MinYPos = i_MinYPos;
            m_MinZPos = i_MinZPos;
            m_MaxXPos = i_MaxXPos;
            m_MaxYPos = i_MaxYPos;
            m_MaxZPos = i_MaxZPos;
            m_IsForwardX = m_IsForwardY = m_IsForwardZ = true;
        }

        public void Draw()
        {
            GL.glPushMatrix();
            GL.glTranslatef(m_XPos, m_YPos, m_ZPos);
            GL.glRotatef(m_Angle, 0f, 1f, 0f);
            GL.glScalef(3f, 3f, 3f);
            GL.glColor3d(1.0, 0.0, 1.0);
            GL.glBegin(GL.GL_POLYGON);
            GL.glVertex2d(0.7, -0.3);
            GL.glVertex2d(0.775, -0.3);
            GL.glVertex2d(0.85, -0.25);
            GL.glVertex2d(0.775, -0.2);
            GL.glVertex2d(0.75, -0.2);
            GL.glEnd();

            GL.glBegin(GL.GL_TRIANGLES);
            GL.glVertex2d(0.83, -0.25);
            GL.glVertex2d(0.9, -0.29);
            GL.glVertex2d(0.9, -0.21);
            GL.glEnd();

            GL.glBegin(GL.GL_TRIANGLES);
            GL.glVertex2d(0.775, -0.2);
            GL.glVertex2d(0.75, -0.2);
            GL.glEnd();

            GL.glBegin(GL.GL_TRIANGLES);
            GL.glVertex2d(0.75, -0.3);
            GL.glVertex2d(0.795, -0.35);
            GL.glVertex2d(0.775, -0.3);
            GL.glEnd();

            GL.glColor3d(0.0, 0.0, 0.0);
            GL.glBegin(GL.GL_POINTS);
            GL.glVertex2d(0.73, -0.235);
            GL.glEnd();
            GL.glPopMatrix();

            if (m_IsForwardX)
            {
                m_XPos = Math.Min(m_XPos + delta, m_MaxXPos);
            }
            else
            {
                m_XPos = Math.Max(m_XPos - delta, m_MinXPos);
            }

            if (m_IsForwardY)
            {
                m_YPos = Math.Min(m_YPos + delta, m_MaxYPos);
            }
            else
            {
                m_YPos = Math.Max(m_YPos - delta, m_MinYPos);
            }

            if (m_IsForwardZ)
            {
                m_ZPos = Math.Min(m_ZPos + delta, m_MaxZPos);
            }
            else
            {
                m_ZPos = Math.Max(m_ZPos - delta, m_MinZPos);
            }

            if (m_XPos >= m_MaxXPos || m_XPos <= m_MinXPos)
            {
                m_IsForwardX = !m_IsForwardX;
            }
            if (m_YPos >= m_MaxYPos || m_YPos <= m_MinYPos)
            {
                m_IsForwardY = !m_IsForwardY;
            }
            if (m_ZPos >= m_MaxZPos || m_ZPos <= m_MinZPos)
            {
                m_IsForwardZ = !m_IsForwardZ;
            }
        }
    }
}
