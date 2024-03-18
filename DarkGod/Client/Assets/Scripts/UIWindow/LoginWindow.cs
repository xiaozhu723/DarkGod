/****************************************************
    文件：LoginWindow.cs
	作者：朱江
    邮箱:  839149608@qq.com
    日期：2024/3/10 18:37:0
	功能：登录界面
*****************************************************/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginWindow : WindowRoot
{
    public InputField m_inputAcc;
    public InputField m_inputPass;
    public Button m_LoginBtn;
    public Button m_NoticeBtn;

    private void Start()
    {
        //m_inputAcc.onEndEdit.AddListener(SetAccount);
        //m_inputPass.onEndEdit.AddListener(SetPassword);
        m_LoginBtn.onClick.AddListener(OnClickLoginBtn);
        m_NoticeBtn.onClick.AddListener(OnClickNoticeBtn);
    }

    protected override void InitWindow()
    {
        base.InitWindow();
        if (PlayerPrefs.HasKey("Account") && PlayerPrefs.HasKey("Password"))
        {
            m_inputAcc.text = PlayerPrefs.GetString("Account");
            m_inputPass.text = PlayerPrefs.GetString("Password");
        }
        else
        {
            m_inputAcc.text = "";
            m_inputPass.text = "";
        }
    }

    public void OnClickLoginBtn()
    {
        audioService.PlayUIMusic(Constants.BGLoginBtn);
        string acct = m_inputAcc.text;
        string Pass = m_inputPass.text;
        if (string.IsNullOrEmpty(acct) || string.IsNullOrEmpty(Pass))
        {
            GameRoot.AddTips("账号密码不能为空。");
            return;
        }
        PlayerPrefs.SetString("Account", acct);
        PlayerPrefs.SetString("Password", Pass);
        LoginSystem.Instance.RequestLogin(acct, Pass);

    }

    public void OnClickNoticeBtn()
    {
        GameRoot.AddTips("功能正在开发中....");
    }
}