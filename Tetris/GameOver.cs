using System;
using System.Windows.Forms;

namespace Tetris
{
    public partial class GameOver : Form
    {
        public GameOver(int finalScore)
        {
            InitializeComponent();

            ScoreFinal(finalScore);
        }
        private void ScoreFinal(int finalScore)
        {
            label1.Text = "Score: " + finalScore.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            var newGame = new MainWindow();
            newGame.Closed += (s, args) => this.Close();
            newGame.Show();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            var mainMenu = new Menu();
            mainMenu.Closed += (s, args) => this.Close();
            mainMenu.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void GameOver_Load(object sender, EventArgs e)
        {

        }
    }
}
