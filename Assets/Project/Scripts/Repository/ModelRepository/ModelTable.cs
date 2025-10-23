using Project.Scripts.Model;

namespace Project.Scripts.Repository.ModelRepository
{
    public class ModelTable<T> where T : ModelBase, new()
    {
        T model;

        public T Get()
        {
            model ??= new T();
            return model;
        }
    }
}
