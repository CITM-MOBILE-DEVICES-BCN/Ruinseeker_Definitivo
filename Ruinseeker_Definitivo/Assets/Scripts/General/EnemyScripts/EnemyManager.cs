using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ruinseeker
{

    public class EnemyManager:MonoBehaviour
    {
        public List<GameObject> enemies = new List<GameObject>();
        public void DestroyAllEnemies()
        {
          

            foreach (GameObject enemy in enemies)
            {
                Enemy enemyScript = enemy.GetComponent<Enemy>();
                if (enemyScript != null)
                {
                    enemyScript.Die();
                  
                }
                else
                {
                    Debug.LogWarning($"El objeto {enemy.name} con tag 'Enemy' no tiene un componente Enemy.");
                }
            }
        }
        public void RespawnAllEnemies()
        {
          

            foreach (GameObject enemy in enemies)
            {
                Enemy enemyScript = enemy.GetComponent<Enemy>();
                if (enemyScript.isActiveAndEnabled == false)
                {
                    enemyScript.respawnEnemy();

                }
               
            }
        }
    }
}