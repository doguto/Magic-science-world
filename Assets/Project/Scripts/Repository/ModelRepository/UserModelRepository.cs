using Project.Scripts.Infra;

namespace Project.Scripts.Repository.ModelRepository
{
    public class UserModelRepository
    {
        public static UserModelRepository Instance { get; } = new();
        

        UserDataModel userData;

        public UserModelRepository()
        {
            userData = new UserDataModel();
        }

        public UserDataModel Get()
        {
            return userData ??= new();
        }

    }
}