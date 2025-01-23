using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ruinseeker
{
    public class LevelButtons : MonoBehaviour
    {
        public Button pauseButton;

        private void Awake()
        {
            pauseButton.onClick.AddListener(() => GameManager.Instance.PauseGame());
            AudioManager.instance.PlayButtonSound();
        }
    }
}
