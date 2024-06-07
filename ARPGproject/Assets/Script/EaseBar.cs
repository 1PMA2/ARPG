using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayerController;
using UnityEditor;

public class EaseBar : MonoBehaviour
{
    public Slider statSlider;
    public float maxStat = 100;
    public float stat;
    [SerializeField] private float lerpSpeed = 3f;
    [SerializeField] private RectTransform rectTransform;
    // Start is called before the first frame update

    void Start()
    {
        lerpSpeed = 3f;
    }



    // Update is called once per frame
    void Update()
    {
        if(statSlider.value > stat)//µ¥¹ÌÁö
        {
            statSlider.value = stat;
        }
        else
        {
            statSlider.value = Mathf.Lerp(statSlider.value, stat, Time.deltaTime * lerpSpeed);
        }


    }

    public void TakeDamageStat(float damage)
    {
        stat -= damage;

        if(stat < 0)
            stat = 0;
    }

    public void TakeHealStat(float heal)
    {
        stat += heal;

        if(stat > maxStat)
            stat = maxStat;
    }

    public void SetStat(float unitstat)
    {
        statSlider.maxValue = unitstat;

        maxStat = unitstat;
        stat = maxStat;

        statSlider.value = stat;
    }

    public void LevelUp(float unitstat)
    {
        statSlider.maxValue = unitstat;
        maxStat = unitstat;
        stat = maxStat;
        statSlider.value = stat;

        Vector3 currentScale = rectTransform.localScale;
        rectTransform.localScale = new Vector3(currentScale.x + 0.6f, currentScale.y, currentScale.z);
    }
}
