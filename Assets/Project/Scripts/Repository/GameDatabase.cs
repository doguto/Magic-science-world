using Project.Scenes.QuestList.Scripts.Model;
using Project.Scripts.Model;

namespace Project.Scripts.Repository
{
    public class GameDatabase
    {
        public static GameDatabase Instance { get; private set; } = new();

        public ModelTable<SceneRouterModel> SceneRouterModelTable { get; } = new();
        public ModelTable<QuestModel> QuestModelTable { get; } = new();
    }
}
