using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    Vector3 cameraFollowPosition;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        cameraFollowPosition = player.transform.position;
        cameraFollowPosition.z = transform.position.z;
        transform.position = cameraFollowPosition;
    }
}
