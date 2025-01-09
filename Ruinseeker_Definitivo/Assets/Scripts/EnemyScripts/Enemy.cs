using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

public abstract class Enemy : MonoBehaviour
{
    public float detectionRange = 5f;
    public float moveSpeed = 2f;
    protected Transform player;

    private Vector3 initialPosition; 
    private Quaternion initialRotation; 
    private bool isActive = true;
    protected virtual void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    public abstract void Patrol();

    protected virtual void Update()
    {
        if (!isActive) return; 

        if (IsPlayerInRange())
        {
            OnPlayerDetected();
        }
        else
        {
            Patrol();
        }
    }

    public abstract void OnPlayerDetected();

    public virtual void Die()
    {
        ResetEnemy();
    }

    private void ResetEnemy()
    {
        transform.position = initialPosition;
        transform.rotation = initialRotation;

        gameObject.SetActive(false);
        isActive = false;

       
       
    }
    protected bool IsPlayerInRange()
    {
        return Vector2.Distance(transform.position, player.position) <= detectionRange;
    }

    public virtual void OnCollisionEnter2D(Collision2D collision)
    {

    }
}
