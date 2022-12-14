using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAttackBullet : MonoBehaviour
{
    float moveSpeed = 7f;
    
    Rigidbody2D rb;

    GameObject target;
    Vector2 moveDirection;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player");

        Debug.Log(target.transform.position.x);

        moveDirection = (target.transform.position - transform.position).normalized * moveSpeed;
        rb.velocity = new Vector2(moveDirection.x, moveDirection.y);
    }
}
