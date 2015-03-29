// Using directives.
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Storage;

// Namespace of the application
namespace JLE_XNA_GameEngine
{
    /// <summary>
    /// Struct that holds the save game data.
    /// </summary>
    [Serializable]
    public struct SaveGameData
    {
        public string PlayerName;
        public Vector2 AvatarPosition;
        public int Level;
        public int Score;
    }

    /// <summary>
    /// Class for handling file IO for the game. Created as a singleton, since only one
    /// instance of the file manager should be used per game.
    /// </summary>
    public sealed class FileManager
    {
        // The device used to store the game data.
        StorageDevice mStorageDevice;

        // Instance for the file manager and a padlock for making the implementation thread safe.
        static FileManager mInstance = null;
        static readonly object mPadlock = new object();

        /// <summary>
        /// An empty constructor
        /// </summary>
        FileManager()
        {
        }

        /// <summary>
        /// Instance for the file manager and the getter for acquiring it.
        /// </summary>
        public static FileManager Instance
        {
            get
            {
                lock (mPadlock)
                {
                    if (mInstance == null)
                    {
                        mInstance = new FileManager();
                    }
                    return mInstance;
                }
            }
        }

        ///// <summary>
        ///// This method synchronously opens a storage container
        ///// </summary>
        ///// <param name="storageDevice">The requested storage device.</param>
        ///// <param name="saveGameName">The name of the save.</param>
        ///// <returns></returns>
        //private static StorageContainer OpenContainer(StorageDevice storageDevice, string saveGameName)
        //{
        //    // Opens the requested storage container (folder)
        //    IAsyncResult result = storageDevice.BeginOpenContainer(saveGameName, null, null);

        //    // Wait for the WaitHandle to become signaled.
        //    result.AsyncWaitHandle.WaitOne();

        //    // The opening of the container is ended.
        //    StorageContainer container = storageDevice.EndOpenContainer(result);

        //    // Close the wait handle.
        //    result.AsyncWaitHandle.Close();

        //    // Return the retrieved container
        //    return container;
        //}

        /// <summary>
        /// Function for initializing the file manager.
        /// </summary>
        public void Initialize()
        {
            // Make a new IAsyncResult.
            IAsyncResult lResult = null;

            // Begin the selector.
            lResult = StorageDevice.BeginShowSelector(PlayerIndex.One, null, null);
            
            // End the selector.
            mStorageDevice = StorageDevice.EndShowSelector(lResult);
        }

        /// <summary>
        /// Function that saved the game data to a file.
        /// </summary>
        /// <param name="pData">The data to save</param>
        public void saveGame(SaveGameData pData, string saveGameName)
        {
            // Open a storage container.
            IAsyncResult lResult = mStorageDevice.BeginOpenContainer("JLE_XNA_TestGame", null, null);

            // Wait for the WaitHandle to become signaled.
            lResult.AsyncWaitHandle.WaitOne();

            // The opening of the container is ended.
            StorageContainer lContainer = mStorageDevice.EndOpenContainer(lResult);

            // Close the wait handle.
            lResult.AsyncWaitHandle.Close();

            string lFilename = saveGameName;

            // Check if the save already exists.
            if (lContainer.FileExists(lFilename))
            {
                // Delete the existing file.
                lContainer.DeleteFile(lFilename);
            }

            // Create the new save file.
            Stream lStream = lContainer.CreateFile(lFilename);

            // Convert the save data into XML data and send it to the file lStream.
            XmlSerializer lSerializer = new XmlSerializer(typeof(SaveGameData));
            lSerializer.Serialize(lStream, pData);

            // Close the file.
            lStream.Close();

            // Dispose the storage container. This commits the changes.
            lContainer.Dispose();
        }

        /// <summary>
        /// Function which reads the game data.
        /// </summary>
        /// <returns>Game data that was loaded.</returns>
        public SaveGameData readGame(string saveGameName)
        {
            // Create and initialize object into which the data is loaded.
            SaveGameData lData;
            lData.PlayerName = "";
            lData.Level = 0;
            lData.Score = 0;
            lData.AvatarPosition.X = 0;
            lData.AvatarPosition.Y = 0;

            IAsyncResult lResult = mStorageDevice.BeginOpenContainer("JLE_XNA_TestGame", null, null);

            // Wait for the WaitHandle to become signaled.
            lResult.AsyncWaitHandle.WaitOne();

            // The opening of the container is ended.
            StorageContainer lContainer = mStorageDevice.EndOpenContainer(lResult);

            // Close the wait handle.
            lResult.AsyncWaitHandle.Close();

            string lFilename = saveGameName;

            // Open the save file.
            Stream lStream = lContainer.OpenFile(lFilename, FileMode.Open);

            // Convert the save data into XML data and send it to the file lStream.
            XmlSerializer lSerializer = new XmlSerializer(typeof(SaveGameData));
            lData = (SaveGameData)lSerializer.Deserialize(lStream);

            // Close the file.
            lStream.Close();

            // Dispose the storage container. This commits the changes.
            lContainer.Dispose();

            return lData;
        }
    }
}
