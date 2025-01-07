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
                GameManager.Instance.LoadSceneRequest("LevelSelectorScene");
            }
        }
    }
}
