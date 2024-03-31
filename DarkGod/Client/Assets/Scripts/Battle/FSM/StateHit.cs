/****************************************************
	文件：StateHit.cs
	作者：朱江
	邮箱:  839149608@qq.com
	日期：2024/03/29 17:19   	
	功能：受击状态
*****************************************************/
public class StateHit : IState
{
    public void Enter(EntityBase entity, params object[] objs)
    {
        entity.currEntityState = EntityState.Hit;
    }

    public void Exit(EntityBase entity, params object[] objs)
    {
        
    }

    public void Process(EntityBase entity, params object[] objs)
    {

    }
}