using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
    public ParticleSystem dashParticle;
    Rigidbody2D rb;
    Animator animator;
    private Animator weaponAnimator;
    private float movementSpeed;
    private Animator weaponClone;

    public Vector2 shootDirection;
    public Joystick movementJoystick;
    public Joystick shootingJoystick;
    public static Vector2 movementDirection;
    public Transform weaponPointRange;
    public LayerMask enemyLayers;

    [Header("Character Stats:")]
    public float MOVEMENT_BASE_SPEED = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        dashParticle.Play();
        rb = GetComponent<Rigidbody2D>();
        animator = GameObject.Find("PlayerGFX").GetComponent<Animator>();
        Physics.IgnoreLayerCollision(8, 9);

        StartCoroutine(rangedWeaponShoot());

    }

    // Update is called once per frame
    void Update()
    {
        ProcessMovementInputs();

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
    public bool isDashing = false;
    private void FixedUpdate()
    {
        if (!isDashing)
        {
            Move();
        }
        else
        {
            isDashing = !isDashing;
        }
    }
    public float dashForce;

    public IEnumerator dash()
    {
        if (isDashing)
        {
            print("DASH");
            Vector2 dashVector = movementDirection;
            dashForce = 75.0f;
            rb.AddForce(dashVector * dashForce, ForceMode2D.Impulse);
            // transform.position = Vector2.MoveTowards(transform.position, dashVector, 1.0f);
        }

        yield return new WaitForSeconds(1.0f);
    }

    private float coolDownDash = 1.0f;
    private float nextDash = 0.0f;

    public void Dash()
    {
        if (Time.time > nextDash)
        {
            isDashing = true;
            // StartCoroutine(dash());

            print("DASH");
            Vector2 dashVector = movementDirection;
            dashForce = 55.0f;
            rb.AddForce(dashVector * dashForce, ForceMode2D.Impulse);
            // transform.position = Vector2.MoveTowards(transform.position, dashVector, 1.0f);

            nextDash = Time.time + coolDownDash;
        }
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
        float damage = GetWeapon.weaponDamageM;


        // creates box and if enemies are in it, they take damage
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(weaponPointRange.position, new Vector3(GetWeapon.attackRangeXM, GetWeapon.attackRangeYM, 0), enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.tag == "Enemy")
            {
                // print("Enemy!    " + enemy);
                // deals damage to enemy in collider. 
                enemy.gameObject.GetComponent<Enemy>().TakeDamage(damage, GetWeapon.weaponCriticalChanceM, GetWeapon.weaponKnockBackM); // Enemy.TakeDamage(); // for static use
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
        Gizmos.DrawWireCube(weaponPointRange.position, new Vector3(GetWeapon.attackRangeXM, GetWeapon.attackRangeYM, 0));
    }

    void Move()
    {
        rb.velocity = MOVEMENT_BASE_SPEED * (movementDirection);
    }

    IEnumerator rangedWeaponShoot()
    {
        while (true)
        {
            float shootDelayTime = GetWeapon.shootDelayTimeR;
            Vector3 playerPos = transform.position;
            shootDirection = new Vector2((shootingJoystick.Horizontal), shootingJoystick.Vertical);
            shootDirection.Normalize();

            if (shootDirection != Vector2.zero)
            {
                // play swing animation
                GameObject.Find("Weapon(Clone)").GetComponent<Animator>().Play("Swing1");

                // get angle between start vector of (-0.45,-0.45) and shoot direction 
                float angle = Vector2.SignedAngle(new Vector2(-0.45f, -0.45f), shootDirection);

                // print(angle);
                // GameObject projectileInstance = Instantiate(GetWeapon.weaponProjectileR, transform.position, Quaternion.identity);
                GameObject projectileInstance = ObjectPooler.i.SpawnFromPool(GetWeapon.weaponProjectileR.name, transform.position, Quaternion.identity);

                Rigidbody2D projRB = projectileInstance.GetComponent<Rigidbody2D>();

                // print(vector);
                projRB.AddForce(shootDirection * GetWeapon.attackSpeedR * 5);
                projectileInstance.transform.eulerAngles = new Vector3(0, 0, angle);
            }

            yield return new WaitForSeconds(shootDelayTime);
        }
    }

    void ProcessMovementInputs()
    {
        movementDirection = new Vector2(movementJoystick.Horizontal, movementJoystick.Vertical);
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
