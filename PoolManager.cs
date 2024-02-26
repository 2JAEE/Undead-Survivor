using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    //프리팹 보관할 변수
    public GameObject[] prefabs;

    //pool 담당하는 리스트들
    //prefab 종류에 따라 리스트 개수 생성 -> List도 배열화
    List<GameObject>[] pools;

    void Awake()
    {
        //List[] 초기화
        pools = new List<GameObject>[prefabs.Length];

        //for문 돌려서 배열 안의 각각의 List들 초기화
        for(int i = 0;i< pools.Length; i++)
        {
            //List 초기화
            pools[i] = new List<GameObject>();
        }
        //Debug.Log(pools.Length);
    }
    //pool 안의 게임오브젝트(item) 반환(가져오는) 메서드
    public GameObject Get(int i) 
    {
        //게임오브젝트 중에 '선택'
        GameObject select = null;  //일단 비워둠

        //... 선택한 pool의 놀고있는(비활성화된) 게임오브젝트 접근
        foreach (GameObject item in pools[i])
        {
            //...비활성화 되었으면
            if(!item.activeSelf)
            {
                //... 발견하면 -> select 변수에 할당 후 활성화
                select = item;
                select.SetActive(true);
                break;
            }
        }

        //... 못 찾았으면?
        if(!select)
        {
            //...새롭게 생성하고 -> select 변수에 할당
            select = Instantiate(prefabs[i],transform);
            pools[i].Add(select);
        }

       return select;    //선택한 것을 반환
    }
}