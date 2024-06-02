using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private List<GameObject> uiPrefabList;
    private List<Canvas> billboardList = new List<Canvas>();

    float billboardOffset = 2.5f;
    // Start is called before the first frame update
    void Start()
    {
        billboardOffset = 2.5f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        SetBillboard();
    }

    public HealthBar CreateHpBar(Transform transform, float scale, float unitHealth, bool isPlayer = false)
    {
        GameObject obj;

        if (isPlayer)
        {
            obj = Instantiate(uiPrefabList[1], transform);

            Transform overlayTransform = obj.GetComponentInChildren<Transform>();


           
        }
        else
        {
            obj = Instantiate(uiPrefabList[0], transform);
            billboardList.Add(obj.GetComponent<Canvas>());

            obj.transform.localScale = new Vector3(scale, scale, scale);
        }

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

    public void Restart()
    {
        billboardList.Clear();
        Destroy(gameObject);
    }
}
