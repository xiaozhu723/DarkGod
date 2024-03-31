/****************************************************
	文件：BattleManager.cs
	作者：朱江
	邮箱:  839149608@qq.com
	日期：2024/03/28 18:29   	
	功能：战斗管理类
*****************************************************/
using System;
using UnityEngine;

public class BattleManager: MonoBehaviour
{
    private CharacterCreatController characterCreatController;
    private PlayerController playerController;
    private ResourceService resService;
    private StateManager stateManager;
    private SkillManager skillManager;
    private MapManager mapManager;
    private AudioService audioService;
    private BattleSystem battleSystem;
    private MapCfg SceneCfg;
    private EntityPlayer selfEntityPlayer;
    private int CurrMapID;
	public void Init(int id)
	{
        characterCreatController = CharacterCreatController.Instance;
        audioService = AudioService.Instance;
        resService = ResourceService.Instance;
        battleSystem = BattleSystem.Instance;
        CurrMapID = id;
        stateManager = gameObject.AddComponent<StateManager>();
        stateManager.Init();
        skillManager = gameObject.AddComponent<SkillManager>();
        skillManager.Init();


        SceneCfg = resService.GetMapCfg(CurrMapID);
        resService.AsyncLoadScene(SceneCfg.sceneName, () =>
        {
            mapManager = GameObject.FindGameObjectWithTag("MapRoot").GetComponent<MapManager>();
            mapManager.Init();

            //诞生主角
            CreatPlayer();

            //更换背景音乐
            audioService.PlayBGMusic(Constants.BGHuangYe, true);
            //打开副本战斗界面
            OpenFightWindow();
        });
    }

    private void CreatPlayer()
    {
        selfEntityPlayer = new EntityPlayer() { stateManager = stateManager };
        characterCreatController.CreatPlayer(SceneCfg, PathDefine.AssissnBattlePlayerPrefab);
        playerController = characterCreatController.GetPlayer();
        selfEntityPlayer.Idle();
        selfEntityPlayer.controller = playerController;
        selfEntityPlayer.skillManager = skillManager;
    }

    private void OpenFightWindow()
    {
        battleSystem.playerCtrlWindow.SetWinState();
    }


    public void SetPlayerDir(Vector2 dir)
    {
        if (dir != Vector2.zero)
        {
            selfEntityPlayer.Move();
            selfEntityPlayer.SetDir(dir);
        }
        else
        {
            selfEntityPlayer.Idle();
        }
    }

    public void SkillAttack(int ID)
    {
        selfEntityPlayer.Attack(ID);
    }
}
