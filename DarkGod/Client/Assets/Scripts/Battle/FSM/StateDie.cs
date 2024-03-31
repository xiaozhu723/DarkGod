/****************************************************
	文件：StateDie.cs
	作者：朱江
	邮箱:  839149608@qq.com
	日期：2024/03/29 17:19   	
	功能：死亡状态
*****************************************************/
public class StateDie : IState
{
    public void Enter(EntityBase entity, params object[] objs)
    {
        entity.currEntityState = EntityState.Die;
    }

    public void Exit(EntityBase entity, params object[] objs)
    {
        
    }

    public void Process(EntityBase entity, params object[] objs)
    {

    }
}