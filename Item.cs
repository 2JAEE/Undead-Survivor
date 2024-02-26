using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public ItemData data;
    public int level;
    public Weapon weapon;
    public Gear gear;

    private Image icon;
    private Text textLevel;
    private Text textName;
    private Text textDesc;

    void Awake()
    {
        //GetComponentsInchildren���� 2��° ������ ��������(1��° : �ڱ��ڽ�)
        this.icon = this.GetComponentsInChildren<Image>()[1];
        this.icon.sprite = data.itemIcon;

        Text[] texts = GetComponentsInChildren<Text>();
        this.textLevel = texts[0];
        this.textName = texts[1];
        this.textDesc = texts[2];

        this.textName.text = data.itemName;
    }
    
    //Ȱ��ȭ�� ����
    void OnEnable()
    {
        //���� 1���� ����
        textLevel.text = string.Format("Lv.{0}", (level + 1).ToString());
        switch (data.itemType) 
        {
            case ItemData.eItemType.Melee:
            case ItemData.eItemType.Range:
                //...damage�� ������� ǥ���ؾ��ϱ� ������ *100 ����
                textDesc.text = string.Format(data.itemDesc, data.damages[level]*100, data.counts[level]);
                break;
            case ItemData.eItemType.Glove:
            case ItemData.eItemType.Shoe:
            textDesc.text = string.Format(data.itemDesc, data.damages[level]*100);
                break;
            default:
                textDesc.text = string.Format(data.itemDesc);
                break;   
        }
    }

    //��ư Ŭ�� �̺�Ʈ
    public void OnClick()
    {
        switch (data.itemType)
        {
            //..2�� ���̵� ����
            //weapon : ��, ����
            case ItemData.eItemType.Melee:
            case ItemData.eItemType.Range:
                if(level == 0)
                {
                    GameObject newWeapon = new GameObject();
                    this.weapon = newWeapon.AddComponent<Weapon>();
                    //Weapon �ʱ�ȭ
                    this.weapon.Init(data);
                }
                else
                {
                    //...ó�� ������ �������� �������� Ƚ���� ���
                    //���ο� �����̸� damage �÷������
                    float nextDamage = data.baseDamage;
                    int nextCount = 0;

                    //damages�� ��з� ���̹Ƿ� ������ ���� ������
                    nextDamage += data.baseDamage * data.damages[level];
                    nextCount += data.counts[level];

                    //Weapon ������
                    weapon.LevelUp(nextDamage, nextCount);
                }
                //level ����
                level++;
                break;
               
            //gear : �尩, ��ȭ
            case ItemData.eItemType.Glove:
            case ItemData.eItemType.Shoe:
                if (level == 0)
                {
                    GameObject newGear = new GameObject();
                    this.gear = newGear.AddComponent<Gear>();
                    //Gear �ʱ�ȭ
                    this.gear.Init(data);
                }
                else
                {
                    float nextRate = data.damages[level];
                    gear.LevelUp(nextRate);
                }
                //level ����
                level++;
                break;
 
            case ItemData.eItemType.Heal:
                //heal : player ü�� -> �ִ� ü������
                //1ȸ�� ������ =>  level++ �ʿ����!!
                GameManager.instance.health = GameManager.instance.maxHealth;
                break;
        }

        //...���� ���� �ѱ��� �ʰ� ���� �߰�
        //Damages�� �迭����:5 �� �Ѿ�� Button�� interactable ��Ȱ��ȭ(�����ϰ�)
        if (level == data.damages.Length)
        {
            GetComponent<Button>().interactable = false;
        }
    }
}