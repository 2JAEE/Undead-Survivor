using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //�ٷ� �޸𸮿� ����
    public static GameManager instance;

    //...Game Object...
    public Player player;
    public PoolManager pool;
    public LevelUp uiLevelUp;
    public Result uiResult;
    public GameObject monsterCleaner;
    public Transform uiJoy;

    //...GameControl...
    //����(�÷���) �ð�
    public float gameTime;
    //�ִ� ����(�÷���) �ð�
    public float maxGameTime = 2 * 10f;
    //�ð� ���� ���� ����
    public bool isLive;

    //...Player Info...
    public int playerId;
    public float health;
    public float maxHealth = 100;
    public int level;
    public int kill;
    public int exp;
    public int[] nextExp = { 10, 30, 60, 100, 150, 210, 280, 360, 450, 600 };

    void Awake()
    {
        instance = this;
        //������ ����
        Application.targetFrameRate = 60;
    }

    public void GameStart(int id)
    {
        this.playerId = id;
        //ü�� �ʱ�ȭ
        this.health = this.maxHealth;
        //ĳ���� Ȱ��ȭ
        this.player.gameObject.SetActive(true);

        //�⺻���� ����
        uiLevelUp.Select(playerId % 2);
        //���ӽð� �ʱ�ȭ
        this.Resume();

        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
    }

    public void GameOver()
    {
        this.StartCoroutine(GameOverRoutine());
    }

    IEnumerator GameOverRoutine()
    {
        this.isLive = false;

        yield return new WaitForSeconds(0.5f);

        this.uiResult.gameObject.SetActive(true);
        this.uiResult.Lose();
        //�ð� ����
        Stop();

        AudioManager.instance.PlaySfx(AudioManager.Sfx.Lose);
    }

    public void GameVictory()
    {
        this.StartCoroutine(GameVictoryRoutine());
    }

    IEnumerator GameVictoryRoutine()
    {
        this.isLive = false;
        this.monsterCleaner.SetActive(true);

        yield return new WaitForSeconds(3.5f);

        this.uiResult.gameObject.SetActive(true);
        this.uiResult.Win();
        //�ð� ����
        Stop();

        AudioManager.instance.PlaySfx(AudioManager.Sfx.Win);
    }


    //���� �����
    public void GameRetry()
    {
        SceneManager.LoadScene(0);
    }


    //���� ����
    public void GameQuit()
    {
        //���� ���� = ���ø����̼� ����
        Application.Quit();
    }

    void Update()
    {
        //�÷��� ���� �ƴ϶��(���� �����̸�) ȣ������ ����
        if (!isLive)
        {
            return;
        }

        //����(�÷���)�ð� = �ΰ��� �ð�
        this.gameTime += Time.deltaTime;

        //���� �ð��� �ִ���� �ð��� �Ѿ��    
        if(this.gameTime > this.maxGameTime)
        {
            //���� ���� �����(�ִ���� �ð��� �Ѿ�� �ʵ���)
            this.gameTime = this.maxGameTime;
            //�¸� �޼��� ȣ��
            this.GameVictory();
        }
    }
    
    //Exp ���� �޼���
    public void GetExp()
    {
        //�������� �� ����ġ ���� ����
        if (!isLive)
        {
            return;
        }

        //exp �� ����
        this.exp++;

        //exp ���� nextExp[level]���� ��������
        //...Min�Լ��� ����Ͽ� �ְ� ����ġ�� �״�� ����ϵ��� ����
        if (this.exp == this.nextExp[Mathf.Min(level,nextExp.Length-1)])
        {
            //level ����
            this.level++;
            //exp �� �ʱ�ȭ
            this.exp = 0;
            //uiLevelUp â �����ֱ�
            uiLevelUp.Show();
        }
    }

    //�ð� ����
    public void Stop()
    {
        this.isLive = false;
        //...TimeScale = ����Ƽ �ð� �ӵ�(����)
        Time.timeScale = 0;

        //���̽�ƽ �����
        this.uiJoy.localScale = Vector3.zero;
    }

    //�ð� �簳
    public void Resume()
    {
        this.isLive = true;
        Time.timeScale = 1;

        //���̽�ƽ ���̱�
        this.uiJoy.localScale = Vector3.one;
    }
}