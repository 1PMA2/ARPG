using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : Singleton<EffectManager>
{
    [SerializeField] private List<Queue<GameObject>> effectPools = new List<Queue<GameObject>>();
    [SerializeField] private List<GameObject> effectPrefabList;
    [SerializeField] private int particleInitialSize = 10;

    private Queue<GameObject> particlePool = new Queue<GameObject>();
    private Queue<GameObject> BrushPool = new Queue<GameObject>();
    private Queue<GameObject> guardParticlePool = new Queue<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        InitEffectPool(particleInitialSize, effectPrefabList[0], particlePool);

        InitEffectPool(particleInitialSize, effectPrefabList[1], BrushPool);

        InitEffectPool(particleInitialSize, effectPrefabList[2], guardParticlePool);

    }

    private void InitEffectPool(int poolSize, GameObject effectPrefab, Queue<GameObject> effectPool)
    {
        effectPools.Add(effectPool);

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(effectPrefab);
            obj.SetActive(false);
            effectPool.Enqueue(obj);
        }  
    }

    public GameObject GetObject(Queue<GameObject> pool, GameObject effectPrefab)
    {
        if (pool.Count > 0)
        {
            GameObject obj = pool.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            GameObject obj = Instantiate(effectPrefab);
            return obj;
        }
    }

    public void ReturnObject(Queue<GameObject> pool, GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }

    public GameObject GetEffect(int effectIndex, Vector3 position, Quaternion rotation, float returnTime)
    {
        if (effectIndex < 0 || effectIndex >= effectPools.Count)
        {
            Debug.LogError("Invalid effect index");
            return null;
        }

        GameObject effect = GetObject(effectPools[effectIndex], effectPrefabList[effectIndex]);
        effect.transform.position = position;
        effect.transform.rotation = rotation;

        StartCoroutine(ReturnEffectAfterDelay(effectIndex, effect, returnTime));

        return effect;
    }

    public void ReturnEffect(int effectIndex, GameObject effect)
    {
        if (effectIndex < 0 || effectIndex >= effectPools.Count)
        {
            Debug.LogError("Invalid effect index");
            return;
        }

        ReturnObject(effectPools[effectIndex], effect);
    }

    private IEnumerator ReturnEffectAfterDelay(int effectIndex, GameObject effect, float delay)
    {
        yield return new WaitForSeconds(delay);
        EffectManager.Instance.ReturnEffect(effectIndex, effect);
    }

}
