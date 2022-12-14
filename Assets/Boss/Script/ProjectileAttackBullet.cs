using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAttackBullet : MonoBehaviour
{
    readonly float moveSpeed = 7f;
    
    Rigidbody2D rb;

    GameObject target;
    PlayerController player;
    Vector2 moveDirection;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player");

        moveDirection = (target.transform.position - transform.position).normalized * moveSpeed;
        rb.velocity = new Vector2(moveDirection.x, moveDirection.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Hit player");
            player.Damage(1);
        }
    }
}
