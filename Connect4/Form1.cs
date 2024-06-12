using System;
using System.Drawing;
using System.Windows.Forms;

namespace Connect4
{
    public partial class Form1 : Form
    {
        private Button[,] buttons;
        private char[,] board;
        private Color[] playerColors = { Color.Red, Color.Blue };
        private int currentPlayerIndex = 0;

        public Form1()
        {
            InitializeComponent();
            InitializeBoard();
        }

        private void InitializeBoard()
        {
            buttons = new Button[6, 7];
            board = new char[6, 7];

            for (int row = 0; row < 6; row++)
            {
                for (int col = 0; col < 7; col++)
                {
                    buttons[row, col] = new Button
                    {
                        Dock = DockStyle.Fill,
                        Font = new Font("Arial", 24, FontStyle.Bold),
                        Tag = new Point(row, col),
                        BackColor = Color.White
                    };
                    buttons[row, col].Click += new EventHandler(Button_Click);
                    tableLayoutPanel1.Controls.Add(buttons[row, col], col, row);
                    board[row, col] = '.';
                }
            }
        }

        private void Button_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            Point point = (Point)button.Tag;
            int col = point.Y;

            for (int row = 5; row >= 0; row--)
            {
                if (board[row, col] == '.')
                {
                    board[row, col] = currentPlayerIndex == 0 ? 'X' : 'O';
                    buttons[row, col].BackColor = playerColors[currentPlayerIndex];
                    if (CheckForWin(row, col))
                    {
                        MessageBox.Show($"Player {currentPlayerIndex + 1} wins!");
                        ResetBoard();
                    }
                    else if (BoardIsFull())
                    {
                        MessageBox.Show("It's a draw!");
                        ResetBoard();
                    }
                    currentPlayerIndex = (currentPlayerIndex + 1) % 2;
                    return;
                }
            }
        }

        private bool BoardIsFull()
        {
            for (int j = 0; j < 7; j++)
            {
                if (board[0, j] == '.')
                {
                    return false;
                }
            }
            return true;
        }

        private bool CheckForWin(int row, int col)
        {
            char player = board[row, col];
            return (CheckDirection(row, col, 1, 0, player) + CheckDirection(row, col, -1, 0, player) > 2 ||
                    CheckDirection(row, col, 0, 1, player) + CheckDirection(row, col, 0, -1, player) > 2 ||
                    CheckDirection(row, col, 1, 1, player) + CheckDirection(row, col, -1, -1, player) > 2 ||
                    CheckDirection(row, col, 1, -1, player) + CheckDirection(row, col, -1, 1, player) > 2);
        }

        private int CheckDirection(int row, int col, int dRow, int dCol, char player)
        {
            int count = 0;
            for (int i = 1; i < 4; i++)
            {
                int r = row + dRow * i;
                int c = col + dCol * i;
                if (r < 0 || r >= 6 || c < 0 || c >= 7 || board[r, c] != player)
                {
                    break;
                }
                count++;
            }
            return count;
        }

        private void ResetBoard()
        {
            for (int row = 0; row < 6; row++)
            {
                for (int col = 0; col < 7; col++)
                {
                    board[row, col] = '.';
                    buttons[row, col].BackColor = Color.White;
                }
            }
            currentPlayerIndex = 0;
        }
    }
}
