using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAttackBullet : MonoBehaviour
{
    readonly float moveSpeed = 14f;
    
    Rigidbody2D rb;

    GameObject target;
    PlayerController playerScript;
    Vector2 moveDirection;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player");
        playerScript= target.GetComponent<PlayerController>();


        moveDirection = (target.transform.position - transform.position).normalized * moveSpeed;
        rb.velocity = new Vector2(moveDirection.x, moveDirection.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerScript.Damage(2);
        }
    }
}
