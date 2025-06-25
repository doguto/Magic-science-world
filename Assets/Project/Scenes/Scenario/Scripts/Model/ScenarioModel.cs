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

      public ScenarioLine Next()
      {
          _index++;
          return _lines[_index];
      }
  }
}