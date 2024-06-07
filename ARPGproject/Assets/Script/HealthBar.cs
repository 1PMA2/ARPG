using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider easeHealthSlider;
    [SerializeField] private Slider restoreSlider;
    public float maxHealth = 100;
    public float health;
    private float lerpSpeed = 1f;
    [SerializeField] private RectTransform[] rectTransforms;
    // Start is called before the first frame update

    void Start()
    {
        lerpSpeed = 1f;

        healthSlider.interactable = false;
        easeHealthSlider.interactable = false;
        restoreSlider.interactable = false;
    }



    // Update is called once per frame
    void Update()
    {
        if(healthSlider.value > health)//데미지
        {
            healthSlider.value = health;
            restoreSlider.value = health;
        }
        else
        {
            healthSlider.value = Mathf.Lerp(healthSlider.value, health, Time.deltaTime * lerpSpeed);
            restoreSlider.value = health;
        }

        if(healthSlider.value != easeHealthSlider.value)//데미지
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

        if (health > maxHealth)
            health = maxHealth;
    }

    public void SetHealth(float unitHealth)
    {
        healthSlider.maxValue = unitHealth;
        easeHealthSlider.maxValue = unitHealth;
        restoreSlider.maxValue = unitHealth;
        maxHealth = unitHealth;
        health = maxHealth;

        healthSlider.value = health;
    }

    public void LevelUp(float unitHealth)
    {
        healthSlider.maxValue = unitHealth;
        easeHealthSlider.maxValue = unitHealth;
        restoreSlider.maxValue = unitHealth;
        maxHealth = unitHealth;
        health = maxHealth;
        healthSlider.value = health;

        Vector3 currentScale = rectTransforms[0].localScale;

        foreach(RectTransform rectTransform in rectTransforms)
        {
            rectTransform.localScale = new Vector3(currentScale.x + 0.6f, currentScale.y, currentScale.z);
        }
    }
}
