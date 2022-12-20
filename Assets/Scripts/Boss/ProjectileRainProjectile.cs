using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileRainProjectile : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector2(Random.Range(-20f, 20f), Random.Range(6f, 26f));

        Destroy(gameObject, 3);
    }
}
