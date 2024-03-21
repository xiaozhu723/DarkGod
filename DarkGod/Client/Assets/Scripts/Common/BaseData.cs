
/****************************************************
	文件：BaseData.cs
	作者：朱江
	邮箱:  839149608@qq.com
	日期：2024/03/18 17:08   	
	功能：数据基类
*****************************************************/
using UnityEngine;

public class BaseData<T>
{
	public int ID;
}

public class MapCfg : BaseData<MapCfg>
{
    public string mapName;
    public string sceneName;
	public int power;
    public Vector3 mainCamPos;
	public Vector3 mainCamRote;
	public Vector3 playerBomPos;
    public Vector3 playerBomRote;
	public string monsterLst;
	public int exp;
    public int coin;
    public int crystal;
}

//自动任务配置
public class AutoGuideData : BaseData<AutoGuideData>
{
    public int npcID;
    public string dilogArr;
    public int actID;
    public int coin;
    public int exp;
}
