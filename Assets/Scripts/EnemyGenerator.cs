using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    [Header("Debug Settings: ")]
    public bool isSpawning = true;
    [Header("Individual Settings: ")]
    GameObject[] currentEnemies; // list of enemies alive right now
    public int currentTotalEnemies; // number of enemies alive right now
    public int totalEnemies; // total number of enemies to spawn
    public string[] enemiesStart; // list of enemies to spawn at start

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
    int widthPG;
    int heightPG;

    [Header("Creates a list of current projectiles based on enemies that will spawn. (For object pooler)")]
    public static List<GameObject> projectileList;
    public List<EnemyScriptObj> enemyScriptObjsList;
    public ProceduralGeneration pg;

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
        widthPG = pg.width;
        heightPG = pg.height;

        print(widthPG + "     " + heightPG);

        if (enemiesStart != null && isSpawning)
        {

            if (isWave)
            {
                Invoke("generateWave", 0.3f);
            }
            else if (isGroup)
            {
                Invoke("generateGroup", 0.3f);
            }
            else if (!isGroup)
            {
                // generateIndividual();
                Invoke("generateIndividual", 0.3f);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        getCurrentTotalEnemies();
    }

    void getCurrentTotalEnemies()
    {
        currentEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        currentTotalEnemies = currentEnemies.Length;
    }

    void generateGroup()
    {
        for (var i = 0; i < groupAmount; i++)
        {
            int dirX = Random.Range(0, widthPG);
            int dirY = Random.Range(0, heightPG);

            if (getMapPG(dirX, dirY) == 0)
            {
                for (var j = 0; j < groupSize; j++)
                {
                    float diff = Random.Range(-1.0f, 1.0f);
                    Vector3 randPos = new Vector3(transform.position.x + dirX + diff, transform.position.y + dirY + diff, 0);

                    generateOne(randPos, enemiesStart[Random.Range(0, enemiesStart.Length)]);
                }
            }
        }
    }

    public void generateIndividual()
    {
        print("generating");
        for (var i = 0; i < totalEnemies; i++)
        {
            int dirX = Random.Range(0, widthPG);
            int dirY = Random.Range(0, heightPG);

            if (getMapPG(dirX, dirY) == 0)
            {
                Vector3 randPos = new Vector3(transform.position.x + dirX, transform.position.y + dirY, 0);

                generateOne(randPos, enemiesStart[Random.Range(0, enemiesStart.Length)]);
            }
        }
    }

    void generateWave()
    {
        StartCoroutine(generateWaves());
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
            // print("wave generated");
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

    int getMapPG(int x, int y)
    {
        var mapPG = pg.map;
        int checkNBAmt = 2;

        for (int nbX = x - checkNBAmt; nbX <= x + checkNBAmt; nbX++)
        {
            for (int nbY = y - checkNBAmt; nbY <= y + checkNBAmt; nbY++)
            {

                if (nbX >= 0 && nbX < widthPG && nbY >= 0 && nbY < heightPG)
                {
                    if (mapPG[nbX, nbY] == 1)
                    {
                        // wall tile is checkNBAmt away from current tile at (x,y), thus dont spawn enemy
                        return 1;
                    }
                }
            }
        }

        return mapPG[x, y];
    }

}
