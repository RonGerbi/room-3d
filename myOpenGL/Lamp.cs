using OpenGL;

namespace myOpenGL
{
    public class Lamp: SelectableObject
    {
        public Lamp()
        {

        }

        public override void Draw(bool i_IsShadow)
        {
            GLUquadric obj = GLU.gluNewQuadric();

            // Lamp base
            if (!i_IsShadow)
            {
                ApplySelectedColor();

                GL.glColor3f(0.78f, 0.78f, 0.78f);
            }

            GL.glPushMatrix();
            GL.glRotatef(90.0f, 1.0f, 0.0f, 0.0f); // Rotate the base
            GLU.gluDisk(obj, 0.0, 1.0, 50, 50);
            GLU.gluCylinder(obj, 1.0, 1.0, 0.3, 50, 50);
            GL.glPopMatrix();

            // Column of the lamp
            GL.glPushMatrix();
            GL.glTranslatef(0.0f, 7.6f, 0.0f); // Relative position from base
            GL.glRotatef(90.0f, 1.0f, 0.0f, 0.0f);
            GLU.gluDisk(obj, 0.5, 0.5, 200, 200);
            GLU.gluCylinder(obj, 0.05, 0.05, 7.7, 20, 20);
            GL.glPopMatrix();

            // Lamp shade
            if (!i_IsShadow)
            {
                GL.glColor3f(1.0f, 0.8f, 0.6f);
            }

            GL.glPushMatrix();
            GL.glTranslatef(0.0f, 7.6f, 0.0f); // Same relative position from base as column top
            GLU.gluCylinder(obj, 1.0f, 0.0f, 1.0f, 30, 30);
            GL.glPopMatrix();

            // Light bulb
            if (!i_IsShadow)
            {
                GL.glColor3f(1.0f, 1.0f, 0.8f);
            }

            GL.glPushMatrix();
            GL.glTranslatef(0.0f, 7.6f, -1.0f); // Adjust relative to initial translation
            GLU.gluSphere(obj, 0.1, 20, 20);
            GL.glPopMatrix();


            GLU.gluDeleteQuadric(obj);
        }
    }
}
