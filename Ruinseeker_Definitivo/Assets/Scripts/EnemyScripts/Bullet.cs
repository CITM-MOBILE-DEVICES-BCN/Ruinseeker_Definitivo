using Ruinseeker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bullet : MonoBehaviour
{
    private Vector3 direction;
    private float speed;

    public void Initialize(Vector3 startPos, Vector3 playerPos, float bulletSpeed)
    {
        transform.position = startPos;
        speed = bulletSpeed;

        direction = (playerPos - startPos).normalized;

        StartCoroutine(DestroyAfterTime(2f));
    }

    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private IEnumerator DestroyAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }

}
