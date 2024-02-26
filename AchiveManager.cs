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

        //key�� ������ �ִ���
        //"MyData"Ű�� ������ �ʱ�ȭ
        if (!PlayerPrefs.HasKey("MyData"))
        {
            this.Init();
        }
    }

    private void Init()
    {
        //������ ���� ����� �����ϴ� ����Ƽ ���� Ŭ����
        //set : ����, Get : ��������
        //keyName���� ����(int)�� ����
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
            //���� �̸� ��������
            string achiveName = achives[i].ToString();
            //�����̸����� ���� ��������
            // 1 : true, ������ : false
            bool isUnlock = PlayerPrefs.GetInt(achiveName) == 1;   //0 �Ǵ� 1
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


        //..�ش� ���� ó�� �޼��϶���
        //isAchive = true�̰� �ر��� �� �� �����϶���
        if (isAchive && PlayerPrefs.GetInt(achive.ToString()) == 0)
        {
            //�ر�
            PlayerPrefs.SetInt(achive.ToString(), 1);

            //uiNotice�� �ڽ��� ����
            for(int i =0;i<uiNotice.transform.childCount;i++) 
            {
                bool isActive = i == (int)achive;
                //Notice�� �ڽ��� ���ӿ�����Ʈ Ȱ��ȭ �Ǵ� ��Ȱ��ȭ
                uiNotice.transform.GetChild(i).gameObject.SetActive(isActive);
            }
            //Noice�ڷ�ƾ ����
            this.StartCoroutine(NoticeRountine());
            
        }
    }

    //Notice â�� Ȱ��ȭ�Ǵ� �ڷ�ƾ
    IEnumerator NoticeRountine()
    {
        uiNotice.SetActive(true);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.LevelUp);

        //���ӽð��� �ƴ� ��¥ �귯���� �ð�(isLive = false���� ������ ����)
        yield return new WaitForSecondsRealtime(3f);

        uiNotice.SetActive(false);
    }
}
