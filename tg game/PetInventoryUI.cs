using CodeStage.AntiCheat.ObscuredTypes;
using DG.Tweening;
using Percent.Event;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PetInventoryUI : BaseUI
{
	[Header("========== DATA FILES")]
	[Header("BOX IMAGE FILES")]
	public Sprite boxNormal;

	public Sprite boxUnique;

	public Sprite boxLegendary;

	public Sprite boxSuperLegendary;

	[Header("EGG IMAGE FILES")]
	public List<Sprite> listEggSprites = new List<Sprite>();

	public List<Sprite> listEggBreakSprite = new List<Sprite>();

	[Header("PET SKILL IMAGES")]
	public List<Sprite> listSkillSprites = new List<Sprite>();

	public List<PetSkillUI> listPetSkillUIs = new List<PetSkillUI>();

	[Header("PET SKILL FONTS")]
	public Color fontRed;

	public Color fontOrange;

	public Color fontGreen;

	public Color fontBlue;

	public Color fontPink;

	public Color fontBlackWhite;

	[Header("PET SKILL MATERIAL")]
	public Material materialBlackWhite;

	[Header("========== OBJECTS")]
	[Header("BOX OBJECTS")]
	public List<Image> listBoxs = new List<Image>();

	public List<Image> listBoxIcons = new List<Image>();

	public List<TextMeshProUGUI> listLevelText = new List<TextMeshProUGUI>();

	public List<GameObject> listLevelBackground = new List<GameObject>();

	[Header("SELECT BUTTONS")]
	public Button buttonSale;

	public Button buttonSelect;

	public Button buttonUpgrade;

	public TextMeshProUGUI textSale;

	public TextMeshProUGUI textUpgrade;

	[Header("PLAYER OBJECT")]
	public GameObject petObject;

	private PetObject petComponent;

	[Header("PLAYER STATUS")]
	public Color fontOnlyGreen;

	public Color fontOnlyRed;

	public TextMeshProUGUI textPower;

	public TextMeshProUGUI textCritical;

	public TextMeshProUGUI textShield;

	public TextMeshProUGUI textSpeed;

	public TextMeshProUGUI textHP;

	public TextMeshProUGUI textAddPower;

	public TextMeshProUGUI textAddCritical;

	public TextMeshProUGUI textAddShield;

	public TextMeshProUGUI textAddSpeed;

	public TextMeshProUGUI textAddHP;

	[Header("EGG BUTTONS")]
	public Button buttonHemerAd;

	[Header("EGG TEXTS")]
	public TextMeshProUGUI textEggCount;

	[Header("EGG BRAKE OBJECTS")]
	public GameObject eggPanel;

	public Image eggImage;

	public Image smallHemer;

	public Image eggBreakObject;

	public Image eggBreakEndObject;

	public Image eggBreakCircle;

	public Image getPetImage;

	public GameObject getPanel;

	public GameObject petLight;

	private PetLight petLightComponent;

	public GameObject selectImage;

	public GameObject nowSelectImage;

	[Header("EGG REWARD OBJECTS")]
	public List<GameObject> listEggs = new List<GameObject>();

	[Header("BUTTON OBJECTS")]
	public List<Button> listButtons = new List<Button>();

	[Header("UPGRADE ACTION OBJECTS")]
	public Material spriteDefaultMaterial;

	public Material spriteWhiteMaterial;

	public Image upgradePanel;

	public Image upgradeEndedPanel;

	public TextMeshProUGUI upgradetext;

	[Header("BACKGROUND OBJECTS")]
	public RectTransform backgroundTransform;

	public Image panel;

	[Header("SELL OBJECTS")]
	public GameObject sellPanel;

	public List<GameObject> listSellSelects = new List<GameObject>();

	private List<int> listSellSelectIndexs = new List<int>();

	private DataManager dataManager;

	private AppCustomEventManager appCustomEventManager;

	private PlayerManager playerManager;

	private SoundManager soundManager;

	private List<PetData> listInventoryData = new List<PetData>();

	private int selectIndex = -1;

	private bool eggBreakActionEnded;

	private PercentTracker tracker;

	private void inventoryListSetting()
	{
		listInventoryData = dataManager.listPets;
	}

	private void refreshinventoryList()
	{
		int count = listInventoryData.Count;
		int count2 = listBoxs.Count;
		bool active = false;
		for (int i = 0; i < count2; i++)
		{
			if (i >= count)
			{
				listBoxs[i].sprite = boxNormal;
				listBoxs[i].gameObject.GetComponent<Button>().enabled = false;
				listBoxs[i].SetNativeSize();
				listBoxIcons[i].sprite = null;
				listBoxIcons[i].transform.localScale = new Vector3(0f, 0f, 1f);
				listLevelText[i].text = "";
				listLevelBackground[i].SetActive(value: false);
				continue;
			}
			switch (listInventoryData[i].rank)
			{
			case EquipmentRank.TYPE_NORMAL:
				listBoxs[i].sprite = boxNormal;
				break;
			case EquipmentRank.TYPE_UNIQUE:
				listBoxs[i].sprite = boxUnique;
				break;
			case EquipmentRank.TYPE_LEGENDARY:
				listBoxs[i].sprite = boxLegendary;
				break;
			case EquipmentRank.TYPE_SUPERLEGENDARY:
				listBoxs[i].sprite = boxSuperLegendary;
				break;
			}
			listBoxs[i].gameObject.GetComponent<Button>().enabled = true;
			listBoxs[i].SetNativeSize();
			Image image = listBoxIcons[i];
			AssetManager instance = Singleton<AssetManager>.Instance;
			PetData petData = listInventoryData[i];
			image.sprite = instance.LoadSprite("Pets/" + petData.imageIndex.ToString());
			listBoxIcons[i].SetNativeSize();
			Rect rect = listBoxIcons[i].sprite.rect;
			float num = (rect.width > rect.height) ? rect.width : rect.height;
			float num2 = 150f / num * 0.6f;
			listBoxIcons[i].transform.localScale = new Vector3(num2, num2, 1f);
			if ((int)listInventoryData[i].level > 0)
			{
				TextMeshProUGUI textMeshProUGUI = listLevelText[i];
				petData = listInventoryData[i];
				textMeshProUGUI.text = "+" + petData.level.ToString();
				listLevelBackground[i].SetActive(value: true);
			}
			else
			{
				listLevelText[i].text = "";
				listLevelBackground[i].SetActive(value: false);
			}
			if ((int)listInventoryData[i].objectIndex == (int)dataManager.selectPet.objectIndex)
			{
				active = true;
				selectImage.transform.localPosition = listBoxs[i].transform.localPosition + new Vector3(0f, 50f, 0f);
				listBoxs[i].color = new Color(0.5f, 0.5f, 0.5f, 1f);
			}
			else
			{
				listBoxs[i].color = Color.white;
			}
		}
		selectImage.SetActive(active);
	}

	private void activeInventoryChange()
	{
		dataManager.listPets = listInventoryData;
	}

	private void onSelectStatus()
	{
		nowSelectImage.SetActive(value: true);
		nowSelectImage.transform.localPosition = listBoxs[selectIndex].transform.localPosition;
		PetData petData = listInventoryData[selectIndex];
		List<float> list = new List<float>
		{
			petData.powerPer,
			petData.criticalPer,
			petData.shieldPer,
			petData.speedPer,
			petData.hpPer
		};
		int count = listPetSkillUIs.Count;
		for (int i = 0; i < count; i++)
		{
			listPetSkillUIs[i].icon.gameObject.SetActive(value: true);
			listPetSkillUIs[i].text.gameObject.SetActive(value: true);
			if (list[i] > 0.1f)
			{
				listPetSkillUIs[i].icon.material = null;
				Color color = default(Color);
				switch (i)
				{
				case 0:
					color = fontRed;
					break;
				case 1:
					color = fontOrange;
					break;
				case 2:
					color = fontGreen;
					break;
				case 3:
					color = fontBlue;
					break;
				case 4:
					color = fontPink;
					break;
				}
				listPetSkillUIs[i].text.color = color;
				listPetSkillUIs[i].text.text = list[i].ToString("0.0") + "%";
			}
			else
			{
				listPetSkillUIs[i].icon.material = materialBlackWhite;
				listPetSkillUIs[i].text.color = fontBlackWhite;
				listPetSkillUIs[i].text.text = "0";
			}
		}
		petComponent.settingPetObject(petData, null, inventory: true);
	}

	private void offSelectStatus()
	{
		nowSelectImage.SetActive(value: false);
		int count = listPetSkillUIs.Count;
		for (int i = 0; i < count; i++)
		{
			listPetSkillUIs[i].text.gameObject.SetActive(value: false);
			listPetSkillUIs[i].icon.gameObject.SetActive(value: false);
		}
		petComponent.settingPetObject(new PetData(EquipmentRank.TYPE_NORMAL, 0, 0), null, inventory: true);
	}

	private void onSelectButtons()
	{
		buttonSale.gameObject.SetActive(value: true);
		buttonSelect.gameObject.SetActive(value: true);
		buttonUpgrade.gameObject.SetActive(value: true);
		PetData petData = listInventoryData[selectIndex];
		textSale.text = "SELL";
		textUpgrade.text = "UPGRADE " + Price.getUpgradePetEquipment(petData.rank, petData.level).ToString();
	}

	private void offSelectButtons()
	{
		buttonSale.gameObject.SetActive(value: false);
		buttonSelect.gameObject.SetActive(value: false);
		buttonUpgrade.gameObject.SetActive(value: false);
	}

	private void activeSelectEquipment()
	{
		playerManager.refreshPet(listInventoryData[selectIndex]);
		dataManager.selectPet = listInventoryData[selectIndex];
	}

	private void refreshEggImage()
	{
		eggImage.sprite = listEggSprites[dataManager.eggImageIndex];
		if ((int)dataManager.eggCount <= 0)
		{
			eggImage.gameObject.SetActive(value: false);
			return;
		}
		eggImage.gameObject.SetActive(value: true);
		eggImage.transform.localScale = new Vector3(0f, 0f, 0f);
		eggImage.transform.DOScale(new Vector3(1f, 1f, 1f), 0.5f).SetEase(Ease.OutBack);
	}

	private void actionEggAdd()
	{
		int count = listEggs.Count;
		for (int i = 0; i < count; i++)
		{
			listEggs[i].transform.localPosition = new Vector3(Random.Range(-431f, 431f), -1100f);
			listEggs[i].transform.DOLocalMoveY(1100f, 1f).SetEase(Ease.InExpo).SetDelay(Random.Range(0f, 0.5f));
		}
	}

	private void refreshPlayerStatus()
	{
		textAddPower.gameObject.SetActive(value: false);
		textAddCritical.gameObject.SetActive(value: false);
		textAddShield.gameObject.SetActive(value: false);
		textAddSpeed.gameObject.SetActive(value: false);
		textAddHP.gameObject.SetActive(value: false);
		textPower.text = playerManager.getPower().ToString();
		textCritical.text = playerManager.getCritical().ToString("0.0") + "%";
		textShield.text = playerManager.getShield().ToString();
		textSpeed.text = playerManager.getSpeed().ToString("0.0");
		textHP.text = playerManager.getPlayerMaxHP().ToString();
	}

	private void selectPetStatus(PetData petData)
	{
		EquipmentData helmetData = playerManager.getHelmetData();
		EquipmentData armorData = playerManager.getArmorData();
		EquipmentData horseData = playerManager.getHorseData();
		EquipmentData weaponData = playerManager.getWeaponData();
		int num = (int)helmetData.power + (int)armorData.power + (int)horseData.power + (int)weaponData.power;
		float num2 = (float)helmetData.critical + (float)armorData.critical + (float)horseData.critical + (float)weaponData.critical;
		int num3 = (int)helmetData.shield + (int)armorData.shield + (int)horseData.shield + (int)weaponData.shield;
		float num4 = (float)helmetData.speed + (float)armorData.speed + (float)horseData.speed + (float)weaponData.speed;
		int num5 = (int)helmetData.hp + (int)armorData.hp + (int)horseData.hp + (int)weaponData.hp;
		num += (int)helmetData.addPower + (int)armorData.addPower + (int)horseData.addPower + (int)weaponData.addPower;
		num2 += (float)helmetData.addCritical + (float)armorData.addCritical + (float)horseData.addCritical + (float)weaponData.addCritical;
		num3 += (int)helmetData.addShield + (int)armorData.addShield + (int)horseData.addShield + (int)weaponData.addShield;
		num4 += (float)helmetData.addSpeed + (float)armorData.addSpeed + (float)horseData.addSpeed + (float)weaponData.addSpeed;
		num5 += (int)helmetData.addHp + (int)armorData.addHp + (int)horseData.addHp + (int)weaponData.addHp;
		if ((float)petData.powerPer >= 0.1f)
		{
			num += (int)((float)num * (float)petData.powerPer * 0.01f);
		}
		if ((float)petData.criticalPer >= 0.1f)
		{
			num2 += num2 * (float)petData.criticalPer * 0.01f;
		}
		if ((float)petData.shieldPer >= 0.1f)
		{
			num3 += (int)((float)num3 * (float)petData.shieldPer * 0.01f);
		}
		if ((float)petData.speedPer >= 0.1f)
		{
			num4 += num4 * (float)petData.speedPer * 0.01f;
		}
		if ((float)petData.hpPer >= 0.1f)
		{
			num5 += (int)((float)num5 * (float)petData.hpPer * 0.01f);
		}
		textPower.text = num.ToString();
		textCritical.text = num2.ToString("0.0") + "%";
		textShield.text = num3.ToString();
		textSpeed.text = num4.ToString("0.0");
		textHP.text = num5.ToString();
		int num6 = num - playerManager.getPower();
		float num7 = num2 - playerManager.getCritical();
		int num8 = num3 - playerManager.getShield();
		float num9 = num4 - playerManager.getSpeed();
		int num10 = num5 - playerManager.getPlayerMaxHP();
		if (num6 != 0)
		{
			textAddPower.gameObject.SetActive(value: true);
			textAddPower.text = ((num6 > 0) ? "+" : "-") + Mathf.Abs(num6).ToString();
			textAddPower.color = ((num6 > 0) ? fontOnlyGreen : fontOnlyRed);
		}
		else
		{
			textAddPower.gameObject.SetActive(value: false);
		}
		if (num7 <= -0.1f || 0.1f <= num7)
		{
			textAddCritical.gameObject.SetActive(value: true);
			textAddCritical.text = ((num7 > 0f) ? "+" : "-") + Mathf.Abs(num7).ToString("0.0") + "%";
			textAddCritical.color = ((num7 > 0f) ? fontOnlyGreen : fontOnlyRed);
		}
		else
		{
			textAddCritical.gameObject.SetActive(value: false);
		}
		if (num8 != 0)
		{
			textAddShield.gameObject.SetActive(value: true);
			textAddShield.text = ((num8 > 0) ? "+" : "-") + Mathf.Abs(num8).ToString();
			textAddShield.color = ((num8 > 0) ? fontOnlyGreen : fontOnlyRed);
		}
		else
		{
			textAddShield.gameObject.SetActive(value: false);
		}
		if (num9 <= -0.1f || 0.1f <= num9)
		{
			textAddSpeed.gameObject.SetActive(value: true);
			textAddSpeed.text = ((num9 > 0f) ? "+" : "-") + Mathf.Abs(num9).ToString("0.0");
			textAddSpeed.color = ((num9 > 0f) ? fontOnlyGreen : fontOnlyRed);
		}
		else
		{
			textAddSpeed.gameObject.SetActive(value: false);
		}
		if (num10 != 0)
		{
			textAddHP.gameObject.SetActive(value: true);
			textAddHP.text = ((num10 > 0) ? "+" : "-") + Mathf.Abs(num10).ToString();
			textAddHP.color = ((num10 > 0) ? fontOnlyGreen : fontOnlyRed);
		}
		else
		{
			textAddHP.gameObject.SetActive(value: false);
		}
	}

	public override void onStart()
	{
		base.onStart();
		if (tracker == null)
		{
			tracker = base.gameObject.AddComponent<PercentTracker>();
		}
		base.gameObject.SetActive(value: true);
		if (dataManager == null)
		{
			dataManager = Singleton<DataManager>.Instance;
			appCustomEventManager = Singleton<AppCustomEventManager>.Instance;
			playerManager = Singleton<PlayerManager>.Instance;
			soundManager = Singleton<SoundManager>.Instance;
		}
		if (petComponent == null)
		{
			petComponent = petObject.GetComponent<PetObject>();
		}
		if (petLightComponent == null)
		{
			petLightComponent = petLight.GetComponent<PetLight>();
		}
		textEggCount.text = "x" + dataManager.eggCount.ToString();
		inventoryListSetting();
		refreshinventoryList();
		refreshEggImage();
		refreshPlayerStatus();
		PetData selectPet = dataManager.selectPet;
		int count = listInventoryData.Count;
		for (int i = 0; i < count; i++)
		{
			if ((int)listInventoryData[i].objectIndex == (int)selectPet.objectIndex)
			{
				selectIndex = i;
				onSelectStatus();
				onSelectButtons();
				break;
			}
		}
		backgroundTransform.localScale = new Vector3(0f, 0f, 0f);
		backgroundTransform.DOScale(1f, 0.5f).SetEase(Ease.OutBack);
		panel.DOFade(0.6f, 0.5f);
		buttonHemerAd.gameObject.SetActive(false);
	}

	public override void onExit()
	{
		base.onExit();
		onDelegate();
		backgroundTransform.DOScale(0f, 0.5f).SetEase(Ease.InBack).OnComplete(delegate
		{
			base.gameObject.SetActive(value: false);
		});
		panel.DOFade(0f, 0.5f);
		Singleton<SoundManager>.Instance.playSound("uiClick");
	}

	public void onSale()
	{
		if (sellPanel.activeSelf)
		{
			if (listSellSelectIndexs.Count == 0)
			{
				textSale.text = "SELL";
				sellPanel.SetActive(value: false);
				listSellSelectIndexs.Clear();
				int count = listSellSelects.Count;
				for (int i = 0; i < count; i++)
				{
					listSellSelects[i].SetActive(value: false);
				}
			}
			else
			{
				Singleton<UIControlManager>.Instance.onPopupYesNo("Do you want to sell? (The items will be deleted.)", delegate
				{
					int count2 = listSellSelects.Count;
					for (int j = 0; j < count2; j++)
					{
						listSellSelects[j].SetActive(value: false);
					}
					sellPanel.SetActive(value: false);
					List<PetData> list = new List<PetData>();
					int count3 = listSellSelectIndexs.Count;
					for (int k = 0; k < count3; k++)
					{
						PetData petData = listInventoryData[listSellSelectIndexs[k]];
						if ((int)dataManager.selectPet.objectIndex != (int)petData.objectIndex && (int)petData.imageIndex != 21 && (int)petData.imageIndex != 22 && (int)petData.imageIndex != 23)
						{
							int salePetEquipment = Price.getSalePetEquipment(petData.rank, petData.level);
							list.Add(petData);
							DataManager obj = dataManager;
							obj.coinCount = (int)obj.coinCount + salePetEquipment;
							obj.maxCoinCount = (int)obj.maxCoinCount + salePetEquipment;
						}
					}
					int count4 = list.Count;
					for (int l = 0; l < count4; l++)
					{
						listInventoryData.Remove(list[l]);
					}
					selectIndex = -1;
					Singleton<UIControlManager>.Instance.setCoinUI(dataManager.coinCount);
					offSelectStatus();
					refreshinventoryList();
					activeInventoryChange();
					refreshPlayerStatus();
					activeInventoryChange();
					dataManager.saveDataAsync();
					textSale.text = "SELL";
					sellPanel.SetActive(value: false);
					listSellSelectIndexs.Clear();
				}, delegate
				{
				});
			}
		}
		else
		{
			textSale.text = "CANCEL";
			sellPanel.SetActive(value: true);
			listSellSelectIndexs.Clear();
		}
		Singleton<SoundManager>.Instance.playSound("uiClick");
	}

	public void onUpgrade()
	{
		Singleton<UIControlManager>.Instance.onPopupYesNo("Do you want to upgrade?", delegate
		{
			if (selectIndex != -1)
			{
				PetData petData = listInventoryData[selectIndex];
				int upgradePetEquipment = Price.getUpgradePetEquipment(petData.rank, petData.level);
				if ((int)dataManager.coinCount < upgradePetEquipment)
				{
					Singleton<UIControlManager>.Instance.onPopupYesNo("Not enough money!");
				}
				else
				{
					StartCoroutine(upgradeAction());
				}
			}
		}, delegate
		{
		});
		Singleton<SoundManager>.Instance.playSound("uiClick");
	}

	private IEnumerator upgradeAction()
	{
		SpriteRenderer selectPetImage = petComponent.getPetImage();
		upgradePanel.gameObject.SetActive(value: true);
		selectPetImage.material = spriteWhiteMaterial;
		float amountValue = 0f;
		while (true)
		{
			selectPetImage.material.SetFloat("_FlashAmount", amountValue);
			amountValue += Time.deltaTime * 1.5f;
			if (amountValue > 1f)
			{
				break;
			}
			yield return null;
		}
		selectPetImage.material.SetFloat("_FlashAmount", 1f);
		yield return new WaitForSeconds(0.5f);
		upgradePanel.DOFade(1f, 0.2f);
		yield return new WaitForSeconds(0.5f);
		upgradeActive();
		upgradePanel.DOKill();
		upgradePanel.DOFade(0f, 0.2f);
		selectPetImage.material = spriteDefaultMaterial;
		upgradeEndedPanel.gameObject.SetActive(value: true);
		upgradetext.gameObject.SetActive(value: true);
		upgradeEndedPanel.color = new Color(0f, 0f, 0f, 0.5f);
		upgradetext.color = new Color(1f, 1f, 1f, 0f);
		upgradetext.transform.localPosition = new Vector3(0f, 0f, 0f);
		upgradetext.transform.DOLocalMoveY(105f, 1f);
		upgradetext.DOFade(1f, 1f);
		yield return new WaitForSeconds(1.5f);
		upgradetext.transform.DOLocalMoveY(210f, 1f);
		upgradetext.DOFade(0f, 1f);
		upgradeEndedPanel.DOFade(0f, 1f);
		yield return new WaitForSeconds(1f);
		upgradeEndedPanel.gameObject.SetActive(value: false);
		upgradetext.gameObject.SetActive(value: false);
		upgradePanel.gameObject.SetActive(value: false);
	}

	public void upgradeActive()
	{
		if (selectIndex != -1)
		{
			PetData petData = listInventoryData[selectIndex];
			int upgradePetEquipment = Price.getUpgradePetEquipment(petData.rank, petData.level);
			++petData.level;
			if ((float)petData.powerPer > 0f)
			{
				ref ObscuredFloat powerPer = ref petData.powerPer;
				powerPer = (float)powerPer + Price.upgradePetPower(petData.rank, petData.level, petData.powerPer);
			}
			if ((float)petData.shieldPer > 0f)
			{
				ref ObscuredFloat shieldPer = ref petData.shieldPer;
				shieldPer = (float)shieldPer + Price.upgradePetShield(petData.rank, petData.level, petData.shieldPer);
			}
			if ((float)petData.hpPer > 0f)
			{
				ref ObscuredFloat hpPer = ref petData.hpPer;
				hpPer = (float)hpPer + Price.upgradePetHP(petData.rank, petData.level, petData.hpPer);
			}
			if ((float)petData.criticalPer > 0.1f)
			{
				ref ObscuredFloat criticalPer = ref petData.criticalPer;
				criticalPer = (float)criticalPer + Price.upgradePetCritical(petData.rank, petData.level, petData.criticalPer);
			}
			if ((float)petData.speedPer > 0.1f)
			{
				ref ObscuredFloat speedPer = ref petData.speedPer;
				speedPer = (float)speedPer + Price.upgradePetSpeed(petData.rank, petData.level, petData.speedPer);
			}
			listInventoryData[selectIndex] = petData;
			dataManager.selectPet = listInventoryData[selectIndex];
			DataManager obj = dataManager;
			obj.coinCount = (int)obj.coinCount - upgradePetEquipment;
			Singleton<UIControlManager>.Instance.setCoinUI(dataManager.coinCount);
			dataManager.saveDataAsync();
			onSelectStatus();
			onSelectButtons();
			refreshinventoryList();
			selectPetStatus(listInventoryData[selectIndex]);
			playerManager.refreshPet(listInventoryData[selectIndex]);
			activeInventoryChange();
		}
	}

	public void onSelect()
	{
		if (selectIndex != -1)
		{
			offSelectButtons();
			activeSelectEquipment();
			dataManager.saveDataAsync();
			selectIndex = -1;
			refreshinventoryList();
			refreshPlayerStatus();
			Singleton<SoundManager>.Instance.playSound("uiClick");
		}
	}

	public void onChoice(int select)
	{
		if (sellPanel.activeSelf)
		{
			if (!listSellSelects[select].activeSelf)
			{
				PetData petData = listInventoryData[select];
				if ((int)dataManager.selectPet.objectIndex == (int)petData.objectIndex)
				{
					Singleton<UIControlManager>.Instance.onPopupYesNo("You cannot sell items that are in use!");
					return;
				}
				if ((int)petData.imageIndex == 21 || (int)petData.imageIndex == 22 || (int)petData.imageIndex == 23)
				{
					Singleton<UIControlManager>.Instance.onPopupYesNo("The SUPER LEGENDARY Pat cannot be sell.");
					return;
				}
				listSellSelects[select].SetActive(value: true);
				listSellSelectIndexs.Add(select);
			}
			else
			{
				listSellSelects[select].SetActive(value: false);
				listSellSelectIndexs.Remove(select);
			}
			int num = 0;
			int count = listSellSelectIndexs.Count;
			for (int i = 0; i < count; i++)
			{
				PetData petData2 = listInventoryData[listSellSelectIndexs[i]];
				num += Price.getSalePetEquipment(petData2.rank, petData2.level);
			}
			if (num == 0)
			{
				textSale.text = "CANCEL";
			}
			else
			{
				textSale.text = "SELL " + num.ToString();
			}
		}
		else
		{
			selectIndex = select;
			onSelectStatus();
			onSelectButtons();
			selectPetStatus(listInventoryData[selectIndex]);
			Singleton<SoundManager>.Instance.playSound("uiClick");
		}
	}

	public void onVideoAd()
	{
		//UCODESHOP TODO 直接跳转到播放广告成功
		//videoEnded();

		if ((int)dataManager.eggCount <= 0)
		{
			Singleton<UIControlManager>.Instance.onPopupYesNo("NOT EGG..");
		}
		else if (dataManager.listPets.Count >= 16)
		{
			Singleton<UIControlManager>.Instance.onPopupYesNo("FULL INVENTORY!");
		}
		else /*if ((bool)dataManager.noAds)*/
		{
			videoEnded();
		}


        // if ((int)dataManager.eggCount <= 0)
        // {
        // 	Singleton<UIControlManager>.Instance.onPopupYesNo("NOT EGG..");
        // }
        // else if (dataManager.listPets.Count >= 16)
        // {
        // 	Singleton<UIControlManager>.Instance.onPopupYesNo("FULL INVENTORY!");
        // }
        // else if ((bool)dataManager.noAds)
        // {
        // 	videoEnded();
        // }
        // else
        // {
        // 	Singleton<UIControlManager>.Instance.onPopupYesNo("Would you like to see the Reward Video ad?", delegate
        // 	{
        // 		if (Singleton<MopubCommunicator>.Instance.hasVideo())
        // 		{
        // 			Singleton<MopubCommunicator>.Instance.showVideo(delegate
        // 			{
        // 				tracker.triggerSeeAdsReward();
        // 				DataManager obj = dataManager;
        // 				obj.rewardVideoWatchCount = (int)obj.rewardVideoWatchCount + 1;
        // 				dataManager.saveData();
        // 				appCustomEventManager.pushEvent("watchRV", "reward_video");
        // 				if ((int)dataManager.rewardVideoWatchCount == 3)
        // 				{
        // 					appCustomEventManager.pushEvent("watchRV", "3times");
        // 				}
        // 				videoEnded();
        // 			});
        // 		}
        // 		else
        // 		{
        // 			Singleton<UIControlManager>.Instance.onPopupYesNo("Failed to load ad. Please check your network status or try in a few minutes.");
        // 			Singleton<MopubCommunicator>.Instance.loadVideo();
        // 		}
        // 	}, delegate
        // 	{
        // 	});
        // }
    }

    public void videoEnded()
	{
		StartCoroutine(actionEggBreak());
	}

	private IEnumerator actionEggBreak()
	{
		if ((int)dataManager.eggCount > 0) {

			eggPanel.SetActive(value: true);

			smallHemer.DOFade(1f, 0.5f);
			yield return new WaitForSeconds(0.5f);
			smallHemer.transform.DORotateQuaternion(Quaternion.Euler(0f, 0f, -65f), 0.5f);
			yield return new WaitForSeconds(0.5f);
			smallHemer.transform.DORotateQuaternion(Quaternion.Euler(0f, 0f, 0f), 0.2f).SetEase(Ease.Flash);
			yield return new WaitForSeconds(0.2f);
			textEggCount.text = "x" + ((int)dataManager.eggCount - 1).ToString();
			smallHemer.DOFade(0f, 1f);
			eggBreakObject.gameObject.SetActive(value: true);
			int count = listEggBreakSprite.Count;
			for (int i = 0; i < count; i++)
			{
				eggImage.transform.DOLocalRotateQuaternion(Quaternion.Euler(0f, 0f, 7f), 0.15f);
				eggImage.transform.DOLocalRotateQuaternion(Quaternion.Euler(0f, 0f, -7f), 0.15f).SetDelay(0.15f);
				eggImage.transform.DOLocalRotateQuaternion(Quaternion.Euler(0f, 0f, 0f), 0.15f).SetDelay(0.3f);
				switch (i)
				{
					case 0:
					case 1:
						soundManager.playSound("ui_pet_gotcha_1");
						break;
					case 2:
					case 3:
						soundManager.playSound("ui_pet_gotcha_2");
						break;
					case 4:
						soundManager.playSound("ui_pet_gotcha_3");
						break;
				}
				eggBreakObject.sprite = listEggBreakSprite[i];
				yield return new WaitForSeconds(0.5f);
			}
			eggBreakEndObject.gameObject.SetActive(value: true);
			eggBreakEndObject.color = new Color(1f, 1f, 1f, 0f);
			eggBreakEndObject.DOFade(1f, 0.5f);
			yield return new WaitForSeconds(0.8f);
			soundManager.playSound("ui_pet_gotcha_4");
			eggBreakCircle.gameObject.SetActive(value: true);
			eggBreakCircle.color = new Color(1f, 1f, 1f, 1f);
			eggBreakCircle.transform.localScale = new Vector3(0f, 0f, 0f);
			eggBreakCircle.transform.DOScale(new Vector3(50f, 50f, 50f), 0.2f);
			yield return new WaitForSeconds(1f);
			eggImage.gameObject.SetActive(value: false);
			eggBreakObject.gameObject.SetActive(value: false);
			eggBreakEndObject.gameObject.SetActive(value: false);
			PetData randomPetData = Balance.getRandomPetData();
			dataManager.eggCount = (int)dataManager.eggCount - 1;
			dataManager.hemerCount = (int)dataManager.hemerCount - 1;
			dataManager.listPets.Add(randomPetData);
			dataManager.saveDataAsync();
			Singleton<UIControlManager>.Instance.setEggUI(dataManager.eggCount);
			getPanel.SetActive(value: true);
			getPetImage.gameObject.SetActive(value: true);
			getPetImage.sprite = Singleton<AssetManager>.Instance.LoadSprite("Pets/" + randomPetData.imageIndex.ToString());
			getPetImage.SetNativeSize();
			petLightComponent.openLight(randomPetData.rank);
			eggBreakCircle.DOFade(0f, 1f);
			yield return new WaitForSeconds(1f);
			soundManager.playSound("ui_random_get");
			eggBreakCircle.gameObject.SetActive(value: false);
			eggBreakActionEnded = true;
		}
	}

	public void eggEndTouch()
	{
		if (eggBreakActionEnded)
		{
			eggBreakActionEnded = false;
			dataManager.eggImageIndex = Random.Range(0, listEggSprites.Count);
			dataManager.saveDataAsync();
			eggPanel.SetActive(value: false);
			getPanel.SetActive(value: false);
			getPetImage.gameObject.SetActive(value: false);
			petLightComponent.closeLight();
			refreshEggImage();
			refreshinventoryList();
		}
	}
}
