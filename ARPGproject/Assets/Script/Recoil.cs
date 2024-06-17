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
            // 현재 애니메이터 속도를 저장
            float originalSpeed = animator.speed;
            float originaSmashlSpeed = controller.smashSpeed;

            // 애니메이터 속도를 0으로 설정하여 애니메이션 일시 정지
            controller.smashSpeed = 0;
            animator.speed = 0;

            // 지정된 시간 동안 대기 (실제 시간 기준)
            yield return YieldCache.WaitForSecondsRealTime(duration);

            // 애니메이터 속도를 원래 속도로 복원
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
