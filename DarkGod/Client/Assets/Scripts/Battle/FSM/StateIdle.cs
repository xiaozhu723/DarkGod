/****************************************************
	文件：StateIdle.cs
	作者：朱江
	邮箱:  839149608@qq.com
	日期：2024/03/29 17:14   	
	功能：静止待机状态
*****************************************************/
using UnityEngine;
public class StateIdle : IState
{
    public void Enter(EntityBase entity, params object[] objs)
    {
        entity.currEntityState = EntityState.Idle;
        entity.SetDir(Vector2.zero);
        entity.CurrSkillEndCBID = -1;
    }

    public void Exit(EntityBase entity, params object[] objs)
    {
        //PECommon.Log("Exit  StateIdle");
    }

    public void Process(EntityBase entity, params object[] objs)
    {
        if (entity.nextSkilID != 0)
        {
            
            entity.Attack(entity.nextSkilID);
        }
        else
        {
            if (entity.currEntityType == EntityType.Player)
            {
                entity.bCanSkill = true;
            }
            if (entity.currDir != Vector2.zero)
            {
                entity.Move();
                entity.SetDir(entity.currDir);
                entity.currDir = Vector2.zero;
            }
            else
            {
                entity.SetBlend(Constants.BlendIdle);
            }
        }
        //PECommon.Log("Process  StateIdle");
    }
}