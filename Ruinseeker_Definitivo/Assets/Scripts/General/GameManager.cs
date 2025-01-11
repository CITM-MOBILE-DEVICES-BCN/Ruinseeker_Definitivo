using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using NavigationSystem;

namespace Ruinseeker
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        public GameObject canvasForInstantiations;
        public NavigationController navigationController;

        private SaveSystem saveSystem;

        public Vector2 checkpointPosition;


        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            saveSystem = new SaveSystem();

        }

        private void Start()
        {
            SaveData saveData = saveSystem.Load();
        }

        public void LoadSceneRequest(string sceneName)
        {
            ScoreManager.Instance.ResetLevelScore();
            navigationController.LoadScene(sceneName);
        }

        public void LoadScreenRequest(string screenName)
        {
            navigationController.LoadScreen(screenName, canvasForInstantiations.transform);
        }

        public void DestroyScreenRequest(string screenName)
        {
            navigationController.DestroyScreen(screenName);
        }

        public void PauseGame()
        {
            Time.timeScale = 0;
            LoadScreenRequest("PauseCanvas");
        }

        public void ResumeGame()
        {
            Time.timeScale = 1;
            DestroyScreenRequest("PauseCanvas");
        }


        public void UpdateCheckpointPosition(Vector2 pos)
        {
            checkpointPosition = pos;

            SaveData saveData = new SaveData()
            {
                lastCheckpointPosition = checkpointPosition
            };

            saveSystem.Save(saveData);
        }
        

    }
}
