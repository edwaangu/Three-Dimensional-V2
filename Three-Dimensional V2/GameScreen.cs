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

            // Create any triangles
            tris.Add(new Triangle3(
                new double[] { 0, 0, 0 }, 
                new double[] { 100, 10, 150 }, 
                new double[] { -100, 100, -100 }
            ));
        }

        /** UPDATE LOOP **/
        private void updateTick_Tick(object sender, EventArgs e)
        {
            switch (mode)
            {
                case "2d":
                    if (keys[65])
                    {
                        camera.position[0] -= 5;
                    }
                    if (keys[68])
                    {
                        camera.position[0] += 5;
                    }
                    if (keys[87])
                    {
                        camera.position[2] -= 5;
                    }
                    if (keys[83])
                    {
                        camera.position[2] += 5;
                    }
                    if (keys[37])
                    {
                        camera.dir.X -= 5;
                    }
                    if (keys[39])
                    {
                        camera.dir.X += 5;
                    }
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
            // Player
            e.Graphics.TranslateTransform(200, 225);
            double cameraX = camera.position[0];
            double cameraY = camera.position[1];
            double cameraZ = camera.position[2];
            e.Graphics.FillEllipse(new SolidBrush(Color.Black), -5, -5, 10, 10);


            // Triangles
            foreach (Triangle3 tri in tris)
            {
                double[][] newPs = new double[3][3];
                int i = 0;
                foreach(double[] point in tri.ps)
                {
                    double x = point[0]; // Horizontal
                    double y = point[1]; // Vertical
                    double z = point[2]; // Horizontal 2

                    double distXZ = Math.Sqrt(Math.Pow((x - cameraX), 2) + Math.Pow((z - cameraZ), 2)) * 2;
                    double dirXZ = Math.Atan2(z - cameraZ, x - cameraX);
                    e.Graphics.DrawEllipse(new Pen(Color.Gray, 2), Convert.ToSingle(-distXZ / 2), Convert.ToSingle(-distXZ / 2), Convert.ToSingle(distXZ), Convert.ToSingle(distXZ));

                    newPs[i][0] = Math.Cos(dirXZ / (180 / 3.14)) * distXZ;
                    newPs[i][2] = Math.Sin(dirXZ / (180 / 3.14)) * distXZ;

                    i ++;
                }
                e.Graphics.DrawPolygon(new Pen(Color.Blue, 2), new PointF[] { 
                    new PointF(Convert.ToSingle(newPs[0][0]) , Convert.ToSingle(newPs[0][2])),
                    new PointF(Convert.ToSingle(newPs[1][0]) , Convert.ToSingle(newPs[1][2])),
                    new PointF(Convert.ToSingle(newPs[2][0]) , Convert.ToSingle(newPs[2][2]))
                });
            }

            e.Graphics.ResetTransform();
        }

        /** 3D PAINT FUNCTION **/
        private void Mode3d(PaintEventArgs e)
        {

        }
    }
}
