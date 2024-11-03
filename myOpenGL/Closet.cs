using OpenGL;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace myOpenGL
{
    public class Closet
    {
        private int? m_SelectedDoorIdx = null;
        private List<Door> m_Doors;

        public Closet()
        {
            m_Doors = new List<Door>() { new Door(eDoorSides.Left), new Door(eDoorSides.Right), new Door(eDoorSides.Left), new Door(eDoorSides.Right) };
        }

        public void SwitchSelectedDoor()
        {
            if (m_SelectedDoorIdx == null)
            {
                m_SelectedDoorIdx = 0;
            }
            else
            {
                m_Doors[m_SelectedDoorIdx.Value].Select = false;
                m_SelectedDoorIdx = (m_SelectedDoorIdx + 1) % m_Doors.Count;
            }

            m_Doors[m_SelectedDoorIdx.Value].Select = true;
        }

        public void UnselectDoor()
        {
            m_SelectedDoorIdx = null;

            foreach (Door door in m_Doors)
            {
                door.Select = false;
            }
        }

        public void Draw(uint? i_ClothesTexture, uint? i_DoorTexture, uint? i_DrawerTexture)
        {
            if (i_ClothesTexture.HasValue)
            {
                drawClothes(i_ClothesTexture.Value);
            }

            float shelfHeightDelta = 0.0f;

            if (i_ClothesTexture.HasValue)
            {
                GL.glColor3f(0.8f, 0.8f, 0.8f);
            }

            drawShelves(ref shelfHeightDelta);
            drawDrawers(i_DrawerTexture);
            drawDoors(i_DoorTexture);
            drawClosetSides(i_ClothesTexture.HasValue);
        }

        public void OpenSelectedDoor()
        {
            if (m_SelectedDoorIdx != null)
            {
                m_Doors[m_SelectedDoorIdx.Value].MoveToAngle = 90f;
            }
        }

        public void CloseSelectedDoor()
        {
            if (m_SelectedDoorIdx != null)
            {
                m_Doors[m_SelectedDoorIdx.Value].MoveToAngle = 0f;
            }
        }

        public void OpenAllDoors()
        {
            foreach (Door door in m_Doors)
            {
                door.MoveToAngle = 90f;
            }
        }

        public void CloseAllDoors()
        {
            foreach (Door door in m_Doors)
            {
                door.MoveToAngle = 0f;
            }
        }

        private void drawDoors(uint? texture)
        {
            GL.glPushMatrix();
            GL.glTranslatef(-0.5f, 0f, 2.02f);

            // left door
            GL.glTranslatef(-1.02f, 6.51f, 3.0f);

            foreach (Door door in m_Doors)
            {
                door.Draw(texture);
                GL.glTranslatef(2f, 0f, 0.0f);
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
            Cube.Draw();
            GL.glPopMatrix();
        }

        private void drawDrawers(uint? texture)
        {
            float drawerXDelta = -2.43f;

            for (int i = 0; i < 4; i++)
            {
                // drawer
                GL.glPushMatrix();
                GL.glTranslatef(drawerXDelta, 1.0f, 3.0f);

                drawDrawerSides();
                drawDrawerBottom();
                drawDrawerFront(texture);

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

        private void drawDrawerFront(uint? texture)
        {
            if (texture.HasValue)
            {
                GL.glEnable(GL.GL_TEXTURE_2D);
                GL.glBindTexture(GL.GL_TEXTURE_2D, texture.Value);
            }

            drawScaledCube(0.95f, 0.2f, 2.02f, 0.1f, 0.15f, 0.001f);

            if (texture.HasValue)
            {
                GL.glDisable(GL.GL_TEXTURE_2D);
            }
        }

        private void drawScaledCube(float i_X, float i_Y, float i_Z, float i_ScaleX, float i_ScaleY, float i_ScaleZ)
        {
            GL.glPushMatrix();
            GL.glTranslatef(i_X, i_Y, i_Z);
            GL.glScalef(i_ScaleX, i_ScaleY, i_ScaleZ);
            Cube.Draw();
            GL.glPopMatrix();
        }

        private void drawShelves(ref float i_ShelfHeightDelta)
        {
            for (int i = 0; i < 5; i++)
            {
                GL.glPushMatrix();
                GL.glTranslatef(1.5f, i_ShelfHeightDelta, 3.0f);
                GL.glScalef(0.4f, 0.03f, 0.2f);
                Cube.Draw();
                GL.glPopMatrix();

                i_ShelfHeightDelta += 2.5f;
            }
        }

        private void drawClothes(uint texture)
        {
            float height = 3.8f;
            float horizontalPosition = -1.5f;
            uint textureNum = 12;

            const float initialZ = 4.0f;
            const float rotationAngle = 90.0f;
            const float scaleX = 0.08f;
            const float scaleY = 0.008f;
            const float scaleZ = 0.085f;
            const float gap = 2f;

            // Set the color for all clothes
            GL.glColor3f(1.0f, 1.0f, 1.0f);

            for (int i = 0; i < 3; i++)
            {
                // Enable texture for each piece of clothing
                GL.glEnable(GL.GL_TEXTURE_2D);
                GL.glBindTexture(GL.GL_TEXTURE_2D, texture);

                // Draw the clothing piece
                GL.glPushMatrix();
                GL.glTranslatef(horizontalPosition, height, initialZ);
                GL.glRotatef(rotationAngle, 1.0f, 0.0f, 0.0f);
                GL.glRotatef(rotationAngle, 0.0f, -1.0f, 0.0f);
                GL.glScalef(scaleX, scaleY, scaleZ);
                Cube.Draw();
                GL.glPopMatrix();

                // Disable texture after drawing
                GL.glDisable(GL.GL_TEXTURE_2D);

                // Update height for the next piece of clothing
                horizontalPosition += gap;

                // Change the texture for the second piece
                if (i == 1)
                {
                    textureNum++;
                }
            }
        }
    }
}
