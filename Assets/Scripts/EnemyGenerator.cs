using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    [Header("Individual Settings: ")]
    public int totalEnemies;
    // list of enemies to spawn at start
    public string[] enemiesStart;

    [Header("Generate in Waves Settings: ")]
    public bool isWave;
    public int waveAmount;
    public float spawnDelayTime;

    // determines if enemies form in group or not
    [Header("Group Settings: ")]
    public bool isGroup;
    public int groupAmount;
    public int groupSize;

    [Header("Map width and height: ")]
    public float mapX;
    public float mapY;

    [Header("Creates a list of current projectiles based on enemies that will spawn. (For object pooler)")]
    public static List<GameObject> projectileList;
    public List<EnemyScriptObj> enemyScriptObjsList;

    private void Awake()
    {
        if (enemiesStart != null)
        {

            projectileList = new List<GameObject>();
            enemyScriptObjsList = new List<EnemyScriptObj>();

            foreach (string enemy in enemiesStart)
            {
                // creates new enemyscriptobj from resources
                EnemyScriptObj enemyScriptObj = Resources.Load("Scriptable Objects/Enemy/" + enemy, typeof(EnemyScriptObj)) as EnemyScriptObj;

                // adds enemyscriptobj into enemyscriptobj list
                enemyScriptObjsList.Add(enemyScriptObj);

                // add projectile only if it list does not contain it
                // remove chance of a duplicate
                if (!projectileList.Contains(enemyScriptObj.projectile))
                {
                    // adds enemyscriptobj projectile into projectile list
                    projectileList.Add(enemyScriptObj.projectile);
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (enemiesStart != null)
        {

            if (isWave)
            {
                StartCoroutine(generateWaves());
            }
            else if (isGroup)
            {
                generateGroup();
            }
            else if (!isGroup)
            {
                generateIndividual();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void generateGroup()
    {
        for (var i = 0; i < groupAmount; i++)
        {
            float dirX = Random.Range(-mapX, mapX);
            float dirY = Random.Range(-mapY, mapY);

            for (var j = 0; j < groupSize; j++)
            {
                float diff = Random.Range(-1.0f, 1.0f);
                Vector3 randPos = new Vector3(transform.position.x + dirX + diff, transform.position.y + dirY + diff, 0);

                generateOne(randPos, enemiesStart[Random.Range(0, enemiesStart.Length)]);
            }
        }
    }

    public void generateIndividual()
    {
        for (var i = 0; i < totalEnemies; i++)
        {
            float dirX = Random.Range(-mapX, mapX);
            float dirY = Random.Range(-mapY, mapY);

            Vector3 randPos = new Vector3(transform.position.x + dirX, transform.position.y + dirY, 0);

            generateOne(randPos, enemiesStart[Random.Range(0, enemiesStart.Length)]);
        }
    }

    IEnumerator generateWaves()
    {
        for (var i = 0; i < waveAmount; i++)
        {
            if (isGroup)
            {
                generateGroup();
            }
            else
            {
                generateIndividual();
            }
            print("wave generated");
            yield return new WaitForSeconds(spawnDelayTime);
        }
    }



    public void generateOne(Vector3 position, string enemyName)
    {
        // load enemy as gameobject from resources folder
        GameObject enemy = Resources.Load("Prefabs/Enemy", typeof(GameObject)) as GameObject;

        // creates enemy at position
        GameObject enemyInstance = Instantiate(enemy, position, Quaternion.identity);

        // loads enemy scriptable object from resources folder
        // EnemyScriptObj enemyScriptObj = Resources.Load("Scriptable Objects/Enemy/" + enemyName, typeof(EnemyScriptObj)) as EnemyScriptObj;

        int index = System.Array.IndexOf(enemiesStart, enemyName);
        EnemyScriptObj enemyScriptObj = enemyScriptObjsList[index];

        // get loaded scrip obj and set enemy script obj
        enemy.GetComponent<Enemy>().enemyScriptObj = enemyScriptObj;
    }

}
