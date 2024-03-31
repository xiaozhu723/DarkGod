/****************************************************
    文件：SkillItemBtn.cs
	作者：朱江
    邮箱:  839149608@qq.com
    日期：2024/3/29 15:50:2
	功能：技能按钮组件
*****************************************************/

using System;
using UnityEngine;
using UnityEngine.UI;

public class SkillItemBtn : MonoBehaviour 
{
    public int CurrSkillID;
    public Image m_SkillIcon;
    public Image m_ImgCD;
    public Image m_ImgLock;
    public Text m_CDText;
    public Button m_SkillBtn;
    private float CDTime;
    private float NowCDTime;
    int taskID;
    bool isCD;
    private void Start()
    {
        m_SkillBtn.onClick.AddListener(OnClickSkillBtn);
    }

    public void Init()
    {
        //根据数据加载技能icon
        //设置当前CD时间 技能是否解锁
    }

    public void SetCD(float time)
    {
        isCD = true;
        m_CDText.text = CDTime.ToString();
        CDTime = NowCDTime = time;
        m_ImgCD.gameObject.SetActive(true);
        m_ImgCD.fillAmount = 1;
        taskID = TimerService.Instance.AddTimerTask(UpdateCD, 1, PETimeUnit.Second, 0);
    }

    public void UpdateCD(int time)
    {
        NowCDTime -= 1;
        if(NowCDTime <= 0)
        {
            NowCDTime = 0;
            m_ImgCD.fillAmount = 0;
            m_ImgCD.gameObject.SetActive(false);
            TimerService.Instance.DeleteTimeTask(taskID);
            isCD = false;
            return;
        }
        m_ImgCD.fillAmount = NowCDTime / CDTime;
    }

    private void OnClickSkillBtn()
    {
        if (isCD) return;
        BattleSystem.Instance.RequestSkillAtt(CurrSkillID);
    }
}