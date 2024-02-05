using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollectable : MonoBehaviour
{
    //设置吃药的音频资源
    public AudioClip audioClip;
    //设置吃药的特效
    public GameObject effectPaticle;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //创建进入触发器方法
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //创建RubyController对象中的RubyController组件
        RubyController controller = collision.GetComponent<RubyController>();
        //判断角色是否带有指定脚本
        if (controller != null)
        {
            //判断角色是否满血
            if (controller.currentHealth < controller.maxHealth)
            {
                //回血
                controller.ChangeHealth(1);
                //Destroy:销毁回血物
                Destroy(gameObject);
                //播放回血音效
                controller.PlaySound(audioClip);
                //克隆吃药特效
                Instantiate(effectPaticle,transform.position,Quaternion.identity);
            }
        }
    }
}
