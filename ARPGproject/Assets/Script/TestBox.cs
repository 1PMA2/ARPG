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
            expBar = UIManager.Instance.CreateBar(4, unitInformation.MaxExp, transform);
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
                
            }
        }
    }

    void LateUpdate()
    {

    }

    public bool TakeDamage(float damage)
    {
        healthBar.TakeDamageHealthBar(damage);

        unitInformation.Health -= damage;

        if (unitInformation.Health <= 0)
        {
            Die();
            return true;
        }

        return false;
    }

    public void TakeEXP(float exp)
    {
        expBar.TakeEXP(exp);
        unitInformation.Exp += exp;

        if (unitInformation.Exp >= unitInformation.MaxExp)
        {
            unitInformation.Level += 1;
            UIManager.Instance.ActiveLevelUpUI(true);
            unitInformation.Exp = unitInformation.Exp - unitInformation.MaxExp;

            unitInformation.MaxExp = (int)(0.25f * unitInformation.MaxExp * unitInformation.MaxExp);
            expBar.LevelUpEXP(unitInformation.MaxExp);
        }
    }

    public void Heal(float heal)
    {
        healthBar.TakeHealHealthBar(heal);
        unitInformation.Health += heal;

        if (unitInformation.Health > maxHealth)
            unitInformation.Health = maxHealth;
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
        staminaBar.TakeHealStat(restore * unitInformation.CombatBreathing);
        unitInformation.Stamina += restore * unitInformation.CombatBreathing;

        if (maxStamina <= unitInformation.Stamina)
            unitInformation.Stamina = maxStamina;
    }
    public void RestoreStmina(float restore, float frequency)
    {
        if (restoreCoroutine == null)
        {
            restoreCoroutine = StartCoroutine(Restore(restore * unitInformation.CombatBreathing, frequency));
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

            if (maxStamina <= unitInformation.Stamina)
                unitInformation.Stamina = maxStamina;

            yield return YieldCache.WaitForSeconds(frequency);
        }

        unitInformation.Stamina = maxStamina;
        restoreCoroutine = null;
    }
    public void MaxHealthUp()
    {
        maxHealth += 10;
        unitInformation.Health = maxHealth;
        healthBar.LevelUp(maxHealth);
    }
    public void MaxStaminaUp()
    {
        maxStamina += 20;
        unitInformation.Stamina = maxStamina;
        staminaBar.LevelUpStat(maxStamina);
    }  
    
    public void DamageUp()
    {
        unitInformation.Damage += 1f;
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
