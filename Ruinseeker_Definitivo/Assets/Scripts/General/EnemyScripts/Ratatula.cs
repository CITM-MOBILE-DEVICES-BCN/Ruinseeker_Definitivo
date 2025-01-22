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

            // Incrementar contador de Ratatulas activas
            if (activeRatatulas == 0)
            {
                // Invertir controles solo si es la primera Ratatula
                player.GetComponent<PlayerMovement>().InvertControls();
            }
            activeRatatulas++;

            // Reiniciar el temporizador global de inversión
            StartCoroutine(InvertControlsTimer());
        }

        private IEnumerator InvertControlsTimer()
        {
            Debug.Log("Attached Ratatula to player. Total active: " + activeRatatulas);
            yield return new WaitForSeconds(5f);

            // Decrementar contador de Ratatulas activas
            activeRatatulas--;

            if (activeRatatulas <= 0)
            {
                // Restaurar controles solo si no quedan Ratatulas activas
                player.GetComponent<PlayerMovement>().InvertControls();
                activeRatatulas = 0; // Garantizar que el contador no sea negativo
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
