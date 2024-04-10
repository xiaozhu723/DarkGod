/****************************************************
	文件：EntityPlayer.cs
	作者：朱江
	邮箱:  839149608@qq.com
	日期：2024/03/29 17:50   	
	功能：玩家实体
*****************************************************/
using System.Collections.Generic;
using UnityEngine;

public class EntityPlayer:EntityBase
{
    public EntityPlayer()
    {
        currEntityType = EntityType.Player;
    }
    public override Vector2 GetDirInput()
    {
        return battleManager.GetDirInput();
    }

    public override Vector2 CalcTargetDir()
    {
        Vector2 dir = Vector2.zero;
        EntityMonster monster = FindClosedTarget();
        if(monster == null) return dir;
        Vector3 target = monster.GetPos();
        Vector3 self = GetPos();
        dir = new Vector2(target.x - self.x, target.z - self.z);
        return dir.normalized;
    }

    private EntityMonster FindClosedTarget()
    {
        //获取当前所有怪物 进行判定
        List<EntityMonster> monsterList = battleManager.GetMonsterList();
        EntityMonster monster = null;
        float dis = 0;
        for (int i = 0; i < monsterList.Count; i++)
        {
            if(i==0)
            {
                monster = monsterList[i];
                dis = Vector3.Distance(GetPos(), monsterList[i].GetPos());
            }
            else
            {
                float temp = Vector3.Distance(GetPos(), monsterList[i].GetPos());
                if(temp< dis)
                {
                    dis = temp;
                   monster = monsterList[i];
                }
            }
        }
        return monster;
    }

    public override void SetHPVal(int oldVal, int newVal)
    {
        BattleSystem.Instance.playerCtrlWindow.UpdateHP(newVal);
    }

    public override void SetDodge()
    {
        GameRoot.Instance.tipsWindow.SetPlayerDodge();
    }
}
