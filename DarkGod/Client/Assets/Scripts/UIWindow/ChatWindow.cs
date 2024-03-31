/****************************************************
    文件：ChatWindow.cs
	作者：朱江
    邮箱:  839149608@qq.com
    日期：2024/3/23 13:54:48
	功能：聊天窗口
*****************************************************/

using PEProtocol;
using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ChatWindow : WindowRoot 
{
    public Button m_CloseBtn;
    public Toggle m_WordToggle;
    public Toggle m_UnionToggle;
    public Toggle m_FriendToggle;
    public Text m_TipsText;
    public PEGrid chatGrid;
    ChatSystem chatSystem;
    PlayerData playerData;
    List<ChatItemData> chatList = new List<ChatItemData>();
    protected override void InitWindow()
    {
        base.InitWindow();
        chatSystem = ChatSystem.Instance;
        playerData = GameRoot.Instance.PlayerData;
        Show();
    }

    public void Start()
    {
        m_CloseBtn.onClick.AddListener(OnClickCloseBtn);
        m_WordToggle.onValueChanged.AddListener(OnClickWordToggle);
        m_UnionToggle.onValueChanged.AddListener(OnClickUnionToggle);
        m_FriendToggle.onValueChanged.AddListener(OnClickFriendToggle);
    }



    public void Show()
    {
        m_WordToggle.isOn = true;
        OnClickWordToggle(true);
    }

    public void RefreshUI()
    {
        m_TipsText.text = "";
        if (chatSystem.CurrChatType == ChatType.UnionChat)
        {
            m_TipsText.text = "暂未加入公会";
            return;
        }
        if (chatSystem.CurrChatType == ChatType.FriendChat)
        {
            m_TipsText.text = "暂无好友信息";
            return;
        }
        chatList = chatSystem.GetChatCash();
        if (chatList.Count <= 0) return;
        List<UIItemData> tData = new List<UIItemData>();
        for (int i = 0; i < chatList.Count; i++)
        {
            tData.Add(chatList[i]);
        }
        chatGrid.UpdateItemList(tData);
    }

    public void AddChatItem(ChatItemData item)
    {
        chatGrid.AddItem(new List<UIItemData>()
        {
            item
        }) ;
    }

    private void OnClickFriendToggle(bool arg0)
    {
        if (!arg0) return;
        PECommon.Log("OnClickFriendToggle");
        chatSystem.CurrChatType = ChatType.FriendChat;
        RefreshUI();
    }

    private void OnClickUnionToggle(bool arg0)
    {
        if (!arg0) return;
        PECommon.Log("OnClickUnionToggle");
        chatSystem.CurrChatType = ChatType.UnionChat;
        RefreshUI();
    }

    private void OnClickWordToggle(bool arg0)
    {
        if (!arg0) return;
        PECommon.Log("OnClickWordToggle");
        chatSystem.CurrChatType = ChatType.WordChat;
        RefreshUI();
    }

    private void OnClickCloseBtn()
    {
        audioService.PlayUIMusic(Constants.UIClickBtn);
        SetWinState(false);
    }
}