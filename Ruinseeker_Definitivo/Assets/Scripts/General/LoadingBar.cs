using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ruinseeker
{
    public class LoadingBar : MonoBehaviour
    {
        public Slider loadingBar;
        private float loadingValue = 0;

        void Update()
        {
            int randomNum = Random.Range(0, 100);

            if (randomNum < 20 && loadingValue < 0.8f)
            {
                FakeLoading();
            }
            else if (loadingValue >= 0.8f)
            {
                StartCoroutine(completeLoad());
            }
        }

        public void FakeLoading()
        {
            loadingValue += 0.01f;
            loadingBar.value = loadingValue;
        }

        IEnumerator completeLoad()
        {
            yield return new WaitForSeconds(1);
            loadingValue = 1;
            loadingBar.value = loadingValue;
            GameManager.Instance.LoadSceneRequest("LobbyScene");
            AudioManager.instance.PlayMenuMusic();
        }
    }
}
