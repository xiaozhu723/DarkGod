/****************************************************
    文件：AudioService.cs
	作者：朱江
    邮箱:  839149608@qq.com
    日期：2024/3/10 21:6:10
	功能：音频服务类
*****************************************************/

using UnityEngine;

public class AudioService : SystemRoot
{
    public static AudioService Instance { get; private set; }
    public AudioSource BgAudio;
    public AudioSource UIAudio;

    public override void Init()
    {
        base.Init();
        Instance = this;
    }
    public void PlayBGMusic(string audioName, bool isLoop = false)
    {
        if (BgAudio == null) return;
        AudioClip audioClip = ResourceService.Instance.LoadAutioClip("ResAudio/" + audioName, true);
        if (audioClip == null)
        {
            Debug.LogError(" AudioService.PlayBGMusic  audioClip==null  audioName= " + audioName);
            return;
        }
        if (BgAudio.clip == null || BgAudio.clip != audioClip)
        {
            BgAudio.clip = audioClip;
            BgAudio.loop = isLoop;
            BgAudio.Play();
        }
    }

    public void PlayUIMusic(string audioName)
    {
        if (UIAudio == null) return;
        AudioClip audioClip = ResourceService.Instance.LoadAutioClip("ResAudio/" + audioName, true);
        if (audioClip == null)
        {
            Debug.LogError("AudioService.PlayUIMusic  audioClip==null  audioName= " + audioName);
            return;
        }
        UIAudio.clip = audioClip;
        UIAudio.Play();
    }
}