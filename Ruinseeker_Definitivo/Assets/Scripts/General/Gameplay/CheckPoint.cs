using Ruinseeker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ruinseeker
{
    public class CheckPoint : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                GameManager.Instance.UpdateCheckpointPosition(transform.position);
                Debug.Log("checkpoint saved");
            }

        }
    }
}
