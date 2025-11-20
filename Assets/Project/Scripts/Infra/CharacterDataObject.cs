using System;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Scripts.Infra;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Database/CharacterData")]
public class CharacterDataObject : ScriptableObject
{
    public List<CharacterData> characterData;
}

[Serializable]
public class CharacterData
{
    public int id;
    public string name;
    public string englishName;
}
