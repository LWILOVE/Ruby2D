using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        //����RubyController�����е�RubyController���
        RubyController controller = collision.GetComponent<RubyController>();
        //�жϽ�ɫ�Ƿ����ָ���ű�
        if (controller != null)
        {
            //��Ѫ
            controller.ChangeHealth(-1);
        }
    }
}
