using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    //������ ������ ����
    public GameObject[] prefabs;

    //pool ����ϴ� ����Ʈ��
    //prefab ������ ���� ����Ʈ ���� ���� -> List�� �迭ȭ
    List<GameObject>[] pools;

    void Awake()
    {
        //List[] �ʱ�ȭ
        pools = new List<GameObject>[prefabs.Length];

        //for�� ������ �迭 ���� ������ List�� �ʱ�ȭ
        for(int i = 0;i< pools.Length; i++)
        {
            //List �ʱ�ȭ
            pools[i] = new List<GameObject>();
        }
        //Debug.Log(pools.Length);
    }
    //pool ���� ���ӿ�����Ʈ(item) ��ȯ(��������) �޼���
    public GameObject Get(int i) 
    {
        //���ӿ�����Ʈ �߿� '����'
        GameObject select = null;  //�ϴ� �����

        //... ������ pool�� ����ִ�(��Ȱ��ȭ��) ���ӿ�����Ʈ ����
        foreach (GameObject item in pools[i])
        {
            //...��Ȱ��ȭ �Ǿ�����
            if(!item.activeSelf)
            {
                //... �߰��ϸ� -> select ������ �Ҵ� �� Ȱ��ȭ
                select = item;
                select.SetActive(true);
                break;
            }
        }

        //... �� ã������?
        if(!select)
        {
            //...���Ӱ� �����ϰ� -> select ������ �Ҵ�
            select = Instantiate(prefabs[i],transform);
            pools[i].Add(select);
        }

       return select;    //������ ���� ��ȯ
    }
}