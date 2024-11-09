using OpenGL;

namespace myOpenGL
{
    public class Football : SelectableObject
    {
        public Football() { }

        public override void Draw(uint? i_Texture)
        {
            GLUquadric obj;
            obj = GLU.gluNewQuadric();

            if (i_Texture.HasValue)
            {
                GL.glEnable(GL.GL_TEXTURE_2D);
                GL.glBindTexture(GL.GL_TEXTURE_2D, i_Texture.Value);
                GL.glColor3f(1.0f, 1.0f, 1.0f);
            }

            GLU.gluQuadricTexture(obj, (byte)GL.GL_TRUE);
            GLU.gluSphere(obj, 0.6, 80, 80);

            if (i_Texture.HasValue)
            {
                GL.glDisable(GL.GL_TEXTURE_2D);
            }

            GLU.gluDeleteQuadric(obj);
        }
    }
}
