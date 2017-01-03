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

            Point P = new Point(3, -1, 2);
            Point Q = new Point(1, 3, 4);
            Point S = new Point(2, 3, 2);

            Plane plane = new Plane();
            plane.Add(P);
            plane.Add(Q);
            plane.Add(S);

            plane.BuildFormula();

           

        }


    }
}
