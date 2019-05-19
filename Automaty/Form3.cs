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
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
            comboBox1.Items.Add("Automats (1D/2D)");
            comboBox1.Items.Add("Game o Life (standard rules)");
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem.ToString() == "Automats (1D/2D)" )
            {

                Form1 frm1 = new Form1();

                frm1.Show();

            }
            else if (comboBox1.SelectedItem.ToString() == "Game o Life (standard rules)")
            {

                Form4 frm4 = new Form4();

                frm4.Show();

            }
        }
    }
}
