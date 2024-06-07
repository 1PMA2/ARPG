using PlayerController;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public ItemData[] allItemDatas; // ��� ������ ������ �迭
    private List<ItemData> selectedItems = new List<ItemData>(); // ���õ� ������ ������ �迭
    ItemData currentItemData;

    public UnitInformation unitInformation;
    private RectTransform rectTransform;

    Image icon;
    public TMP_Text textLevel;

    private void Awake()
    {
        icon = GetComponentsInChildren<Image>()[1];

        rectTransform = GetComponentsInChildren<RectTransform>()[2];
        TMP_Text[] texts = GetComponentsInChildren<TMP_Text>();
        textLevel = texts[0]; 

        for(int i = 0; i < allItemDatas.Length; ++i)
        {
            allItemDatas[i].level = 0;
        }
    }

    private void OnEnable()
    {
        currentItemData = UIManager.Instance.SelectRandomItems(allItemDatas);

        UpdateItem(); // �ʱ� ������ ����
    }

    private void OnDisable()
    {
        UIManager.Instance.ClearItemList();
    }



    private void UpdateItem()
    {
        icon.sprite = currentItemData.itemIcon;
        rectTransform.sizeDelta = new Vector2(currentItemData.itemIcon.textureRect.width, currentItemData.itemIcon.textureRect.height);

        if (currentItemData.level < currentItemData.counts.Length)
        {
            GetComponent<Button>().interactable = true;

            switch (currentItemData.itemType)
            {
                case ItemData.ItemType.Counter:
                    textLevel.text = "ī���Ͱ� " + currentItemData.counts[currentItemData.level] + "ȸ ���������ϴ�.";
                    break;     
                case ItemData.ItemType.Recoil:
                    textLevel.text = "�������� " + ((1 - currentItemData.counts[currentItemData.level]) * 100) + "% �����մϴ�.";
                    break;
                case ItemData.ItemType.Thunder:
                    textLevel.text = "���� ���� �� ������ ����Ĩ�ϴ�.";
                    break;
                case ItemData.ItemType.SuperArmor:
                    textLevel.text = "������ ���Ͽ� �鿪�� �˴ϴ�.";
                    break;
                case ItemData.ItemType.AutoGuard:
                    textLevel.text = (currentItemData.counts[currentItemData.level] * 100) + "% Ȯ���� �ڵ� ����մϴ�.";
                    break;
                case ItemData.ItemType.Drain:
                    textLevel.text = "ī���� ���� �� ü���� ȸ���մϴ�.";
                    break;
                case ItemData.ItemType.CombatBreathing:
                    textLevel.text = "���׹̳� ȸ������ " + ((currentItemData.counts[currentItemData.level] - 1) * 100) + "% �����մϴ�.";
                    break;
            }
        }
        else
        {
            GetComponent<Button>().interactable = false;
            textLevel.text = "��ȭ�Ϸ�";
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
}
