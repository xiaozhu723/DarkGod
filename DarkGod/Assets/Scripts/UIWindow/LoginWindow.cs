/****************************************************
    文件：LoginWindow.cs
	作者：朱江
    邮箱:  839149608@qq.com
    日期：2024/3/10 18:37:0
	功能：登录界面
*****************************************************/

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

    private void SetAccount(string account)
    {
        Debug.Log("account:" + account);
    }

    private void SetPassword(string password)
    {
        Debug.Log("password:" + password);
    }
}