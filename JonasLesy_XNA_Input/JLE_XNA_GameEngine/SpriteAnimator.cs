using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JLE_XNA_GameEngine
{
    public class SpriteAnimator
    {
        // Coordinates used to see which part of the sprites is displayed.
        private int XCoord = 0;
        private int YCoord = 0;

        // Counters to keep track of the sprite times.
        private int XCounter = 0;
        private int YCounter = 0;

        // Get details about the spritesheet
        private int numberOfLines = 0;
        private int numberOfColumns = 0;
        private int spriteWidth = 0;
        private int spriteHeight = 0;
        private int numberOnLastLine = 0;

        public SpriteAnimator()
        {
        }

        //Initialize all of the values.
        public void InitializeSprite(int pLines, int pColumns, int pSpriteWidth, int pSpriteHeight, int pNumberOnLastLine)
        {
            XCoord = 0;
            YCoord = 0;
            XCounter = 0;
            YCounter = 0;
            numberOfLines = pLines;
            numberOfColumns = pColumns;
            spriteWidth = pSpriteWidth;
            spriteHeight = pSpriteHeight;
            numberOnLastLine = pNumberOnLastLine;
        }

        //Animate the sprite
        public void AnimateSprite()
        {
            //If the position is located on the 
            if (XCounter < numberOfColumns && YCounter < numberOfLines)
            {
                // Go to next sprite in the current row of the sheet.
                XCoord += spriteWidth;
            }

            // If we have reached the end of the row but not the end of the sheet ...
            else if (XCounter >= numberOfColumns && YCounter < numberOfColumns)
            {
                // Go to the beginning of the next row
                XCounter = 0;
                YCounter++;

                // Update X and Y coordinates
                XCoord = 0;
                YCoord += spriteHeight;
            }
            // If the last line is reached and the fourth picture is shown, the counters and coordinates are reset to 0 because there are no more picture on that line!
            else if (XCounter > numberOnLastLine)
            {
                // Go to the beginning of the first row.
                XCounter = 0;
                YCounter = 0;

                // Update X and Y coordinates
                XCoord = 0;
                YCoord = 0;
            }
            // Increment the Xcounter, so the next image will be taken.
            XCounter++;
        }

        // Return the X coordinate
        public int getXCoord()
        {
            return XCoord;
        }

        // Return the Y coordinate
        public int getYCoord()
        {
            return YCoord;
        }

        // Return the X counter
        public int getXCounter()
        {
            return XCounter;
        }

        // Return the Y counter
        public int getYCounter()
        {
            return YCounter;
        }
    }
}
