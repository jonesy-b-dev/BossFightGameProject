using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileRainProjectile : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector2(Random.Range(-20, 20), Random.Range(6, 26));

        Destroy(gameObject, 3);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
