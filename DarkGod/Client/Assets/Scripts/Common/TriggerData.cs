/****************************************************
    文件：TriggerData.cs
	作者：朱江
    邮箱:  839149608@qq.com
    日期：2024/4/5 13:44:5
	功能：Nothing
*****************************************************/

using UnityEngine;

public class TriggerData : MonoBehaviour 
{
    public int LevelID;
    public MapManager mapManager;

    public void OnTriggerExit(Collider other)
    {
        if(mapManager!=null)
        {
            mapManager.TriggerMonsterBorn(LevelID);
        }
    }
}