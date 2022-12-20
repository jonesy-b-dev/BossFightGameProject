using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileRainProjectile : MonoBehaviour
{
    GameObject target;
    PlayerController playerScript;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        playerScript = target.GetComponent<PlayerController>();
        transform.position = new Vector2(Random.Range(-20f, 20f), Random.Range(6f, 26f));

        Destroy(gameObject, 3);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerScript.Damage(1);
        }
    }
}
