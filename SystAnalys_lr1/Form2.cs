using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace SystAnalys_lr1
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            StreamReader print = new StreamReader("gist.txt");
            string line = print.ReadLine();
            int size = Convert.ToInt32(line);
            double[] y_values = new double[size];
            int[] x_values = new int[size];
            int i = 0;
            while (print.Peek() > -1)
            {
                line = print.ReadLine();
                x_values[i] = Convert.ToInt32(line);
                line = print.ReadLine();
                y_values[i] = Convert.ToDouble(line);
                i++;
            }


                chart1.Series[0].Points.DataBindXY(x_values, y_values);

        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }
    }
}
