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
        //부모 transform 설정
        this.transform.parent = GameManager.instance.player.transform;
        this.transform.localPosition = Vector3.zero;

        //...Property Set
        type = data.itemType;
        rate = data.damages[0];

        //...장비가 생성되면 Gear의 기능을 적용시킴
        this.ApplyGear();
    }

    //...레벨업
    public void LevelUp(float rate)
    {
        //rate 값 갱신
        this.rate = rate;

        //...장비가 새롭게 추가 되거나 레벨업 할 때 로직적용 함수 호출
        //플레이어의 weapon들에게 다시 적용
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

    //...삽의 회전속도 증가
    void RateUp()
    {
        Weapon[] weapons = transform.parent.GetComponentsInChildren<Weapon>();
        
        foreach(Weapon weapon in weapons)
        {
            switch (weapon.id)
            {
                //id =0 -> 삽 
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

    //...player의 이동속도 증가
    void SpeedUp()
    {
        float speed = 5f * Character.Speed;
        GameManager.instance.player.speed = speed + speed * this.rate;
    }
}