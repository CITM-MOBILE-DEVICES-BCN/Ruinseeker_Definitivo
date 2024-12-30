using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ruinseeker
{
    public class PauseButtons : MonoBehaviour
    {
        public Button resumeButton;
        public Button settingsButton;
        public Button exitButton;

        private void Awake()
        {
            resumeButton.onClick.AddListener(() => GameManager.instance.ResumeGame());
            settingsButton.onClick.AddListener(() => GameManager.instance.DestroyScreenRequest("PauseCanvas"));
            settingsButton.onClick.AddListener(() => GameManager.instance.LoadScreenRequest("SettingsCanvas"));
            exitButton.onClick.AddListener(() => GameManager.instance.DestroyScreenRequest("PauseCanvas"));
            exitButton.onClick.AddListener(() => GameManager.instance.LoadSceneRequest("LevelSelectorScene"));
            
        }
    }
}
