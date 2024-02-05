using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//����UI��

public class UIHealthBar : MonoBehaviour
{
    //����ImageUI����
    public Image mask;
    //����Ѫ���Ŀ�ȱ���
    float originalSize;
    //���þ�̬����  ���ڶ�д������ֻ��
    //������ȫ��Ŀ����������Ψһ��ʵ��
    public static UIHealthBar instance { get; private set; }
    //�ж��Ƿ�������״̬
    public bool hasTask;
    //�ж������Ƿ����
    //public bool ifCompleteTask;
    //������Ŀ����
    public int fixedNum;
    //��Ѫ����������ʱ�ͳ�ʼ������
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        //��ȡѪ��mask�Ŀ��
        originalSize = mask.rectTransform.rect.width;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //���õ�ǰUIѪ����ʾֵ
    public void SetValue(float fillPercent)
    {
        //rectTransform.SetSizeWithCurrentAnchors:���ڵ�ǰê�����ͼ��ߴ�
        //����1��Ҫ���ķ���   ����2�����ֵ
        mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,originalSize*fillPercent);
    }
}
