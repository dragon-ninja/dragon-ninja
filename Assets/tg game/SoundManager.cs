using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
	private Dictionary<string, AudioSource> mapAudioDatas = new Dictionary<string, AudioSource>();

	private string strBGMTag = "";

	private bool soundState = true;

	private bool bgmState = true;

	private void Awake()
	{
		Object.DontDestroyOnLoad(this);
		int childCount = base.transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			Transform child = base.transform.GetChild(i);
			mapAudioDatas.Add(child.gameObject.name, child.GetComponent<AudioSource>());
		}
	}

	public void playBGM(string tag)
	{
		stopBGM();
		strBGMTag = tag;
		if (bgmState)
		{
			mapAudioDatas[strBGMTag].loop = true;
			mapAudioDatas[strBGMTag].volume = 1f;
			mapAudioDatas[strBGMTag].Play();
		}
	}

	public void stopBGM()
	{
		if (!strBGMTag.Equals(""))
		{
			mapAudioDatas[strBGMTag].Stop();
			strBGMTag = "";
		}
	}

	public void stopBGMFade()
	{
		if (!strBGMTag.Equals(""))
		{
			mapAudioDatas[strBGMTag].DOFade(0f, 0.5f);
			strBGMTag = "";
		}
	}

	public void playSound(string tag)
	{
		if (soundState)
		{
			mapAudioDatas[tag].Play();
		}
	}

	public void setSoundState(bool state)
	{
		soundState = state;
	}

	public void setBGMState(bool state)
	{
		bgmState = state;
		if (!strBGMTag.Equals(""))
		{
			if (bgmState)
			{
				mapAudioDatas[strBGMTag].loop = true;
				mapAudioDatas[strBGMTag].Play();
			}
			else
			{
				mapAudioDatas[strBGMTag].Stop();
			}
		}
	}
}
