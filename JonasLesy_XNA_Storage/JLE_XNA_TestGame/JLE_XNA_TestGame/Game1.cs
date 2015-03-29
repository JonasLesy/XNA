// General includes.
using System;
using System.Collections.Generic;
using System.Linq;

// XNA includes.
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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
        // I use the following mGameState enum to check whether the game has started or not.
        public enum GameState
        {
            Menu,
            Game,
            Won,
            Lost
        }

        // Make a global variable of the mGameState enum.
        GameState mGameState;

        // Singleton object for handling XNA graphics routines.
        GraphicsManager GraMa = GraphicsManager.Instance;

        // Singleton object for handling XNA input routines.
        InputManager InpMa = InputManager.Instance;

        // Singleton object for handling XNA sound routines.
        SoundManager SouMa = SoundManager.Instance;

        // Sound effect to be played.
        cSound mShootEffect;
        cSound mWinEffect;
        cSound mLoseEffect;

        // Game music to be played.
        cSound mStartScreenSong;
        cSound mLevelSong;

        // Texture to draw the 2D images.
        Texture2D mCharacterSpriteTexture;
        Texture2D mBulletSpriteTexture;
        Texture2D mVolumeSpriteTexture;

        // The objects for the characters and projectiles.
        Character mScout;
        Character mBirdEnemy;
        Projectile mBullet;

        // A variable to keep the score.
        private int mPointsScored;

        // The spritebatch to draw.
        SpriteBatch mSpriteBatch;

        // The fonts for displaying text.
        private SpriteFont mTitleFont;
        private SpriteFont mNormalFont;

        // Save the safe dimensions for the screen.
        private Rectangle mTitleSafe;

        // Sprite and animator for the bird enemy.
        private Texture2D mBirdSpriteTexture;
        private SpriteAnimator mBirdAnimator;

        // Time interval to move the sprite.
        const double timestep = 0.2;
        double timetick = timestep;

        public Game1()
        {
            // Call the initialize function of the graphics, input and sound manager.
            GraMa.initialize(this);
            InpMa.initialize(this);
            SouMa.initialize(this);

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

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // (re)set the score to 0.
            mPointsScored = 0;

            // Set the mGameState to 'menu'
            mGameState = GameState.Menu;

            // Initialize the playable character, the enemy and the projectiles.
            mScout = new Character((GraMa.GetTitleSafeArea().Height - 100), GraMa.GetTitleSafeArea().Width / 2, 2, 2, 2);
            mBirdEnemy = new Character((GraMa.GetTitleSafeArea().Height - 90), GraMa.GetTitleSafeArea().Width, 2, 2, 2);
            mBullet = new Projectile();

            // Initialization of the images.
            mCharacterSpriteTexture = Content.Load<Texture2D>("Images/CharacterSprite");
            mBulletSpriteTexture = Content.Load<Texture2D>("Images/Bullet");
            mVolumeSpriteTexture = Content.Load<Texture2D>("Images/Volume5");

            // Load character texture and set its coordinates.
            mScout.setTexture(Content.Load<Texture2D>("Images/CharacterSprite"));
            mScout.setDirectionRight(true);
            mScout.setYCoordinate(mScout.getYCoord());
            mScout.setXCoordinate(mScout.getXCoord());

            // Load the bullet texture and set the Y coordinate, X coordinate is set later (during the game).
            mBullet.setTexture(Content.Load<Texture2D>("Images/Bullet"));
            mBullet.setYCoord(mScout.getYCoord());

            // Load the bird texture and set its coordinates.
            mBirdEnemy.setTexture(Content.Load<Texture2D>("Images/birdEnemyLeft"));
            mBirdEnemy.setYCoordinate(mBirdEnemy.getYCoord());
            mBirdEnemy.setXCoordinate(mBirdEnemy.getXCoord());

            // Create and initialize the sprite and its animator.
            mBirdAnimator = new SpriteAnimator();
            mBirdAnimator.InitializeSprite(3, 2, 60, 49, 2);
            mBirdSpriteTexture = Content.Load<Texture2D>("Images/birdEnemyLeft");

            // Here I load the spritebatch to display in it later.
            mSpriteBatch = GraMa.getSpriteBatch();

            // We load the fonts
            mTitleFont = Content.Load<SpriteFont>("TitleFont");
            mNormalFont = Content.Load<SpriteFont>("NormalFont");

            // Create game sounds and load the corresponding assets.
            mShootEffect = SouMa.mSoundFactory.Get(SoundType.SOUND_EFFECT);
            mShootEffect.LoadContent(this, "Audio/Gunshot");
            mWinEffect = SouMa.mSoundFactory.Get(SoundType.SOUND_EFFECT);
            mWinEffect.LoadContent(this, "Audio/WinApplause");
            mLoseEffect = SouMa.mSoundFactory.Get(SoundType.SOUND_EFFECT);
            mLoseEffect.LoadContent(this, "Audio/GameOver");

            // Create game songs and load the corresponding assets.
            mStartScreenSong = SouMa.mSoundFactory.Get(SoundType.SONG);
            mStartScreenSong.LoadContent(this, "Audio/StartscreenSong");
            mLevelSong = SouMa.mSoundFactory.Get(SoundType.SONG);
            mLevelSong.LoadContent(this, "Audio/LevelSong");
            // Set the same dimensions to those from the GraphicalManager.
            mTitleSafe = GraMa.GetTitleSafeArea();
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
            // Input the input manager (check old and new states).
            InpMa.Update();

            // The game can be shut down at any moment by pressing 'Escape'.
            if (InpMa.keyboardButtonPressed(Keys.Escape))
            {
                this.Exit();
            }

            // If the 'R' key is pressed at any time during the game, the game will reset (by reloading the content).
            if (InpMa.keyboardButtonPressed(Keys.R))
            {
                LoadContent();
            }

            if (InpMa.keyboardButtonPressed(Keys.G))
            {
                mStartScreenSong.changeVolume(0.1f);
                mLevelSong.changeVolume(0.1f);
            }

            if (InpMa.keyboardButtonPressed(Keys.H))
            {
                mStartScreenSong.changeVolume(-0.1f);
                mLevelSong.changeVolume(-0.1f);
            }

            // If the mGameState is 'Menu' then ..
            if (mGameState == GameState.Menu)
            {
                if (!mStartScreenSong.isPlaying())
                {
                    mStartScreenSong.play();
                }
                // Check if the Enter key is pressed, if so, the game will start (mGameState will be put to 'Game').
                if (InpMa.keyboardButtonPressed(Keys.Enter))
                {
                    mGameState = GameState.Game;
                    mStartScreenSong.stop();
                    mLevelSong.play();
                }
            }

            // If the mGameState is 'Game' then ..
            if (mGameState == GameState.Game)
            {
                if (!mLevelSong.isPlaying())
                {
                    mLevelSong.play();
                }

                // If the bird is on-screen then..
                if (mBirdEnemy.getVisible())
                {
                    // If the bird touches the player then..
                    if (Enumerable.Range(mBirdEnemy.getXCoord(), mBirdEnemy.getXCoord() + 47).Contains(mScout.getXCoord() + 47))
                    {
                        // GAME OVER, The mGameState is set to 'Lost'.
                        mGameState = GameState.Lost;
                        mLevelSong.stop();
                        // Play the Game Over sound effect.
                        mLoseEffect.play();
                    }

                    // If a bullet is fired then..
                    if (mBullet.getVisible())
                    {
                        // If the bullet touched the bird ...
                        if (Enumerable.Range(mBirdEnemy.getXCoord(), mBirdEnemy.getXCoord() + 47).Contains(mBullet.getXCoord()))
                        {
                            // The bird is set to visible as if it's been 'killed'.
                            mBirdEnemy.setVisible(false);
                            // Also the bullet disappears.
                            mBullet.setVisible(false);
                            // The scorecounter is increased.
                            mPointsScored++;

                            // If the player reaches a specific score then..
                            if (mPointsScored == 5 || mPointsScored == 10)
                            {
                                // The speed of the bird will be increased by 2.
                                mBirdEnemy.increaseSpeed(2);
                            }

                        }
                    }
                }

                // If the bird is not visible then..
                else
                {
                    // Initialize the bird again (reset-like actions are done).
                    mBirdEnemy = new Character((GraMa.GetTitleSafeArea().Height - 90), GraMa.GetTitleSafeArea().Width, 2, mBirdEnemy.getMoveLeft(), mBirdEnemy.getMoveRight());
                    mBirdEnemy.setXCoordinate(mBirdEnemy.getXCoord());
                    mBirdEnemy.setYCoordinate(mBirdEnemy.getYCoord());
                    mBirdEnemy.setTexture(Content.Load<Texture2D>("Images/birdEnemyLeft"));
                    mBirdEnemy.setVisible(true);
                }

                // Animate the sprite every timetick.
                if (gameTime.TotalGameTime.TotalSeconds > timetick)
                {
                    // Set the next tick.
                    timetick += timestep;

                    //Animate the sprite
                    mBirdAnimator.AnimateSprite();

                    //Update the game.
                    base.Update(gameTime);
                }

                // Move the bird to the right with it's MOVE_LEFT parameter.
                mBirdEnemy.decreaseXCoordinate(mBirdEnemy.getMoveLeft());

                // If the player pushed down the 'Right' key on the keyboard then..
                if (InpMa.keyboardButtonDown(Keys.Right) || InpMa.gamePadDPadPressed(GamePad.GetState(PlayerIndex.One).DPad.Right) || InpMa.mouseScrollWheelValue() > 0)
                {
                    // If the character hasn't reached the right border of the screen.
                    if (mScout.getXCoord() < (mTitleSafe.Width - mScout.ObjectTexture.Width / 2))
                    {
                        // Move the character with its MOVE_RIGHT variable.
                        mScout.increaseXCoordinate(mScout.getMoveRight());
                        // Flip the character sprite so the character is facing right.
                        mScout.setXSpriteCoord(0);
                        // Flip the bullet sprite so it is facing right.
                        mBullet.setXSpriteCoord(0);
                    }
                }

                // If the player pushed down the 'Left' key on the keyboard then..
                if (InpMa.keyboardButtonDown(Keys.Left) || InpMa.gamePadDPadPressed(GamePad.GetState(PlayerIndex.One).DPad.Left) || InpMa.mouseScrollWheelValue() < 0)
                {
                    // If the character hasn't reached the left border of the screen.
                    if (mScout.getXCoord() > 0)
                    {
                        // Move the character with its MOVE_LEFT variable.
                        mScout.decreaseXCoordinate(mScout.getMoveLeft());
                        // Flip the character sprite so the character is facing left.
                        mScout.setXSpriteCoord(80);
                        // Flip the bullet sprite so it is facing left.
                        mBullet.setXSpriteCoord(35);
                    }
                }

                // The next two checks are commented out because I want to implement some kind of jumping mechanism.
                // This will probably be done during the next module!
                //if (InpMa.keyboardButtonDown(Keys.Up))
                //{
                //    if (mScout.getYCoord() > 0)
                //    {
                //        mScout.increaseYCoordinate(mScout.getMoveUp());
                //    }
                //}

                //if (InpMa.keyboardButtonDown(Keys.Down))
                //{
                //    if (mScout.getYCoord() < (mTitleSafe.Height - mScout.ObjectTexture.Height))
                //    {
                          // For now this uses a pre-defined value (2) because it'll have to be changed into some sort of gravity system.
                          // I'll change this later.
                //        mScout.decreaseYCoordinate(2);
                //    }
                //}

                // If the player presses the 'Space' key on his keyboard or left-clicks with his mouse then..
                if (InpMa.keyboardButtonPressed(Keys.Space) || InpMa.mouseButtonPressed(Mouse.GetState().LeftButton) || InpMa.gamePadButtonPressed(GamePad.GetState(PlayerIndex.One).Buttons.A))
                {
                    // If the bullet is not yet visible then.. (this makes only one bullet possible at a time!)
                    if (!mBullet.getVisible())
                    {
                        mShootEffect.play();
                        // Make the bullet visible.
                        mBullet.setVisible(true);
                        // If the character is facing right then..
                        if (mScout.getXSpriteCoord() == 0)
                        {
                            // Set the bullet's direction to right.
                            mBullet.setDirection(true);
                            // Set the bullet's position to the right place.
                            mBullet.setXCoord(mScout.Dimensions.X + 80);
                            mBullet.setYCoord(mScout.Dimensions.Y + 14);
                        }
                        // Otherwise, the character will be facing left, then..
                        else
                        {
                            // Set the bullet's direction to left.
                            mBullet.setDirection(false);
                            // Set the bullet's position to the right place.
                            mBullet.setXCoord(mScout.Dimensions.X - 30);
                            mBullet.setYCoord(mScout.Dimensions.Y + 14);
                        }
                    }
                }

                // Check if the player has score 15 points, then..
                if (mPointsScored == 15)
                {
                    // GAME WON, the mGameState is changed to 'Won'.
                    mGameState = GameState.Won;
                    mLevelSong.stop();
                    // Play the applause sound effect.
                    mWinEffect.play();
                }

            }

            // If the game is in the 'Lost' or 'Won' state then..
            if (mGameState == GameState.Lost || mGameState == GameState.Won)
            {
                // Check if the left-mousebutton is clicked, then..
                if (InpMa.mouseButtonPressed(Mouse.GetState().LeftButton))
                {
                    // 'Reload' the game by loading the content.
                    LoadContent();
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

            // I round the volume (float) to a decimal with only one digit after the comma. This is done to avoid 0.599999999...
            String decimalValue = (Math.Round((decimal)mStartScreenSong.getVolume(),1)).ToString();

            // Now I check which volume image is needed to be shown depending on the volume settings.
            String volumeURL;
            switch (decimalValue)
            {
                case "0":
                    volumeURL = "Images/Volume0";
                    break;
                case "0,2":
                    volumeURL = "Images/Volume1";
                    break;
                case "0,4":
                    volumeURL = "Images/Volume2";
                    break;
                case "0,6":
                    volumeURL = "Images/Volume3";
                    break;
                case "0,8":
                    volumeURL = "Images/Volume4";
                    break;
                case "1":
                default:
                    volumeURL = "Images/Volume5";
                    break;
            }
            // Set the right image url for the volume texture.
            mVolumeSpriteTexture = Content.Load<Texture2D>(volumeURL);

            // Draw the volume image on the screen
            mSpriteBatch.Begin();
            mSpriteBatch.Draw(mVolumeSpriteTexture, new Vector2(5, 5), Color.RoyalBlue);
            mSpriteBatch.End();

            // If the player is in the 'Menu' mGameState then..
            if (mGameState == GameState.Menu)
            {
                // Display the Menuscreen.
                mSpriteBatch.Begin();
                mSpriteBatch.DrawString(mTitleFont, "XNA SHOOTER", new Vector2(50, mTitleSafe.Height / 4), Color.Black);
                mSpriteBatch.DrawString(mNormalFont, "Press ENTER to start playing!", new Vector2(mTitleSafe.Width / 2 - 200, mTitleSafe.Height / 3 + 65), Color.White);
                mSpriteBatch.DrawString(mNormalFont, "Game controls:\n---------------\nLeft/Right: according keyboard buttons\nShoot: Space or Left-click\nVolume Up: G\nVolume Down: H\nReset: R-key\nQuit:  Escape-key", new Vector2(5, mTitleSafe.Height - 200), Color.Black);
                mSpriteBatch.DrawString(mNormalFont, "By Jonas Lesy", new Vector2(mTitleSafe.Width - 150, mTitleSafe.Height - 30), Color.Black);
                mSpriteBatch.End();
            }

            // If the player is in the 'Game' mGameState then..
            if (mGameState == GameState.Game)
            {
                // Start the SpriteBatch
                mSpriteBatch.Begin();

                // Display the score in the top-left corner of the screen.
                mSpriteBatch.DrawString(mNormalFont, "Score: " + mPointsScored, new Vector2(10, 0), Color.Black);

                // If the bird is visible then..
                if (mBirdEnemy.getVisible())
                {
                    // Draw the correct part of the birdsprite and place it in the appropriate position.
                    mSpriteBatch.Draw(mBirdEnemy.ObjectTexture, new Rectangle(mBirdEnemy.getXCoord(), mBirdEnemy.getYCoord(), 60, 49), new Rectangle(mBirdAnimator.getXCoord(), mBirdAnimator.getYCoord(), 60, 49), Color.White);
                }

                // Draw the correct part of the scoutsprite (facing left or right) and place it in the appropriate position.
                mSpriteBatch.Draw(mScout.ObjectTexture, new Rectangle(mScout.getXCoord(), mScout.getYCoord(), 80, 102), new Rectangle(mScout.getXSpriteCoord(), 0, 80, 102), Color.White);
                
                // If the bullet is visible then..
                if (mBullet.getVisible())
                {
                    // Draw the correct part of the bulletsprite (facing left or right) and place it in the appropriate position.
                    mSpriteBatch.Draw(mBullet.ObjectTexture, new Rectangle(mBullet.Dimensions.X, mBullet.Dimensions.Y, 35, 12), new Rectangle(mBullet.getXSpriteCoord(), 0, 35, 12), Color.White);
                    
                    // If the bullet is facing right then..
                    if (mBullet.getDirection())
                    {
                        // Increase the coordinate of the bullet.
                        mBullet.increaseXCoordinate(5);
                    }

                    // Otherwise..
                    else
                    {
                        // Decrease the coordinate of the bullet by 5.
                        mBullet.decreaseXCoordinate(5);
                    }
                }

                // If the bullet reaches one of the corners of the screen, then..
                if (mBullet.Dimensions.X < -35 || mBullet.Dimensions.X > GraMa.GetTitleSafeArea().Width)
                {
                    // Make the bullet disappear.
                    mBullet.setVisible(false);
                }

                mSpriteBatch.End();
                base.Draw(gameTime);
            }

            // If the player is in the 'Won' mGameState then..
            if (mGameState == GameState.Won)
            {
                // Display the 'You won' screen.
                mSpriteBatch.Begin();
                mSpriteBatch.DrawString(mTitleFont, "You won!", new Vector2(50, mTitleSafe.Height / 3), Color.Black);
                mSpriteBatch.DrawString(mNormalFont, "Press 'R' or left-click to restart, press 'Escape' to quit", new Vector2(5, mTitleSafe.Height - 30), Color.White);
                mSpriteBatch.End();
            }

            // If the player is in the 'Lost' mGameState then..
            if (mGameState == GameState.Lost)
            {
                // Display the 'You lost' screen with the score.
                mSpriteBatch.Begin();
                mSpriteBatch.DrawString(mTitleFont, "You lost!", new Vector2(50, mTitleSafe.Height / 3), Color.Black);
                mSpriteBatch.DrawString(mNormalFont, "Your score was: " + mPointsScored, new Vector2(mTitleSafe.Width / 4, mTitleSafe.Height - 200), Color.White);
                mSpriteBatch.DrawString(mNormalFont, "Press 'R' or left-click to restart, press 'Escape' to quit", new Vector2(5, mTitleSafe.Height - 30), Color.White);
                mSpriteBatch.End();
            }
        }
    }
}
