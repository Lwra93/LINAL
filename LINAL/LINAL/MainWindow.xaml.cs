using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public MainWindow()
        {
            InitializeComponent();
            Draw();
        }

        public void Draw()
        {
            
            Vector v1 = new Vector(200, 100, 100, 100);
            Vector v2 = VectorFactory.Enlarge(v1, 0.1f);

            //DrawVector(v1);
           DrawVector(v2);

        }

        public void DrawVector(Vector v)
        {

            Line myLine = new Line();
            myLine.Stroke = System.Windows.Media.Brushes.Red;
            myLine.X1 = v.GetX1();
            myLine.X2 = v.GetX2();
            myLine.Y1 = v.GetY1();
            myLine.Y2 = v.GetY2();
            myLine.HorizontalAlignment = HorizontalAlignment.Left;
            myLine.VerticalAlignment = VerticalAlignment.Center;
            myLine.StrokeThickness = 2;
            MyCanvas.Children.Add(myLine);
            

        }

    }
}
