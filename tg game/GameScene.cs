using CodeStage.AntiCheat.ObscuredTypes;
using DG.Tweening;
using Percent;
using Percent.Event;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GameScene : MonoBehaviour
{
	private ObscuredInt stage = 0;

	private ObscuredInt level = 0;

	[Header("INDEXS")]
	public float startWidthSize;

	[HideInInspector]
	public float maxWidthSize;

	public float startHeight;

	public float playerX;

	[Header("OBJECTS")]
	public GameObject gameBackground;

	private GameBackground gameBackgroundComponent;

	public GameObject player;

	private Player playerComponent;

	public GameObject petObject;

	private PetObject petComponent;

	public GameObject mainCamera;

	private CameraSetting cameraComponent;

	public GameObject storeFlag;

	private StoreFlag storeFlagComponent;

	public GameObject portalAnimation;

	private PortalChangeAnimation portalAnimationComponent;

	public GameObject sceneChangeParticle;

	private ParticleSystem sceneChangeParticleSystem;

	private PortalParticleColor sceneChangeComponet;

	[Header("MANAGERS")]
	private DataManager dataManager;

	private AppCustomEventManager appCustomEventManager;

	private SoundManager soundManager;

	private PlayerManager playerManager;

	private EnemyManager enemyManager;

	private DamageManager damageManager;

	private StoreManager storeManager;

	private UIControlManager uiControlManager;

	private CoinManager coinManager;

	private ItemManager itemManager;

	private WorldParticleManager worldParticleManager;

	private TutorialManager tutorialManager;

	private TouchEvent touchEvent;

	private bool touchCancel;

	private bool touchEndedcallback;

	public List<Sprite> listEquipmentBgImages;

	public GameObject dropEquipment;

	public GameObject dropEquipmentBg;

	public GameObject deleteAction;

	private bool choiceDeleteEquipment;

	private EquipmentData equipmentDeleteData;

	private ObscuredInt bossKillCount = 0;

	private bool gameEnded;

	private bool gameCleared;

	private bool clearInterstitial;

	private bool cameraMoveVillage;

	private PercentTracker tracker;

	public void setCameraMoveVillage(bool state)
	{
		cameraMoveVillage = state;
	}

	public Player getPlayerComponent()
	{
		return playerComponent;
	}

	public TouchEvent getTouchEvent()
	{
		return touchEvent;
	}

	private void Start()
	{
		initAsync();
	}

	async Task initAsync() {
		if (tracker == null)
		{
			tracker = base.gameObject.AddComponent<PercentTracker>();
		}
		dataManager = Singleton<DataManager>.Instance;
		await dataManager.loadDataAsync();
		appCustomEventManager = Singleton<AppCustomEventManager>.Instance;
		appCustomEventManager.settingEvent(dataManager.logVersion);
		soundManager = Singleton<SoundManager>.Instance;
		soundManager.setSoundState(dataManager.soundState);
		soundManager.setBGMState(dataManager.bgmState);
		stage = Singleton<SceneManager>.Instance.getStage();
		level = Singleton<DataManager>.Instance.selectLevel;
		if ((int)stage == 0)
		{
			maxWidthSize = 110f;
			soundManager.playBGM("BGM_village");
		}
		else
		{
			if ((int)stage != 8)
			{
				soundManager.playBGM("BGM_game");
			}
			else
			{
				soundManager.stopBGM();
			}
			if ((int)stage == 7)
			{
				maxWidthSize = 320f;
			}
			else if ((int)stage == 8)
			{
				maxWidthSize = 90f;
			}
			else
			{
				maxWidthSize = 300f;
			}
			//GoogleAnalyticsV3.instance.LogEvent("GAME", "STAGE", "STAGE_START_" + stage.ToString(), 1L);
		}
		if ((int)dataManager.villageStartSize > 0)
		{
			startWidthSize = -60f;
		}
		initManagers();
		initObjects();
		initUIs();
		StartCoroutine(startPlayerAction());
		if (!Singleton<DataManager>.Instance.noAds)
		{
			Singleton<MopubCommunicator>.Instance.showBanner();
		}
		else
		{
			StartCoroutine(gameUIUP());
		}

		if (GameObject.Find("VillageScene")!=null) {
			GameObject.Find("VillageScene").GetComponent<VillageScene>().init();
			GameObject.Find("LoadingPanel").SetActive(false);
		}
	}


	private IEnumerator gameUIUP()
	{
		yield return new WaitForSeconds(0f);
		Singleton<UIControlManager>.Instance.upGameUI();
	}

	private IEnumerator startPlayerAction()
	{
		touchEvent.setEnabled(s: false);
		player.gameObject.SetActive(value: false);
		yield return new WaitForSeconds(1f);
		Vector3 position = player.transform.position;
		playerChangeParticle(new Vector3(position.x, position.y + 1f, -8f), Singleton<SceneManager>.Instance.getOldState(), state: true);
	}

	private void Update()
	{
		if (touchEvent == null)
			return;

		if (gameEnded)
		{
			Input.ResetInputAxes();
			return;
		}
		touchEvent.touchUpdate();
		objectMove();
		CollisionUpdate();
		storeUpdate();
	}

	private void objectMove()
	{
		if (!cameraMoveVillage)
		{
			cameraComponent.moveCamera(startWidthSize, maxWidthSize, player.transform.position);
		}
		gameBackgroundComponent.moveBackground(mainCamera.transform.position);
	}

	private void CollisionUpdate()
	{
		Vector3 position = player.transform.position;
		Enemy enemy = enemyManager.crashCheck(position, playerComponent.getOldPosition(), 1f);
		if (enemy != null)
		{
			damage(enemy);
		}
		Item item = itemManager.crashCheck(position);
		if (item != null)
		{
			getItem(item);
		}
	}

	private void damage(Enemy enemy)
	{
		playerManager.skillAttackCallback(enemy);
		playerManager.addDamage(enemy.getPower());
		float gameUIHP = (float)playerManager.getPlayerNowHP() / (float)playerManager.getPlayerMaxHP();
		uiControlManager.setGameUIHP(gameUIHP);
		if (!playerManager.isLife())
		{
			onGameOver();
			return;
		}
		int num = playerManager.getPower();
		Color color = Color.white;
		float scale = 1f;
		if (playerManager.getCricialActive())
		{
			num *= 2;
			color = new Color(1f, 0.376f, 0.156f, 1f);
			scale = 2f;
			soundManager.playSound("attackCritical");
		}
		else
		{
			soundManager.playSound("attack" + UnityEngine.Random.Range(1, 4).ToString());
		}
		Vector3 position = enemy.transform.position;
		damageManager.createActionDamage(num, color, position, scale);
		float lastMoveDirection = playerComponent.getLastMoveDirection();
		Vector3 position2 = enemy.transform.position;
		position2.x += 0.5f * lastMoveDirection;
		position2.y += 0.7f;
		position2.z = -5f;
		worldParticleManager.createAttackParticle(position2, dataManager.selectWeapon);
		if (!enemy.addDamage(num, lastMoveDirection))
		{
			playerComponent.knockBack();
		}
		else
		{
			enemyDie(enemy);
		}
	}

	public void enemyDie(Enemy enemy)
	{
		dropEnemy(enemy);
		if (enemy.GetEnemyType() == EnemyType.TYPE_BOSS)
		{
			++bossKillCount;
			if (enemyManager.checkStageClear(bossKillCount))
			{
				onGameClear();
			}
		}
	}

	public void dropEnemy(Enemy enemy)
	{
		Vector3 position = enemy.gameObject.transform.position;
		float lastMoveDirection = playerComponent.getLastMoveDirection();
		int levelByDropCoin = Balance.getLevelByDropCoin(stage, level);
		coinManager.onCoins(position, (int)lastMoveDirection, levelByDropCoin);
		DataManager obj = dataManager;
		obj.coinCount = (int)obj.coinCount + levelByDropCoin;
		obj.maxCoinCount = (int)obj.maxCoinCount + levelByDropCoin;
		Singleton<UIControlManager>.Instance.setCoinUI(dataManager.coinCount);
		if (Balance.getDropItem(0f))
		{
			itemManager.createRandomItem(position, lastMoveDirection);
		}
		bool flag = enemy.GetEnemyType() == EnemyType.TYPE_BOSS;
		if (flag)
		{


			int num = 3;
			if ((int)stage == 8)
			{
				num = 1;
			}
			/*stage = 10;
			level = 10;*/
			for (int i = 0; i < num * (int)level; i++)
			{
				if (Balance.getDropEquipment(stage, level, flag))
				{
					itemManager.createEquipment(position, flag ? (-0.5f) : lastMoveDirection, Balance.getRandomEquipmentData(stage, level, flag));
				}
			}
		}
		else if (tutorialManager.getGameTutorialState())
		{
			if (tutorialManager.createTutorialEquipment())
			{
				appCustomEventManager.pushEvent("tutorial", "All");
				dataManager.tutorialGame = true;
				dataManager.saveDataAsync();
			}
			else
			{
				EquipmentData data = new EquipmentData(EquipmentType.TYPE_WEAPON, EquipmentRank.TYPE_NORMAL, EquipmentGrade.TYPE_D, 1, 0, 75, 2f, 0, 0, 0f, 0, 0f, 0, 0, 0f);
				itemManager.createEquipment(position, flag ? (-1f) : lastMoveDirection, data);
			}
		}
		else if (Balance.getDropEquipment(stage, level, flag))
		{
			EquipmentData randomEquipmentData = Balance.getRandomEquipmentData(stage, level, flag);
			itemManager.createEquipment(position, flag ? (-1f) : lastMoveDirection, randomEquipmentData);
		}
		if (Balance.getDropEgg())
		{
			itemManager.createEgg(position, lastMoveDirection);
		}
	}

	private void getItem(Item item)
	{
		if (item.getItemState() == ItemState.TYPE_EQUIPMENT)
		{
			if (!dataManager.getEquipment(item.getItemEquipmentData()))
			{
				return;
			}
			if (item.getItemEquipmentData().rank == EquipmentRank.TYPE_UNIQUE && !dataManager.firstUnique)
			{
				appCustomEventManager.pushEvent("getPurple");
				dataManager.firstUnique = true;
				dataManager.saveDataAsync();
			}
			soundManager.playSound("equipmentGet");
		}
		else
		{
			switch (item.getItemState())
			{
			case ItemState.TYPE_ITEM_ATTACK:
				playerManager.setActivePower(state: true);
				playerComponent.onActivePowerUp(delegate
				{
					playerManager.setActivePower(state: false);
					playerComponent.offActivePowerUp();
					refreshGameUI();
				}, 4f);
				break;
			case ItemState.TYPE_ITEM_SHIELD:
				playerManager.setActiveShield(state: true);
				playerComponent.onActiveShieldUp(delegate
				{
					playerManager.setActiveShield(state: false);
					playerComponent.offActiveShieldUp();
					refreshGameUI();
				}, 4f);
				break;
			case ItemState.TYPE_ITEM_SPEED:
				playerManager.setActiveSpeed(state: true);
				playerComponent.settingSpeed(playerManager.getSpeed());
				playerComponent.onActiveSpeedUp(delegate
				{
					playerManager.setActiveSpeed(state: false);
					playerComponent.settingSpeed(playerManager.getSpeed());
					playerComponent.offActiveSpeedUp();
					refreshGameUI();
				}, 4f);
				break;
			case ItemState.TYPE_EGG:
				++dataManager.eggCount;
				uiControlManager.setEggUI(dataManager.eggCount);
				break;
			}
			soundManager.playSound("getItem");
		}
		refreshGameUI();
		itemManager.removeItem(item);
	}

	private void storeUpdate()
	{
		Transform transform = storeManager.crashObject(player.transform.position);
		if (transform != null && !storeFlagComponent.flagOpenState())
		{
			storeFlagComponent.onOpen(transform);
			playerManager.settingNowHP();
			uiControlManager.setGameUIHP(1f);
			playerComponent.onParticleHealing();
			Singleton<SoundManager>.Instance.playSound("ingameHeal");
			if (!gameCleared)
			{
				enemyManager.resetEnemyState(storeManager.getCrashStoreIndex(), (int)stage == 7);
			}
			if (tutorialManager.onFirstInventoryFlag())
			{
				onTouchEnded(null, null);
			}
			if ((int)stage > (int)dataManager.clearStage)
			{
				int crashStoreIndex = storeManager.getCrashStoreIndex();
				if (crashStoreIndex > (int)dataManager.clearSection)
				{
					dataManager.clearSection = crashStoreIndex;
				}
			}
		}
		else if (transform == null && storeFlagComponent.flagOpenState())
		{
			storeFlagComponent.onClose();
		}
	}

	public Enemy initKnightEnemy()
	{
		return enemyManager.createKnightEnemy(level, maxWidthSize, startHeight);
	}

	private void initManagers()
	{
		enemyManager = Singleton<EnemyManager>.Instance;
		enemyManager.initObjects(stage, level, maxWidthSize, startHeight);
		playerManager = Singleton<PlayerManager>.Instance;
		refreshPlayerDataFile(player.GetComponent<Player>());
		damageManager = Singleton<DamageManager>.Instance;
		damageManager.initObjects();
		storeManager = Singleton<StoreManager>.Instance;
		storeManager.initObjects(stage, enemyManager.getListStorePositions());
		uiControlManager = Singleton<UIControlManager>.Instance;
		coinManager = Singleton<CoinManager>.Instance;
		coinManager.initObjects();
		itemManager = Singleton<ItemManager>.Instance;
		itemManager.initObjects();
		worldParticleManager = Singleton<WorldParticleManager>.Instance;
		worldParticleManager.initObjects();
		tutorialManager = Singleton<TutorialManager>.Instance;
		if ((int)stage == 0)
		{
			tutorialManager.initVillageTutorial();
			tutorialManager.onStartTouch();
		}
		else
		{
			tutorialManager.initGameTutorial();
		}
	}

	private void initObjects()
	{
		if (portalAnimation != null)
		{
			portalAnimationComponent = portalAnimation.GetComponent<PortalChangeAnimation>();
			portalAnimationComponent.setEndedCallback(delegate
			{
				portalAnimation.SetActive(value: false);
			});
			portalAnimationComponent.offPortal();
		}
		touchEvent = new TouchEvent();
		touchEvent.setTouchBegan(onTouchBegan);
		touchEvent.setTouchMoved(onTouchMoved);
		touchEvent.setTouchEnded(onTouchEnded);
		cameraComponent = mainCamera.GetComponent<CameraSetting>();
		cameraComponent.setPlayerXpos(playerX, startWidthSize);
		gameBackgroundComponent = gameBackground.GetComponent<GameBackground>();
		gameBackgroundComponent.settingBackgroundImages(stage, this, cameraComponent);
		gameBackgroundComponent.createBackground(startWidthSize, maxWidthSize, startHeight, mainCamera.transform);
		gameBackgroundComponent.createBackgroundSub(cameraComponent.getCameraWidth(), cameraComponent.getCameraHeight(), mainCamera.transform);
		playerComponent = player.GetComponent<Player>();
		petComponent = petObject.GetComponent<PetObject>();
		player.transform.position = new Vector3(mainCamera.transform.position.x - playerX, startHeight, 0f);
		refreshPlayer();
		playerComponent.settingMoveSize(startWidthSize, maxWidthSize);
		storeFlagComponent = storeFlag.GetComponent<StoreFlag>();
		sceneChangeParticleSystem = sceneChangeParticle.GetComponent<ParticleSystem>();
		sceneChangeComponet = sceneChangeParticle.GetComponent<PortalParticleColor>();
	}

	private void initUIs()
	{
		uiControlManager.initGameUI();
		uiControlManager.setCoinUI(dataManager.coinCount);
		uiControlManager.setEggUI(dataManager.eggCount);
		refreshGameUI();
	}

	public void refreshPlayerDataFile(Player p)
	{
		playerManager.initObjects(dataManager.selectHelmet, dataManager.selectArmor, dataManager.selectWeapon, dataManager.selectHorse, dataManager.selectPet, p, this);
		playerManager.settingNowHP();
	}

	public void refreshPlayer()
	{
		playerComponent.settingImage(playerManager.getHelmetData().imageIndex, playerManager.getArmorData().imageIndex, playerManager.getWeaponData().imageIndex, playerManager.getHorseData().imageIndex);
		playerComponent.settingSpeed(playerManager.getSpeed());
		petComponent.settingPetObject(dataManager.selectPet, player);
	}

	public void refreshGameUI()
	{
		uiControlManager.refreshGameUIEquipments(playerManager.getHelmetData(), playerManager.getArmorData(), playerManager.getWeaponData(), playerManager.getHorseData());
		uiControlManager.refreshGameUIStatus(playerManager.getPower(), playerManager.getCritical(), playerManager.getShield(), playerManager.getSpeed(), playerManager.getPlayerMaxHP());
	}

	private void onTouchBegan(RaycastHit2D[] hits, TouchPosition touch)
	{
		touchCancel = false;
		touchEndedcallback = false;
		if (storeFlagComponent.flagOpenState())
		{
			Vector3 a = Camera.main.ScreenToWorldPoint(touch.position);
			Vector3 position = storeFlag.transform.position;
			a.z = 0f;
			position.z = 0f;
			if (Vector3.Distance(a, position) < 1.2f)
			{
				storeFlagComponent.onClick(delegate
				{
					refreshPlayerData();
				});
				soundManager.playSound("uiFlagClick");
				touchCancel = true;
				tutorialManager.onInventoryOpen();
			}
		}
	}

	private void onTouchMoved(RaycastHit2D[] hits, TouchPosition touch)
	{
		if (!touchCancel && !tutorialManager.touchBlack)
		{
			float num = (!(Camera.main.ScreenToViewportPoint(touch.position).x < 0.5f)) ? 1 : (-1);
			playerComponent.move(num);
			cameraComponent.setMoveState(num);
			tutorialManager.onStartTouchEnd();
		}
	}

	private void onTouchEnded(RaycastHit2D[] hits, TouchPosition touch)
	{
		touchEndedcallback = true;
		if (!touchCancel)
		{
			cameraComponent.stopCameraMove();
			playerComponent.stop();
		}
	}

	public void setTouchLock()
	{
		touchCancel = true;
	}

	public void onGameOver()
	{
		//GoogleAnalyticsV3.instance.LogEvent("GAME", "STAGE", "STAGE_FAIL_" + stage.ToString(), 1L);
		gameEnded = true;
		playerComponent.onDieAnimation();
		cameraComponent.onDieAnimation();
		soundManager.playSound("die");
		Time.timeScale = 0.2f;
		List<int> list = new List<int>();
		if (dataManager.listArmors.Count > 1)
		{
			list.Add(1);
		}
		if (dataManager.listWeapons.Count > 1)
		{
			list.Add(2);
		}
		if (dataManager.listHelmets.Count > 1)
		{
			list.Add(3);
		}
		if (dataManager.listHorses.Count > 1)
		{
			list.Add(4);
		}
		if (list.Count > 0)
		{
			int index = UnityEngine.Random.Range(0, list.Count);
			int num = list[index];
			List<EquipmentData> list2 = new List<EquipmentData>();
			EquipmentData equipmentData = dataManager.selectArmor;
			switch (num)
			{
			case 1:
				list2 = dataManager.listArmors;
				equipmentData = dataManager.selectArmor;
				break;
			case 2:
				list2 = dataManager.listWeapons;
				equipmentData = dataManager.selectWeapon;
				break;
			case 3:
				list2 = dataManager.listHelmets;
				equipmentData = dataManager.selectHelmet;
				break;
			case 4:
				list2 = dataManager.listHorses;
				equipmentData = dataManager.selectHorse;
				break;
			}
			int num2 = UnityEngine.Random.Range(0, list2.Count);
			for (int i = 0; i < list2.Count; i++)
			{
				if ((int)equipmentData.objectIndex != (int)list2[num2].objectIndex && (int)list2[num2].imageIndex != 10000)
				{
					break;
				}
				num2++;
				if (num2 >= list2.Count)
				{
					num2 = 0;
				}
			}
			if ((int)equipmentData.objectIndex != (int)list2[num2].objectIndex && (int)list2[num2].imageIndex != 10000)
			{
				equipmentDeleteData = list2[num2];
				choiceDeleteEquipment = true;
				list2.RemoveAt(num2);
				switch (num)
				{
				case 1:
					dataManager.listArmors = list2;
					break;
				case 2:
					dataManager.listWeapons = list2;
					break;
				case 3:
					dataManager.listHelmets = list2;
					break;
				case 4:
					dataManager.listHorses = list2;
					break;
				}
			}
		}
		dataManager.saveDataAsync();
		StartCoroutine(gameEndedNext());
	}

	private IEnumerator gameEndedNext()
	{
		yield return new WaitForSeconds(0.4f);
		if (choiceDeleteEquipment)
		{
			dropEquipmentBg.SetActive(value: true);
			SpriteRenderer component = dropEquipmentBg.GetComponent<SpriteRenderer>();
			SpriteRenderer component2 = dropEquipment.GetComponent<SpriteRenderer>();
			component.sprite = listEquipmentBgImages[(int)equipmentDeleteData.rank];
			float num = 1f;
			string text = "";
			switch (equipmentDeleteData.type)
			{
			case EquipmentType.TYPE_ARMOR:
				text = "Armor";
				break;
			case EquipmentType.TYPE_WEAPON:
				text = "Weapon";
				num = 0.6f;
				break;
			case EquipmentType.TYPE_HELMET:
				text = "Helmet";
				break;
			case EquipmentType.TYPE_HORSE:
				text = "Horse";
				break;
			}
			Sprite sprite2 = component2.sprite = Singleton<AssetManager>.Instance.LoadSprite("Equipment/" + text + "/" + equipmentDeleteData.imageIndex);
			float pixelsPerUnit = sprite2.pixelsPerUnit;
			float num2 = sprite2.pivot.x / sprite2.rect.size.x - 0.5f;
			float num3 = sprite2.pivot.y / sprite2.rect.size.y - 0.5f;
			float x = sprite2.rect.size.x * (num2 * num) / pixelsPerUnit;
			float y = sprite2.rect.size.y * (num3 * num) / pixelsPerUnit;
			Vector3 localPosition = new Vector3(x, y, 0f);
			dropEquipment.transform.localPosition = localPosition;
			dropEquipment.transform.localScale = new Vector3(num, num, num);
			Vector3 bodyPosition = playerComponent.getBodyPosition();
			bodyPosition.z = -8f;
			dropEquipmentBg.transform.position = bodyPosition;
			dropEquipmentBg.transform.DOMoveY(1.5f, 0.2f).SetRelative(isRelative: true).SetEase(Ease.OutExpo);
			component.DOFade(0f, 0.1f).SetDelay(0.3f);
			component2.DOFade(0f, 0.1f).SetDelay(0.3f);
			deleteAction.SetActive(value: true);
			deleteAction.transform.DOScale(1f, 0.05f).SetDelay(0.2f).SetEase(Ease.OutBack);
			deleteAction.GetComponent<SpriteRenderer>().DOFade(0f, 0.1f).SetDelay(0.3f);
			yield return new WaitForSeconds(0.2f);
			soundManager.playSound("dieItemBreak");
		}
		yield return new WaitForSeconds(0.2f + (choiceDeleteEquipment ? 0.2f : 0f));
		Vector3 position = player.transform.position;
		playerChangeParticle(new Vector3(playerComponent.getBodyPosition().x, position.y + 1f, -8f), Singleton<SceneManager>.Instance.getOldState(), state: false, 5f);
		yield return new WaitForSeconds(0.1f);
		if (portalAnimation != null)
		{
			portalAnimation.SetActive(value: true);
			portalAnimationComponent.setAnimationTime(0.1f);
			portalAnimationComponent.onPortal();
			portalAnimationComponent.setEndedCallback(delegate
			{
				Time.timeScale = 1f;
				Singleton<SceneManager>.Instance.changeScene(0);
				if (CrossPromotion.isInterstitialShowable() && (int)dataManager.clearStage >= 1)
				{
					if (!dataManager.noAds)
					{
						Singleton<MopubCommunicator>.Instance.showInterstitial(delegate
						{
							tracker.triggerSeeAdsInterstitial();
							Singleton<AppCustomEventManager>.Instance.pushEvent("watchIS", "interstitial");
							DataManager obj = dataManager;
							obj.interstitialWatchCount = (int)obj.interstitialWatchCount + 1;
						});
					}
					CrossPromotion.reportShowInterstitial();
				}
			});
		}
	}

	private void onGameClear()
	{
		gameCleared = true;
		if ((int)dataManager.clearStage >= 1)
		{
			clearInterstitial = true;
		}
		if ((int)dataManager.clearStage < (int)stage)
		{
			dataManager.clearStage = stage;
			dataManager.clearLevelAndStage = level + "-" + stage;
			dataManager.clearSection = 0;
			//GoogleAnalyticsV3.instance.LogEvent("GAME", "STAGE", "STAGE_CLEAR_FIRST_" + stage.ToString(), 1L);
			if ((int)stage == 7)
			{
				dataManager.villageStartSize = 2;
			}
		}
		if ((int)stage >= 7)
		{
			dataManager.registerClearEquipment();
		}
		dataManager.saveDataAsync();
		StartCoroutine(gameClearAction());
		//GoogleAnalyticsV3.instance.LogEvent("GAME", "STAGE", "STAGE_CLEAR_" + stage.ToString(), 1L);
		soundManager.stopBGMFade();
	}

	private IEnumerator gameClearAction()
	{
		Time.timeScale = 0.05f;
		float timeCount = 0f;
		do
		{
			yield return null;
			timeCount += Time.deltaTime;
			playerComponent.move(1f);
			cameraComponent.setMoveState(1f);
			touchCancel = true;
		}
		while (!(timeCount >= 0.1f));
		touchCancel = false;
		if (touchEndedcallback)
		{
			cameraComponent.stopCameraMove();
			playerComponent.stop();
		}
		Time.timeScale = 1f;
		uiControlManager.startGameClearUI(delegate
		{
			refreshPlayerData();
		}, delegate
		{
			StartCoroutine(gameClearOut());
		});
	}

	private IEnumerator gameClearOut()
	{
		dataManager.saveDataAsync();
		gameEnded = true;
		playerComponent.stop();
		Vector3 position = player.transform.position;
		playerChangeParticle(new Vector3(position.x, position.y + 1f, -8f), Singleton<SceneManager>.Instance.getOldState(), state: false);
		yield return new WaitForSeconds(0.5f);
		portalAnimation.SetActive(value: true);
		portalAnimationComponent.onPortal();
		portalAnimationComponent.setEndedCallback(delegate
		{
			Time.timeScale = 1f;
			Singleton<SceneManager>.Instance.changeScene(0);
			if (CrossPromotion.isInterstitialShowable() && clearInterstitial)
			{
				if (!dataManager.noAds)
				{
					Singleton<MopubCommunicator>.Instance.showInterstitial(delegate
					{
						tracker.triggerSeeAdsInterstitial();
						Singleton<AppCustomEventManager>.Instance.pushEvent("watchIS", "interstitial");
						DataManager obj = dataManager;
						obj.interstitialWatchCount = (int)obj.interstitialWatchCount + 1;
					});
				}
				CrossPromotion.reportShowInterstitial();
			}
		});
	}

	private void refreshPlayerData()
	{
		refreshPlayerDataFile(playerComponent);
		refreshPlayer();
		refreshGameUI();
	}

	public void playerChangeParticle(Vector3 position, int index, bool state, float speedPer = 1f)
	{
		StartCoroutine(particleUpdate(position, index, state, speedPer));
	}

	private IEnumerator particleUpdate(Vector3 position, int index, bool state, float speedPer = 1f)
	{
		new Color(1f, 1f, 1f, 1f);
		sceneChangeParticle.SetActive(value: true);
		sceneChangeParticle.transform.position = position;
		var main = sceneChangeParticleSystem.main;
		main.simulationSpeed = 1f * speedPer;
		sceneChangeParticleSystem.Play();
		soundManager.playSound("sceneChange");
		touchEvent.setEnabled(!state);
		yield return new WaitForSeconds(0.05f);
		player.gameObject.SetActive(state);
		touchEvent.setEnabled(state);
	}

	private void OnApplicationQuit()
	{
		//UCODESHOP TODO
		//GoogleAnalyticsV3.instance.StopSession();
	}
}
