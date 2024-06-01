using PlayerController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : BaseState
{
    private float patrolRadius = 5.0f;
    private float patrolSpeed = 1.0f;
    private float angle;
    private float patrolTime;
    private float maxPatrolTime; // 최대 순찰 시간
    public EnemyPatrol(UnitController controller) : base(controller)
    {

    }

    public override void OnEnterState()
    {
        
        controller.ChangeAnimation("Walk", 0.2f, 1f);
        angle = 0;
        patrolTime = 0;

        maxPatrolTime = Random.Range(1f, 3f);
    }

    public override void OnFixedUpdateState()
    {

    }

    public override void OnUpdateState()
    {

        Patrol();

        patrolTime += Time.deltaTime;

        if (patrolTime >= maxPatrolTime)
        {
          controller.stateMachine.ChangeState(UnitState.ENEMY_MOVE);
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
        angle += patrolSpeed * Time.deltaTime;
        if (angle > 360) angle -= 360;

        Vector3 offset = new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle)) * patrolRadius;
        Vector3 patrolPosition = controller.Player.transform.position + offset;
        // 플레이어와 일정 거리 유지
        Vector3 direction = (patrolPosition - controller.transform.position).normalized;
        controller.characterController.Move(direction * Time.deltaTime * patrolSpeed);
        //controller.transform.LookAt(controller.Player.transform);

        Quaternion targetRotation = Quaternion.LookRotation(controller.Player.transform.position - controller.transform.position);
        controller.transform.rotation = Quaternion.Slerp(controller.transform.rotation, targetRotation, 2f * Time.deltaTime);
    }
}

