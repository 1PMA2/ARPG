using PlayerController;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
#if UNITY_EDITOR
using UnityEditor.PackageManager;
#endif

public static class DamageState
{
    private static readonly float recoilDuration = 0.2f;
    public static readonly float smashStamina = 20f;
    public static readonly float evadeStamina = 10f;
    public static readonly float comboStamina = 5f;


    public static void SetState(Collider other, Recoil recoil, UnitInformation unitInformation, BoxCollider boxCollider) //적이 나를 때릴때
    {
        if (other.CompareTag("Player"))
        {
            boxCollider.enabled = false; //다음 물리 업데이트 주기부터 적용 캐릭터는 판정있으니 괜찮음.
            TestBox player = other.gameObject.GetComponent<TestBox>();

            if (player != null)
            {
                UnitInformation playerInfo = other.gameObject.GetComponent<UnitInformation>();
               


                switch (playerInfo.currentState)
                {
                    case UnitState.GUARD_01:
                        EffectManager.Instance.HitEffect(recoil, recoilDuration, other, 2);
                        player.UseStamina(5);
                        break;

                    case UnitState.GUARD_02:
                        break;

                    case UnitState.EVADE:
                        break;

                    case UnitState.SMASH_00:
                    case UnitState.SMASH_01:
                    case UnitState.COUNTER:
                        EffectManager.Instance.HitEffect(recoil, recoilDuration, other, 0);
                        player.TakeDamage(unitInformation.Damage);
                        break;

                    case UnitState.HIT:
                        break;

                    default:
                        if (Random.value < playerInfo.AutoGuard)
                        {
                            EffectManager.Instance.HitEffect(recoil, recoilDuration, other, 2);
                            playerInfo.currentState = UnitState.GUARD_01;
                        }
                        else
                        {
                            EffectManager.Instance.HitEffect(recoil, recoilDuration, other, 0);
                            player.TakeDamage(unitInformation.Damage);
                        }

                        break;
                }
            }
        }
    }

    public static void HitChangeState(Collider other, UnitInformation unitInformation, Transform transform, StateMachine stateMachine) //맞았을때 상태
    {
        if (other.CompareTag("Untagged"))
        {
            switch (unitInformation.currentState)
            {
                case UnitState.GUARD_01:
                    LookAtHit(other, transform);
                    stateMachine.ChangeState(UnitState.GUARD_02);
                    break;
                case UnitState.GUARD_02:
                    break;
                case UnitState.EVADE:
                    break;
                case UnitState.SMASH_00:
                    break;
                case UnitState.SMASH_01:
                    break;
                case UnitState.COUNTER:
                    break;
                default:
                    if (unitInformation.SuperArmor <= 0)
                    {
                        LookAtHit(other, transform);
                        stateMachine.ChangeState(UnitState.HIT);
                    }
                    break;
            }
        }
    }

    private static void LookAtHit(Collider other, Transform transform)
    {
        if (other != null)
        {
            Vector3 direction = other.gameObject.GetComponentInParent<UnitController>().transform.position - transform.position;

            direction.y = 0f;

            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(direction);
            }
        }
    }


    public static void Attack(Collider other, Recoil recoil, UnitInformation unitInformation, UnitController unit, Transform transform, float smashDamage) //스매시, 카운터스매시
    {
        if (other.CompareTag("Enemy"))
        {
            recoil.StartRecoil(recoilDuration * unitInformation.RecoilPower);

            TestBox enemy = other.gameObject.GetComponent<TestBox>();

            if (enemy != null)
            {
                CameraManager.Instance.ShakeCamera("PlayerCamera", 0.2f, 0.1f);

                Vector3 collisionPoint = other.ClosestPoint(transform.position);

                Vector3 enemyCenter = other.bounds.center;

                Vector3 direction = (enemyCenter - collisionPoint).normalized;


                EffectManager.Instance.GetEffect(0, enemyCenter, Quaternion.LookRotation(direction), 1f);

                enemy.TakeDamage(unitInformation.Damage * smashDamage);

                if (unitInformation.Lightninig > 0)
                    EffectManager.Instance.GetEffect(4, new Vector3(collisionPoint.x, 0, collisionPoint.z), Quaternion.identity, 2f);

                if ((unitInformation.currentState == UnitState.COUNTER) && unitInformation.Drain > 0)
                    unit.GetComponent<TestBox>().Heal(1);
            }
        }
    }

    public static void Attack(Collider other, Recoil recoil, UnitInformation unitInformation, Transform transform)
    {
        if (other.CompareTag("Enemy"))
        {
            recoil.StartRecoil(recoilDuration * unitInformation.RecoilPower);

            TestBox enemy = other.gameObject.GetComponent<TestBox>();
            TestBox player = unitInformation.GetComponent<TestBox>();

            if (enemy != null)
            {
                CameraManager.Instance.ShakeCamera("PlayerCamera", 0.2f, 0.1f);

                Vector3 collisionPoint = other.ClosestPoint(transform.position);

                Vector3 enemyCenter = other.bounds.center;

                Vector3 direction = (enemyCenter - collisionPoint).normalized;


                EffectManager.Instance.GetEffect(0, collisionPoint, Quaternion.LookRotation(direction), 1f);

                enemy.TakeDamage(unitInformation.Damage);

                if(unitInformation.currentState == UnitState.COMBO_03)
                    player.RestoreStmina(comboStamina * 2f);
                else
                    player.RestoreStmina(comboStamina);


                if (unitInformation.Lightninig > 0)
                    EffectManager.Instance.GetEffect(4, new Vector3(collisionPoint.x, 0, collisionPoint.z), Quaternion.identity, 2f);
            }

        }
    }

}