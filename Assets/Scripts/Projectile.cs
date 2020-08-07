using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float DestroyTime = 2.0f;

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, DestroyTime);
    }
}
