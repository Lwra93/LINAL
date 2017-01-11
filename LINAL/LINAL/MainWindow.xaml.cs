using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace LINAL
{
    /// <summary>
    /// floateraction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private Camera camera;
        private Perspective perspective;

        private List<Matrix> weergaveVectoren;
        private Dictionary<string, Matrix> wereldVectoren;

        private Point r1, r2;

        private DispatcherTimer t = new DispatcherTimer();

        /*
         * Initializes the Cube, Player, Camera, Perspective and the dispatchtimer for the cube.
         */
        public MainWindow()
        {
            InitializeComponent();
            weergaveVectoren = new List<Matrix>();
            wereldVectoren = new Dictionary<string, Matrix>();
            camera = new Camera();
            perspective = new Perspective(1, 150);

            CreatePlayer();
            CreateCube();

            t.Tick += ScaleCube;
            t.Interval = new TimeSpan(0, 0, 0, 0, 400);
            t.Start();

        }

        /*
         * Scales the cube every 400 milliseconds. Stops when size reaches 1/3 of screensize
         */
        void ScaleCube(object sender, EventArgs e)
        {
            wereldVectoren["Cube"].Scale(1.1f, 1.1f, 1.1f);
            Draw();
            
            Console.WriteLine(Math.Abs(wereldVectoren["Cube"].GetWidth()) + " - " + perspective.GetScreenSize());

            if (Math.Abs(wereldVectoren["Cube"].GetWidth()) >= perspective.GetScreenSize())
                t.Stop();
            
        }

        /*
         * Create the player object and add it to the world vectors
         */
        public void CreatePlayer()
        {

            Point A = new Point(-80, 0, 195);
            Point B = new Point(0, 0, 195);
            Point C = new Point(-80, 0, 179);
            Point D = new Point(0, 0, 179);

            Point E = new Point(-80, 16, 195);
            Point F = new Point(0, 16, 195);
            Point G = new Point(-80, 16, 179);
            Point H = new Point(0, 16, 179);

            Vector AB = new Vector(A, B);
            Vector AC = new Vector(A, C);
            Vector BD = new Vector(B, D);
            Vector CD = new Vector(C, D);
            Vector AE = new Vector(A, E);
            Vector BF = new Vector(B, F);
            Vector DH = new Vector(D, H);
            Vector CG = new Vector(C, G);
            Vector EF = new Vector(E, F);
            Vector EG = new Vector(E, G);
            Vector FH = new Vector(F, H);
            Vector GH = new Vector(G, H);

            Vector[] x =
            {
                AB,
                AC,
                BD,
                CD,
                AE,
                BF,
                DH,
                CG,
                EF,
                EG,
                FH,
                GH
            };

            Matrix punten = new Matrix(4, x.Length*2);

            int i = 0;
            int place = 0;
            while (i < punten.GetColumns()/2)
            {
                punten.AddVector(place, x[i]);
                i++;
                place += 2;
            }

            wereldVectoren.Add("Player", punten);
            SetPlayerMiddle();

        }

        /*
         * Calculate the middle of the player
         */
        private void SetPlayerMiddle()
        {
            r1 = wereldVectoren["Player"].GetMiddle();
            r1.SetZ(0);
            r2 = wereldVectoren["Player"].GetMiddle();
        }

        /*
         * Create the cube that grows at the horizon
         */
        private void CreateCube()
        {
            
            Point A = new Point(-1, -1, 1);
            Point B = new Point(1, -1, 1);
            Point C = new Point(-1, -1, -1);
            Point D = new Point(1, -1, -1);

            Point E = new Point(-1, 1, 1);
            Point F = new Point(1, 1, 1);
            Point G = new Point(-1, 1, -1);
            Point H = new Point(1, 1, -1);

            Vector AB = new Vector(A, B);
            Vector AC = new Vector(A, C);
            Vector BD = new Vector(B, D);
            Vector CD = new Vector(C, D);
            Vector AE = new Vector(A, E);
            Vector BF = new Vector(B, F);
            Vector DH = new Vector(D, H);
            Vector CG = new Vector(C, G);
            Vector EF = new Vector(E, F);
            Vector EG = new Vector(E, G);
            Vector FH = new Vector(F, H);
            Vector GH = new Vector(G, H);

            Vector[] x =
            {
                AB,
                AC,
                BD,
                CD,
                AE,
                BF,
                DH,
                CG,
                EF,
                EG,
                FH,
                GH
            };

            Matrix punten = new Matrix(4, x.Length * 2);

            int i = 0;
            int place = 0;
            while (i < punten.GetColumns() / 2)
            {
                punten.AddVector(place, x[i]);
                i++;
                place += 2;
            }

            wereldVectoren.Add("Cube", punten);

        }

        /*
         * Creates the view vectors. Camera, Perspective & Recalculation
         */
        private void CreateViewVectors()
        {
            
            weergaveVectoren.Clear();

            foreach (string wereld in wereldVectoren.Keys)
            {
                Matrix m = wereldVectoren[wereld];
                Matrix wereldVector = perspective.Get();
                wereldVector.Multiply(camera.Get());
                wereldVector.Multiply(m);

                for (int column = 0; column < wereldVector.GetColumns(); column++)
                {
                    float x = wereldVector.Get(0, column);
                    float y = wereldVector.Get(1, column);
                    float z = wereldVector.Get(2, column);
                    float w = wereldVector.Get(3, column);

                    x = (float)((perspective.GetScreenSize() / 2) + ((x + 1) / w) * perspective.GetScreenSize() * 0.5);
                    y = (float)((perspective.GetScreenSize() / 2) + ((y + 1) / w) * perspective.GetScreenSize() * 0.5);
                    z = -z;

                    wereldVector.Set(0, column, x);
                    wereldVector.Set(1, column, y);
                    wereldVector.Set(2, column, z);

                }

                weergaveVectoren.Add(wereldVector);
            }


        }

        /*
         * Draws all view vectors
         */
        private void Draw()
        {

            CreateViewVectors();
            MyCanvas.Children.Clear();

            foreach (Matrix weergave in weergaveVectoren)
            {
                for (int column = 0; column < weergave.GetColumns(); column+=2)
                {

                    if (weergave.Get(3, column) <= 0 || weergave.Get(3, column + 1) <= 0)
                        continue;

                    var line = new Line();
                    line.X1 = weergave.Get(0, column);
                    line.X2 = weergave.Get(0, column + 1);
                    line.Y1 = weergave.Get(1, column);
                    line.Y2 = weergave.Get(1, column + 1);
                    line.StrokeThickness = 2;
                    line.Stroke = Brushes.Blue;
                    MyCanvas.Children.Add(line);

                }
            }

        }

        /*
         * Handles all key presses
         */
        private void MainWindow_OnKeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Q)
            {
                wereldVectoren["Player"].Rotate(-10, true, r2, r1);
            }

            else if (e.Key == Key.E)
            {
                wereldVectoren["Player"].Rotate(10, true, r2, r1);
            }

            if (e.Key == Key.Left)
            {
                wereldVectoren["Player"].Translate(-10, 0, 0, true);
                SetPlayerMiddle();
            }

            else if (e.Key == Key.Right)
            {
                wereldVectoren["Player"].Translate(10, 0, 0, true);
                SetPlayerMiddle();
            }
            else if (e.Key == Key.Up)
            {
                wereldVectoren["Player"].Translate(0, 0, -10, true);
                SetPlayerMiddle();
            }
            else if (e.Key == Key.Down)
            {
                wereldVectoren["Player"].Translate(0, 0, 10, true);
                SetPlayerMiddle();
            }
                
            Draw();

        }
    }
}
