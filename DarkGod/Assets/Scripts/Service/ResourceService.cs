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

public class ResourceService : MonoBehaviour
{
    public static ResourceService Instance { get; private set; }

    Dictionary<string, AudioClip> AudioClipCacheDic = new Dictionary<string, AudioClip>();

    public void Init()
    {
        Instance = this;
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

    //加载音频
    public AudioClip LoadAutioClip(string path,bool cache)
    {
        AudioClip clip = null;
        if (!AudioClipCacheDic.TryGetValue(path,out clip))
        {
            clip = Resources.Load<AudioClip>(path);
            if (clip==null)
            {
                return null;
            }
            if(cache)
            {
                AudioClipCacheDic.Add(path, clip);
            }
        }
       
        return clip;
    }
}
