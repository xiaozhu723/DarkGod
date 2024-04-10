/****************************************************
	文件：EntityBase.cs
	作者：朱江
	邮箱:  839149608@qq.com
	日期：2024/03/29 17:09   	
	功能：实体基类
*****************************************************/
using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class EntityBase
{
    public EntityState currEntityState = EntityState.None;
    public EntityType currEntityType = EntityType.None;
    public EntitySkillState currEntitySkillState = EntitySkillState.None;
    public StateManager stateManager = null;
    public SkillManager skillManager = null;
    protected Controller controller = null;
    public BattleManager battleManager = null;
    public bool canControl = true;
    public Vector2 currDir = Vector2.zero;
    private string name;
    private EntityData entityData;
    public Queue<int> comboQueue = new Queue<int>();
    public int nextSkilID = 0;
    public SkillData curtSkillCfg;
    public bool bCanSkill = true;
    public List<int> skMoveCbList = new List<int>();
    public List<int> skActionCbList = new List<int>();
    public int CurrSkillEndCBID = -1;
    public int CurrDieCBID = -1;
    public EntityData Props
    {
        get { return entityData; }
        protected set
        {
            entityData = value;
        }
    }

    private int hp;
    public int HP
    {
        get { return hp; }
        set
        {
            SetHPVal(hp, value);
            hp = value;
        }
    }

    public string Name
    {
        get { return name; }
        set
        {
            name = value;
        }
    }
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
        if (controller != null)
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

    public virtual Vector2 GetDir()
    {
        if (controller != null)
        {
            return controller.Dir;
        }
        return Vector2.zero;
    }

    public virtual void SetCurrDir()
    {
        if (controller != null)
        {
            currDir = controller.Dir;
        }
    }

    public virtual Vector2 GetDirInput()
    {
        return Vector2.zero;
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

    public virtual void SkillAttack(int skillID)
    {
        if (skillManager != null)
        {
            skillManager.SkillAttack(this, skillID);
        }
    }

    public virtual void SetSkillMoveState(bool isMove, float speed = 0)
    {
        if (controller != null)
        {
            controller.SetSkillMoveState(isMove, speed);
        }
    }

    public void SetController(Controller contr)
    {
        controller = contr;
    }

    public Controller GetController()
    {
        return controller ;
    }

    public AnimationClip[] GetAnimationClips()
    {
        if (controller != null)
        {
            return controller.animator.runtimeAnimatorController.animationClips;
        }
        return null;
    }

    public void SetActive(bool isAct)
    {
        if (controller != null)
        {
            controller.gameObject.SetActive(isAct);
        }
    }
    //设置攻击旋转
    public virtual void SetAtkRotation(Vector2 dir, bool Offset = false)
    {
        if (controller != null)
        {
            if (Offset)
                controller.SetAtkRotationCam(dir);
            else
                controller.SetAtkRotationLocal(dir);
        }
    }

    public virtual Vector3 GetPos()
    {
        if (controller != null)
        {
            return controller.transform.position;
        }
        return Vector3.zero;
    }

    public virtual Transform GetTransform()
    {
        if (controller != null)
        {
            return controller.transform;
        }
        return null;

    }

    public virtual void SetEntityData(EntityData data)
    {
        Props = data;
    }

    public virtual void SetBoreAction()
    {
        if (controller != null)
        {
            Debug.Log("================ SetBoreAction  SetActive(true)    " + name);
            controller.gameObject.SetActive(true);
            controller.SetAction(Constants.SkillActionBorn);
            TimerService.Instance.AddTimerTask((int id) =>
            {
                controller.SetAction(Constants.SkillActionDefault);
            }, 500);
        }
    }


    public virtual void SetCritical(int num)
    {
        if (controller != null)
            GameRoot.Instance.tipsWindow.SetCritical(Name, num);
    }

    public virtual void SetDodge()
    {
        if (controller != null)
            GameRoot.Instance.tipsWindow.SetDodge(Name);
    }

    public virtual void SetHurt(int num)
    {
        if (controller != null)
            GameRoot.Instance.tipsWindow.SetHurt(Name, num);
    }

    public virtual void SetHPVal(int oldVal, int newVal)
    {
        if (controller != null)
            GameRoot.Instance.tipsWindow.SetHPVal(Name, oldVal, newVal);
    }

    public virtual void ExitStateAttack()
    {
        canControl = true;
        if (curtSkillCfg != null)
        {
            if (!curtSkillCfg.isBreak)
                currEntitySkillState = EntitySkillState.None;
            if (curtSkillCfg.isCombo)
            {
                if (comboQueue.Count > 0)
                {
                    nextSkilID = comboQueue.Dequeue();
                }
                else
                {
                    nextSkilID = 0;
                }
            }
        }
        
        SetAction(Constants.SkillActionDefault);
    }

    public virtual Vector2 CalcTargetDir()
    {
        return Vector2.zero;
    }

    public AudioSource GetAudio()
    {
        return controller.GetComponent<AudioSource>();
    }

    public void RemoveMoveCb(int id)
    {
        for (int i = 0; i < skMoveCbList.Count; i++)
        {
            if (skMoveCbList[i] == id)
            {
                skMoveCbList.RemoveAt(i);
                return;
            }
        }
    }

    public void RemoveAllMoveCb()
    {
        for (int i = 0; i < skMoveCbList.Count; i++)
        {
            TimerService.Instance.DeleteTimeTask(skMoveCbList[i]);
        }
        skMoveCbList.Clear();
    }

    public void RemoveActionCb(int id)
    {
        for (int i = 0; i < skActionCbList.Count; i++)
        {
            if (skActionCbList[i] == id)
            {
                skActionCbList.RemoveAt(i);
                return;
            }
        }
    }
    public void RemoveAllActionCb()
    {
        for (int i = 0; i < skActionCbList.Count; i++)
        {
            TimerService.Instance.DeleteTimeTask(skActionCbList[i]);
        }
        skActionCbList.Clear();
    }

    public virtual bool GetBreakState()
    {
        return true;
    }

    public virtual void ClearSkillCB()
    {
        SetDir(Vector2.zero);
        SetSkillMoveState(false);

        RemoveAllMoveCb();
        RemoveAllActionCb();
        if (nextSkilID != 0 || comboQueue.Count > 0)
        {
            comboQueue.Clear();
            nextSkilID = 0;
            battleManager.lastTime = 0;
            battleManager.nComboIndex = 0;
        }
        if (CurrSkillEndCBID != -1)
        {
            TimerService.Instance.DeleteTimeTask(CurrSkillEndCBID);
            CurrSkillEndCBID = -1;
        }
    }
}
