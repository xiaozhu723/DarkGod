/****************************************************
	文件：SkillManager.cs
	作者：朱江
	邮箱:  839149608@qq.com
	日期：2024/03/29 11:55   	
	功能：技能管理器
*****************************************************/

using System.Collections.Generic;
using System.Drawing;
using System.Security.Cryptography;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;
public class SkillManager : MonoBehaviour
{
    TimerService timerService;
    ResourceService resourceService;
    public void Init()
    {
        resourceService = ResourceService.Instance;
        timerService = TimerService.Instance;
        PECommon.Log("SkillManager Init Done");
    }
    public void SkillAttack(EntityBase entitly, int skillID)
    {
        entitly.RemoveAllMoveCb();
        entitly.RemoveAllActionCb();
        AttackEffect(entitly, skillID);
        AttackDamage(entitly, skillID);
    }
    //技能效果
    public void AttackEffect(EntityBase entitly, int skillID)
    {
        SkillData skillData = resourceService.GetSkillData(skillID);

        if (!skillData.isCollide)
        {
            Physics.IgnoreLayerCollision(9, 10);
            timerService.AddTimerTask((int tid) =>
            {
                Physics.IgnoreLayerCollision(9, 10, false);
            }, skillData.nSkillTime);
        }

        //攻击方向锁定
        if (entitly.currEntityType == EntityType.Player)
        {
            Vector2 currDir = entitly.GetDirInput();
            if (currDir == Vector2.zero)
            {
                Vector2 dir = entitly.CalcTargetDir();
                if (dir != Vector2.zero)
                {
                    entitly.SetAtkRotation(dir);
                }
            }
            else
            {
                entitly.SetAtkRotation(currDir, true);
            }
        }


        entitly.SetAction(skillData.nAniAction);
        entitly.SetFX(skillData.strFxName, skillData.nSkillTime);

        SkillMove(entitly, skillData);

        entitly.canControl = false;
        entitly.SetCurrDir();
        entitly.SetDir(Vector2.zero);
        if (!skillData.isBreak)
            entitly.currEntitySkillState = EntitySkillState.BatiState;

        entitly.CurrSkillEndCBID =timerService.AddTimerTask((int id) =>
        {
            entitly.Idle();
        }, skillData.nSkillTime);
    }
    //技能伤害
    public void AttackDamage(EntityBase entityBase, int skillID)
    {
        SkillData skillData = resourceService.GetSkillData(skillID);
        int[] skillActionLst = skillData.skillActionLst;
        if (skillActionLst != null && skillActionLst.Length > 0)
        {
            int sum = 0;
            for (int i = 0; i < skillActionLst.Length; i++)
            {
                SkillAction skillAction = resourceService.GetSkillActionData(skillActionLst[i]);
                sum += skillAction.nDelayTime;
                if (sum > 0)
                {
                    int tid = timerService.AddTimerTask((int id) =>
                    {
                        if (entityBase == null) return;
                        entityBase.RemoveActionCb(id);
                        SkillAction(entityBase, skillAction, skillData, i);
                    }, sum);
                    entityBase.skActionCbList.Add(tid);
                }
                else
                {
                    SkillAction(entityBase, skillAction, skillData, i);
                }
            }
        }
    }

    //技能伤害计算
    private void SkillAction(EntityBase entityBase, SkillAction skillAction, SkillData skillData, int index)
    {
        int damage = 0;
        if (index <= skillData.skillDamageLst.Length - 1)
        {
            damage = skillData.skillDamageLst[index];
        }
        if (entityBase.currEntityType == EntityType.Player)
        {
            //获取当前所有怪物 进行判定
            List<EntityMonster> monsterList = entityBase.battleManager.GetMonsterList();
            for (int i = 0; i < monsterList.Count; i++)
            {
                EntityMonster monster = monsterList[i];
                if (InRange(entityBase.GetPos(), monster.GetPos(), skillAction.radius)
                    && InAngle(entityBase.GetTransform(), monster.GetPos(), skillAction.angle))
                {
                    //伤害计算
                    CalculateDamage(entityBase, monster, skillData.dmgType, damage);
                }
            }
        }
        else if (entityBase.currEntityType == EntityType.Monster)
        {
            EntityPlayer target = BattleSystem.Instance.BattleManager.GetSelfEntityPlayer();
            if (InRange(entityBase.GetPos(), target.GetPos(), skillAction.radius)
                && InAngle(entityBase.GetTransform(), target.GetPos(), skillAction.angle))
            {
                //伤害计算
                CalculateDamage(entityBase, target, skillData.dmgType, damage);
            }
        }

    }

    //距离判断
    private bool InRange(Vector3 form, Vector3 to, float range)
    {
        if (Vector3.Distance(form, to) <= range)
        {
            return true;
        }
        return false;
    }

    //角度判断
    private bool InAngle(Transform trans, Vector3 to, float angle)
    {

        if (angle == 360)
        {
            return true;
        }
        else
        {
            Vector3 start = trans.position;
            Vector3 end = (to - start).normalized;
            float currAngle = Vector3.Angle(trans.forward, end);
            if (currAngle <= angle / 2)
            {
                return true;
            }
            return false;
        }
    }
    System.Random rd = new System.Random();
    //伤害计算
    private void CalculateDamage(EntityBase caster, EntityBase target, DamageType damType, int damage)
    {
        int dmgSum = damage;
        if (damType == DamageType.ADDamage)
        {
            //计算闪避
            int dodgeNum = CommonUtility.RadomInt(1, 100, rd);
            if (dodgeNum <= target.Props.dodge)
            {
                //UI显示闪避 TODO
                target.SetDodge();
                return;
            }
            //计算属性加成
            dmgSum += caster.Props.ad;

            //计算暴击
            int criticalNum = CommonUtility.RadomInt(1, 100, rd);
            if (criticalNum <= caster.Props.critical)
            {
                float criticalRate = 1 + (CommonUtility.RadomInt(1, 100, rd) / 100.0f);
                dmgSum = (int)criticalRate * dmgSum;
                target.SetCritical(dmgSum);
            }

            //计算穿甲
            int addef = (int)((1 - caster.Props.pierce / 100.0f) * target.Props.addef);
            dmgSum -= addef;
        }
        else if (damType == DamageType.APDamage)
        {
            //计算属性加成
            dmgSum += caster.Props.ap;
            //计算魔法抗性
            dmgSum -= target.Props.apdef;
        }

        //最终伤害
        if (dmgSum < 0)
        {
            dmgSum = 0;
            return;
        }
        target.SetHurt(dmgSum);
        if (target.HP < dmgSum)
        {
            target.HP = 0;
            //目标死亡
            target.Die();
            if (target.currEntityType == EntityType.Monster)
                target.battleManager.RemoveMonster(target.Name);
            else
            {
                target.battleManager.SetSelfEntityPlayer();
                target.battleManager.EndBattle(false, 0);
            }
        }
        else
        {
            target.HP -= dmgSum;
            if (target.currEntitySkillState == EntitySkillState.None && target.GetBreakState())
                target.Hit();
        }
    }

    //技能位移
    private void SkillMove(EntityBase entitly, SkillData skillData)
    {
        int[] skillMoveLst = skillData.skillMoveLst;
        if (skillMoveLst != null && skillMoveLst.Length > 0)
        {
            int sum = 0;
            for (int i = 0; i < skillMoveLst.Length; i++)
            {
                SkillMove skillMove = resourceService.GetSkillMoveData(skillMoveLst[i]);
                float speed = skillMove.moveDis / (skillMove.nMoveTime / 1000f);
                sum += skillMove.nDelayTime;
                if (sum > 0)
                {
                    int tid1 = timerService.AddTimerTask((int id) =>
                    {
                        if (entitly == null) return;
                        entitly.RemoveMoveCb(id);
                        entitly.SetSkillMoveState(true, speed);
                    }, sum);
                    entitly.skMoveCbList.Add(tid1);
                }
                else
                {
                    entitly.SetSkillMoveState(true, speed);
                }
                sum += skillMove.nMoveTime;
                int tid2 = timerService.AddTimerTask((int id) =>
                {
                    if (entitly == null) return;
                    entitly.RemoveMoveCb(id);
                    entitly.SetSkillMoveState(false);
                }, sum);
                entitly.skMoveCbList.Add(tid2);
            }
        }

    }


}
