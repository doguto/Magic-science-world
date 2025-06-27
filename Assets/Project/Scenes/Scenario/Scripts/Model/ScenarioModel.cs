using System.Linq;
using Project.Commons.Scripts.Constants;

namespace Project.Scenes.Scenario.Scripts.Model
{
    public class ScenarioModel
    {
        private ScenarioLine[] _lines;
        private int _index = -1;

        public ScenarioModel(ScenarioLine[] lines)
        {
            _lines = lines;
        }

        public bool HasNext => _index + 1 < _lines.Length;
        public bool HasPrevious => _index > 0;

        public ScenarioLine Next()
        {
            _index++;
            return _lines[_index];
        }
        public ScenarioLine UndoDev()
        {
            if (_index <= 0)
            {
                return _lines[0];
            }
            _index--;
            return _lines[_index];
        }
        public ScenarioLine GetCurrentLine()
        {
            return _lines[_index];
        }

        public string GetSpeakingCharacterKey()
        {
            return ScenarioDataSO.CharacterJaNameToKey[_lines[_index].character];
        }

        public string GetEnemyCharacterKey()
        {
            string enemyName = _lines.Select(line => line.character).Distinct().Where(character => character != MainCharacter.Me).ToArray()[0];
            return ScenarioDataSO.CharacterJaNameToKey[enemyName];
        }

        public string GetMainCharacterKey()
        {
            return ScenarioDataSO.CharacterJaNameToKey[MainCharacter.Me];
        }
    }
}