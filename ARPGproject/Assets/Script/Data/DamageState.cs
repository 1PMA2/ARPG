using Controller;
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
    public static readonly string EnemyTag = "Enemy";
    public static readonly string PlayerTag = "Player";
    public static readonly string PlayerCameraTag = "PlayerCamera";
    public static readonly string BossTag = "MultiColliderEnemy";

    public static readonly float recoilDuration = 0.2f;
    public static readonly float smashStamina = 20f;
    public static readonly float evadeStamina = 10f;
    public static readonly float comboStamina = 5f;
    private static readonly float smashRatio = 1.5f;
    private static readonly float comboRatio = 1.2f;


    public static void SetState(Collider other, Recoil recoil, UnitInformation enemyInformation, BoxCollider boxCollider) //적이 나를 때릴때
    {
        if (other.CompareTag(PlayerTag))
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
                        player.TakeDamage(enemyInformation.Damage);
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
                            player.TakeDamage(enemyInformation.Damage);
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


    public static void SmashAttack(Collider other, Recoil recoil, UnitInformation unitInformation, UnitController unit, Transform transform, float drain) //스매시, 카운터스매시
    {
        if (other.CompareTag(EnemyTag))
        {
            recoil.StartRecoil(recoilDuration * unitInformation.RecoilPower);

            TestBox enemy = other.gameObject.GetComponent<TestBox>();

            if (enemy != null)
            {
                CameraManager.Instance.ShakeCamera(PlayerCameraTag, 0.2f, 0.1f);

                Vector3 collisionPoint = other.ClosestPoint(transform.position);

                Vector3 enemyCenter = other.bounds.center;

                Vector3 direction = (enemyCenter - collisionPoint).normalized;


                EffectManager.Instance.GetEffect(0, enemyCenter, Quaternion.LookRotation(direction), 1f);

                if (enemy.TakeDamage(unitInformation.Damage * smashRatio))
                {
                    unit.GetComponent<TestBox>().TakeEXP(other.gameObject.GetComponent<UnitInformation>().Exp);
                }
                

                if (unitInformation.Lightninig > 0)
                    EffectManager.Instance.GetEffect(4, new Vector3(collisionPoint.x, 0, collisionPoint.z), Quaternion.identity, 2f);

                if ((unitInformation.currentState == UnitState.COUNTER) && unitInformation.Drain > 0)
                    unit.GetComponent<TestBox>().Heal(drain);
            }
        }
    }

    public static void SmashAttack(MultiColliderManager manager, Collider weapon, Recoil recoil, UnitInformation unitInformation, UnitController unit, Transform transform, float drain) //스매시, 카운터스매시
    {
        if (manager.CompareTag(BossTag))
        {
            recoil.StartRecoil(recoilDuration * unitInformation.RecoilPower);

            TestBox enemy = manager.gameObject.GetComponent<TestBox>();

            if (enemy != null)
            {
                CameraManager.Instance.ShakeCamera(PlayerCameraTag, 0.2f, 0.1f);

                Vector3 collisionPoint = weapon.ClosestPoint(manager.transform.position);

                Vector3 enemyCenter = new Vector3(manager.colliders[1].bounds.center.x, 0, manager.colliders[1].bounds.center.z);

                Vector3 direction = (collisionPoint - enemyCenter).normalized;


                EffectManager.Instance.GetEffect(0, enemyCenter, Quaternion.LookRotation(direction), 1f);

                if (enemy.TakeDamage(unitInformation.Damage * smashRatio))
                {
                    unit.GetComponent<TestBox>().TakeEXP(manager.gameObject.GetComponent<UnitInformation>().Exp);
                }


                if (unitInformation.Lightninig > 0)
                    EffectManager.Instance.GetEffect(4, new Vector3(collisionPoint.x, 0, collisionPoint.z), Quaternion.identity, 2f);

                if ((unitInformation.currentState == UnitState.COUNTER) && unitInformation.Drain > 0)
                    unit.GetComponent<TestBox>().Heal(drain);
            }
        }
    }

    public static void Attack(Collider other, Recoil recoil, UnitInformation unitInformation, Transform transform)
    {
        if (other.CompareTag(EnemyTag))
        {
            recoil.StartRecoil(recoilDuration * unitInformation.RecoilPower);

            TestBox enemy = other.gameObject.GetComponent<TestBox>();
            TestBox player = unitInformation.gameObject.GetComponent<TestBox>();

            if (enemy != null)
            {
                CameraManager.Instance.ShakeCamera(PlayerCameraTag, 0.2f, 0.1f);

                Vector3 collisionPoint = other.ClosestPoint(transform.position);

                Vector3 enemyCenter = other.bounds.center;

                Vector3 direction = (enemyCenter - collisionPoint).normalized;


                EffectManager.Instance.GetEffect(0, collisionPoint, Quaternion.LookRotation(direction), 1f);

                

                if(unitInformation.currentState == UnitState.COMBO_03)
                {
                    player.RestoreStmina(comboStamina * 2f);
                    if(enemy.TakeDamage(unitInformation.Damage * comboRatio))
                    {
                        player.TakeEXP(other.gameObject.GetComponent<UnitInformation>().Exp);
                    }
                }
                else
                {
                    player.RestoreStmina(comboStamina);
                    if(enemy.TakeDamage(unitInformation.Damage))
                    {
                        player.TakeEXP(other.gameObject.GetComponent<UnitInformation>().Exp);
                    }
                }


                if (unitInformation.Lightninig > 0)
                    EffectManager.Instance.GetEffect(4, new Vector3(collisionPoint.x, 0, collisionPoint.z), Quaternion.identity, 2f);
            }

        }
    }

    public static void Attack(MultiColliderManager manager, Collider weapon, Recoil recoil, UnitInformation unitInformation, Transform transform)
    {
        if (manager.CompareTag(BossTag))
        {
            recoil.StartRecoil(recoilDuration * unitInformation.RecoilPower);

            TestBox enemy = manager.gameObject.GetComponent<TestBox>();
            TestBox player = unitInformation.gameObject.GetComponent<TestBox>();

            if (enemy != null)
            {
                CameraManager.Instance.ShakeCamera(PlayerCameraTag, 0.2f, 0.1f);

                Vector3 collisionPoint = weapon.ClosestPoint(manager.transform.position);

                Vector3 enemyCenter = new Vector3(manager.colliders[1].bounds.center.x, 0, manager.colliders[1].bounds.center.z);

                Vector3 direction = (collisionPoint - enemyCenter).normalized;

                EffectManager.Instance.GetEffect(0, collisionPoint, Quaternion.LookRotation(direction), 1f);


                if (unitInformation.currentState == UnitState.COMBO_03)
                {
                    player.RestoreStmina(comboStamina * 2f);
                    if (enemy.TakeDamage(unitInformation.Damage * comboRatio))
                    {
                        player.TakeEXP(manager.gameObject.GetComponent<UnitInformation>().Exp);
                    }
                }
                else
                {
                    player.RestoreStmina(comboStamina);
                    if (enemy.TakeDamage(unitInformation.Damage))
                    {
                        player.TakeEXP(manager.gameObject.GetComponent<UnitInformation>().Exp);
                    }
                }


                if (unitInformation.Lightninig > 0)
                    EffectManager.Instance.GetEffect(4, new Vector3(collisionPoint.x, 0, collisionPoint.z), Quaternion.identity, 2f);
            }

        }
    }



}