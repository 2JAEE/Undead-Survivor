using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : MonoBehaviour
{
    public ItemData.eItemType type;
    public float rate;

    public void Init(ItemData data)
    {
        //...Basic Set
        name = "Gear" + data.itemId;
        //�θ� transform ����
        this.transform.parent = GameManager.instance.player.transform;
        this.transform.localPosition = Vector3.zero;

        //...Property Set
        type = data.itemType;
        rate = data.damages[0];

        //...��� �����Ǹ� Gear�� ����� �����Ŵ
        this.ApplyGear();
    }

    //...������
    public void LevelUp(float rate)
    {
        //rate �� ����
        this.rate = rate;

        //...��� ���Ӱ� �߰� �ǰų� ������ �� �� �������� �Լ� ȣ��
        //�÷��̾��� weapon�鿡�� �ٽ� ����
        this.ApplyGear();
    }


    void ApplyGear()
    {
        switch (type)
        {
            case ItemData.eItemType.Glove:
                this.RateUp();
                break;
            case ItemData.eItemType.Shoe:
                this.SpeedUp();
                break;
        }
    }

    //...���� ȸ���ӵ� ����
    void RateUp()
    {
        Weapon[] weapons = transform.parent.GetComponentsInChildren<Weapon>();
        
        foreach(Weapon weapon in weapons)
        {
            switch (weapon.id)
            {
                //id =0 -> �� 
                case 0:
                    float speed = 180 * Character.WeaponSpeed;
                    weapon.speed = speed + (speed * this.rate);
                    break;
                default:
                    speed = 0.3f * Character.WeaponRate;
                    weapon.speed = speed * (1f - this.rate);
                    break;
            }
        }
    }

    //...player�� �̵��ӵ� ����
    void SpeedUp()
    {
        float speed = 5f * Character.Speed;
        GameManager.instance.player.speed = speed + speed * this.rate;
    }
}