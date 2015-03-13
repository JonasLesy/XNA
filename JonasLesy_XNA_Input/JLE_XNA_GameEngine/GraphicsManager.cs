//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Audio;
//using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Input;
//using Microsoft.Xna.Framework.Media;

namespace JLE_XNA_GameEngine
{
    // The sealed keyword defines that this class is a singleton.
    public sealed class GraphicsManager
    {
        // GraphicsDeviceManager that handles configuarion and management of the graphics device.
        GraphicsDeviceManager mGraphics;

        // Object that enables group of sprites to be drawn with the same settings.
        SpriteBatch mSpriteBatch;

        // The game object that uses this graphics manager to draw its 2D content.
        Game mGame;

        //The safe drawing area. In practice the dimensions of the viewport.
        private Rectangle TitleSafe;

        // Instance for the graphics manager and padlock for making the implementation thread safe.
        static GraphicsManager mInstance = null;
        static readonly object padlock = new object();

        GraphicsManager()
        {
        }

        public static GraphicsManager Instance
        {
            get
            {
                lock (padlock)
                {
                    if (mInstance == null)
                    {
                        mInstance = new GraphicsManager();
                    }
                    return mInstance;
                }
            }
        }

        // Initialize the game
        public void initialize(Game pGame)
        {
            mGame = pGame;
            mGraphics = new GraphicsDeviceManager(mGame);
        }

        public void InitializeContent()
        {
            // Create new SpriteBatch.
            mSpriteBatch = new SpriteBatch(mGame.GraphicsDevice);

            // Get the dimensions within which the drawing can occur.
            TitleSafe = GetTitleSafeArea();
        }

        // Clear the screen and fill it with a color.
        public void ClearScreen(Color pColor)
        {
            mGraphics.GraphicsDevice.Clear(pColor);
        }

        // Draw the picture in a safe place.
        public void Draw(Texture2D pSpriteTexture)
        {
            mSpriteBatch.Begin();
            Vector2 pos = new Vector2(TitleSafe.Left, TitleSafe.Top);
            mSpriteBatch.Draw(pSpriteTexture, pos, Color.White);
            mSpriteBatch.End();
        }

        // Draw the texture to fill the screen.
        public void DrawFS(Texture2D pSpriteTexture)
        {
            mSpriteBatch.Begin();
            mSpriteBatch.Draw(pSpriteTexture, new Rectangle(0, 0, mGraphics.GraphicsDevice.Viewport.Width, mGraphics.GraphicsDevice.Viewport.Height), Color.White);
            mSpriteBatch.End();
        }

        // Draw specified part of texture to specified position
        public void DrawPI(Texture2D pSpriteTexture, Vector2 pPosition, Rectangle pSourceRectangle)
        {
            mSpriteBatch.Begin();
            mSpriteBatch.Draw(pSpriteTexture, pPosition, pSourceRectangle, Color.White);
            mSpriteBatch.End();
        }

        // Draw texture to specified position.
        public void DrawPS(Texture2D pSpriteTexture, Rectangle pSourceRectangle)
        {
            mSpriteBatch.Begin();
            mSpriteBatch.Draw(pSpriteTexture, pSourceRectangle, Color.White);
            mSpriteBatch.End();
        }

        // Draw a part of the texture to specified position.
        public void DrawPIPS(Texture2D pSpriteTexture, Rectangle pSourceRectangle, Rectangle pDestinationRectangle)
        {
            mSpriteBatch.Begin();
            mSpriteBatch.Draw(pSpriteTexture, pDestinationRectangle, pSourceRectangle, Color.White);
            mSpriteBatch.End();
        }

        // Define the dimensions of the screen.
        public Rectangle GetTitleSafeArea()
        {
            Rectangle retval = new Rectangle(
            
                mGraphics.GraphicsDevice.Viewport.X,
                mGraphics.GraphicsDevice.Viewport.Y,
                mGraphics.GraphicsDevice.Viewport.Width,
                mGraphics.GraphicsDevice.Viewport.Height);

            return retval;
        }

        // Return the SpriteBatch.
        public SpriteBatch getSpriteBatch()
        {
            return mSpriteBatch;
        }
    }
}
