/****************************************************
	文件：BattleManager.cs
	作者：朱江
	邮箱:  839149608@qq.com
	日期：2024/03/28 18:29   	
	功能：战斗管理类
*****************************************************/
using System.Collections.Generic;
using UnityEngine;
using PEProtocol;

public class BattleManager : MonoBehaviour
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
    public double lastTime = 0;
    public int nComboIndex = 0;
    private int[] ComboArr = new int[]
    {
        111,112,113,114,115
    };
    private Dictionary<string, EntityMonster> entityMonsterDic = new Dictionary<string, EntityMonster>();
    private Dictionary<int, List<GameObject>> MonsterPoolDic = new Dictionary<int, List<GameObject>>();
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
            mapManager.Init(this);

            //诞生主角
            CreatPlayer();
            SetMonsterListAction();
            //更换背景音乐
            audioService.PlayBGMusic(Constants.BGHuangYe, true);
            //打开副本战斗界面
            OpenFightWindow();
            battleSystem.StartTime = TimerService.Instance.GetNowTime();
        });
    }

    private void Update()
    {
        if (selfEntityPlayer != null && selfEntityPlayer.currEntityState != EntityState.Die)
        {
            foreach (var item in entityMonsterDic)
            {
                if (item.Value != null)
                {
                    item.Value.StartMonsterAI();
                }
            }
        }

    }

    public void StopAI()
    {
        foreach (var item in entityMonsterDic)
        {
            if (item.Value != null)
            {
                item.Value.StopMonsterAI();
            }
        }
    }

    public void RunAI()
    {
        foreach (var item in entityMonsterDic)
        {
            if (item.Value != null)
            {
                item.Value.RunMonsterAI();
            }
        }
    }

    private void CreatPlayer()
    {
        selfEntityPlayer = new EntityPlayer() { stateManager = stateManager };
        characterCreatController.CreatPlayer(SceneCfg, PathDefine.AssissnBattlePlayerPrefab);
        playerController = characterCreatController.GetPlayer();
        selfEntityPlayer.Idle();
        selfEntityPlayer.SetController(playerController);
        selfEntityPlayer.skillManager = skillManager;
        selfEntityPlayer.battleManager = this;
        PlayerData playerData = GameRoot.Instance.PlayerData;
        EntityData entityData = new EntityData()
        {
            hp = playerData.hp,
            ad = playerData.ad,
            ap = playerData.ap,
            addef = playerData.addef,
            apdef = playerData.apdef,
            dodge = playerData.dodge,
            pierce = playerData.pierce,
            critical = playerData.critical,
        };
        selfEntityPlayer.Name = "Player";
        selfEntityPlayer.SetEntityData(entityData);
        selfEntityPlayer.HP = playerData.hp;
    }

    public void CreatMonsterList(int LevelID)
    {
        characterCreatController.InitMonsterCfg(SceneCfg);
        List<MonsterData> list = characterCreatController.GetBattleSceneMonster(LevelID);

        for (int i = 0; i < list.Count; i++)
        {
            MonsterCfg monsterCfg = resService.GetMonsterCfg(list[i].ID);
            if (monsterCfg == null)
            {
                continue;
            }
            GameObject monster = GetMonsterToPool(list[i].ID);
            if (monster == null)
                monster = resService.LoadPrefab(monsterCfg.strResPath, true);
            monster.transform.position = list[i].pos;
            monster.transform.localEulerAngles = list[i].eulerAngles;
            monster.transform.localScale = Vector3.one;

            EntityMonster monsterEntity = new EntityMonster() { stateManager = stateManager };
            MonsterController monsterController = monster.GetComponent<MonsterController>();
            if (monsterController)
                monsterController.Init();
            monsterEntity.Idle();
            monsterEntity.SetController(monsterController);
            monsterEntity.skillManager = skillManager;
            monsterEntity.battleManager = this;
            monster.name = "monster_" + LevelID + "_" + i;
            monsterEntity.Name = monster.name;


            EntityData entityData = new EntityData()
            {
                hp = monsterCfg.hp,
                ad = monsterCfg.ad,
                ap = monsterCfg.ap,
                addef = monsterCfg.addef,
                apdef = monsterCfg.apdef,
                dodge = monsterCfg.dodge,
                pierce = monsterCfg.pierce,
                critical = monsterCfg.critical,
                Level = list[i].level,
            };
            monsterEntity.SetEntityData(entityData);
            monsterEntity.SetMonsterCfg(monsterCfg);
            monster.SetActive(false);
            entityMonsterDic.Add(monster.name, monsterEntity);
            GameRoot.Instance.tipsWindow.SetWinState();
            GameRoot.Instance.tipsWindow.AddItemEntityHp(monster.name, monsterController.hpRoot, monsterEntity.HP);
        }
    }

    public void RemoveMonster(string key)
    {
        EntityMonster item = null;
        if (entityMonsterDic.TryGetValue(key, out item))
        {
            entityMonsterDic.Remove(key);
        }
        GameRoot.Instance.tipsWindow.RemoveItemEntityHp(key);
        AddMonsterPool(item.monsterCfg.ID, item.GetController().gameObject);
        if (mapManager != null)
        {
            if (entityMonsterDic.Count <= 0)
            {
                bool isPassLevel = mapManager.SetNextTrigger();
                if (isPassLevel)
                {
                    //通关
                    EndBattle(true, selfEntityPlayer.HP);
                }
            }
        }
    }

    public void  EndBattle(bool isWin,int hp)
    {
        audioService.StopBGMusic();
        battleSystem.EndBattle();
        if(isWin)
        {
            double endTime = TimerService.Instance.GetNowTime();
            //发送结算副本的包
            GameInstanceSystem.Instance.RequestBattleEnd(isWin, hp, (int)(endTime-battleSystem.StartTime)/1000);
        }
        else
        {
            battleSystem.battleEndWindow.SetBattleEndType(BattleEndType.None);
            battleSystem.battleEndWindow.SetWinState(true);
        }
    }

    public void SetMonsterListAction()
    {
        TimerService.Instance.AddTimerTask((int tid) =>
        {
            foreach (var item in entityMonsterDic)
            {
                item.Value.Born();
                TimerService.Instance.AddTimerTask((int id) =>
                {
                    item.Value.Idle();
                }, 1000);
            }
        }, 500);
    }

    private void AddMonsterPool(int ID, GameObject go)
    {
        List<GameObject> list = null;
        if (MonsterPoolDic.TryGetValue(ID, out list))
        {
            list.Add(go);
        }
        else
        {
            list = new List<GameObject>();
            list.Add(go);
            MonsterPoolDic.Add(ID, list);
        }
    }

    private GameObject GetMonsterToPool(int ID)
    {
        List<GameObject> list = null;
        if (MonsterPoolDic.TryGetValue(ID, out list))
        {
            GameObject go = null;
            if (list.Count > 0)
            {
                go = list[0];
                list.RemoveAt(0);
            }
            return go;
        }
        else
        {
            return null;
        }
    }

    private void OpenFightWindow()
    {
        battleSystem.playerCtrlWindow.SetWinState();
    }

    public Vector2 GetDirInput()
    {
        return battleSystem.GetDirInput();
    }

    public EntityPlayer GetSelfEntityPlayer()
    {
        return selfEntityPlayer;
    }

    public void SetSelfEntityPlayer()
    {
        selfEntityPlayer =null;
    }


    public void SetPlayerDir(Vector2 dir)
    {
        if (!selfEntityPlayer.canControl) return;

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
        if (selfEntityPlayer.currEntityState == EntityState.Attack)
        {
            selfEntityPlayer.comboQueue.Clear();
            selfEntityPlayer.nextSkilID = 0;
            selfEntityPlayer.Idle();
        }
        selfEntityPlayer.Attack(ID);
    }

    public void NormalAttack()
    {
        if (selfEntityPlayer.currEntityState == EntityState.Attack)
        {
            double nowTime = TimerService.Instance.GetNowTime();
            if (nowTime - lastTime < Constants.ComboSpace && lastTime != 0)
            {
                nComboIndex++;
                nComboIndex = nComboIndex % (ComboArr.Length - 1);
                selfEntityPlayer.comboQueue.Enqueue(ComboArr[nComboIndex]);
                lastTime = TimerService.Instance.GetNowTime();
            }
        }
        else if (selfEntityPlayer.currEntityState == EntityState.Idle || selfEntityPlayer.currEntityState == EntityState.Move)
        {
            nComboIndex = 0;
            lastTime = TimerService.Instance.GetNowTime();
            selfEntityPlayer.Attack(ComboArr[nComboIndex]);
        }
    }

    public List<EntityMonster> GetMonsterList()
    {
        List<EntityMonster> list = new List<EntityMonster>();
        foreach (var item in entityMonsterDic)
        {
            list.Add(item.Value);
        }
        return list;
    }


    public bool GetCanSkill()
    {
        return selfEntityPlayer.bCanSkill;
    }
}
