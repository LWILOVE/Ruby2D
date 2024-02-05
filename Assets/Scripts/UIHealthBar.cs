using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//引入UI库

public class UIHealthBar : MonoBehaviour
{
    //设置ImageUI对象
    public Image mask;
    //设置血条的宽度变量
    float originalSize;
    //设置静态单例  对内读写，对外只读
    //单例：全项目均可引用且唯一的实例
    public static UIHealthBar instance { get; private set; }
    //判断是否处于任务状态
    public bool hasTask;
    //判断任务是否完成
    //public bool ifCompleteTask;
    //修理数目变量
    public int fixedNum;
    //当血条对象设置时就初始化单例
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        //获取血条mask的宽度
        originalSize = mask.rectTransform.rect.width;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //设置当前UI血条显示值
    public void SetValue(float fillPercent)
    {
        //rectTransform.SetSizeWithCurrentAnchors:基于当前锚点填充图像尺寸
        //参数1：要填充的方向   参数2：填充值
        mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,originalSize*fillPercent);
    }
}
