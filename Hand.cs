using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public bool isLeft;
    public SpriteRenderer spriter;
    private SpriteRenderer player;

    //������ ��ġ
    private Vector3 rightPos = new Vector3(0.35f, -0.15f, 0);
    //������ ���� ��ġ
    private Vector3 rightPosReverse = new Vector3(-0.15f, -0.1f, 0);
    //�޼� ����
    private Quaternion leftRot = Quaternion.Euler(0, 0, -35f);
    //�޼� ���� ����
    private Quaternion leftRotReverse = Quaternion.Euler(0, 0, -135);

    private void Awake()
    {
        //
        this.player = GetComponentsInParent<SpriteRenderer>()[1];
    }

    private void Update()
    {
        bool isReverse = player.flipX;
        if (isLeft) //�ٰŸ� ����
        {
            //..player�� ������ȯ�� �ߴٸ�               ��          ����
            transform.localRotation = isReverse ? leftRotReverse : leftRot;
            this.spriter.flipY = isReverse;
            spriter.sortingOrder = isReverse ? 4 : 6;

        }
        else  //���Ÿ� ����
        {
            transform.localPosition = isReverse ? rightPosReverse : rightPos;
            this.spriter.flipX = isReverse;
            spriter.sortingOrder = isReverse ? 6 : 4;
        }
    }
}