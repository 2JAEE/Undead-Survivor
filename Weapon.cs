using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Weapon : MonoBehaviour
{ 
    //����id, ������id, damage, ����, �ӵ�
    public int id;
    public int prefabId;
    public float damage;
    public int count;
    public float speed;

    private float timer;

    private Player player;


    private void Awake()
    {
        //�θ� ������Ʈ�� �ִ� ������Ʈ ��������
        this.player = GameManager.instance.player;
    }
    
    void Update()
    {
        if (!GameManager.instance.isLive)
        {
            return;
        }

        //id ���� ����
        switch (id)
        {
            case 0:
                //bullet(��) ȸ����Ű��
                this.transform.Rotate(Vector3.back * this.speed * Time.deltaTime);
                break;
            default:
                this.timer += Time.deltaTime;

                if(this.timer > this.speed)
                {
                    //timer �ʱ�ȭ
                    this.timer = 0f;
                    //�Ѿ� �߻�   
                    this.Fire();
                }
                break;
        }
    }

    //id���� �ʱ�ȭ
    public void Init(ItemData data)
    {
        //...Basic Set
        this.name = "Weapon" + data.itemId;
        //�θ� transform ����
        this.transform.parent = player.transform;
        this.transform.localPosition = Vector3.zero;
        
        //...Property Set
        this.id = data.itemId;
        this.damage = data.baseDamage * Character.Damage;
        this.count = data.baseCount + Character.Count; 

        for(int i = 0; i < GameManager.instance.pool.prefabs.Length; i++)
        {
            //���� �����հ� ������Ʈ Ǯ�� ������ ����
            if(data.projectile == GameManager.instance.pool.prefabs[i])
            {
                this.prefabId = i;
                break;
            }
        }

        switch (id) 
        {
            case 0:
                this.speed = 180 * Character.WeaponSpeed;
                this.Batch();
                break;
            default:
                //�Ѿ� �߻� �ӵ�
                speed = 0.3f * Character.WeaponRate;
                break;
        }

        //...Hand Set
        //���� �տ� ���
        Hand hand = player.hands[(int)data.itemType];
        hand.spriter.sprite = data.hand;
        hand.gameObject.SetActive(true);


        //player�� ������ �ִ� ��� ��� ApplyGear ����
        //...Ư�� �Լ� ȣ���� ��� �ڽĿ��� ����ϴ� �Լ�
        //2��° ���ڰ����� 'DontRequireReceiver' �߰� (�� receiver�� �ʿ����� �ʴ�)
        this.player.BroadcastMessage("ApplyGear",SendMessageOptions.DontRequireReceiver);
    }

    //...������ �޼���
    //������ �� damage�� count �����ϱ�
    public void LevelUp(float damage, int count)
    {
        this.damage = damage;
        this.count += count;

        //���� = 0 , ���������� ���
        if(id == 0)
        {
            //��ġ�ؾ���
            this.Batch();
        }

        //...BroadcastMessage �ʱ�ȭ
        this.player.BroadcastMessage("ApplyGear",SendMessageOptions.DontRequireReceiver);
    }


    //...�ٰŸ� bullet(��) ��ġ�ϱ�
    void Batch()
    {
        for(int i = 0; i < this.count; i++)
        {
            //...bullet ���ӿ�����Ʈ ���� ��
            //Weapon�� �ڽ����� �ֱ� ���� transform������ ���������� �ֱ�
            Transform bullet;

            //...i�� childCount ���� �����
            if(i < this.transform.childCount)
            {
                //���� child�� ���
                bullet = this.transform.GetChild(i);
            }
            else
            {
                //...i�� ������ �Ѿ��
                //'���ڶ� �� ��ŭ' Ǯ������ ������
                bullet= GameManager.instance.pool.Get(prefabId).transform;
                //���� transform�� �θ�� ����
                bullet.parent = this.transform;
            }
           
            //...bullet��ġ player�� ��ġ�� �ʱ�ȭ
            //levelup ������ ��ġ ���� ����
            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            //...ȸ�� ����(z��) ����
            //count(����)��ŭ ȸ���� ����� ��ġs
            Vector3 rot = Vector3.forward * 360 * i / count;
            bullet.Rotate(rot);
            //Player�� ���� 1.5f �Ÿ���ŭ �������� ��ġ
            bullet.Translate(bullet.up * 1.5f, Space.World);

            //bullet �ʱ�ȭ
            bullet.GetComponent<Bullet>().Init(this.damage, -1,Vector3.zero);  // -1 == ����, ���� ����
        }
    }

    //...���Ÿ� Bullet(�Ѿ�) �߻�
    void Fire()
    {
        if(!this.player.scanner.nearestTarget)
        {
            return;
        }

        //���� ����� ���� ��ġ
        Vector3 targetPos = this.player.scanner.nearestTarget.position;
        //...�Ѿ� ���� ����
        Vector3 dir = targetPos - transform.position;
        //���� Vector�� ������ ����, ũ��� 1�� ��ȯ
        dir = dir.normalized;

        Transform bullet = GameManager.instance.pool.Get(prefabId).transform;
        bullet.position = this.transform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);

        //bullet �ʱ�ȭ
        bullet.GetComponent<Bullet>().Init(this.damage, count, dir);  // -1 == ����, ���� ����

        AudioManager.instance.PlaySfx(AudioManager.Sfx.Range);
    } 
}