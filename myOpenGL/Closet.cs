using OpenGL;
using System.Collections.Generic;

namespace myOpenGL
{
    public class Closet
    {
        private List<SelectableObject> m_Selectables;
        private List<Door> m_Doors;
        private List<Drawer> m_Drawers;
        private int? m_SelectedIdx = null;
        private uint? m_ClothesTexture;

        public Closet(uint? i_DrawerTexture, uint? i_LeftDoorTexture, uint? i_RightDoorTexture, uint? i_ClothesTexture)
        {
            int numOfDoors = 4;
            int numOfDrawers = 4;

            m_ClothesTexture = i_ClothesTexture;

            m_Selectables = new List<SelectableObject>(numOfDoors + numOfDrawers);
            m_Doors = new List<Door>(numOfDoors) { new Door(eDoorSides.Left, i_LeftDoorTexture),
                new Door(eDoorSides.Right, i_RightDoorTexture),
                new Door(eDoorSides.Left, i_LeftDoorTexture),
                new Door(eDoorSides.Right, i_RightDoorTexture) };
            m_Drawers = new List<Drawer>(numOfDrawers);

            foreach (Door door in m_Doors)
            {
                m_Selectables.Add(door);
            }

            for (int i = 0; i < numOfDrawers; i++)
            {
                m_Drawers.Add(new Drawer(i_DrawerTexture));
                m_Selectables.Add(m_Drawers[i]);
            }
        }

        public void SwitchSelectedObject()
        {
            if (m_SelectedIdx == null)
            {
                m_SelectedIdx = 0;
            }
            else
            {
                m_Selectables[m_SelectedIdx.Value].Select = false;
                m_SelectedIdx = (m_SelectedIdx + 1) % m_Selectables.Count;
            }

            m_Selectables[m_SelectedIdx.Value].Select = true;
        }

        public void UnselectObjects()
        {
            m_SelectedIdx = null;

            foreach (SelectableObject selectable in m_Selectables)
            {
                selectable.Select = false;
            }
        }

        public void Draw(bool i_IsShadow)
        {
            if (!i_IsShadow)
            {
                drawClothes();
            }

            float shelfHeightDelta = 0.0f;

            if (!i_IsShadow)
            {
                GL.glColor3f(0.8f, 0.8f, 0.8f);
            }

            drawShelves(ref shelfHeightDelta);
            drawDrawers(i_IsShadow);
            drawDoors(i_IsShadow);
            drawClosetSides(i_IsShadow);
        }

        public void OpenSelectedObject()
        {
            if (m_SelectedIdx != null && m_Selectables[m_SelectedIdx.Value] is IOpenCloseable openable)
            {
                openable.Open();
            }
        }

        public void CloseSelectedObject()
        {
            if (m_SelectedIdx != null && m_Selectables[m_SelectedIdx.Value] is IOpenCloseable openable)
            {
                openable.Close();
            }
        }

        public void OpenAllObjects()
        {
            foreach (SelectableObject selectable in m_Selectables)
            {
                if (selectable is IOpenCloseable openable)
                {
                    openable.Open();
                }
            }
        }

        public void CloseAllObjects()
        {
            foreach (SelectableObject selectable in m_Selectables)
            {
                if (selectable is IOpenCloseable openable)
                {
                    openable.Close();
                }
            }
        }

        private void drawDoors(bool i_IsShadow)
        {
            GL.glPushMatrix();
            GL.glTranslatef(-0.5f, 0f, 2.02f);

            // left door
            GL.glTranslatef(-1.02f, 6.51f, 3.0f);

            foreach (Door door in m_Doors)
            {
                door.Draw(i_IsShadow);
                GL.glTranslatef(2f, 0f, 0.0f);
            }

            GL.glPopMatrix();
        }

        private void drawClosetSides(bool i_IsShadow)
        {
            if (!i_IsShadow)
            {
                GL.glColor3f(0.8f, 0.8f, 0.8f);
            }

            // left side
            Cube.DrawScaledCube(-2.5f, 5.0f, 3f, 0.001f, 0.53f, 0.2f);

            // right side
            Cube.DrawScaledCube(5.5f, 5.0f, 3f, 0.001f, 0.53f, 0.2f);

            //back side
            GL.glPushMatrix();
            GL.glRotatef(90.0f, 0.0f, 1.0f, 0.0f);
            GL.glTranslatef(-1.0f, 5.0f, 1.5f);
            GL.glScalef(0.001f, 0.53f, 0.4f);
            Cube.Draw();
            GL.glPopMatrix();
        }

        private void drawDrawers(bool i_IsShadow)
        {
            float drawerXDelta = -2.43f;

            foreach (Drawer drawer in m_Drawers)
            {
                GL.glPushMatrix();
                GL.glTranslatef(drawerXDelta, 1.0f, 3.0f);

                drawer.Draw(i_IsShadow);

                GL.glPopMatrix();

                drawerXDelta += 1.99f;
            }
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

        private void drawClothes()
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
                GL.glBindTexture(GL.GL_TEXTURE_2D, m_ClothesTexture.Value);

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
