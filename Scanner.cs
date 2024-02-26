using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Scanner : MonoBehaviour
{
    public float scanRange;
    public LayerMask targetLayer;
    public RaycastHit2D[] targets;
    public Transform nearestTarget;

    private void FixedUpdate()
    {
        //scanRange ���� �ش��ϴ� targetLayer�� ��ġ�� �迭�� ����
        targets = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0,targetLayer);
        nearestTarget = GetNearest();
    }

    //���� ����� target ��ġ ��ȯ �޼���
    Transform GetNearest()
    {
        Transform result = null;
        float dis = 100;
        foreach(RaycastHit2D target in targets)
        {
            Vector3 myPos = transform.position;
            Vector3 targetPos = target.transform.position;
            float curDis = Vector3.Distance(myPos, targetPos);

            //�� ����� ��ġ�� ����
            if (curDis < dis)
            {
                dis = curDis;
                result = target.transform;
            }
        }

        //���� ����� target��ġ ��ȯ
        return result;
    }
}
