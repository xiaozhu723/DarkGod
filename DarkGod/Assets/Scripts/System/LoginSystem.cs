/****************************************************
    文件：LoginSystem.cs
	作者：朱江
    邮箱:  839149608@qq.com
    日期：2024/3/10 15:48:33
	功能：登录系统
*****************************************************/

using UnityEngine;

public class LoginSystem : SystemRoot
{
    public override void Init()
    {
        base.Init();
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
}