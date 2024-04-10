
/****************************************************
	文件：BaseData.cs
	作者：朱江
	邮箱:  839149608@qq.com
	日期：2024/03/18 17:08   	
	功能：数据基类
*****************************************************/
using UnityEngine;

public class BaseData<T>
{
	public int ID;
}

public class MapCfg : BaseData<MapCfg>
{
    public string mapName;
    public string sceneName;
	public int power;
    public Vector3 mainCamPos;
	public Vector3 mainCamRote;
	public Vector3 playerBomPos;
    public Vector3 playerBomRote;
	public string monsterLst;
	public int exp;
    public int coin;
    public int crystal;
}

public class MonsterCfg : BaseData<MonsterCfg>
{
    public string strMonsterName;
    public int nType;
    public bool bIsStop;
    public string strResPath;
    public int skillID;
    public float atkDis;
    public int hp;
    public int ad;
    public int ap;
    public int addef;
    public int apdef;
    public int dodge;
    public int pierce;
    public int critical;
}

public class EntityData : BaseData<EntityData>
{
    public int hp;
    public int ad;
    public int ap;
    public int addef;
    public int apdef;
    public int dodge;
    public int pierce;
    public int critical;
    public int Level;
}

//自动任务配置
public class AutoGuideData : BaseData<AutoGuideData>
{
    public int npcID;
    public string dilogArr;
    public int actID;
    public int coin;
    public int exp;
}

//强化配置
public class StrongData : BaseData<StrongData>
{
    public int nPartID;//部件id
    public int nStarLevel;
    public int nAddHP;
    public int nAddHurt;
    public int nAddDef;
    public int nNeedLevel;
    public int nNeedCoin;
    public int nNeedCrystal;
    public string iconPath;
}

//任务配置
public class TaskData : BaseData<TaskData>
{
    public string strTaskName;
    public int nMaxCount;
    public int nExp;
    public int nCoin;
}

//技能配置
public class SkillData : BaseData<SkillData>
{
    public string strSkillName;
    public int nCDTime;
    public int nSkillTime;
    public int nAniAction;//动画播放标志位
    public string strFxName;//特效名字
    public bool isCombo;
    public bool isCollide;
    public bool isBreak;
    public DamageType dmgType;
    public int[] skillMoveLst;
    public int[] skillActionLst;
    public int[] skillDamageLst;
}

//技能位移
public class SkillMove : BaseData<SkillMove>
{
    public int nDelayTime;
    public int nMoveTime;
    public float moveDis;
}

//技能伤害点配置
public class SkillAction : BaseData<SkillAction>
{
    public int nDelayTime;
    public float radius;//伤害半径
    public float angle;//伤害角度
}