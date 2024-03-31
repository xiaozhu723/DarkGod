/****************************************************
    文件：ResourceService.cs
	作者：朱江
    邮箱:  839149608@qq.com
    日期：2024/3/10 15:47:34
	功能：资源加载服务
*****************************************************/

using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;
using System.Xml;
using PEProtocol;
using System.Runtime.InteropServices.ComTypes;
using System.Xml.Linq;

public class ResourceService : MonoBehaviour
{
    public static ResourceService Instance { get; private set; }

    Dictionary<string, AudioClip> AudioClipCacheDic = new Dictionary<string, AudioClip>();
    Dictionary<string, Sprite> SpriteCacheDic = new Dictionary<string, Sprite>();

    public void Init()
    {
        Instance = this;
        InitRDNameCfg2();
        InitMapCfg();
        InitAutoGuideCfg();
        InitStrongCfg();
        InitTaskCfg();
        InitSkillCfg();
        InitSkillMoveCfg();
        Debug.Log("Init ResourceService....");
    }

    //异步加载场景
    public void AsyncLoadScene(string sceneName, Action function)
    {
        GameRoot.Instance.lodingWindow.SetWinState();
        StartCoroutine(LoadingSceneAsync(sceneName, function));
    }

    public IEnumerator LoadingSceneAsync(string sceneName, Action function)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncOperation.isDone)
        {
            GameRoot.Instance.lodingWindow.SetProgress(asyncOperation.progress);
            yield return asyncOperation.progress;
        }
        yield return asyncOperation;
        GameRoot.Instance.lodingWindow.SetProgress(1);
        if (function != null)
        {
            function();
        }

    }

    Dictionary<string, GameObject> GoDic =new Dictionary<string, GameObject> ();
    //加载预制体
    public GameObject LoadPrefab(string path,bool cache =false)
    {
        GameObject Prefab = null;
        if(!GoDic.TryGetValue(path,out Prefab))
        {
            Prefab = Resources.Load<GameObject>(path);
            if(cache)
            {
                GoDic.Add(path, Prefab) ;
            }
        }

        GameObject go = null;
        if(Prefab!=null)
        {
            go = Instantiate(Prefab);
        }
        return go;
    }

    //加载音频
    public AudioClip LoadAutioClip(string path, bool cache)
    {
        AudioClip clip = null;
        if (!AudioClipCacheDic.TryGetValue(path, out clip))
        {
            clip = Resources.Load<AudioClip>(path);
            if (clip == null)
            {
                return null;
            }
            if (cache)
            {
                AudioClipCacheDic.Add(path, clip);
            }
        }

        return clip;
    }

    //加载图片
    public Sprite LoadSprite(string path, bool cache =false)
    {
        Sprite sprite = null;
        if (!SpriteCacheDic.TryGetValue(path, out sprite))
        {
            sprite = Resources.Load<Sprite>(path);
            if (sprite == null)
            {
                return null;
            }
            if (cache)
            {
                SpriteCacheDic.Add(path, sprite);
            }
        }

        return sprite;
    }
    #region 加载配置
    private XmlNodeList ParseXMLCfg(string xmlCfg)
    {
        TextAsset xmlAsset = Resources.Load<TextAsset>(xmlCfg);
        if (xmlAsset == null)
        {
            Debug.LogError("xml file: " + xmlCfg + " not exist");
            return null;
        }
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(xmlAsset.text);
        XmlNodeList nodeList = doc.SelectSingleNode("root").ChildNodes;

        return nodeList;
    }
    List<string> surNameList = new List<string>();
    List<string> manList = new List<string>();
    List<string> woManList = new List<string>();

    List<List<string>> ParseRowAll(string csvName)
    {
        string path =
#if UNITY_ANDROID && !UNITY_EDITOR
		Application.streamingAssetsPath; //安卓的Application.streamingAssetsPath已默认有"file://"
#elif UNITY_IOS && !UNITY_EDITOR
		"file://" + Application.streamingAssetsPath;
#elif UNITY_STANDLONE_WIN || UNITY_EDITOR
            "file://" + Application.streamingAssetsPath;
#else
		string.Empty;
#endif

        return CsvUtils.ParseRowAll(string.Format(path + "/StreamingAssets/ResCfgs/{0}.csv", csvName));
    }
    //初始化随机名字配置
    public void InitRDNameCfg()
    {
        CommonUtility.StartCountTime();
        XmlNodeList nodeList = ParseXMLCfg(PathDefine.RDNameCfg);
        if (nodeList == null) return;
        for (int i = 0; i < nodeList.Count; i++)
        {
            XmlElement ele = nodeList[i] as XmlElement;
            if (ele.GetAttributeNode("ID") == null) continue;

            int ID = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);
            foreach (XmlElement e in nodeList[i].ChildNodes)
            {
                switch (e.Name)
                {
                    case "surname":
                        surNameList.Add(e.InnerText);
                        break;
                    case "man":
                        manList.Add(e.InnerText);
                        break;
                    case "woman":
                        woManList.Add(e.InnerText);
                        break;
                }
            }
        }
        CommonUtility.StopCountTime("XML读取速度");
    }
    public void InitRDNameCfg2()
    {
        CommonUtility.StartCountTime();
        surNameList.Clear();
        manList.Clear();
        woManList.Clear();
        var arr = CSVHelper.ReadCSVFile(CSVConfig.csvFolderPath, "rdname");
        string[,] temp = CSVHelper.AnalysisCsvData(arr);
        int row = temp.GetLength(0);
        int col = temp.GetLength(1);
        //List<List<string>> temp = ParseRowAll("rdname");
        for (int i = 0; i < row; i++)
        {
            string ID = temp[i, 0];
            string surname = temp[i, 1];
            string man = temp[i, 2];
            string woman = temp[i, 3];
            surNameList.Add(surname);
            manList.Add(man);
            woManList.Add(woman);
        }
        CommonUtility.StopCountTime("Csv读取速度");
    }
    

    public string GetRadomName(bool man = true)
    {
        if (surNameList.Count <= 0 || manList.Count <= 0 || woManList.Count <= 0) return "";
        string name = "";
        System.Random random = new System.Random();
        name = surNameList[CommonUtility.RadomInt(0, surNameList.Count - 1, random)];
        name += man ? manList[CommonUtility.RadomInt(0, manList.Count - 1, random)] : woManList[CommonUtility.RadomInt(0, woManList.Count - 1, random)]; ;

        return name;
    }
    Dictionary<int, MapCfg> mapCfgDic = new Dictionary<int, MapCfg>();
    public void InitMapCfg()
    {
        CommonUtility.StartCountTime();
        mapCfgDic.Clear();
        //List<List<string>> temp = ParseRowAll("map");
        var arr = CSVHelper.ReadCSVFile(CSVConfig.csvFolderPath, "map");
        string[,] temp = CSVHelper.AnalysisCsvData(arr);
        int row = temp.GetLength(0);
        int col = temp.GetLength(1);
        for (int i = 0; i < row; i++)
        {

            if (!int.TryParse(temp[i,0], out int id))
            {
                continue;
            }
            //for (int j = 0; j < temp[i].Count; j++)
            //{
            //    Debug.LogError("temp[i]  " + "i    " + i + "   j   " + j + temp[i,j]);
            //}
            MapCfg cfg = new MapCfg();
            cfg.ID = id;
            cfg.mapName = temp[i,1];
            cfg.sceneName = temp[i,2];
            cfg.power = int.Parse(temp[i,3]);
            cfg.mainCamPos = StrToVector3(temp[i,4]);
            cfg.mainCamRote = StrToVector3(temp[i,5]);
            cfg.playerBomPos = StrToVector3(temp[i,6]);
            cfg.playerBomRote = StrToVector3(temp[i,7]);
            cfg.monsterLst = temp[i,8];
           
            cfg.exp = int.Parse(temp[i,9]);
            cfg.coin = int.Parse(temp[i,10]);
            cfg.crystal = int.Parse(temp[i,11]);
            mapCfgDic.Add(id, cfg);
        }
        CommonUtility.StopCountTime("MapCfg.Csv读取速度");
    }

    public MapCfg GetMapCfg(int id)
    {
        MapCfg cfg = null;
        if (mapCfgDic.TryGetValue(id, out cfg))
        {
            return cfg;
        }
        return cfg;
    }

    private Vector3 StrToVector3(string str)
    {
        Vector3 vector = Vector3.zero;
        var temp = str.Split(';');
        if (temp.Length >= 3)
        {
            vector = new Vector3(float.Parse(temp[0]), float.Parse(temp[1]), float.Parse(temp[2]));
        }
        return vector;
    }

    //引导配置
    Dictionary<int, AutoGuideData> autoGuideDataDic = new Dictionary<int, AutoGuideData>();
    public void InitAutoGuideCfg()
    {
        CommonUtility.StartCountTime();
        autoGuideDataDic.Clear();
        //List<List<string>> temp = ParseRowAll("guide");

        var arr = CSVHelper.ReadCSVFile(CSVConfig.csvFolderPath, "guide");
        string[,] temp = CSVHelper.AnalysisCsvData(arr);
        int row = temp.GetLength(0);
        int col = temp.GetLength(1);
        for (int i = 0; i < row; i++)
        {
            if (!int.TryParse(temp[i,0], out int id))
            {
                continue;
            }
            AutoGuideData cfg = new AutoGuideData();
            cfg.ID = id;
            cfg.npcID = int.Parse(temp[i,1]);
            cfg.dilogArr = temp[i,2];
            cfg.actID = int.Parse(temp[i,3]);
            cfg.coin = int.Parse(temp[i,4]);
            cfg.exp = int.Parse(temp[i,5]);
            autoGuideDataDic.Add(id, cfg);
        }
        CommonUtility.StopCountTime("Guide.Csv读取速度");
    }

    public AutoGuideData GetAutoGuideCfg(int id)
    {
        AutoGuideData cfg = null;
        if(autoGuideDataDic.TryGetValue(id,out cfg))
        {

        }
        return cfg;
       
    }
    GuideDataInfo[] DataInfos = null;
    public GuideDataInfo GetGuideDataInfo(int id)
    {
        GuideDataInfo info = null;
        if (DataInfos == null)
        {
            DataInfos = GuideData.Instance.guideDataInfos;
        }
        for (int i = 0; i < DataInfos.Length; i++)
        {
            if (DataInfos[i].nNpcID ==id )
            {
                info = DataInfos[i];
                return info;
            }
        }
        return info;
    }


    //强化配置
    Dictionary<int, List<StrongData>> StrongDataDic = new Dictionary<int, List<StrongData>>();
    public void InitStrongCfg()
    {
        CommonUtility.StartCountTime();
        StrongDataDic.Clear();
        //List<List<string>> temp = ParseRowAll("strong");
        int partID=-1;
        string path = "";
        var arr = CSVHelper.ReadCSVFile(CSVConfig.csvFolderPath, "strong");
        string[,] temp = CSVHelper.AnalysisCsvData(arr);
        int row = temp.GetLength(0);
        int col = temp.GetLength(1);
        for (int i = 0; i < row; i++)
        {
            if (!int.TryParse(temp[i,0], out int id))
            {
                continue;
            }
        
            StrongData cfg = new StrongData();
            cfg.ID = id;
            int paId = int.Parse(temp[i,1]);
            if(partID!= paId)
            {
                partID = paId;
                StrongDataDic.Add(partID, new List<StrongData>());
            }
            cfg.nPartID = paId;
            cfg.nStarLevel = int.Parse(temp[i,2]);
            cfg.nAddHP = int.Parse(temp[i,3]);
            cfg.nAddHurt = int.Parse(temp[i,4]);
            cfg.nAddDef = int.Parse(temp[i,5]);
            cfg.nNeedLevel = int.Parse(temp[i,6]);
            cfg.nNeedCoin = int.Parse(temp[i,7]);
            
            cfg.nNeedCrystal = int.Parse(temp[i,8]);
            string str = temp[i,9];
            if (str != "")
            {
                path = str;
            }
            cfg.iconPath = path;
            StrongDataDic[partID].Add(cfg);
        }
        CommonUtility.StopCountTime("Strong.Csv读取速度");
    }

    public StrongData GetStrongCfg(int id,int starLevel)
    {
        List<StrongData> cfg = null;
        StrongData data = null;
        if (StrongDataDic.TryGetValue(id, out cfg))
        {
            for (int i = 0; i < cfg.Count; i++)
            {
                if(starLevel== cfg[i].nStarLevel)
                {
                    data = cfg[i];
                    return data;
                }
            }
        }
        return data;
    }

    public int GetStrongMaxSartLevel(int id)
    {
        int starLevel = 0;
        List<StrongData> cfg = null;
        if (StrongDataDic.TryGetValue(id, out cfg))
        {
            for (int i = 0; i < cfg.Count; i++)
            {
                if (cfg[i].nStarLevel> starLevel)
                {
                    starLevel = cfg[i].nStarLevel;
                }
            }
        }
        return starLevel;
    }

    public int GetPropAddValPreLv(int pos, int starlv, int type)
    {
        int val = 0;
        List<StrongData> cfg = null;
        if (!StrongDataDic.TryGetValue(pos, out cfg))
        {
            return val;
        }

        for (int i = 0; i < cfg.Count; i++)
        {
            if(starlv>= cfg[i].nStarLevel)
            {
                switch (type)
                {
                    case 1://hp
                        val += cfg[i].nAddHP;
                        break;
                    case 2://hurt
                        val += cfg[i].nAddHurt;
                        break;
                    case 3://def
                        val += cfg[i].nAddDef;
                        break;
                }
            }
          
        }
        return val;
    }

    //任务配置
    List<TaskData> TaskDataList = new List<TaskData>();
    public void InitTaskCfg()
    {
        CommonUtility.StartCountTime();
        TaskDataList.Clear();
        //List<List<string>> temp = ParseRowAll("taskreward");
        int partID = -1;
        string path = "";
        var arr = CSVHelper.ReadCSVFile(CSVConfig.csvFolderPath, "strong");
        string[,] temp = CSVHelper.AnalysisCsvData(arr);
        int row = temp.GetLength(0);
        int col = temp.GetLength(1);
        for (int i = 0; i < row; i++)
        {
            if (!int.TryParse(temp[i,0], out int id))
            {
                continue;
            }

            TaskData cfg = new TaskData();
            cfg.ID = id;
            cfg.strTaskName = temp[i,1];
            cfg.nMaxCount = int.Parse(temp[i,2]);
            cfg.nExp = int.Parse(temp[i,3]);
            cfg.nCoin = int.Parse(temp[i,4]);
            TaskDataList.Add(cfg);
        }
        CommonUtility.StopCountTime("taskreward.Csv读取速度");
    }

    public List<TaskData> GetTaskDataList()
    {
        return TaskDataList;
    }

    //技能配置
    List<SkillData> SkillDataList = new List<SkillData>();
    List<SkillMove> SkillMoveDataList = new List<SkillMove>();
    public void InitSkillCfg()
    {
        CommonUtility.StartCountTime();
        SkillDataList.Clear();
     
        var arr = CSVHelper.ReadCSVFile(CSVConfig.csvFolderPath, "skill");
        string[,] temp = CSVHelper.AnalysisCsvData(arr);
        int row = temp.GetLength(0);
        int col = temp.GetLength(1);
        for (int i = 0; i < row; i++)
        {
            if (!int.TryParse(temp[i, 0], out int id))
            {
                continue;
            }

            SkillData cfg = new SkillData();
            cfg.ID = id;
            cfg.strSkillName = temp[i, 1];
            cfg.nCDTime = int.Parse(temp[i, 2]);
            cfg.nSkillTime = int.Parse(temp[i, 3]);
            
            cfg.nAniAction = int.Parse(temp[i, 4]);//动画播放标志位
            cfg.strFxName = temp[i, 5] ;//特效名字
            cfg.isCombo = int.Parse(temp[i,6])==1;
            cfg.isCollide = int.Parse(temp[i, 7]) == 1;
            cfg.isBreak = int.Parse(temp[i, 8]) == 1;
            cfg.dmgType = int.Parse(temp[i, 9]);
            cfg.skillMoveLst = StrToArray(temp[i, 10]);
            cfg.skillActionLst = StrToArray(temp[i, 11]);
            cfg.skillDamageLst = StrToArray(temp[i, 12]); 
            SkillDataList.Add(cfg);
        }
        CommonUtility.StopCountTime("skill.Csv读取速度");
    }
    public void InitSkillMoveCfg()
    {
        CommonUtility.StartCountTime();
        SkillMoveDataList.Clear();

        var arr = CSVHelper.ReadCSVFile(CSVConfig.csvFolderPath, "skillmove");
        string[,] temp = CSVHelper.AnalysisCsvData(arr);
        int row = temp.GetLength(0);
        int col = temp.GetLength(1);
        for (int i = 0; i < row; i++)
        {
            if (!int.TryParse(temp[i, 0], out int id))
            {
                continue;
            }

            SkillMove cfg = new SkillMove();
            cfg.ID = id;
            cfg.nDelayTime = int.Parse(temp[i, 1]);
            cfg.nMoveTime = int.Parse(temp[i, 2]);
            cfg.moveDis = float.Parse(temp[i, 3]);
            SkillMoveDataList.Add(cfg);
        }
        CommonUtility.StopCountTime("skill.Csv读取速度");
    }
    public int[] StrToArray(string str)
    {
        int[] arr = null;
        if(string.IsNullOrEmpty(str))
        {
            return arr;
        }
        var temp = str.Split('|');
        if(temp.Length<=0)
        {
            return arr;
        }
        arr =new int[temp.Length];
        for (int i = 0; i < temp.Length; i++)
        {
            arr[i] = int.Parse(temp[i]);
        }
        return arr;
    }

    public List<SkillData> GetSkillDataList()
    {
        return SkillDataList;
    }

    public SkillData GetSkillData(int id)
    {
        for (int i = 0; i < SkillDataList.Count; i++)
        {
            if (SkillDataList[i].ID == id)
            {
                return SkillDataList[i];
            }
        }
        return null;
    }

    public SkillMove GetSkillMoveData(int id)
    {
        for (int i = 0; i < SkillMoveDataList.Count; i++)
        {
            if (SkillMoveDataList[i].ID == id)
            {
                return SkillMoveDataList[i];
            }
        }
        return null;
    }
    #endregion


}
