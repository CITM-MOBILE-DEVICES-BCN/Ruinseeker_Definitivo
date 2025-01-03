using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ruinseeker
{
    public class SettingsButtons : MonoBehaviour
    {
        public Button closeButton;

        private void Awake()
        {
            closeButton.onClick.AddListener(() => GameManager.Instance.DestroyScreenRequest("SettingsCanvas"));
            closeButton.onClick.AddListener(() => GameManager.Instance.LoadScreenRequest("PauseCanvas"));
        }
    }
}
