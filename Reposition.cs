using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reposition : MonoBehaviour
{
    //��� class�� collider2D�� �ƿ츧
    private Collider2D coll;

    private void Awake()
    {
        this.coll = this.GetComponent<Collider2D>();
    }

    //Trigger�� üũ�� Collier���� ������ �� 
    void OnTriggerExit2D(Collider2D collision)
    {

        if (!collision.CompareTag("Area"))
        {
            return;
        }

        //Player ������
        Vector3 playerPos = GameManager.instance.player.transform.position;
        //Ÿ�ϸ� ������
        Vector3 myPos = this.transform.position;
        //�� ������ �Ÿ� ����(x,y), ���밪���� ��ȯ
        float disX = Mathf.Abs(playerPos.x - myPos.x);
        float disY = Mathf.Abs(playerPos.y - myPos.y);

        //Player ����(x,y)
        Vector3 playerDir = GameManager.instance.player.inputVec;
        //�밢���� ���� nomalized�� ���� 1���� ���� �۾���
        //������ (����) ? (true�� �� ��) : (false�� �� ��)
        float dirX = playerDir.x < 0 ? -1 : 1;
        float dirY = playerDir.y < 0 ? -1 : 1;

        switch (transform.tag)
        {
            case "Ground":
                //�¿�� �̵��ϸ�
                if (disX > disY)
                {
                    //dirX �������� 40ĭ �ǳʼ� �̵�
                    this.transform.Translate(Vector3.right * dirX * 40);
                }
                //���Ϸ� �̵��ϸ�
                else if (disX < disY)
                {
                    //dirY �������� 40ĭ �ǳʼ� �̵�
                    this.transform.Translate(Vector3.up * dirY * 40);
                }
                break;
            case "Monster":
                //���Ͱ� ���������
                if (coll.enabled)
                {
                    //Player �������� ���� ����(ȭ�� ��)
                    //ȭ�� ũ���� 20 ������ �Ÿ����� +-3��ŭ �������� ����
                    transform.Translate(playerDir * 20 + new Vector3(Random.Range(-3f,3f),Random.Range(-3f,3f),0));
                }
                break;
        }
    }
}
