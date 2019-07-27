using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnakeGUI
{
    public partial class Form1 : Form
    {
        public static int boardSize = 20;
        static bool[,] playerHistory = new bool[boardSize, boardSize];
        static int[,] historyAge = new int[boardSize, boardSize];
        static Cell[,] gameBoard = new Cell[boardSize, boardSize];
        static List<Cell> historyList = new List<Cell>();
        static int playerX = 0;
        static int playerY = 0;
        static bool gameOver = false;
        static int counter = 0;
        static int playerDX = 1;
        static int playerDY = 0;
        static int playerLength = 10;
        static int foodX = -1;
        static int foodY = -1;
        static Random rand = new Random();
        static Timer GameTimer = new Timer();

        public Form1()
        {
            InitializeComponent();
            this.KeyPreview = true;
            this.KeyPress +=
                new KeyPressEventHandler(Form1_KeyPress);

            GameTimer.Interval = 500;

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
           
            if (e.KeyChar == 'a')
            {
                Console.WriteLine("LEFT");
                playerDX = -1;
                playerDY = 0;
            }
            if (e.KeyChar == 'd')
            {
                Console.WriteLine("RIGHT");
                playerDX = 1;
                playerDY = 0;
            }
            if (e.KeyChar == 'w')
            {
                Console.WriteLine("UP");
                playerDX = 0;
                playerDY = -1;
            }
            if (e.KeyChar == 's')
            {
                Console.WriteLine("DOWN");
                playerDX = 0;
                playerDY = 1;
            }
        }

        private void StartButton_Click(object sender, EventArgs e)
        {

            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    gameBoard[i, j] = new Cell(i, j);
                    gameBoard[i, j].Location = new Point((i + 1) * 21, (j + 2) * 21);
                    gameBoard[i, j].Size = new System.Drawing.Size(21, 21);
                    //gameBoard[i, j].Click += cellClicked;
                    this.Controls.Add((gameBoard[i, j]));
                }
            }
            gameBoard[0, 0].Image = SnakeGUI.Properties.Resources.snakeHR;
            historyList.Add(gameBoard[playerX, playerY]);
            startButton.Hide();
            Console.WriteLine("START");
            GameTimer.Start();
            GameTimer.Tick += new EventHandler(Timer_Tick);
            
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            //Console.WriteLine("TICK");
            //historyList.Add(gameBoard[playerX, playerY]);

            //gameBoard[playerX, playerY].playerPresent = false;
            bool foundInHistory = false;
            playerX += playerDX;
            playerY += playerDY;

            //gameBoard[playerX, playerY].Image = SnakeGUI.Properties.Resources.snakeHR;
            gameBoard[playerX, playerY].playerPresent = true;
            gameBoard[playerX, playerY].lengthTimer = playerLength;
            
            for (int i = historyList.Count - 1; i >= 0; i--)
            {
                if (historyList[i].cellX == playerX && historyList[i].cellY == playerY)
                {
                    historyList[i] = gameBoard[playerX, playerY];
                    foundInHistory = true;
                }
            }
            if (!foundInHistory)
                historyList.Add(gameBoard[playerX, playerY]);


            processCells(playerDX, playerDY);
        }

        private void processCells(int pDX, int pDY)
        {
            int processCounter = 1;
            foreach (Cell aCell in historyList)
            {
                aCell.processCell(pDX, pDY);
                Console.WriteLine("Processed cell" + processCounter + " length: " + aCell.lengthTimer);
               // if (aCell.lengthTimer <= 0)
                   // historyList.Remove(aCell);
                processCounter++;
            }
            for (int i = historyList.Count - 1; i >= 0; i--)
            {
                if (historyList[i].lengthTimer < 0)
                    historyList.RemoveAt(i);
            }

        }

    }

    
}
