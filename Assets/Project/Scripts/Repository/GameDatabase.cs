using Project.Scripts.Model;

namespace Project.Scripts.Repository
{
    public class GameDatabase
    {
        public static GameDatabase Instance { get; private set; }

        public T Find<T>(int id) where T : ModelBase
        {
            return null;
        }
    }
}
