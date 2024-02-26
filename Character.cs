using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    //...이동속도 10% 증가
    public static float Speed 
    {
        //playerId가 0 => 속도*1.1f , 0 아니면 => 속도*1f
        get { return GameManager.instance.playerId == 0 ? 1.1f : 1f; }
    }

    //...삽 속도 10% 증가
    public static float WeaponSpeed
    {
        //playerId가 0 => 속도*1.1f , 0 아니면 => 속도*1f
        get { return GameManager.instance.playerId == 1 ? 1.1f : 1f; }
    }

    //...엽총 발사간격 10% 증가
    public static float WeaponRate
    {
        //playerId가 0 => 속도*1.1f , 0 아니면 => 속도*1f
        get { return GameManager.instance.playerId == 1 ? 0.9f : 1f; }
    }


    //...데미지 10% 증가
    public static float Damage
    {
        //playerId가 0 => 속도*1.1f , 0 아니면 => 속도*1f
        get { return GameManager.instance.playerId == 2 ? 1.1f : 1f; }
    }

    //...무기 갯수 1개 증가
    public static int Count
    {
        //playerId가 0 => 속도*1.1f , 0 아니면 => 속도*1f
        get { return GameManager.instance.playerId == 2 ? 1 : 0; }
    }
}
