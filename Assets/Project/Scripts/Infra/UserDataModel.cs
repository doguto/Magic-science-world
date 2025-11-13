using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Project.Scripts.Infra
{
    public class UserDataModel
    {
        public UserData UserData { get; private set; }

        const string DataAddress = "Assets/Project/DataStore/UserData.json";

        string saveDirectoryPath;
        string saveFilePath;


        public UserDataModel()
        {
            saveDirectoryPath = Path.Combine(Application.persistentDataPath, "DataStore");
            saveFilePath = Path.Combine(saveDirectoryPath, "saveData.json");
            if (!File.Exists(saveFilePath))
            {
                UserData = new UserData();
            }
            else
            {
                try
                {
                    string json = File.ReadAllText(saveFilePath);
                    UserData data = JsonUtility.FromJson<UserData>(json);
                    UserData = data;
                }
                catch (System.Exception e)
                {
                    Debug.LogError("Failed to load user data: " + e.Message);
                    UserData = new UserData();
                }
            }
        }
        
        public void ProgressStage()
        {
            UserData.clearedStageNumber++;
        }

        public int GetClearedStageNumber()
        {
            return UserData.clearedStageNumber;
        }

        public bool IsClearedStage(int stageNumber)
        {
            return UserData.clearedStageNumber >= stageNumber;
        }

        public bool IsOpenedStage(int stageNumber)
        {
            return UserData.clearedStageNumber >= stageNumber - 1;
        }

        
        public UserData Load()
        {
            if (!File.Exists(saveFilePath))
            {
                return new UserData();
            }
          
            try 
            { 
                string json = File.ReadAllText(saveFilePath);
                UserData data = JsonUtility.FromJson<UserData>(json);
                return data;
            }
            catch (System.Exception e)
            {
                 Debug.LogError("Failed to load user data: " + e.Message);
                 return new UserData();
            }
        }

        public void Save()
        {
            try
            {
                if (!Directory.Exists(saveDirectoryPath))
                {
                    Directory.CreateDirectory(saveDirectoryPath);
                }

                string json = JsonUtility.ToJson(UserData, true);
                File.WriteAllText(saveFilePath, json);
            }
            catch (System.Exception e)
            {
                Debug.LogError("Failed to save user data: " + e.Message);
            }
        }
    }

    public class UserData
    {
        public int clearedStageNumber;
        public Dictionary<string, string> keyConfigMaps;
        
        public UserData()
        {
            clearedStageNumber = 0;
            keyConfigMaps = new ()
            {
                {"Attack","Enter"},
                {"Charge", "Space"},
                {"MoveLeft","AllowLeft"},
                {"MoveRight","AllowRight"},
                {"MoveUp","AllowUp"},
                {"MoveDown","AllowDown"},
            };
        }
    }
    
}
