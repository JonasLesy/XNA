// Using directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

// Namespace of the application
namespace JLE_XNA_GameEngine
{
    public enum SoundType{ SONG, SOUND_EFFECT};

    // SoundFactory used to create new sound effects or sounds.
    public class cSoundFactory
    {
        public cSound Get(SoundType pType)
        {
            switch (pType)
            {
                case SoundType.SONG:
                    return new cSong();
                case SoundType.SOUND_EFFECT:
                default:
                    return new cSoundEffect();
            }
        }
    }

    public abstract class cSound
    {
        public abstract void LoadContent(Game pGame, String pAssetName);
        public abstract void play();
        public abstract void stop();
        public abstract void pause();
        public abstract void resume();
        public abstract bool isPlaying();
        public abstract void changeVolume(float pChange);
        public abstract float getVolume();
    }

    public class cSoundEffect : cSound
    {
        /// <summary>
        /// Sound effect instance used for playing the sound effect.
        /// </summary>
        SoundEffectInstance mSoundEffectInstance;
        public SoundEffect mSoundEffect;

        public override void LoadContent(Game pGame, string pAssetName)
        {
            // Load the sound effect.
            mSoundEffect = pGame.Content.Load<SoundEffect>(pAssetName);
            // Create and instance and store it.
            mSoundEffectInstance = mSoundEffect.CreateInstance();
        }

        /// <summary>
        /// Function which plays the sound effect.
        /// </summary>
        public override void play()
        {
            mSoundEffectInstance.Play();
        }

        /// <summary>
        /// Function which stops the sound effect.
        /// </summary>
        public override void stop()
        {
            mSoundEffectInstance.Stop();
        }

        /// <summary>
        /// Function which pauses the sound effect.
        /// </summary>
        public override void pause()
        {
            mSoundEffectInstance.Pause();
        }

        /// <summary>
        /// Function which resumes the sound effect.
        /// </summary>
        public override void resume()
        {
            mSoundEffectInstance.Resume();
        }

        /// <summary>
        /// Function which checks if the sound effect is playing.
        /// </summary>
        public override bool isPlaying()
        {
            if(mSoundEffectInstance.State == SoundState.Playing)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Function which changes the volume of the sound effect.
        /// </summary>
        /// <param name="pChange"></param>
        public override void changeVolume(float pChange)
        {
            // To avoid runtime errors and exceptions, we have to check if the change is withing the appropriate boundaries.
            if ((mSoundEffectInstance.Volume + pChange > 0.0f) && (mSoundEffectInstance.Volume + pChange < 1.0f))
                mSoundEffectInstance.Volume += pChange;
        }

        /// <summary>
        /// Return the value of the volume.
        /// </summary>
        /// <returns>The volume as float type</returns>
        public override float getVolume()
        {
            return Microsoft.Xna.Framework.Media.MediaPlayer.Volume;
        }
    }

    public class cSong : cSound
    {
        // Member variable which Will be used to store the music clips
        public Microsoft.Xna.Framework.Media.Song mMusicClip;

        public override void LoadContent(Game pGame, String pAssetName)
        {
            mMusicClip = pGame.Content.Load<Microsoft.Xna.Framework.Media.Song>(pAssetName);
        }

        /// <summary>
        /// Function which plays the song.
        /// </summary>
        public override void play()
        {
            Microsoft.Xna.Framework.Media.MediaPlayer.Play(mMusicClip);
        }

        /// <summary>
        /// Function which stops the song.
        /// </summary>
        public override void stop()
        {
            Microsoft.Xna.Framework.Media.MediaPlayer.Stop();
        }

        /// <summary>
        /// Function which pauses the song.
        /// </summary>
        public override void pause()
        {
            Microsoft.Xna.Framework.Media.MediaPlayer.Pause();
        }

        /// <summary>
        /// Function which resumes the song.
        /// </summary>
        public override void resume()
        {
            Microsoft.Xna.Framework.Media.MediaPlayer.Resume();
        }

        /// <summary>
        /// Function which checks if the song is playing.
        /// </summary>
        public override bool isPlaying()
        {
            if (Microsoft.Xna.Framework.Media.MediaPlayer.State == Microsoft.Xna.Framework.Media.MediaState.Playing)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Function which changes the volume of the song.
        /// </summary>
        /// <param name="pChange">The number to increase or decrease the volume with</param>
        public override void changeVolume(float pChange)
        {
            // MediaPlayer checks invalid values itself, no check is needed.
            Microsoft.Xna.Framework.Media.MediaPlayer.Volume += pChange;
        }

        /// <summary>
        /// Return the value of the volume.
        /// </summary>
        /// <returns>The volume as float type</returns>
        public override float getVolume()
        {
            return Microsoft.Xna.Framework.Media.MediaPlayer.Volume;
        }
    }

    /// <summary>
    /// This class deals with sound effects and music. It is created as a singleton, because only
    /// one instance of the sound manager should be user per game.
    /// </summary>
    public sealed class SoundManager
    {
        /// <summary>
        /// Instance for the sound manager.
        /// Padlock for making the implementation thread safe.
        /// </summary>
        static SoundManager mInstance = null;
        static readonly object mPadlock = new object();

        // Reference to the game which will use the sound manager.
        Game mGame;

        // Reference the used sound factory.
        public cSoundFactory mSoundFactory;

        /// <summary>
        /// Empty constructor.
        /// </summary>
        SoundManager()
        {
        }

        /// <summary>
        /// Instance for the sound manager and the accessor to get it.
        /// </summary>
        public static SoundManager Instance
        {
            get
            {
                lock (mPadlock)
                {
                    if (mInstance == null)
                    {
                        mInstance = new SoundManager();
                    }
                    return mInstance;
                }
            }
        }

        /// <summary>
        /// Initializing the sound manager.
        /// </summary>
        /// <param name="pGame"> The game will be using the sound manager</param>
        public void initialize(Game pGame)
        {
            // store reference to the game that uses the manager.
            mGame = pGame;

            // Create a sound factory for creating the sounds.
            mSoundFactory = new cSoundFactory();
        }
    }
}