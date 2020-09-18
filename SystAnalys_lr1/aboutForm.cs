using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Xml.Serialization;


namespace SystAnalys_lr1
{
    public partial class aboutForm : Form
    {
        List<Cars> C;
        int[,] CarsPoint;
        double percent;
        int VCount;
        public aboutForm()
        {
            InitializeComponent();

            C = new List<Cars>();
            CarsPoint = new int[60, 10];
           

        

        }

       

    }
}
