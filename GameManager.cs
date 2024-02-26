using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //바로 메모리에 저장
    public static GameManager instance;

    //...Game Object...
    public Player player;
    public PoolManager pool;
    public LevelUp uiLevelUp;
    public Result uiResult;
    public GameObject monsterCleaner;
    public Transform uiJoy;

    //...GameControl...
    //게임(플레이) 시간
    public float gameTime;
    //최대 게임(플레이) 시간
    public float maxGameTime = 2 * 10f;
    //시간 정지 여부 변수
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
        //프레임 지정
        Application.targetFrameRate = 60;
    }

    public void GameStart(int id)
    {
        this.playerId = id;
        //체력 초기화
        this.health = this.maxHealth;
        //캐릭터 활성화
        this.player.gameObject.SetActive(true);

        //기본무기 지급
        uiLevelUp.Select(playerId % 2);
        //게임시간 초기화
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
        //시간 멈춤
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
        //시간 멈춤
        Stop();

        AudioManager.instance.PlaySfx(AudioManager.Sfx.Win);
    }


    //게임 재시작
    public void GameRetry()
    {
        SceneManager.LoadScene(0);
    }


    //게임 종료
    public void GameQuit()
    {
        //게임 종료 = 어플리케이션 종료
        Application.Quit();
    }

    void Update()
    {
        //플레이 중이 아니라면(정지 상태이면) 호출하지 않음
        if (!isLive)
        {
            return;
        }

        //게임(플레이)시간 = 인게임 시간
        this.gameTime += Time.deltaTime;

        //게임 시간이 최대게임 시간을 넘어가면    
        if(this.gameTime > this.maxGameTime)
        {
            //둘을 같게 만들기(최대게임 시간을 넘어가지 않도록)
            this.gameTime = this.maxGameTime;
            //승리 메서드 호출
            this.GameVictory();
        }
    }
    
    //Exp 갱신 메서드
    public void GetExp()
    {
        //게임종료 후 경험치 증가 방지
        if (!isLive)
        {
            return;
        }

        //exp 값 증가
        this.exp++;

        //exp 값이 nextExp[level]값과 같아지면
        //...Min함수를 사용하여 최고 경험치를 그대로 사용하도록 변경
        if (this.exp == this.nextExp[Mathf.Min(level,nextExp.Length-1)])
        {
            //level 증가
            this.level++;
            //exp 값 초기화
            this.exp = 0;
            //uiLevelUp 창 보여주기
            uiLevelUp.Show();
        }
    }

    //시간 정지
    public void Stop()
    {
        this.isLive = false;
        //...TimeScale = 유니티 시간 속도(배율)
        Time.timeScale = 0;

        //조이스틱 숨기기
        this.uiJoy.localScale = Vector3.zero;
    }

    //시간 재개
    public void Resume()
    {
        this.isLive = true;
        Time.timeScale = 1;

        //조이스틱 보이기
        this.uiJoy.localScale = Vector3.one;
    }
}