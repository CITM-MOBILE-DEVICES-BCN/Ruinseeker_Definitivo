using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ruinseeker
{
    public class FinishLevelController : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                ScoreManager.Instance.FinishLevel();
                GameManager.Instance.UpdateCheckpointPosition(new Vector3(0, -4, 0));
                GameManager.Instance.LoadSceneRequest("LevelSelectorScene");
                AudioManager.instance.PlayMenuMusic();
            }
        }
    }
}
