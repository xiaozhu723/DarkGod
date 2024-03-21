/****************************************************
    文件：DetailItem.cs
	作者：朱江
    邮箱:  839149608@qq.com
    日期：2024/3/20 17:0:7
	功能：Nothing
*****************************************************/

using UnityEngine;
using UnityEngine.UI;

public class DetailItem : UIItem
{
    public Text infoName;
    public Text infoNum;
    public override void Show()
    {
        base.Show();
    }
    public override void SetData(UIItemData data)
    {
     
        DetailItemData temp = data as DetailItemData;
        infoName.text = temp.Name;
        infoNum.text = temp.Num;
    }
}