/****************************************************
    文件：FuBenBtn.cs
	作者：朱江
    邮箱:  839149608@qq.com
    日期：2024/3/27 12:7:9
	功能：Nothing
*****************************************************/

using System;
using UnityEngine;
using UnityEngine.UI;

public class FuBenBtn : MonoBehaviour 
{
    public int SceneID;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClickBtn);
    }

    private void OnClickBtn()
    {
        GameInstanceSystem.Instance.RequestFuBenFight(10000+ SceneID);
    }
}