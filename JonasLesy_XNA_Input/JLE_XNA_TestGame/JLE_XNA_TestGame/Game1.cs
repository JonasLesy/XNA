// General includes.
using System;
using System.Collections.Generic;
using System.Linq;

// XNA includes.
using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Audio;
//using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
//using Microsoft.Xna.Framework.Media;

// Own includes.
using JLE_XNA_GameEngine;

//Namespace for the project
namespace JLE_XNA_TestGame
{

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        // I use the following GameState enum to check whether the game has started or not.
        public enum GameState
        {
            Menu,
            Game,
            Won
            // More gamestates might be added later on.
        }

        // Make a global variable of the GameState enum.
        GameState gameState;

        // Singleton object for handling XNA graphics routines.
        GraphicsManager GraMa = GraphicsManager.Instance;

        // Singleton object for handling XNA input routines.
        InputManager InpMa = InputManager.Instance;

        // Texture to draw the 2D image.
        Texture2D mCharacterSpriteTexture;
        Texture2D mBulletSpriteTexture;

        // The objects for the characters and projectiles.
        Character mScout;
        Character mBirdEnemy;
        Projectile mBullet;

        // A variable to keep the score.
        private int pointsScored;

        // The spritebatch to draw.
        SpriteBatch mSpriteBatch;

        // The fonts for displaying text.
        private SpriteFont mTitleFont;
        private SpriteFont mNormalFont;

        // Save the safe dimensions for the screen.
        private Rectangle titleSafe;

        // Sprite and animator for the bird enemy.
        private Texture2D mBirdSpriteTexture;
        private SpriteAnimator mBirdAnimator;

        // Time interval to move the sprite.
        const double timestep = 0.2;
        double timetick = timestep;

        public Game1()
        {
            // Call the initialize function of the graphics and input manager.
            GraMa.Initialize(this);
            InpMa.initialize(this);

            // Set up content directory for the game.
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // Initialize the graphics manager
            GraMa.InitializeContent();

            // Initialize the gameState
            //gameState = GameState.Menu;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Set the score to 0.
            pointsScored = 0;

            // Set the gamestate to 'menu'
            gameState = GameState.Menu;

            // Initialize the playable character, the enemy and the projectiles.
            mScout = new Character((GraMa.GetTitleSafeArea().Height - 100), GraMa.GetTitleSafeArea().Width / 2, 2, 2, 2);
            mBirdEnemy = new Character((GraMa.GetTitleSafeArea().Height - 90), GraMa.GetTitleSafeArea().Width, 2, 2, 2);
            mBullet = new Projectile();

            // Initialization of the images.
            mCharacterSpriteTexture = Content.Load<Texture2D>("Images/CharacterSprite");
            mBulletSpriteTexture = Content.Load<Texture2D>("Images/Bullet");

            // Load character texture.
            mScout.setTexture(Content.Load<Texture2D>("Images/CharacterSprite"));
            mScout.setYCoordinate(mScout.getYCoord());
            mScout.setXCoordinate(mScout.getXCoord());

            // Load the bullet texture.
            mBullet.setTexture(Content.Load<Texture2D>("Images/Bullet"));
            mBullet.setYCoord(mScout.getYCoord());

            // Load the bird texture.
            mBirdEnemy.setTexture(Content.Load<Texture2D>("Images/birdEnemyLeft"));
            mBirdEnemy.setYCoordinate(mBirdEnemy.getYCoord());
            mBirdEnemy.setXCoordinate(mBirdEnemy.getXCoord());

            // Create and initialize the sprite.
            mBirdAnimator = new SpriteAnimator();
            mBirdAnimator.InitializeSprite(3, 2, 60, 49, 2);
            mBirdSpriteTexture = Content.Load<Texture2D>("Images/birdEnemyLeft");

            // Here I load the spritebatch to display in it later.
            mSpriteBatch = GraMa.getSpriteBatch();

            // We load the fonts
            mTitleFont = Content.Load<SpriteFont>("TitleFont");
            mNormalFont = Content.Load<SpriteFont>("NormalFont");

            // Set the same dimensions to those from the GraphicalManager.
            titleSafe = GraMa.GetTitleSafeArea();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            InpMa.Update();

            if (InpMa.keyboardButtonPressed(Keys.Escape))
            {
                this.Exit();
            }

            if (InpMa.keyboardButtonPressed(Keys.R))
            {
                LoadContent();
            }

            if (gameState == GameState.Menu)
            {
                if (InpMa.keyboardButtonPressed(Keys.Enter))
                {
                    gameState = GameState.Game;
                }
            }

            if (gameState == GameState.Game)
            {
                try
                {
                    if (mBullet.getVisible() && mBirdEnemy.getVisible())
                    {
                        if (Enumerable.Range(mBirdEnemy.getXCoord(), mBirdEnemy.getXCoord() + 47).Contains(mBullet.getXCoord()))
                        {
                            mBirdEnemy.setVisible(false);
                            mBullet.setVisible(false);
                            pointsScored++;

                            if (pointsScored == 5 || pointsScored == 10)
                            {
                                mBirdEnemy.increaseSpeed(2);
                            }

                        }
                    }
                }
                catch (ArgumentOutOfRangeException)
                {
                }


                if (!mBirdEnemy.getVisible())
                {
                    mBirdEnemy = new Character((GraMa.GetTitleSafeArea().Height - 90), GraMa.GetTitleSafeArea().Width, 2, mBirdEnemy.getMoveLeft(), mBirdEnemy.getMoveRight());
                    mBirdEnemy.setXCoordinate(mBirdEnemy.getXCoord());
                    mBirdEnemy.setYCoordinate(mBirdEnemy.getYCoord());
                    mBirdEnemy.setTexture(Content.Load<Texture2D>("Images/birdEnemyLeft"));
                    mBirdSpriteTexture = Content.Load<Texture2D>("Images/birdEnemyLeft");
                    mBirdEnemy.setVisible(true);
                }

                if (gameTime.TotalGameTime.TotalSeconds > timetick)
                {
                    // Set the next tick.
                    timetick += timestep;

                    //Animate the sprite
                    mBirdAnimator.AnimateSprite();

                    //Update the game.
                    base.Update(gameTime);
                }

                mBirdEnemy.decreaseXCoordinate(mBirdEnemy.getMoveLeft());
                
                
                //if (mScout.getYCoord() > (titleSafe.Height-mScout.ObjectTexture.Height-10))
                //{
                //    //mScout.decreaseYCoordinate(5);
                //    mScout.setYCoordinate(mScout.getStartY());
                //}

                if (InpMa.keyboardButtonDown(Keys.Right))
                {
                    if (mScout.getXCoord() < (titleSafe.Width - mScout.ObjectTexture.Width / 2))
                    {
                        mScout.increaseXCoordinate(mScout.getMoveRight());
                        mScout.setXSpriteCoord(0);
                        mBullet.setXSpriteCoord(0);
                    }
                }

                if (InpMa.keyboardButtonDown(Keys.Left))
                {
                    if (mScout.getXCoord() > 0)
                    {
                        mScout.decreaseXCoordinate(mScout.getMoveLeft());
                        mScout.setXSpriteCoord(80);
                        mBullet.setXSpriteCoord(35);
                    }
                }

                if (InpMa.keyboardButtonDown(Keys.Up))
                {
                    if (mScout.getYCoord() > 0)
                    {
                        mScout.increaseYCoordinate(mScout.getMoveUp());
                    }
                }

                if (InpMa.keyboardButtonDown(Keys.Down))
                {
                    if (mScout.getYCoord() < (titleSafe.Height - mScout.ObjectTexture.Height))
                    {
                        mScout.decreaseYCoordinate(2);
                    }
                }

                if (InpMa.keyboardButtonPressed(Keys.Space) || InpMa.mouseButtonPressed(Mouse.GetState().LeftButton))
                {
                    if (!mBullet.getVisible())
                    {
                        mBullet.setVisible(true);
                        if (mScout.getXSpriteCoord() == 0)
                        {
                            mBullet.setDirection(true);
                            mBullet.setXCoord(mScout.Dimensions.X + 80);
                            mBullet.setYCoord(mScout.Dimensions.Y + 14);
                        }
                        else
                        {
                            mBullet.setDirection(false);
                            mBullet.setXCoord(mScout.Dimensions.X - 30);
                            mBullet.setYCoord(mScout.Dimensions.Y + 14);
                        }
                    }
                }
                if(pointsScored == 15)
                {
                    gameState = GameState.Won;
                }

            }
                if (gameTime.TotalGameTime.TotalSeconds > timetick)
                {
                    // Set the next tick.
                    timetick += timestep;

                    //Update the game.
                    base.Update(gameTime);
                }
            
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // Clear the screen and set the color to blue.
            GraMa.ClearScreen(Color.RoyalBlue);

            // If the player is in the Menu gamestate
            if (gameState == GameState.Menu)
            {
                mSpriteBatch.Begin();
                mSpriteBatch.DrawString(mTitleFont, "XNA SHOOTER", new Vector2(50, titleSafe.Height / 3), Color.Black);
                mSpriteBatch.DrawString(mNormalFont, "Press ENTER to start playing!", new Vector2(titleSafe.Width / 2 - 200, titleSafe.Height / 2 + 40), Color.White);
                mSpriteBatch.DrawString(mNormalFont, "Game controls:\n---------------\nLeft/Right/Jump: according keyboard buttons\nShoot: Space or Left-click\nReset: R-key\nQuit:  Escape-key", new Vector2(5, titleSafe.Height - 150), Color.Black);
                mSpriteBatch.DrawString(mNormalFont, "By Jonas Lesy", new Vector2(titleSafe.Width - 150, titleSafe.Height - 30), Color.Black);
                mSpriteBatch.End();
            }

            if (gameState == GameState.Game)
            {
                // Start the SpriteBatch
                mSpriteBatch.Begin();

                mSpriteBatch.DrawString(mNormalFont, "Score: " + pointsScored, new Vector2(10, 0), Color.Black);

                if (mBirdEnemy.getVisible())
                {
                    mSpriteBatch.Draw(mBirdEnemy.ObjectTexture, new Rectangle(mBirdEnemy.getXCoord(), mBirdEnemy.getYCoord(), 60, 49), new Rectangle(mBirdAnimator.getXCoord(), mBirdAnimator.getYCoord(), 60, 49), Color.White);
                }

                mSpriteBatch.Draw(mScout.ObjectTexture, new Rectangle(mScout.getXCoord(), mScout.getYCoord(), 80, 102), new Rectangle(mScout.getXSpriteCoord(), 0, 80, 102), Color.White);
                if (mBullet.getVisible())
                {
                    mSpriteBatch.Draw(mBullet.ObjectTexture, new Rectangle(mBullet.Dimensions.X, mBullet.Dimensions.Y, 35, 12), new Rectangle(mBullet.getXSpriteCoord(), 0, 35, 12), Color.White);
                    if (mBullet.getDirection())
                    {
                        mBullet.setXCoord(mBullet.Dimensions.X + 5);
                    }
                    else
                    {
                        mBullet.setXCoord(mBullet.Dimensions.X - 5);
                    }
                }
                if (mBullet.Dimensions.X < -35 || mBullet.Dimensions.X > GraMa.GetTitleSafeArea().Width)
                {
                    mBullet.setVisible(false);
                }

                mSpriteBatch.End();
                //GraMa.DrawPS(mCharacterSpriteTexture, new Rectangle(mScout.Dimensions.X, mScout.Dimensions.Y, mPacSpriteTexture.Width / 15, mPacSpriteTexture.Height / 15));

                base.Draw(gameTime);
            }

            if (gameState == GameState.Won)
            {
                mSpriteBatch.Begin();
                mSpriteBatch.DrawString(mTitleFont, "You won!", new Vector2(50, titleSafe.Height / 3), Color.Black);
                mSpriteBatch.End();
            }
        }
    }
}
