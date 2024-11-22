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
                GL.glEnable(GL.GL_BLEND);
                GL.glBlendFunc(GL.GL_SRC_ALPHA, GL.GL_ONE_MINUS_SRC_ALPHA);
                GL.glEnable(GL.GL_TEXTURE_2D);
                GL.glBindTexture(GL.GL_TEXTURE_2D, m_MirrorTexture.Value);

                GL.glColor4f(1.0f, 1.0f, 1.0f, 0.5f);
            }

            Cube.Draw();

            if (!i_IsShadow && m_MirrorTexture.HasValue)
            {
                GL.glDisable(GL.GL_TEXTURE_2D);
                GL.glDisable(GL.GL_BLEND);
            }
        }
    }
}
