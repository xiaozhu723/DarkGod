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
        PECommon.Log("Enter  StateIdle");
    }

    public void Exit(EntityBase entity, params object[] objs)
    {
        
        PECommon.Log("Exit  StateIdle");
    }

    public void Process(EntityBase entity, params object[] objs)
    {
        PECommon.Log("Process  StateIdle");
        entity.SetBlend(Constants.BlendIdle);
    }
}