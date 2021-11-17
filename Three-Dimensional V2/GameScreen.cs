using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Three_Dimensional_V2
{
    /**
     * THREE - DIMENSIONAL V2 (The hopefully struct 3d engine that doesn't require me to learn calculus)
     * Created by Ted Angus 16/11/2021 - ../../202.
     * 
     * IMPORTANT FOR ME TO READ: X is length, Y is height, Z is depth!!!!!!!!
     *   ^ I mixed up Y and Z multiple times, main reason why I have restarted
     *   ^ Also learned that Cos is used for X and not Y in math class
     *   
     * Version v2.0.1: (Nov 17th)
     * - Finished Top down and side view mode
     * - Ready to start 3d
     * 
     * Version v2.0.0: (Nov 16th)
     * - Basis on Triangles
     * - Removed unnecessary point class, made the list of points in Triangle class double arrays instead
    */



    public partial class GameScreen : UserControl
    {
        /** VARIABLES **/

        // Main variables
        string mode = "2d";
        bool[] keys = new bool[256];

        // Triangles
        List<Triangle3> tris = new List<Triangle3>();

        // Camera
        Camera camera = new Camera(
            new double[] { 0, 0, 0 }, 60, new PointD(0, 0)
        );

        /** START-UP **/
        public GameScreen()
        {
            // Initalization, don't remove this!
            InitializeComponent();

            // Create any triangles here:
            tris.Add(new Triangle3(
                new double[] { -100, 0, 0 },
                new double[] { -100, 100, 0 },
                new double[] { -100, 100, 100 }
            ));
            tris.Add(new Triangle3(
                new double[] { -100, 100, 100 },
                new double[] { -100, 100, 0 },
                new double[] { -100, 100, 100 }
            ));

            tris.Add(new Triangle3(
                new double[] { 0, 0, 0 }, 
                new double[] { 100, 50, 0 }, 
                new double[] { 0, 50, 100 }
            ));
            tris.Add(new Triangle3(
                new double[] { 100, 100, 100 },
                new double[] { 100, 50, 0 },
                new double[] { 0, 50, 100 }
            ));
        }

        /** UPDATE LOOP **/
        private void updateTick_Tick(object sender, EventArgs e)
        {
            switch (mode)
            {
                case "2d":
                    // Key presses to move WEST EAST UP DOWN NORTH SOUTH
                    if (keys[65])
                    {
                        camera.position[0] -= 5;
                    }
                    if (keys[68])
                    {
                        camera.position[0] += 5;
                    }
                    if (keys[16])
                    {
                        camera.position[1] -= 5;
                    }
                    if (keys[32])
                    {
                        camera.position[1] += 5;
                    }
                    if (keys[87])
                    {
                        camera.position[2] -= 5;
                    }
                    if (keys[83])
                    {
                        camera.position[2] += 5;
                    }
                    
                    // Key presses to turn direction left and right
                    if (keys[37])
                    {
                        camera.dir.X += 5;
                    }
                    if (keys[39])
                    {
                        camera.dir.X -= 5;
                    }
                    
                    // Key press to turn direction up and down
                    if (keys[38])
                    {
                        camera.dir.Y -= 5;
                    }
                    if (keys[40])
                    {
                        camera.dir.Y += 5;
                    }

                    // limit camera direction y and keep camera direction x on a 0 - 360
                    camera.dir.Y = camera.dir.Y > 90 ? 90 : (camera.dir.Y < -90 ? -90 : camera.dir.Y);
                    camera.dir.X = camera.dir.X >= 360 ? camera.dir.X - 360 : (camera.dir.X < 0 ? camera.dir.X + 360 : camera.dir.X);
                    break;
                case "3d":

                    break;
            }
            this.Refresh();
        }

        /** DRAWING LOOP **/
        private void GameScreen_Paint(object sender, PaintEventArgs e)
        {
            // Switch the mode
            switch (mode)
            {
                case "2d":
                    Mode2d(e);
                    break;
                case "3d":
                    Mode3d(e);
                    break;
            }
        }

        /** KEY PRESSES **/
        private void GameScreen_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            keys[e.KeyValue] = true;
        }

        /** KEY RELEASES **/
        private void GameScreen_KeyUp(object sender, KeyEventArgs e)
        {
            keys[e.KeyValue] = false;
        }

        /** 2D PAINT FUNCTION **/
        private void Mode2d(PaintEventArgs e)
        {
            // Transformation to 1/4th of the screen width and 1/2th of the screen height
            e.Graphics.TranslateTransform(200, 225);

            // Player Circle
            e.Graphics.FillEllipse(new SolidBrush(Color.Black), -5, -5, 10, 10);

            // Camera variables for simplicity
            double cameraX = camera.position[0];
            double cameraY = camera.position[1];
            double cameraZ = camera.position[2];

            // Triangles TopDown
            foreach (Triangle3 tri in tris)
            {
                // Arrays to draw them top down
                double[][] newPs = new double[3][];
                newPs[0] = new double[3];
                newPs[1] = new double[3];
                newPs[2] = new double[3];

                // i is to count which point we're on
                int i = 0;

                // For each point in the triangle
                foreach(double[] point in tri.ps)
                {
                    double x = point[0]; // Horizontal
                    double y = point[1]; // Vertical
                    double z = point[2]; // Horizontal 2

                    // Calculate distance along X and Z axis
                    double distXZ = Math.Sqrt(Math.Pow((x - cameraX), 2) + Math.Pow((z - cameraZ), 2));

                    // Calculate direction along X and Z axis
                    double dirXZ = Math.Atan2(cameraZ - z, cameraX - x) - (camera.dir.X / (180 / 3.14));

                    // Limit direction to 0 - 360
                    while (dirXZ * (180 / 3.14) >= 360)
                    {
                        dirXZ -= 360 / (180 / 3.14);
                    }
                    while (dirXZ * (180 / 3.14) < 0)
                    {
                        dirXZ += 360 / (180 / 3.14);
                    }

                    // Draw circle of distance
                    e.Graphics.DrawEllipse(new Pen(Color.Gray, 2), Convert.ToSingle(-distXZ), Convert.ToSingle(-distXZ), Convert.ToSingle(distXZ * 2), Convert.ToSingle(distXZ * 2));

                    // Draw fov lines
                    e.Graphics.DrawLine(new Pen(Color.Yellow, 2), 0, 0, Convert.ToSingle(distXZ), 0);
                    e.Graphics.DrawLine(new Pen(Color.Red, 2), 0, 0, Convert.ToSingle(Math.Cos(-camera.fov / (180 / 3.14)) * distXZ), Convert.ToSingle(Math.Sin(-camera.fov / (180 / 3.14)) * distXZ));
                    e.Graphics.DrawLine(new Pen(Color.Blue, 2), 0, 0, Convert.ToSingle(Math.Cos(camera.fov / (180 / 3.14)) * distXZ), Convert.ToSingle(Math.Sin(camera.fov / (180 / 3.14)) * distXZ));

                    // Get positions on screen
                    newPs[i][0] = Math.Cos(dirXZ) * (distXZ);
                    newPs[i][2] = Math.Sin(dirXZ) * (distXZ);

                    // Line pointing from player to point
                    e.Graphics.DrawLine(new Pen(Color.Green, 2), 0, 0, Convert.ToSingle(newPs[i][0]), Convert.ToSingle(newPs[i][2]));

                    // Text containing direction based on player direction
                    e.Graphics.DrawString(Convert.ToSingle(dirXZ).ToString(), DefaultFont, new SolidBrush(Color.Green), Convert.ToSingle(newPs[i][0]), Convert.ToSingle(newPs[i][2]));

                    // Increase i
                    i ++;
                }

                // Draw shape
                e.Graphics.DrawPolygon(new Pen(Color.Blue, 2), new PointF[] { 
                    new PointF(Convert.ToSingle(newPs[0][0]) , Convert.ToSingle(newPs[0][2])),
                    new PointF(Convert.ToSingle(newPs[1][0]) , Convert.ToSingle(newPs[1][2])),
                    new PointF(Convert.ToSingle(newPs[2][0]) , Convert.ToSingle(newPs[2][2]))
                });
            }


            // Transformation of another screen width half
            e.Graphics.TranslateTransform(400, 0);

            // Draw Player Again
            e.Graphics.FillEllipse(new SolidBrush(Color.Black), -5, -5, 10, 10);

            // Triangles From Side
            foreach (Triangle3 tri in tris)
            {
                // Arrays to draw the from side
                double[][] newPs = new double[3][];
                newPs[0] = new double[3];
                newPs[1] = new double[3];
                newPs[2] = new double[3];

                // i is to count which point we're on
                int i = 0;
                foreach (double[] point in tri.ps)
                {
                    double x = point[0]; // Horizontal
                    double y = point[1]; // Vertical
                    double z = point[2]; // Horizontal 2

                    // Calculate distance along X and Z axis
                    double distXZ = Math.Sqrt(Math.Pow((x - cameraX), 2) + Math.Pow((z - cameraZ), 2));

                    // Calculate distance between XZ Axis and Y Axis
                    double distXYZ = Math.Sqrt(Math.Pow(distXZ, 2) + Math.Pow(y - cameraY, 2));

                    // Calculate direction based on Y and XZ distance
                    double dirY = Math.Atan2(cameraY - y, distXZ) - (camera.dir.Y / (180 / 3.14));

                    // Draw circle of distance
                    e.Graphics.DrawEllipse(new Pen(Color.Gray, 2), Convert.ToSingle(-distXYZ), Convert.ToSingle(-distXYZ), Convert.ToSingle(distXYZ * 2), Convert.ToSingle(distXYZ * 2));

                    // Draw fov lines
                    e.Graphics.DrawLine(new Pen(Color.Yellow, 3), 0, 0, Convert.ToSingle(distXYZ), 0);
                    e.Graphics.DrawLine(new Pen(Color.Red, 3), 0, 0, Convert.ToSingle(Math.Cos(-camera.fov / (180 / 3.14)) * distXYZ), Convert.ToSingle(Math.Sin(-camera.fov / (180 / 3.14)) * distXYZ));
                    e.Graphics.DrawLine(new Pen(Color.Blue, 3), 0, 0, Convert.ToSingle(Math.Cos(camera.fov / (180 / 3.14)) * distXYZ), Convert.ToSingle(Math.Sin(camera.fov / (180 / 3.14)) * distXYZ));

                    // Set positions on screen
                    newPs[i][0] = Math.Cos(dirY) * (distXYZ);
                    newPs[i][2] = Math.Sin(dirY) * (distXYZ);

                    // Line pointing from player to point
                    e.Graphics.DrawLine(new Pen(Color.Green, 2), 0, 0, Convert.ToSingle(newPs[i][0]), Convert.ToSingle(newPs[i][2]));

                    // Text containing information about direction based on player direction
                    e.Graphics.DrawString(Convert.ToSingle(dirY).ToString(), DefaultFont, new SolidBrush(Color.Green), Convert.ToSingle(newPs[i][0]), Convert.ToSingle(newPs[i][2]));

                    // Increase i
                    i++;
                }

                // Draw triangle
                e.Graphics.DrawPolygon(new Pen(Color.Blue, 2), new PointF[] {
                    new PointF(Convert.ToSingle(newPs[0][0]) , Convert.ToSingle(newPs[0][2])),
                    new PointF(Convert.ToSingle(newPs[1][0]) , Convert.ToSingle(newPs[1][2])),
                    new PointF(Convert.ToSingle(newPs[2][0]) , Convert.ToSingle(newPs[2][2]))
                });
            }

            // End transform
            e.Graphics.ResetTransform();
        }

        /** 3D PAINT FUNCTION **/
        private void Mode3d(PaintEventArgs e)
        {

        }
    }
}
