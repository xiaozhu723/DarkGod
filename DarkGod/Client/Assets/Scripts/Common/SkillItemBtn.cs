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
    private float currCDTime;
    public AttackType m_CurrAttackType;
    int taskID;
    bool isCD;
    private void Start()
    {
        if (m_CurrAttackType == AttackType.Attack)
        {
            m_SkillBtn.onClick.AddListener(OnClickNormalAttackBtn);
        }
        else
        {
            m_SkillBtn.onClick.AddListener(OnClickSkillBtn);
        }
       
    }

    public void Init(float time)
    {
        //根据数据加载技能icon
        //设置当前CD时间 技能是否解锁
        CDTime = time;
    }

    private void Update()
    {
        if(isCD)
        {
            float delta = Time.deltaTime;
            NowCDTime -= delta;
            if (NowCDTime <= 0)
            {
                isCD = false;
                m_ImgCD.gameObject.SetActive(false);
                m_ImgCD.fillAmount = 0;
                NowCDTime = 0;
            }
            else
            {
                int result = (int)Math.Ceiling(NowCDTime);
                m_CDText.text = result.ToString(); ;
                m_ImgCD.fillAmount = NowCDTime / currCDTime;
            }
        }
    }

    public void SetCD()
    {
        isCD = true;
        currCDTime = NowCDTime = CDTime / 1000;
        m_CDText.text = currCDTime.ToString();
        m_ImgCD.gameObject.SetActive(true);
        m_ImgCD.fillAmount = 1;
    }

    public void OnClickSkillBtn()
    {
        if (isCD|| !BattleSystem.Instance.BattleManager.GetCanSkill()) return;
        SetCD();
        BattleSystem.Instance.RequestSkillAtt(CurrSkillID);
    }

    private void OnClickNormalAttackBtn()
    {
        BattleSystem.Instance.RequestNormalAttack();
    }
}