/****************************************************
    文件：UIItem.cs
	作者：朱江
    邮箱:  839149608@qq.com
    日期：2024/3/19 16:3:44
	功能：Item基类
*****************************************************/

using System.Collections.Generic;
using UnityEngine;

public class UIItem : MonoBehaviour 
{
    public int nIndex;

    public virtual void Show()
    {
        gameObject.SetActive(true);

    }

    public virtual void SetData(UIItemData data)
    {

    }
}

public class UIItemData
{
    public int nIndex;
}