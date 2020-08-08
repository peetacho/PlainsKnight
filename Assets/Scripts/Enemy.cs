using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEditor;

public class Enemy : MonoBehaviour
{
    // public float idleSpeed = 1.0f;
    // private Coroutine slimeUpdate;
    // public float agroSpeed = 2.0f;
    // private bool chasingPlayer = false;
    AIPath aipath;
    AIDestinationSetter aIDestinationSetter;
    Animator gfxAnim;
    SpriteRenderer gfxSr;
    CircleCollider2D circleCollider2D;
    private Rigidbody2D rb;
    private float distToPlayer;
    private GameObject projectileInstance;
    private Transform player;


    [Header("Artwork:")]
    public EnemyScriptObj enemyScriptObj;
    public GameObject projectile;


    [Header("Enemy Stats:")]
    public float maxHealth;
    public float currentHealth;
    public string enemyName;
    public float meleeDamage;
    public float rangedDamage;
    public float rangedAttackSpeed;
    public float attackDelayTime;
    public float attackRange;
    public float movementSpeed;
    public float colliderRadius;


    void Awake()
    {
        // Get a reference to physics component
        rb = GetComponent<Rigidbody2D>();
        // Get a reference to AIPATH component
        aipath = GetComponent<AIPath>();
        circleCollider2D = GetComponent<CircleCollider2D>();
    }

    void Start()
    {
        // initialize player transform
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        // init things
        initStats();
        initTarget();
        initArt();
        initUniqueScript();

        // slimeUpdate = StartCoroutine(Idle());
        StartCoroutine(Shoot());

    }

    // gets unique script
    void initUniqueScript()
    {
        if (enemyScriptObj.uniqueScript)
        {
            var script = ObjectNames.GetDragAndDropTitle(enemyScriptObj.uniqueScript).Replace(" (MonoScript)", "");
            // print(script);
            Type scriptType = Type.GetType(script);

            // print(scriptType);
            gameObject.AddComponent(scriptType);
        }
    }

    void initStats()
    {
        var es = enemyScriptObj;
        maxHealth = es.maxHealth;
        currentHealth = maxHealth;

        enemyName = es.enemyName;
        meleeDamage = es.meleeDamage;
        rangedDamage = es.rangedDamage;
        rangedAttackSpeed = es.rangedAttackSpeed;
        attackDelayTime = es.attackDelayTime;
        attackRange = es.attackRange;

        // set max speed in AIPath component
        movementSpeed = es.movementSpeed;
        aipath.maxSpeed = movementSpeed;

        // set circle collider radius
        colliderRadius = es.colliderRadius;
        circleCollider2D.radius = colliderRadius;

    }

    // initializes target
    void initTarget()
    {
        aIDestinationSetter = GetComponent<AIDestinationSetter>();
        aIDestinationSetter.target = player;
    }

    // initializes animator and sprite renderer
    void initArt()
    {
        gfxAnim = transform.GetChild(0).GetComponent<Animator>();
        gfxSr = transform.GetChild(0).GetComponent<SpriteRenderer>();
        gfxAnim.runtimeAnimatorController = enemyScriptObj.controller;
        gfxSr.sprite = enemyScriptObj.artwork;
    }

    private void Update()
    {
        flipEnemy();

        // gets distance to player
        distToPlayer = Vector2.Distance(player.transform.position, transform.position);

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }

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

    public void TakeDamage(float damage)
    {
        Color32 enemyDamagePopUp = new Color32(198, 64, 84, 255);

        // random number determines critical hit
        float rand = UnityEngine.Random.Range(0.0f, 1.0f);
        Vector2 enemyPos = transform.position;

        float dirY = UnityEngine.Random.Range(0.0f, 0.8f);
        float dirX = UnityEngine.Random.Range(-0.8f, 0.8f);

        // GetWeapon.weaponCriticalChance is a float from 0.0f to 1.0f. All numbers <= this number will be considered the 'critical' chance.
        if (rand == GetWeapon.weaponCriticalChance)
        {
            // hits 3 times! 1/100 chance
            for (var i = 0; i < 2; i++)
            {
                dirY = UnityEngine.Random.Range(0.0f, 0.8f);
                dirX = UnityEngine.Random.Range(-1.0f, 1.0f);
                enemyPos = new Vector2(enemyPos.x + dirX, enemyPos.y + dirY);

                Popup.Create(enemyPos, damage, enemyDamagePopUp);
            }
        }
        else if (rand < GetWeapon.weaponCriticalChance)
        {
            // one critical strike. hits 1 time and has a chance depending on the weapon
            damage *= 2;
        }

        enemyPos = new Vector2(enemyPos.x + dirX, enemyPos.y + dirY);
        Popup.Create(enemyPos, damage, enemyDamagePopUp);


        currentHealth -= damage;
        FindObjectOfType<CameraShake>().Shake(0.01f, 0.01f);
        StartCoroutine(Hurt());
    }

    IEnumerator Hurt()
    {
        Color hurtColor = new Vector4(gfxSr.color.r, gfxSr.color.g, gfxSr.color.b, 0.2f);
        gfxSr.color = hurtColor;

        yield return new WaitForSeconds(0.45f);

        hurtColor = new Vector4(gfxSr.color.r, gfxSr.color.g, gfxSr.color.b, 1.0f);
        gfxSr.color = hurtColor;
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


    // draws attackRange from transform.position
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

}
