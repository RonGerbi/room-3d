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
        private uint[] texture;
        Control p;
        float[,] floor = new float[3, 3];
        float[] lightPos = new float[4];
<<<<<<< Updated upstream
=======
        public float doorAngle = 0.0f;
        public float ballAngle = 0.0f;
        public float fishAngle = 0.0f;
        public bool isDoorOpen = false;
        public bool fishbool = false;
        public bool isCeilingLightBulbOn = false;
        public bool applyShadows = false;
        public bool ballInRight = false;
        public bool doorFlag = false;
>>>>>>> Stashed changes
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

<<<<<<< Updated upstream
            texture = new uint[2];		// storage for texture
=======
            string[] imageFiles = { "wood.bmp", "soccer_ball.bmp", "bed_body.bmp", "blanket.bmp", "Pillow.bmp",
          "door.bmp", "closet.bmp", "drawers.bmp", "doors_right.bmp", "mirror.bmp", "dressingTable.bmp",
          "LED.bmp", "clothes.bmp", "jeans.bmp" , "aqua_table.bmp", "aqua_bottom.bmp","aqua_back2.bmp",
          "closet_poster2.bmp", "water_proof.bmp", "bed_pannel.bmp"};
>>>>>>> Stashed changes

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
            //if (doorFlag)
            //{
            //    GL.glColor3f(1.0f, 1.0f, 1.0f);
            //    doorFlag = false;
            //}
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
<<<<<<< Updated upstream
            GL.glPushMatrix(); // Save the mat state
            GL.glTranslatef(-4f, -0.49f, 6.2f); // Change the position of the object by adding values ​​to the axes(x,y,z).
            GL.glScalef(0.05f, 0.16f, 0.5f); // Change the object size. scale(2,2,2) make it bigger twice
            drawCube();
            GL.glPopMatrix();

            //bed body
            GL.glPushMatrix();
            GL.glTranslatef(0f, -1.1f, 6.2f);
            GL.glScalef(0.4f, 0.1f, 0.5f); //1, 0.2, 0.9
=======

            drawScaledTranslatedCube(-4f, -0.49f, 6.2f, 0.05f, 0.16f, 0.5f);

            if (i_DrawWithTexturesAndColors)
            {
                GL.glDisable(GL.GL_TEXTURE_2D);
            }

            //bed body
            if (i_DrawWithTexturesAndColors)
            {
                GL.glEnable(GL.GL_TEXTURE_2D);
                GL.glBindTexture(GL.GL_TEXTURE_2D, texture[2]);
            }

            drawScaledTranslatedCube(0f, -1.1f, 6.2f, 0.4f, 0.1f, 0.5f);

            if (i_DrawWithTexturesAndColors)
            {
                GL.glDisable(GL.GL_TEXTURE_2D);
            }

            // bed panel
            // front
            GL.glEnable(GL.GL_TEXTURE_2D);
            GL.glBindTexture(GL.GL_TEXTURE_2D, texture[19]);
            GL.glPushMatrix();
            GL.glTranslatef(-0.23f, -1.3f, 11.2f);
            GL.glRotatef(90.0f, 1.0f, 0.0f, 0.0f);
            GL.glScalef(0.426f, 0.001f, 0.084f); //0.425f
            drawCube();
            GL.glPopMatrix();

            // back
            GL.glPushMatrix();
            GL.glTranslatef(-0.26f, -1.3f, 1.2f);
            GL.glRotatef(90.0f, 1.0f, 0.0f, 0.0f);
            GL.glScalef(0.425f, 0.001f, 0.084f);
            drawCube();
            GL.glPopMatrix();

            // right
            GL.glPushMatrix();
            GL.glTranslatef(4.01f, -1.3f, 6.2f);
            GL.glRotatef(90.0f, 0.0f, 0.0f, 1.0f);
            GL.glScalef(0.0843f, 0.001f, 0.501f);
            drawCube();
            GL.glPopMatrix();

            //left
            GL.glPushMatrix();
            GL.glTranslatef(-4.499f, -1.3f, 6.2f);
            GL.glRotatef(90.0f, 0.0f, 0.0f, 1.0f);
            GL.glScalef(0.0843f, 0.001f, 0.501f);
            drawCube();
            GL.glPopMatrix();
            GL.glDisable(GL.GL_TEXTURE_2D);

            drawPillow(-2.8f, 0.68f, 3.8f, 40f, i_DrawWithTexturesAndColors);
            drawPillow(-2.8f, 0.68f, 8.8f, 40f, i_DrawWithTexturesAndColors);

            //blanket
            if (i_DrawWithTexturesAndColors)
            {
                GL.glColor3f(1.0f, 0.98f, 0.8f);
                GL.glEnable(GL.GL_TEXTURE_2D);
                GL.glBindTexture(GL.GL_TEXTURE_2D, texture[3]);
            }

            GL.glPushMatrix();
            GL.glTranslatef(1.51f, -0.15f, 6.20f);
            GL.glScalef(0.25f, 0.015f, 0.5f);
>>>>>>> Stashed changes
            drawCube();
            GL.glPopMatrix();

            //pillow right far
            GL.glPushMatrix();
<<<<<<< Updated upstream
            GL.glTranslatef(-2.8f, 0.68f, 3.8f);
            GL.glRotatef(40f, 0f, 0f, 1f);
=======
            GL.glTranslatef(x, y, z);
            GL.glScalef(scaleX, scaleY, scaleZ);
            drawCube();
            GL.glPopMatrix();
        }

        private void drawPillow(float x, float y, float z, float rotationAngle, bool i_DrawWithTexturesAndColors)
        {
            if (i_DrawWithTexturesAndColors)
            {
                //GL.glColor3f(0.627f, 0.322f, 0.176f);
                GL.glColor3f(0.95f, 0.95f, 0.95f);
                GL.glEnable(GL.GL_TEXTURE_2D);
                GL.glBindTexture(GL.GL_TEXTURE_2D, texture[4]);
            }

            GL.glPushMatrix();
            GL.glTranslatef(x, y, z);
            GL.glRotatef(rotationAngle, 0f, 0f, 1f);
>>>>>>> Stashed changes
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
            GL.glColor4f(1.0f, 1.0f, 0.0f, 0.5f);

            GLU.gluSphere(obj, 0.2, 20, 20);
            GL.glDisable(GL.GL_BLEND);


            GL.glColor3f(0.15f, 0.15f, 0.15f);          
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

<<<<<<< Updated upstream
=======
        private void drawSphere(bool i_DrawWithTexturesAndColors)
        {
            GLUquadric obj;
            obj = GLU.gluNewQuadric();

            if (i_DrawWithTexturesAndColors)
            {
                GL.glEnable(GL.GL_TEXTURE_2D);
                GL.glBindTexture(GL.GL_TEXTURE_2D, texture[1]);
                GL.glColor3f(1.0f, 1.0f, 1.0f);
            }

            GL.glDisable(GL.GL_LIGHTING);
            GLU.gluQuadricTexture(obj, (byte)GL.GL_TRUE);
            GL.glPushMatrix();
            GL.glTranslatef(2.8f, 0.60f, 15f);
            GL.glRotatef(120.0f, 0.0f, 1.0f, 0.0f);

            if (!ballInRight)
            {
                ballAngle = Math.Min(ballAngle + 6.0f, 0.0f);
            }

            else
            {
                ballAngle = Math.Max(ballAngle - 6.0f, -360.0f);
            }

            GL.glRotatef(ballAngle, 0.0f, 0.0f, 1.0f);
            GLU.gluSphere(obj, 0.6, 80, 80);
            GL.glPopMatrix();

            if (i_DrawWithTexturesAndColors)
            {
                GL.glDisable(GL.GL_TEXTURE_2D);
            }

            GLU.gluDeleteQuadric(obj);
        }
  
        private void drawCloset(bool i_DrawWithTexturesAndColors)
        {
            if (i_DrawWithTexturesAndColors)
            {
                drawClothes();
            }

            GL.glPushMatrix();
            GL.glTranslatef(15.0f, 0.5f, 0.0f);
            GL.glScalef(1.5f, 1.5f, 1.2f);

            float shelfHeightDelta = 0.0f;

            if (i_DrawWithTexturesAndColors)
            {
                GL.glColor3f(0.8f, 0.8f, 0.8f);
            }

            drawShelves(ref shelfHeightDelta);
            drawDrawers(i_DrawWithTexturesAndColors);
            drawDoors(i_DrawWithTexturesAndColors);
            drawClosetSides(i_DrawWithTexturesAndColors);

            GL.glPopMatrix();
        }

        private void drawShelves(ref float i_ShelfHeightDelta)
        {
            for (int i = 0; i < 5; i++)
            {
                GL.glPushMatrix();
                GL.glTranslatef(1.5f, i_ShelfHeightDelta, 3.0f);
                GL.glScalef(0.4f, 0.03f, 0.2f);
                drawCube();
                GL.glPopMatrix();

                i_ShelfHeightDelta += 2.5f;
            }
        }

        private void drawDrawers(bool i_DrawWithTexturesAndColors)
        {
            float drawerXDelta = -2.43f;

            for (int i = 0; i < 4; i++)
            {
                // drawer
                GL.glPushMatrix();
                GL.glTranslatef(drawerXDelta, 1.0f, 3.0f);

                drawDrawerSides();
                drawDrawerBottom();
                drawDrawerFront(i_DrawWithTexturesAndColors);

                GL.glPopMatrix();

                drawerXDelta += 1.99f;
            }
        }

        private void drawDrawerSides()
        {
            drawScaledCube(0.0f, 0.25f, 0.0f, 0.005f, 0.095f, 0.2f);
            drawScaledCube(1.9f, 0.25f, 0.0f, 0.005f, 0.095f, 0.2f);
        }

        private void drawDrawerBottom()
        {
            drawScaledCube(0.95f, -0.63f, 0.0f, 0.088f, 0.005f, 0.2f);
        }

        private void drawDrawerFront(bool i_DrawWithTexturesAndColors)
        {
            if (i_DrawWithTexturesAndColors)
            {
                GL.glEnable(GL.GL_TEXTURE_2D);
                GL.glBindTexture(GL.GL_TEXTURE_2D, texture[7]);
            }

            drawScaledCube(0.95f, 0.2f, 2.02f, 0.1f, 0.15f, 0.001f);

            if (i_DrawWithTexturesAndColors)
            {
                GL.glDisable(GL.GL_TEXTURE_2D);
            }
        }

        private void drawDoors(bool i_DrawWithTexturesAndColors)
        {
            GL.glPushMatrix();
            GL.glTranslatef(-0.5f, 0f, 2.02f);

            // left door
            drawDynamicLeftDoor(isDoorOpen, i_DrawWithTexturesAndColors);

            // right door
            if (i_DrawWithTexturesAndColors)
            {
                GL.glEnable(GL.GL_TEXTURE_2D);
                GL.glBindTexture(GL.GL_TEXTURE_2D, texture[5]);
            }

            drawScaledCube(2.95f, 6.51f, 3.0f, 0.105f, 0.376f, 0.005f);

            if (i_DrawWithTexturesAndColors)
            {
                GL.glDisable(GL.GL_TEXTURE_2D);
            }

            GL.glTranslatef(2f, 0.0f, 0.0f);

            // left door
            if (i_DrawWithTexturesAndColors)
            {
                GL.glEnable(GL.GL_TEXTURE_2D);
                GL.glBindTexture(GL.GL_TEXTURE_2D, texture[8]);
            }

            drawScaledCube(-0.96f, 6.51f, 3.0f, 0.105f, 0.376f, 0.005f);

            if (i_DrawWithTexturesAndColors)
            {
                GL.glDisable(GL.GL_TEXTURE_2D);
            }

            // right door
            if (i_DrawWithTexturesAndColors)
            {
                GL.glEnable(GL.GL_TEXTURE_2D);
                GL.glBindTexture(GL.GL_TEXTURE_2D, texture[8]);
            }

            drawScaledCube(2.95f, 6.51f, 3.0f, 0.105f, 0.376f, 0.005f);

            if (i_DrawWithTexturesAndColors)
            {
                GL.glDisable(GL.GL_TEXTURE_2D);
            }

            GL.glPopMatrix();
        }

        private void drawClosetSides(bool i_DrawWithTexturesAndColors)
        {
            if (i_DrawWithTexturesAndColors)
            {
                GL.glColor3f(0.8f, 0.8f, 0.8f);
            }

            // left side
            drawScaledCube(-2.5f, 5.0f, 3f, 0.001f, 0.53f, 0.2f);

            // right side
            drawScaledCube(5.5f, 5.0f, 3f, 0.001f, 0.53f, 0.2f);

            //back side
            GL.glPushMatrix();
            GL.glRotatef(90.0f, 0.0f, 1.0f, 0.0f);
            GL.glTranslatef(-1.0f, 5.0f, 1.5f);
            GL.glScalef(0.001f, 0.53f, 0.4f);
            drawCube();
            GL.glPopMatrix();
        }

        private void drawScaledCube(float i_X, float i_Y, float i_Z, float i_ScaleX, float i_ScaleY, float i_ScaleZ)
        {
            GL.glPushMatrix();
            GL.glTranslatef(i_X, i_Y, i_Z);
            GL.glScalef(i_ScaleX, i_ScaleY, i_ScaleZ);
            drawCube();
            GL.glPopMatrix();
        }

        private void drawMirror(bool i_DrawWithTexturesAndColors)
        {
            GL.glEnable(GL.GL_STENCIL_TEST);
            GL.glClear(GL.GL_STENCIL_BUFFER_BIT);

            // Draw the mirror
            GL.glStencilFunc(GL.GL_ALWAYS, 1, 1);
            GL.glStencilOp(GL.GL_KEEP, GL.GL_KEEP, GL.GL_REPLACE);

            GL.glPushMatrix();

            if (i_DrawWithTexturesAndColors)
            {
                GL.glEnable(GL.GL_TEXTURE_2D);
                GL.glBindTexture(GL.GL_TEXTURE_2D, texture[9]);
            }

            GL.glTranslatef(23.47f, 6.2f, 21.7f);
            GL.glRotatef(90.0f, 0.0f, 0.0f, 1.0f);
            GL.glScalef(0.28f, 0.001f, 0.20f);
            drawCube();

            if (i_DrawWithTexturesAndColors)
            {
                GL.glDisable(GL.GL_TEXTURE_2D);
            }

            GL.glPopMatrix();

            // Configure stencil buffer for reflection
            GL.glStencilFunc(GL.GL_EQUAL, 1, 1);
            GL.glStencilOp(GL.GL_KEEP, GL.GL_KEEP, GL.GL_KEEP);

            // Draw the reflected object
            GL.glPushMatrix();
            GL.glScalef(1.0f, -1.0f, 1.0f); // Flip the object for reflection
            GL.glTranslatef(0.0f, -12.4f, 0.0f); // Adjust position for reflection
            drawCube(); // Example of reflected cube, color red
            GL.glPopMatrix();

            GL.glDisable(GL.GL_STENCIL_TEST);
        }

        private void drawDressingTable(bool i_DrawWithTexturesAndColors)
        {
            // mirror
            drawMirror(i_DrawWithTexturesAndColors);

            if (i_DrawWithTexturesAndColors)
            {
                GL.glColor3f(0.85f, 0.8f, 0.7f);

                GL.glEnable(GL.GL_TEXTURE_2D);
                GL.glBindTexture(GL.GL_TEXTURE_2D, texture[10]);
            }

            // table left leg
            drawScaledCube(23.0f, 1.8f, 19.89f, 0.05f, 0.17f, 0.03f);

            // table right leg
            drawScaledCube(23.0f, 1.8f, 23.5f, 0.05f, 0.17f, 0.03f);

            // table plate
            drawScaledCube(23.0f, 3.5f, 21.7f, 0.05f, 0.016f, 0.21f);

            // table upper panel
            drawScaledCube(22.989f, 8.94f, 21.7f, 0.05f, 0.01f, 0.21f);

            // table left panel
            GL.glPushMatrix();
            GL.glTranslatef(23.5f, 6.0f, 19.74f);
            GL.glRotatef(90.0f, 1.0f, 0.0f, 0.0f);
            GL.glScalef(0.001f, 0.008f, 0.29f);
            drawCube();
            GL.glPopMatrix();

            // table right panel
            GL.glPushMatrix();
            GL.glTranslatef(23.48f, 6.0f, 23.78f);
            GL.glRotatef(90.0f, 1.0f, 0.0f, 0.0f);
            GL.glScalef(0.001f, 0.008f, 0.29f);
            drawCube();
            GL.glPopMatrix();

            if (i_DrawWithTexturesAndColors)
            {
                GL.glDisable(GL.GL_TEXTURE_2D);
            }

            if (i_DrawWithTexturesAndColors)
            {
                GL.glEnable(GL.GL_TEXTURE_2D);
                GL.glBindTexture(GL.GL_TEXTURE_2D, texture[11]);
                GL.glColor3f(1.0f, 1.0f, 1.0f);
            }

            drawScaledCube(22.989f, 8.73f, 21.7f, 0.02f, 0.01f, 0.15f);

            if (i_DrawWithTexturesAndColors)
            {
                GL.glDisable(GL.GL_TEXTURE_2D);
            }
        }

        private void drawFloorLamp(bool i_DrawWithTexturesAndColors)
        {
            // base of the lamp

            GLUquadric obj = GLU.gluNewQuadric();

            if (i_DrawWithTexturesAndColors)
            {
                GL.glColor3f(0.78f, 0.78f, 0.78f);
            }

            GL.glPushMatrix();
            GL.glTranslatef(22.8f, 0.5f, 16.0f);
            GL.glRotatef(90.0f, 1.0f, 0.0f, 0.0f);
            GLU.gluDisk(obj, 0.0, 1.0, 50, 50);
            GLU.gluCylinder(obj, 1.0, 1.0, 0.3, 50, 50);
            GL.glPopMatrix();

            // column of the lamp

            GL.glPushMatrix();
            GL.glTranslatef(22.8f, 8.1f, 16.0f);
            GL.glRotatef(90.0f, 1.0f, 0.0f, 0.0f);
            GLU.gluDisk(obj, 0.5, 0.5, 200, 200);
            GLU.gluCylinder(obj, 0.05, 0.05, 7.7, 20, 20);
            GL.glPopMatrix();

            // lamp shade
            if (i_DrawWithTexturesAndColors)
            {
                GL.glColor3f(1.0f, 0.8f, 0.6f);
            }

            GL.glPushMatrix();
            GL.glTranslatef(22.8f, 9.05f, 16.0f);
            GL.glRotatef(90.0f, 1.0f, 0.0f, 0.0f);
            GLU.gluCylinder(obj, 1.0f, 0.0f, 1.0f, 30, 30);
            GL.glPopMatrix();

            // light bulb
            if (i_DrawWithTexturesAndColors)
            {
                GL.glColor3f(1.0f, 1.0f, 0.8f);
            }

            GL.glPushMatrix();
            GL.glTranslatef(22.8f, 9.05f, 15.9f);

            GLU.gluSphere(obj, 0.1, 20, 20);
            GL.glPopMatrix();

            GLU.gluDeleteQuadric(obj);
        }

        private void drawPoster()
        {
            GL.glPushMatrix();
            GL.glEnable(GL.GL_BLEND);
            GL.glBlendFunc(GL.GL_SRC_ALPHA, GL.GL_ONE_MINUS_SRC_ALPHA);
            GL.glColor4f(0.8f, 0.8f, 0.8f, 0.9f);
            GL.glEnable(GL.GL_TEXTURE_2D);
            GL.glBindTexture(GL.GL_TEXTURE_2D, texture[17]);
            GL.glTranslatef(21.84f, 10.2f, 6.09f);
            GL.glRotatef(90.0f, 1.0f, 0.0f, 0.0f);
            GL.glScalef(0.1385f, 0.0001f, 0.567f);      
            drawCube();
            GL.glPopMatrix();
            GL.glDisable(GL.GL_BLEND);
            GL.glDisable(GL.GL_TEXTURE_2D);
        }

        private void drawDynamicLeftDoor(bool isDoorOpen, bool i_DrawWithTexturesAndColors)
        {
            if (!isDoorOpen)
            {
                doorAngle = doorAngle >= 0.0f ? doorAngle - 2.0f : doorAngle;

                if (i_DrawWithTexturesAndColors)
                {
                    GL.glEnable(GL.GL_TEXTURE_2D);
                    GL.glBindTexture(GL.GL_TEXTURE_2D, texture[5]);
                }

                GL.glPushMatrix();
                GL.glTranslatef(-0.96f, 6.51f, 3.0f);
                GL.glTranslatef(-1f, 0f, 0f);
                GL.glRotatef(-doorAngle, 0.0f, 1.0f, 0.0f);
                GL.glTranslatef(1f, 0f, 0f);
                GL.glScalef(0.105f, 0.376f, 0.005f);
                doorFlag = true;
                drawCube();
                GL.glPopMatrix();

                if (i_DrawWithTexturesAndColors)
                {
                    GL.glDisable(GL.GL_TEXTURE_2D);
                }
            }

            else
            {
                doorAngle = doorAngle <= 90f ? doorAngle + 2.0f : doorAngle;

                if (i_DrawWithTexturesAndColors)
                {
                    GL.glEnable(GL.GL_TEXTURE_2D);
                    GL.glBindTexture(GL.GL_TEXTURE_2D, texture[5]);
                }
                GL.glPushMatrix();
                GL.glTranslatef(-0.96f, 6.51f, 3.0f);
                GL.glTranslatef(-1f, 0f, 0f);
                GL.glRotatef(doorAngle, 0.0f, -1.0f, 0.0f);
                GL.glTranslatef(1f, 0f, 0f);
                GL.glScalef(0.105f, 0.376f, 0.005f);
                doorFlag = true;
                drawCube();
                GL.glPopMatrix();

                if (i_DrawWithTexturesAndColors)
                {
                    GL.glDisable(GL.GL_TEXTURE_2D);
                }
            }
        }

        private void drawClothes()
        {
            float height = 6.20f;
            uint textureNum = 12;
            const float initialX = 12.5f;
            const float initialZ = 5.0f;
            const float rotationAngle = 90.0f;
            const float scaleX = 0.10f;
            const float scaleY = 0.01f;
            const float scaleZ = 0.12f;
            const float heightIncrement = 3.5f;

            // Set the color for all clothes
            GL.glColor3f(1.0f, 1.0f, 1.0f);

            for (int i = 0; i < 3; i++)
            {
                // Enable texture for each piece of clothing
                GL.glEnable(GL.GL_TEXTURE_2D);
                GL.glBindTexture(GL.GL_TEXTURE_2D, texture[textureNum]);

                // Draw the clothing piece
                GL.glPushMatrix();
                GL.glTranslatef(initialX, height, initialZ);
                GL.glRotatef(rotationAngle, 1.0f, 0.0f, 0.0f);
                GL.glScalef(scaleX, scaleY, scaleZ);
                drawCube();
                GL.glPopMatrix();

                // Disable texture after drawing
                GL.glDisable(GL.GL_TEXTURE_2D);

                // Update height for the next piece of clothing
                height += heightIncrement;

                // Change the texture for the second piece
                if (i == 1)
                {
                    textureNum++;
                }
            }
        }

        private float fishRotating()
        {
            if (fishbool)
            {
                fishAngle = Math.Min(fishAngle + 0.01f, 0.0f);
            }

            else
            {
                fishAngle = Math.Max(fishAngle - 0.01f, -1.5f);
            }

            return fishAngle;
        }

        private void drawFish()
        {
            float x_axis = 3f, y_axis = 4.2f, z_axis = 21.5f;

            for (int i = 0; i <= 2; i++)
            {
                GL.glPushMatrix();
                fishAngle = fishRotating();
                GL.glRotatef(fishAngle, 0.0f, 0.0f, 1.0f);
                GL.glTranslatef(x_axis, y_axis, z_axis);
                GL.glColor3f(1.0f, 1.0f, 0.0f);
                GL.glBegin(GL.GL_POLYGON);
                GL.glVertex2f(-0.7f, -0.05f);
                GL.glVertex2f(-0.75f, -0.1f);
                GL.glVertex2f(-0.85f, -0.05f);
                GL.glVertex2f(-0.75f, 0.0f);
                GL.glEnd();

                GL.glBegin(GL.GL_TRIANGLES);
                GL.glColor3f(0.8f, 0.5f, 0.0f);
                GL.glVertex2f(-0.83f, -0.05f);
                GL.glVertex2f(-0.9f, -0.09f);
                GL.glVertex2f(-0.9f, -0.01f);
                GL.glEnd();

                GL.glBegin(GL.GL_TRIANGLES);
                GL.glColor3f(0.8f, 0.5f, 0.0f);
                GL.glVertex2f(-0.75f, -0.095f);
                GL.glVertex2f(-0.79f, -0.125f);
                GL.glVertex2f(-0.77f, -0.07f);
                GL.glEnd();

                GL.glBegin(GL.GL_TRIANGLES);
                GL.glColor3f(0.8f, 0.5f, 0.0f);
                GL.glVertex2f(-0.75f, -0.007f);
                GL.glVertex2f(-0.795f, 0.035f);
                GL.glVertex2f(-0.77f, -0.02f);
                GL.glEnd();

                GL.glColor3f(0.0f, 0.0f, 0.0f);
                GL.glBegin(GL.GL_POINTS);
                GL.glVertex2f(-0.73f, -0.035f);         
                GL.glEnd();

                GL.glPopMatrix();

                x_axis += 0.2f; y_axis += 0.5f; z_axis += 0.2f;
            }
        }

        private void drawFish2()
        {
            float x_axis = 1.4f, y_axis = 4.2f, z_axis = 20f;

            for (int i = 0; i <= 2; i++)
            {
                GL.glPushMatrix();
                fishAngle = fishRotating();
                GL.glRotatef(fishAngle, 0.0f, 0.0f, 1.0f);
                GL.glTranslatef(x_axis, y_axis, z_axis);
                GL.glColor3d(0.0, 1.0, 0.0);
                GL.glBegin(GL.GL_POLYGON);
                GL.glVertex2d(0.7, 0.05);
                GL.glVertex2d(0.75, 0.1);
                GL.glVertex2d(0.85, 0.05);
                GL.glVertex2d(0.75, 0.0);
                GL.glEnd();

                GL.glBegin(GL.GL_TRIANGLES);
                GL.glColor3d(0.0, 0.0, 1.0);
                GL.glVertex2d(0.83, 0.05);
                GL.glVertex2d(0.9, 0.09);
                GL.glVertex2d(0.9, 0.01);
                GL.glEnd();

                GL.glBegin(GL.GL_TRIANGLES);
                GL.glColor3d(1.0, 1.0, 0.0);
                GL.glVertex2d(0.75, 0.095);
                GL.glColor3d(1.0, 0.0, 0.0);
                GL.glVertex2d(0.79, 0.125);
                GL.glVertex2d(0.77, 0.07);
                GL.glEnd();

                GL.glBegin(GL.GL_TRIANGLES);
                GL.glColor3d(1.0, 1.0, 0.0);
                GL.glVertex2d(0.75, 0.007);
                GL.glColor3d(1.0, 0.0, 0.0);
                GL.glVertex2d(0.795, -0.035);
                GL.glVertex2d(0.77, 0.02);
                GL.glEnd();

                GL.glColor3d(0.0, 0.0, 0.0);
                GL.glBegin(GL.GL_POINTS);
                GL.glVertex2d(0.73, 0.065);
                GL.glEnd();
                GL.glPopMatrix();

                x_axis += 0.3f; y_axis += 0.5f; z_axis += 0.5f;
            }
        }

        private void drawFish3()
        {
            float x_axis = 1.4f, y_axis = 4.5f;

            for (int i = 0; i <= 2; i++)
            {
                GL.glPushMatrix();
                fishAngle = fishRotating();
                GL.glRotatef(fishAngle, 0.0f, 0.0f, 1.0f);
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

        private void drawAquarium(bool i_DrawWithTexturesAndColors)
        {
            GL.glEnable(GL.GL_TEXTURE_2D);
            GL.glBindTexture(GL.GL_TEXTURE_2D, texture[14]);

            // tank cover
            drawScaledCube(2.4f, 6.5f, 21.7f, 0.05f, 0.01f, 0.21f);

            // aqua left leg
            drawScaledCube(2.4f, 1.8f, 19.89f, 0.05f, 0.17f, 0.03f);

            // aqua right leg
            drawScaledCube(2.4f, 1.8f, 23.5f, 0.05f, 0.17f, 0.03f);

            // aqua plate
            drawScaledCube(2.4f, 3.5f, 21.7f, 0.05f, 0.016f, 0.21f);

            GL.glDisable(GL.GL_TEXTURE_2D);

            // tank - bottom
            GL.glEnable(GL.GL_TEXTURE_2D);
            GL.glBindTexture(GL.GL_TEXTURE_2D, texture[15]);
            drawScaledCube(2.4f, 3.7f, 21.7f, 0.05f, 0.003f, 0.21f);
            GL.glDisable(GL.GL_TEXTURE_2D);

            // tank - fish
            drawFish();
            drawFish2();
            drawFish3();

            // tank - left
            GL.glPushMatrix();
            GL.glEnable(GL.GL_BLEND);
            GL.glBlendFunc(GL.GL_SRC_ALPHA, GL.GL_ONE_MINUS_SRC_ALPHA);
            GL.glColor4f(1.0f, 1.0f, 1.0f, 0.6f);
            GL.glEnable(GL.GL_TEXTURE_2D);
            GL.glBindTexture(GL.GL_TEXTURE_2D, texture[16]);
            GL.glTranslatef(1.9f, 5.1f, 21.7f);
            GL.glRotatef(90.0f, 0.0f, 0.0f, 1.0f);
            GL.glScalef(0.15f, 0.001f, 0.21f);
            drawCube();
            GL.glPopMatrix();
            GL.glDisable(GL.GL_TEXTURE_2D);
            GL.glDisable(GL.GL_BLEND);

            //tank - water 
            GL.glEnable(GL.GL_BLEND);
            GL.glBlendFunc(GL.GL_SRC_ALPHA, GL.GL_ONE_MINUS_SRC_ALPHA);
            //GL.glEnable(GL.GL_TEXTURE_2D);
            GL.glBindTexture(GL.GL_TEXTURE_2D, texture[18]);
            GL.glColor4f(0.0f, 0.8f, 0.8f, 0.4f);
            drawScaledCube(2.4f, 4.6f, 21.7f, 0.05f, 0.1f, 0.21f);
            GL.glDisable(GL.GL_BLEND);
            GL.glDisable(GL.GL_TEXTURE_2D);

            // tank - right
            GL.glPushMatrix();
            GL.glEnable(GL.GL_BLEND);
            GL.glBlendFunc(GL.GL_SRC_ALPHA, GL.GL_ONE_MINUS_SRC_ALPHA);
            GL.glColor4f(1.0f, 1.0f, 1.0f, 0.3f);
            GL.glTranslatef(2.9f, 5.1f, 21.7f);
            GL.glRotatef(90.0f, 0.0f, 0.0f, 1.0f);
            GL.glScalef(0.15f, 0.001f, 0.21f); 
            drawCube();
            GL.glPopMatrix();

            // tank - back
            GL.glColor4f(1.0f, 1.0f, 1.0f, 0.3f);
            GL.glPushMatrix();
            GL.glTranslatef(2.4f, 5.1f, 19.6f);
            GL.glRotatef(90.0f, 1.0f, 0.0f, 0.0f);
            GL.glScalef(0.052f, 0.001f, 0.15f);
            drawCube();
            GL.glPopMatrix();

            //tank - front
            GL.glPushMatrix();
            GL.glTranslatef(2.4f, 5.1f, 23.81f);
            GL.glRotatef(90.0f, 1.0f, 0.0f, 0.0f);
            GL.glScalef(0.052f, 0.001f, 0.15f);
            drawCube();
            GL.glPopMatrix();
            GL.glDisable(GL.GL_COLOR);
            GL.glDisable(GL.GL_BLEND);
        }

>>>>>>> Stashed changes
        private void DrawObjects(bool isForShades, int c)
        {
            if (isForShades)
            {
                if (c == 1)
                    GL.glColor3d(0.5, 0.5, 0.5);
                else
                    GL.glColor3d(0.8, 0.8, 0.8);
            }
<<<<<<< Updated upstream
            else
            {
                GL.glColor3d(0.3f, 0.6f, 0.9f);
            }

            drawBed();
            drawCloset();
=======
            drawBed(!isForShades);
            drawCloset(!isForShades);
            drawFloorLamp(!isForShades);
            drawDressingTable(!isForShades);
            drawSphere(!isForShades);
>>>>>>> Stashed changes
            drawWindow();
            drawAquarium(!isForShades);
            drawPoster();

        }
    }

}
