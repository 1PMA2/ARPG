using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LevelUpUI : MonoBehaviour
{
    TestBox box;
    // Start is called before the first frame update
    void Start()
    {
        box = DungeonGenerator.Instance.playerInfo.gameObject.GetComponent<TestBox>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnHealthButtonClick()
    {
        box?.MaxHealthUp();
        UIManager.Instance.ActiveLevelUpUI(false);
    }

    public void OnStaminaButtonClick()
    {
        box?.MaxStaminaUp();
        UIManager.Instance.ActiveLevelUpUI(false);
    }

    public void OnDamageButtonClick()
    {
        box?.DamageUp();
        UIManager.Instance.ActiveLevelUpUI(false);
    }
}
