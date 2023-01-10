using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class Arrow : MonoBehaviour
{
    private readonly float speed = 30f;
    private readonly float lifeTime = 3f;
    private readonly int arrowDamage = 100;

    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform particleTrail;
    public int dirX;

    void Start()
    {
        Destroy(gameObject, lifeTime);
        particleTrail.parent = null;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(dirX * speed * Time.deltaTime * transform.right);

        Collider2D hitObject = Physics2D.OverlapBox(transform.position, transform.localScale, 0, enemyLayer);

        // Change to: enemy.GetComponent<BossScript>().Damage();
        if (hitObject != null)
        {
            Debug.Log("You hit " + hitObject.name);
            
            //Breakable wall
            if (hitObject.gameObject.layer == 6)
            {
                ParticleSystem wallParticle =  hitObject.GetComponent<ParticleSystem>();
                Collider2D wallCol = hitObject.GetComponent<Collider2D>();
                Renderer wallRenderer = hitObject.GetComponent<Renderer>();
                wallRenderer.enabled = false;
                wallCol.enabled = false;
                wallParticle.Play();

                Destroy(hitObject.gameObject, 1);
            }
            if (hitObject.gameObject.tag == "Boss")
            {
                BossMovement boss = hitObject.gameObject.GetComponent<BossMovement>();
                boss.Damage(arrowDamage);

                Destroy(hitObject.gameObject, 1);
            }

            Destroy(gameObject);
        }

        // Makes sure the arrow gets destroyed after hitting an object if said object isn't an enemy.
        if (Physics2D.OverlapBox(transform.position, transform.localScale, 0, groundLayer)) Destroy(gameObject);
    }
}
