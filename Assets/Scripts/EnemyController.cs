using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    //设置敌人的速度
    public float speed = 3;
    //设置敌人的刚体对象
    private Rigidbody2D rigidBody2D;
    //控制敌人的移动方向
    public bool vertical;
    //控制敌人的移动方向
    private int direction = 1;
    //控制改变方向的时间间隔
    public float changeTime = 3.0f;
    //计时器
    private float timer;
    //设置动画控制对象
    private Animator animator;
    //判断当前机器人是否正常
    private bool broken;
    //设置粒子系统对象
    public ParticleSystem smokeEffect;
    //设置音频源对象
    private AudioSource audioSource;
    //设置被修复音频资源对象
    public AudioClip fixedSound;
    //设置被命中音频资源数组
    public AudioClip[] hitSounds;
    //设置敌人命中特效变量
    public GameObject hitEffectParticle;
    // Start is called before the first frame update
    void Start()
    {
        //获取RigidBody2D组件  比直接拖更好，因为更新看起来更容易懂
        rigidBody2D = GetComponent<Rigidbody2D>();
        //获取机器人身上的动画控制组件
        animator = GetComponent<Animator>();
        //开局先让机器人有动作
        //animator.SetFloat("MoveX", direction);
        //设置开局的初始状态
        //animator.SetBool("Vertical",vertical);
        //控制动画轴向
        PlayMoveAnimation();
        //设置机器人初始是坏的状态
        broken = true;
        //获取机器人音频源组件
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //判断机器人是否正常
        if (!broken)
        {
            //如果机器人正常，则不再移动
            return;
        }
        //让敌人自己转换方向
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            direction = -direction;
            //animator.SetFloat("MoveX",direction);
            PlayMoveAnimation();        
            timer = changeTime;
        }
        //获取敌人当前位置
        Vector2 position = rigidBody2D.position;
        //控制敌人的移动方向
        if (vertical)
        {
            position.y += Time.deltaTime * speed * direction;
        }
        else
        {
            position.x += speed * Time.deltaTime * direction;
        }
        rigidBody2D.MovePosition(position);
    }

    //敌人的碰撞检测
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //让本碰撞器的碰撞器参数指向主角
        RubyController rubyController = collision.gameObject.GetComponent<RubyController>();
        if (rubyController != null)
        {
            rubyController.ChangeHealth(-1);
        }
    }
    //将敌人的移动封装成方法
    private void PlayMoveAnimation()
    {
        if (vertical)
        {
            animator.SetFloat("MoveX", 0);
            animator.SetFloat("MoveY", direction);
        }
        else
        {
            animator.SetFloat("MoveX", direction);
            animator.SetFloat("MoveY", 0);
        }
    }
    //修复机器人
    public void Fix()
    {
        //克隆一个击中特效
        Instantiate(hitEffectParticle,transform.position,Quaternion.identity);
        broken = false;
        //使机器人不再是刚体
        rigidBody2D.simulated = false;
        //播放被修复机器人的动画
        animator.SetTrigger("Fixed");
        //暂停粒子系统 比销毁更真实
        smokeEffect.Stop();
        //暂停走路音效
        audioSource.Stop();
        //设置随机数来控制播放的音乐种类
        int randomNum = Random.Range(0, 2);
            //放大机器人被攻击时的互动音效
            audioSource.volume = 0.8f;
            //播放被攻击音效,它的参数2的音量比例调节，不是放大音量
            audioSource.PlayOneShot(hitSounds[randomNum]);
        //设置修复延时
        //Invoke:延时方法调用
        //参数1：延时方法   参数2：延时时长单位：秒
        Invoke("PlayFixedSound",1.05f);
        //记录被修好的机器人数目
        UIHealthBar.instance.fixedNum++;
    }
    //定义修复音效方法
    private void PlayFixedSound()
    {
        //播放被修复音效
        audioSource.PlayOneShot(fixedSound);
        //Invoke("StopAudioSourcePlay", 1.3f);
    }
    //定义停放音乐方法
    //private void StopAudioSourcePlay()
    //{
    //    //播放完后停止音乐（不然会因为外面LOOP是true而不断循环）
    //    audioSource.Stop();
    //}
}   
