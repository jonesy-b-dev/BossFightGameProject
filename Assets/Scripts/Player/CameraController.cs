using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
        private GameObject cam;
        private CinemachineVirtualCamera cm;

    [SerializeField]
        private Transform cameraLocation;
    [SerializeField]
        private GameObject bigRoomCollider;

    private void Start()
    {
        cm = cam.GetComponent<CinemachineVirtualCamera>();
        cm.Follow = gameObject.transform;
        cm.m_Lens.OrthographicSize = 6;
        cm.GetComponent<CinemachineConfiner2D>().enabled = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "BigRoomCollider")
        {
            cm.Follow = cameraLocation;
            cm.GetComponent<CinemachineConfiner2D>().enabled = false;
            cm.m_Lens.OrthographicSize = 12;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "BigRoomCollider")   
        {
            cm.Follow = gameObject.transform;
            cm.m_Lens.OrthographicSize = 6;
            cm.GetComponent<CinemachineConfiner2D>().enabled = true;

            if (gameObject.transform.position.x > 140)
            {
                bigRoomCollider.GetComponent<PolygonCollider2D>().isTrigger = false;
            }
        }
    }
}
