using System.Collections.Generic;
using UnityEngine;

public class UIControlManager : Singleton<UIControlManager>
{
	public GameObject gameScene;

	private GameScene gameSceneComponent;

	public GameObject villageScene;

	private VillageScene villageSceneComponent;

	[Header("MAIN UI")]
	public GameObject gameUI;

	private GameUI gameUIComponent;

	public GameClearUI gameClearUI;

	private GameClearUI gameClearUIComponent;

	[Header("BASE UI OBJECTS")]
	public BaseUI inventoryUI;

	public BaseUI petInventoryUI;

	public BaseUI inappStoreUI;

	public BaseUI settingUI;

	public BaseUI stageLevelSelectUI;

	public BaseUI creditUI;

	public BaseUI boxStoreUI;

	[Header("POPUP UI OBJECTS")]
	public PopupYesNo popupYesNo;

	public EquipmentSkillPopup equipmentSkillPopup;

	private float delay;

	private int gameViewTouchIndex;

	public List<BaseUI> listOpenUIs = new List<BaseUI>();

	public void addOpenUI(BaseUI ui)
	{
		listOpenUIs.Add(ui);
	}

	public void closeUI(BaseUI ui)
	{
		listOpenUIs.Remove(ui);
	}

	private void Update()
	{
		if (delay < 0.5f)
		{
			delay += Time.deltaTime;
		}
		else if (Application.platform == RuntimePlatform.Android && UnityEngine.Input.GetKey(KeyCode.Escape))
		{
			delay = 0f;
			if (listOpenUIs.Count > 0)
			{
				listOpenUIs[listOpenUIs.Count - 1].onDelegate();
				listOpenUIs[listOpenUIs.Count - 1].onExit();
			}
			else
			{
				onPopupYesNo("Exit?", delegate
				{
					Application.Quit();
				}, delegate
				{
				});
			}
		}
	}

	public void initGameUI()
	{
		gameSceneComponent = gameScene.GetComponent<GameScene>();
		gameUIComponent = gameUI.GetComponent<GameUI>();
		gameClearUIComponent = gameClearUI.GetComponent<GameClearUI>();
		if (villageScene != null)
		{
			villageSceneComponent = villageScene.GetComponent<VillageScene>();
		}
	}

	public void refreshGameUIEquipments(EquipmentData helmetData, EquipmentData armorData, EquipmentData weaponData, EquipmentData horseData)
	{
		gameUIComponent = gameUI.GetComponent<GameUI>();
		gameUIComponent.setHelmetIndex(helmetData);
		gameUIComponent.setArmorIndex(armorData);
		gameUIComponent.setWeaponIndex(weaponData);
		gameUIComponent.setHorseIndex(horseData);
	}

	public void refreshGameUIStatus(int power, float critical, int shield, float speed, int hp)
	{
		gameUIComponent.refreshStatus(power, critical, shield, speed, hp);
	}

	public void setGameUIHP(float per)
	{
		gameUIComponent.setHpPercent(per);
	}

	public void setCoinUI(int coin)
	{
		gameUIComponent.setCoin(coin);
	}

	public void setEggUI(int egg)
	{
		gameUIComponent.setEgg(egg);
	}

	public void upGameUI()
	{
		gameUIComponent.settingUIup();
	}

	public void onRateUs()
	{
		gameUIComponent.onRateusButton();
	}

	public void startGameClearUI(UICallbackDelegate call, UICallbackDelegate ended)
	{
		gameClearUIComponent.onGameClear(call, ended);
	}

	public void onInventoryUI(UICallbackDelegate call)
	{
		onUI(inventoryUI, call);
	}

	public void onPetInventoryUI(UICallbackDelegate call)
	{
		onUI(petInventoryUI, call);
	}

	public void onInappStoreUI(UICallbackDelegate call)
	{
		onUI(inappStoreUI, call);
	}

	public void onSettingUI(UICallbackDelegate call)
	{
		onUI(settingUI, call);
	}

	public void onStageLevelSelectUI(UICallbackDelegate call)
	{
		onUI(stageLevelSelectUI, call);
	}

	public void onCreditUI(UICallbackDelegate call)
	{
		onUI(creditUI, call);
	}

	public void onBoxStoreUI(UICallbackDelegate call)
	{
		onUI(boxStoreUI, call);
	}

	private void onUI(BaseUI baseUI, UICallbackDelegate call)
	{
		if (!(baseUI == null) && !baseUI.gameObject.activeSelf)
		{
			if (call != null)
			{
				baseUI.setDelegate(call);
			}
			baseUI.onStart();
		}
	}

	public void onPopupYesNo(string text, PopupDelegate yes = null, PopupDelegate no = null)
	{
		if (!(popupYesNo == null))
		{
			popupYesNo.onPopup(text, yes, no);
		}
	}

	public void onEquipmentSkillPopup()
	{
		equipmentSkillPopup.onPopup();
	}

	public void uiOpenCount()
	{
		gameViewTouchIndex++;
		gameSceneTouchEnable(state: false);
	}

	public void uiCloseCount()
	{
		gameViewTouchIndex--;
		if (gameViewTouchIndex <= 0)
		{
			gameViewTouchIndex = 0;
			gameSceneTouchEnable(state: true);
		}
	}

	public void gameSceneTouchEnable(bool state)
	{
		gameSceneComponent.getTouchEvent().setEnabled(state);
		if (villageSceneComponent != null)
		{
			villageSceneComponent.getTouchEvent().setEnabled(state);
		}
	}
}
