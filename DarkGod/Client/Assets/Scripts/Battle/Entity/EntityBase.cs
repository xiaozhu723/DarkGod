/****************************************************
	文件：EntityBase.cs
	作者：朱江
	邮箱:  839149608@qq.com
	日期：2024/03/29 17:09   	
	功能：实体基类
*****************************************************/
using System.Xml.Linq;
using UnityEngine;

public class EntityBase
{
	public EntityState currEntityState = EntityState.None;

	public StateManager stateManager = null;
    public SkillManager skillManager = null;
    public Controller controller = null;

    public virtual void Born()
    {
        stateManager.ChangeStatus(this, EntityState.Born);
    }

    public virtual void Idle()
    {
        stateManager.ChangeStatus(this, EntityState.Idle);
    }

    public virtual void Move()
	{
        stateManager.ChangeStatus(this, EntityState.Move);
    }

    public virtual void Attack(int skillID)
    {
        stateManager.ChangeStatus(this, EntityState.Attack, skillID);
    }

    public virtual void Hit()
    {
        stateManager.ChangeStatus(this, EntityState.Hit);
    }

    public virtual void Die()
    {
        stateManager.ChangeStatus(this, EntityState.Die);
    }

    public virtual void SetBlend(float blend)
    {
        if(controller!=null)
        {
            controller.SetBlend(blend);
        }
    }

    public virtual void SetDir(Vector2 dir)
    {
        if (controller != null)
        {
            controller.Dir = dir;
        }
    }

    public virtual void SetAction(int act)
    {
        if (controller != null)
        {
            controller.SetAction(act);
        }
    }

    public virtual void SetFX(string fxName, float time)
    {
        if (controller != null)
        {
            controller.SetFX(fxName, time);
        }
    }

    public virtual void AttackEffect( int skillID)
    {
        if (skillManager != null)
        {
            skillManager.AttackEffect(this,skillID);
        }
    }

    public virtual void SetSkillMoveState(bool isMove, float speed =0)
    {
        if (controller != null)
        {
            controller.SetSkillMoveState(isMove, speed);
        }
    }
}
