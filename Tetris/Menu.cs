using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris
{
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
        }

        // Bouton quitter
        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        // Bouton play
        private void GameLoad(object sender, EventArgs e)
        {
            this.Hide();
            var game1 = new MainWindow();
            game1.Closed += (s, args) => this.Close();
            game1.Show();
        }

        // Bouton options
        private void LoadOptions(object sender, EventArgs e)
        {
            this.Hide();
            var options1 = new Options();
            options1.Closed += (s, args) => this.Close();
            options1.Show();
        }
    }
}
