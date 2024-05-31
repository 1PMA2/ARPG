using PlayerController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBox : MonoBehaviour
{

    private HealthBar healthBar;
    float hp = 100;

    void Start()
    {
        healthBar = UIManager.Instance.CreateHpBar(transform, 0.005f, hp);
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
        hp -= damage;

        if (hp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

}
