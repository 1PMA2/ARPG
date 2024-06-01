using PlayerController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonSword : MonoBehaviour
{
    private Recoil recoil;
    public float recoilDuration = 0.5f;

    private UnitInformation unitInformation;
    // Start is called before the first frame update
    void Start()
    {
        recoil = GetComponentInParent<Recoil>();
        unitInformation = GetComponentInParent<UnitInformation>();
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
                        HitEffect(other, 2);        
                        break;
                    case UnitState.GUARD_02:      
                        break;
                    case UnitState.EVADE:
                        break;
                    case UnitState.HIT:
                        break;
                    default:
                        HitEffect(other, 0);
                        enemy.TakeDamage(unitInformation.Damage);
                        break;
                }     
            }

        }
    }

    private void HitEffect(Collider other, int index)
    {
        recoil.StartRecoil(recoilDuration);

        CameraManager.Instance.ShakeCamera("PlayerCamera", 0.2f, 0.1f);

        Vector3 collisionPoint = other.ClosestPoint(transform.position);

        Vector3 enemyCenter = other.bounds.center;

        Vector3 direction = (enemyCenter - collisionPoint).normalized;

        EffectManager.Instance.GetEffect(index, enemyCenter, Quaternion.LookRotation(direction), 1f);
    }
}
