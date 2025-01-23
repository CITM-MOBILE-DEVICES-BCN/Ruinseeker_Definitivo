using DG.Tweening;
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
            public Image completedLevelImage;
            public Button levelButton;            
        }

        [Header("Global Progress")]
        [SerializeField] private TextMeshProUGUI totalGemsText;
        [SerializeField] private TextMeshProUGUI totalScoreText;

        [Header("Level Progress")]
        [SerializeField] private LevelUI[] levelUIs;


        [SerializeField] private Image[] ArrowLevel1;
        [SerializeField] private Image[] ArrowLevel2;
        [SerializeField] private Image[] ArrowLevel3;
        [SerializeField] private Image[] ArrowLevel4;


        public Button backButton;

        private void Awake()
        {

            backButton.onClick.AddListener(() => GameManager.Instance.LoadSceneRequest("LobbyScene"));
            AudioManager.instance.PlayMenuMusic();

        }

        private void Start()
        {
            RestartArrowsScale();
            UpdateUI();
            SpawnArrowsSequentially();

            ScoreManager.Instance.OnProgressUpdated += UpdateUI;
        }

        public void RestartArrowsScale()
        {
            for (int i = 0; i < ArrowLevel1.Length; i++)
            {
                ArrowLevel1[i].transform.localScale = Vector3.zero;
            }
            for (int i = 0; i < ArrowLevel2.Length; i++)
            {
                ArrowLevel2[i].transform.localScale = Vector3.zero;
            }
            for (int i = 0; i < ArrowLevel3.Length; i++)
            {
                ArrowLevel3[i].transform.localScale = Vector3.zero;
            }
            for (int i = 0; i < ArrowLevel4.Length; i++)
            {
                ArrowLevel4[i].transform.localScale = Vector3.zero;
            }
        }



        private void SpawnArrowsSequentially()
        {
            Image[][] arrowLevels = { ArrowLevel1, ArrowLevel2, ArrowLevel3, ArrowLevel4 };

            for (int i = 1; i < levelUIs.Length ; i++)
            {
                //var (stars, _) = ScoreManager.Instance.GetLevelProgress(levelUIs[i].levelName);

                var stars = i < 4 ? 3 : 0;
                AnimateArrows(arrowLevels[i - 1], stars == 0);
                if (stars == 0) return;
            }
        }

        private void AnimateArrows(Image[] arrows, bool isLast = true)
        {
            for (int i = 0; i < arrows.Length; i++)
            {
                var arrow = arrows[i];
                if (arrow != null)
                {
                    //TODO:^Pass isLast as a parameter

                    arrow.transform.localScale = Vector3.zero;
                    DOTween.Kill(arrow.transform); // Detén cualquier animación previa en este transform
                    arrow.transform.DOScale(Vector3.one, isLast ? 0.4f : 0).SetDelay(isLast ? 0.2f * i : 0).SetEase(Ease.OutBack, 10);
                }
            }
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
            bool wasPreviousLevelCompleted = true;

            // Update each level's progress
            foreach (var levelUI in levelUIs)
            {
                if (levelUI == null) continue;
                var (stars, maxStars) = ScoreManager.Instance.GetLevelProgress(levelUI.levelName);
                stars =  3;
                if (stars == 0)
                {
                    levelUI.completedLevelImage.gameObject.SetActive(false);
                    if (wasPreviousLevelCompleted)
                    {
                        levelUI.levelButton.onClick.AddListener(() => GameManager.Instance.LoadSceneRequest(levelUI.levelName));
                        AudioManager.instance.PlayButtonSound();
                        AudioManager.instance.PlayInGameMusic();


                        wasPreviousLevelCompleted = false;
                    }
                    else
                    {
                        levelUI.levelButton.interactable = false;
                    }
                    
                }
                else 
                {
                    levelUI.levelButton.onClick.AddListener(() => GameManager.Instance.LoadSceneRequest(levelUI.levelName));
                    AudioManager.instance.PlayButtonSound();
                    AudioManager.instance.PlayInGameMusic();
                    
                }

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
