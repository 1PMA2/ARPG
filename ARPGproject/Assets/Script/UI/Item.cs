using Controller;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Item : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public ItemData[] allItemDatas; // 모든 아이템 데이터 배열
    ItemData currentItemData;
    public ItemData CurrentItemData { get { return currentItemData; } }

    public UnitInformation unitInformation;
    private RectTransform rectTransform;

    [SerializeField] private Material rareMaterial;
    private Material instanceMaterial;
    private Image icon;
    private Button button;
    private Image buttonImage;
    public TMP_Text textLevel;

    private void Awake()
    {
        icon = GetComponentsInChildren<Image>()[1];
        button = GetComponent<Button>();
        buttonImage = button.GetComponent<Image>();
        rectTransform = GetComponentsInChildren<RectTransform>()[2];

        TMP_Text[] texts = GetComponentsInChildren<TMP_Text>();
        textLevel = texts[0];
        for (int i = 0; i < allItemDatas.Length; ++i)
        {
            allItemDatas[i].level = 0;
        }

        instanceMaterial = Instantiate(rareMaterial);
    }

    private void Start()
    {
        
    }



    private void OnEnable()
    {
        currentItemData = UIManager.Instance.SelectRandomItems(allItemDatas);

        UpdateItem(); // 초기 아이템 설정

        
    }

    private void OnDisable()
    {
        UIManager.Instance.ClearItemList();
    }



    private void UpdateItem()
    {
        if (currentItemData.itemGrade == ItemData.Grade.RARE)
            buttonImage.material = rareMaterial;
        else
            buttonImage.material = null;

        icon.sprite = currentItemData.itemIcon;
        rectTransform.sizeDelta = new Vector2(currentItemData.itemIcon.textureRect.width, currentItemData.itemIcon.textureRect.height);

        if (currentItemData.level < currentItemData.counts.Length)
        {
            GetComponent<Button>().interactable = true;

            switch (currentItemData.itemType)
            {
                case ItemData.ItemType.Counter:
                    textLevel.text = "카운터가 " + currentItemData.counts[currentItemData.level] + "회 가능해집니다.";
                    break;     
                case ItemData.ItemType.Recoil:
                    textLevel.text = "역경직이 " + ((1 - currentItemData.counts[currentItemData.level]) * 100) + "% 감소합니다.";
                    break;
                case ItemData.ItemType.Thunder:
                    textLevel.text = "공격 성공 시 번개가 내려칩니다.";
                    break;
                case ItemData.ItemType.SuperArmor:
                    textLevel.text = "경직에 대하여 면역이 됩니다.";
                    break;
                case ItemData.ItemType.AutoGuard:
                    textLevel.text = (currentItemData.counts[currentItemData.level] * 100) + "% 확률로 자동 방어합니다.";
                    break;
                case ItemData.ItemType.Drain:
                    textLevel.text = "카운터 공격 시 체력을 회복합니다.";
                    break;
                case ItemData.ItemType.CombatBreathing:
                    textLevel.text = "스테미나 회복량이 " + ((currentItemData.counts[currentItemData.level] - 1) * 100) + "% 증가합니다.";
                    break;
            }
        }
        else
        {
            button.interactable = false;
            textLevel.text = "강화완료";
        }
    }

    private void Update()
    {
        if (buttonImage.material != null)
        {
            buttonImage.material.SetFloat("_UnscaledTime", Time.unscaledTime);
        }
    }



    public void OnClick()
    {
        
        switch (currentItemData.itemType)
        {
            case ItemData.ItemType.Counter:
                DungeonGenerator.Instance.playerInfo.MaxCounter = (int)currentItemData.counts[currentItemData.level];
                break;
            case ItemData.ItemType.Recoil:
                DungeonGenerator.Instance.playerInfo.RecoilPower = currentItemData.counts[currentItemData.level];
                break;
            case ItemData.ItemType.Thunder:
                DungeonGenerator.Instance.playerInfo.Lightninig = (int)currentItemData.counts[currentItemData.level];
                break;
            case ItemData.ItemType.SuperArmor:
                DungeonGenerator.Instance.playerInfo.SuperArmor = (int)currentItemData.counts[currentItemData.level];
                break;
            case ItemData.ItemType.AutoGuard:
                DungeonGenerator.Instance.playerInfo.AutoGuard = currentItemData.counts[currentItemData.level];
                break;
            case ItemData.ItemType.Drain:
                DungeonGenerator.Instance.playerInfo.Drain = (int)currentItemData.counts[currentItemData.level];
                break;
            case ItemData.ItemType.CombatBreathing:
                DungeonGenerator.Instance.playerInfo.CombatBreathing = currentItemData.counts[currentItemData.level];
                break;
            default:
                break;
        }
        

       
        currentItemData.level++;
        UIManager.Instance.ActiveItemUI(false);

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if ((currentItemData.itemGrade == ItemData.Grade.RARE) && button.interactable)
        {
            buttonImage.material = instanceMaterial;
            StartCoroutine(TransitionColor(1));
        }
    }

    // Called when the pointer exits the button area
    public void OnPointerExit(PointerEventData eventData)
    {
        if ((currentItemData.itemGrade == ItemData.Grade.RARE) && button.interactable)
        {
            StartCoroutine(TransitionColor(0));
        }
    }

    private IEnumerator TransitionColor(float targetLerp)
    {
        float elapsedTime = 0f;
        float initialLerp = instanceMaterial.GetFloat("_Lerp");

        while (elapsedTime < 0.1f)
        {
            float lerp = Mathf.Lerp(initialLerp, targetLerp, elapsedTime / 0.1f);
            instanceMaterial.SetFloat("_Lerp", lerp);
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        instanceMaterial.SetFloat("_Lerp", targetLerp);
    }
}
