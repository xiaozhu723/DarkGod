/****************************************************
	文件：StateHit.cs
	作者：朱江
	邮箱:  839149608@qq.com
	日期：2024/03/29 17:19   	
	功能：受击状态
*****************************************************/
using System.Security.Cryptography;
using UnityEngine;

public class StateHit : IState
{
    int tID = -1;
    public void Enter(EntityBase entity, params object[] objs)
    {
        entity.currEntityState = EntityState.Hit;
        entity.ClearSkillCB();
    }

    public void Exit(EntityBase entity, params object[] objs)
    {

    }

    public void Process(EntityBase entity, params object[] objs)
    {
        if (entity.currEntityType == EntityType.Player)
        {
            entity.bCanSkill = false;
            //受击音效
            AudioSource charAudio = entity.GetAudio();
            AudioService.Instance.PlayCharAudio(Constants.AssassinHit, charAudio);
        }
        entity.SetDir(Vector2.zero);
        entity.SetAction(Constants.SkillActionHit);
        TimerService.Instance.AddTimerTask((int id) =>
        {
            entity.Idle();
            entity.SetAction(Constants.SkillActionDefault);

        }, (int)(GetAinHitLength(entity) * 1000));
    }

    private float GetAinHitLength(EntityBase entity)
    {
        AnimationClip[] animations = entity.GetAnimationClips();
        for (int i = 0; i < animations.Length; i++)
        {
            string ainName = animations[i].name;
            if (ainName.Contains("hit") ||
                ainName.Contains("Hit") ||
                ainName.Contains("HIT"))
            {
                return animations[i].length;
            }
        }
        return 1;
    }
}