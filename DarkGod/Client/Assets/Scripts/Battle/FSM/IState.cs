/****************************************************
	文件：IState.cs
	作者：朱江
	邮箱:  839149608@qq.com
	日期：2024/03/29 17:11   	
	功能：状态接口
*****************************************************/

public interface IState
{
	void Enter(EntityBase entity,params object[] objs);

    void Exit(EntityBase entity, params object[] objs);

    void Process(EntityBase entity, params object[] objs);
}

public enum EntityState
{
    None,
    Born,
    Idle,
    Move,
    Attack,
    Hit,
    Die,
    Max,
}