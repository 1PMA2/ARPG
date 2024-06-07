using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MonsterManager : Singleton<MonsterManager>
{
    // Start is called before the first frame update
    [SerializeField] private GameObject monsterPrefab;
    [SerializeField] private int initialPoolSize = 5;

    private Queue<GameObject> monsterPool = new Queue<GameObject>();
    void Start()
    {
        InitializePool(0);
    }

    private void InitializePool(int poolSize)
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject monster = CreateNewMonster();
            monster.SetActive(false);
            monsterPool.Enqueue(monster);
        }
    }

    private GameObject CreateNewMonster()
    {
        return Instantiate(monsterPrefab);
    }

    public GameObject GetMonster(Vector3 position, Quaternion rotation)
    {
        GameObject monster = GetObject(monsterPool, monsterPrefab, position);

        monster.transform.position = position;
        monster.transform.rotation = rotation;

        EffectManager.Instance.GetEffect(3, new Vector3(monster.transform.position.x, 1.25f, monster.transform.position.z), Quaternion.identity, 2f);
        return monster;
    }


    public GameObject GetObject(Queue<GameObject> pool, GameObject MonsterPrefab, Vector3 position)
    {
        if (pool.Count > 0)
        {
            GameObject obj = pool.Dequeue();
            StartCoroutine(Delay(obj, 0.1f));
            obj.transform.position = position;
            return obj;
        }
        else
        {
            GameObject obj = Instantiate(monsterPrefab, position, Quaternion.identity);
            return obj;
        }
    }

    private IEnumerator Delay(GameObject monster, float delay)
    {
        yield return new WaitForSeconds(delay);
        monster.SetActive(true);
    }

    public void ReturnMonster(GameObject monster)
    {
        monster.SetActive(false);
        monsterPool.Enqueue(monster);
    }

    public void Restart()
    {
        foreach (GameObject monster in monsterPool)
        {
            Destroy(monster);
        }
        monsterPool.Clear();

        InitializePool(initialPoolSize);

        Destroy(gameObject);
    }
}
