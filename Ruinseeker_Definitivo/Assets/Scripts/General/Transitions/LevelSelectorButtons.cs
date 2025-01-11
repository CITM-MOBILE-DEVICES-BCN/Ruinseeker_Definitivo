using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ruinseeker
{
    public class LevelSelectorButtons : MonoBehaviour
    {
        [System.Serializable]
        public class LevelUI
        {
            public string levelName;
            public TextMeshProUGUI starsText;
            public Image[] starImages;
            public Button levelButton;
        }

        [Header("Global Progress")]
        [SerializeField] private TextMeshProUGUI totalGemsText;
        [SerializeField] private TextMeshProUGUI totalScoreText;

        [Header("Level Progress")]
        [SerializeField] private LevelUI[] levelUIs;


        public Button backButton;
        public Button level1Button;
        public Button level2Button;
        public Button level3Button;
        public Button level4Button;
        public Button level5Button;

        private void Awake()
        {
            backButton.onClick.AddListener(() => GameManager.Instance.LoadSceneRequest("LobbyScene"));
            level1Button.onClick.AddListener(() => GameManager.Instance.LoadSceneRequest("Level 1"));
            level2Button.onClick.AddListener(() => GameManager.Instance.LoadSceneRequest("Level 2"));
            level3Button.onClick.AddListener(() => GameManager.Instance.LoadSceneRequest("Level 3"));
            level4Button.onClick.AddListener(() => GameManager.Instance.LoadSceneRequest("Level 4"));
            level5Button.onClick.AddListener(() => GameManager.Instance.LoadSceneRequest("Level 5"));
        }

        private void Start()
        {
            UpdateUI();
            ScoreManager.Instance.OnProgressUpdated += UpdateUI;
        }

        private void OnDestroy()
        {
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.OnProgressUpdated -= UpdateUI;
            }
        }

        private void UpdateUI()
        {
            // Update total progress
            if (totalGemsText != null)
                totalGemsText.text = $"Total Gems: {ScoreManager.Instance.TotalGems}";

            if (totalScoreText != null)
                totalScoreText.text = $"Total Score: {ScoreManager.Instance.TotalScore}";

            // Update each level's progress
            foreach (var levelUI in levelUIs)
            {
                if (levelUI == null) continue;

                var (stars, maxStars) = ScoreManager.Instance.GetLevelProgress(levelUI.levelName);

                // Update stars text
                if (levelUI.starsText != null)
                    levelUI.starsText.text = $"{stars}/{maxStars}";

                // Update star images if they exist
                if (levelUI.starImages != null)
                {
                    for (int i = 0; i < levelUI.starImages.Length; i++)
                    {
                        if (levelUI.starImages[i] != null)
                            levelUI.starImages[i].enabled = i < stars;
                    }
                }
            }
        }
    }

    

    


}
