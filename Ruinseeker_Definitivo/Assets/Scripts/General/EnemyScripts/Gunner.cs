
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Ruinseeker
{

    public class Gunner : Enemy
    {
        public GameObject bulletPrefab;
        public float bulletSpeed = 5f;
        public float bulletRange = 10f;

        private bool isShooting;

        public override void Patrol()
        {

        }

        public override void OnPlayerDetected()
        {
            if (!isShooting)
            {
                StartCoroutine(Shoot());
            }
        }

        private IEnumerator Shoot()
        {

            isShooting = true;

            var bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            var bulletScript = bullet.GetComponent<Bullet>();
            bulletScript.Initialize(transform.position, player.position, bulletSpeed);

            yield return new WaitUntil(() => bullet == null);

            isShooting = false;
        }

        public override void Die()
        {
            Debug.Log("Boomerang Guy defeated!");

            ResetBulletState();

            base.Die();
        }

        private void ResetBulletState()
        {
            isShooting = false;
        }

        public override void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                Vector2 collisionDirection = (collision.transform.position - transform.position).normalized;
                float angle = Vector2.Angle(Vector2.up, collisionDirection);

                if (angle < 70)
                {
                    Die();
                    // falta añadir la función de salto después de matar al enemigo
                    GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<PlayerMovement>().JumpAfterKillingEnemy();
                }
                else
                {
                    //falta la funcion de muerte del jugador
                    //collision.gameObject.GetComponent<PlayerMovement>().DeadFunction();
                }
            }
        }
    }
}

