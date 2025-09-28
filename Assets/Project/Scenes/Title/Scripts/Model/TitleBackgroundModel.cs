using UnityEngine;
using Project.Scripts.Infra;
using Project.Scripts.Model;


namespace Project.Scenes.Title.Scripts.Model
{
    public class TitleBackgroundModel : ModelBase
    {
        readonly UserDataObject userDataObject = new();

        public int ClearedStageAmount => userDataObject.ClearedStageNumber;
    }
}
