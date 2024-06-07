using PlayerController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBox : MonoBehaviour
{

    private HealthBar healthBar;
    private EaseBar staminaBar;
    private EaseBar expBar;
    private UnitInformation unitInformation;
    private float maxHealth;
    private float maxStamina;

    private Coroutine restoreCoroutine = null;

    void Start()
    {
        unitInformation = GetComponent<UnitInformation>();
        maxHealth = unitInformation.Health;
        healthBar = UIManager.Instance.CreateHpBar(transform, 0.005f, unitInformation.Health, unitInformation.IsPlayer);

        if(unitInformation.IsPlayer)
        {
            maxStamina = unitInformation.Stamina;
            staminaBar = UIManager.Instance.CreateBar(3, unitInformation.Stamina, transform);
            expBar = UIManager.Instance.CreateBar(4, unitInformation.Exp, transform);
        }
        
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        if(unitInformation != null)
            unitInformation.Health = maxHealth;

        if(healthBar != null)
            healthBar.SetHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            if(unitInformation.IsPlayer)
            {
                maxStamina += 20;
                unitInformation.Stamina = maxStamina;
                staminaBar.LevelUp(maxStamina);
            }
        }
    }

    void LateUpdate()
    {

    }

    public void TakeDamage(float damage)
    {
        healthBar.TakeDamageHealthBar(damage);

        unitInformation.Health -= damage;

        if (unitInformation.Health <= 0)
        {
            Die();
        }
    }

    public void Heal(float heal)
    {
        if (unitInformation.Health < maxHealth)
        {
            healthBar.TakeHealHealthBar(heal);
            unitInformation.Health += heal;
        }

    }

    public void UseStamina(float usage)
    {
        staminaBar.TakeDamageStat(usage);

        unitInformation.Stamina -= usage;

        if (unitInformation.Stamina <= 0)
        {
            unitInformation.Stamina = 0;
        }
    }

    public bool AbleStamina(float usage)
    {
        if(unitInformation.Stamina >= usage)
        {
            return true;
        }

        return false;
    }

    public void RestoreStmina(float restore)
    {
        staminaBar.TakeHealStat(restore);
        unitInformation.Stamina += restore;

        if (maxStamina <= unitInformation.Stamina)
            unitInformation.Stamina = maxStamina;
    }
    public void RestoreStmina(float restore, float frequency)
    {
        if (restoreCoroutine == null)
        {
            restoreCoroutine = StartCoroutine(Restore(restore, frequency));
        }
    }


    public void StopRestore()
    {
        if (restoreCoroutine != null)
        {
            StopCoroutine(restoreCoroutine);
            restoreCoroutine = null;
        }
    }

    IEnumerator Restore(float restore, float frequency)
    {
        while (maxStamina > unitInformation.Stamina)
        {
            staminaBar.TakeHealStat(restore);
            unitInformation.Stamina += restore;
            yield return YieldCache.WaitForSeconds(frequency);
        }
        restoreCoroutine = null;
    }

    private void Die()
    {
        if(!unitInformation.IsPlayer)
        {
            EffectManager.Instance.GetEffect(3, new Vector3(transform.position.x, 1.25f, transform.position.z), Quaternion.identity, 2f);
            MonsterManager.Instance.ReturnMonster(gameObject);
        }
    }

}
