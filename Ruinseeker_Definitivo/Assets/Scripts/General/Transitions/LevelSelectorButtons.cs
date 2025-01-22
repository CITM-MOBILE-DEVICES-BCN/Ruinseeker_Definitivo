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

        Sequence arrowSequence;


        public Button backButton;

        private void Awake()
        {

            backButton.onClick.AddListener(() => GameManager.Instance.LoadSceneRequest("LobbyScene"));
           
        }
        private void Start()
        {
            RestartArrowsScale(); // Reinicia las escalas y mata las animaciones previas
            UpdateUI();           // Actualiza el progreso de la UI
            SpawnArrowsSequentially(); // Inicia la animación de las flechas


            ScoreManager.Instance.OnProgressUpdated += UpdateUI;
        }

        private void OnDestroy()
        {
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.OnProgressUpdated -= UpdateUI;
            }

            // Detén todas las animaciones asociadas con las flechas
            Image[][] arrowLevels = { ArrowLevel1, ArrowLevel2, ArrowLevel3, ArrowLevel4 };
            foreach (var arrowLevel in arrowLevels)
            {
                foreach (var arrow in arrowLevel)
                {
                    if (arrow != null)
                    {
                        DOTween.Kill(arrow.transform); // Detén cualquier animación activa
                    }
                }
            }
        }


        public void RestartArrowsScale()
        {
            Image[][] arrowLevels = { ArrowLevel1, ArrowLevel2, ArrowLevel3, ArrowLevel4 };

            foreach (var arrowLevel in arrowLevels)
            {
                foreach (var arrow in arrowLevel)
                {
                    if (arrow != null)
                    {
                        arrow.transform.localScale = Vector3.zero; // Reinicia la escala de todas las flechas
                        DOTween.Kill(arrow.transform); // Asegúrate de eliminar cualquier animación previa
                    }
                }
            }
        }

        private void SpawnArrowsSequentially()
        {
    
            Image[][] arrowLevels = { ArrowLevel1, ArrowLevel2, ArrowLevel3, ArrowLevel4 };
            for (int i = 1; i < levelUIs.Length ; i++)
            {
                //var (stars, _) = ScoreManager.Instance.GetLevelProgress(levelUIs[i].levelName);

                var stars = i < 4 ? 3 : 0;
                AnimateArrows(arrowLevels[i-1], stars == 0);
                if(stars == 0) return;
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
                    arrow.transform.DOScale(Vector3.one, isLast ? 0.4f: 0).SetDelay(isLast ? 0.2f * i : 0).SetEase(Ease.OutBack,10);
                }
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
                if (stars == 0)
                {
                    levelUI.completedLevelImage.gameObject.SetActive(false);
                    if (wasPreviousLevelCompleted)
                    {     
                        levelUI.levelButton.onClick.AddListener(() =>
                        {
                            StopAllCoroutines();
                            if(arrowSequence != null)
                            {
                                arrowSequence.Kill();
                            }
                            GameManager.Instance.LoadSceneRequest(levelUI.levelName);
                        });
                        wasPreviousLevelCompleted = false;
                    }
                    else
                    {
                        levelUI.levelButton.interactable = false;
                    }
                    
                }
                else 
                {
                    levelUI.levelButton.onClick.AddListener(() =>
                    {
                        StopAllCoroutines();
                        if (arrowSequence != null)
                        {
                            arrowSequence.Kill();
                        }
                        GameManager.Instance.LoadSceneRequest(levelUI.levelName);
                    });
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
