/****************************************************
	文件：StateManager.cs
	作者：朱江
	邮箱:  839149608@qq.com
	日期：2024/03/29 11:53   	
	功能：状态管理器
*****************************************************/
using UnityEngine;
using System.Collections.Generic;

public class StateManager : MonoBehaviour
{
    private Dictionary<EntityState, IState> FSMDic = new Dictionary<EntityState, IState>();
    public void Init()
    {
        FSMDic.Add(EntityState.Born, new StateBorn());
        FSMDic.Add(EntityState.Idle, new StateIdle());
        FSMDic.Add(EntityState.Move, new StateMove());
        FSMDic.Add(EntityState.Attack, new StateAttack());
        FSMDic.Add(EntityState.Hit, new StateHit());
        FSMDic.Add(EntityState.Die, new StateDie());
        PECommon.Log("StateManager init Done");
    }

    //切换状态
    public void ChangeStatus(EntityBase entity, EntityState targetState, params object[] objs)
    {
        if (entity.currEntityState == EntityState.Die) return;
        if (targetState == entity.currEntityState) return;
        if (FSMDic.TryGetValue(entity.currEntityState, out IState state))
        {
            if (state != null)
                state.Exit(entity, objs);
        }
        if (FSMDic.TryGetValue(targetState, out IState vaule))
        {
            vaule.Enter(entity, objs);
            vaule.Process(entity, objs);
        }
    }
}

