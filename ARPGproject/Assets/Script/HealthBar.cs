using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider;
    public Slider easeHealthSlider;
    public float maxHealth = 100;
    public float health;
    private float lerpSpeed = 1f;
    // Start is called before the first frame update

    void Start()
    {
        lerpSpeed = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if(healthSlider.value != health)
        {
            healthSlider.value = health;
        }

        if(healthSlider.value != easeHealthSlider.value)
        {
            easeHealthSlider.value = Mathf.Lerp(easeHealthSlider.value, health, Time.deltaTime * lerpSpeed);
        }
    }

    public void TakeDamageHealthBar(float damage)
    {
        health -= damage;
    }

    public void TakeHealHealthBar(float heal)
    {
        health += heal;
    }

    public void SetHealth(float unitHealth)
    {
        healthSlider.maxValue = unitHealth;
        easeHealthSlider.maxValue = unitHealth;
        maxHealth = unitHealth;
        health = maxHealth;
    }
}
