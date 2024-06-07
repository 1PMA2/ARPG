using PlayerController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Object/ItemData")]
public class ItemData : ScriptableObject
{
   
    public enum ItemType { Counter, Recoil, Thunder, SuperArmor, AutoGuard, Drain, CombatBreathing }

    [Header("#Main Info")]
    public ItemType itemType;
    public int itemId;
    public string itemName;
    public string itemDesc;
    public Sprite itemIcon;

    [Header("#Level Info")]
    public float baseDamage;
    public int level;
    public int baseCount;
    public float[] damages;
    public float[] counts;
}
