/****************************************************
    文件：TaskItem.cs
	作者：朱江
    邮箱:  839149608@qq.com
    日期：2024/3/25 9:42:46
	功能：任务item
*****************************************************/

using PEProtocol;
using System;
using UnityEngine;
using UnityEngine.UI;

public class TaskItem : UIItem
{
    public Text m_TaskName;
    public Text m_ExpNum;
    public Text m_CoinNum;
    public Text m_ProgressText;
    public GameObject m_DisBtn;
    public Image m_ImageComp;
    public Image m_ProgressImage;
    public Button m_btnTake;
    TaskItemData tData;
    private void Start()
    {
        m_btnTake.onClick.AddListener(OnClickTakeBtn);
    }

    public override void Show()
    {
        base.Show();
    }
    public override void SetData(UIItemData data)
    {
        if (data == null) return;
        tData = data as TaskItemData;
        m_TaskName.text = tData.strTaskName;
        m_ExpNum.text = string.Format("奖励：    经验{0}", tData.nAddExpNum);
        m_CoinNum.text = string.Format("金币{0}", tData.nAddCoinNum);
        m_ProgressText.text = string.Format("{0}/{1}", tData.nCurrPro, tData.nMaxPro);
        m_DisBtn.SetActive(tData.nCurrType != ReceiveType.Unclaim);
        m_ImageComp.gameObject.SetActive(tData.nCurrType == ReceiveType.Already);
        m_ProgressImage.fillAmount = tData.nCurrPro*1.0f/ tData.nMaxPro;
    }
    private void OnClickTakeBtn()
    {
        if (tData == null) return;
        MainCitySystem.Instance.RequestTaskReceive(tData.nID);
    }
}

public class TaskItemData : UIItemData
{
    public int nID;
    public string strTaskName;
    public int nAddExpNum;
    public int nAddCoinNum;
    public int nMaxPro;
    public int nCurrPro;
    public ReceiveType nCurrType;
}