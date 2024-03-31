/****************************************************
    文件：ChatItem.cs
	作者：朱江
    邮箱:  839149608@qq.com
    日期：2024/3/23 14:29:13
	功能：聊天Item
*****************************************************/

using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ChatItem : UIItem
{
    public Text m_PlayerName;
    public Text m_ChatCont;
    public Text m_MyName;
    public Text m_MyChatCont;
    public override void Show()
    {
        base.Show();
    }
    public override void SetData(UIItemData data)
    {
        ChatItemData temp = data as ChatItemData;
        if(temp.isMy)
        {
            m_PlayerName.gameObject.SetActive(false);
            m_MyName.gameObject.SetActive(true);
            m_ChatCont.gameObject.SetActive(false);
            m_MyChatCont.gameObject.SetActive(true);
            m_MyName.text = ":"+temp.Name;
            m_MyChatCont.text = temp.ChatCont;
        }
        else
        {
            m_PlayerName.gameObject.SetActive(true);
            m_MyName.gameObject.SetActive(false);
            m_ChatCont.gameObject.SetActive(true);
            m_MyChatCont.gameObject.SetActive(false);
            m_PlayerName.text = temp.Name + ":";
            m_ChatCont.text = temp.ChatCont;
        }
        
    }
}

