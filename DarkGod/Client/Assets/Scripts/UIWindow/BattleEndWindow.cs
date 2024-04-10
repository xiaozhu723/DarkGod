/****************************************************
    文件：BattleEndWindow.cs
	作者：朱江
    邮箱:  839149608@qq.com
    日期：2024/4/6 15:10:39
	功能：Nothing
*****************************************************/

using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class BattleEndWindow : WindowRoot
{
    public GameObject CenterPin;
    public Image imgLv;
    public Button btnSure;
    public Button btnExit;
    public Button btnClose;
    public Text txtTime;
    public Text txtRestHP;
    public Text txtReward;
    protected override void InitWindow()
    {
        base.InitWindow();
        battleSystem.StopAI();
    }

    private void Start()
    {
        btnSure.onClick.AddListener(OnClickSureBtn);
        btnExit.onClick.AddListener(OnClickExitBtn);
        btnClose.onClick.AddListener(OnClickCloseBtn);
    }

    public void SetBattleEndType(BattleEndType battleEndType)
    {
        switch (battleEndType)
        {
            case BattleEndType.None:
                SetActive(btnExit.gameObject, true);
                SetActive(btnClose.gameObject, false);
                SetActive(CenterPin, false);
                break;
            case BattleEndType.Pause:
                SetActive(btnExit.gameObject, true);
                SetActive(btnClose.gameObject, true);
                SetActive(CenterPin, false);
                break;
            case BattleEndType.Exit:
                SetActive(btnExit.gameObject, false);
                SetActive(btnClose.gameObject, false);
                SetActive(CenterPin, true);
                break;
        }
    }
    public void SetBattleEndExitData(int ID,int hp, int costTime)
    {
        MapCfg SceneCfg = resService.GetMapCfg(ID);
        txtTime.text = string.Format("通关时间：{0:d2}：{1:d2}", costTime / 60, costTime % 60);
        txtRestHP.text = "剩余血量：" + hp;
        txtReward.text = string.Format("关卡奖励：{0}金币 {1}经验 {2}水晶", SceneCfg.coin, SceneCfg.exp, SceneCfg.crystal);
    }
    private void OnClickCloseBtn()
    {
        audioService.PlayUIMusic(Constants.UIClickBtn);
        battleSystem.RunAI();
        SetWinState(false);
    }

    private void OnClickExitBtn()
    {
        //回到主场景 销毁战斗
        MainCitySystem.Instance.EnterMainCity();
        battleSystem.DestroyBattle();
        SetWinState(false);
    }

    private void OnClickSureBtn()
    {
        MainCitySystem.Instance.EnterMainCity();
        battleSystem.DestroyBattle();
        SetWinState(false);
    }
}

public enum BattleEndType
{
    None,
    Pause,
    Exit,
}