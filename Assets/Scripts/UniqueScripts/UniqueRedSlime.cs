using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniqueRedSlime : MonoBehaviour
{
    Enemy es;
    public int projectileNum = 4;
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
                for (var i = 1; i <= projectileNum; i++)
                {
                    angle = (360f * i) / projectileNum;

                    // GameObject projectileInstance = Instantiate(es.projectile, enemyPos, transform.rotation);
                    GameObject projectileInstance = ObjectPooler.i.SpawnFromPool(es.projectile.name, enemyPos, transform.rotation);
                    // print("blue " + es.projectile.name);


                    Rigidbody2D projRB = projectileInstance.GetComponent<Rigidbody2D>();

                    float dirX = es.rangedAttackSpeed * 5 * Mathf.Sin((angle * Mathf.PI) / 180f);
                    float dirY = es.rangedAttackSpeed * 5 * Mathf.Cos((angle * Mathf.PI) / 180f);

                    // find vector between player position and enemy position
                    Vector3 vector = new Vector3(dirX, dirY, 0f);

                    // debug line
                    Debug.DrawLine(enemyPos, vector + enemyPos, Color.red, es.attackDelayTime);

                    // print(vector);
                    projRB.AddForce(vector);
                    if (angle > 360f)
                    {
                        angle = 0f;
                    }
                }
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
