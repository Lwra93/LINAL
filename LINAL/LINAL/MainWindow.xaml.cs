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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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

        public MainWindow()
        {
            InitializeComponent();
            weergaveVectoren = new List<Matrix>();
            wereldVectoren = new Dictionary<string, Matrix>();
            camera = new Camera();
            perspective = new Perspective(1, 100);

            CreateCube();

        }

        public void CreateCube()
        {

            Point A = new Point(-4, 0, 0);
            Point B = new Point(6, 0, 0);
            Point C = new Point(-4, 0, -2);
            Point D = new Point(6, 0, -2);

            Point E = new Point(-4, 2, 0);
            Point F = new Point(6, 2, 0);
            Point G = new Point(-4, 2, -2);
            Point H = new Point(6, 2, -2);

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

            r1 = punten.GetMiddle();
            r1.SetZ(0);
            r2 = punten.GetMiddle();

            wereldVectoren.Add("Player", punten);

        }

        private void CreateViewVectors()
        {
            
            weergaveVectoren.Clear();

            foreach (string wereld in wereldVectoren.Keys)
            {
                Matrix m = wereldVectoren[wereld];
                Matrix wereldVector = perspective.Get();
                wereldVector.Multiply(camera.Get());
                wereldVector.Multiply(m);
                weergaveVectoren.Add(wereldVector);
            }


        }

        private void Draw()
        {

            MyCanvas.Children.Clear();

            foreach (Matrix weergave in weergaveVectoren)
            {
                for (int column = 0; column < weergave.GetColumns(); column++)
                {

                    float x1 = (float)((perspective.GetScreenSize() / 2) + ((weergave.Get(0, column) + 1) / weergave.Get(3, column)) * perspective.GetScreenSize() * 0.5);
                    float x2 = (float)((perspective.GetScreenSize() / 2) + ((weergave.Get(0, column + 1) + 1) / weergave.Get(3, column + 1)) * perspective.GetScreenSize() * 0.5);
                    float y1 = (float)((perspective.GetScreenSize() / 2) + ((weergave.Get(1, column) + 1) / weergave.Get(3, column)) * perspective.GetScreenSize() * 0.5);
                    float y2 = (float)((perspective.GetScreenSize() / 2) + ((weergave.Get(1, column + 1) + 1) / weergave.Get(3, column + 1)) * perspective.GetScreenSize() * 0.5);

                    if (weergave.Get(3, column) <= 0 || weergave.Get(3, column + 1) <= 0)
                    {
                        column++;
                        continue;
                    }

                    var x = x2 - x1;
                    var y = y2 - y1;
                    

                    var line = new Line();
                    line.X1 = x1;
                    line.X2 = x2;
                    line.Y1 = y1;
                    line.Y2 = y2;
                    line.StrokeThickness = 2;
                    line.Stroke = Brushes.Blue;
                    MyCanvas.Children.Add(line);
                    column++;

                }
            }

        }

        private void MainWindow_OnKeyDown(object sender, KeyEventArgs e)
        {


            if(e.Key == Key.Q)
                wereldVectoren["Player"].Rotate3D(10, r2, r1);
            else if(e.Key == Key.E)
                wereldVectoren["Player"].Rotate3D(-10, r2, r1);


            CreateViewVectors();
            Draw();

            
        }



    }
}
