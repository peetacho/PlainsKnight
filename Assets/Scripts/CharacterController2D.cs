using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController2D : MonoBehaviour
{

    Rigidbody2D rb;
    Animator animator;
    private Animator weaponAnimator;
    private float movementSpeed;
    private Animator weaponClone;


    public Joystick joystick;
    public static Vector2 movementDirection;
    public Transform weaponPointRange;
    public LayerMask enemyLayers;

    [Header("Character Stats:")]
    public float MOVEMENT_BASE_SPEED = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GameObject.Find("PlayerGFX").GetComponent<Animator>();
        Physics.IgnoreLayerCollision(8, 9);

    }

    // Update is called once per frame
    void Update()
    {
        ProcessInputs();

        Animate();

        if (movementDirection.x > 0f)
        {
            weaponPointRange.localPosition = new Vector2(Mathf.Abs(weaponPointRange.localPosition.x), weaponPointRange.localPosition.y);
        }
        else if (movementDirection.x < 0f)
        {
            float newX = (Mathf.Abs(weaponPointRange.localPosition.x) * -1);
            weaponPointRange.localPosition = new Vector2(newX, weaponPointRange.localPosition.y);
        }

    }
    private void FixedUpdate()
    {
        Move();
    }

    public void Dash()
    {
        print("DASH");
    }

    public void Swing()
    {
        weaponClone = GameObject.Find("Weapon(Clone)").GetComponent<Animator>();
        weaponClone.Play("Swing1");
        meleeAttack();
    }

    public void meleeAttack()
    {
        // get weapon damage from get weapon script. static variable.
        float damage = GetWeapon.weaponDamage;


        // creates box and if enemies are in it, they take damage
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(weaponPointRange.position, new Vector3(GetWeapon.attackRangeX, GetWeapon.attackRangeY, 0), enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.tag == "Enemy")
            {
                // print("Enemy!    " + enemy);
                // deals damage to enemy in collider. 
                enemy.gameObject.GetComponent<Enemy>().TakeDamage(damage); // Enemy.TakeDamage(); // for static use
            }
            if (enemy.tag == "Projectile")
            {
                print("projectile!    " + enemy);
                Destroy(enemy.gameObject);
            }
        }

    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(weaponPointRange.position, new Vector3(GetWeapon.attackRangeX, GetWeapon.attackRangeY, 0));
    }

    void Move()
    {
        rb.velocity = MOVEMENT_BASE_SPEED * (movementDirection * movementSpeed);
    }

    void ProcessInputs()
    {
        movementDirection = new Vector2(joystick.Horizontal, joystick.Vertical);
        movementSpeed = Mathf.Clamp(movementDirection.magnitude, 0.0f, 1.0f);
        movementDirection.Normalize();
    }

    void Animate()
    {
        if (movementDirection != Vector2.zero)
        {
            animator.SetFloat("Horizontal", movementDirection.x);
            animator.SetFloat("Vertical", movementDirection.y);
        }
        animator.SetFloat("Speed", movementSpeed);
    }

}
