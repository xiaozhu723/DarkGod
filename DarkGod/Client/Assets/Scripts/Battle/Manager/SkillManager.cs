/****************************************************
	文件：SkillManager.cs
	作者：朱江
	邮箱:  839149608@qq.com
	日期：2024/03/29 11:55   	
	功能：技能管理器
*****************************************************/
using System;
using System.ComponentModel.Design;
using UnityEngine;
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

    public void AttackEffect(EntityBase entitly, int skillID)
    {
        SkillData skillData = resourceService.GetSkillData(skillID);
        entitly.SetAction(skillData.nAniAction);
        entitly.SetFX(skillData.strFxName, skillData.nSkillTime);
        int[] skillMoveLst = skillData.skillMoveLst;
        if(skillMoveLst!=null && skillMoveLst.Length>0)
        {
            for (int i = 0; i < skillMoveLst.Length; i++)
            {

            }
            SkillMove skillMove = resourceService.GetSkillMoveData(skillMoveLst[0]);
            float speed = skillMove.moveDis / (skillMove.nMoveTime / 1000f);
            entitly.SetSkillMoveState(true, speed);
            timerService.AddTimerTask((int id) =>
            {
                entitly.SetSkillMoveState(false);
            }, skillMove.nMoveTime);
        }
       

        timerService.AddTimerTask((int id) =>
        {
            entitly.Idle();
        }, skillData.nSkillTime);
    }
}
