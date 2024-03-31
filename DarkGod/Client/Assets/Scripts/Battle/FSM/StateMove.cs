/****************************************************
	文件：StateMove.cs
	作者：朱江
	邮箱:  839149608@qq.com
	日期：2024/03/29 17:13   	
	功能：移动状态
*****************************************************/
public class StateMove : IState
{
    public void Enter(EntityBase entity, params object[] objs)
    {
        entity.currEntityState = EntityState.Move;
        PECommon.Log("Enter  StateMove");
    }

    public void Exit(EntityBase entity, params object[] objs)
    {
        
        PECommon.Log("Exit  StateMove");
    }

    public void Process(EntityBase entity, params object[] objs)
    {
        PECommon.Log("Process  StateMove");
        entity.SetBlend(Constants.BlendWalk);
    }
}