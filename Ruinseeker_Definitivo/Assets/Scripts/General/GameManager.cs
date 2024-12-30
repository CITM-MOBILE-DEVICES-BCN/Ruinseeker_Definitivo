using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using NavigationSystem;

namespace Ruinseeker
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;
        public GameObject canvasForInstantiations;
        public NavigationController navigationController;
        

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
        }

        public void LoadSceneRequest(string sceneName)
        {
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
    }
}
