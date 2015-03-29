using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

// Own Imports
using JLE_XNA_GameEngine;

namespace JLE_XNA_TestGame
{
    /// <summary>
    /// A class for the game character.
    /// </summary>
    class Character
    {
        private int START_Y;
        private int START_X;
        private int MOVE_UP;
        private int MOVE_LEFT;
        private int MOVE_RIGHT;
        private bool visible = true;
        private bool goingRight = true;

        public Character(int Y, int X, int Up, int Left, int Right)
        {
            START_Y = Y;
            START_X = X;
            MOVE_UP = Up;
            MOVE_LEFT = Left;
            MOVE_RIGHT = Right;
            mDimensions.Y = START_Y;
            mDimensions.X = START_X;
        }

        public void setVisible(bool newBool)
        {
            visible = newBool;
        }

        public bool getVisible()
        {
            return visible;
        }

        public void increaseSpeed(int speedToAdd)
        {
            MOVE_LEFT += speedToAdd;
            MOVE_RIGHT += speedToAdd;
        }

        public void setDirectionRight(bool newBool)
        {
            goingRight = newBool;
        }

        public bool getDirectionRight()
        {
            return goingRight;
        }

        public void setStartY(int Y)
        {
            START_Y = Y;
        }

        public int getStartY()
        {
            return START_Y;
        }

        private Rectangle mDimensions;
        public Rectangle Dimensions
        {
            get
            {
                return mDimensions;
            }
            set
            {
                mDimensions = value;
            }
        }

        // Texture of the gameobject with a getter
        private Texture2D mObjectTexture;
        public Texture2D ObjectTexture
        {
            get
            {
                return mObjectTexture;
            }
        }
        // The X coord used to flip the character around.
        private int mXSpriteCoord = 0;
        public int getXSpriteCoord()
        {
            return mXSpriteCoord;
        }

        public void setXSpriteCoord(int newXSpriteCoord)
        {
            mXSpriteCoord = newXSpriteCoord;
        }

        // A method for setting the object texture.
        public void setTexture(Texture2D pTexture)
        {
            mObjectTexture = pTexture;
        }

        // A method for increasing the X coordinate of the game object.
        public void increaseXCoordinate(int pChange)
        {
            mDimensions.X += pChange;
        }

        // A method for increasing the Y coordinate of the game object.
        public void increaseYCoordinate(int pChange)
        {
            mDimensions.Y -= pChange;
        }

        // A method for decreasing the X coordinate of the game object.
        public void decreaseXCoordinate(int pChange)
        {
            mDimensions.X -= pChange;
        }

        // A method for decreasing the Y coordinate of the game object.
        public void decreaseYCoordinate(int pChange)
        {
            mDimensions.Y += pChange;
        }

        // Return the X coordinate
        public int getXCoord()
        {
            return mDimensions.X;
        }

        // Return the Y coordinate
        public int getYCoord()
        {
            return mDimensions.Y;
        }

        // A method to set the X coordinate of the game object.
        public void setXCoordinate(int pXCoord)
        {
            mDimensions.X = pXCoord;
        }

        // A method to set the Y coordinate of the game object.
        public void setYCoordinate(int pYCoord)
        {
            mDimensions.Y = pYCoord;
        }

        public int getMoveLeft()
        {
            return MOVE_LEFT;
        }

        public int getMoveRight()
        {
            return MOVE_RIGHT;
        }

        public int getMoveUp()
        {
            return MOVE_UP;
        }
    }
}
