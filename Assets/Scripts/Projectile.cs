using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float DestroyTime;// = 2.0f;
    public float projectileDamage; // 0.25f;
    public float rotZ; // = 15.0f;

    Rigidbody2D rb;

    // Update is called once per frame
    void Update()
    {
        // transform.Rotate(0, 0, rotZ);
        // Destroy(gameObject, DestroyTime);
        Invoke("destroyInTime", DestroyTime);
    }

    private void Awake()
    {
        rb = transform.GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        rb.WakeUp();
    }

    private void OnDisable()
    {
        rb.Sleep();
    }

    void destroyInTime()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // if (other.gameObject.tag == "Player")
        // {
        //     FindObjectOfType<HealthBarManager>().resetHearts("damage", projectileDamage);
        //     gameObject.SetActive(false);
        // }
        // else if (other.gameObject.tag == "Obstacle")
        // {
        //     gameObject.SetActive(false);
        // }
    }
}
