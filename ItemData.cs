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
    public string itemDesc;  //..������ ����
    public Sprite itemIcon;

    //�������� ����ϴ� �ɷ�ġ
    [Header("# Level Data")]
    public float baseDamage;
    public int baseCount;
    public float[] damages;
    public int[] counts;

    //���� ����
    [Header("# Weapon")]
    public GameObject projectile;
    public Sprite hand;
}