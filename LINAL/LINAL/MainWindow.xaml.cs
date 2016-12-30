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
            Calc();
        }

        public void Calc()
        {
            
            Matrix m1 = new Matrix(2,3);
            m1.Add(0,0,4);
            m1.Add(0,1,3);
            m1.Add(0,2,5);
            m1.Add(1,0,1);
            m1.Add(1,1,3);
            m1.Add(1,2,3);

            m1.Print();
            Point p = new Point(3,5);
            m1.Rotate2D(10, p);

            m1.Print();






        }


    }
}
