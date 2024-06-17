using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System;
using Random = UnityEngine.Random;

#if UNITY_EDITOR
using static UnityEditor.Progress;
#endif

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private List<GameObject> uiPrefabList;
    private List<Canvas> billboardList = new List<Canvas>();
    private HashSet<ItemData> selectedItems = new HashSet<ItemData>();

    private GameObject itemUI;
    private GameObject levelUpUI;

    float billboardOffset = 2.5f;
    int firstRare = 3;
    // Start is called before the first frame update
    void Start()
    {
        itemUI = Instantiate(uiPrefabList[2]);
        levelUpUI = Instantiate(uiPrefabList[5]);
        ActiveLevelUpUI(false);
        ActiveItemUI(false);
        firstRare = 3;
    }


    private void OnDisable()
    {
        ActiveItemUI(false);
        ActiveLevelUpUI(false);
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

        if(index == 4)
            bar.LevelUpEXP(unitStat);
        else
            bar.SetStat(unitStat);

        return bar;
    }

    public void ActiveItemUI(bool active)
    {
        if(itemUI)
        {
            if (EventSystem.current != null)
            {
                EventSystem.current.SetSelectedGameObject(null);
            }
            itemUI.SetActive(active);          
        }

        if (active)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }

    public event Action OnLevelUpUIClose;
    public void ActiveLevelUpUI(bool active)
    {
        if (levelUpUI)
        {
            if (EventSystem.current != null)
            {
                EventSystem.current.SetSelectedGameObject(null);
            }
            levelUpUI.SetActive(active);
        }

        if (active)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;

        if (!active)
        {
            OnLevelUpUIClose?.Invoke();
        }
    }

    public ItemData SelectRandomItems(ItemData[] allItemDatas)
    {
        bool selected = false;

        ItemData selectedItem = allItemDatas[0];

        while (!selected)
        {

            selectedItem = GetRandomItem(allItemDatas);

            if (!selectedItems.Contains(selectedItem))
            {
                
                if (selectedItem.level < selectedItem.counts.Length) // 강화 완료 안된 아이템
                {
                    selectedItems.Add(selectedItem);
                    selected = true;
                }
                else if (Random.value < 0.3f) // 완료된 아이템은 낮은 확률로 등장
                {
                    selectedItems.Add(selectedItem);
                    selected = true;
                }
            }
        }
        firstRare--;
        return selectedItem;
    }

    private ItemData GetRandomItem(ItemData[] items)
    {
        float commonProbability;
        float rareProbability;

        if (firstRare > 0)
        {
            commonProbability = 0f;
            rareProbability = 1f;
        }
        else
        {
            commonProbability = 0.8f;
            rareProbability = 0.2f;
        }
        
        
        while (true)
        {
            int randIndex = Random.Range(0, items.Length);
            ItemData item = items[randIndex];

            if (item.itemGrade == ItemData.Grade.COMMON && Random.value < commonProbability)
            {
                return item;
            }
            else if (item.itemGrade == ItemData.Grade.RARE && Random.value < rareProbability)
            {
                return item;
            }
        }
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
        firstRare = 3;
        billboardList.Clear();
        selectedItems.Clear();
        Destroy(itemUI);
        Destroy(levelUpUI);
        Destroy(gameObject);
    }



}
