using Project.Commons.Scripts.Extensions;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/GameData")]
public class GameData : ScriptableObject
{
    [ReadOnly]
    public int clearedStageNumber = 0;
}
