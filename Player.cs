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

        //상하, 좌우 인풋값 받기
        //this.inputVec.x = Input.GetAxisRaw("Horizontal");
        //this.inputVec.y = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate()
    {
        if (!GameManager.instance.isLive)
        {
            return;
        }

        //방향 설정
        Vector3 dir = new Vector3(inputVec.x, inputVec.y, 0).normalized;
        //이동
        this.transform.Translate(inputVec * this.speed * Time.deltaTime);
        //Debug.Log(dir);

        //이동을 하지 않고 있다면
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
                //방향전환
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
        //적절한 피격 데미지 계산
        GameManager.instance.health -= Time.deltaTime * 10;
        
        if(GameManager.instance.health < 0)
        {
            //player가 죽으면 무덤 스프라이트로 바뀌므로 Area 아래의 필요X(비활성화)
            for (int i = 2; i < transform.childCount; i++)
            {
                //현재 트렌스폼의 자식을 게임오브젝트로 꺼낸후 비활성화
                transform.GetChild(i).gameObject.SetActive(false);
            }
            this.anim.SetInteger("State", 2);
            //게임종료 메서드 호출
            GameManager.instance.GameOver();
        }
    }
    
    //InputSystem을 이용해 Input값 받기
    void OnMove(InputValue value)
    {
        this.inputVec = value.Get<Vector2>();
        Debug.Log(inputVec);
    }
}