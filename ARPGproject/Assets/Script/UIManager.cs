using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using static UnityEditor.Progress;
#endif

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private List<GameObject> uiPrefabList;
    private List<Canvas> billboardList = new List<Canvas>();
    private List<ItemData> selectedItems = new List<ItemData>();

    private GameObject itemUI;

    float billboardOffset = 2.5f;
    // Start is called before the first frame update
    void Start()
    {
        billboardOffset = 2.5f;

        itemUI = Instantiate(uiPrefabList[2]);
        ActiveItemUI(false);
    }

    private void OnDisable()
    {
        ActiveItemUI(false);
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

    public EaseBar CreateBar(int index, float unitStat, Transform transform)
    {
        GameObject obj;
        obj = Instantiate(uiPrefabList[index], transform);


        EaseBar bar = obj.GetComponent<EaseBar>();
        bar.SetStat(unitStat);

        return bar;
    }

    public void ActiveItemUI(bool active)
    {
        if(itemUI)
            itemUI.SetActive(active);

        if (active)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }
    public ItemData SelectRandomItems(ItemData[] allItemDatas)
    {
        int rand = 0;

        bool selected = false;

        while (!selected)
        {
            rand = Random.Range(0, allItemDatas.Length);

            if (!selectedItems.Contains(allItemDatas[rand]))
            {
                if (allItemDatas[rand].level < allItemDatas[rand].counts.Length)
                {
                    selectedItems.Add(allItemDatas[rand]);
                    selected = true;
                }
                else if (Random.value < 0.3f) //완료된 아이템은 낮은확률로 등장 //수정할것
                {
                    selectedItems.Add(allItemDatas[rand]);
                    selected = true;
                }
            }
        }

        return allItemDatas[rand];
    }

    public void ClearItemList()
    {
        selectedItems.Clear();
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
        selectedItems.Clear();
        Destroy(itemUI);
        Destroy(gameObject);
    }



}
