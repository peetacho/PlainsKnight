using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Enemy : MonoBehaviour
{
    // public float idleSpeed = 1.0f;

    // private Coroutine slimeUpdate;
    private Rigidbody2D rb;
    public EnemyScriptObj enemyScriptObj;

    public GameObject projectile;

    // public float agroSpeed = 2.0f;

    // private bool chasingPlayer = false;

    private Transform player;

    public float meleeDamage = 0.25f;
    public float attackDelayTime = 2.0f;
    public float rotZ = 5.0f;
    public float attackRange = 5.0f;
    public float rangedAttackSpeed = 2.0f;
    private Coroutine shoot;
    private float distToPlayer;

    AIPath aipath;

    Animator gfxAnim;

    SpriteRenderer gfxSr;

    private GameObject projectileInstance;

    void Awake()
    {
        // Get a reference to physics component
        rb = GetComponent<Rigidbody2D>();
        // Get a reference to AIPATH component
        aipath = GetComponent<AIPath>();
    }

    void Start()
    {
        // initialize player transform
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        // initializes animator and sprite renderer
        gfxAnim = transform.GetChild(0).GetComponent<Animator>();
        gfxSr = transform.GetChild(0).GetComponent<SpriteRenderer>();
        initArt();

        // slimeUpdate = StartCoroutine(Idle());

        StartCoroutine(Shoot());

    }

    void initArt()
    {
        print("initart!");
        gfxAnim.runtimeAnimatorController = enemyScriptObj.controller;
        gfxSr.sprite = enemyScriptObj.artwork;
    }

    private void Update()
    {
        flipEnemy();

        // Set rotation
        if (projectileInstance != null)
        {
            projectileInstance.transform.Rotate(new Vector3(0, 0, rotZ));
        }

        distToPlayer = Vector2.Distance(player.transform.position, transform.position);
    }

    // flips enemy. uses aipath module
    private void flipEnemy()
    {
        if (aipath.desiredVelocity.x >= 0.01f)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (aipath.desiredVelocity.x <= -0.01f)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    public static void TakeDamage()
    {
        print("Enemy has taken damage!");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // if (other.tag == "Player" && !chasingPlayer)
        // {
        //     chasingPlayer = true;
        //     // Stop Idle
        //     StopCoroutine(slimeUpdate);
        // }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            FindObjectOfType<HealthBarManager>().resetHearts("damage", meleeDamage);
        }
    }

    IEnumerator Shoot()
    {
        while (true)
        {
            if (distToPlayer <= attackRange)
            {
                Vector3 enemyPos = transform.position;
                Vector3 playerPos = player.transform.position;

                projectileInstance = Instantiate(projectile, enemyPos, transform.rotation);

                Rigidbody2D projRB = projectileInstance.GetComponent<Rigidbody2D>();


                // find vector between player position and enemy position
                Vector3 vector = (playerPos - enemyPos).normalized * (rangedAttackSpeed * 5);

                // debug line
                Debug.DrawLine(enemyPos, vector + enemyPos, Color.red, attackDelayTime);

                // print(vector);
                projRB.AddForce(vector);

            }

            // waits for attackDelayTime seconds
            yield return new WaitForSeconds(attackDelayTime);
        }
    }

    // IEnumerator Idle()
    // {
    //     // int direction = 1;

    //     while (true)
    //     {
    //         // Move in a direction
    //         float dirX = Random.Range(-1.0f, 1.0f);
    //         float dirY = Random.Range(-1.0f, 1.0f);
    //         // print(dirX.ToString() + "    " + dirY.ToString());
    //         rb.velocity = (new Vector2(idleSpeed * dirX, idleSpeed * dirY));
    //         // print("idle");

    //         float wait = Random.Range(0.7f, 1.5f);

    //         // Wait
    //         yield return new WaitForSeconds(wait);
    //     }
    // }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

}
