using PlayerController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBox : MonoBehaviour
{

    private HealthBar healthBar;
    private UnitInformation unitInformation;
    private float maxHealth;

    void Start()
    {
        unitInformation = GetComponent<UnitInformation>();
        maxHealth = unitInformation.Health;
        healthBar = UIManager.Instance.CreateHpBar(transform, 0.005f, unitInformation.Health);
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

    private void Die()
    {
        if(!unitInformation.IsPlayer)
        {
            EffectManager.Instance.GetEffect(3, new Vector3(transform.position.x, 1.25f, transform.position.z), Quaternion.identity, 2f);
            MonsterManager.Instance.ReturnMonster(gameObject);
        }
    }

}
