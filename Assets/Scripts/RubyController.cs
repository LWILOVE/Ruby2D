using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubyController : MonoBehaviour
{
    //����Rigidbody2D�������
    private Rigidbody2D rigidbody2d;
    //�����ɫ�����Ѫ��
    public int maxHealth = 5;
    //�����ɫ��ĿǰѪ��
    public int currentHealth;
    //���ý�ɫ���ٶ�
    public int speed = 3;
    //���ý�ɫ�������޵�ʱ��
    public float timeInvincible = 2.0f;
    private bool isInvincible;
    //�����޵м�ʱ��
    private float invincibleTimer;
    //���ü�¼��ɫ����Ķ���
    private Vector2 lookDirection = new Vector2(1,0);
    //��ȡ����������
    private Animator animator;
    //�����ӵ�����Ԥ����
    public GameObject projectilePrefab;
    //������ƵԴ����
    private AudioSource audioSource;
    //������·��ƵԴ����
    public AudioSource walkAudioSource;
    //����������Ƶ��Դ����
    public AudioClip playerHit;
    //���ù�����Ƶ��Դ����
    public AudioClip attackSound;
    //������·��Ƶ��Դ����
    public AudioClip walkSound;
    //����Ruby��������
    private Vector3 respawnPosition;
    // Start is called before the first frame update
    void Start()
    {
        //Application.targetFrameRate:������Ϸ��֡��
        Application.targetFrameRate = 30;
        //����Rigidbody2D���
        rigidbody2d = GetComponent<Rigidbody2D>();
        //��ɫ��������Ѫ��
        currentHealth = maxHealth;
        //���ս�ɫ�Ķ����������
        animator = GetComponent<Animator>();
        //����AudioSource���
        audioSource = GetComponent<AudioSource>();
        //����Ruby�ĳ�����
        respawnPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //2D��Ϸ����Ҫ��Ӹ���������������˶�����Ϊû��Z��
        //������Ҫ�õ��������ʵģ�ʵ����3DҲ�ǣ�
        //Vector2���ͣ��洢XYֵ������    
        //Vector2 position = transform.position;
        //position.x = position.x + 0.1f;
        //transform.position = position;
        //Input.GetAxis():�����������
        //ʵ������Debug.log��printûɶ����
        //print(horizontal);
        //Debug.Log(horizontal);
        //AD��
        float horizontal = Input.GetAxis("Horizontal");
        //WS��
        float vertical = Input.GetAxis("Vertical");
        //ʵ�ֽ�ɫ����
        //�洢XY��ֵ
        Vector2 move = new Vector2(horizontal,vertical);
        //Mathf.Approximately:�ж����������Ƿ�������
        if (!Mathf.Approximately(move.x, 0) || !Mathf.Approximately(move.y, 0))
        {
            //����ɫ����������ն���
            lookDirection.Set(move.x, move.y);
            //ͬ��
            //lookDirection = move;
            //X.Normalize���淶������ֵ�������������1��-1
            lookDirection.Normalize();
            if (!walkAudioSource.isPlaying)
            {
                //�л�����Ϊ��·����
                walkAudioSource.clip = walkSound;
                //������·��Ч
                walkAudioSource.Play();
            }
        }
        else
        {
            //����Ҳ���ʱֹͣ������·����
            walkAudioSource.Stop();
        }
        //��������
        animator.SetFloat("Look X",lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        //��������ģ
        animator.SetFloat("Speed",move.magnitude);
        //ʵ��¶�ȵ��ƶ�
        //Time.deltaTime:��ȡÿ֡��Ҫ��ʱ��
        Vector2 position = transform.position;
        //position.x += speed*horizontal * Time.deltaTime;
        //position.y += speed*vertical * Time.deltaTime;
        //�����������Ч
        position += speed * move * Time.deltaTime*3;
        //transform.position = position;
        //��λ�øı��޸Ľ���rigidbody2d
        rigidbody2d.MovePosition(position);
        //�жϽ�ɫ�Ƿ����޵�״̬
        if (isInvincible)
        {
            //�����޵�ʱ��
            invincibleTimer -= Time.deltaTime;
            //����޵�״̬
            if (invincibleTimer <= 0)
            {
                isInvincible = false;   
            }
        }
        //����ϵͳ
        //Input.GetKeyDown(KeyCode.X):�ж�����Ƿ���X��
        if (Input.GetKeyDown(KeyCode.J))
        {
            //�����ӵ�
            Launch();
        }
        //NPC�Ի����
        if (Input.GetKeyDown(KeyCode.H))
        {
            //�ý�ɫ����������
            //����1�����ߵ����  ����2�����ߵķ���  ����3�����ߵĳ���  ����4�����߼��㼶
            //lookDirection�������棬��������Ѿ������ˣ����ǽ�ɫ��������
            //LayerMask.GetMask("X");��ȡX�㼶
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position+Vector2.up*0.2f,lookDirection,1.5f,LayerMask.GetMask("NPC"));
            if (hit.collider != null)
            {
                //��ȡNPCDialog�ű�
                NPCDialog npcDialog = hit.collider.GetComponent<NPCDialog>();
                //��ʾ�Ի���
                if (npcDialog != null)
                {
                    npcDialog.DiaplayDialog();
                }
            }
        }
    }

    //����ı�Ѫ���ķ���
    public void ChangeHealth(int amount)
    {
        //���˽����޵�״̬
        if (amount < 0)
        {
            //�޵�״̬���ã�������֮ǰ�ͷ��أ�
            if (isInvincible)
            {
                return;
            }
            //�ܵ��˺������޵�ʱ��
            isInvincible = true;
            invincibleTimer = timeInvincible;
            //���Ž�ɫ������Ч
            PlaySound(playerHit);
            //�����ܵ��˺�����
            animator.SetTrigger("Hit");
        }
        //Mathf.Clamp:����ɫ��Ѫ���������ֵʱǿ��Ŀ��ع鵽maxHealth
        //����1�����Ƶ���������2��Сֵ������3���ֵ
        currentHealth = Mathf.Clamp(currentHealth+amount,0,maxHealth);
        //����ɫ��Ѫ�����������̨
        //Debug.Log(currentHealth+"/"+maxHealth);
        //����Ѫ���ĵ������γ�Ѫ���䶯
        UIHealthBar.instance.SetValue(currentHealth/(float)maxHealth);
        //�ý�ɫ����
        if (currentHealth <= 0)
        {
            Respawn();
        }
    }
    //���巢���ӵ��ķ���
    private void Launch()
    {
        //���û�н����������޷������ӵ�
        if (!UIHealthBar.instance.hasTask)
        {
            return;
        }
        //��¡�ӵ�����,����һ������¡��Ϸ����(Ҳ����Ԥ����)������2������ֵ�λ�ã�����3�������ת�Ƕ�
        //Quaternion.identity���޽Ƕ���ת����       
        GameObject projectileObject = Instantiate(projectilePrefab,rigidbody2d.position+new Vector2(0.4f,0.8f),Quaternion.identity);
        //��ȡ�ӵ��Ľű�
        Projectile projectile = projectileObject.AddComponent<Projectile>();
        //�����ӵ�����ʹ�С
        projectile.Launch(lookDirection,300);
        //���ڽ�ɫ�Ķ���������������ģʽ
        animator.SetTrigger("Launch");
        //���Ź�����Ч
        PlaySound(attackSound);
    }
    //���岥�����ֵķ���
    public void PlaySound(AudioClip audioClip)
    {
        //PlayOneShot:ֻ����һ�ε�����
        audioSource.PlayOneShot(audioClip);
    }
    //���帴��Ruby�ķ���
    private void Respawn()
    {
        //�ָ�Ruby��Ѫ��
        ChangeHealth(maxHealth);
        //��Ruby�ص�������
        transform.position = respawnPosition;
    }
}
