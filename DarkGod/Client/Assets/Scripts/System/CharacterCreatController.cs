/****************************************************
    文件：CharacterCreatController.cs
	作者：朱江
    邮箱:  839149608@qq.com
    日期：2024/3/21 14:45:20
	功能：角色诞生管理器
*****************************************************/

using System.Collections.Generic;
using UnityEngine;

public class CharacterCreatController : SystemRoot
{
    public static CharacterCreatController Instance { get; private set; }
    private MapCfg mapCfg;
    PlayerController playerController;
    Dictionary<int, GameObject> NPCDic = new Dictionary<int, GameObject>();
    List<NpcInfo> npcInfos = new List<NpcInfo>();
    public override void Init()
    {
        base.Init();
        Instance = this;
        mapCfg = resService.GetMapCfg(Constants.MainCitySceneID);
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

    public void CreatMainSceneNpc()
    {
        var npcInfoArray = mapCfg.monsterLst.Split('|');
        if (npcInfos.Count != npcInfoArray.Length)
        {
            npcInfos.Clear();
            for (int i = 0; i < npcInfoArray.Length; i++)
            {
                var temp = npcInfoArray[i].Replace("\"", "").Split(';');
                var temp1 = temp[1].Split('，');
                var temp2 = temp[2].Split('，');
                var temp3 = temp[3].Split('，');

                NpcInfo npcInfo = new NpcInfo()
                {
                    ID = int.Parse(temp[0]),
                    pos = new Vector3(float.Parse(temp1[0]), float.Parse(temp1[1]), float.Parse(temp1[2])),
                    eulerAngles = new Vector3(float.Parse(temp2[0]), float.Parse(temp2[1]), float.Parse(temp2[2])),
                    scale = new Vector3(float.Parse(temp3[0]), float.Parse(temp3[1]), float.Parse(temp3[2])),
                };
                npcInfos.Add(npcInfo);
            }
        }
        for (int i = 0; i < npcInfos.Count; i++)
        {
            GameObject npc = resService.LoadPrefab(PathDefine.AssissnCityNPCPrefab + npcInfos[i].ID, true);
            npc.transform.position = npcInfos[i].pos;
            npc.transform.localEulerAngles = npcInfos[i].eulerAngles;
            npc.transform.localScale = npcInfos[i].scale;
            NPCDic.Add(npcInfos[i].ID, npc);
        }
    }

    public Vector3 GetNPCPos(int id)
    {
        Vector3 temp = Vector3.zero;
        if (npcInfos.Count - 1 >= id)
        {
            temp = npcInfos[id].pos;
        }
        return temp;
    }

    public Vector3 GetNPCNavPos(int id)
    {
        Vector3 temp = Vector3.zero;
        if (npcInfos.Count - 1 >= id)
        {
            temp = npcInfos[id].pos;
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