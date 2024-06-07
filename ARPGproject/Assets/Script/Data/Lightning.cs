using PlayerController;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class Lightning : MonoBehaviour
{
    private SphereCollider SphereCollider;
    private VisualEffect effect;
    [SerializeField] private ItemData data;
    private UnitInformation unitInformation;
    // Start is called before the first frame update
    void Start()
    {
        SphereCollider = GetComponent<SphereCollider>();
        effect = gameObject.GetComponentInChildren<VisualEffect>();
        unitInformation = DungeonGenerator.Instance.playerInfo;
        effect.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        StartCoroutine(DelayCollider(1f));
        if(effect != null)
            effect.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            TestBox enemy = other.gameObject.GetComponent<TestBox>();

            if (enemy != null)
            {
                CameraManager.Instance.ShakeCamera("PlayerCamera", 0.1f, 0.1f);

                if(enemy.TakeDamage(unitInformation.Damage * 0.5f))
                {
                    TestBox player = unitInformation.gameObject.GetComponent<TestBox>();
                    UnitInformation enemyInfo = other.gameObject.GetComponent<UnitInformation>();
                    player.TakeEXP(enemyInfo.Exp);
                }

                SphereCollider.enabled = false;
            }

        }
    }



    IEnumerator DelayCollider(float delay)
    {
        yield return new WaitForSeconds(delay);
        SphereCollider.enabled = true;
    }


}
