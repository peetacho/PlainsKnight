using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    [Header("Individual Settings: ")]
    public int totalEnemies;
    // enemies to spawn at start
    public string[] enemiesStart;

    [Header("Update Settings: ")]
    // enemies to spawn at update
    public string[] enemiesUpdate;
    public float spawnDelayTime;

    // determines if enemies form in group or not
    [Header("Group Settings: ")]
    public bool isGroup;
    public int groupAmount;
    public int groupSize;

    [Header("Map width and height: ")]
    public float mapX;
    public float mapY;

    // Start is called before the first frame update
    void Start()
    {
        if (enemiesStart != null)
        {
            if (isGroup)
            {
                generateGroupStart();
            }
            else
            {
                generateStart();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void generateGroupStart()
    {
        for (var i = 0; i < groupAmount; i++)
        {
            float dirX = Random.Range(-mapX, mapX);
            float dirY = Random.Range(-mapY, mapY);

            for (var j = 0; j < groupSize; j++)
            {
                float diff = Random.Range(-1.0f, 1.0f);
                Vector3 randPos = new Vector3(transform.position.x + dirX + diff, transform.position.y + dirY + diff, 0);

                generateEnemy(randPos, enemiesStart[Random.Range(0, enemiesStart.Length)]);
            }
        }
    }

    void generateStart()
    {
        for (var i = 0; i < totalEnemies; i++)
        {
            float dirX = Random.Range(-mapX, mapX);
            float dirY = Random.Range(-mapY, mapY);

            Vector3 randPos = new Vector3(transform.position.x + dirX, transform.position.y + dirY, 0);

            generateEnemy(randPos, enemiesStart[Random.Range(0, enemiesStart.Length)]);
        }
    }

    void generateUpdate()
    {
    }

    public void generateEnemy(Vector3 position, string enemyName)
    {
        // load enemy as gameobject from resources folder
        GameObject enemy = Resources.Load("Prefabs/Enemy", typeof(GameObject)) as GameObject;

        // creates enemy at position
        GameObject enemyInstance = Instantiate(enemy, position, Quaternion.identity);

        // loads enemy scriptable object from resources folder
        EnemyScriptObj enemyScriptObj = Resources.Load("Scriptable Objects/Enemy/" + enemyName, typeof(EnemyScriptObj)) as EnemyScriptObj;

        // get loaded scrip obj and set enemy script obj
        enemy.GetComponent<Enemy>().enemyScriptObj = enemyScriptObj;
    }

}
