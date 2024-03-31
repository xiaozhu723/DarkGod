/****************************************************
    文件：StrongItem.cs
	作者：朱江
    邮箱:  839149608@qq.com
    日期：2024/3/22 12:6:57
	功能：强化Item
*****************************************************/

using System;
using UnityEngine;
using UnityEngine.UI;

public class StrongItem : UIItem
{
    public Text infoName;
    public Image icon;
    public Toggle toggle;
    StrongItemData CurrData;

    private void Start()
    {
        toggle.onValueChanged.AddListener(OnClickToggle);
    }

    

    public override void Show()
    {
        base.Show();
    }
    public override void SetData(UIItemData data)
    {
        base.SetData(data);
        CurrData = data as StrongItemData;
        infoName.text = CurrData.name;
        icon.sprite = ResourceService.Instance.LoadSprite(CurrData.iconPath,true);
    }
    private void OnClickToggle(bool isOn)
    {
        if (!isOn) return;
        if (CurrData == null) return;
        if(CurrData.fun!=null)
        {
            CurrData.fun(CurrData.partID);
        }
    }

    public void SetToggleState(bool isOn)
    {
        if (!isOn) return;
        toggle.isOn = true;
        OnClickToggle(true);
    }
}

public class StrongItemData : UIItemData
{
    public string iconPath;
    public string name;
    public int partID;
    public Action<int> fun; 
}