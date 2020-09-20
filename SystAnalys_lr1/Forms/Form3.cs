using System;
using System.Windows.Forms;
using System.IO;
using MetroFramework.Forms;

namespace SystAnalys_lr1
{
    public partial class Form3 : MetroForm
    {
        public Form3()
        {
            InitializeComponent();           
        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            File.Delete("gen.txt");
            StreamWriter print = new StreamWriter("gen.txt", true);
            if ((textBox1 != null) && (textBox2 != null) && (textBox3 != null) && (textBox4 != null))
            {
                print.Write(textBox1.Text);
                print.WriteLine();
                print.Write(textBox2.Text);
                print.WriteLine();
                print.Write(textBox3.Text);
                print.WriteLine();
                print.Write(textBox4.Text);
            }
            print.Close();
            this.Close();


        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if (!Char.IsDigit(number))
            {
                e.Handled = true;
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if (!Char.IsDigit(number))
            {
                e.Handled = true;
            }
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if (!Char.IsDigit(number))
            {
                e.Handled = true;
            }
        }
    }
}
