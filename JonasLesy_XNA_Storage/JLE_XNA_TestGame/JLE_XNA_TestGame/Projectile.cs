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
    /// A class for projectiles.
    /// </summary>
    class Projectile
    {
        bool projectileDirection = true;
        bool visible = false;
        const int MOVE_LEFT = -3;
        const int MOVE_RIGHT = 3;

        public void setDirectionRight(bool newBool)
        {
            projectileDirection = newBool;
        }

        public bool getDirection()
        {
            return projectileDirection;
        }

        public void setVisible(bool newBool)
        {
            visible = newBool;
        }

        public bool getVisible()
        {
            return visible;
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

        public void setXCoord(int pXCoord)
        {
            mDimensions.X = pXCoord;
        }

        // Return the Y coordinate
        public int getYCoord()
        {
            return mDimensions.Y;
        }

        // A method to set the X coordinate of the game object.
        public void setYCoord(int pYCoord)
        {
            mDimensions.Y = pYCoord;
        }
    }
}
