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
        return CsvUtils.ParseRowAll(string.Format(Application.dataPath + "/Resources/ResCfgs/{0}.csv", csvName));
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
        List<List<string>> temp = CsvUtils.ParseRowAll(Application.dataPath + "/Resources/ResCfgs/rdname.csv");
        for (int i = 0; i < temp.Count; i++)
        {
            string ID = temp[i][0];
            string surname = temp[i][1];
            string man = temp[i][2];
            string woman = temp[i][3];
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
        List<List<string>> temp = ParseRowAll("map");
        for (int i = 0; i < temp.Count; i++)
        {

            if (!int.TryParse(temp[i][0], out int id))
            {
                continue;
            }
            //for (int j = 0; j < temp[i].Count; j++)
            //{
            //    Debug.LogError("temp[i]  " + "i    " + i + "   j   " + j + temp[i][j]);
            //}
            MapCfg cfg = new MapCfg();
            cfg.ID = id;
            cfg.mapName = temp[i][1];
            cfg.sceneName = temp[i][2];
            cfg.power = int.Parse(temp[i][3]);
            cfg.mainCamPos = StrToVector3(temp[i][4]);
            cfg.mainCamRote = StrToVector3(temp[i][5]);
            cfg.playerBomPos = StrToVector3(temp[i][6]);
            cfg.playerBomRote = StrToVector3(temp[i][7]);
            cfg.monsterLst = temp[i][8];
           
            cfg.exp = int.Parse(temp[i][9]);
            cfg.coin = int.Parse(temp[i][10]);
            cfg.crystal = int.Parse(temp[i][11]);
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


    Dictionary<int, AutoGuideData> autoGuideDataDic = new Dictionary<int, AutoGuideData>();
    public void InitAutoGuideCfg()
    {
        CommonUtility.StartCountTime();
        autoGuideDataDic.Clear();
        List<List<string>> temp = ParseRowAll("guide");
        for (int i = 0; i < temp.Count; i++)
        {
            if (!int.TryParse(temp[i][0], out int id))
            {
                continue;
            }
            AutoGuideData cfg = new AutoGuideData();
            cfg.ID = id;
            cfg.npcID = int.Parse(temp[i][1]);
            cfg.dilogArr = temp[i][2];
            cfg.actID = int.Parse(temp[i][3]);
            cfg.coin = int.Parse(temp[i][4]);
            cfg.exp = int.Parse(temp[i][5]);
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
    #endregion


}
