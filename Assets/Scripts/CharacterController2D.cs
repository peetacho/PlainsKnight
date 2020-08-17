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

    public static Vector2 shootDirection;
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

        StartCoroutine(rightJoyStick());

    }

    // Update is called once per frame
    void Update()
    {
        ProcessMovementInputs();

        // get shoot direction
        shootDirection = new Vector2((shootingJoystick.Horizontal), shootingJoystick.Vertical);
        shootDirection.Normalize();

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

    [Range(0.0f, 2.0f)]
    public float meleeRange;

    void OnDrawGizmos()
    {
        // Gizmos.DrawWireCube(weaponPointRange.position, new Vector3(GetWeapon.attackRangeXM, GetWeapon.attackRangeYM, 0));
        if (WeaponHandler.weaponRangedPos != null)
        {
            Gizmos.DrawWireSphere(WeaponHandler.weaponRangedPos.transform.position, meleeRange);
        }
    }

    void Move()
    {
        rb.velocity = MOVEMENT_BASE_SPEED * (movementDirection);
    }

    IEnumerator rightJoyStick()
    {
        while (true)
        {
            // if current weapon type is melee
            if (GetWeapon.currentWeaponType == GetWeapon.weapontype.melee)
            {
                float attackDelayTime = GetWeapon.shootDelayTimeM;
                meleeWeaponShoot();
                yield return new WaitForSeconds(attackDelayTime);
            }
            else if (GetWeapon.currentWeaponType == GetWeapon.weapontype.ranged)
            {
                // ranged weapons
                float shootDelayTime = GetWeapon.shootDelayTimeR;
                rangedWeaponShoot();
                yield return new WaitForSeconds(shootDelayTime);
            }
        }
    }

    void meleeWeaponShoot()
    {
        // get weapon damage from get weapon script. static variable.
        float damage = GetWeapon.weaponDamageM;
        Vector3 weaponRangedPos = WeaponHandler.weaponRangedPos.transform.position;

        // swings only if right joystick is moving
        if (shootDirection != Vector2.zero)
        {
            // GameObject.Find("Weapon(Clone)").GetComponent<Animator>().Play("Swing1");
            GameObject.Find("Weapon(Clone)").GetComponent<Animator>().SetTrigger("Attack");
            float angle = Vector2.SignedAngle(new Vector2(-1f, -1f), shootDirection);

            // print(angle);
            // GameObject projectileInstance = Instantiate(GetWeapon.weaponProjectileR, transform.position, Quaternion.identity);
            GameObject projectileInstance = ObjectPooler.i.SpawnFromPool(GetWeapon.weaponProjectileM.name, weaponRangedPos, Quaternion.identity);

            Rigidbody2D projRB = projectileInstance.GetComponent<Rigidbody2D>();

            // print(vector);
            Vector2 force = shootDirection * GetWeapon.weaponProjectileSpeedM * 5;
            projRB.AddForce(force);

            // Vector3 force3 = new Vector3(force.x, force.y, 0);
            // Debug.DrawLine(weaponRangedPos, weaponRangedPos + force3, Color.yellow, 1.0f);

            projectileInstance.transform.eulerAngles = new Vector3(0, 0, angle);


            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(weaponRangedPos, meleeRange, enemyLayers);

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
                    // print("projectile!    " + enemy);
                    enemy.gameObject.SetActive(false);
                }
            }
        }
    }

    void rangedWeaponShoot()
    {
        Vector3 playerPos = transform.position;

        if (shootDirection != Vector2.zero)
        {
            // play swing animation
            // GameObject.Find("Weapon(Clone)").GetComponent<Animator>().Play("Swing1");
            FindObjectOfType<EnergyBarManager>().getEnergy("use", 1);

            // get angle between start vector of (-0.45,-0.45) and shoot direction 
            float angle = Vector2.SignedAngle(new Vector2(-1f, -1f), shootDirection);

            // print(angle);
            // GameObject projectileInstance = Instantiate(GetWeapon.weaponProjectileR, transform.position, Quaternion.identity);
            GameObject projectileInstance = ObjectPooler.i.SpawnFromPool(GetWeapon.weaponProjectileR.name, WeaponHandler.weaponRangedPos.transform.position, Quaternion.identity);

            Rigidbody2D projRB = projectileInstance.GetComponent<Rigidbody2D>();

            // print(vector);
            projRB.AddForce(shootDirection * GetWeapon.attackSpeedR * 5);
            projectileInstance.transform.eulerAngles = new Vector3(0, 0, angle);
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
