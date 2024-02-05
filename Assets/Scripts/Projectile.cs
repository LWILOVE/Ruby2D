using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    //���ø����������
    private Rigidbody2D rigidBody2D;
    // Start is called before the first frame update
    //Awake:Unity�Դ�����������Startǰ�ᱻ���ã���Ϸ����ʵ������Ҳ����������
    void Awake()
    {
        //��ȡ����Ϸ����ĸ������
        rigidBody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //�ж�ģ���Ƿ����1000������һǧ�ж��������ӵ�
        if (transform.position.magnitude > 1000)
        {
            //���������ӵ�
            Destroy(gameObject);
        }
    }

    //������ӵ�ʩ���ķ���
    public void Launch(Vector2 direction, float force)
    {
        //���ӵ�ʩ����
        rigidBody2D.AddForce(direction*force);
    }
    //��ײ���
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //���õ���ΪĿ����ײ����
        EnemyController enemyController = collision.gameObject.GetComponent<EnemyController>();
        if (enemyController != null)
        {
            //�޸�������
            enemyController.Fix();
        }
        //��ײ�����˺��ӵ���ʧ
        Destroy(gameObject);
    }
}
