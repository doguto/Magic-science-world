namespace Project.Scripts.Repository
{
    public class ModelRepository
    {
        protected string dataName = "";
        protected string DataAddress => $"Assets/Project/DataStore/{dataName}.asset";
    }
}
