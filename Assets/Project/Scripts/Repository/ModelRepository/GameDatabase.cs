using Project.Scripts.Model;

namespace Project.Scripts.Repository.ModelRepository
{
    public class GameDatabase
    {
        public static GameDatabase Instance { get; private set; } = new();

        public ModelTable<SceneRouterModel> SceneRouterModelTable { get; } = new();
    }
}
