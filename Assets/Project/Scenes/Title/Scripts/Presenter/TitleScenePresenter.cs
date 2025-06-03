using System;
using Project.Scenes.Title.Scripts.Model;
using Project.Scenes.Title.Scripts.View;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;

namespace Project.Scenes.Title.Scripts.Presenter
{
    public class TitleScenePresenter : MonoBehaviour
    {
        [SerializeField] TitleMenuView titleMenuView;
        
        TitleBackgroundModel _titleBackgroundModel;

        void Start()
        {
            titleMenuView.OnPressedStart.Subscribe(StartGame);
        }

        void StartGame(Unit _)
        {
            
        }
    }
}