using OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace myOpenGL
{
    public class Door
    {
        private const float k_ClosedAngle = 0f;
        private const float k_OpenAngle = 90f;
        private const float k_Delta = 2.0f;

        private float m_ToAngle;
        private float m_Angle;

        private Color m_HighlightColor;
        public eDoorSides DoorSides { get; set; }
        public bool Select { get; set; }

        public Door(eDoorSides i_DoorSides)
        {
            DoorSides = i_DoorSides;
            m_Angle = k_ClosedAngle;
            m_HighlightColor = Color.Yellow;
        }

        public float MoveToAngle
        {
            set
            {
                if (value < k_ClosedAngle)
                {
                    m_ToAngle = k_ClosedAngle;
                }
                else if (value > k_OpenAngle)
                {
                    m_ToAngle = k_OpenAngle;
                }
                else
                {
                    m_ToAngle = value;
                }
            }
        }

        public void Draw(uint? texture)
        {
            if (texture.HasValue)
            {
                if (Select)
                {
                    GL.glColor3f(m_HighlightColor.R / 255, m_HighlightColor.G / 255, m_HighlightColor.B / 255);
                }

                GL.glEnable(GL.GL_TEXTURE_2D);
                GL.glBindTexture(GL.GL_TEXTURE_2D, texture.Value);
            }

            GL.glPushMatrix();

            if (m_Angle < m_ToAngle)
            {
                m_Angle += k_Delta;
            }
            else if (m_Angle > m_ToAngle)
            {
                m_Angle -= k_Delta;
            }

            switch (DoorSides)
            {
                case eDoorSides.Left:
                    GL.glTranslatef(-1f, 0f, 0f);
                    GL.glRotatef(-m_Angle, 0.0f, 1.0f, 0.0f);
                    GL.glTranslatef(1f, 0f, 0f);
                    break;
                case eDoorSides.Right:
                    GL.glTranslatef(1f, 0f, 0f);
                    GL.glRotatef(m_Angle, 0.0f, 1.0f, 0.0f);
                    GL.glTranslatef(-1f, 0f, 0f);
                    break;
            }

            GL.glScalef(0.1f, 0.376f, 0.005f);
            Cube.Draw();

            GL.glPopMatrix();

            if (texture.HasValue)
            {
                GL.glDisable(GL.GL_TEXTURE_2D);
            }
        }
    }
}
