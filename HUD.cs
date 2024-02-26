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

    //UI Ÿ�Ե�
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
                //���� exp
                float curExp = GameManager.instance.exp;
                //�ش� level�� �´� exp�� maxExp�� ����
                float maxExp = 
                    GameManager.instance.nextExp[Mathf.Min(GameManager.instance.level, GameManager.instance.nextExp.Length - 1)];
                //slider �� ���� (���� ����ġ / �ִ� ����ġ)
                slider.value = curExp / maxExp;
                break;

            case eInfoType.Level:
                text.text = string.Format("Lv.{0}", GameManager.instance.level.ToString());
                break;

            case eInfoType.Kill:
                //ų �� (F0 : �Ҽ����ڸ� = 0 & string ���� ��ȯ)
                text.text = string.Format("{0:F0}", GameManager.instance.kill);
                break;

            case eInfoType.Time:
                float remainTime = GameManager.instance.maxGameTime - GameManager.instance.gameTime;
                //��
                int min = Mathf.FloorToInt(remainTime / 60);
                //�� (% : ������, D: ǥ���� �ڸ���)
                int sec = Mathf.FloorToInt(remainTime % 60);
                text.text = string.Format("{0:D2}:{1:D2}", min, sec);
                break;

            case eInfoType.Health:
                //���� exp
                float curHealth = GameManager.instance.health;
                //�ش� level�� �´� exp�� maxExp�� ����
                float maxHealth = GameManager.instance.maxHealth;
                //slider �� ���� (���� ����ġ / �ִ� ����ġ)
                slider.value = curHealth / maxHealth;
                break;
        }
    }
}
