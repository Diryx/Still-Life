using System;
using UnityEngine;

namespace Infrastructure
{
    [Serializable]
    public class SceneReference
    {
        [SerializeField] private SceneType _sceneType;

        private enum SceneType
        {
            MainMenu = 0,
            House = 1,
            Outside = 2,
        }

        public int SceneIndex => (int)_sceneType;
    }
}