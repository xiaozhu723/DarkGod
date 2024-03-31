/****************************************************
    文件：Constants.cs
	作者：朱江
    邮箱:  839149608@qq.com
    日期：2024/3/10 16:12:23
	功能：常量配置
*****************************************************/

using UnityEngine;

public class Constants
{
    //场景名称
    public const string LoginSceneName = "SceneLogin";
    //public const string MainCitySceneName = "SceneMainCity";
    public const int MainCitySceneID = 10000;

    //背景音效
    public const string BGLogin = "bgLogin";
    public const string BGMainCity = "bgMainCity";
    public const string BGHuangYe = "bgHuangYe";

    //进入游戏按钮音效
    public const string BGLoginBtn = "uiLoginBtn";

    //按钮音效
    public const string UIClickBtn = "uiClickBtn";
    public const string UIExtenBtn = "uiExtenBtn";
    public const string UIOpenPage = "uiOpenPage";
    public const string Fbitem = "fbitem";

    //原始屏幕
    public const int ScreenStandrdHeight = 750;
    public const int ScreenStandrdWidth = 1334;
    //摇杆操作点标准距离
    public const int ScreenOPDis = 90;

    //角色速度
    public const int PlayerSpeed = 8;
    //怪物速度
    public const int MonsterSpeed = 4;

    //动画平滑加速度
    public const float AccelerSpeed = 5;

    //SkilAnction触发参数
    public const int SkillActionDefault = -1;

    //动画混合参数
    public const int BlendIdle = 0;
    public const int BlendWalk = 1;

    //聊天最大储存值
    public const int ChatMaxCount = 100;
}