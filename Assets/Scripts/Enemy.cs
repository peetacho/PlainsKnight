using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float idleSpeed = 1.0f;

    private Coroutine slimeUpdate;
    private Rigidbody2D rb;

    public float agroSpeed = 2.0f;

    private bool chasingPlayer = false;

    private Transform player;


    void Awake()
    {
        // Get a reference to our physics component
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        // Start the idle coroutine
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        slimeUpdate = StartCoroutine(Idle());
    }

    private void Update()
    {
        if (chasingPlayer)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, 2.0f * Time.deltaTime);
        }
    }

    public static void TakeDamage()
    {
        print("Took damage!");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && !chasingPlayer)
        {
            chasingPlayer = true;
            // Stop Idle
            StopCoroutine(slimeUpdate);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            // print("ouch!");
        }
    }

    IEnumerator Idle()
    {
        // int direction = 1;

        while (true)
        {
            // Move in a direction
            float dirX = Random.Range(-1.0f, 1.0f);
            float dirY = Random.Range(-1.0f, 1.0f);
            // print(dirX.ToString() + "    " + dirY.ToString());
            rb.velocity = (new Vector2(idleSpeed * dirX, idleSpeed * dirY));
            // print("idle");

            float wait = Random.Range(0.7f, 1.5f);

            // Wait
            yield return new WaitForSeconds(wait);
        }
    }
}
