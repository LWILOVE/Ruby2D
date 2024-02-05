using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//引入UI库

public class NPCDialog : MonoBehaviour
{
    //设置游戏对话框对象
    public GameObject dialogBox;
    //设置对话框展示的时间
    public float displayTime = 3.0f;
    private float timerDisplay;
    //设置对话框UI对象
    public Text dialogText;
    //设置音频源对象
    public AudioSource audioSource;
    //设置音频资源变量
    public AudioClip comepleteTaskClip;
    //设置控制胜利音效只播放一次
    private bool hasPlayed;
    // Start is called before the first frame update
    void Start()
    {
        //开局先隐藏对话框
        dialogBox.SetActive(false);
        //开始计时
        timerDisplay = -1;
    }

    // Update is called once per frame
    void Update()
    {
        //显示时间倒数
        if (timerDisplay >= 0)
        {
            timerDisplay -= Time.deltaTime;
            if (timerDisplay < 0)
            {
                dialogBox.SetActive(false);
            }
        }
    }
    //控制对话框的显隐
    public void DiaplayDialog()
    {
        //开始显示
        timerDisplay = displayTime;
        dialogBox.SetActive(true);
        //接受任务
        UIHealthBar.instance.hasTask = true;
        //判断任务是否完成
        if (UIHealthBar.instance.fixedNum>=7)
        {
            //设置对话框的内容
            dialogText.text = "感谢你的努力！";
            if (!hasPlayed)
            {
                //播放任务完成音效
                audioSource.PlayOneShot(comepleteTaskClip);
                hasPlayed = true;
            }
        }
    }
}
