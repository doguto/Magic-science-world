using System;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Scripts.Infra
{
    [CreateAssetMenu(fileName = "StageData", menuName = "Database/StageData")]
    public class StageDataObject : ScriptableObject
    {
        public List<StageData> stageData;
    }

    [Serializable]
    public class StageData
    {
        public int id;
        public string charaStillAddress;
    }
}
