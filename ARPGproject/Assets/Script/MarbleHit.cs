using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarbleHit : MonoBehaviour
{
    private GlowEffect glowEffect;
    private Coroutine shakeCoroutine;

    public float duration = 0.5f; // 진동 지속 시간
    public float magnitude = 0.05f; // 진동 강도

    private Room room;
    private Vector3 roomPos;

    private bool IsSummoned = false;
    void Awake()
    {
        glowEffect = GetComponent<GlowEffect>();
        room = GetComponentInParent<Room>();
        IsSummoned = false;

        roomPos = room.transform.position;
    }
    public GlowEffect GetGlowEffect()
    {
        return glowEffect;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Attack"))
        {
            ShakeMarble();

            room.OnMarbleHit(this);
        }
    }

    private void Update()
    {

    }



    public void Summons()
    {
        
        if (!IsSummoned)
        {
            StartCoroutine(SummomMonster(5));

            IsSummoned = true;
        }
    }

    private void ShakeMarble()
    {
        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
        }
        shakeCoroutine = StartCoroutine(ShakeObject());
    }

    private IEnumerator SummomMonster(int num)
    {
        int count = 0;
        Vector3 randomPos;
        int randomX;
        int randomY;
        int randomZ;

        while (count < num)
        {
            randomX = Random.Range(-7, 8);
            randomY = Random.Range(0, 360);
            randomZ = Random.Range(-7, 8);
            //summon effect
            randomPos = new Vector3(randomX, 0f, randomZ);

            MonsterManager.Instance.GetMonster(roomPos + randomPos, Quaternion.Euler(0, randomY, 0));

            count++;
            yield return null;
        }

        count = 0;
        StopCoroutine(SummomMonster(num));
    }


    private IEnumerator ShakeObject()
    {  
        Vector3 originalPosition = transform.localPosition;

        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float z = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(originalPosition.x + x, originalPosition.y, originalPosition.z + z);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPosition;
    }
}
