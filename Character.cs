using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    //...�̵��ӵ� 10% ����
    public static float Speed 
    {
        //playerId�� 0 => �ӵ�*1.1f , 0 �ƴϸ� => �ӵ�*1f
        get { return GameManager.instance.playerId == 0 ? 1.1f : 1f; }
    }

    //...�� �ӵ� 10% ����
    public static float WeaponSpeed
    {
        //playerId�� 0 => �ӵ�*1.1f , 0 �ƴϸ� => �ӵ�*1f
        get { return GameManager.instance.playerId == 1 ? 1.1f : 1f; }
    }

    //...���� �߻簣�� 10% ����
    public static float WeaponRate
    {
        //playerId�� 0 => �ӵ�*1.1f , 0 �ƴϸ� => �ӵ�*1f
        get { return GameManager.instance.playerId == 1 ? 0.9f : 1f; }
    }


    //...������ 10% ����
    public static float Damage
    {
        //playerId�� 0 => �ӵ�*1.1f , 0 �ƴϸ� => �ӵ�*1f
        get { return GameManager.instance.playerId == 2 ? 1.1f : 1f; }
    }

    //...���� ���� 1�� ����
    public static int Count
    {
        //playerId�� 0 => �ӵ�*1.1f , 0 �ƴϸ� => �ӵ�*1f
        get { return GameManager.instance.playerId == 2 ? 1 : 0; }
    }
}
