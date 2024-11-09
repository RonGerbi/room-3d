using OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace myOpenGL
{
    public class Drawer : SelectableObject, IOpenCloseable
    {
        private const float k_ZPosClose = 0f;
        private const float k_ZPosOpen = 3f;
        private const float k_FrontDelta = 2.02f;
        private const float k_Delta = 0.3f;
        private float m_ToZPos;
        private float m_ZPos;

        public Drawer() : base()
        {
            m_ToZPos = m_ZPos = k_ZPosClose;
        }

        public override void Draw(uint? i_Texture)
        {
            if (i_Texture.HasValue)
            {
                ApplySelectedColor();
            }

            if (m_ZPos > m_ToZPos)
            {
                m_ZPos = Math.Max(m_ZPos - k_Delta, m_ToZPos);
            }
            else if (m_ZPos < m_ToZPos)
            {
                m_ZPos = Math.Min(m_ZPos + k_Delta, m_ToZPos);
            }

            drawDrawerSides();
            drawDrawerBottom();
            drawDrawerFront(i_Texture);
        }

        private void drawDrawerSides()
        {
            Cube.DrawScaledCube(0.0f, 0.25f, m_ZPos, 0.005f, 0.095f, 0.2f);
            Cube.DrawScaledCube(1.9f, 0.25f, m_ZPos, 0.005f, 0.095f, 0.2f);
        }

        private void drawDrawerBottom()
        {
            Cube.DrawScaledCube(0.95f, -0.63f, m_ZPos, 0.088f, 0.005f, 0.2f);
        }

        private void drawDrawerFront(uint? texture)
        {
            if (texture.HasValue)
            {
                GL.glEnable(GL.GL_TEXTURE_2D);
                GL.glBindTexture(GL.GL_TEXTURE_2D, texture.Value);
            }

            Cube.DrawScaledCube(0.95f, 0.2f, m_ZPos + k_FrontDelta, 0.1f, 0.15f, 0.001f);

            if (texture.HasValue)
            {
                GL.glDisable(GL.GL_TEXTURE_2D);
            }
        }

        void IOpenCloseable.Open()
        {
            m_ToZPos = k_ZPosOpen;
        }

        void IOpenCloseable.Close()
        {
            m_ToZPos = k_ZPosClose;
        }
    }
}
