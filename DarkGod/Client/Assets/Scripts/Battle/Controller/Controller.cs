/****************************************************
	文件：Controller.cs
	作者：朱江
	邮箱:  839149608@qq.com
	日期：2024/03/29 18:05   	
	功能：显示实体抽象基类
*****************************************************/
using System;
using UnityEngine;
using UnityEngine.Rendering;

public abstract class Controller: MonoBehaviour
{
    protected TimerService timerService;
    //角色动画控制器
    public Animator animator;
    //行进方向
    private Vector2 dir = Vector2.zero;
    protected bool isMove = false;

    public bool isSkillMove = false;
    public float skillMoveSpeed = 0;
    public Vector2 Dir
    {
        get { return dir; }
        set
        {
            if (value == Vector2.zero)
            {
                isMove = false;
            }
            else
            {
                isMove = true;
            }
            dir = value;
        }
    }

    public virtual void Init()
    {
        timerService = TimerService.Instance;
    }

    public virtual void SetBlend(float blend)
    {
        animator.SetFloat("Blend", blend);
    }

    public virtual void SetAction(int act)
    {
        animator.SetInteger("SkillAction", act);
    }

    public virtual void SetFX(string fxName, float time)
    {
        
    }

    public virtual void SetSkillMoveState(bool isMove, float speed)
    {
        isSkillMove = isMove;
        skillMoveSpeed = speed;
    }

}
