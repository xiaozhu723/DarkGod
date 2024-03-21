/****************************************************
    文件：GuideData.cs
	作者：朱江
    邮箱:  839149608@qq.com
    日期：2024/3/21 11:34:9
	功能：Nothing
*****************************************************/

// 自定义的数据类型，用于序列化
using System;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "GuideData", menuName = "ScriptableObject/引导数据", order = 0)]
[Serializable]

public class GuideData : SingleScriptableObject<GuideData>
{
    public GuideDataInfo[] guideDataInfos ;
}
[Serializable]
public class GuideDataInfo 
{
    public int nNpcID;
    public string strNpcName;
    public string strNpcIconPath;
    public string strNpcGuideSpritePath;
}