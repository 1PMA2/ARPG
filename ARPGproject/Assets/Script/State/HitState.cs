using Controller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitState : PlayerState
{
    float hitTime = 0f;
    float maxHitTime = 0.2f;
    public HitState(Player controller) : base(controller)
    {

    }

    public override void OnEnterState()
    {
        playerController.UnitInfo.currentState = UnitState.HIT;
        int rand = Random.Range(0, 5);
        switch(rand)
        {
            case 0:
                playerController.ChangeAnimation("Hit1", 0f, 1.5f);
                break;
            case 1:
                playerController.ChangeAnimation("Hit2", 0f, 1.5f);
                break;
            case 2:
                playerController.ChangeAnimation("Hit3", 0f, 1.5f);
                break;
            case 3:
                playerController.ChangeAnimation("Hit4", 0f, 1.5f);
                break;
            case 4:
                playerController.ChangeAnimation("Hit5", 0f, 1.5f);
                break;
        }
        playerController.IsAnimationStart();
        playerController.DisableWeaponTrigger();
        playerController.DisableSmashTrigger();
        playerController.animator.applyRootMotion = true;
        hitTime = 0f;
        maxHitTime = 0.2f;

        
    }

    public override void OnFixedUpdateState()
    {

    }

    public override void OnUpdateState()
    {
        hitTime += Time.deltaTime;

        if(hitTime >= maxHitTime)
        {
            playerController.animator.applyRootMotion = false;
            playerController.stateMachine.ChangeState(UnitState.IDLE);
            playerController.SetEquip(true);
            return;
        }

    }

    public override void OnLateUpdateState()
    {

    }

    public override void OnExitState()
    {
        playerController.SetEquip(true);
    }
}
