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

    private bool IsSummoned = false;
    void Awake()
    {
        glowEffect = GetComponent<GlowEffect>();
        room = GetComponentInParent<Room>();
        IsSummoned = false;
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

    public void Summons()
    {
        if (!IsSummoned)
        {
            Debug.Log("summon");
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
