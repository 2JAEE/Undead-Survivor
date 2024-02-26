using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Animator anim;
    public float speed = 5f;
    public Vector2 inputVec;
    private Rigidbody2D rBody;
    public RuntimeAnimatorController[] animCon;

    public Scanner scanner;
    public Hand[] hands;
    [SerializeField]
    private SpriteRenderer sprite;

    void Awake()
    {
        this.anim = this.GetComponent<Animator>();
        this.scanner = this.GetComponent<Scanner>();
        this.sprite = this.GetComponent<SpriteRenderer>();
        this.rBody = this.GetComponent<Rigidbody2D>();
        this.hands = GetComponentsInChildren<Hand>(true);
    }

    void OnEnable()
    {
        Debug.LogFormat("<color=red>{0}</color>", GameManager.instance.playerId);

        this.speed *= Character.Speed;
        this.anim.runtimeAnimatorController = animCon[GameManager.instance.playerId];
    }

    void Update()
    {
        if (!GameManager.instance.isLive)
        {
            return;
        }

        //����, �¿� ��ǲ�� �ޱ�
        //this.inputVec.x = Input.GetAxisRaw("Horizontal");
        //this.inputVec.y = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate()
    {
        if (!GameManager.instance.isLive)
        {
            return;
        }

        //���� ����
        Vector3 dir = new Vector3(inputVec.x, inputVec.y, 0).normalized;
        //�̵�
        this.transform.Translate(inputVec * this.speed * Time.deltaTime);
        //Debug.Log(dir);

        //�̵��� ���� �ʰ� �ִٸ�
        if (dir == Vector3.zero)
        {
            //Idle
            this.anim.SetInteger("State", 0);
        }
        else
        {
            //Run
            this.anim.SetInteger("State", 1);
            if (inputVec.x != 0)
            {
                //������ȯ
                //this.transform.localScale = new Vector3(inputVec.x, 1, 1);
                this.sprite.flipX = inputVec.x < 0;
            }
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (!GameManager.instance.isLive)
        {
            return;
        }
        //������ �ǰ� ������ ���
        GameManager.instance.health -= Time.deltaTime * 10;
        
        if(GameManager.instance.health < 0)
        {
            //player�� ������ ���� ��������Ʈ�� �ٲ�Ƿ� Area �Ʒ��� �ʿ�X(��Ȱ��ȭ)
            for (int i = 2; i < transform.childCount; i++)
            {
                //���� Ʈ�������� �ڽ��� ���ӿ�����Ʈ�� ������ ��Ȱ��ȭ
                transform.GetChild(i).gameObject.SetActive(false);
            }
            this.anim.SetInteger("State", 2);
            //�������� �޼��� ȣ��
            GameManager.instance.GameOver();
        }
    }
    
    //InputSystem�� �̿��� Input�� �ޱ�
    void OnMove(InputValue value)
    {
        this.inputVec = value.Get<Vector2>();
        Debug.Log(inputVec);
    }
}