using OpenGL;

namespace myOpenGL
{
    public class Door : SelectableObject, IOpenCloseable
    {
        private const float k_ClosedAngle = 0f;
        private const float k_OpenAngle = 90f;
        private const float k_Delta = 2.0f;

        private float m_ToAngle;
        private float m_Angle;

        private uint? m_DoorTexture;

        public eDoorSides DoorSides { get; set; }

        public Door(eDoorSides i_DoorSides, uint? i_DoorTexture) : base()
        {
            DoorSides = i_DoorSides;
            m_Angle = k_ClosedAngle;
            m_DoorTexture = i_DoorTexture;
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

        public override void Draw(bool i_IsShadow)
        {
            if (!i_IsShadow && m_DoorTexture.HasValue)
            {
                ApplySelectedColor();

                GL.glEnable(GL.GL_TEXTURE_2D);
                GL.glBindTexture(GL.GL_TEXTURE_2D, m_DoorTexture.Value);
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

            if (!i_IsShadow && m_DoorTexture.HasValue)
            {
                GL.glDisable(GL.GL_TEXTURE_2D);
            }
        }

        void IOpenCloseable.Open()
        {
            MoveToAngle = k_OpenAngle;
        }

        void IOpenCloseable.Close()
        {
            MoveToAngle = k_ClosedAngle;
        }
    }
}
