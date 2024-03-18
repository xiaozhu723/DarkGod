/****************************************************
    文件：LoginSystem.cs
	作者：朱江
    邮箱:  839149608@qq.com
    日期：2024/3/10 15:48:33
	功能：登录系统
*****************************************************/

using PEProtocol;
using UnityEngine;

public class LoginSystem : SystemRoot
{
    public static LoginSystem Instance { get; private set; }
    public  CreaterWindow createrWindow;
    public override void Init()
    {
        base.Init();
        Instance = this;    
        Debug.Log("Init LoginSystem....");
    }

    //进入登陆场景
    public void EnterLoding()
    {
        //TODO
        //异步加载登陆场景
        //并显示加载进度
        //加载完成之后在显示登录注册界面
        GameRoot.Instance.lodingWindow.SetWinState();
        resService.AsyncLoadScene(Constants.LoginSceneName, () =>
        {
            audioService.PlayBGMusic(Constants.BGLogin, true);
            OpenLoginWindow();
        });

    }

    //打开登陆界面
    public void OpenLoginWindow()
    {
        GameRoot.Instance.loginWindow.SetWinState();
    }

    //发送消息进入游戏
    public void RequestLogin(string username, string password)
    {
        GameMsg gameMsg = new GameMsg
        {
            cmd = (int)EMCMD.RequestLogin,
            reqLogin = new RequestLogin { 
                strAcct = username,
                strPass = password,
            }
        };
       
        netServer.SendMessage(gameMsg);
    }

    public void ResponseLogin(GameMsg msg)
    {
        GameRoot.AddTips("登录游戏成功！");
        GameRoot.Instance.SetPlayerData(msg.resLogin.playerData);
        if (string.IsNullOrEmpty(msg.resLogin.playerData.Name))
        {
            if (createrWindow)
                createrWindow.SetWinState();
        }
        else
        {
            //进入主城
            MainCitySystem.Instance.EnterMainCity();
        }
        GameRoot.Instance.loginWindow.SetWinState(false);
    }

    //重命名
    public void ResponseRename(GameMsg msg)
    {
        //GameRoot.AddTips("登录游戏成功！");
        if (string.IsNullOrEmpty(msg.rspRename.strName))
        {
            return;
        }
        else
        {
            GameRoot.Instance.SetPlayerName(msg.rspRename.strName);
       
            if (createrWindow)
                createrWindow.SetWinState(false);
            //进入主城
            MainCitySystem.Instance.EnterMainCity();
        }
    }
}