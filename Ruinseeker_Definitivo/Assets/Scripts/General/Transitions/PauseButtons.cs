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
            resumeButton.onClick.AddListener(() => GameManager.Instance.ResumeGame());
            settingsButton.onClick.AddListener(() => GameManager.Instance.DestroyScreenRequest("PauseCanvas"));
            settingsButton.onClick.AddListener(() => GameManager.Instance.LoadScreenRequest("SettingsCanvas"));
            exitButton.onClick.AddListener(() => {
                GameManager.Instance.DestroyScreenRequest("PauseCanvas");
                Time.timeScale = 1;
                GameManager.Instance.LoadSceneRequest("LevelSelectorScene");
            }
            );
            
        }
    }
}
