using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    //���õ��˵��ٶ�
    public float speed = 3;
    //���õ��˵ĸ������
    private Rigidbody2D rigidBody2D;
    //���Ƶ��˵��ƶ�����
    public bool vertical;
    //���Ƶ��˵��ƶ�����
    private int direction = 1;
    //���Ƹı䷽���ʱ����
    public float changeTime = 3.0f;
    //��ʱ��
    private float timer;
    //���ö������ƶ���
    private Animator animator;
    //�жϵ�ǰ�������Ƿ�����
    private bool broken;
    //��������ϵͳ����
    public ParticleSystem smokeEffect;
    //������ƵԴ����
    private AudioSource audioSource;
    //���ñ��޸���Ƶ��Դ����
    public AudioClip fixedSound;
    //���ñ�������Ƶ��Դ����
    public AudioClip[] hitSounds;
    //���õ���������Ч����
    public GameObject hitEffectParticle;
    // Start is called before the first frame update
    void Start()
    {
        //��ȡRigidBody2D���  ��ֱ���ϸ��ã���Ϊ���¿����������׶�
        rigidBody2D = GetComponent<Rigidbody2D>();
        //��ȡ���������ϵĶ����������
        animator = GetComponent<Animator>();
        //�������û������ж���
        //animator.SetFloat("MoveX", direction);
        //���ÿ��ֵĳ�ʼ״̬
        //animator.SetBool("Vertical",vertical);
        //���ƶ�������
        PlayMoveAnimation();
        //���û����˳�ʼ�ǻ���״̬
        broken = true;
        //��ȡ��������ƵԴ���
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //�жϻ������Ƿ�����
        if (!broken)
        {
            //��������������������ƶ�
            return;
        }
        //�õ����Լ�ת������
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            direction = -direction;
            //animator.SetFloat("MoveX",direction);
            PlayMoveAnimation();        
            timer = changeTime;
        }
        //��ȡ���˵�ǰλ��
        Vector2 position = rigidBody2D.position;
        //���Ƶ��˵��ƶ�����
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

    //���˵���ײ���
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //�ñ���ײ������ײ������ָ������
        RubyController rubyController = collision.gameObject.GetComponent<RubyController>();
        if (rubyController != null)
        {
            rubyController.ChangeHealth(-1);
        }
    }
    //�����˵��ƶ���װ�ɷ���
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
    //�޸�������
    public void Fix()
    {
        //��¡һ��������Ч
        Instantiate(hitEffectParticle,transform.position,Quaternion.identity);
        broken = false;
        //ʹ�����˲����Ǹ���
        rigidBody2D.simulated = false;
        //���ű��޸������˵Ķ���
        animator.SetTrigger("Fixed");
        //��ͣ����ϵͳ �����ٸ���ʵ
        smokeEffect.Stop();
        //��ͣ��·��Ч
        audioSource.Stop();
        //��������������Ʋ��ŵ���������
        int randomNum = Random.Range(0, 2);
            //�Ŵ�����˱�����ʱ�Ļ�����Ч
            audioSource.volume = 0.8f;
            //���ű�������Ч,���Ĳ���2�������������ڣ����ǷŴ�����
            audioSource.PlayOneShot(hitSounds[randomNum]);
        //�����޸���ʱ
        //Invoke:��ʱ��������
        //����1����ʱ����   ����2����ʱʱ����λ����
        Invoke("PlayFixedSound",1.05f);
        //��¼���޺õĻ�������Ŀ
        UIHealthBar.instance.fixedNum++;
    }
    //�����޸���Ч����
    private void PlayFixedSound()
    {
        //���ű��޸���Ч
        audioSource.PlayOneShot(fixedSound);
        //Invoke("StopAudioSourcePlay", 1.3f);
    }
    //����ͣ�����ַ���
    //private void StopAudioSourcePlay()
    //{
    //    //�������ֹͣ���֣���Ȼ����Ϊ����LOOP��true������ѭ����
    //    audioSource.Stop();
    //}
}   
