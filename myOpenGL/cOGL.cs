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
        float[,] floor = new float[3, 3];
        float[] lightPos = new float[4];
        int Width;
        int Height;

        public cOGL(Control pb)
        {
            p = pb;
            Width = p.Width;
            Height = p.Height;

            floor[0, 0] = 12.5f;
            floor[0, 1] = 0f;
            floor[0, 2] = 25f;

            floor[1, 0] = -12.5f;
            floor[1, 1] = 0f;
            floor[1, 2] = 25f;

            floor[2, 0] = -12.5f;
            floor[2, 1] = 0f;
            floor[2, 2] = 0f;

            lightPos[0] = 12.5f;
            lightPos[1] = 17f;
            lightPos[2] = 12.5f;
            lightPos[3] = 1.0f;
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
            GL.glPushMatrix(); // Save the current transformation matrix
            GL.glTranslatef(-12.5f, 0.0f, 0.0f);
            drawFloor(25f, 0f, 0f, 0f);
            drawLamp();

            GL.glEnable(GL.GL_LIGHTING);

            DrawObjects(false, 1);

            GL.glDisable(GL.GL_LIGHTING);
            // Draw shadow
            GL.glPushMatrix(); // Save matrix for shadow drawing
            MakeShadowMatrix(floor);
            GL.glMultMatrixf(cubeXform);
            DrawObjects(true, 1);
            GL.glPopMatrix(); // Restore matrix after drawing shadow

            GL.glPopMatrix(); // Restore the initial transformation matrix
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

            //DrawAxes();

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

            //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

            //for Stencil support 

            pfd.cStencilBits = 32;

            //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

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
            if (m_uint_DC == 0 || m_uint_RC == 0 || this.Width == 0 || this.Height == 0)
                return;

            // Set the clear color to a dark gray
            GL.glClearColor(0.1f, 0.1f, 0.1f, 1.0f);

            // Enable depth testing
            GL.glEnable(GL.GL_DEPTH_TEST);
            GL.glDepthFunc(GL.GL_LEQUAL);

            // Enable lighting and color material
            GL.glEnable(GL.GL_LIGHTING);
            GL.glEnable(GL.GL_LIGHT0);
            GL.glEnable(GL.GL_COLOR_MATERIAL);

            // Configure the light properties
            float[] ambientLight = { 0.2f, 0.2f, 0.2f, 1.0f };  // Ambient light
            float[] diffuseLight = { 0.8f, 0.8f, 0.8f, 1.0f };  // Diffuse light
            float[] specularLight = { 1.0f, 1.0f, 1.0f, 1.0f }; // Specular light

            // Set light properties
            GL.glLightfv(GL.GL_LIGHT0, GL.GL_AMBIENT, ambientLight);
            GL.glLightfv(GL.GL_LIGHT0, GL.GL_DIFFUSE, diffuseLight);
            GL.glLightfv(GL.GL_LIGHT0, GL.GL_SPECULAR, specularLight);
            GL.glLightfv(GL.GL_LIGHT0, GL.GL_POSITION, lightPos);

            // Set the viewport to match the window size
            GL.glViewport(0, 0, this.Width, this.Height);

            // Set up the projection matrix
            GL.glMatrixMode(GL.GL_PROJECTION);
            GL.glLoadIdentity();
            GLU.gluPerspective(45.0, 1.0, 0.4, 100.0);

            // Switch to the modelview matrix and reset it
            GL.glMatrixMode(GL.GL_MODELVIEW);
            GL.glLoadIdentity();

            // Save the current MODELVIEW Matrix (now it is Identity)
            GL.glGetDoublev(GL.GL_MODELVIEW_MATRIX, AccumulatedRotationsTraslations);

            // Initialize texture
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

        private void drawCube()
        {
            GL.glBegin(GL.GL_QUADS);
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
            drawCube();
            GL.glPopMatrix();

            //bed body
            GL.glPushMatrix();
            GL.glTranslatef(0f, -1.1f, 6.2f);
            GL.glScalef(0.4f, 0.1f, 0.5f); //1, 0.2, 0.9
            drawCube();
            GL.glPopMatrix();

            //pillow right far
            GL.glPushMatrix();
            GL.glTranslatef(-2.8f, 0.68f, 3.8f);
            GL.glRotatef(40f, 0f, 0f, 1f);
            GL.glScalef(0.025f, 0.08f, 0.2f);
            drawCube();
            GL.glPopMatrix();

            //pillow left near
            GL.glPushMatrix();
            GL.glTranslatef(-2.8f, 0.68f, 8.8f);
            GL.glRotatef(40f, 0f, 0f, 1f);
            GL.glScalef(0.025f, 0.08f, 0.2f);
            drawCube();
            GL.glPopMatrix();

            //blanket
            GL.glPushMatrix();
            GL.glTranslatef(1.51f, -0.15f, 6.15f);
            //glRotatef(22, 0,0,1);
            GL.glScalef(0.25f, 0.025f, 0.51f);
            drawCube();
            GL.glPopMatrix();
            GL.glPopMatrix();
        }

        private void drawCloset()
        {
            GL.glPushMatrix();
            GL.glTranslatef(15.0f, 0.5f, 0.0f);
            GL.glScalef(1.5f, 1.5f, 1.2f);
            
            float shelfHeightDelta = 0.0f;

            for (int i = 0; i < 5; i++)
            {
                //wall shelf
                GL.glPushMatrix();
                GL.glTranslatef(1.5f, shelfHeightDelta, 3.0f);
                GL.glScalef(0.4f, 0.03f, 0.2f);
                drawCube();
                GL.glPopMatrix();

                shelfHeightDelta += 2.5f;
            }

            float drawerXDelta = -2.43f;

            for (int i = 0; i < 4; i++)
            {
                // drawer
                GL.glPushMatrix();
                GL.glTranslatef(drawerXDelta, 1.0f, 3.0f);

                // left side
                GL.glPushMatrix();
                GL.glTranslatef(0.0f, 0.25f, 0.0f);
                GL.glScalef(0.005f, 0.095f, 0.2f);
                drawCube();
                GL.glPopMatrix();

                // right side
                GL.glPushMatrix();
                GL.glTranslatef(1.9f, 0.25f, 0.0f);
                GL.glScalef(0.005f, 0.095f, 0.2f);
                drawCube();
                GL.glPopMatrix();

                // down side
                GL.glPushMatrix();
                GL.glTranslatef(0.95f, -0.63f, 0.0f);
                GL.glScalef(0.088f, 0.005f, 0.2f);
                drawCube();
                GL.glPopMatrix();

                // front side
                GL.glPushMatrix();
                GL.glTranslatef(0.95f, 0.2f, 2.02f);
                GL.glScalef(0.1f, 0.15f, 0.001f);
                drawCube();
                GL.glPopMatrix();

                GL.glPopMatrix();

                drawerXDelta += 1.99f;
            }

            float doorXDelta = 2f;

            GL.glPushMatrix();
            GL.glTranslatef(-0.5f, 0f, 2.02f);

            for (int i = 0; i < 2; i++)
            {
                // left door
                GL.glPushMatrix();
                GL.glTranslatef(-0.96f, 6.51f, 3.0f);
                GL.glScalef(0.105f, 0.376f, 0.005f);
                drawCube();
                GL.glPopMatrix();

                // right door
                GL.glPushMatrix();
                GL.glTranslatef(2.95f, 6.51f, 3.0f);
                GL.glScalef(0.105f, 0.376f, 0.005f);
                drawCube();
                GL.glPopMatrix();

                GL.glTranslatef(doorXDelta, 0.0f, 0.0f);
            }

            GL.glPopMatrix();

            // left side
            GL.glPushMatrix();
            GL.glTranslatef(-2.5f, 5.0f, 3f);
            GL.glScalef(0.001f, 0.53f, 0.2f);
            drawCube();
            GL.glPopMatrix();

            // right side
            GL.glPushMatrix();
            GL.glTranslatef(5.5f, 5.0f, 3f);
            GL.glScalef(0.001f, 0.53f, 0.2f);
            drawCube();
            GL.glPopMatrix();

            //back side
            
            GL.glPushMatrix();
            GL.glRotatef(90.0f, 0.0f, 1.0f, 0.0f);
            GL.glTranslatef(-1.0f, 5.0f, 1.5f);
            GL.glScalef(0.001f, 0.53f, 0.4f);
            drawCube();
            GL.glPopMatrix();
            

            GL.glPopMatrix();
        }

        private void drawWindow()
        {
            GL.glPushMatrix();

            GL.glRotatef(90f, 0.0f, 1f, 0.0f);
            GL.glTranslatef(-12.0f, 8f, 0f);

            // left side
            GL.glPushMatrix();
            GL.glScalef(0.01f, 0.3f, 0.01f);

            drawCube();
            GL.glPopMatrix();

            // right side
            GL.glPushMatrix();
            GL.glTranslatef(5f, 0f, 0f);
            GL.glScalef(0.01f, 0.3f, 0.01f);
            drawCube();
            GL.glPopMatrix();

            GL.glPopMatrix();

            GL.glPushMatrix();

            // down side
            GL.glRotatef(90f, 1f, 0f, 0f);
            GL.glTranslatef(0f, 9.5f, -5f);

            GL.glPushMatrix();
            GL.glScalef(0.02f, 0.27f, 0.01f);

            drawCube();
            GL.glPopMatrix();

            // upper side
            GL.glPushMatrix();
            GL.glTranslatef(0f, 0f, -6f);
            GL.glScalef(0.02f, 0.27f, 0.01f);

            drawCube();
            GL.glPopMatrix();

            float height = 0.6f;
            // Blinds
            for (int i = 0; i < 10; i++)
            {
                GL.glTranslatef(0f, 0f, -height);

                GL.glPushMatrix();
                GL.glScalef(0.02f, 0.24f, 0.01f);

                drawCube();
                GL.glPopMatrix();
            }

            GL.glPopMatrix();
        }

        private void drawLamp()
        {
            GL.glPushMatrix();
            GL.glTranslatef(12.5f, 17f, 12.5f);
            GLUquadric obj = GLU.gluNewQuadric();
            GL.glEnable(GL.GL_BLEND);

            GL.glBlendFunc(GL.GL_SRC_ALPHA, GL.GL_ONE_MINUS_SRC_ALPHA);
            GL.glColor4f(1.0f, 1.0f, 1.0f, 0.5f);

            GLU.gluSphere(obj, 0.2, 20, 20);
            GL.glDisable(GL.GL_BLEND);

            GL.glColor3f(0f, 0f, 0f);
            GL.glTranslatef(0f, 0.1f, 0f);
            GL.glRotatef(-90f, 1f, 0f, 0f);
            GLU.gluCylinder(obj, 0.2, 0.3, 0.6, 20, 20);

            GLU.gluDeleteQuadric(obj);
            GL.glPopMatrix();

            drawLightSource();
        }

        private void drawLightSource()
        {
            // Draw Light Source
            GL.glDisable(GL.GL_LIGHTING);
            GL.glPushMatrix(); // Save matrix before translating for the light source
            GL.glTranslatef(lightPos[0], lightPos[1], lightPos[2]);

            // Yellow Light source
            GL.glColor3f(1, 1, 0);
            GLUT.glutSolidSphere(0.05, 8, 8);
            GL.glPopMatrix(); // Restore matrix after drawing light source

            // Projection line from source to plane
            GL.glBegin(GL.GL_LINES);
            GL.glColor3d(0.5, 0.5, 0);
            GL.glVertex3d(lightPos[0], floor[0, 1] - 0.01, lightPos[2]);
            GL.glVertex3d(lightPos[0], lightPos[1], lightPos[2]);
            GL.glEnd();
        }

        float[] cubeXform = new float[16];
        private void MakeShadowMatrix(float[,] points)
        {
            float[] planeCoeff = new float[4];
            float dot;

            // Find the plane equation coefficients
            // Find the normal based on the adjusted coordinate system
            calcNormal(points, planeCoeff);

            // Find the last coefficient by back substitutions
            planeCoeff[3] = -(
                (planeCoeff[0] * points[2, 0]) + (planeCoeff[1] * points[2, 1]) +
                (planeCoeff[2] * points[2, 2]));

            // Dot product of the plane and light position
            dot = planeCoeff[0] * lightPos[0] +
                  planeCoeff[1] * lightPos[1] +
                  planeCoeff[2] * lightPos[2] +
                  planeCoeff[3];

            // Now do the projection
            // Adjust the cubeXform based on the new axis orientation
            // First column
            cubeXform[0] = dot - lightPos[0] * planeCoeff[0];  // X axis
            cubeXform[4] = 0.0f - lightPos[0] * planeCoeff[1]; // Y axis (up)
            cubeXform[8] = 0.0f - lightPos[0] * planeCoeff[2]; // Z axis (forward)
            cubeXform[12] = 0.0f - lightPos[3] * planeCoeff[3];

            // Second column
            cubeXform[1] = 0.0f - lightPos[1] * planeCoeff[0]; // X axis
            cubeXform[5] = dot - lightPos[1] * planeCoeff[1];  // Y axis (up)
            cubeXform[9] = 0.0f - lightPos[1] * planeCoeff[2];  // Z axis (forward)
            cubeXform[13] = 0.0f - lightPos[3] * planeCoeff[3];

            // Third Column
            cubeXform[2] = 0.0f - lightPos[2] * planeCoeff[0]; // X axis
            cubeXform[6] = 0.0f - lightPos[2] * planeCoeff[1]; // Y axis (up)
            cubeXform[10] = dot - lightPos[2] * planeCoeff[2]; // Z axis (forward)
            cubeXform[14] = 0.0f - lightPos[3] * planeCoeff[3];

            // Fourth Column (homogeneous coordinate)
            cubeXform[3] = 0.0f; // Assuming the lightPos[3] is set for the perspective (w-component)
            cubeXform[7] = 0.0f; // No change in Y for the homogeneous part
            cubeXform[11] = 0.0f; // No change in Z for the homogeneous part
            cubeXform[15] = dot - lightPos[3] * planeCoeff[3]; // Typically for perspective divide
        }

        const int x = 0;
        const int y = 1;
        const int z = 2;

        private void calcNormal(float[,] v, float[] outp)
        {
            float[] v1 = new float[3];
            float[] v2 = new float[3];

            // Calculate two vectors from the three points
            v1[0] = v[0, 0] - v[1, 0]; // X1 - X2
            v1[2] = v[0, 2] - v[1, 2]; // Z1 - Z2 (use v[0, 2] for Y)
            v1[1] = v[0, 1] - v[1, 1]; // Y1 - Y2 (use v[0, 1] for Z)

            v2[0] = v[1, 0] - v[2, 0]; // X2 - X3
            v2[2] = v[1, 2] - v[2, 2]; // Z2 - Z3 (use v[1, 2] for Y)
            v2[1] = v[1, 1] - v[2, 1]; // Y2 - Y3 (use v[1, 1] for Z)

            // Take the cross product of the two vectors to get
            // the normal vector which will be stored in outp
            outp[0] = v1[2] * v2[1] - v1[1] * v2[2]; // X-component
            outp[1] = v1[0] * v2[2] - v1[2] * v2[0]; // Y-component
            outp[2] = v1[1] * v2[0] - v1[0] * v2[1]; // Z-component

            // Normalize the vector (shorten length to one)
            ReduceToUnit(outp);
        }

        private void ReduceToUnit(float[] vector)
        {
            float length;

            // Calculate the length of the vector		
            length = (float)Math.Sqrt((vector[0] * vector[0]) +
                                       (vector[1] * vector[1]) +
                                       (vector[2] * vector[2]));

            // Prevent division by zero for vectors too close to zero.
            if (length == 0.0f)
                length = 1.0f;

            // Normalize the vector
            vector[0] /= length; // X-component
            vector[1] /= length; // Y-component
            vector[2] /= length; // Z-component
        }

        private void DrawObjects(bool isForShades, int c)
        {
            if (isForShades)
            {
                if (c == 1)
                    GL.glColor3d(0.5, 0.5, 0.5);
                else
                    GL.glColor3d(0.8, 0.8, 0.8);
            }
            else
            {
                GL.glColor3d(0.3f, 0.6f, 0.9f);
            }

            drawBed();
            drawCloset();
            drawWindow();
        }
    }

}

