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
    public Transform hpRoot;
    //行进方向
    private Vector2 dir = Vector2.zero;
    protected bool isMove = false;
    //角色控制器
    public CharacterController controller;
    protected Camera mCamera;

    protected bool isSkillMove = false;
    protected float skillMoveSpeed = 0;
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

    public virtual void SetAtkRotationCam(Vector2 dir)
    {
        //用 要的移动方向  和 当前正前朝向 取得旋转角度（因为有摄像机的照射角度  所有要加上摄像机的旋转）最后得到旋转角度
        float angle = Vector2.SignedAngle(dir, new Vector2(0, 1)) + mCamera.transform.eulerAngles.y;
        Vector3 eulerAngles = new Vector3(0, angle, 0);
        transform.localEulerAngles = eulerAngles;
    }

    public void SetAtkRotationLocal(Vector2 dir)
    {
        float angle = Vector2.SignedAngle(dir, new Vector2(0, 1));
        Vector3 eulerAngles = new Vector3(0, angle, 0);
        transform.localEulerAngles = eulerAngles;
    }
}
