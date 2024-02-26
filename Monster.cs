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

        //�ٶ󺸴� ���� ����    
        if (dir.x < 0)
        {
            dirX = -1;  //����
        }
        else if (dir.x > 1)
        {
            dirX = 1;  //������
        }

        //������ȯ
        if (dir != Vector3.zero)
        {
            this.transform.localScale = new Vector3(dirX, 1, 1);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //�浹�� �ױװ� "Bullet"�� �ƴϰų� isLive ���°� �ƴ϶��
        if (!collision.CompareTag("Bullet")||!isLive)
        {
            return;
        }

        this.health -= this.bullet.damage;
        //KnockBack �ڷ�ƾ ����
        this.StartCoroutine(KnockBack());

        if (health > 0)
        {
            //���� �������
            this.anim.SetTrigger("Hit");
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Hit);
        }
        else
        {
            this.isLive = false;
            //rigitbody ��Ȱ��ȭ
            this.rBody.simulated = false;
            this.col.enabled = false;
            this.anim.SetBool("Dead", true);
            //sprite layer ���ҽ�Ű��
            this.spriter.sortingOrder = 1;
            //kill info ����
            GameManager.instance.kill++;
            //����ġ ��������
            GameManager.instance.GetExp();

            //���� ����ÿ��� ��� ���� ���� �ʵ��� �����߰�
            if (GameManager.instance.isLive)
            {
                AudioManager.instance.PlaySfx(AudioManager.Sfx.Dead);
            }
        }
    }

    //Ȱ��ȭ�Ǹ� �ҷ��� ���� �޼���(�������ο� ü�� �ʱ�ȭ)
    private void OnEnable()
    {
        this.player = GameManager.instance.player.GetComponent<Player>();
        //Ȱ��ȭ �Ǹ� isLive
        this.isLive = true;
        this.health = this.maxHealth;
        //rigitbody Ȱ��ȭ
        this.rBody.simulated = true;
        this.col.enabled = true;
        this.spriter.sortingOrder = 2;
        this.anim.SetBool("Dead", false);
    }

    public void Init(SpawnData data)
    {
        //��������ƮŸ�Կ� ���� anim �����ϱ�
        this.anim.runtimeAnimatorController = animCon[data.spriteType];
        this.speed = data.speed;
        this.maxHealth = data.health;
        this.health = data.health;
    }

    //�ǰݽ� ��� �ڷ� �и��� �׼� (�ڷ�ƾ)
    IEnumerator KnockBack()
    {
        yield return wait;

        Vector3 playerPos = GameManager.instance.player.transform.position;
        //���� ����
        Vector3 dir = this.transform.position - playerPos;
        //Addforce(���� * ��, ���� ����) ,impulse => ª�� ���� ��(�浹,����)
        this.rBody.AddForce(dir.normalized * this.force,ForceMode2D.Impulse);
    }

    private void Dead()
    {
        gameObject.SetActive(false);
    }
}