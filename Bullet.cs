using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;   //������
    public int per;   //����

    private Rigidbody2D rBody;
   
    private void Awake()
    {
        this.rBody = this.GetComponent<Rigidbody2D>();
    }

    public void Init(float damage, int per, Vector3 dir)
    {
        this.damage = damage;
        this.per = per;

        if(per > -1)
        {
            //�ӵ� �ο� (��ġ * �ӷ�)
            this.rBody.velocity = dir * 15f ;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //�±װ� Monster�� �ƴϰų� ������� -1�� ���
        if (!collision.CompareTag("Monster")| per == -1)
        {
            return;
        }

        //�浹���� ����� ����
        per--;

        //������� -1�� �Ǹ�
        if (per == -1)
        {
            //�ӵ� = 0 , ������Ʈ ��Ȱ��ȭ
            rBody.velocity = Vector2.zero;
            gameObject.SetActive(false);
        }
    }
}
