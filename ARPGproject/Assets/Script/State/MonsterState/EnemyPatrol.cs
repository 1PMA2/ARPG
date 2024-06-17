using Controller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : SkeletonState
{
    private float patrolRadius = 5.0f;
    private float patrolSpeed = 1.0f;
    private float angle;
    private float patrolTime;
    private float maxPatrolTime; // 최대 순찰 시간
    public EnemyPatrol(Skeleton controller) : base(controller)
    {

    }

    public override void OnEnterState()
    {
        skeletonController.ChangeAnimation("Walk", 0.2f, 1f);
        angle = 0;
        patrolTime = 0;

        maxPatrolTime = Random.Range(1f, 3f);
    }

    public override void OnFixedUpdateState()
    {

    }

    public override void OnUpdateState()
    {
        if (skeletonController.nearUnitTransform == null)
        {
            skeletonController.stateMachine.ChangeState(UnitState.ENEMY_IDLE);
            return;
        }
        else
            Patrol();

        patrolTime += Time.deltaTime;

        if (patrolTime >= maxPatrolTime)
        {
            skeletonController.stateMachine.ChangeState(UnitState.ENEMY_MOVE);
        }

    }

    public override void OnLateUpdateState()
    {

    }

    public override void OnExitState()
    {
        
    }

    private void Patrol()
    {
        if (skeletonController.nearUnitTransform != null)
        {
            angle += patrolSpeed * Time.deltaTime;
            if (angle > 360) angle -= 360;
            Vector3 offset = new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle)) * patrolRadius;
            Vector3 patrolPosition = skeletonController.nearUnitTransform.position + offset;
            // 플레이어와 일정 거리 유지
            Vector3 direction = (patrolPosition - skeletonController.transform.position).normalized;
            skeletonController.characterController.Move(direction * Time.deltaTime * patrolSpeed);

            Quaternion targetRotation = Quaternion.LookRotation(skeletonController.nearUnitTransform.position - skeletonController.transform.position);
            skeletonController.transform.rotation = Quaternion.Slerp(skeletonController.transform.rotation, targetRotation, 2f * Time.deltaTime);
        }
    }
}

