using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubyController : MonoBehaviour
{
    //创建Rigidbody2D组件对象
    private Rigidbody2D rigidbody2d;
    //定义角色的最大血量
    public int maxHealth = 5;
    //定义角色的目前血量
    public int currentHealth;
    //设置角色的速度
    public int speed = 3;
    //设置角色的受伤无敌时间
    public float timeInvincible = 2.0f;
    private bool isInvincible;
    //设置无敌计时器
    private float invincibleTimer;
    //设置记录角色看向的对象
    private Vector2 lookDirection = new Vector2(1,0);
    //获取动画控制器
    private Animator animator;
    //设置子弹对象预制体
    public GameObject projectilePrefab;
    //设置音频源对象
    private AudioSource audioSource;
    //设置走路音频源对象
    public AudioSource walkAudioSource;
    //设置受伤音频资源对象
    public AudioClip playerHit;
    //设置攻击音频资源对象
    public AudioClip attackSound;
    //设置走路音频资源对象
    public AudioClip walkSound;
    //设置Ruby重生变量
    private Vector3 respawnPosition;
    // Start is called before the first frame update
    void Start()
    {
        //Application.targetFrameRate:设置游戏的帧率
        Application.targetFrameRate = 30;
        //接收Rigidbody2D组件
        rigidbody2d = GetComponent<Rigidbody2D>();
        //角色开局是满血的
        currentHealth = maxHealth;
        //接收角色的动画控制组件
        animator = GetComponent<Animator>();
        //接收AudioSource组件
        audioSource = GetComponent<AudioSource>();
        //保存Ruby的出生点
        respawnPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //2D游戏不需要添加刚体来控制物体的运动，因为没有Z轴
        //除非有要用到物理性质的（实际上3D也是）
        //Vector2类型：存储XY值的类型    
        //Vector2 position = transform.position;
        //position.x = position.x + 0.1f;
        //transform.position = position;
        //Input.GetAxis():输入监听方法
        //实践表明Debug.log和print没啥区别
        //print(horizontal);
        //Debug.Log(horizontal);
        //AD键
        float horizontal = Input.GetAxis("Horizontal");
        //WS键
        float vertical = Input.GetAxis("Vertical");
        //实现角色动画
        //存储XY的值
        Vector2 move = new Vector2(horizontal,vertical);
        //Mathf.Approximately:判断两个参数是否近似相等
        if (!Mathf.Approximately(move.x, 0) || !Mathf.Approximately(move.y, 0))
        {
            //将角色的面向传入接收对象
            lookDirection.Set(move.x, move.y);
            //同上
            //lookDirection = move;
            //X.Normalize：规范化对象值，根据情况返回1或-1
            lookDirection.Normalize();
            if (!walkAudioSource.isPlaying)
            {
                //切换音乐为走路音乐
                walkAudioSource.clip = walkSound;
                //播放走路音效
                walkAudioSource.Play();
            }
        }
        else
        {
            //当玩家不动时停止播放走路音乐
            walkAudioSource.Stop();
        }
        //动画控制
        animator.SetFloat("Look X",lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        //求向量的模
        animator.SetFloat("Speed",move.magnitude);
        //实现露比的移动
        //Time.deltaTime:获取每帧需要的时间
        Vector2 position = transform.position;
        //position.x += speed*horizontal * Time.deltaTime;
        //position.y += speed*vertical * Time.deltaTime;
        //与上面两句等效
        position += speed * move * Time.deltaTime*3;
        //transform.position = position;
        //将位置改变修改交给rigidbody2d
        rigidbody2d.MovePosition(position);
        //判断角色是否处于无敌状态
        if (isInvincible)
        {
            //减少无敌时间
            invincibleTimer -= Time.deltaTime;
            //解除无敌状态
            if (invincibleTimer <= 0)
            {
                isInvincible = false;   
            }
        }
        //攻击系统
        //Input.GetKeyDown(KeyCode.X):判断玩家是否按下X键
        if (Input.GetKeyDown(KeyCode.J))
        {
            //发射子弹
            Launch();
        }
        //NPC对话检测
        if (Input.GetKeyDown(KeyCode.H))
        {
            //让角色发射检测射线
            //参数1：射线的起点  参数2：射线的方向  参数3：射线的长度  参数4：测线检测层级
            //lookDirection：看上面，这个变量已经定义了，就是角色的面向方向
            //LayerMask.GetMask("X");获取X层级
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position+Vector2.up*0.2f,lookDirection,1.5f,LayerMask.GetMask("NPC"));
            if (hit.collider != null)
            {
                //获取NPCDialog脚本
                NPCDialog npcDialog = hit.collider.GetComponent<NPCDialog>();
                //显示对话框
                if (npcDialog != null)
                {
                    npcDialog.DiaplayDialog();
                }
            }
        }
    }

    //定义改变血量的方法
    public void ChangeHealth(int amount)
    {
        //受伤进入无敌状态
        if (amount < 0)
        {
            //无敌状态设置（在受伤之前就返回）
            if (isInvincible)
            {
                return;
            }
            //受到伤害进入无敌时间
            isInvincible = true;
            invincibleTimer = timeInvincible;
            //播放角色受伤音效
            PlaySound(playerHit);
            //播放受到伤害动画
            animator.SetTrigger("Hit");
        }
        //Mathf.Clamp:当角色的血量超过最大值时强制目标回归到maxHealth
        //参数1被控制的数，参数2最小值。参数3最大值
        currentHealth = Mathf.Clamp(currentHealth+amount,0,maxHealth);
        //将角色的血量输出到控制台
        //Debug.Log(currentHealth+"/"+maxHealth);
        //调用血条的单例来形成血条变动
        UIHealthBar.instance.SetValue(currentHealth/(float)maxHealth);
        //让角色重生
        if (currentHealth <= 0)
        {
            Respawn();
        }
    }
    //定义发射子弹的方法
    private void Launch()
    {
        //如果没有接受任务则无法发射子弹
        if (!UIHealthBar.instance.hasTask)
        {
            return;
        }
        //克隆子弹对象,参数一：待克隆游戏物体(也能是预制体)，参数2物体出现的位置，参数3物体的旋转角度
        //Quaternion.identity：无角度旋转出现       
        GameObject projectileObject = Instantiate(projectilePrefab,rigidbody2d.position+new Vector2(0.4f,0.8f),Quaternion.identity);
        //获取子弹的脚本
        Projectile projectile = projectileObject.AddComponent<Projectile>();
        //传入子弹方向和大小
        projectile.Launch(lookDirection,300);
        //调节角色的动画控制器到发射模式
        animator.SetTrigger("Launch");
        //播放攻击音效
        PlaySound(attackSound);
    }
    //定义播放音乐的方法
    public void PlaySound(AudioClip audioClip)
    {
        //PlayOneShot:只播放一次的音乐
        audioSource.PlayOneShot(audioClip);
    }
    //定义复活Ruby的方法
    private void Respawn()
    {
        //恢复Ruby的血量
        ChangeHealth(maxHealth);
        //让Ruby回到出生点
        transform.position = respawnPosition;
    }
}
