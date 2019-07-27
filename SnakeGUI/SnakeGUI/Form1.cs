using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SnakeGUI
{
    public partial class Form1 : Form
    {
        public static int boardSize = 20;
        static Cell[,] gameBoard = new Cell[boardSize, boardSize];
        static List<Cell> historyList = new List<Cell>(); //CELLS THAT ARE PART OF PLAYER BODY
        static int playerX = 0;
        static int playerY = 0;
        static int playerDX = 1;
        static int playerDY = 0;
        static int lastDX = 1;
        static int lastDY = 0;
        static int playerLength = 1;
        static int foodX = -1;
        static int foodY = -1;
        static Random rand = new Random();
        static Timer GameTimer = new Timer();
        static bool firstGame = true;

        public Form1()
        {
            InitializeComponent();
            this.KeyPreview = true;
            this.KeyPress +=
                new KeyPressEventHandler(Form1_KeyPress); //ADD KEYPRESS HANDLER
            GameTimer.Interval = 250; //SET TICK INTERVAL
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            //USE KEYPRESSES TO CHANGE PLAYER DIRECTION - CAN'T TURN BACK ON SELF
            if (e.KeyChar == 'a' && lastDX != 1)
            {
                Console.WriteLine("LEFT");
                playerDX = -1;
                playerDY = 0;
            }
            if (e.KeyChar == 'd' && lastDX != -1)
            {
                Console.WriteLine("RIGHT");
                playerDX = 1;
                playerDY = 0;
            }
            if (e.KeyChar == 'w' && lastDY != 1)
            {
                Console.WriteLine("UP");
                playerDX = 0;
                playerDY = -1;
            }
            if (e.KeyChar == 's' && lastDY != -1)
            {
                Console.WriteLine("DOWN");
                playerDX = 0;
                playerDY = 1;
            }
            //END KEYPRESS HANDLING
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            //INITIALIZE BOARD
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
            }//BOARD INITIALIZATION COMPLETE

            gameBoard[0, 0].Image = SnakeGUI.Properties.Resources.snakeHR; //DRAW PLAYER
            historyList.Add(gameBoard[playerX, playerY]); //ADD CURRENT POSITION TO HISTORY
            GenerateFood(); //GENERATE FIRST FOOD
            gameBoard[foodX, foodY].Image = SnakeGUI.Properties.Resources.snakeFood; //DRAW FIRST FOOD
            startButton.Hide(); //HIDE THE START BUTTON
            Console.WriteLine("START");
            GameTimer.Start(); //START THE TIMER FOR GAME TICKS
            if(firstGame)
                GameTimer.Tick += new EventHandler(Timer_Tick); //ADD GAME LOGIC TO TICK EVENT IF FIRST PLAYTHROUGH
            
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            //bool foundInHistory = false; //CHECK IF 

            lastDX = playerDX; //FOR PREVENTING TURNBACK
            lastDY = playerDY; //FOR PREVENTING TURNBACK
            playerX += playerDX; //MOVE PLAYER
            playerY += playerDY; //MOVE PLAYER

            if(playerX < 0 || playerX > boardSize-1 || playerY < 0 || playerY > boardSize-1)
            {//DID PLAYER GO OUT OF BOUNDS?
                GameTimer.Stop();
                gameOverYall();
                return;
            }

            if(playerX == foodX && playerY == foodY)
            {//DID PLAYER HIT FOOD?
                playerLength += 1;
                while(!GenerateFood())//GENERATE FOOD UNTIL VALID PLACEMENT
                {
                    GenerateFood();
                }
                gameBoard[foodX, foodY].Image = SnakeGUI.Properties.Resources.snakeFood;//DRAW FOOD
            }


            gameBoard[playerX, playerY].playerPresent = true; //MARK PLAYER AT NEXT CELL
            gameBoard[playerX, playerY].lengthTimer = playerLength; //SET LENGTH TIMER FOR CURRENT CELL
            
            for (int i = historyList.Count - 1; i >= 0; i--)
            { //DID PLAYER HIT BODY?
                if (historyList[i].cellX == playerX && historyList[i].cellY == playerY)
                {
                    GameTimer.Stop();
                    gameOverYall();
                    return;
                }
            }

            historyList.Add(gameBoard[playerX, playerY]); //ADD POSITION TO BODYLIST


            processCells(playerDX, playerDY); //ITERATE BODY CELLS
        }

        private void processCells(int pDX, int pDY)
        {
            foreach (Cell aCell in historyList)
            {//CALL CELL TO ITERATE AND DRAW CORRECT TILE
                aCell.processCell(pDX, pDY); 
            }

            for (int i = historyList.Count - 1; i >= 0; i--)
            {//REMOVE CELL FROM LIST IF TIMER EXPIRES
                if (historyList[i].lengthTimer < 0)
                    historyList.RemoveAt(i);
            }

        }

        private void gameOverYall()
        {
            //DRAW PLAYER POINTING IN FINAL DIRECTION
            if (playerDX == 1 && playerDY == 0)
                gameBoard[playerX - playerDX, playerY - playerDY].Image = SnakeGUI.Properties.Resources.snakeHR;
            if (playerDX == -1 && playerDY == 0)
                gameBoard[playerX - playerDX, playerY - playerDY].Image = SnakeGUI.Properties.Resources.snakeHL;
            if (playerDX == 0 && playerDY == 1)
                gameBoard[playerX - playerDX, playerY - playerDY].Image = SnakeGUI.Properties.Resources.snakeHD;
            if (playerDX == 0 && playerDY == -1)
                gameBoard[playerX - playerDX, playerY - playerDY].Image = SnakeGUI.Properties.Resources.snakeHU;
            //FINISH DRAWING

            historyList.Clear(); //CLEAR BODY HISTORY
                        
            playerX = 0;
            playerY = 0;
            playerDX = 1;
            playerDY = 0;
            playerLength = 1;
            startButton.Show();

            MessageBox.Show("GAME OVER");

            for (int i = 0; i < boardSize; i++)
            {//REMOVE CELLS FROM GAMEBOARD
                for (int j = 0; j < boardSize; j++)
                {
                    this.Controls.Remove((gameBoard[i, j]));
                }
            }
            firstGame = false; //NO LONGER FIRST PLAY, DON'T CREATE NEW TICK HANDLER
        }

        private bool GenerateFood() //GENERATE FOOD, CHECK THAT FOOD DOESN'T GENERATE ON PLAYER
        {
            foodX = rand.Next(1, boardSize - 1);
            foodY = rand.Next(1, boardSize - 1);

            foreach (Cell aCell in historyList)
            {
                if(aCell.cellX == foodX && aCell.cellY == foodY)
                {
                    return false;
                }
            }

            return true;
        }

    }

    
}
