using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    [SerializeField] private Transform target;
    private bool targetExists = true;
    void Update()
    {
        if (targetExists)
        {
            try
            {
                transform.position = target.position;
            }
            catch
            {
                targetExists = false;
            }
        }
    }
}
