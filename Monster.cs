using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.U2D;

public class Monster : MonoBehaviour
{
    [SerializeField]
    private Player player;
    public float speed =3f;
    private Rigidbody2D rBody;
    private Collider2D col;
    private int dirX;
    public RuntimeAnimatorController[] animCon;
    private bool isLive;
    float health;
    float maxHealth;
    private Animator anim;
    [SerializeField]
    private Bullet bullet;
    public float force = 3f;
    private WaitForFixedUpdate wait;
    public SpriteRenderer spriter;

    void Awake()
    {
        this.rBody = this.GetComponent<Rigidbody2D>();
        this.col = this.GetComponent<Collider2D>();
        this.anim = this.GetComponent<Animator>();
        this.wait = new WaitForFixedUpdate();
        this.spriter = this.GetComponent <SpriteRenderer>();
    }

    void FixedUpdate()
    {

        if (!GameManager.instance.isLive)
        {
            return;
        }

        if (!this.isLive || this.anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
        {
            return;
        }

        Vector3 dir = this.player.transform.position - this.transform.position;
        this.transform.Translate(dir.normalized * this.speed * Time.deltaTime);
        rBody.velocity = Vector2.zero;

        //바라보는 방향 설정    
        if (dir.x < 0)
        {
            dirX = -1;  //왼쪽
        }
        else if (dir.x > 1)
        {
            dirX = 1;  //오른쪽
        }

        //방향전환
        if (dir != Vector3.zero)
        {
            this.transform.localScale = new Vector3(dirX, 1, 1);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //충돌한 테그가 "Bullet"이 아니거나 isLive 상태가 아니라면
        if (!collision.CompareTag("Bullet")||!isLive)
        {
            return;
        }

        this.health -= this.bullet.damage;
        //KnockBack 코루틴 실행
        this.StartCoroutine(KnockBack());

        if (health > 0)
        {
            //몬스터 살아있음
            this.anim.SetTrigger("Hit");
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Hit);
        }
        else
        {
            this.isLive = false;
            //rigitbody 비활성화
            this.rBody.simulated = false;
            this.col.enabled = false;
            this.anim.SetBool("Dead", true);
            //sprite layer 감소시키기
            this.spriter.sortingOrder = 1;
            //kill info 증가
            GameManager.instance.kill++;
            //경험치 가져오기
            GameManager.instance.GetExp();

            //게임 종료시에는 사망 사운드 나지 않도록 조건추가
            if (GameManager.instance.isLive)
            {
                AudioManager.instance.PlaySfx(AudioManager.Sfx.Dead);
            }
        }
    }

    //활성화되면 불러와 지는 메서드(생존여부와 체력 초기화)
    private void OnEnable()
    {
        this.player = GameManager.instance.player.GetComponent<Player>();
        //활성화 되면 isLive
        this.isLive = true;
        this.health = this.maxHealth;
        //rigitbody 활성화
        this.rBody.simulated = true;
        this.col.enabled = true;
        this.spriter.sortingOrder = 2;
        this.anim.SetBool("Dead", false);
    }

    public void Init(SpawnData data)
    {
        //스프라이트타입에 따라 anim 배정하기
        this.anim.runtimeAnimatorController = animCon[data.spriteType];
        this.speed = data.speed;
        this.maxHealth = data.health;
        this.health = data.health;
    }

    //피격시 잠시 뒤로 밀리는 액션 (코루틴)
    IEnumerator KnockBack()
    {
        yield return wait;

        Vector3 playerPos = GameManager.instance.player.transform.position;
        //몬스터 뱡향
        Vector3 dir = this.transform.position - playerPos;
        //Addforce(방향 * 힘, 힘의 종류) ,impulse => 짧은 순간 힘(충돌,폭발)
        this.rBody.AddForce(dir.normalized * this.force,ForceMode2D.Impulse);
    }

    private void Dead()
    {
        gameObject.SetActive(false);
    }
}