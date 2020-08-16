using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float DestroyTime;// = 2.0f;
    public int projectileDamage; // 0.25f;
    public float rotZ; // = 15.0f;

    Rigidbody2D rb;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, rotZ);
        // Destroy(gameObject, DestroyTime);

        if (DestroyTime != 0)
        {
            Invoke("destroyInTime", DestroyTime);
        }
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
        // print(gameObject.name);
        if (gameObject.name.Contains("Enemy"))
        {
            enemyProjectile(other);
        }
        else if (gameObject.name.Contains("Player"))
        {
            playerProjectile(other);
        }
    }

    void enemyProjectile(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            FindObjectOfType<HealthBarManager>().getHearts("damage", projectileDamage);
            gameObject.SetActive(false);
        }
        else if (other.gameObject.tag == "Obstacle")
        {
            gameObject.SetActive(false);
        }
    }

    void playerProjectile(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            // if current weapon type is melee
            if (GetWeapon.currentWeaponType == GetWeapon.weapontype.melee)
            {
                // melee weapons
                other.gameObject.GetComponent<Enemy>().TakeDamage(GetWeapon.weaponProjectileDamageM, GetWeapon.weaponCriticalChanceM, GetWeapon.weaponKnockBackM);
            }

            else if (GetWeapon.currentWeaponType == GetWeapon.weapontype.ranged)
            {
                // ranged weapons
                other.gameObject.GetComponent<Enemy>().TakeDamage(GetWeapon.weaponDamageR, GetWeapon.weaponCriticalChanceR, GetWeapon.weaponKnockBackR);
            }
            gameObject.SetActive(false);
        }
        else if (other.gameObject.tag == "Obstacle")
        {
            gameObject.SetActive(false);
        }
    }
}
