using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour {

    [System.Serializable]
    public class Pool {
        public string Tag;
        public GameObject Prefab;
        public int size;
    }

    public static ObjectPooler Instance;

    private void Awake() {
        Instance = this;
    }

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    // Use this for initialization


    void Start () {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach(Pool pool in pools) {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            
            for(int i = 0; i< pool.size; i++) {
                GameObject obj = Instantiate(pool.Prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.Tag, objectPool);
        }
	}
	
	// Update is called once per frame
	public GameObject SpawnFromPool(string Tag, Vector3 position) {

        if (!poolDictionary.ContainsKey(Tag)) {
            Debug.LogWarning("Pool with tag " + (Tag) +" doesn't exist");
            return null;
        }

        GameObject objectToSpawn = poolDictionary[Tag].Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;


        poolDictionary[Tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }
}
