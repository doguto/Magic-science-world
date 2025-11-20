using Project.Scripts.Infra;

namespace Project.Scripts.Model;

public class CharacterModel : ModelBase
{
    public CharacterData CharacterData { get; }

    public string Name => CharacterData.name;
    public string EnglishName => CharacterData.englishName;

    public CharacterModel(CharacterData characterData)
    {
        CharacterData = characterData;
    }
}
