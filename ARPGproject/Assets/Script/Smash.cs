using PlayerController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smash : MonoBehaviour
{
    public float recoilDuration = 0.2f;
    private float smashDamage = 5f;

    private Recoil recoil;
    private UnitInformation unitInformation;
    // Start is called before the first frame update
    void Start()
    {
        smashDamage = 5f;
        recoil = GetComponentInParent<Recoil>();
        unitInformation = GetComponentInParent<UnitInformation>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            recoil.StartRecoil(recoilDuration);

            TestBox enemy = other.gameObject.GetComponent<TestBox>();

            if (enemy != null)
            {
                CameraManager.Instance.ShakeCamera("PlayerCamera", 0.2f, 0.1f);

                Vector3 collisionPoint = other.ClosestPoint(transform.position);

                Vector3 enemyCenter = other.bounds.center;

                Vector3 direction = (enemyCenter - collisionPoint).normalized;


                EffectManager.Instance.GetEffect(0, enemyCenter, Quaternion.LookRotation(direction), 1f);

                enemy.TakeDamage(unitInformation.Damage * smashDamage);
            }

        }
    }
}
