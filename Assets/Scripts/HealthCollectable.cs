using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollectable : MonoBehaviour
{
    //���ó�ҩ����Ƶ��Դ
    public AudioClip audioClip;
    //���ó�ҩ����Ч
    public GameObject effectPaticle;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //�������봥��������
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //����RubyController�����е�RubyController���
        RubyController controller = collision.GetComponent<RubyController>();
        //�жϽ�ɫ�Ƿ����ָ���ű�
        if (controller != null)
        {
            //�жϽ�ɫ�Ƿ���Ѫ
            if (controller.currentHealth < controller.maxHealth)
            {
                //��Ѫ
                controller.ChangeHealth(1);
                //Destroy:���ٻ�Ѫ��
                Destroy(gameObject);
                //���Ż�Ѫ��Ч
                controller.PlaySound(audioClip);
                //��¡��ҩ��Ч
                Instantiate(effectPaticle,transform.position,Quaternion.identity);
            }
        }
    }
}
