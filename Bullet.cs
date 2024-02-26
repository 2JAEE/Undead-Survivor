using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;   //데미지
    public int per;   //관통

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
            //속도 부여 (위치 * 속력)
            this.rBody.velocity = dir * 15f ;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //태그가 Monster가 아니거나 관통력이 -1인 경우
        if (!collision.CompareTag("Monster")| per == -1)
        {
            return;
        }

        //충돌마다 관통력 감소
        per--;

        //관통력이 -1이 되면
        if (per == -1)
        {
            //속도 = 0 , 오브젝트 비활성화
            rBody.velocity = Vector2.zero;
            gameObject.SetActive(false);
        }
    }
}
