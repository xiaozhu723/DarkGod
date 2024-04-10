/****************************************************
	文件：EntityMonster.cs
	作者：朱江
	邮箱:  839149608@qq.com
	日期：2024/04/01 14:55   	
	功能：怪物逻辑实体
*****************************************************/
using UnityEngine.Analytics;
using UnityEngine;

public class EntityMonster : EntityBase
{
    private float checkCountTime = 0;
    private float checkTime = 2;

    private float atkCountTime = 0;
    private float atkTime = 2;
    float aiMoveDis = 8;
    public MonsterCfg monsterCfg;
    bool ranAI = true;
    EntityPlayer player;
    public EntityMonster()
    {
        currEntityType = EntityType.Monster;
    }

    public override void SetEntityData(EntityData data)
    {
        EntityData entityData = new EntityData()
        {
            hp = data.hp * data.Level,
            ad = data.ad * data.Level,
            ap = data.ap * data.Level,
            addef = data.addef * data.Level,
            apdef = data.apdef * data.Level,
            dodge = data.dodge * data.Level,
            pierce = data.pierce * data.Level,
            critical = data.critical * data.Level,
            Level = data.Level,
        };
        Props = entityData;
        HP = entityData.hp;
    }

    public void SetMonsterCfg(MonsterCfg data)
    {
        monsterCfg = data;
        player = battleManager.GetSelfEntityPlayer();
    }

    public void StartMonsterAI()
    {
        if (!ranAI) return;


        if (currEntityState == EntityState.Die)
        {
            return;
        }

        if (currEntityState != EntityState.Idle && currEntityState != EntityState.Move)
        {
            return;
        }
        //判断时间间隔
        checkCountTime += Time.deltaTime;
        if (checkCountTime < checkTime)
        {
            return;
        }
        else
        {
            Vector2 dir = CalcTargetDir();
            //判断是否进入攻击距离
            if (!InAttackRange())
            {
                SetDir(dir);
                Move();
            }
            else
            {
                SetDir(Vector2.zero);
                atkCountTime += checkCountTime;
                if (atkCountTime > atkTime)
                {
                    SetAtkRotation(dir);
                    Attack(monsterCfg.skillID);
                    checkCountTime = 0;
                }
                else
                {
                    Idle();
                }
                checkCountTime = 0;
                checkTime = CommonUtility.RadomInt(1, 5) * 1.0f / 10;
            }
        }

    }
    public void StopMonsterAI()
    {
        ranAI = false;
        SetDir(Vector2.zero);
        Idle();
    }

    public void RunMonsterAI()
    {
        ranAI = true;
    }
    private bool InMoveRange()
    {
        EntityPlayer player = battleManager.GetSelfEntityPlayer();
        if (player == null || player.currEntityState == EntityState.Die)
        {
            return false;
        }
        return Vector3.Distance(player.GetPos(), GetPos()) < aiMoveDis;
    }
    private bool InAttackRange()
    {
        EntityPlayer player = battleManager.GetSelfEntityPlayer();
        if (player == null || player.currEntityState == EntityState.Die)
        {
            return false;
        }
        return Vector3.Distance(player.GetPos(), GetPos()) < monsterCfg.atkDis;
    }

    public override Vector2 CalcTargetDir()
    {
        Vector2 dir = Vector2.zero;
        EntityPlayer player = battleManager.GetSelfEntityPlayer();
        if (player == null || player.currEntityState == EntityState.Die) return dir;
        Vector3 target = player.GetPos();
        Vector3 self = GetPos();
        dir = new Vector2(target.x - self.x, target.z - self.z);
        return dir.normalized;
    }
    public override bool GetBreakState()
    {
        if (monsterCfg.bIsStop)
        {
            if(curtSkillCfg!=null)
            {
                return curtSkillCfg.isBreak;
            }
            else
            {
                return true;
            }
        }
        else
        {
            return false;
        }
    }


}
