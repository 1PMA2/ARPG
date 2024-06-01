using PlayerController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitState : BaseState
{
    float hitTime = 0f;
    float maxHitTime = 0.2f;
    public HitState(UnitController controller) : base(controller)
    {

    }

    public override void OnEnterState()
    {
        controller.UnitInfo.currentState = UnitState.HIT;

        controller.animator.applyRootMotion = false;

        int rand = Random.Range(0, 5);

        switch(rand)
        {
            case 0:
                controller.ChangeAnimation("Hit1", 0f, 1.5f);
                break;
            case 1:
                controller.ChangeAnimation("Hit2", 0f, 1.5f);
                break;
            case 2:
                controller.ChangeAnimation("Hit3", 0f, 1.5f);
                break;
            case 3:
                controller.ChangeAnimation("Hit4", 0f, 1.5f);
                break;
            case 4:
                controller.ChangeAnimation("Hit5", 0f, 1.5f);
                break;
        }

        hitTime = 0f;
        maxHitTime = 0.2f;

        controller.DisableWeaponTrigger();
        controller.DisableSmashTrigger();
        controller.IsAnimationStart();
    }

    public override void OnFixedUpdateState()
    {

    }

    public override void OnUpdateState()
    {
        hitTime += Time.deltaTime;

        if(hitTime >= maxHitTime)
        {
            controller.stateMachine.ChangeState(UnitState.IDLE);
            controller.SetEquip(true);
            return;
        }

    }

    public override void OnLateUpdateState()
    {

    }

    public override void OnExitState()
    {
        controller.SetEquip(true);
    }
}
