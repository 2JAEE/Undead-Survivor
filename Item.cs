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
        //GetComponentsInchildren에서 2번째 값으로 가져오기(1번째 : 자기자신)
        this.icon = this.GetComponentsInChildren<Image>()[1];
        this.icon.sprite = data.itemIcon;

        Text[] texts = GetComponentsInChildren<Text>();
        this.textLevel = texts[0];
        this.textName = texts[1];
        this.textDesc = texts[2];

        this.textName.text = data.itemName;
    }
    
    //활성화시 동작
    void OnEnable()
    {
        //레벨 1부터 시작
        textLevel.text = string.Format("Lv.{0}", (level + 1).ToString());
        switch (data.itemType) 
        {
            case ItemData.eItemType.Melee:
            case ItemData.eItemType.Range:
                //...damage는 백분율로 표현해야하기 때문에 *100 해줌
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

    //버튼 클릭 이벤트
    public void OnClick()
    {
        switch (data.itemType)
        {
            //..2개 같이도 가능
            //weapon : 삽, 엽총
            case ItemData.eItemType.Melee:
            case ItemData.eItemType.Range:
                if(level == 0)
                {
                    GameObject newWeapon = new GameObject();
                    this.weapon = newWeapon.AddComponent<Weapon>();
                    //Weapon 초기화
                    this.weapon.Init(data);
                }
                else
                {
                    //...처음 이후의 레벨업은 데미지와 횟수를 계산
                    //새로운 무기이면 damage 올려줘야함
                    float nextDamage = data.baseDamage;
                    int nextCount = 0;

                    //damages가 백분률 값이므로 곱해준 다음 더해줌
                    nextDamage += data.baseDamage * data.damages[level];
                    nextCount += data.counts[level];

                    //Weapon 레벨업
                    weapon.LevelUp(nextDamage, nextCount);
                }
                //level 증가
                level++;
                break;
               
            //gear : 장갑, 장화
            case ItemData.eItemType.Glove:
            case ItemData.eItemType.Shoe:
                if (level == 0)
                {
                    GameObject newGear = new GameObject();
                    this.gear = newGear.AddComponent<Gear>();
                    //Gear 초기화
                    this.gear.Init(data);
                }
                else
                {
                    float nextRate = data.damages[level];
                    gear.LevelUp(nextRate);
                }
                //level 증가
                level++;
                break;
 
            case ItemData.eItemType.Heal:
                //heal : player 체력 -> 최대 체력으로
                //1회성 아이템 =>  level++ 필요없음!!
                GameManager.instance.health = GameManager.instance.maxHealth;
                break;
        }

        //...레벨 개수 넘기지 않게 로직 추가
        //Damages의 배열길이:5 를 넘어가면 Button의 interactable 비활성화(투명하게)
        if (level == data.damages.Length)
        {
            GetComponent<Button>().interactable = false;
        }
    }
}