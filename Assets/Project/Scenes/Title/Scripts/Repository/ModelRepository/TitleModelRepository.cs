using Project.Scenes.Title.Scripts.Model;
using Project.Scripts.Model;
using Project.Scripts.Repository.ModelRepository;

namespace Project.Scenes.Title.Scripts.Repository.ModelRepository
{
    public class TitleModelRepository : ModelRepositoryBase
    {
        public static TitleModelRepository Instance => new();
        
        UserModel userModel;
        TitleModel titleModel;

        public TitleModelRepository()
        {
            userModel = UserModel.Instance;
            titleModel = new(userModel.ClearedStageNumber);
        }

        public TitleModel Get()
        {
            titleModel ??= new(userModel.ClearedStageNumber);
            return titleModel;
        }

        public void Refresh()
        {
            titleModel = null;
        }
    }
}
