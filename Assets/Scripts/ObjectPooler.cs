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
    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;
    public static ObjectPooler op;

    // Start is called before the first frame update
    void Awake()
    {
        if (op==null) op = this;
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    private void Start()
    {
        foreach (GameObject bs in GameObject.FindGameObjectsWithTag("BloodSplatter"))
        {
            bs.GetComponent<ISpawn>().Spawn();
        }
        GameManager.gm.ShowText("Testi", new Vector2(0f, 15f), "FFFFFF");        
    }

    public GameObject Spawn (string tag, Vector3 position, Quaternion? rotation = null, Transform parent = null, bool willSpawn = false)
    {
        GameObject obj = poolDictionary[tag].Dequeue();
        obj.SetActive(true);
        obj.transform.position = position;
        poolDictionary[tag].Enqueue(obj);
        obj.transform.SetParent(parent);
        if (tag == "Spikes") obj.GetComponent<SpikesScript>().noSpawn = willSpawn;

        if (rotation == null) ;
        else obj.transform.rotation = (Quaternion) rotation;
       
        obj.GetComponent<ISpawn>().Spawn();

        return obj;

    }

 
}
