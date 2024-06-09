using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MouseOver : MonoBehaviour
{
    private Button button;
    private Image buttonImage;
    public Color highlightedColor = Color.yellow;
    public Color normalColor = Color.white;

    [SerializeField] private Material prismaticMaterial;
    private Material instanceMaterial;
    private ItemData itemData;

    void Start()
    {
        button = GetComponent<Button>();
        buttonImage = button.GetComponent<Image>();

        // ���� ��Ƽ������ �����Ͽ� �ν��Ͻ��� �����մϴ�.
        
    }

    private void OnEnable()
    {
        itemData = GetComponent<Item>().CurrentItemData;
    }


    // Start is called before the first frame update

    

   
}
