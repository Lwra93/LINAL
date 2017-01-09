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

            Point A = new Point(20, 20, -20);
            Point B = new Point(70, 20, -20);
            Point C = new Point(70, 20, -70);
            Point D = new Point(20, 20, -70);

            Point E = new Point(45, 70, -45);

            Vector AB = new Vector(A, B);
            Vector AD = new Vector(A, D);
            Vector BC = new Vector(B, C);
            Vector CD = new Vector(C, D);

            Vector AE = new Vector(A, E);
            Vector BE = new Vector(B, E);
            Vector CE = new Vector(C, E);
            Vector DE = new Vector(D, E);


            Vector[] x =
            {
                AB,
                AD,
                BC,
                CD,
                AE,
                BE,
                CE,
                DE
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

            wereldVectoren.Add("Triangle", punten);

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



                wereldVectoren["Triangle"].Rotate3D(10, wereldVectoren["Triangle"].GetMiddle());
                CreateViewVectors();
                Draw();
                

            

            return;

            MyCanvas.Children.Clear();

            if (e.Key == Key.Down)
            {

                Vector directional = (new Point(-2, 0, 0)).MakeVector();

                for (int column = 0; column < weergaveVectoren[0].GetColumns(); column++)
                {
                    Vector v =
                    (new Point(weergaveVectoren[0].Get(0, column), weergaveVectoren[0].Get(1, column),
                        weergaveVectoren[0].Get(2, column))).MakeVector();
                    v.Move(2, directional);

                    weergaveVectoren[0].Add(0, column, v.GetPoint(1).GetX());
                    weergaveVectoren[0].Add(1, column, v.GetPoint(1).GetY());
                    weergaveVectoren[0].Add(2, column, v.GetPoint(1).GetZ());



                }

                
            }
        }



    }
}
