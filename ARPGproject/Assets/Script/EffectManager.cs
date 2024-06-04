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
    private Queue<GameObject> impactParticlePool = new Queue<GameObject>();
    private Queue<GameObject> lightningPool = new Queue<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        InitEffectPool(5, effectPrefabList[0], particlePool);

        InitEffectPool(particleInitialSize, effectPrefabList[1], BrushPool);

        InitEffectPool(2, effectPrefabList[2], guardParticlePool);

        InitEffectPool(5, effectPrefabList[3], impactParticlePool);

        InitEffectPool(3, effectPrefabList[4], lightningPool);

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

    public void HitEffect(Recoil recoil, float recoilDuration, Collider other, int index)
    {
        recoil.StartRecoil(recoilDuration);

        CameraManager.Instance.ShakeCamera("PlayerCamera", 0.2f, 0.1f);

        Vector3 collisionPoint = other.ClosestPoint(transform.position);

        Vector3 enemyCenter = other.bounds.center;

        Vector3 direction = (enemyCenter - collisionPoint).normalized;

        GetEffect(index, enemyCenter, Quaternion.LookRotation(direction), 1f);
    }

    public void Restart()
    {
        foreach (GameObject particle in particlePool)
        {
            Destroy(particle);
        }
        particlePool.Clear();

        foreach (GameObject Brush in BrushPool)
        {
            Destroy(Brush);
        }
        BrushPool.Clear();

        foreach (GameObject guardParticle in guardParticlePool)
        {
            Destroy(guardParticle);
        }
        guardParticlePool.Clear();

        foreach (GameObject impactParticle in impactParticlePool)
        {
            Destroy(impactParticle);
        }
        impactParticlePool.Clear();

        foreach (GameObject lighthning in lightningPool)
        {
            Destroy(lighthning);
        }
        impactParticlePool.Clear();

        InitEffectPool(5, effectPrefabList[0], particlePool);

        InitEffectPool(particleInitialSize, effectPrefabList[1], BrushPool);

        InitEffectPool(2, effectPrefabList[2], guardParticlePool);

        InitEffectPool(5, effectPrefabList[3], impactParticlePool);

        InitEffectPool(3, effectPrefabList[4], lightningPool);

        Destroy(gameObject);
    }

    private IEnumerator ReturnEffectAfterDelay(int effectIndex, GameObject effect, float delay)
    {
        yield return new WaitForSeconds(delay);
        EffectManager.Instance.ReturnEffect(effectIndex, effect);
    }

}
