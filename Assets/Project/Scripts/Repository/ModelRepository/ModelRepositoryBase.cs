using Cysharp.Text;
using Project.Scripts.Extensions;

namespace Project.Scripts.Repository.ModelRepository
{
    public class ModelRepositoryBase
    {
        protected string dataName = "";
        protected string DataAddress 
            => ZString.Format("{0}/{1}.asset", GamePath.DataStorepath, dataName);
    }
}
