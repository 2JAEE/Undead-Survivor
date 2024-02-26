using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reposition : MonoBehaviour
{
    //모든 class의 collider2D를 아우름
    private Collider2D coll;

    private void Awake()
    {
        this.coll = this.GetComponent<Collider2D>();
    }

    //Trigger가 체크된 Collier에서 나갔을 때 
    void OnTriggerExit2D(Collider2D collision)
    {

        if (!collision.CompareTag("Area"))
        {
            return;
        }

        //Player 포지션
        Vector3 playerPos = GameManager.instance.player.transform.position;
        //타일맵 포지션
        Vector3 myPos = this.transform.position;
        //둘 사이의 거리 측정(x,y), 절대값으로 반환
        float disX = Mathf.Abs(playerPos.x - myPos.x);
        float disY = Mathf.Abs(playerPos.y - myPos.y);

        //Player 방향(x,y)
        Vector3 playerDir = GameManager.instance.player.inputVec;
        //대각선일 때는 nomalized에 의해 1보다 값이 작아짐
        //연산자 (조건) ? (true일 때 값) : (false일 때 값)
        float dirX = playerDir.x < 0 ? -1 : 1;
        float dirY = playerDir.y < 0 ? -1 : 1;

        switch (transform.tag)
        {
            case "Ground":
                //좌우로 이동하면
                if (disX > disY)
                {
                    //dirX 방향으로 40칸 건너서 이동
                    this.transform.Translate(Vector3.right * dirX * 40);
                }
                //상하로 이동하면
                else if (disX < disY)
                {
                    //dirY 방향으로 40칸 건너서 이동
                    this.transform.Translate(Vector3.up * dirY * 40);
                }
                break;
            case "Monster":
                //몬스터가 살아있으면
                if (coll.enabled)
                {
                    //Player 맞은편에서 몬스터 생성(화면 밖)
                    //화면 크기인 20 떨어진 거리에서 +-3만큼 랜덤으로 생성
                    transform.Translate(playerDir * 20 + new Vector3(Random.Range(-3f,3f),Random.Range(-3f,3f),0));
                }
                break;
        }
    }
}
