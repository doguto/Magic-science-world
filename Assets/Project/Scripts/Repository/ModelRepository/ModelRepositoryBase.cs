namespace Project.Scripts.Repository.ModelRepository
{
    public class ModelRepositoryBase
    {
        protected string dataName = "";
        protected string DataAddress => $"Assets/Project/DataStore/{dataName}.asset";
    }
}
