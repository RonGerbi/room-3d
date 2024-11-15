using OpenGL;

namespace myOpenGL
{
    public class Fish
    {
        public Fish() { }

        public void Draw()
        {
            float x_axis = 1.4f, y_axis = 4.5f;

            for (int i = 0; i <= 2; i++)
            {
                GL.glPushMatrix();
                GL.glTranslatef(x_axis, y_axis, 23f);
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

                x_axis += 0.2f; y_axis += 0.5f;
            }
        }
    }
}
