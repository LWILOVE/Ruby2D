using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    //设置刚体组件对象
    private Rigidbody2D rigidBody2D;
    // Start is called before the first frame update
    //Awake:Unity自带方法，调用Start前会被调用，游戏物体实例化后也会立即调用
    void Awake()
    {
        //获取本游戏物体的刚体组件
        rigidBody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //判断模长是否大于1000，大于一千判断是意外子弹
        if (transform.position.magnitude > 1000)
        {
            //销毁意外子弹
            Destroy(gameObject);
        }
    }

    //定义给子弹施力的方法
    public void Launch(Vector2 direction, float force)
    {
        //给子弹施加力
        rigidBody2D.AddForce(direction*force);
    }
    //碰撞检测
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //设置敌人为目标碰撞对象
        EnemyController enemyController = collision.gameObject.GetComponent<EnemyController>();
        if (enemyController != null)
        {
            //修复机器人
            enemyController.Fix();
        }
        //碰撞到敌人后子弹消失
        Destroy(gameObject);
    }
}
