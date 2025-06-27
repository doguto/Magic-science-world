using UnityEngine;
using System.Collections.Generic;

namespace Project.Scenes.Scenario.Scripts.Model
{
    [CreateAssetMenu(
        fileName = "ScenarioData",
        menuName = "Scenario/ScenarioData",
        order = 1)]
    public class ScenarioDataSO : ScriptableObject
    {
        public ScenarioLine[] scenarioLines;
        
        // キャラクター名の日本語から英語への変換辞書
        public static readonly Dictionary<string, string> CharacterJaNameToKey = new Dictionary<string, string>
        {
            { "テン", "Tenn" },
            { "ロコ", "Loco" },
            { "アズマ", "Azuma" },
            { "タツミ", "Tatsumi" },
            { "ウシトラ", "Ushitora" },
            { "スイ", "Sui" },
            { "ハナレ", "Hanare" },
            { "コン", "Kon" },
        };
    }

    [System.Serializable]
    public class ScenarioLine
    {
        public string character;
        [TextArea(3, 10)]
        public string content;
        public FaceType faceType;
    }

    // ヘルパー型 顔の種類
    public enum FaceType
    {
        // 共通寄り
        Default,
        Smile,
        Confused,
        Embarrassed,
        Sad,
        Warn,
        Confident,

        // テン
        Speak,
        Shout,

        // コン
        SecondDefault,
        
        // 狂乱時
        CrazyDefault,
        CrazyConfident,
    }


}
