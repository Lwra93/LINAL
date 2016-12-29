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
            
            Matrix m1 = new Matrix(2, 2);
            m1.Add(0,0,1.2f);
            m1.Add(0,1,0f);
            m1.Add(1,0,0f);
            m1.Add(1,1,1.1f);

            Matrix m2 = new Matrix(2,4);
            m2.Add(0,0,3f);
            m2.Add(0,1,4f);
            m2.Add(0,2,6f);
            m2.Add(0,3,1f);
            m2.Add(1,0,2f);
            m2.Add(1,1,1f);
            m2.Add(1,2,7f);
            m2.Add(1,3,5f);

            MatrixMath m = new MatrixMath();

            Console.WriteLine("Matrix1:");
            m1.Print();

            Console.WriteLine("Matrix2:");
            m2.Print();

            Console.WriteLine("Multiplied:");
            m.Multiply(m1, m2).Print();

            Console.Read();


        }


    }
}
