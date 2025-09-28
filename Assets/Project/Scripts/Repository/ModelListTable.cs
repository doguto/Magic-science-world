using System.Collections.Generic;
using Project.Scripts.Model;

namespace Project.Scripts.Repository
{
    public class ModelListTable<T> where T : ModelBase, new()
    {
        readonly List<T> modelList = new();

        public T FindByIndex(int index)
        {
            if (index < 0) return null;
            if (index >= modelList.Count) return null;

            return modelList[index];
        }

        public List<T> All()
        {
            return modelList;
        }
    }
}
