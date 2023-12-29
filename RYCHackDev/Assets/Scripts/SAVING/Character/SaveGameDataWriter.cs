using System;
using System.IO;
using UnityEngine;

namespace Deceilio.Psychain
{
    public class SaveGameDataWriter 
    {
        public string saveDataDirectoryPath = ""; // Save data file location
        public string dataSaveFileName = ""; // Save data file name

        // LOGIC: Before we save the file, we must check to see if the character slot already exists (Max 10 character slots)
        public bool CheckToSeeIfFileExists()
        {
            if (File.Exists(Path.Combine(saveDataDirectoryPath, dataSaveFileName)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public CharacterSaveData LoadCharacterDataFromJson()
        {
            CharacterSaveData loadedSaveData = null;
            string savePath = Path.Combine(saveDataDirectoryPath, dataSaveFileName);

            if(File.Exists(savePath))
            {
                try
                {
                    string saveDataToLoad = "";

                    using (FileStream stream =  new FileStream(savePath, FileMode.Open))
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            saveDataToLoad = reader.ReadToEnd();
                        }
                    }

                    // BELOW CODE: Deserialize the data
                    loadedSaveData = JsonUtility.FromJson<CharacterSaveData>(saveDataToLoad);
                }
                catch(Exception ex) 
                {
                    Debug.LogWarning(ex.Message);
                }
            }
            else
            {
                Debug.Log("SAVE FILE DOES NOT EXIST" % WRLD_COLORIZE_EDITOR.DarkRed % WRLD_FONT_FORMAT.Bold);
            }
            return loadedSaveData;
        }
        public void WriteCharacterDataToSaveFile(CharacterSaveData characterData)
        {
            // BELOW CODE: Creates a path to save our file
            string savePath = Path.Combine(saveDataDirectoryPath, dataSaveFileName);

            try
            {
                // BELOW CODE: Create a file dictionary
                Directory.CreateDirectory(Path.GetDirectoryName(savePath));
                //Debug.Log("SAVE PATH = " % WRLD_COLORIZE_EDITOR.DarkGreen % WRLD_FONT_FORMAT.Bold + savePath);

                // BELOW CODE: Serialise the C# game data to json
                string dataToStore = JsonUtility.ToJson(characterData, true);

                // BELOW CODE: Write the file to our system
                using (FileStream stream = new FileStream(savePath, FileMode.Create))
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        writer.Write(dataToStore);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("ERROR WHILE TRYING TO SAVE DATA, GAME COULD NOT BE SAVED!" % WRLD_COLORIZE_EDITOR.DarkRed % WRLD_FONT_FORMAT.Bold + ex);
            }
        }
        public void DeleteSaveFile()
        {
            File.Delete(Path.Combine(saveDataDirectoryPath, dataSaveFileName));
        }
        public bool CheckIfSaveFileExists()
        {
            if(File.Exists(Path.Combine(saveDataDirectoryPath, dataSaveFileName)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}