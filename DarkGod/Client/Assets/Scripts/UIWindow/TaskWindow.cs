/****************************************************
    文件：TaskWindow.cs
	作者：朱江
    邮箱:  839149608@qq.com
    日期：2024/3/25 11:8:37
	功能：任务窗口
*****************************************************/

using PEProtocol;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class TaskWindow : WindowRoot 
{
    public Button m_CloseBtn;
    public PEGrid tGrid;
    List<TaskData> dataList;
    PlayerData playerData;
    protected override void InitWindow()
    {
        base.InitWindow();
        RefreshUI();
    }

    public void Start()
    {
        m_CloseBtn.onClick.AddListener(OnClickCloseBtn);

    }

    public void RefreshUI()
    {
        playerData = GameRoot.Instance.PlayerData;
        dataList = resService.GetTaskDataList();
        List<UIItemData> list = new List<UIItemData>();
        for (int i = 0; i < dataList.Count; i++)
        {
            TaskItemData data = new TaskItemData();
            data.nID = dataList[i].ID;
            data.strTaskName = dataList[i].strTaskName;
            data.nMaxPro =dataList[i].nMaxCount ;
            data.nAddExpNum = dataList[i].nExp ;
            data.nAddCoinNum = dataList[i].nCoin ;
            
            data.nCurrPro = playerData.taskArr[i];
            data.nCurrType = (ReceiveType)playerData.taskReceiveArr[i] == ReceiveType.None ? ReceiveType.Unfinish : (ReceiveType)playerData.taskReceiveArr[i];
            list.Add(data);
        }
        list.Sort((a, b) =>
        {
            TaskItemData temp1 = a as TaskItemData;
            TaskItemData temp2 = b as TaskItemData;
            if (temp1.nCurrType != temp2.nCurrType)
            {
                return temp1.nCurrType.CompareTo(temp2.nCurrType);
            }
            else 
            {
                return temp2.nID.CompareTo(temp1.nID);
            }
        });
        tGrid.UpdateItemList(list);
    }

    private void OnClickCloseBtn()
    {
        SetWinState(false);
    }
}


