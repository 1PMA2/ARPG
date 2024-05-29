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
            // ���� �ִϸ����� �ӵ��� ����
            float originalSpeed = parentAnimator.speed;

            // �ִϸ����� �ӵ��� 0���� �����Ͽ� �ִϸ��̼� �Ͻ� ����
            parentAnimator.speed = 0;

            // ������ �ð� ���� ��� (���� �ð� ����)
            yield return new WaitForSecondsRealtime(duration);

            // �ִϸ����� �ӵ��� ���� �ӵ��� ����
            parentAnimator.speed = originalSpeed;

            isRecoilActive = false;
        }
    }
}
