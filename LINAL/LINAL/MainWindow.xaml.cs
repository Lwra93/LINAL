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

        public MainWindow()
        {
            InitializeComponent();
            camera = new Camera();
            perspective = new Perspective(1, 50);

        }

        public Matrix Calc()
        {

            Point A = new Point(20, 20, -20);
            Point B = new Point(70, 20, -20);
            Point C = new Point(70, 20, -70);
            Point D = new Point(20, 20, -70);

            Point E = new Point(20, 70, -20);
            Point F = new Point(20, 70, -70);
            Point G = new Point(70, 70, -70);
            Point H = new Point(70, 70, -20);

            //Bodem
            Vector AB = new Vector(A, B);
            Vector AD = new Vector(A, D);
            Vector BC = new Vector(B, C);
            Vector CD = new Vector(C, D);

            //Verticaal
            Vector AE = new Vector(A, E);
            Vector BH = new Vector(B, H);
            Vector CG = new Vector(C, G);
            Vector DF = new Vector(D, F);

            //Top
            Vector EH = new Vector(E, H);
            Vector EF = new Vector(E, F);
            Vector GH = new Vector(G, H);
            Vector FG = new Vector(F, G);

            Vector[] x = {
                AB,
                AD,
                BC,
                CD,
                AE,
                BH,
                CG,
                DF,
                EH,
                EF,
                GH,
                FG
            };

            Matrix m = new Matrix(4, 24);

            int i = 0;
            int place = 0;
            while (i < m.GetColumns()/2)
            {
                m.AddVector(place, x[i]);
                i++;
                place += 2;
            }

            Matrix t = perspective.Get();
            t.Multiply(camera.Get());
            t.Multiply(m.ConvertToVectors());
            t.Recalculate(Width);

            t.Print();

            return t;



        }

        public Matrix Axises()
        {

            Point X = new Point(300, 0, 0);
            Vector XV = X.MakeVector();
            XV.SetHelp(1);
            Point Y = new Point(0, -300, 0);
            Vector YV = Y.MakeVector();
            YV.SetHelp(1);
            Point Z = new Point(0,0,300);
            Vector ZV = Z.MakeVector();
            ZV.SetHelp(1);

            Matrix m = new Matrix(4, 6);
            m.AddVector(0, XV);
            m.AddVector(2, YV);
            m.AddVector(4, ZV);

            Matrix t = perspective.Get();
            t.Multiply(camera.Get());
            t.Multiply(m);
            t.Recalculate(Width);

            

            return t;
        }

        private void MainWindow_OnKeyDown(object sender, object e)
        {
            MyCanvas.Children.Clear();
            Matrix t = Calc();
            Matrix axis = Axises();
            camera.turn(5);

            SolidColorBrush[] b = new SolidColorBrush[6];
            b[0] = Brushes.Green;
            b[2] = Brushes.Red;
            b[4] = Brushes.Purple;

            for (int column = 0; column < axis.GetColumns(); column++)
            {

                if (axis.Get(3, column) < 0 || axis.Get(3, column + 1) < 0)
                {
                    column++;
                    continue;
                }

                var line = new Line();
                line.X1 = axis.Get(0, column + 1);
                line.X2 = axis.Get(0, column);
                line.Y1 = axis.Get(1, column + 1);
                line.Y2 = axis.Get(1, column);
                line.StrokeThickness = 2;
                line.Stroke = b[column];
                MyCanvas.Children.Add(line);
                column++;

            }

            for (int column = 0; column < t.GetColumns(); column++)
            {

                if (t.Get(3, column) < 0 || t.Get(3, column + 1) < 0)
                {
                    column++;
                    continue;
                }

                var line = new Line();
                line.X1 = t.Get(0, column + 1);
                line.X2 = t.Get(0, column);
                line.Y1 = t.Get(1, column + 1);
                line.Y2 = t.Get(1, column);
                line.StrokeThickness = 2;
                line.Stroke = Brushes.Blue;
                MyCanvas.Children.Add(line);
                column++;

            }



            
        }

    }
}
