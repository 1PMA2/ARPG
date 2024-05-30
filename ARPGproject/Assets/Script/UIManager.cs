using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private List<GameObject> uiPrefabList;
    private List<Canvas> billboardList = new List<Canvas>();

    float billboardOffset = 2.5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        SetBillboard();
    }

    public HealthBar CreateHpBar(Transform transform, float scale, float unitHealth)
    {
        GameObject obj = Instantiate(uiPrefabList[0], transform);
            
        obj.transform.localScale = new Vector3(scale, scale, scale);

        billboardList.Add(obj.GetComponent<Canvas>());

        HealthBar healthBar = obj.GetComponent<HealthBar>();

        healthBar.SetHealth(unitHealth);

        return healthBar;
    }

    private void SetBillboard()
    {
        foreach (Canvas obj in billboardList)
        {
            if(obj != null)
            {
                obj.transform.position = new Vector3(obj.transform.position.x, billboardOffset, obj.transform.position.z);

                obj.transform.rotation = CameraManager.Instance.GetCamera("PlayerCamera").transform.rotation;
            }
           
        }
    }
}
