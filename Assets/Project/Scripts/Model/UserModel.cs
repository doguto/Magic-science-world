using System;
using System.Collections.Generic;
using System.IO;
using Project.Scripts.Model;
using UnityEngine;

namespace Project.Scripts.Model
{
    public class UserModel : ModelBase
    {
        UserData UserData { get; set; }
        public static UserModel Instance => new();
        
        public int ClearedStageNumber => UserData.clearedStageNumber;

        readonly string saveDirectoryPath;
        readonly string saveFilePath;


        public UserModel()
        {
            saveDirectoryPath = Path.Combine(Application.persistentDataPath, "DataStore");
            saveFilePath = Path.Combine(saveDirectoryPath, "UserData.json");
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
        
        public void StageClear(int  stageNumber)
        {
            UserData.clearedStageNumber = Math.Max(UserData.clearedStageNumber, stageNumber);
            Save();
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
