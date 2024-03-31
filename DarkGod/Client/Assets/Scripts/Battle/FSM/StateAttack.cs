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
        PECommon.Log("Enter  Attack");
    }

    public void Exit(EntityBase entity, params object[] objs)
    {
        entity.SetAction(Constants.SkillActionDefault);
        PECommon.Log("Exit  Attack");
    }

    public void Process(EntityBase entity, params object[] objs)
    {
        //entity.SetAction(1);
        PECommon.Log("Process  Attack  "+ (int)objs[0]);
        entity.AttackEffect((int)objs[0]);
    }
}