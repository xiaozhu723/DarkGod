/****************************************************
	文件：StateDie.cs
	作者：朱江
	邮箱:  839149608@qq.com
	日期：2024/03/29 17:19   	
	功能：死亡状态
*****************************************************/
using UnityEngine;

public class StateDie : IState
{
    public void Enter(EntityBase entity, params object[] objs)
    {
        entity.currEntityState = EntityState.Die;
        entity.ClearSkillCB();
        if (entity.currEntityType == EntityType.Player)
        {
            entity.battleManager.StopAI();
        }
    }

    public void Exit(EntityBase entity, params object[] objs)
    {
        
    }

    public void Process(EntityBase entity, params object[] objs)
    {
        entity.SetAction(Constants.SkillActionDie);
        if (entity.currEntityType == EntityType.Player) return;
        entity.CurrDieCBID = TimerService.Instance.AddTimerTask((int id) =>
        {
            entity.CurrDieCBID = -1;
            entity.SetAction(Constants.SkillActionDefault);
            entity.SetActive(false);
        }, Constants.DieAinLength);
    }
}