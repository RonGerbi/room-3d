using OpenGL;

namespace myOpenGL
{
    public class Mirror
    {
        private uint? m_MirrorTexture;

        public Mirror(uint? i_MirrorTexture)
        {
            m_MirrorTexture = i_MirrorTexture;
        }

        public void Draw(bool i_IsShadow)
        {
            if (!i_IsShadow && m_MirrorTexture.HasValue)
            {
                GL.glEnable(GL.GL_TEXTURE_2D);
                GL.glBindTexture(GL.GL_TEXTURE_2D, m_MirrorTexture.Value);
            }

            Cube.Draw();

            if (m_MirrorTexture.HasValue)
            {
                GL.glDisable(GL.GL_TEXTURE_2D);
            }
        }
    }
}
