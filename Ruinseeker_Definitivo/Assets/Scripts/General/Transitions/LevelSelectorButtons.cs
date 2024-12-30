using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ruinseeker
{
    public class LevelSelectorButtons : MonoBehaviour
    {
        public Button backButton;
        public Button level1Button;
        public Button level2Button;
        public Button level3Button;
        public Button level4Button;
        public Button level5Button;

        private void Awake()
        {
            backButton.onClick.AddListener(() => GameManager.instance.LoadSceneRequest("LobbyScene"));
            level1Button.onClick.AddListener(() => GameManager.instance.LoadSceneRequest("Level 1"));
            level2Button.onClick.AddListener(() => GameManager.instance.LoadSceneRequest("Level 2"));
            level3Button.onClick.AddListener(() => GameManager.instance.LoadSceneRequest("Level 3"));
            level4Button.onClick.AddListener(() => GameManager.instance.LoadSceneRequest("Level 4"));
            level5Button.onClick.AddListener(() => GameManager.instance.LoadSceneRequest("Level 5"));
        }
    }
}
