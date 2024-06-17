using Controller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : UnitController
{
    protected override void Awake()
    {
        base.Awake();
        InitStateMachine();
    }
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    protected override void OnEnable()
    {
        weaponTrigger.enabled = false;
        stateMachine?.ChangeState(UnitState.ENEMY_IDLE);
        nearUnitTransform = null;

        base.OnEnable();
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();
    }

    private void InitStateMachine()
    {
        stateMachine = new StateMachine(UnitState.ENEMY_IDLE, new EnemyIdle(this));
        stateMachine.AddState(UnitState.ENEMY_PATROL, new EnemyPatrol(this));
        stateMachine.AddState(UnitState.ENEMY_ATTACK, new EnemyAttack(this));
        stateMachine.AddState(UnitState.ENEMY_MOVE, new EnemyMove(this));
    }


}
