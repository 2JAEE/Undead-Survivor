using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Cinemachine.DocumentationSortingAttribute;

public class HUD : MonoBehaviour
{
    public enum eInfoType 
    {
        Exp, Level, Kill, Time, Health
    }
    public eInfoType type;

    //UI 타입들
    private Text text;
    private Slider slider;

    void Awake()
    {
        this.text = this.GetComponent<Text>();
        this.slider = this.GetComponent<Slider>();
    }

    void Update()
    {
        switch (type) 
        {
            case eInfoType.Exp:
                //현재 exp
                float curExp = GameManager.instance.exp;
                //해당 level에 맞는 exp를 maxExp로 설정
                float maxExp = 
                    GameManager.instance.nextExp[Mathf.Min(GameManager.instance.level, GameManager.instance.nextExp.Length - 1)];
                //slider 값 설정 (현재 경험치 / 최대 경험치)
                slider.value = curExp / maxExp;
                break;

            case eInfoType.Level:
                text.text = string.Format("Lv.{0}", GameManager.instance.level.ToString());
                break;

            case eInfoType.Kill:
                //킬 수 (F0 : 소수점자리 = 0 & string 형식 반환)
                text.text = string.Format("{0:F0}", GameManager.instance.kill);
                break;

            case eInfoType.Time:
                float remainTime = GameManager.instance.maxGameTime - GameManager.instance.gameTime;
                //분
                int min = Mathf.FloorToInt(remainTime / 60);
                //초 (% : 나머지, D: 표시할 자릿수)
                int sec = Mathf.FloorToInt(remainTime % 60);
                text.text = string.Format("{0:D2}:{1:D2}", min, sec);
                break;

            case eInfoType.Health:
                //현재 exp
                float curHealth = GameManager.instance.health;
                //해당 level에 맞는 exp를 maxExp로 설정
                float maxHealth = GameManager.instance.maxHealth;
                //slider 값 설정 (현재 경험치 / 최대 경험치)
                slider.value = curHealth / maxHealth;
                break;
        }
    }
}
