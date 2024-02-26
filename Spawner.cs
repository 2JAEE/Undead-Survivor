using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    //spawnPoint �迭
    public Transform[] spawnPoint;
    //spawnData �迭
    public SpawnData[] spawnData;

    float timer;
    int level;

    void Start()
    {
        this.spawnPoint = this.GetComponentsInChildren<Transform>();    
    }
    
    void Update()
    {
        if (!GameManager.instance.isLive)
        {
            return;
        }

        timer += Time.deltaTime;

        //���� = ���ӽð�/10
        //level�� int�����̹Ƿ� ���� �������� ������ �� int�� ��ȯ
        this.level = Mathf.FloorToInt(GameManager.instance.gameTime / 10f);

        //spawnData[]���� ������ �ش��ϴ� spawnTime�� �Ǹ� ���� ��ȯ
        if (timer > spawnData[level].spawnTime)
        {
            timer = 0f;
            this.Spawn(); //
        }
    }

    void Spawn()   
    {
        //pool�� �߿� level�� ���� ���� ȣ��
        GameObject monster = GameManager.instance.pool.Get(0);

        //������ pool�� spawnPoint�� �߿� �������� ��ġ
        monster.transform.position 
            = this.spawnPoint[Random.Range(1, this.spawnPoint.Length)].transform.position;
        Debug.LogFormat("<color=yellow>level : {0}</color>", level);
        //���� ȣ�� �� �ʱ�ȭ
        monster.GetComponent<Monster>().Init(spawnData[level]);
    }
}

[System.Serializable]  //����ȭ ���ֱ� - ��ü ���� Ȥ�� ���� ����
//�ϳ��� ��ũ��Ʈ ���� �������� Ŭ���� ���� ����
public class SpawnData
{
    //��ȯ�ð�, ��������Ʈ Ÿ��, ü��, �ӵ�
    public float spawnTime;
    public int spriteType;
    public int health;
    public float speed;
}

