using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName ="Scriptable Object/ItemData")]
public class ItemData : ScriptableObject
{
    public enum eItemType 
    { 
        Melee, Range, Glove, Shoe, Heal
    }

    [Header("# Main Info")]
    public eItemType itemType;
    public int itemId;
    public string itemName;
    [TextArea]
    public string itemDesc;  //..아이템 설명
    public Sprite itemIcon;

    //레벨별로 상승하는 능력치
    [Header("# Level Data")]
    public float baseDamage;
    public int baseCount;
    public float[] damages;
    public int[] counts;

    //무기 정보
    [Header("# Weapon")]
    public GameObject projectile;
    public Sprite hand;
}