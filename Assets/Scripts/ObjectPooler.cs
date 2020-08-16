using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }
    private GameObject[] poolObjectsProjectile;
    public List<Pool> pools;
    public int poolProjectileSize = 150;
    private Dictionary<string, Queue<GameObject>> poolDictionary;

    // singleton
    public static ObjectPooler i;
    private void Awake()
    {
        i = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        // create dictionary of string and queue
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        // loops through projectiles in resources folder and adds to list of Pools

        // print(EnemyGenerator.projectileList.ToArray());
        // print(EnemyGenerator.projectileList);

        // poolObjectsProjectile = Resources.LoadAll<GameObject>("Enemy Projectiles");
        poolObjectsProjectile = EnemyGenerator.projectileList.ToArray();
        foreach (GameObject poolObject in poolObjectsProjectile)
        {
            Pool p = new Pool();
            p.tag = poolObject.name;
            p.prefab = poolObject;
            p.size = poolProjectileSize;
            pools.Add(p);
        }

        // loops through projectiles in resources folder and adds to list of Pools
        poolObjectsProjectile = Resources.LoadAll<GameObject>("Player Projectiles");
        foreach (GameObject poolObject in poolObjectsProjectile)
        {
            Pool p = new Pool();
            p.tag = poolObject.name;
            p.prefab = poolObject;
            p.size = poolProjectileSize;
            pools.Add(p);
        }

        // loop through pool items inside of the list of pools
        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                // create object
                GameObject obj = Instantiate(pool.prefab);
                obj.transform.parent = this.transform;

                // disable object 
                obj.SetActive(false);

                // add object to queue
                objectPool.Enqueue(obj);
            }

            // add object pool to poolDictionary
            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {

        // print(tag);
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning(tag + " doesn't exist.");
            return null;
        }

        GameObject obj = poolDictionary[tag].Dequeue();
        obj.SetActive(true);
        obj.transform.position = position;
        obj.transform.rotation = rotation;

        poolDictionary[tag].Enqueue(obj);

        return obj;
    }

}
