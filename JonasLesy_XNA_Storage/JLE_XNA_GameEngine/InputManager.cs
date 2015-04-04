using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace JLE_XNA_GameEngine
{
    /// <summary>
    /// This class deals with user input. It is created as a singleton, because only
    /// one instance of the input manager should be user per game.
    /// </summary>
    public sealed class InputManager
    {
        // Reference to the game which will use this input manager.
        Game mGame;

        // This variable is used to save the old keyboard state (for comparisons).
        KeyboardState mKOldState;

        // This variable saves the new state of the keyboard.
        KeyboardState lNewState;

        /// <summary>
        /// Instance for the input manager.
        /// Padlock for making the implementation thread safe.
        /// </summary>
        static InputManager mInstance = null;
        static readonly object mPadlock = new object();

        
        /// <summary>
        /// Empty constructor.
        /// </summary>
        InputManager()
        {
        }

        /// <summary>
        /// Instance for the input manager and the accessor to get it.
        /// </summary>
        public static InputManager Instance
        {
            get
            {
                lock (mPadlock)
                {
                    if (mInstance == null)
                    {
                        mInstance = new InputManager();
                    }
                    return mInstance;
                }
            }
        }

        /// <summary>
        /// Initializing the input manager.
        /// </summary>
        /// <param name="pGame"> The game will be using the input manager</param>
        public void initialize(Game pGame)
        {
            // store reference to the game that uses the manager.
            mGame = pGame;

            // Get the initial state of the keyboard.
            mKOldState = Keyboard.GetState();

            // Set the mouse to visible.
            mGame.IsMouseVisible = true;
        }

        /// <summary>
        /// Update the keyboard state.
        /// </summary>
        public void Update()
        {
            // Set the old state to the new state.
            mKOldState = lNewState;
        }

        /// <summary>
        /// Check if a button on the Gamepad is pressed.
        /// </summary>
        /// <param name="pButtonState">The state of the game pad buttons.</param>
        /// <returns>True if button in question is pressed, false in the other case</returns>
        public bool gamePadButtonPressed(ButtonState pButtonState)
        {
            if (pButtonState == ButtonState.Pressed)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Check if one of the Gamepad's DPad buttons has been pressed.
        /// </summary>
        /// <param name="pButtonState">State of the Gamepad's DPad buttons.</param>
        /// <returns>True if the button in question is been pressed, false in the other case</returns>
        public bool gamePadDPadPressed(ButtonState pButtonState)
        {
            if (pButtonState == ButtonState.Pressed)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Find out the position of the Gamepad's left stick.
        /// </summary>
        /// <param name="pPadState">State of the Gamepad.</param>
        /// <returns>Position of the Gamepad's left stick.</returns>
        public Vector2 gamePadLTStickPosition(GamePadState pPadState)
        {
            Vector2 lTmpPoint;

            lTmpPoint.X = pPadState.ThumbSticks.Left.X;
            lTmpPoint.Y = pPadState.ThumbSticks.Left.Y;

            return lTmpPoint;
        }

        /// <summary>
        /// Find out the position of the Gamepad's right stick.
        /// </summary>
        /// <param name="pPadState">State of the Gamepad.</param>
        /// <returns>Position of the Gamepad's right stick.</returns>
        public Vector2 gamePadRTStickPosition(GamePadState pPadState)
        {
            Vector2 rTmpPoint;

            rTmpPoint.X = pPadState.ThumbSticks.Right.X;
            rTmpPoint.Y = pPadState.ThumbSticks.Right.Y;

            return rTmpPoint;
        }

        /// <summary>
        /// Find out the position of the Gamepad's left trigger.
        /// </summary>
        /// <param name="pPadState">State of the Gamepad.</param>
        /// <returns>Position of the Gamepad's left trigger.</returns>
        public float gamePadLTriggerPosition(GamePadState pPadState)
        {
            float lTmpValue;
            lTmpValue = pPadState.Triggers.Left;
            return lTmpValue;
        }

        /// <summary>
        /// Find out the position of the Gamepad's right trigger.
        /// </summary>
        /// <param name="pPadState">State of the Gamepad.</param>
        /// <returns>Position of the Gamepad's right trigger.</returns>
        public float gamePadRTriggerPosition(GamePadState pPadState)
        {
            float rTmpValue;
            rTmpValue = pPadState.Triggers.Right;
            return rTmpValue;
        }

        /// <summary>
        /// Find out whether a key has been pressed.
        /// </summary>
        /// <param name="pKey">The key in question.</param>
        /// <returns>True if the key in question is pressed, false in the other case.</returns>
        public bool keyboardButtonPressed(Keys pKey)
        {
            lNewState = Keyboard.GetState();

            // If the key is down now.
            if (lNewState.IsKeyDown(pKey))
            {
                // And it was not pressed down before.
                if (!mKOldState.IsKeyDown(pKey))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Find out whether a key is down.
        /// </summary>
        /// <param name="pKey">The key in question.</param>
        /// <returns>True if the key in question is down, false otherwise.</returns>
        public bool keyboardButtonDown(Keys pKey)
        {
            lNewState = Keyboard.GetState();

            // If the key is down now.
            if (lNewState.IsKeyDown(pKey))
            {
                // And the key was down earlier.
                if (mKOldState.IsKeyDown(pKey))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Find out whether a key is being released.
        /// </summary>
        /// <param name="pKey">The key in question.</param>
        /// <returns>True if the key in question has been released, false in the other case.</returns>
        public bool keyboardButtonReleased(Keys pKey)
        {
            lNewState = Keyboard.GetState();

            if (!lNewState.IsKeyDown(pKey))
            {
                if (mKOldState.IsKeyDown(pKey))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Checks if the given key is a letter or not.
        /// </summary>
        /// <param name="key">The key needed to be checked.</param>
        /// <returns>Boolean, yes if the key is a character of the alfabet, no if not.</returns>
        public bool IsKeyAChar(Keys key)
        {
            return key >= Keys.A && key <= Keys.Z;
        }

        /// <summary>
        /// Check whether a mouse button has been pressed.
        /// </summary>
        /// <param name="pButtonState">State of the mouse button in question.</param>
        /// <returns>True if button in question is pressed, false in the other case.</returns>
        public bool mouseButtonPressed(ButtonState pButtonState)
        {
            if (pButtonState == ButtonState.Pressed)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Get the position of mouse's scroll wheel.
        /// </summary>
        /// <returns>Integer which tells the wheel's position.</returns>
        public int mouseScrollWheelValue()
        {
            MouseState lTmpState = Mouse.GetState();

            return lTmpState.ScrollWheelValue;
        }

        /// <summary>
        /// Return the mouse's absolute coordinates.
        /// </summary>
        /// <returns>Point that indicates mouse's current absolute coordinates.</returns>
        public Point mouseAbsoluteCoords()
        {
            MouseState lTmpState = Mouse.GetState();

            Point lTmpPoint;

            lTmpPoint.X = lTmpState.X;
            lTmpPoint.Y = lTmpState.Y;

            return lTmpPoint;
        }

    }
}
