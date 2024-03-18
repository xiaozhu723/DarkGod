/****************************************************
    文件：CreaterWindow.cs
	作者：朱江
    邮箱:  839149608@qq.com
    日期：2024/3/12 12:33:16
	功能：Nothing
*****************************************************/

using PEProtocol;
using UnityEngine;
using UnityEngine.UI;

public class CreaterWindow : WindowRoot 
{
    public Button m_RadomBtn;
    public Button m_EnterGameButton;
    public InputField m_input;
    protected override void InitWindow()
    {
        base.InitWindow();
    }


    private void Start()
    {
        m_RadomBtn.onClick.AddListener(OnClickRadomBtn);
        m_EnterGameButton.onClick.AddListener(OnClickm_EnterGameButton);
        m_input.text = resService.GetRadomName(false);
    }

    public void OnClickRadomBtn()
    {
        audioService.PlayUIMusic(Constants.UIClickBtn);
        string name  =  resService.GetRadomName(false);
        m_input.text  = name;
    }

    public void OnClickm_EnterGameButton()
    {
        audioService.PlayUIMusic(Constants.UIClickBtn);
        if(string.IsNullOrEmpty(m_input.text))
        {
            GameRoot.AddTips("请先赋予角色名字！");
        }
        else
        {
            GameMsg msg = new GameMsg
            {
                cmd = (int)EMCMD.RequestRename,
                reqRename = new RequestRename { strName = m_input.text, }
            };
            netServer.SendMessage(msg);
        }
    }
}