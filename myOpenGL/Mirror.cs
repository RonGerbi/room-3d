using OpenGL;

namespace myOpenGL
{
    public class Mirror
    {
        public void Draw(uint? i_Texture)
        {
            if (i_Texture.HasValue)
            {
                GL.glEnable(GL.GL_TEXTURE_2D);
                GL.glBindTexture(GL.GL_TEXTURE_2D, i_Texture.Value);
            }

            Cube.Draw();

            if (i_Texture.HasValue)
            {
                GL.glDisable(GL.GL_TEXTURE_2D);
            }
        }
    }
}
