using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Controller;

public class Recoil : MonoBehaviour
{
    private Animator animator;
    private UnitController controller;
    // Start is called before the first frame update
    private bool isRecoilActive = false;
    void Start()
    {
        animator = GetComponent<Animator>();
        controller= GetComponent<UnitController>();
    }
 
    private IEnumerator PauseAnimation(float duration)
    {
        if (animator != null)
        {
            isRecoilActive = true;
            // ���� �ִϸ����� �ӵ��� ����
            float originalSpeed = animator.speed;
            float originaSmashlSpeed = controller.smashSpeed;

            // �ִϸ����� �ӵ��� 0���� �����Ͽ� �ִϸ��̼� �Ͻ� ����
            controller.smashSpeed = 0;
            animator.speed = 0;

            // ������ �ð� ���� ��� (���� �ð� ����)
            yield return YieldCache.WaitForSecondsRealTime(duration);

            // �ִϸ����� �ӵ��� ���� �ӵ��� ����
            animator.speed = originalSpeed;
            controller.smashSpeed = originaSmashlSpeed;

            isRecoilActive = false;
        }
    }

    public void StartRecoil(float recoilDuration)
    {
        if (!isRecoilActive)
            StartCoroutine(PauseAnimation(recoilDuration));
    }


}
