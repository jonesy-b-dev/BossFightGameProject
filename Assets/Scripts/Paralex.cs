using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paralex : MonoBehaviour
{
    private float lenght, startPos;
    [SerializeField] private GameObject cameraObj;
    [SerializeField] private float parallexEffect;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position.x;
        lenght = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        float temp = (cameraObj.transform.position.x * (1 - parallexEffect));
        float dist = (cameraObj.transform.position.x * parallexEffect);

        transform.position = new Vector3(startPos + dist, transform.position.y, transform.position.z);

        if (temp > startPos + lenght)
        {
            startPos += lenght;
        }
        else if (temp < startPos - lenght)
        {
            startPos -= lenght;
        }
    }
}
