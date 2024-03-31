/****************************************************
    文件：BuyTipsWindow.cs
	作者：朱江
    邮箱:  839149608@qq.com
    日期：2024/3/24 14:56:40
	功能：通用购买窗口
*****************************************************/

using PEProtocol;
using System;
using UnityEngine;
using UnityEngine.UI;

public class BuyTipsWindow : WindowRoot 
{
    public Button m_CloseBtn;
    public Button m_SureBtn;
    public Text m_CharText;
    private BuyType currType;
    PlayerData playerData;
    protected override void InitWindow()
    {
        base.InitWindow();
        m_SureBtn.interactable = true;
        playerData = GameRoot.Instance.PlayerData;
        RefreshUI();
    }

    public void Start()
    {
        m_CloseBtn.onClick.AddListener(OnClickCloseBtn);
        m_SureBtn.onClick.AddListener(OnClickSureBtn);
    }

    public void SetBuyType(BuyType type)
    {
        currType = type;
    }
    public void RefreshUI()
    {
        switch(currType)
        {
            case BuyType.BuyPower:
                m_CharText.text = "是否花费<color=#FF0000FF>10钻石</color>购买<color=#00FF00FF>100点体力</color>？";
                break;
            case BuyType.BuyCoin:
                m_CharText.text = "是否花费<color=#FF0000FF>10钻石</color>购买<color=#00FF00FF>1000金币</color>？";
                break;
        }
        
    }

    private void OnClickSureBtn()
    {
        m_SureBtn.interactable = false;
        int powerLimit = PECommon.GetPowerLimit(playerData.Level);
        Debug.LogError("powerLimit   " + powerLimit);
        if (playerData.Diamond<10)
        {
            GameRoot.AddTips("钻石不足！");
        }
        if (playerData.Power >= powerLimit)
        {
            GameRoot.AddTips("体力充足无需购买！");
        }
        MainCitySystem.Instance.RequestBuy((int)currType,10);
    }


    private void OnClickCloseBtn()
    {
        SetWinState(false);
    }
}