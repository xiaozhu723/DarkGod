/****************************************************
    文件：PlayerController.cs
	作者：朱江
    邮箱:  839149608@qq.com
    日期：2024/3/18 15:25:13
	功能：显示实体角色控制器
*****************************************************/


using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : Controller
{
    private Camera mCamera;
    Vector3  carOffset;
    
    //角色控制器
    public CharacterController controller;
    //导航寻路组件
    public NavMeshAgent navMeshAgent;
    public Transform fxParent;

    bool isNav = false;
    Vector3 endNavPos;
    Action NavCallBack;

    float CurrentBlend = 0;
    float targetBlend = 0;

  
    Dictionary<string, GameObject> fxDic = new Dictionary<string, GameObject>();
    public override void Init()
    {
        base.Init();
        mCamera = Camera.main;
        carOffset = transform.position - mCamera.transform.position;
        if(fxParent)
        {
            for (int i = 0; i < fxParent.childCount; i++)
            {
                var temp = fxParent.GetChild(i);
                fxDic.Add(temp.name, temp.gameObject);
            }
        }
    }

    void OnAnimatorMove()
    {
        //勾选apply root motion 会导致碰撞体失效 先加上OnAnimatorMove
    }

    private void Update()
    {
        //float h = Input.GetAxis("Horizontal");
        //float v = Input.GetAxis("Vertical");
        //Vector2 _dir = new Vector2(h, v).normalized;
        //if (_dir != Vector2.zero)
        //{
        //    SetBlend(Constants.BlendWalk);
        //    Dir = _dir;

        //}
        //else
        //{
        //    SetBlend(Constants.BlendIdle);
        //    Dir = Vector2.zero;
        //}

        if (CurrentBlend != targetBlend)
        {
            UpdateMixBlend();
        }

        if (isMove)
        {
            //设置方向
            SetDir();
            //移动角色
            SetMove();
            //摄像机跟随
            SetCar();
        }

        if(isSkillMove)
        {
            SetSkillMove();
            //摄像机跟随
            SetCar();
        }

        //开始导航
        if (isNav)
        {
            float dis = Vector3.Distance(transform.position, endNavPos);
            SetCar();
            if (dis<1.0f)
            {
                StopAutoNavigation();
                if(NavCallBack!=null)
                {
                    NavCallBack();
                }
            }
        }
    }

    void SetDir()
    {
        float angle = Vector2.SignedAngle(Dir, new Vector2(0, 1)) + mCamera.transform.eulerAngles.y;
        Vector3 eulerAngles = new Vector3(0, angle, 0);
        transform.localEulerAngles = eulerAngles;
    }

    void SetMove()
    {
        controller.Move(transform.forward * Time.deltaTime * Constants.PlayerSpeed);
    }

    void SetSkillMove()
    {
        controller.Move(transform.forward * Time.deltaTime * skillMoveSpeed);
    }

    void SetCar()
    {
        mCamera.transform.position = transform.position - carOffset;
    }

    public override void SetBlend(float blend)
    {
        targetBlend = blend;
    }

    void UpdateMixBlend()
    {
        if(Mathf.Abs(CurrentBlend - targetBlend) < Time.deltaTime * Constants.AccelerSpeed)
        {
            CurrentBlend = targetBlend;
        }
        else if(CurrentBlend > targetBlend)
        {
            CurrentBlend -= Time.deltaTime * Constants.AccelerSpeed;
        }
        else
        {
            CurrentBlend += Time.deltaTime * Constants.AccelerSpeed;
        }
        animator.SetFloat("Blend", CurrentBlend);
    }

    public void StartAutoNavigation(Vector3 endPos,Action fun)
    {
        NavCallBack = fun;
        endNavPos = endPos;
        controller.enabled = false;
        navMeshAgent.enabled = true;
        SetBlend(Constants.BlendWalk);
        isNav = true;
        navMeshAgent.speed = Constants.PlayerSpeed;
        navMeshAgent.SetDestination(endNavPos);
    }
    public void StopAutoNavigation()
    {
        if (!isNav) return;
        isNav = false;
        controller.enabled = true;
        navMeshAgent.isStopped = true;
        navMeshAgent.enabled = false;
        SetBlend(Constants.BlendIdle);
    }

    public override void SetFX(string fxName, float time)
    {
        GameObject fx = null;
        if(fxDic.TryGetValue(fxName,out fx))
        {
            fx.SetActive(true);
            timerService.AddTimerTask((int id) => {
                fx.SetActive(false);
            }, time);
        }
    }
}