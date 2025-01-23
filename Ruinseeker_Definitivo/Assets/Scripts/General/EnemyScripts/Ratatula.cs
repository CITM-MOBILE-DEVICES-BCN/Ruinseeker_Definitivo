using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ruinseeker
{
    public class Ratatula : Enemy
    {
        private bool isAttached = false;
        private static int activeRatatulas = 0;

        public override void Patrol() { }

        public override void OnPlayerDetected()
        {
            if (!isAttached)
            {
                AttachToPlayer();
            }
        }

        private void AttachToPlayer()
        {
            isAttached = true;
            transform.SetParent(player);
            transform.localPosition = Vector3.zero;

            if (activeRatatulas == 0)
            {
                player.GetComponent<PlayerMovement>().InvertControls();
            }
            activeRatatulas++;

            StartCoroutine(InvertControlsTimer());
        }

        private IEnumerator InvertControlsTimer()
        {
            Debug.Log("Attached Ratatula to player. Total active: " + activeRatatulas);
            yield return new WaitForSeconds(5f);

            activeRatatulas--;

            if (activeRatatulas <= 0)
            {
                player.GetComponent<PlayerMovement>().InvertControls();
                activeRatatulas = 0;
            }

            Die();
        }

        public override void Die()
        {
            if (isAttached)
            {
                transform.SetParent(null);
                isAttached = false;
            }
            base.Die();
        }
    }
}
