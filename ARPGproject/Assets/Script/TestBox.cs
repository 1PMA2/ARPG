using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBox : MonoBehaviour
{
    float hp = 10;

    // Start is called before the first frame update
    void Start()
    {
        hp = 100;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        hp -= damage;
        Debug.Log(hp);

        if (hp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // ���� �׾��� ���� ����
        Destroy(gameObject);
    }

}