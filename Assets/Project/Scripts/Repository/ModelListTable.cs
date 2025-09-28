using System.Collections.Generic;
using Project.Scripts.Model;

namespace Project.Scripts.Repository
{
    public class ModelListTable<T> where T : ModelBase, new()
    {
        readonly List<T> models = new();

        public T FindByIndex(int index)
        {
            if (index < 0) return null;
            if (index >= models.Count) return null;

            return models[index];
        }

        public IReadOnlyList<T> All()
        {
            return models.AsReadOnly();
        }
    }
}
