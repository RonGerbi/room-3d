using System;
using System.Collections.Generic;
using System.Windows.Forms;

//2
using System.Drawing;
using System.Security.AccessControl;

namespace OpenGL
{
    class cOGL
    {
        private readonly string r_TexturePath = "C:\\Users\\רון\\RoomOpenGL\\wood.bmp";
        private uint[] texture;
        Control p;
        int Width;
        int Height;

        public cOGL(Control pb)
        {
            p = pb;
            Width = p.Width;
            Height = p.Height;
            InitializeGL();
        }

        ~cOGL()
        {
            WGL.wglDeleteContext(m_uint_RC);
        }

        uint m_uint_HWND = 0;

        public uint HWND
        {
            get { return m_uint_HWND; }
        }

        uint m_uint_DC = 0;

        public uint DC
        {
            get { return m_uint_DC; }
        }
        uint m_uint_RC = 0;

        public uint RC
        {
            get { return m_uint_RC; }
        }


        void DrawOldAxes()
        {
            //for this time
            //Lights positioning is here!!!
            float[] pos = new float[4];
            pos[0] = 10; pos[1] = 10; pos[2] = 10; pos[3] = 1;
            GL.glLightfv(GL.GL_LIGHT0, GL.GL_POSITION, pos);
            GL.glDisable(GL.GL_LIGHTING);

            //INITIAL axes
            GL.glEnable(GL.GL_LINE_STIPPLE);
            GL.glLineStipple(1, 0xFF00);  //  dotted   
            GL.glBegin(GL.GL_LINES);
            //x  RED
            GL.glColor3f(1.0f, 0.0f, 0.0f);
            GL.glVertex3f(-3.0f, 0.0f, 0.0f);
            GL.glVertex3f(3.0f, 0.0f, 0.0f);
            //y  GREEN 
            GL.glColor3f(0.0f, 1.0f, 0.0f);
            GL.glVertex3f(0.0f, -3.0f, 0.0f);
            GL.glVertex3f(0.0f, 3.0f, 0.0f);
            //z  BLUE
            GL.glColor3f(0.0f, 0.0f, 1.0f);
            GL.glVertex3f(0.0f, 0.0f, -3.0f);
            GL.glVertex3f(0.0f, 0.0f, 3.0f);
            GL.glEnd();
            GL.glDisable(GL.GL_LINE_STIPPLE);
        }
        void DrawAxes()
        {
            GL.glBegin(GL.GL_LINES);
            //x  RED
            GL.glColor3f(1.0f, 0.0f, 0.0f);
            GL.glVertex3f(-3.0f, 0.0f, 0.0f);
            GL.glVertex3f(3.0f, 0.0f, 0.0f);
            //y  GREEN 
            GL.glColor3f(0.0f, 1.0f, 0.0f);
            GL.glVertex3f(0.0f, -3.0f, 0.0f);
            GL.glVertex3f(0.0f, 3.0f, 0.0f);
            //z  BLUE
            GL.glColor3f(0.0f, 0.0f, 1.0f);
            GL.glVertex3f(0.0f, 0.0f, -3.0f);
            GL.glVertex3f(0.0f, 0.0f, 3.0f);
            GL.glEnd();
        }
        void DrawFigures()
        {
            GL.glTranslatef(-12.5f, 0.0f, 0.0f);
            drawFloor(25f, 0f, 0f, 0f);
            drawBed();
            drawCloset();
        }



        public float[] ScrollValue = new float[10];
        public float zShift = 0.0f;
        public float yShift = 0.0f;
        public float xShift = 0.0f;

        public float zAngle = 0.0f;
        public float yAngle = 0.0f;
        public float xAngle = 0.0f;
        public int intOptionC = 0;
        double[] AccumulatedRotationsTraslations = new double[16];
        public void Draw()
        {
            if (m_uint_DC == 0 || m_uint_RC == 0)
                return;

            GL.glClear(GL.GL_COLOR_BUFFER_BIT | GL.GL_DEPTH_BUFFER_BIT);

            //FULL and COMPLICATED				
            GL.glViewport(0, 0, Width, Height);
            GL.glLoadIdentity();

            // not trivial
            double[] ModelVievMatrixBeforeSpecificTransforms = new double[16];
            double[] CurrentRotationTraslation = new double[16];

            GLU.gluLookAt(ScrollValue[0], ScrollValue[1], ScrollValue[2],
                       ScrollValue[3], ScrollValue[4], ScrollValue[5],
                       ScrollValue[6], ScrollValue[7], ScrollValue[8]);
            GL.glTranslatef(0.0f, 0.0f, -40.0f);

            DrawOldAxes();

            //save current ModelView Matrix values
            //in ModelVievMatrixBeforeSpecificTransforms array
            //ModelView Matrix ========>>>>>> ModelVievMatrixBeforeSpecificTransforms
            GL.glGetDoublev(GL.GL_MODELVIEW_MATRIX, ModelVievMatrixBeforeSpecificTransforms);
            //ModelView Matrix was saved, so
            GL.glLoadIdentity(); // make it identity matrix

            //make transformation in accordance to KeyCode
            float delta;
            if (intOptionC != 0)
            {
                delta = 5.0f * Math.Abs(intOptionC) / intOptionC; // signed 5

                switch (Math.Abs(intOptionC))
                {
                    case 1:
                        GL.glRotatef(delta, 1, 0, 0);
                        break;
                    case 2:
                        GL.glRotatef(delta, 0, 1, 0);
                        break;
                    case 3:
                        GL.glRotatef(delta, 0, 0, 1);
                        break;
                    case 4:
                        GL.glTranslatef(delta / 20, 0, 0);
                        break;
                    case 5:
                        GL.glTranslatef(0, delta / 20, 0);
                        break;
                    case 6:
                        GL.glTranslatef(0, 0, delta / 20);
                        break;
                }
            }
            //as result - the ModelView Matrix now is pure representation
            //of KeyCode transform and only it !!!

            //save current ModelView Matrix values
            //in CurrentRotationTraslation array
            //ModelView Matrix =======>>>>>>> CurrentRotationTraslation
            GL.glGetDoublev(GL.GL_MODELVIEW_MATRIX, CurrentRotationTraslation);

            //The GL.glLoadMatrix function replaces the current matrix with
            //the one specified in its argument.
            //The current matrix is the
            //projection matrix, modelview matrix, or texture matrix,
            //determined by the current matrix mode (now is ModelView mode)
            GL.glLoadMatrixd(AccumulatedRotationsTraslations); //Global Matrix

            //The GL.glMultMatrix function multiplies the current matrix by
            //the one specified in its argument.
            //That is, if M is the current matrix and T is the matrix passed to
            //GL.glMultMatrix, then M is replaced with M • T
            GL.glMultMatrixd(CurrentRotationTraslation);

            //save the matrix product in AccumulatedRotationsTraslations
            GL.glGetDoublev(GL.GL_MODELVIEW_MATRIX, AccumulatedRotationsTraslations);

            //replace ModelViev Matrix with stored ModelVievMatrixBeforeSpecificTransforms
            GL.glLoadMatrixd(ModelVievMatrixBeforeSpecificTransforms);
            //multiply it by KeyCode defined AccumulatedRotationsTraslations matrix
            GL.glMultMatrixd(AccumulatedRotationsTraslations);

            DrawAxes();

            DrawFigures();

            GL.glFlush();

            WGL.wglSwapBuffers(m_uint_DC);

        }

        protected virtual void InitializeGL()
        {
            m_uint_HWND = (uint)p.Handle.ToInt32();
            m_uint_DC = WGL.GetDC(m_uint_HWND);

            // Not doing the following WGL.wglSwapBuffers() on the DC will
            // result in a failure to subsequently create the RC.
            WGL.wglSwapBuffers(m_uint_DC);

            WGL.PIXELFORMATDESCRIPTOR pfd = new WGL.PIXELFORMATDESCRIPTOR();
            WGL.ZeroPixelDescriptor(ref pfd);
            pfd.nVersion = 1;
            pfd.dwFlags = (WGL.PFD_DRAW_TO_WINDOW | WGL.PFD_SUPPORT_OPENGL | WGL.PFD_DOUBLEBUFFER);
            pfd.iPixelType = (byte)(WGL.PFD_TYPE_RGBA);
            pfd.cColorBits = 32;
            pfd.cDepthBits = 32;
            pfd.iLayerType = (byte)(WGL.PFD_MAIN_PLANE);

            int pixelFormatIndex = 0;
            pixelFormatIndex = WGL.ChoosePixelFormat(m_uint_DC, ref pfd);
            if (pixelFormatIndex == 0)
            {
                MessageBox.Show("Unable to retrieve pixel format");
                return;
            }

            if (WGL.SetPixelFormat(m_uint_DC, pixelFormatIndex, ref pfd) == 0)
            {
                MessageBox.Show("Unable to set pixel format");
                return;
            }
            //Create rendering context
            m_uint_RC = WGL.wglCreateContext(m_uint_DC);
            if (m_uint_RC == 0)
            {
                MessageBox.Show("Unable to get rendering context");
                return;
            }
            if (WGL.wglMakeCurrent(m_uint_DC, m_uint_RC) == 0)
            {
                MessageBox.Show("Unable to make rendering context current");
                return;
            }


            initRenderingGL();
        }

        public void OnResize()
        {
            Width = p.Width;
            Height = p.Height;
            GL.glViewport(0, 0, Width, Height);
            Draw();
        }

        protected virtual void initRenderingGL()
        {
            if (m_uint_DC == 0 || m_uint_RC == 0)
                return;
            if (this.Width == 0 || this.Height == 0)
                return;
            GL.glClearColor(1.0f, 1.0f, 1.0f, 0.0f);
            GL.glEnable(GL.GL_DEPTH_TEST);
            GL.glDepthFunc(GL.GL_LEQUAL);

            GL.glViewport(0, 0, this.Width, this.Height);
            GL.glMatrixMode(GL.GL_PROJECTION);
            GL.glLoadIdentity();

            //nice 3D
            GLU.gluPerspective(45.0, 1.0, 0.4, 100.0);

            GL.glMatrixMode(GL.GL_MODELVIEW);
            GL.glLoadIdentity();

            //save the current MODELVIEW Matrix (now it is Identity)
            GL.glGetDoublev(GL.GL_MODELVIEW_MATRIX, AccumulatedRotationsTraslations);
            int i = 0;

            // background 

            if (m_uint_DC == 0 || m_uint_RC == 0)
                return;
            if (this.Width == 0 || this.Height == 0)
                return;

            // Set the background color to light blue (RGBA)
            //GL.glClearColor(0.678f, 0.847f, 0.902f, 1.0f); // Light blue
            GL.glClearColor(0.1f, 0.1f, 0.1f, 1.0f);
            GL.glEnable(GL.GL_DEPTH_TEST);
            GL.glDepthFunc(GL.GL_LEQUAL);

            GL.glViewport(0, 0, this.Width, this.Height);
            GL.glMatrixMode(GL.GL_PROJECTION);
            GL.glLoadIdentity();

            // Set up perspective projection
            GLU.gluPerspective(45.0, 1.0, 0.4, 100.0);

            GL.glMatrixMode(GL.GL_MODELVIEW);
            GL.glLoadIdentity();

            // Save the current MODELVIEW Matrix (now it is Identity)
            GL.glGetDoublev(GL.GL_MODELVIEW_MATRIX, AccumulatedRotationsTraslations);

            InitTexture("wood.bmp");
        }

        private void InitTexture(string imageBMPfile)
        {
            GL.glEnable(GL.GL_TEXTURE_2D);

            texture = new uint[2];		// storage for texture

            Bitmap image = new Bitmap(imageBMPfile);
            image.RotateFlip(RotateFlipType.RotateNoneFlipY); //Y axis in Windows is directed downwards, while in OpenGL-upwards
            System.Drawing.Imaging.BitmapData bitmapdata;
            Rectangle rect = new Rectangle(0, 0, image.Width, image.Height);

            bitmapdata = image.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadOnly,
                System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            GL.glGenTextures(1, texture);
            GL.glBindTexture(GL.GL_TEXTURE_2D, texture[0]);
            //  VN-in order to use System.Drawing.Imaging.BitmapData Scan0 I've added overloaded version to
            //  OpenGL.cs
            //  [DllImport(GL_DLL, EntryPoint = "glTexImage2D")]
            //  public static extern void glTexImage2D(uint target, int level, int internalformat, int width, int height, int border, uint format, uint type, IntPtr pixels);
            GL.glTexImage2D(GL.GL_TEXTURE_2D, 0, (int)GL.GL_RGB8, image.Width, image.Height,
                0, GL.GL_BGR_EXT, GL.GL_UNSIGNED_byte, bitmapdata.Scan0);

            GL.glTexParameteri(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_MIN_FILTER, (int)GL.GL_LINEAR);		// Linear Filtering
            GL.glTexParameteri(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_MAG_FILTER, (int)GL.GL_LINEAR);		// Linear Filtering

            image.UnlockBits(bitmapdata);
            image.Dispose();
        }

        private void drawFloor(float i_Length, float i_RootX, float i_RootY, float i_RootZ)
        {
            GL.glColor3f(1.0f, 1.0f, 1.0f);
            GL.glEnable(GL.GL_TEXTURE_2D);
            GL.glBindTexture(GL.GL_TEXTURE_2D, texture[0]);
            GL.glDisable(GL.GL_LIGHTING);


            GL.glBegin(GL.GL_QUADS);
            GL.glTexCoord2f(1.0f, 1.0f);			// top right of texture
            GL.glVertex3f(i_RootX + i_Length, i_RootY, i_RootZ + i_Length);		// top right of quad
            GL.glTexCoord2f(0.0f, 1.0f);			// top left of texture
            GL.glVertex3f(i_RootX, i_RootY, i_RootZ + i_Length);		// top left of quad
            GL.glTexCoord2f(0.0f, 0.0f);			// bottom left of texture
            GL.glVertex3f(i_RootX, i_RootY, i_RootZ);	    // bottom left of quad
            GL.glTexCoord2f(1.0f, 0.0f);			// bottom right of texture
            GL.glVertex3f(i_RootX + i_Length, i_RootY, i_RootZ);		// bottom right of quad
            GL.glEnd();
            GL.glDisable(GL.GL_TEXTURE_2D);
        }

        private void drawRectangle(float i_Length, float i_RootX, float i_RootY, float i_RootZ, float i_Red, float i_Green, float i_Blue)
        {
            // Cube
            GL.glBegin(GL.GL_QUADS);

            //1

            GL.glColor3f(i_Red, i_Green, i_Blue);
            GL.glVertex3f(i_RootX, i_RootY, i_RootZ);

            GL.glVertex3f(i_RootX, i_RootY + i_Length, i_RootZ);

            GL.glVertex3f(i_RootX + i_Length, i_RootY + i_Length, i_RootZ);

            GL.glVertex3f(i_RootX + i_Length, i_RootY, i_RootZ);

            //2

            GL.glVertex3f(i_RootX, i_RootY, i_RootZ);


            GL.glVertex3f(i_RootX, i_RootY, i_RootZ + i_Length);

            GL.glVertex3f(i_RootX, i_RootY + i_Length, i_RootZ + i_Length);

            GL.glVertex3f(i_RootX, i_RootY + i_Length, i_RootZ);


            //3


            GL.glVertex3f(i_RootX, i_RootY, i_RootZ);

            GL.glVertex3f(i_RootX + i_Length, i_RootY, i_RootZ);

            GL.glVertex3f(i_RootX + i_Length, i_RootY, i_RootZ + i_Length);

            GL.glVertex3f(i_RootX, i_RootY, i_RootZ + i_Length);


            //4

            GL.glVertex3f(i_RootX + i_Length, i_RootY, i_RootZ);

            GL.glVertex3f(i_RootX + i_Length, i_RootY, i_RootZ + i_Length);

            GL.glVertex3f(i_RootX + i_Length, i_RootY + i_Length, i_RootZ + i_Length);
            GL.glVertex3f(i_RootX + i_Length, i_RootY + i_Length, i_RootZ);


            //5

            GL.glVertex3f(i_RootX + i_Length, i_RootY + i_Length, i_RootZ + i_Length);


            GL.glVertex3f(i_RootX + i_Length, i_RootY + i_Length, i_RootZ);

            GL.glVertex3f(i_RootX, i_RootY + i_Length, i_RootZ);

            GL.glVertex3f(i_RootX, i_RootY + i_Length, i_RootZ + i_Length);


            //6


            GL.glVertex3f(i_RootX + i_Length, i_RootY + i_Length, i_RootZ + i_Length);

            GL.glVertex3f(i_RootX, i_RootY + i_Length, i_RootZ + i_Length);

            GL.glVertex3f(i_RootX, i_RootY, i_RootZ + i_Length);

            GL.glVertex3f(i_RootX + i_Length, i_RootY, i_RootZ + i_Length);


            GL.glEnd();
        }

        private void drawNormalAxis(float i_Length, float i_RootX, float i_RootY, float i_RootZ,
            float i_Red, float i_Green, float i_Blue)
        {
            GL.glBegin(GL.GL_LINES);

            GL.glColor3f(i_Red, i_Green, i_Blue);
            GL.glVertex3f(i_RootX, i_RootY, i_RootZ);
            GL.glVertex3f(i_RootX, i_RootY + i_Length, i_RootZ);

            GL.glEnd();

            GLUquadric obj = GLU.gluNewQuadric();

            GL.glPushMatrix();

            GL.glTranslatef(i_RootX, i_RootY + i_Length, i_RootZ);
            GL.glRotatef(270.0f, 1.0f, 0.0f, 0.0f);
            GLU.gluCylinder(obj, 0.2, 0.01, 0.5, 16, 16);
            GL.glPopMatrix();

            GLU.gluDeleteQuadric(obj);
        }

        private void drawForwardAxis(float i_Length, float i_RootX, float i_RootY, float i_RootZ,
            float i_Red, float i_Green, float i_Blue)
        {
            GL.glBegin(GL.GL_LINES);

            GL.glColor3f(i_Red, i_Green, i_Blue);
            GL.glVertex3f(i_RootX, i_RootY, i_RootZ);
            GL.glVertex3f(i_RootX - i_Length, i_RootY, i_RootZ);

            GL.glEnd();

            GLUquadric obj = GLU.gluNewQuadric();

            GL.glPushMatrix();

            GL.glTranslatef(i_RootX - i_Length, i_RootY, i_RootZ);
            GL.glRotatef(270.0f, 0.0f, 1.0f, 0.0f);
            GLU.gluCylinder(obj, 0.2, 0.01, 0.5, 16, 16);
            GL.glPopMatrix();

            GLU.gluDeleteQuadric(obj);
        }

        private void drawBackwardAxis(float i_Length, float i_RootX, float i_RootY, float i_RootZ,
    float i_Red, float i_Green, float i_Blue)
        {
            GL.glBegin(GL.GL_LINES);

            GL.glColor3f(i_Red, i_Green, i_Blue);
            GL.glVertex3f(i_RootX, i_RootY, i_RootZ);
            GL.glVertex3f(i_RootX + i_Length, i_RootY, i_RootZ);

            GL.glEnd();

            GLUquadric obj = GLU.gluNewQuadric();

            GL.glPushMatrix();

            GL.glTranslatef(i_RootX + i_Length, i_RootY, i_RootZ);
            GL.glRotatef(90.0f, 0.0f, 1.0f, 0.0f);
            GLU.gluCylinder(obj, 0.2, 0.01, 0.5, 16, 16);
            GL.glPopMatrix();

            GLU.gluDeleteQuadric(obj);
        }

        private void drawCube(float i_Red, float i_Green, float i_Blue)
        {
            GL.glBegin(GL.GL_QUADS);
            GL.glColor3f(i_Red, i_Green, i_Blue);
            //GL.glBindTexture(GL.GL_TEXTURE_2D, texture[1]);
            //front
            GL.glNormal3f(0.0f, 0.0f, 1.0f);
            GL.glTexCoord2f(0.0f, 0.0f);
            GL.glVertex3f(-10.0f, -10.0f, 10.0f);
            GL.glTexCoord2f(1.0f, 0.0f);
            GL.glVertex3f(10.0f, -10.0f, 10.0f);
            GL.glTexCoord2f(1.0f, 1.0f);
            GL.glVertex3f(10.0f, 10.0f, 10.0f);
            GL.glTexCoord2f(0.0f, 1.0f);
            GL.glVertex3f(-10.0f, 10.0f, 10.0f);

            //back
            GL.glNormal3f(0.0f, 0.0f, -1.0f);
            GL.glTexCoord2f(0.0f, 0.0f);
            GL.glVertex3f(-10.0f, -10.0f, -10.0f);
            GL.glTexCoord2f(1.0f, 0.0f);
            GL.glVertex3f(10.0f, -10.0f, -10.0f);
            GL.glTexCoord2f(1.0f, 1.0f);
            GL.glVertex3f(10.0f, 10.0f, -10.0f);
            GL.glTexCoord2f(0.0f, 1.0f);
            GL.glVertex3f(-10.0f, 10.0f, -10.0f);
            //top
            GL.glNormal3f(0.0f, 1.0f, 0.0f);
            GL.glTexCoord2f(0.0f, 0.0f);
            GL.glVertex3f(10.0f, 10.0f, 10.0f);
            GL.glTexCoord2f(1.0f, 0.0f);
            GL.glVertex3f(10.0f, 10.0f, -10.0f);
            GL.glTexCoord2f(1.0f, 0.0f);
            GL.glVertex3f(-10.0f, 10.0f, -10.0f);
            GL.glTexCoord2f(0.0f, 1.0f);
            GL.glVertex3f(-10.0f, 10.0f, 10.0f);
            //bottom
            GL.glNormal3f(0.0f, -1.0f, 0.0f);
            GL.glTexCoord2f(0.0f, 0.0f);
            GL.glVertex3f(10.0f, -10.0f, 10.0f);
            GL.glTexCoord2f(1.0f, 0.0f);
            GL.glVertex3f(10.0f, -10.0f, -10.0f);
            GL.glTexCoord2f(1.0f, 1.0f);
            GL.glVertex3f(-10.0f, -10.0f, -10.0f);
            GL.glTexCoord2f(0.0f, 1.0f);
            GL.glVertex3f(-10.0f, -10.0f, 10.0f);
            //right
            GL.glNormal3f(1.0f, 0.0f, 0.0f);
            GL.glTexCoord2f(0.0f, 0.0f);
            GL.glVertex3f(10.0f, -10.0f, 10.0f);
            GL.glTexCoord2f(1.0f, 0.0f);
            GL.glVertex3f(10.0f, -10.0f, -10.0f);
            GL.glTexCoord2f(1.0f, 1.0f);
            GL.glVertex3f(10.0f, 10.0f, -10.0f);
            GL.glTexCoord2f(0.0f, 1.0f);
            GL.glVertex3f(10.0f, 10.0f, 10.0f);
            //left
            GL.glNormal3f(-1.0f, 0.0f, -1.0f);
            GL.glTexCoord2f(0.0f, 0.0f);
            GL.glVertex3f(-10.0f, -10.0f, 10.0f);
            GL.glTexCoord2f(1.0f, 0.0f);
            GL.glVertex3f(-10.0f, -10.0f, -10.0f);
            GL.glTexCoord2f(1.0f, 1.0f);
            GL.glVertex3f(-10.0f, 10.0f, -10.0f);
            GL.glTexCoord2f(0.0f, 1.0f);
            GL.glVertex3f(-10.0f, 10.0f, 10.0f);
            GL.glEnd();
        }

        private void drawBed()
        {
            // Rotate X + Translate X = BUG.
            //bed head
            GL.glPushMatrix();
            GL.glTranslatef(5.0f, 2.15f, 0.5f);
            GL.glPushMatrix(); // Save the mat state
            GL.glTranslatef(-4f, -0.49f, 6.2f); // Change the position of the object by adding values ​​to the axes(x,y,z).
            GL.glScalef(0.05f, 0.16f, 0.5f); // Change the object size. scale(2,2,2) make it bigger twice
            drawCube(1.0f, 1.0f, 0.0f);
            GL.glPopMatrix();

            //bed body
            GL.glPushMatrix();
            GL.glTranslatef(0f, -1.1f, 6.2f);
            GL.glScalef(0.4f, 0.1f, 0.5f); //1, 0.2, 0.9
            drawCube(1.0f, 1.0f, 0.0f);
            GL.glPopMatrix();

            //pillow right far
            GL.glColor3f(0.627f, 0.322f, 0.176f);
            GL.glPushMatrix();
            GL.glTranslatef(-2.8f, 0.68f, 3.8f);
            GL.glRotatef(40f, 0f, 0f, 1f);
            GL.glScalef(0.025f, 0.08f, 0.2f);
            drawCube(1.0f, 1.0f, 0.0f);
            GL.glPopMatrix();

            //pillow left near
            GL.glColor3f(0.627f, 0.322f, 0.176f);
            GL.glPushMatrix();
            GL.glTranslatef(-2.8f, 0.68f, 8.8f);
            GL.glRotatef(40f, 0f, 0f, 1f);
            GL.glScalef(0.025f, 0.08f, 0.2f);
            drawCube(1.0f, 1.0f, 0.0f);
            GL.glPopMatrix();

            //blanket
            GL.glColor3f(0.627f, 0.322f, 0.176f);
            GL.glPushMatrix();
            GL.glTranslatef(1.51f, -0.15f, 6.15f);
            //glRotatef(22, 0,0,1);
            GL.glScalef(0.25f, 0.025f, 0.51f);
            drawCube(1.0f, 1.0f, 0.0f);
            GL.glPopMatrix();
            GL.glPopMatrix();
        }

        private void drawCloset()
        {
            GL.glPushMatrix();
            GL.glTranslatef(15.0f, 0.5f, 0.0f);
            GL.glScalef(1.5f, 1.5f, 1.2f);
            
            float shelfHeight = 0.0f;

            for (int i = 0; i < 5; i++)
            {
                //wall shelf
                GL.glPushMatrix();
                GL.glTranslatef(1.5f, shelfHeight, 3.0f);
                GL.glScalef(0.4f, 0.03f, 0.2f);
                drawCube(1.0f, 1.0f, 0.0f);
                GL.glPopMatrix();

                shelfHeight += 2.5f;
            }

            float drawerXLength = -2.43f;

            for (int i = 0; i < 4; i++)
            {
                // drawer
                GL.glPushMatrix();
                GL.glTranslatef(drawerXLength, 1.0f, 3.0f);

                // left side
                GL.glPushMatrix();
                GL.glTranslatef(0.0f, 0.25f, 0.0f);
                GL.glScalef(0.005f, 0.095f, 0.2f);
                drawCube(1.0f, 0.0f, 1.0f);
                GL.glPopMatrix();

                // right side
                GL.glPushMatrix();
                GL.glTranslatef(1.9f, 0.25f, 0.0f);
                GL.glScalef(0.005f, 0.095f, 0.2f);
                drawCube(1.0f, 0.0f, 0.0f);
                GL.glPopMatrix();

                // down side
                GL.glPushMatrix();
                GL.glTranslatef(0.95f, -0.63f, 0.0f);
                GL.glScalef(0.088f, 0.005f, 0.2f);
                drawCube(1.0f, 1.0f, 1.0f);
                GL.glPopMatrix();

                // front side
                GL.glPushMatrix();
                GL.glTranslatef(0.95f, 0.2f, 2.02f);
                GL.glScalef(0.1f, 0.15f, 0.001f);
                drawCube(1.0f, 1.0f, 1.0f);
                GL.glPopMatrix();

                GL.glPopMatrix();

                drawerXLength += 1.99f;
            }

            // left side
            GL.glPushMatrix();
            GL.glTranslatef(-2.5f, 5.0f, 3f);
            GL.glScalef(0.001f, 0.53f, 0.2f);
            drawCube(1.0f, 0.0f, 1.0f);
            GL.glPopMatrix();

            // right side
            GL.glPushMatrix();
            GL.glTranslatef(5.5f, 5.0f, 3f);
            GL.glScalef(0.001f, 0.53f, 0.2f);
            drawCube(1.0f, 0.0f, 1.0f);
            GL.glPopMatrix();

            //back side
            /*
            GL.glPushMatrix();
            GL.glRotatef(90.0f, 0.0f, 1.0f, 0.0f);
            GL.glTranslatef(-1.0f, 5.0f, 1.5f);
            GL.glScalef(0.001f, 0.53f, 0.4f);
            drawCube();
            GL.glPopMatrix();
            */

            GL.glPopMatrix();
        }
    }

}

