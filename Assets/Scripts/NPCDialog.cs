using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//����UI��

public class NPCDialog : MonoBehaviour
{
    //������Ϸ�Ի������
    public GameObject dialogBox;
    //���öԻ���չʾ��ʱ��
    public float displayTime = 3.0f;
    private float timerDisplay;
    //���öԻ���UI����
    public Text dialogText;
    //������ƵԴ����
    public AudioSource audioSource;
    //������Ƶ��Դ����
    public AudioClip comepleteTaskClip;
    //���ÿ���ʤ����Чֻ����һ��
    private bool hasPlayed;
    // Start is called before the first frame update
    void Start()
    {
        //���������ضԻ���
        dialogBox.SetActive(false);
        //��ʼ��ʱ
        timerDisplay = -1;
    }

    // Update is called once per frame
    void Update()
    {
        //��ʾʱ�䵹��
        if (timerDisplay >= 0)
        {
            timerDisplay -= Time.deltaTime;
            if (timerDisplay < 0)
            {
                dialogBox.SetActive(false);
            }
        }
    }
    //���ƶԻ��������
    public void DiaplayDialog()
    {
        //��ʼ��ʾ
        timerDisplay = displayTime;
        dialogBox.SetActive(true);
        //��������
        UIHealthBar.instance.hasTask = true;
        //�ж������Ƿ����
        if (UIHealthBar.instance.fixedNum>=7)
        {
            //���öԻ��������
            dialogText.text = "��л���Ŭ����";
            if (!hasPlayed)
            {
                //�������������Ч
                audioSource.PlayOneShot(comepleteTaskClip);
                hasPlayed = true;
            }
        }
    }
}
