/****************************************************
	文件：StateAttack.cs
	作者：朱江
	邮箱:  839149608@qq.com
	日期：2024/03/29 17:20   	
	功能：攻击状态
*****************************************************/
public class StateAttack : IState
{
    public void Enter(EntityBase entity, params object[] objs)
    {
        entity.currEntityState = EntityState.Attack;
        entity.curtSkillCfg = ResourceService.Instance.GetSkillData((int)objs[0]);
        //PECommon.Log("Enter  Attack");
    }

    public void Exit(EntityBase entity, params object[] objs)
    {
        entity.ExitStateAttack();
        entity.curtSkillCfg = null;
        //PECommon.Log("Exit  Attack");
    }

    public void Process(EntityBase entity, params object[] objs)
    {
        //PECommon.Log("Process  Attack  "+ (int)objs[0]);
        //技能攻击
        if(entity.currEntityType == EntityType.Player)
        {
            entity.bCanSkill = false;
        }
        entity.SkillAttack((int)objs[0]);
    }
}