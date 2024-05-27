using CodeStage.AntiCheat.ObscuredTypes;
using DG.Tweening;
using UnityEngine;

public class TutorialManager : Singleton<TutorialManager>
{
	private ObscuredBool tutorialDrop = false;

	private ObscuredBool tutorialFail = false;

	private ObscuredInt tutorialEquipmentCreate = 0;

	public TutorialVillage tutorialVillage;

	public TutorialVillageTouch tutorialVillageTouch;

	public TutorialGame tutorialGame;

	public TutorialGameTouch tutorialGameTouch;

	public bool touchBlack;

	public int attachIndex;

	public bool getTutorialDrop()
	{
		return tutorialDrop;
	}

	public bool getTutorialFail()
	{
		return tutorialFail;
	}

	public void failClear()
	{
		tutorialFail = false;
	}

	public bool getGameTutorialState()
	{
		return tutorialGame != null;
	}

	public bool createTutorialEquipment()
	{
		++tutorialEquipmentCreate;
		if ((int)tutorialEquipmentCreate > 3)
		{
			tutorialDrop = false;
			return true;
		}
		return false;
	}

	public void initVillageTutorial()
	{
		if ((bool)Singleton<DataManager>.Instance.tutorialVillage)
		{
			tutorialVillage = null;
			tutorialVillageTouch = null;
		}
		tutorialGame = null;
		tutorialGameTouch = null;
	}

	public void onStartTouch()
	{
		if (tutorialVillage != null)
		{
			tutorialVillage.onStartTutorial();
		}
	}

	public void onStartTouchEnd()
	{
		if (tutorialVillage != null)
		{
			tutorialVillage.onStartTutorialEnd();
		}
	}

	public void onDungeonOpenStart()
	{
		if (tutorialVillage != null)
		{
			tutorialVillage.onDungeonStart();
		}
	}

	public void onDungeon()
	{
		if (tutorialVillageTouch != null)
		{
			tutorialVillageTouch.onDungeon();
		}
	}

	public void initGameTutorial()
	{
		Debug.Log("-------------------------Singleton<DataManager>.Instance.listWeapons.Count");
		Debug.Log(Singleton<DataManager>.Instance.listWeapons.Count);

		if ((bool)Singleton<DataManager>.Instance.tutorialGame
			|| Singleton<DataManager>.Instance.listWeapons.Count>1
			)
		{
			tutorialGame = null;
			tutorialGameTouch = null;
		}
		if (tutorialGame != null)
		{
			tutorialDrop = true;
			tutorialFail = true;
		}
	}

	public bool onFirstInventoryFlag()
	{
		if (tutorialGameTouch != null)
		{
			tutorialGameTouch.onTouch();
			touchBlack = true;
			return true;
		}
		return false;
	}

	public void onInventoryOpen()
	{
		if (tutorialGameTouch != null)
		{
			tutorialGameTouch.gameObject.SetActive(value: false);
			touchBlack = false;
		}
	}

	public void onInventoryAttach()
	{
		if (tutorialGame != null)
		{
			attachIndex++;
			if (attachIndex == 1)
			{
				tutorialGame.onAttach1();
			}
			else if (attachIndex == 2)
			{
				tutorialGame.offAttach1();
				tutorialGame.onAttach2();
			}
			else
			{
				tutorialGame.offAttach2();
				tutorialEnded();
			}
		}
	}

	public void tutorialEnded()
	{
		tutorialGame = null;
		tutorialGameTouch = null;
		tutorialVillage = null;
		tutorialVillageTouch = null;
	}

	public static void fingerAction(GameObject finger)
	{
		Sequence sequence = DOTween.Sequence();
		sequence.Append(finger.transform.DOScale(1.1f, 0.2f).SetDelay(1f));
		sequence.Append(finger.transform.DOScale(1f, 0.2f));
		sequence.SetLoops(-1);
		sequence.Play();
	}
}
