/****************************************************
    文件：GameInstanceWindow.cs
	作者：朱江
    邮箱:  839149608@qq.com
    日期：2024/3/25 17:31:28
	功能：游戏副本界面
*****************************************************/

using PEProtocol;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameInstanceWindow : WindowRoot
{
    public Button m_CloseBtn;
    public Transform m_IconBtnParent;
    public Transform m_Pointer;
    private List<GameObject> IconBtnList = new List<GameObject>();
    PlayerData playerData;
    protected override void InitWindow()
    {
        base.InitWindow();
        for (int i = 0; i < m_IconBtnParent.childCount; i++)
        {
            GameObject go = m_IconBtnParent.GetChild(i).gameObject;
            SetActive(go, false);
            IconBtnList.Add(go);
        }
        Show();
    }

    public void Start()
    {
        m_CloseBtn.onClick.AddListener(OnClickCloseBtn);

    }
    
    public void Show()
    {
       
        RefreshUI();
    }

    public void RefreshUI()
    {
        playerData = GameRoot.Instance.PlayerData;
        int missionNum = playerData.missionNum%1000;
        for (int i = 0; i < IconBtnList.Count; i++)
        {
            if(i== missionNum-1)
            {
                SetActive(IconBtnList[i]);
                m_Pointer.parent = IconBtnList[i].transform;
                m_Pointer.localPosition = Vector3.zero;
                break;
            }
        }
    }

    private void OnClickCloseBtn()
    {
        SetWinState(false);
    }

}