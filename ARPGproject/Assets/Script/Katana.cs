using PlayerController;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;

public class Katana : MonoBehaviour
{
    private Recoil recoil;
    public float recoilDuration = 0.5f;
    private UnitInformation unitInformation;

    private void Awake()
    {
        
    }
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


                EffectManager.Instance.GetEffect(0, collisionPoint, Quaternion.LookRotation(direction), 1f);

                enemy.TakeDamage(unitInformation.Damage);
            }
 
        }
    }


}
