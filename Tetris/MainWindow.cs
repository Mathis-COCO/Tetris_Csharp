using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Media;

namespace Tetris
{
    public partial class MainWindow : Form
    {
        Control[] activePiece = { null, null, null, null };
        Control[] activePiece2 = { null, null, null, null };
        Control[] nextPiece = { null, null, null, null };
        List<int> PieceSequence = new List<int>();
        int timeElapsed = 0;
        int currentPiece;
        int nextPieceInt;
        int rotations = 0;
        Color pieceColor = Color.White;
        int score = 0;
        int clears = 0;
        int level = 0;
        bool gameOver = false;
        int PieceSequenceIteration = 0;
        public static string game_type;
        
        readonly Color[] colorList =
        {
            Color.Cyan,
            Color.Orange,
            Color.Blue,
            Color.Green,
            Color.Red,
            Color.Yellow,
            Color.Purple
        };

        // Chargement de la fenêtre du jeu
        public MainWindow()
        {
            InitializeComponent();

            // On initialise le timer de vitesse
            SpeedTimer.Start();
            SpeedTimer.Interval = 800;
            if (game_type == "Easy")
            {
                SpeedTimer.Interval = 800;
            }
            else if (game_type == "Medium")
            {
                SpeedTimer.Interval = 700;
            }
            else if (game_type == "Hard")
            {
                SpeedTimer.Interval = 600;
            }
            GameTimer.Start();

            activePiece2[0] = box1;
            activePiece2[1] = box2;
            activePiece2[2] = box3;
            activePiece2[3] = box4;

            // On génère une pièce
            System.Random random = new System.Random();
            while (PieceSequence.Count < 7)
            {
                int x = random.Next(7);
                if (!PieceSequence.Contains(x))
                {
                    PieceSequence.Add(x);
                }
            }
            SoundPlayer splayer = new SoundPlayer(@"C:\Users\yrlan\OneDrive - Ynov\GitHub\Tetris_CSharp\Tetris\Music\Game-Music.wav");
            splayer.Play();


            // Sélectionner la première pièce
            nextPieceInt = PieceSequence[0];
            PieceSequenceIteration++;
            
            DropNewPiece();
            Console.WriteLine(SpeedTimer);
        }

        public void DropNewPiece()
        {
            rotations = 0;
            currentPiece = nextPieceInt;

            if (PieceSequenceIteration == 7)
            {
                PieceSequenceIteration = 0;

                // Scramble PieceSequence
                PieceSequence.Clear();
                System.Random random = new System.Random();
                while (PieceSequence.Count < 7)
                {
                    int x = random.Next(7);
                    if (!PieceSequence.Contains(x))
                    {
                        PieceSequence.Add(x);
                    }
                }
                Console.WriteLine(SpeedTimer);
            }

            nextPieceInt = PieceSequence[PieceSequenceIteration];
            PieceSequenceIteration++;

            if (nextPiece.Contains(null) == false)
            {
                foreach (Control x in nextPiece)
                {
                    x.BackColor = Color.White;
                }
            }
            // Définition du template de la pièce suivante
            Control[,] nextPieceArray =
            {
                { box203, box207, box211, box215 },
                { box202, box206, box210, box211 },
                { box203, box207, box211, box210 },
                { box206, box207, box203, box204 },
                { box202, box203, box207, box208 },
                { box206, box207, box210, box211 },
                { box207, box210, box211, box212 }
            };

            for (int x = 0; x < 4; x++)
            {
                nextPiece[x] = nextPieceArray[nextPieceInt, x];
            }
            foreach (Control square in nextPiece)
            {
                square.BackColor = colorList[nextPieceInt];
            }

            /// Options du template de la pièce actuelle
            Control[,] activePieceArray =
            {
                { box6, box16, box26, box36 }, 
                { box4, box14, box24, box25 }, 
                { box5, box15, box25, box24 }, 
                { box14, box15, box5, box6 }, 
                { box5, box6, box16, box17 }, 
                { box5, box6, box15, box16 },
                { box6, box15, box16, box17 }
            };

            // Select falling piece
            for (int x = 0; x < 4; x++)
            {
                activePiece[x] = activePieceArray[currentPiece, x];
            }

            for (int x = 0; x < 4; x++)
            {
                activePiece2[x] = activePieceArray[currentPiece, x];
            }

            // On vérifie si la partie est terminée
            foreach (Control box in activePiece)
            {
                if (box.BackColor != Color.White & box.BackColor != Color.LightGray)
                {
                    // Partie terminée
                    SpeedTimer.Stop();
                    GameTimer.Stop();
                    gameOver = true;
                    this.Hide();
                    var GameOver = new GameOver(score);
                    GameOver.Closed += (s, args) => this.Close();
                    GameOver.Show();
                    return;
                }
            }

            foreach (Control square in activePiece)
            {
                square.BackColor = colorList[currentPiece];
            }
        }

        // Test des déplacements
        public bool TestMove(string direction)
        {
            int currentHighRow = 21;
            int currentLowRow = 0;
            int currentLeftCol = 9;
            int currentRightCol = 0;

            int nextSquare = 0;

            Control newSquare = new Control();

            // Définition des mouvements possibles
            foreach (Control square in activePiece)
            {
                if (grid.GetRow(square) < currentHighRow)
                {
                    currentHighRow = grid.GetRow(square);
                }
                if (grid.GetRow(square) > currentLowRow)
                {
                    currentLowRow = grid.GetRow(square);
                }
                if (grid.GetColumn(square) < currentLeftCol)
                {
                    currentLeftCol = grid.GetColumn(square);
                }
                if (grid.GetColumn(square) > currentRightCol)
                {
                    currentRightCol = grid.GetColumn(square);
                }
            }

            // Vérifier si les pièces restent bien dans la grille
            foreach (Control square in activePiece)
            {
                int squareRow = grid.GetRow(square);
                int squareCol = grid.GetColumn(square);

                if (direction == "left" & squareCol > 0)
                {
                    newSquare = grid.GetControlFromPosition(squareCol - 1, squareRow);
                    nextSquare = currentLeftCol;
                }
                else if (direction == "left" & squareCol == 0)
                {
                    return false;
                }
                else if (direction == "right" & squareCol < 9)
                {
                    newSquare = grid.GetControlFromPosition(squareCol + 1, squareRow);
                    nextSquare = currentRightCol;
                }
                else if (direction == "right" & squareCol == 9)
                {
                    return false;
                }
                else if (direction == "down" & squareRow < 21)
                {
                    newSquare = grid.GetControlFromPosition(squareCol, squareRow + 1);
                    nextSquare = currentLowRow;
                }
                else if (direction == "down" & squareRow == 21)
                {
                    return false;
                }

                // Vérification des collisions
                if ((newSquare.BackColor != Color.White & newSquare.BackColor != Color.LightGray) & activePiece.Contains(newSquare) == false & nextSquare > 0)
                {
                    return false;
                }

            }

            // Si tous les tests sont passés
            return true;
        }

        public void MovePiece(string direction)
        {
            // On efface la position de la pièce
            // On détermine sa nouvelle position en fonction de la direction
            int x = 0;
            foreach (PictureBox square in activePiece)
            {
                square.BackColor = Color.White;
                int squareRow = grid.GetRow(square);
                int squareCol = grid.GetColumn(square);
                int newSquareRow = 0;
                int newSquareCol = 0;
                if (direction == "left")
                {
                    newSquareCol = squareCol - 1;
                    newSquareRow = squareRow;
                }
                else if (direction == "right")
                {
                    newSquareCol = squareCol + 1;
                    newSquareRow = squareRow;
                }
                else if (direction == "down")
                {
                    newSquareCol = squareCol;
                    newSquareRow = squareRow + 1;
                }

                activePiece2[x] = grid.GetControlFromPosition(newSquareCol, newSquareRow);
                x++;
            }

            x = 0;
            foreach (PictureBox square in activePiece2)
            {
                activePiece[x] = square;
                x++;
            }

            x = 0;
            foreach (PictureBox square in activePiece2)
            {
                square.BackColor = colorList[currentPiece];
                x++;
            }
        }

        private bool TestOverlap()
        {
            foreach (PictureBox square in activePiece2)
            {
                if ((square.BackColor != Color.White & square.BackColor != Color.LightGray) & activePiece.Contains(square) == false)
                {
                    return false;
                }
            }
            return true;
        }

        private void SpeedTimer_Tick(object sender, EventArgs e)
        {
            if (CheckGameOver() == true)
            {
                SpeedTimer.Stop();
                GameTimer.Stop();
                this.Hide();
                var gameOver = new GameOver(score);
                gameOver.Closed += (s, args) => this.Close();
                gameOver.Show();
            }

            else
            {
                if (TestMove("down") == true)
                {
                    MovePiece("down");
                }
                else
                {
                    if (CheckGameOver() == true)
                    {
                        SpeedTimer.Stop();
                        GameTimer.Stop();
                        this.Hide();
                        var gameOver = new GameOver(score);
                        gameOver.Closed += (s, args) => this.Close();
                        gameOver.Show();
                    }
                    if (CheckForCompleteRows() > -1)
                    {
                        ClearFullRow();
                    }
                    DropNewPiece();
                }
            }
        }
        // Temps de jeu (secondes écoulées)
        private void GameTimer_Tick(object sender, EventArgs e)
        {
            timeElapsed++;
            TimeLabel.Text = "Temps : " + timeElapsed.ToString() + "s";
        }

        // Efface la ligne complète la plus basse
        private void ClearFullRow()
        {
            int completedRow = CheckForCompleteRows();

            //Transforme cette rangée en blanc
            for (int x = 0; x <= 9; x++)
            {
                Control z = grid.GetControlFromPosition(x, completedRow);
                z.BackColor = Color.White;
            }

            //Déplace tous les autres carrés vers le bas
            for (int x = completedRow - 1; x >= 0; x--)
            {
                for (int y = 0; y <= 9; y++)
                {
                    Control z = grid.GetControlFromPosition(y, x);

                    Control zz = grid.GetControlFromPosition(y, x + 1);

                    zz.BackColor = z.BackColor;
                    z.BackColor = Color.White;
                }
            }

            UpdateScore();

            clears++;
            ClearsLabel.Text = "Validé: " + clears;


            if (score % 500 == 0)
            {
                SpeedUp();
            }
            if (CheckForCompleteRows() > -1)
            {
                ClearFullRow();
            }
        }

        private void UpdateScore()
        {
            score += 100;

            // Puis on l'affiche
            ScoreLabel.Text = "Score: " + score.ToString();
            ScoreUpdateTimer.Start();
        }

        // Renvoie le numéro de ligne de la ligne complète la plus basse
        // S'il n'y a pas de lignes complètes, retourne -1
        private int CheckForCompleteRows()
        {
            for (int x = 21; x >= 2; x--)
            {
                for (int y = 0; y <= 9; y++)
                {
                    Control z = grid.GetControlFromPosition(y, x);
                    if (z.BackColor == Color.White)
                    {
                        break;
                    }
                    if (y == 9)
                    {
                        return x;
                    }
                }
            }
            return -1; 
        }

        // Augmenter la vitesse du jeu en augmentant le niveau
        private void SpeedUp()
        {
            level++;
            LevelLabel.Text = "Level: " + level.ToString();

            // 3 Niveaux de difficultés
            // La vitesse augmente en fonction du niveau ET de la difficulté choisie
            int[] levelSpeed =
            {
                800, 760, 720, 680, 640, 600, 560, 520, 480, 440, 400, 360, 320, 280, 240,
                200, 160, 120, 080, 050, 050, 050, 050, 050, 030, 030, 030, 016, 016, 016
            };
            int[] levelSpeedMed =
            {
                700, 660, 620, 580, 540, 500, 460, 420, 380, 340, 300, 260, 220, 180, 140,
                100, 060, 033, 033, 033, 033, 033, 033, 033, 033, 033, 033, 016, 016, 016
            };
            int[] levelSpeedHard =
            {
                600, 560, 520, 480, 440, 400, 360, 320, 280, 240, 200, 160, 120, 080, 040,
                016, 016, 016, 016, 016, 016, 016, 016, 016, 016, 016, 016, 016, 016, 016
            };

            // La vitesse n'augmente plus après le niveau 29
            if (level <= 29)
            {
                if (game_type == "Easy")
                {
                    SpeedTimer.Interval = levelSpeed[level];
                    Console.WriteLine(SpeedTimer);
                }
                else if (game_type == "Medium")
                {
                    SpeedTimer.Interval = levelSpeedMed[level];
                    Console.WriteLine(SpeedTimer);
                }
                else if (game_type == "Hard")
                {
                    SpeedTimer.Interval = levelSpeedHard[level];
                    Console.WriteLine(SpeedTimer);
                }
            }
        }

        // Le jeu se termine si une pièce est dans la rangée du haut lorsque la pièce suivante tombe
        private bool CheckGameOver()
        {
            Control[] topRow = { box1, box2, box3, box4, box5, box6, box7, box8, box9, box10 };

            foreach (Control box in topRow)
            {
                if ((box.BackColor != Color.White & box.BackColor != Color.LightGray) & !activePiece.Contains(box))
                {
                    //Game over!
                    return true;
                }
            }

            if (gameOver == true)
            {
                return true;
            }

            return false;
        }

        // update du score toutes les 2s
        private void ScoreUpdateTimer_Tick(object sender, EventArgs e)
        {
            ScoreUpdateTimer.Stop();
        }
        private void MainWindow_Load(object sender, EventArgs e)
        {

        }
        private void ScoreLabel_Click(object sender, EventArgs e)
        {

        }

        private void LevelLabel_Click(object sender, EventArgs e)
        {

        }

        private void ClearsLabel_Click(object sender, EventArgs e)
        {

        }
    }
}
