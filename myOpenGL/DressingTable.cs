using OpenGL;

namespace myOpenGL
{
    public class DressingTable: SelectableObject
    {
        private uint? m_TableTexture;

        public DressingTable(uint? i_TableTexture)
        {
            m_TableTexture = i_TableTexture;
        }

        public override void Draw(bool i_IsShadow)
        {
            if (!i_IsShadow && m_TableTexture.HasValue)
            {
                ApplySelectedColor();

                GL.glColor3f(0.85f, 0.8f, 0.7f);

                GL.glEnable(GL.GL_TEXTURE_2D);
                GL.glBindTexture(GL.GL_TEXTURE_2D, m_TableTexture.Value);
            }

            // table left leg
            Cube.DrawScaledCube(23.0f, 1.8f, 19.89f, 0.05f, 0.17f, 0.03f);

            // table right leg
            Cube.DrawScaledCube(23.0f, 1.8f, 23.5f, 0.05f, 0.17f, 0.03f);

            // table plate
            Cube.DrawScaledCube(23.0f, 3.5f, 21.7f, 0.05f, 0.016f, 0.21f);

            // table upper panel
            Cube.DrawScaledCube(22.989f, 8.94f, 21.7f, 0.05f, 0.01f, 0.21f);

            // table left panel
            GL.glPushMatrix();
            GL.glTranslatef(23.5f, 6.0f, 19.74f);
            GL.glRotatef(90.0f, 1.0f, 0.0f, 0.0f);
            GL.glScalef(0.001f, 0.008f, 0.29f);
            Cube.Draw();
            GL.glPopMatrix();

            // table right panel
            GL.glPushMatrix();
            GL.glTranslatef(23.48f, 6.0f, 23.78f);
            GL.glRotatef(90.0f, 1.0f, 0.0f, 0.0f);
            GL.glScalef(0.001f, 0.008f, 0.29f);
            Cube.Draw();
            GL.glPopMatrix();

            if (!i_IsShadow && m_TableTexture.HasValue)
            {
                GL.glDisable(GL.GL_TEXTURE_2D);
            }
        }
    }
}
