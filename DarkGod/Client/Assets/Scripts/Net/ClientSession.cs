/****************************************************
    文件：ClientSession.cs
	作者：朱江
    邮箱:  839149608@qq.com
    日期：2024/3/14 14:52:28
	功能：网络会话层
*****************************************************/

using UnityEngine;
using PEProtocol;
using PENet;

public class ClientSession : PESession<GameMsg> 
{
    protected override void OnConnected()
    {
        Debug.Log("Connected To  Server succ");
    }

    protected override void OnDisConnected()
    {
        Debug.Log("OnDisConnected.");
    }

    protected override void OnReciveMsg(GameMsg msg)
    {
        Debug.Log("OnReciveMsg  cmd: " + ((EMCMD)msg.cmd).ToString());
        NetServer.Instance.AddPackMsg(msg);
    }
}