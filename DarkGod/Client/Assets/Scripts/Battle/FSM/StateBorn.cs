/****************************************************
	文件：StateBorn.cs
	作者：朱江
	邮箱:  839149608@qq.com
	日期：2024/03/29 17:21   	
	功能：诞生状态
*****************************************************/
public class StateBorn : IState
{
    public void Enter(EntityBase entity, params object[] objs)
    {
        entity.currEntityState = EntityState.Born;
    }

    public void Exit(EntityBase entity, params object[] objs)
    {
       
    }

    public void Process(EntityBase entity, params object[] objs)
    {

    }
}