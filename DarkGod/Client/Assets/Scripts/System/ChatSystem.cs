/****************************************************
	文件：ChatSystem.cs
	作者：朱江
	邮箱:  839149608@qq.com
	日期：2024/03/23 14:15   	
	功能：聊天管理类
*****************************************************/
using PEProtocol;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
public class ChatSystem : SystemRoot
{
    public ChatWindow chatWindow;
    private Dictionary<int, List<ChatItemData>> chatCashDic = new Dictionary<int, List<ChatItemData>>();
    public static ChatSystem Instance { get; private set; }
    private ChatType currChatType = ChatType.WordChat;
    int nID;
    string strPlayerName;
    public ChatType CurrChatType
    {
        get
        {
            return currChatType;
         }
        set
        {
            currChatType = value;
        }
    }

    public override void Init()
    {
        base.Init();
        Instance = this;
      
        Debug.Log("Init ChatSystem Succeed");
    }

    public void PushChat(GameMsg msg)
    {
        SetChatCash(msg.pushChat);
    }

    public void SendChat(string msg)
    {
        nID = GameRoot.Instance.PlayerData.ID;
        strPlayerName = GameRoot.Instance.PlayerData.Name;
        GameMsg gameMsg = new GameMsg
        {
            cmd = (int)EMCMD.SendChat,
            sendChat = new SendChat
            {
                nPlayerID = nID,
                nChatType =(int) CurrChatType,
                nChatInfo = msg,
                nName = strPlayerName,
            }
        };

        netServer.SendMessage(gameMsg);
    }

    public void SetChatCash(PushChat data)
    {
        ChatType type = (ChatType)data.nChatType;
        if (!chatCashDic.ContainsKey(data.nChatType))
        {
            chatCashDic.Add(data.nChatType, new List<ChatItemData>());
        }
        if (chatCashDic[data.nChatType].Count>Constants.ChatMaxCount)
        {
            chatCashDic[data.nChatType].Remove(chatCashDic[data.nChatType][0]);
        }
        ChatItemData temp = new ChatItemData()
        {
            Name = data.nName,
            ChatCont = data.nChatInfo,
            isMy = data.nPlayerID == GameRoot.Instance.PlayerData.ID,
        };
        chatCashDic[data.nChatType].Add(temp);

        //通知UI显示
        AddChatItem(temp);
    }

    public List<ChatItemData> GetChatCash()
    {
        if (!chatCashDic.ContainsKey((int)currChatType))
        {
            chatCashDic.Add((int)currChatType , new List<ChatItemData>()) ;
        }
        return chatCashDic[(int)currChatType];
    }


    public void AddChatItem(ChatItemData item)
    {
        if (!chatWindow.gameObject.activeSelf) return;
        chatWindow.AddChatItem(item);
    }
}
