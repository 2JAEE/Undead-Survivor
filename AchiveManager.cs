using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchiveManager : MonoBehaviour
{

    private enum Achive 
    {
        UnlockBean, UnlockPotato
    }

    private Achive[] achives;

    public GameObject[] lockCharacter;
    public GameObject[] unlockCharacter;
    public GameObject uiNotice;

    void Awake()
    {
        achives = (Achive[])Enum.GetValues(typeof(Achive));
        Debug.LogFormat("{0}", achives);

        //key를 가지고 있는지
        //"MyData"키가 없으면 초기화
        if (!PlayerPrefs.HasKey("MyData"))
        {
            this.Init();
        }
    }

    private void Init()
    {
        //간단한 저장 기능을 제공하는 유니티 제공 클래스
        //set : 저장, Get : 가져오기
        //keyName으로 숫자(int)를 저장
        PlayerPrefs.SetInt("MyData", 1);

        foreach(Achive achive in achives)
        {
            PlayerPrefs.SetInt(achive.ToString(),0);
            //PlayerPrefs.SetInt("UnlockBean", 0);
            //PlayerPrefs.SetInt("UnlockPotato", 0);
        }
        
    }

    void Start()
    {
        this.UnlockCharacter();   
    }

    void UnlockCharacter()
    {
        for(int i =0; i < lockCharacter.Length; i++)
        {
            //업적 이름 가져오기
            string achiveName = achives[i].ToString();
            //업적이름으로 숫자 가져오기
            // 1 : true, 나머지 : false
            bool isUnlock = PlayerPrefs.GetInt(achiveName) == 1;   //0 또는 1
            lockCharacter[i].SetActive(!isUnlock); 
            unlockCharacter[i].SetActive(isUnlock);
        }
    }

    void LateUpdate()
    {
        foreach(Achive achive in achives)
        {
            this.CheckAchive(achive);
        }
    }

    void CheckAchive(Achive achive)
    {
        bool isAchive = false;

        switch (achive)
        {
            case Achive.UnlockBean:
                isAchive = GameManager.instance.kill >= 10;
                break;
            case Achive.UnlockPotato:
                isAchive = GameManager.instance.gameTime == GameManager.instance.maxGameTime;
                break;
        }


        //..해당 업적 처음 달성일때만
        //isAchive = true이고 해금이 안 된 상태일때만
        if (isAchive && PlayerPrefs.GetInt(achive.ToString()) == 0)
        {
            //해금
            PlayerPrefs.SetInt(achive.ToString(), 1);

            //uiNotice의 자식의 갯수
            for(int i =0;i<uiNotice.transform.childCount;i++) 
            {
                bool isActive = i == (int)achive;
                //Notice의 자식의 게임오브젝트 활성화 또는 비활성화
                uiNotice.transform.GetChild(i).gameObject.SetActive(isActive);
            }
            //Noice코루틴 실행
            this.StartCoroutine(NoticeRountine());
            
        }
    }

    //Notice 창만 활성화되는 코루틴
    IEnumerator NoticeRountine()
    {
        uiNotice.SetActive(true);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.LevelUp);

        //게임시간이 아닌 진짜 흘러가는 시간(isLive = false여도 멈추지 않음)
        yield return new WaitForSecondsRealtime(3f);

        uiNotice.SetActive(false);
    }
}
