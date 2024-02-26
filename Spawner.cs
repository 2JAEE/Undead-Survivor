using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    //spawnPoint 배열
    public Transform[] spawnPoint;
    //spawnData 배열
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

        //레벨 = 게임시간/10
        //level이 int형식이므로 나눈 나머지를 버리고 몫만 int로 변환
        this.level = Mathf.FloorToInt(GameManager.instance.gameTime / 10f);

        //spawnData[]에서 레벨에 해당하는 spawnTime이 되면 몬스터 소환
        if (timer > spawnData[level].spawnTime)
        {
            timer = 0f;
            this.Spawn(); //
        }
    }

    void Spawn()   
    {
        //pool들 중에 level에 따라 몬스터 호출
        GameObject monster = GameManager.instance.pool.Get(0);

        //가져온 pool을 spawnPoint들 중에 랜덤으로 배치
        monster.transform.position 
            = this.spawnPoint[Random.Range(1, this.spawnPoint.Length)].transform.position;
        Debug.LogFormat("<color=yellow>level : {0}</color>", level);
        //몬스터 호출 및 초기화
        monster.GetComponent<Monster>().Init(spawnData[level]);
    }
}

[System.Serializable]  //직렬화 해주기 - 개체 저장 혹은 전송 가능
//하나의 스크립트 내에 여러개의 클래스 선언 가능
public class SpawnData
{
    //소환시간, 스프라이트 타입, 체력, 속도
    public float spawnTime;
    public int spriteType;
    public int health;
    public float speed;
}

