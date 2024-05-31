using PlayerController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonSword : MonoBehaviour
{
    private Recoil recoil;
    public float recoilDuration = 0.5f;
    

    // Start is called before the first frame update
    void Start()
    {
        recoil = GetComponentInParent<Recoil>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // UnitController enemy = other.gameObject.GetComponent<UnitController>();

        if (other.CompareTag("Player"))
        {
            TestBox enemy = other.gameObject.GetComponent<TestBox>();

            if (enemy != null)
            {
                UnitInformation info = other.gameObject.GetComponent<UnitInformation>();

                switch(info.currentState)
                {
                    case UnitState.GUARD_01:
                        HitEffect(other);        
                        break;
                    case UnitState.EVADE:
                        break;
                    default:
                        HitEffect(other);
                        enemy.TakeDamage(1);
                        break;
                }     
            }

        }
    }

    private void HitEffect(Collider other)
    {
        recoil.StartRecoil(recoilDuration);

        CameraManager.Instance.ShakeCamera("PlayerCamera", 0.2f, 0.1f);

        Vector3 collisionPoint = other.ClosestPoint(transform.position);

        Vector3 enemyCenter = other.bounds.center;

        Vector3 direction = (enemyCenter - collisionPoint).normalized;

        EffectManager.Instance.GetEffect(0, collisionPoint, Quaternion.LookRotation(direction), 1f);
    }
}
