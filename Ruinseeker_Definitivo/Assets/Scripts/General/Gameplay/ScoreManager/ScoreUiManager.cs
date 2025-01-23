using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace Ruinseeker
{
        public class ScoreUIManager : MonoBehaviour
        {
        #region Singleton
        public static ScoreUIManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }
        #endregion

        [Header("UI References")]
        [SerializeField] private TextMeshProUGUI gemsText;
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private Transform starContainer;
        [SerializeField] private Image[] starImages;

        private void Start()
        {
            for (int i = 0; i < starImages.Length; i++)
            {
                starImages[i].enabled = false;
            }
            ScoreManager.Instance.OnGemsChanged += UpdateGemsDisplay;
        }

        private void OnDestroy()
        {
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.OnGemsChanged -= UpdateGemsDisplay;
                ScoreManager.Instance.OnScoreChanged -= UpdateScoreDisplay;
                ScoreManager.Instance.OnStarsChanged -= UpdateStarsDisplay;
            }
        }

        public void UpdateGemsDisplay(int gems)
        {
            if (gemsText != null)
            {
                gemsText.text = $"{gems}";
            }
            UpdateStarsDisplay(ScoreManager.Instance.CalculateStars());
            UpdateScoreDisplay(ScoreManager.Instance.CalculateStars());
        }

        public void UpdateScoreDisplay(int score)
        {
            if (scoreText != null)
            {
                scoreText.text = $"{score}";
            }
        }

        public void UpdateStarsDisplay(int stars)
        {
            if (starImages != null)
            {
                for (int i = 0; i < stars; i++)
                {
                    starImages[i].gameObject.SetActive(true);
                }
            }
        }
    }
}