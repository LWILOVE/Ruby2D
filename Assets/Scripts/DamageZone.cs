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
        //创建RubyController对象中的RubyController组件
        RubyController controller = collision.GetComponent<RubyController>();
        //判断角色是否带有指定脚本
        if (controller != null)
        {
            //掉血
            controller.ChangeHealth(-1);
        }
    }
}
