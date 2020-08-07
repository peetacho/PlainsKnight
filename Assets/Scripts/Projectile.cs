using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float DestroyTime = 2.0f;
    public float projectileDamage = 0.25f;
    public float rotZ = 15.0f;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, rotZ);
        Destroy(gameObject, DestroyTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            FindObjectOfType<HealthBarManager>().resetHearts("damage", projectileDamage);
            Destroy(gameObject);
        }
    }
}
