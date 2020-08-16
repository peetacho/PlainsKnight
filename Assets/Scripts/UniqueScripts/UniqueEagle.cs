using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniqueEagle : MonoBehaviour
{
    Enemy es;
    public int projectileNum = 1;
    public float angle = 0f;

    // Start is called before the first frame update
    void Start()
    {
        es = GetComponent<Enemy>();
        StartCoroutine(Shoot());
    }

    IEnumerator Shoot()
    {
        while (true)
        {
            if (es.distToPlayer <= es.attackRange)
            {
                Vector3 enemyPos = transform.position;
                Vector3 playerPos = es.player.transform.position;

                // GameObject projectileInstance = Instantiate(es.projectile, enemyPos, transform.rotation);
                GameObject projectileInstance = ObjectPooler.i.SpawnFromPool(es.projectile.name, enemyPos, transform.rotation);
                // print("brown " + projectile.name);

                // set damage in projectile script
                projectileInstance.GetComponent<Projectile>().projectileDamage = es.rangedDamage;

                Rigidbody2D projRB = projectileInstance.GetComponent<Rigidbody2D>();


                // find vector between player position and enemy position
                Vector3 vector = (playerPos - enemyPos).normalized * (es.rangedAttackSpeed * 5);

                // debug line
                Debug.DrawLine(enemyPos, vector + enemyPos, Color.red, es.attackDelayTime);

                // print(vector);
                projRB.AddForce(vector);
            }
            // waits for attackDelayTime seconds
            yield return new WaitForSeconds(es.attackDelayTime);
        }
    }
    // Update is called once per frame
    void Update()
    {
    }
}

