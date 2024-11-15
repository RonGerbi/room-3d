using OpenGL;

namespace myOpenGL
{
    public class Fish
    {
        public float XPos {  get; set; }
        public float YPos { get; set; }
        public float ZPos { get; set; }

        public Fish(float i_XPos, float i_YPos, float i_ZPos) 
        {
            XPos = i_XPos;
            YPos = i_YPos;
            ZPos = i_ZPos;
        }

        public void Draw()
        {
            GL.glPushMatrix();
            GL.glTranslatef(XPos, YPos, ZPos);
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
        }
    }
}
