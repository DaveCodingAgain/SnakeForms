using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace SnakeGUI
{
    class Cell : PictureBox
    {
        // public Bitmap emptyCell = new Bitmap(@"C:\Users\davemin\OneDrive\Documents\C#\SnakeGUI\SnakeGUI\SnakeGUI\emptyCell.bmp");
        // public Bitmap snakeHR = new Bitmap(@"C:\Users\davemin\OneDrive\Documents\C#\SnakeGUI\SnakeGUI\SnakeGUI\snakeHR.bmp");
        // public Bitmap snakeHL = new Bitmap(@"C:\Users\davemin\OneDrive\Documents\C#\SnakeGUI\SnakeGUI\SnakeGUI\snakeHL.bmp");
        // public Bitmap snakeHU = new Bitmap(@"C:\Users\davemin\OneDrive\Documents\C#\SnakeGUI\SnakeGUI\SnakeGUI\snakeHU.bmp");
        // public Bitmap snakeHD = new Bitmap(@"C:\Users\davemin\OneDrive\Documents\C#\SnakeGUI\SnakeGUI\SnakeGUI\snakeHD.bmp");
        //public Bitmap snakeHD2 = new Bitmap(resource: snakeHD.bmp);
        //public Bitmap clickedCell = new Bitmap(@"C:\Users\dbuckner\source\repos\SnakeGUI\SnakeGUI\clickedCell.bmp");
        public bool playerPresent = false;
        public int lengthTimer = 0;
        public int cellX;
        public int cellY;

        public Cell(int cX, int cY)
        {
            cellX = cX;
            cellY = cY;
            this.Image = SnakeGUI.Properties.Resources.emptyCell; //INITIALIZE AS EMPTY CELL
            
        }

        public void processCell(int playerDX, int playerDY)
        { //CHECK FOR PLAYER, DECREMENT TIMERS, DRAW CELL
            if(playerPresent)
            { //DRAW PLAYER PICTURE IN APPROPRIATE DIRECTION
                if (playerDX == 1 && playerDY == 0)
                    this.Image = SnakeGUI.Properties.Resources.snakeHR;
                if (playerDX == -1 && playerDY == 0)
                    this.Image = SnakeGUI.Properties.Resources.snakeHL;
                if (playerDX == 0 && playerDY == 1)
                    this.Image = SnakeGUI.Properties.Resources.snakeHD;
                if (playerDX == 0 && playerDY == -1)
                    this.Image = SnakeGUI.Properties.Resources.snakeHU;
            }

            if (!playerPresent)
            { //IF PLAYER IS NO LONGER PRESENT, DECREMENT TIMER AND DRAW APPROPRIATE CELL
                if (lengthTimer > 0)
                    this.Image = SnakeGUI.Properties.Resources.snakeBody;
                else
                    this.Image = SnakeGUI.Properties.Resources.emptyCell;
            }

            lengthTimer -= 1;
            playerPresent = false; //PROCESSING COMPLETE, PLAYER WILL MOVE OFF CELL NEXT TICK

        }
    }
}
