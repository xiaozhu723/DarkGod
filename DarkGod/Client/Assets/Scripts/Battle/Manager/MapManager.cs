/****************************************************
	文件：MapManager.cs
	作者：朱江
	邮箱:  839149608@qq.com
	日期：2024/03/29 11:55   	
	功能：地图管理器
*****************************************************/
using System;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    private int CurrMapLevelID;
    BattleManager battleManager;
    public TriggerData[] triggerDataArr;
    public void Init(BattleManager manager)
    {
		battleManager = manager;
        CurrMapLevelID = 1;
        battleManager.CreatMonsterList(CurrMapLevelID);
        PECommon.Log("MapManager Init Done");
    }

    public void TriggerMonsterBorn( int levelID)
    {
        if(battleManager!=null)
        {
            battleManager.CreatMonsterList(levelID);
            battleManager.SetMonsterListAction();
        }
    }

    public bool SetNextTrigger()
    {
        CurrMapLevelID++;
        for (int i = 0; i < triggerDataArr.Length; i++)
        {
            if (triggerDataArr[i].LevelID== CurrMapLevelID)
            {
                triggerDataArr[i].GetComponent<Collider>().isTrigger = true;
                return true;
            }
        }
        return false;
    }
}