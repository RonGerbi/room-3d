using OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace myOpenGL
{
    public class Door : SelectableObject
    {
        private const float k_ClosedAngle = 0f;
        private const float k_OpenAngle = 90f;
        private const float k_Delta = 2.0f;

        private float m_ToAngle;
        private float m_Angle;

        public eDoorSides DoorSides { get; set; }

        public Door(eDoorSides i_DoorSides) : base()
        {
            DoorSides = i_DoorSides;
            m_Angle = k_ClosedAngle;
        }

        private float MoveToAngle
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

        public override void Draw(uint? i_Texture)
        {
            if (i_Texture.HasValue)
            {
                ApplySelectedColor();

                GL.glEnable(GL.GL_TEXTURE_2D);
                GL.glBindTexture(GL.GL_TEXTURE_2D, i_Texture.Value);
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
                    GL.glTranslatef(-0.9f, 0f, 0f);
                    GL.glRotatef(-m_Angle, 0.0f, 1.0f, 0.0f);
                    GL.glTranslatef(0.9f, 0f, 0f);
                    break;
                case eDoorSides.Right:
                    GL.glTranslatef(0.9f, 0f, 0f);
                    GL.glRotatef(m_Angle, 0.0f, 1.0f, 0.0f);
                    GL.glTranslatef(-0.9f, 0f, 0f);
                    break;
            }

            GL.glScalef(0.1f, 0.376f, 0.005f);
            Cube.Draw();

            GL.glPopMatrix();

            if (i_Texture.HasValue)
            {
                GL.glDisable(GL.GL_TEXTURE_2D);
            }
        }

        public override void Open()
        {
            MoveToAngle = k_OpenAngle;
        }

        public override void Close()
        {
            MoveToAngle = k_ClosedAngle;
        }
    }
}
