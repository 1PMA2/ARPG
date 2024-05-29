using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Katana : MonoBehaviour
{
    public Animator parentAnimator;
    public float recoilDuration = 0.5f;
    private bool isRecoilActive = false;

    public GameObject particle;

    private int damage = 1;
    public int Damage
    {
        get
        {
            return damage;
        }
        set
        {
            damage = value;
        }
    }

    private void Awake()
    {
        parentAnimator = GetComponentInParent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            TestBox enemy = other.gameObject.GetComponent<TestBox>();

            if (enemy != null)
            {
                Vector3 collisionPoint = other.ClosestPoint(transform.position);

                Vector3 enemyCenter = other.bounds.center;

                Vector3 direction = (enemyCenter - collisionPoint).normalized;

                Instantiate(particle, collisionPoint, Quaternion.LookRotation(direction));

                enemy.TakeDamage(Damage);

                if (!isRecoilActive)
                    StartCoroutine(PauseAnimation(recoilDuration));

            }
 
        }
    }

    IEnumerator PauseAnimation(float duration)
    {
        if (parentAnimator != null)
        {
            isRecoilActive = true;
            // 현재 애니메이터 속도를 저장
            float originalSpeed = parentAnimator.speed;

            // 애니메이터 속도를 0으로 설정하여 애니메이션 일시 정지
            parentAnimator.speed = 0;

            // 지정된 시간 동안 대기 (실제 시간 기준)
            yield return new WaitForSecondsRealtime(duration);

            // 애니메이터 속도를 원래 속도로 복원
            parentAnimator.speed = originalSpeed;

            isRecoilActive = false;
        }
    }
}
