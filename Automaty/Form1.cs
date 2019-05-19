using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Automaty
{
    public partial class Form1 : Form
    {
        public static int setSizeX = 0;
        public static int setSizeY = 0;

        public static int setRule = 0;
        public static int setNeighbours = 0;




        public Form1()
        {
            InitializeComponent();


            comboBox1.Items.Add("1D");
            comboBox1.Items.Add("2D");

            



        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
         
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(comboBox1.SelectedItem.ToString() == "1D" && textBox2.Text.Length > 0 && Convert.ToInt32(textBox2.Text) > 0 && textBox3.Text.Length > 0) // && Convert.ToInt32(textBox3.Text) > 0)
            {
                setSizeX = Convert.ToInt32(textBox2.Text);
                setSizeY = Convert.ToInt32(textBox1.Text);
                setRule = Convert.ToInt32(textBox3.Text);
                setNeighbours = 2;

                Form2 frm2 = new Form2();

                frm2.Show();
               
            }

            //TODO : 2D form
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
