/****************************************************
    文件：CharacterCreatController.cs
	作者：朱江
    邮箱:  839149608@qq.com
    日期：2024/3/21 14:45:20
	功能：角色诞生管理器
*****************************************************/

using PEProtocol;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Types;

public class CharacterCreatController : SystemRoot
{
    public static CharacterCreatController Instance { get; private set; }
    private MapCfg mapCfg;
    PlayerController playerController;
    Dictionary<int, GameObject> NPCDic = new Dictionary<int, GameObject>();
    Dictionary<int, List<NpcInfo>> npcInfos = new Dictionary<int, List<NpcInfo>>();
    Dictionary<int, List<MonsterData>> monsterInfos = new Dictionary<int, List<MonsterData>>();
    
    public override void Init()
    {
        base.Init();
        Instance = this;
        Debug.Log("Init CharacterCreatController Succeed");
    }

    //诞生主角
    public void CreatPlayer(MapCfg cfg, string path = PathDefine.AssissnCityPlayerPrefab)
    {
        if (cfg != null)
            mapCfg = cfg;
        GameObject player = resService.LoadPrefab(path, true);
        player.transform.position = mapCfg.playerBomPos;
        player.transform.localEulerAngles = mapCfg.playerBomRote;
        if (mapCfg.ID == 10000)
            player.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        playerController = player.GetComponent<PlayerController>();

        Camera.main.transform.position = mapCfg.mainCamPos;
        Camera.main.transform.localEulerAngles = mapCfg.mainCamRote;
        if (playerController)
            playerController.Init();
    }

    public PlayerController GetPlayer()
    {
        return playerController;
    }

    public void CreatMainSceneNpc(MapCfg cfg)
    {
        var npcInfoArray = mapCfg.monsterLst.Split('|');
        List<NpcInfo> list = new List<NpcInfo>();
        if (!npcInfos.ContainsKey(cfg.ID))
        {
            for (int i = 0; i < npcInfoArray.Length; i++)
            {
                var temp0 = npcInfoArray[i].Replace("\"", "").Split(';');
                var temp1 = temp0[1].Split('，');
                var temp2 = temp0[2].Split('，');
                var temp3 = temp0[3].Split('，');

                NpcInfo npcInfo = new NpcInfo()
                {
                    ID = int.Parse(temp0[0]),
                    pos = new Vector3(float.Parse(temp1[0]), float.Parse(temp1[1]), float.Parse(temp1[2])),
                    eulerAngles = new Vector3(float.Parse(temp2[0]), float.Parse(temp2[1]), float.Parse(temp2[2])),
                    scale = new Vector3(float.Parse(temp3[0]), float.Parse(temp3[1]), float.Parse(temp3[2])),
                };
                list.Add(npcInfo);
            }
            npcInfos.Add(cfg.ID, list);
        }
        else
        {
            list = npcInfos[cfg.ID];
        }

        for (int i = 0; i < list.Count; i++)
        {
            GameObject npc = resService.LoadPrefab(PathDefine.AssissnCityNPCPrefab + list[i].ID, true);
            npc.transform.position = list[i].pos;
            npc.transform.localEulerAngles = list[i].eulerAngles;
            npc.transform.localScale = list[i].scale;
            NPCDic.Add(list[i].ID, npc);
        }
    }


    public void ClearMapNpc(int mapID)
    {
        List<NpcInfo> list = new List<NpcInfo>();
        if (npcInfos.TryGetValue(mapID, out list))
        {
            for (int i = 0; i < list.Count; i++)
            {
                NPCDic.Remove(list[i].ID);
            }
        }
        npcInfos.Remove(mapID);
    }

    public void InitMonsterCfg(MapCfg cfg)
    {
        mapCfg = cfg;
        monsterInfos.Clear();

        var npcInfoArray = cfg.monsterLst.Split('#');
        int levelID = 0;
        for (int i = 0; i < npcInfoArray.Length; i++)
        {
            if (!string.IsNullOrEmpty(npcInfoArray[i].Trim()))
            {

                if (monsterInfos.ContainsKey(i)) continue;
                var arr = npcInfoArray[i].Replace("\"", "").Split('|');
                List<string> strlist = new List<string>();
                for (int j = 0; j < arr.Length; j++)
                {
                    if (!string.IsNullOrEmpty(arr[j].Trim()))
                    {
                        strlist.Add(arr[j]);
                    }
                }
                List<MonsterData> list = new List<MonsterData>();
                for (int j = 0; j < strlist.Count; j++)
                {
                    var temp = strlist[j].Replace("\"", "").Split(';');
                    MonsterData npcInfo = new MonsterData()
                    {
                        ID = int.Parse(temp[0]),
                        pos = new Vector3(float.Parse(temp[1]), float.Parse(temp[2]), float.Parse(temp[3])),
                        eulerAngles = new Vector3(0, float.Parse(temp[4]), 0),
                        level = int.Parse(temp[5]),
                    };
                    list.Add(npcInfo);
                   
                }
                if(list.Count>0)
                {
                    levelID++;
                   monsterInfos.Add(levelID, list);
                }
            }
        }
    }

    public List<MonsterData> GetBattleSceneMonster(int levelID)
    {
        List<MonsterData> list = new List<MonsterData>();
        if (!monsterInfos.ContainsKey(levelID))
        {
            return null;
        }
        else
        {
            list = monsterInfos[levelID];
        }

        return list;
    }

    public Vector3 GetNPCPos(int mapID, int id)
    {
        Vector3 temp = Vector3.zero;
        List<NpcInfo> list = new List<NpcInfo>();
        if (npcInfos.TryGetValue(mapID, out list))
        {
            if (list.Count - 1 >= id)
            {
                temp = list[id].pos;
            }
        }

        return temp;
    }

    public Vector3 GetNPCNavPos(int mapID, int id)
    {
        Vector3 temp = Vector3.zero;
        List<NpcInfo> list = new List<NpcInfo>();
        if (npcInfos.TryGetValue(mapID, out list))
        {
            if (list.Count - 1 >= id)
            {
                temp = list[id].pos;
            }
        }
        return temp + NPCDic[id].transform.forward * 2f;
    }
}

public struct NpcInfo
{
    public int ID;
    public Vector3 pos;
    public Vector3 eulerAngles;
    public Vector3 scale;
}

public struct MonsterData
{
    public int ID;
    public Vector3 pos;
    public Vector3 eulerAngles;
    public int level;
}