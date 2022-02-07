using System;
using System.Windows.Forms;

namespace Tetris
{
    public partial class Options : Form
    {
        public Options()
        {
            InitializeComponent();
            MainWindow.game_type = "Easy";
        }
        //Easy button
        private void button1_Click(object sender, EventArgs e)
        {
            MainWindow.game_type = "Easy";
            Console.WriteLine(MainWindow.game_type);
        }

        //Medium button
        private void button2_Click(object sender, EventArgs e)
        {
            MainWindow.game_type = "Medium";
            Console.WriteLine(MainWindow.game_type);
        }

        //Hard button
        private void button3_Click(object sender, EventArgs e)
        {
            MainWindow.game_type = "Hard";
            Console.WriteLine(MainWindow.game_type);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            var menuPrincipal = new Menu();
            menuPrincipal.Closed += (s, args) => this.Close();
            menuPrincipal.Show();
        }
    }
}
