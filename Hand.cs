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

    //오른손 위치
    private Vector3 rightPos = new Vector3(0.35f, -0.15f, 0);
    //오른손 반전 위치
    private Vector3 rightPosReverse = new Vector3(-0.15f, -0.1f, 0);
    //왼손 각도
    private Quaternion leftRot = Quaternion.Euler(0, 0, -35f);
    //왼손 반전 각도
    private Quaternion leftRotReverse = Quaternion.Euler(0, 0, -135);

    private void Awake()
    {
        //
        this.player = GetComponentsInParent<SpriteRenderer>()[1];
    }

    private void Update()
    {
        bool isReverse = player.flipX;
        if (isLeft) //근거리 무기
        {
            //..player가 방향전환을 했다면               참          거짓
            transform.localRotation = isReverse ? leftRotReverse : leftRot;
            this.spriter.flipY = isReverse;
            spriter.sortingOrder = isReverse ? 4 : 6;

        }
        else  //원거리 무기
        {
            transform.localPosition = isReverse ? rightPosReverse : rightPos;
            this.spriter.flipX = isReverse;
            spriter.sortingOrder = isReverse ? 6 : 4;
        }
    }
}