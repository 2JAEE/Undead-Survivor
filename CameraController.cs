using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //����ٴ� ���ӿ�����Ʈ
    public GameObject PlayerGo;

    void Update()
    {
        //����ٴ� ���ӿ�����Ʈ ��ġ�� x,y������ ����
        this.transform.position = 
            new Vector3(this.PlayerGo.transform.position.x, this.PlayerGo.transform.position.y, this.transform.position.z);
    }
}
