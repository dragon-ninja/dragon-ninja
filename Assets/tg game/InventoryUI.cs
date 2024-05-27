using CodeStage.AntiCheat.ObscuredTypes;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryUI : BaseUI, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler, IDragHandler, ICancelHandler
{
	[Header("========== DATA FILES")]
	[Header("BOX IMAGE FILES")]
	public Sprite boxNormal;

	public Sprite boxUnique;

	public Sprite boxLegendary;

	public Sprite boxSuperLegendary;

	[Header("FONTS")]
	public Color fontRed;

	public Color fontOrange;

	public Color fontGreen;

	public Color fontBlue;

	public Color fontPink;

	public Color fontBlackWhite;

	public Color fontNormalOnlyGreen;

	public Color fontNormalOnlyRed;

	public Color fontOnlyGreen;

	public Color fontOnlyRed;

	[Header("ICON IMAGE FILES")]
	public Sprite iconPower;

	public Sprite iconCritical;

	public Sprite iconShield;

	public Sprite iconSpeed;

	public Sprite iconHP;

	public List<Sprite> listSkillIcons = new List<Sprite>();

	[Header("MATERIALS")]
	public Material materialGray;

	[Header("========== OBJECTS")]
	[Header("CAMERA")]
	public Camera uiCamera;

	[Header("BOX OBJECTS")]
	public List<Image> listBoxs = new List<Image>();

	public List<Image> listBoxIcons = new List<Image>();

	public List<TextMeshProUGUI> listLevelText = new List<TextMeshProUGUI>();

	public List<GameObject> listLevelBackground = new List<GameObject>();

	public GameObject selectImage;

	public TextMeshProUGUI textAttachPercent;

	public TextMeshProUGUI textAttach;

	[Header("SELECT BUTTONS")]
	public Button buttonUpgrade;

	public Button buttonSale;

	public Button buttonSelect;

	public TextMeshProUGUI textSale;

	public TextMeshProUGUI textUpgrade;

	[Header("SELECT UI OBJECTS")]
	public Image selectEquipmentImage;

	public Image equipmentBackground;

	public GameObject nowSelectImage;

	public List<SelectEquipmentStateUI> listSelectUIs = new List<SelectEquipmentStateUI>();

	[Header("STATUS UI OBJECTS")]
	public TextMeshProUGUI textPower;

	public TextMeshProUGUI textCritical;

	public TextMeshProUGUI textShield;

	public TextMeshProUGUI textSpeed;

	public TextMeshProUGUI textHP;

	public TextMeshProUGUI textPowerChangeValue;

	public TextMeshProUGUI textCriticalChangeValue;

	public TextMeshProUGUI textShieldChangeValue;

	public TextMeshProUGUI textSpeedChangeValue;

	public TextMeshProUGUI textHPChangeValue;

	[Header("INVENTORY BUTTONS")]
	public List<Image> listInventoryButtons = new List<Image>();

	[Header("PLAYER OBJECT")]
	public GameObject playerObject;

	private Player playerComponent;

	public Image iconLegendaryWeapon;

	public Image iconLegendaryHelmet;

	public Image iconLegendaryArmor;

	public Image iconLegendaryHorse;

	public TextMeshProUGUI textLegendaryWeapon;

	public TextMeshProUGUI textLegendaryHelmet;

	public TextMeshProUGUI textLegendaryArmor;

	public TextMeshProUGUI textLegendaryHorse;

	public Image iconSelectEquipment;

	[Header("BUTTON OBJECTS")]
	public List<Button> listButtons = new List<Button>();

	[Header("BACKGROUND OBJECTS")]
	public RectTransform backgroundTransform;

	public Image panel;

	[Header("UPGRADE ACTION OBJECTS")]
	public Material spriteDefaultMaterial;

	public Material spriteWhiteMaterial;

	public Image upgradePanel;

	public Image upgradeEndedPanel;

	public TextMeshProUGUI upgradetext;

	[Header("PARTICLE OBJECTS")]
	public ParticleSystem particleNormal;

	public ParticleSystem particleUnique;

	public ParticleSystem particleLegendary;

	public ParticleSystem particleSuperLegendary;

	public ParticleSystem particleAttachUpgrade;

	[Header("SELL OBJECTS")]
	public GameObject sellPanel;

	public List<GameObject> listSellSelects = new List<GameObject>();

	private List<int> listSellSelectIndexs = new List<int>();

	private DataManager dataManager;

	private PlayerManager playerManager;

	private SoundManager soundManager;

	private TutorialManager tutorialManager;

	private List<EquipmentData> listInventoryData = new List<EquipmentData>();

	private List<ObscuredInt> listInventoryIndexs = new List<ObscuredInt>();

	private EquipmentType selectInventoryType;

	private int selectIndex = -1;

	private GameObject touchInventoryBox;

	private Vector2 boxStartPosition;

	private int touchIndex = -1;

	private Vector2 touchStartPosition;

	private bool boxMoveState;

	private void inventoryListSetting(EquipmentType type)
	{
		selectInventoryType = type;
		EquipmentData selectHelmet = dataManager.selectHelmet;
		switch (selectInventoryType)
		{
		case EquipmentType.TYPE_HELMET:
		{
			listInventoryData = dataManager.listHelmets;
			listInventoryIndexs = dataManager.listInventoryHelmets;
			EquipmentData selectHelmet2 = dataManager.selectHelmet;
			break;
		}
		case EquipmentType.TYPE_ARMOR:
		{
			listInventoryData = dataManager.listArmors;
			listInventoryIndexs = dataManager.listInventoryArmors;
			EquipmentData selectArmor = dataManager.selectArmor;
			break;
		}
		case EquipmentType.TYPE_WEAPON:
		{
			listInventoryData = dataManager.listWeapons;
			listInventoryIndexs = dataManager.listInventoryWeapons;
			EquipmentData selectWeapon = dataManager.selectWeapon;
			break;
		}
		case EquipmentType.TYPE_HORSE:
		{
			listInventoryData = dataManager.listHorses;
			listInventoryIndexs = dataManager.listInventoryHorses;
			EquipmentData selectHorse = dataManager.selectHorse;
			break;
		}
		}
		int count = listInventoryButtons.Count;
		for (int i = 0; i < count; i++)
		{
			if (i == (int)selectInventoryType)
			{
				listInventoryButtons[i].color = Color.white;
			}
			else
			{
				listInventoryButtons[i].color = new Color(0.5f, 0.5f, 0.5f, 1f);
			}
		}
	}

	private void firstSelectEquipment()
	{
		EquipmentData equipmentData = dataManager.selectHelmet;
		switch (selectInventoryType)
		{
		case EquipmentType.TYPE_HELMET:
			equipmentData = dataManager.selectHelmet;
			break;
		case EquipmentType.TYPE_ARMOR:
			equipmentData = dataManager.selectArmor;
			break;
		case EquipmentType.TYPE_WEAPON:
			equipmentData = dataManager.selectWeapon;
			break;
		case EquipmentType.TYPE_HORSE:
			equipmentData = dataManager.selectHorse;
			break;
		}
		int count = listInventoryData.Count;
		int num = 0;
		while (true)
		{
			if (num < count)
			{
				if ((int)listInventoryData[num].objectIndex == (int)equipmentData.objectIndex)
				{
					break;
				}
				num++;
				continue;
			}
			return;
		}
		onChoice(searchInventoryLocation(listInventoryData[num].objectIndex));
	}

	private void refreshinventoryList()
	{
		int count = listInventoryData.Count;
		int count2 = listBoxs.Count;
		int num = 0;
		switch (selectInventoryType)
		{
		case EquipmentType.TYPE_HELMET:
			num = dataManager.selectHelmet.objectIndex;
			break;
		case EquipmentType.TYPE_ARMOR:
			num = dataManager.selectArmor.objectIndex;
			break;
		case EquipmentType.TYPE_WEAPON:
			num = dataManager.selectWeapon.objectIndex;
			break;
		case EquipmentType.TYPE_HORSE:
			num = dataManager.selectHorse.objectIndex;
			break;
		}
		for (int i = 0; i < count2; i++)
		{
			listBoxs[i].sprite = boxNormal;
			listBoxs[i].color = Color.white;
			listBoxs[i].SetNativeSize();
			listBoxIcons[i].sprite = null;
			listBoxIcons[i].transform.localScale = new Vector3(0f, 0f, 1f);
			listLevelText[i].text = "";
			listLevelBackground[i].SetActive(value: false);
		}
		List<bool> list = new List<bool>
		{
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false
		};
		for (int j = 0; j < count; j++)
		{
			int num2 = searchInventoryLocation(listInventoryData[j].objectIndex);
			if (num2 == -1)
			{
				num2 = settingInventoryLocation(listInventoryData[j].objectIndex);
				if (num2 == -1)
				{
					continue;
				}
				listInventoryIndexs[num2] = listInventoryData[j].objectIndex;
			}
			list[num2] = true;
			switch (listInventoryData[j].rank)
			{
			case EquipmentRank.TYPE_NORMAL:
				listBoxs[num2].sprite = boxNormal;
				listBoxs[num2].rectTransform.pivot = new Vector2(0.5f, 0.5f);
				break;
			case EquipmentRank.TYPE_UNIQUE:
				listBoxs[num2].sprite = boxUnique;
				listBoxs[num2].rectTransform.pivot = new Vector2(0.5f, 0.5f);
				break;
			case EquipmentRank.TYPE_LEGENDARY:
				listBoxs[num2].sprite = boxLegendary;
				listBoxs[num2].rectTransform.pivot = new Vector2(0.5f, 0.5f);
				break;
			case EquipmentRank.TYPE_SUPERLEGENDARY:
				listBoxs[num2].sprite = boxSuperLegendary;
				listBoxs[num2].rectTransform.pivot = new Vector2(0.5f, 0.47f);
				break;
			}
			listBoxs[num2].SetNativeSize();
			string text = "";
			float num3 = 0f;
			switch (listInventoryData[j].type)
			{
			case EquipmentType.TYPE_HELMET:
				text = "Helmet";
				num3 = 90f;
				break;
			case EquipmentType.TYPE_ARMOR:
				text = "Armor";
				num3 = 90f;
				break;
			case EquipmentType.TYPE_WEAPON:
				text = "Weapon";
				num3 = 120f;
				break;
			case EquipmentType.TYPE_HORSE:
				text = "Horse";
				num3 = 120f;
				break;
			}
			Image image = listBoxIcons[num2];
			AssetManager instance = Singleton<AssetManager>.Instance;
			string str = text;
			EquipmentData equipmentData = listInventoryData[j];
			image.sprite = instance.LoadSprite("Equipment/" + str + "/" + equipmentData.imageIndex.ToString());
			listBoxIcons[num2].SetNativeSize();
			if ((int)listInventoryData[j].imageIndex == 10000)
			{
				listLevelText[num2].text = "";
				listLevelBackground[num2].SetActive(value: false);
				listBoxs[num2].color = Color.white;
				listBoxIcons[num2].transform.rotation = Quaternion.Euler(0f, 0f, 0f);
				listBoxIcons[num2].transform.localScale = new Vector3(1f, 1f, 1f);
				continue;
			}
			Rect rect = listBoxIcons[num2].sprite.rect;
			float num4 = (rect.width > rect.height) ? rect.width : rect.height;
			float num5 = num3 / num4;
			listBoxIcons[num2].transform.localScale = new Vector3(num5, num5, 1f);
			if (listInventoryData[j].type == EquipmentType.TYPE_WEAPON)
			{
				listBoxIcons[num2].transform.rotation = Quaternion.Euler(0f, 0f, 45f);
			}
			else
			{
				listBoxIcons[num2].transform.rotation = Quaternion.Euler(0f, 0f, 0f);
			}
			if (listInventoryData[j].rank >= EquipmentRank.TYPE_LEGENDARY)
			{
				TextMeshProUGUI textMeshProUGUI = listLevelText[num2];
				equipmentData = listInventoryData[j];
				textMeshProUGUI.text = equipmentData.level.ToString();
			}
			else
			{
				TextMeshProUGUI textMeshProUGUI2 = listLevelText[num2];
				equipmentData = listInventoryData[j];
				textMeshProUGUI2.text = equipmentData.imageIndex.ToString();
			}
			listLevelBackground[num2].SetActive(value: true);
			if ((int)listInventoryData[j].objectIndex == num)
			{
				selectImage.transform.localPosition = listBoxs[num2].transform.localPosition + new Vector3(0f, 100f, 0f);
				listBoxs[num2].color = new Color(0.5f, 0.5f, 0.5f, 1f);
			}
			else
			{
				listBoxs[num2].color = Color.white;
			}
		}
		int count3 = list.Count;
		for (int k = 0; k < count3; k++)
		{
			if (!list[k])
			{
				listInventoryIndexs[k] = -1;
			}
		}
	}

	private void activeInventoryChange()
	{
		switch (selectInventoryType)
		{
		case EquipmentType.TYPE_HELMET:
			dataManager.listHelmets = listInventoryData;
			dataManager.listInventoryHelmets = listInventoryIndexs;
			break;
		case EquipmentType.TYPE_ARMOR:
			dataManager.listArmors = listInventoryData;
			dataManager.listInventoryArmors = listInventoryIndexs;
			break;
		case EquipmentType.TYPE_WEAPON:
			dataManager.listWeapons = listInventoryData;
			dataManager.listInventoryWeapons = listInventoryIndexs;
			break;
		case EquipmentType.TYPE_HORSE:
			dataManager.listHorses = listInventoryData;
			dataManager.listInventoryHorses = listInventoryIndexs;
			break;
		}
	}

	private void onSelectStatus()
	{
		if (selectIndex == -1)
		{
			return;
		}
		int num = inventoryLocationToData(selectIndex);
		if (num == -1)
		{
			return;
		}
		nowSelectImage.SetActive(value: true);
		Vector3 localPosition = listBoxs[selectIndex].transform.localPosition;
		localPosition.y += 97f;
		nowSelectImage.transform.localPosition = localPosition;
		EquipmentData equipmentData = listInventoryData[num];
		if (equipmentData.rank == EquipmentRank.TYPE_SUPERLEGENDARY)
		{
			textAttachPercent.gameObject.SetActive(value: false);
			textAttach.gameObject.SetActive(value: false);
		}
		else
		{
			textAttachPercent.gameObject.SetActive(value: true);
			textAttach.gameObject.SetActive(value: true);
			float num2 = 100f - Balance.getAttackEquipmentPercent(equipmentData);
			textAttach.text = num2.ToString("0") + "%";
			Color color = (num2 >= 90f) ? fontRed : ((!(num2 >= 69.9f)) ? fontOrange : fontGreen);
			textAttach.color = color;
		}
		List<float> list = new List<float>();
		list.Add((int)equipmentData.power);
		list.Add(equipmentData.critical);
		list.Add((int)equipmentData.shield);
		list.Add(equipmentData.speed);
		list.Add((int)equipmentData.hp);
		int count = listSelectUIs.Count;
		for (int i = 0; i < count; i++)
		{
			listSelectUIs[i].icon.gameObject.SetActive(value: true);
			listSelectUIs[i].text.gameObject.SetActive(value: true);
			string text = list[i].ToString((i == 1 || i == 3) ? "0.0" : "0");
			switch (i)
			{
			case 0:
				listSelectUIs[i].icon.sprite = iconPower;
				break;
			case 1:
				listSelectUIs[i].icon.sprite = iconCritical;
				break;
			case 2:
				listSelectUIs[i].icon.sprite = iconShield;
				break;
			case 3:
				listSelectUIs[i].icon.sprite = iconSpeed;
				break;
			case 4:
				listSelectUIs[i].icon.sprite = iconHP;
				break;
			}
			if (list[i] <= 0.1f)
			{
				text = "0";
				if (i == 1)
				{
					text += "%";
				}
				listSelectUIs[i].text.text = text;
				listSelectUIs[i].icon.material = materialGray;
				listSelectUIs[i].text.color = fontBlackWhite;
				continue;
			}
			listSelectUIs[i].icon.material = null;
			if (i == 1)
			{
				text += "%";
			}
			listSelectUIs[i].text.text = text;
			switch (i)
			{
			case 0:
				listSelectUIs[i].text.color = fontRed;
				break;
			case 1:
				listSelectUIs[i].text.color = fontOrange;
				break;
			case 2:
				listSelectUIs[i].text.color = fontGreen;
				break;
			case 3:
				listSelectUIs[i].text.color = fontBlue;
				break;
			case 4:
				listSelectUIs[i].text.color = fontPink;
				break;
			}
		}
		if ((int)equipmentData.imageIndex > 10)
		{
			iconSelectEquipment.gameObject.SetActive(value: true);
			iconSelectEquipment.sprite = listSkillIcons[(int)equipmentData.imageIndex - 11];
		}
		else
		{
			iconSelectEquipment.gameObject.SetActive(value: false);
		}
	}

	private void offSelectStatus()
	{
		textAttachPercent.gameObject.SetActive(value: false);
		textAttach.gameObject.SetActive(value: false);
		nowSelectImage.SetActive(value: false);
		int count = listSelectUIs.Count;
		for (int i = 0; i < count; i++)
		{
			listSelectUIs[i].icon.gameObject.SetActive(value: false);
			listSelectUIs[i].text.gameObject.SetActive(value: false);
		}
		iconSelectEquipment.gameObject.SetActive(value: false);
	}

	private void onSelectButtons()
	{
		if (selectIndex == -1)
		{
			return;
		}
		int num = inventoryLocationToData(selectIndex);
		if (num != -1)
		{
			buttonSale.gameObject.SetActive(value: true);
			buttonSelect.gameObject.SetActive(value: true);
			EquipmentData equipmentData = listInventoryData[num];
			textUpgrade.text = "UPGRADE " + Price.getUpgradeEquipment(equipmentData.rank, equipmentData.level).ToString();
			textSale.text = "SELL";
			if (equipmentData.rank == EquipmentRank.TYPE_SUPERLEGENDARY)
			{
				buttonUpgrade.gameObject.SetActive(value: true);
				buttonSale.transform.localPosition = new Vector3(205.5f, 238f, 0f);
				buttonSelect.transform.localPosition = new Vector3(205.5f, 112f, 0f);
			}
			else
			{
				buttonUpgrade.gameObject.SetActive(value: false);
				buttonSale.transform.localPosition = new Vector3(205.5f, 267f, 0f);
				buttonSelect.transform.localPosition = new Vector3(205.5f, 141f, 0f);
			}
		}
	}

	private void offSelectButtons()
	{
		buttonUpgrade.gameObject.SetActive(value: false);
		buttonSale.gameObject.SetActive(value: false);
		buttonSelect.gameObject.SetActive(value: false);
	}

	private void activeSelectEquipment()
	{
		if (selectIndex == -1)
		{
			return;
		}
		int num = inventoryLocationToData(selectIndex);
		if (num != -1)
		{
			EquipmentData equipmentData = listInventoryData[num];
			switch (equipmentData.type)
			{
			case EquipmentType.TYPE_HELMET:
				dataManager.selectHelmet = equipmentData;
				break;
			case EquipmentType.TYPE_ARMOR:
				dataManager.selectArmor = equipmentData;
				break;
			case EquipmentType.TYPE_WEAPON:
				dataManager.selectWeapon = equipmentData;
				break;
			case EquipmentType.TYPE_HORSE:
				dataManager.selectHorse = equipmentData;
				break;
			}
		}
	}

	private void refreshStatus()
	{
		EquipmentData equipmentData = dataManager.selectHelmet;
		EquipmentData equipmentData2 = dataManager.selectArmor;
		EquipmentData equipmentData3 = dataManager.selectWeapon;
		EquipmentData equipmentData4 = dataManager.selectHorse;
		if (selectIndex != -1)
		{
			int num = inventoryLocationToData(selectIndex);
			if (num == -1)
			{
				return;
			}
			EquipmentData equipmentData5 = default(EquipmentData);
			EquipmentData equipmentData6 = listInventoryData[num];
			switch (equipmentData6.type)
			{
			case EquipmentType.TYPE_HELMET:
				equipmentData5 = equipmentData;
				equipmentData = equipmentData6;
				break;
			case EquipmentType.TYPE_ARMOR:
				equipmentData5 = equipmentData2;
				equipmentData2 = equipmentData6;
				break;
			case EquipmentType.TYPE_WEAPON:
				equipmentData5 = equipmentData3;
				equipmentData3 = equipmentData6;
				break;
			case EquipmentType.TYPE_HORSE:
				equipmentData5 = equipmentData4;
				equipmentData4 = equipmentData6;
				break;
			}
			int num2 = (int)equipmentData6.power + (int)equipmentData6.addPower - ((int)equipmentData5.power + (int)equipmentData5.addPower);
			float num3 = (float)equipmentData6.critical + (float)equipmentData6.addCritical - ((float)equipmentData5.critical + (float)equipmentData5.addCritical);
			int num4 = (int)equipmentData6.shield + (int)equipmentData6.addShield - ((int)equipmentData5.shield + (int)equipmentData5.addShield);
			float num5 = (float)equipmentData6.speed + (float)equipmentData6.addSpeed - ((float)equipmentData5.speed + (float)equipmentData5.addSpeed);
			int num6 = (int)equipmentData6.hp + (int)equipmentData6.addHp - ((int)equipmentData5.hp + (int)equipmentData5.addHp);
			if (num2 != 0)
			{
				textPowerChangeValue.gameObject.SetActive(value: true);
				textPowerChangeValue.text = ((num2 > 0) ? "+" : "-") + Mathf.Abs(num2).ToString();
				textPowerChangeValue.color = ((num2 > 0) ? fontOnlyGreen : fontOnlyRed);
			}
			else
			{
				textPowerChangeValue.gameObject.SetActive(value: false);
			}
			if (num3 <= -0.1f || 0.1f <= num3)
			{
				textCriticalChangeValue.gameObject.SetActive(value: true);
				textCriticalChangeValue.text = ((num3 > 0f) ? "+" : "-") + Mathf.Abs(num3).ToString("0.0") + "%";
				textCriticalChangeValue.color = ((num3 > 0f) ? fontOnlyGreen : fontOnlyRed);
			}
			else
			{
				textCriticalChangeValue.gameObject.SetActive(value: false);
			}
			if (num4 != 0)
			{
				textShieldChangeValue.gameObject.SetActive(value: true);
				textShieldChangeValue.text = ((num4 > 0) ? "+" : "-") + Mathf.Abs(num4).ToString();
				textShieldChangeValue.color = ((num4 > 0) ? fontOnlyGreen : fontOnlyRed);
			}
			else
			{
				textShieldChangeValue.gameObject.SetActive(value: false);
			}
			if (num5 <= -0.1f || 0.1f <= num5)
			{
				textSpeedChangeValue.gameObject.SetActive(value: true);
				textSpeedChangeValue.text = ((num5 > 0f) ? "+" : "-") + Mathf.Abs(num5).ToString("0.0");
				textSpeedChangeValue.color = ((num5 > 0f) ? fontOnlyGreen : fontOnlyRed);
			}
			else
			{
				textSpeedChangeValue.gameObject.SetActive(value: false);
			}
			if (num6 != 0)
			{
				textHPChangeValue.gameObject.SetActive(value: true);
				textHPChangeValue.text = ((num6 > 0) ? "+" : "-") + Mathf.Abs(num6).ToString();
				textHPChangeValue.color = ((num6 > 0) ? fontOnlyGreen : fontOnlyRed);
			}
			else
			{
				textHPChangeValue.gameObject.SetActive(value: false);
			}
		}
		else
		{
			textPowerChangeValue.gameObject.SetActive(value: false);
			textCriticalChangeValue.gameObject.SetActive(value: false);
			textShieldChangeValue.gameObject.SetActive(value: false);
			textSpeedChangeValue.gameObject.SetActive(value: false);
			textHPChangeValue.gameObject.SetActive(value: false);
		}
		int num7 = (int)equipmentData.power + (int)equipmentData2.power + (int)equipmentData3.power + (int)equipmentData4.power;
		float num8 = (float)equipmentData.critical + (float)equipmentData2.critical + (float)equipmentData3.critical + (float)equipmentData4.critical;
		int num9 = (int)equipmentData.shield + (int)equipmentData2.shield + (int)equipmentData3.shield + (int)equipmentData4.shield;
		float num10 = (float)equipmentData.speed + (float)equipmentData2.speed + (float)equipmentData3.speed + (float)equipmentData4.speed;
		int num11 = (int)equipmentData.hp + (int)equipmentData2.hp + (int)equipmentData3.hp + (int)equipmentData4.hp;
		num7 += (int)equipmentData.addPower + (int)equipmentData2.addPower + (int)equipmentData3.addPower + (int)equipmentData4.addPower;
		num8 += (float)equipmentData.addCritical + (float)equipmentData2.addCritical + (float)equipmentData3.addCritical + (float)equipmentData4.addCritical;
		num9 += (int)equipmentData.addShield + (int)equipmentData2.addShield + (int)equipmentData3.addShield + (int)equipmentData4.addShield;
		num10 += (float)equipmentData.addSpeed + (float)equipmentData2.addSpeed + (float)equipmentData3.addSpeed + (float)equipmentData4.addSpeed;
		num11 += (int)equipmentData.addHp + (int)equipmentData2.addHp + (int)equipmentData3.addHp + (int)equipmentData4.addHp;
		textPower.text = "+" + num7.ToString();
		textCritical.text = num8.ToString("0.0") + "%";
		textShield.text = "+" + num9.ToString();
		textSpeed.text = "+" + num10.ToString("0.0");
		textHP.text = "+" + num11.ToString();
	}

	private void refreshSelectEquipmentImage()
	{
		if (selectIndex != -1)
		{
			int num = inventoryLocationToData(selectIndex);
			if (num != -1)
			{
				EquipmentData equipmentData = listInventoryData[num];
				float num2 = 0f;
				switch (equipmentData.type)
				{
				case EquipmentType.TYPE_HELMET:
					num2 = 100f;
					break;
				case EquipmentType.TYPE_ARMOR:
					num2 = 100f;
					break;
				case EquipmentType.TYPE_WEAPON:
					num2 = 140f;
					break;
				case EquipmentType.TYPE_HORSE:
					num2 = 120f;
					break;
				}
				selectEquipmentImage.sprite = listBoxIcons[selectIndex].sprite;
				selectEquipmentImage.SetNativeSize();
				selectEquipmentImage.transform.localScale = new Vector3(1f, 1f, 1f);
				Rect rect = selectEquipmentImage.sprite.rect;
				float num3 = (rect.width > rect.height) ? rect.width : rect.height;
				float num4 = num2 / num3;
				selectEquipmentImage.transform.localScale = new Vector3(num4, num4, 1f);
				if (equipmentData.type == EquipmentType.TYPE_WEAPON)
				{
					selectEquipmentImage.transform.rotation = Quaternion.Euler(0f, 0f, 45f);
				}
				else
				{
					selectEquipmentImage.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
				}
				equipmentBackground.gameObject.SetActive(value: true);
			}
		}
		else
		{
			selectEquipmentImage.sprite = null;
			selectEquipmentImage.transform.localScale = new Vector3(0f, 0f, 1f);
			equipmentBackground.gameObject.SetActive(value: false);
		}
	}

	private void refreshPlayer()
	{
		int num = dataManager.selectHelmet.imageIndex;
		int num2 = dataManager.selectArmor.imageIndex;
		int num3 = dataManager.selectWeapon.imageIndex;
		int num4 = dataManager.selectHorse.imageIndex;
		if (selectIndex != -1)
		{
			int num5 = inventoryLocationToData(selectIndex);
			if (num5 == -1)
			{
				return;
			}
			EquipmentData equipmentData = listInventoryData[num5];
			switch (equipmentData.type)
			{
			case EquipmentType.TYPE_HELMET:
				num = equipmentData.imageIndex;
				break;
			case EquipmentType.TYPE_ARMOR:
				num2 = equipmentData.imageIndex;
				break;
			case EquipmentType.TYPE_WEAPON:
				num3 = equipmentData.imageIndex;
				break;
			case EquipmentType.TYPE_HORSE:
				num4 = equipmentData.imageIndex;
				break;
			}
		}
		if (num > 10)
		{
			iconLegendaryHelmet.gameObject.SetActive(value: true);
			iconLegendaryHelmet.sprite = listSkillIcons[num - 11];
		}
		else
		{
			iconLegendaryHelmet.gameObject.SetActive(value: false);
		}
		if (num2 > 10)
		{
			iconLegendaryArmor.gameObject.SetActive(value: true);
			iconLegendaryArmor.sprite = listSkillIcons[num2 - 11];
		}
		else
		{
			iconLegendaryArmor.gameObject.SetActive(value: false);
		}
		if (num3 > 10)
		{
			iconLegendaryWeapon.gameObject.SetActive(value: true);
			iconLegendaryWeapon.sprite = listSkillIcons[num3 - 11];
		}
		else
		{
			iconLegendaryWeapon.gameObject.SetActive(value: false);
		}
		if (num4 > 10)
		{
			iconLegendaryHorse.gameObject.SetActive(value: true);
			iconLegendaryHorse.sprite = listSkillIcons[num4 - 11];
		}
		else
		{
			iconLegendaryHorse.gameObject.SetActive(value: false);
		}
		refreshLegendaryCount(dataManager.selectWeapon, dataManager.selectArmor, dataManager.selectHelmet, dataManager.selectHorse);
		playerComponent.settingImage(num, num2, num3, num4);
	}

	private void refreshLegendaryCount(EquipmentData weapon, EquipmentData armor, EquipmentData helmet, EquipmentData horse)
	{
		List<int> list = new List<int>
		{
			0,
			0,
			0,
			0,
			0
		};
		List<TextMeshProUGUI> list2 = new List<TextMeshProUGUI>
		{
			textLegendaryWeapon,
			textLegendaryArmor,
			textLegendaryHelmet,
			textLegendaryHorse
		};
		if ((int)weapon.imageIndex > 10)
		{
			List<int> list3 = list;
			int index = (int)weapon.imageIndex - 11;
			list3[index]++;
		}
		if ((int)armor.imageIndex > 10)
		{
			List<int> list4 = list;
			int index2 = (int)armor.imageIndex - 11;
			list4[index2]++;
		}
		if ((int)helmet.imageIndex > 10)
		{
			List<int> list5 = list;
			int index = (int)helmet.imageIndex - 11;
			list5[index]++;
		}
		if ((int)horse.imageIndex > 10)
		{
			List<int> list6 = list;
			int index2 = (int)horse.imageIndex - 11;
			list6[index2]++;
		}
		for (int i = 0; i < 4; i++)
		{
			int num = 0;
			switch (i)
			{
			case 0:
				if ((int)weapon.imageIndex > 10)
				{
					num = list[(int)weapon.imageIndex - 11];
				}
				break;
			case 1:
				if ((int)armor.imageIndex > 10)
				{
					num = list[(int)armor.imageIndex - 11];
				}
				break;
			case 2:
				if ((int)helmet.imageIndex > 10)
				{
					num = list[(int)helmet.imageIndex - 11];
				}
				break;
			case 3:
				if ((int)horse.imageIndex > 10)
				{
					num = list[(int)horse.imageIndex - 11];
				}
				break;
			}
			list2[i].gameObject.SetActive(num > 0);
			list2[i].text = "x" + num;
		}
	}

	public void OnPointerDown(PointerEventData pointerEventData)
	{

		touchStartPosition = pointerEventData.position;
		GameObject pointerEnter = pointerEventData.pointerEnter;
		if (pointerEnter != null && pointerEnter.tag.Equals("InventoryBox"))
		{
			int count = listBoxs.Count;
			for (int i = 0; i < count; i++)
			{
				if (listBoxs[i].gameObject == pointerEnter)
				{
					if (!(listBoxIcons[i].sprite == null))
					{
						touchIndex = i;
						touchInventoryBox = pointerEnter;
						boxStartPosition.x = pointerEnter.transform.localPosition.x;
						boxStartPosition.y = pointerEnter.transform.localPosition.y;
					}
					break;
				}
			}
		}
		if (!sellPanel.activeSelf || !(touchInventoryBox != null))
		{
			return;
		}
	
		int count2 = listBoxs.Count;
		int num = 0;
		while (true)
		{
			if (num < count2)
			{
				if (listBoxs[num].gameObject == touchInventoryBox)
				{
					break;
				}
				num++;
				continue;
			}
			return;
		}
		
		if (!listSellSelects[num].activeSelf)
		{
			int num2 = inventoryLocationToData(num);
			if (num2 == -1)
			{
				return;
			}
			EquipmentData equipmentData = listInventoryData[num2];
			if ((int)dataManager.selectArmor.objectIndex == (int)equipmentData.objectIndex || (int)dataManager.selectHelmet.objectIndex == (int)equipmentData.objectIndex || (int)dataManager.selectWeapon.objectIndex == (int)equipmentData.objectIndex || (int)dataManager.selectHorse.objectIndex == (int)equipmentData.objectIndex)
			{
				Singleton<UIControlManager>.Instance.onPopupYesNo("You cannot sell items that are in use!");
				return;
			}
			if ((int)equipmentData.imageIndex == 10000)
			{
				Singleton<UIControlManager>.Instance.onPopupYesNo("Joker card types cannot be sell!");
				return;
			}
			listSellSelects[num].SetActive(value: true);
			listSellSelectIndexs.Add(num);
		}
		else
		{
			listSellSelects[num].SetActive(value: false);
			listSellSelectIndexs.Remove(num);
		}
		int num3 = 0;
		int count3 = listSellSelectIndexs.Count;
		for (int j = 0; j < count3; j++)
		{
			int index = listSellSelectIndexs[j];
			int num4 = inventoryLocationToData(index);
			if (num4 != -1)
			{
				EquipmentData equipmentData2 = listInventoryData[num4];
				num3 += Price.getSaleEquipment(equipmentData2.rank, equipmentData2.level, equipmentData2.imageIndex);
			}
		}
		if (num3 == 0)
		{
			textSale.text = "CANCEL";
		}
		else
		{
			textSale.text = "SELL " + num3.ToString();
		}
	}

	public void OnDrag(PointerEventData pointerEventData)
	{
		if (sellPanel.activeSelf || touchInventoryBox == null || !RectTransformUtility.ScreenPointToLocalPointInRectangle(touchInventoryBox.transform.parent as RectTransform, pointerEventData.position, uiCamera, out Vector2 localPoint))
		{
			return;
		}
		if (!boxMoveState)
		{
			if (Vector2.Distance(localPoint, boxStartPosition) > 50f)
			{
				boxMoveState = true;
				touchInventoryBox.transform.SetAsLastSibling();
			}
		}
		else
		{
			touchInventoryBox.transform.localPosition = new Vector3(localPoint.x, localPoint.y, 100f);
		}
	}

	public void OnPointerUp(PointerEventData pointerEventData)
	{
		if (sellPanel.activeSelf)
		{
			touchInventoryBox = null;
		}
		else
		{
			if (touchInventoryBox == null)
			{
				return;
			}
			RectTransformUtility.ScreenPointToLocalPointInRectangle(touchInventoryBox.transform.parent as RectTransform, pointerEventData.position, uiCamera, out Vector2 localPoint);
			Vector3 b = default(Vector3);
			b.x = localPoint.x;
			b.y = localPoint.y;
			b.z = 0f;
			int count = listBoxs.Count;
			for (int i = 0; i < count; i++)
			{
				if (Vector3.Distance(listBoxs[i].transform.localPosition, b) < 70f)
				{
					if (listBoxs[i].gameObject == touchInventoryBox)
					{
						onChoice(i);
						break;
					}
					attachEquipment(touchIndex, i);
					activeInventoryChange();
					dataManager.saveDataAsync();
					break;
				}
			}
			OnCancel(pointerEventData);
		}
	}

	public void OnCancel(BaseEventData eventData)
	{
		boxMoveState = false;
		touchInventoryBox.transform.localPosition = new Vector3(boxStartPosition.x, boxStartPosition.y, 0f);
		touchInventoryBox = null;
	}

	private void attachEquipment(int touchIndex, int standIndex)
	{
		int num = inventoryLocationToData(touchIndex);
		int num2 = inventoryLocationToData(standIndex);
		if (num == -1 || num2 == -1)
		{
			return;
		}
		EquipmentData equipmentData = listInventoryData[num];
		if (listInventoryData.Count <= num2)
		{
			return;
		}
		EquipmentData equipmentData2 = listInventoryData[num2];
		if ((int)equipmentData.imageIndex == 10000 && (int)equipmentData2.imageIndex == 10000)
		{
			Singleton<UIControlManager>.Instance.onPopupYesNo("Joker card types cannot be combined!");
			return;
		}
		bool joker = false;
		if ((int)equipmentData.imageIndex == 10000)
		{
			equipmentData = equipmentData2;
			joker = true;
		}
		else if ((int)equipmentData2.imageIndex == 10000)
		{
			equipmentData2 = equipmentData;
			joker = true;
		}

		if ((int)equipmentData.imageIndex != (int)equipmentData2.imageIndex)
		{
			Singleton<UIControlManager>.Instance.onPopupYesNo("Not the same equipment!");
			return;
		}
		if (equipmentData.rank == EquipmentRank.TYPE_LEGENDARY && (int)equipmentData.level != (int)equipmentData2.level)
		{
			Singleton<UIControlManager>.Instance.onPopupYesNo("Not the same equipment!");
			return;
		}
		if ((int)dataManager.selectArmor.objectIndex == (int)equipmentData.objectIndex || (int)dataManager.selectHelmet.objectIndex == (int)equipmentData.objectIndex || (int)dataManager.selectWeapon.objectIndex == (int)equipmentData.objectIndex || (int)dataManager.selectHorse.objectIndex == (int)equipmentData.objectIndex || (int)dataManager.selectArmor.objectIndex == (int)equipmentData2.objectIndex || (int)dataManager.selectHelmet.objectIndex == (int)equipmentData2.objectIndex || (int)dataManager.selectWeapon.objectIndex == (int)equipmentData2.objectIndex || (int)dataManager.selectHorse.objectIndex == (int)equipmentData2.objectIndex)
		{
			Singleton<UIControlManager>.Instance.onPopupYesNo("You cannot combine items that are in use!");
			return;
		}
		if (equipmentData.rank > EquipmentRank.TYPE_LEGENDARY || equipmentData2.rank > EquipmentRank.TYPE_SUPERLEGENDARY)
		{
			Singleton<UIControlManager>.Instance.onPopupYesNo("SUPER LEGENDARY cannot be combined");
			return;
		}
		Vector3 localPosition = listBoxs[standIndex].transform.localPosition;
		EquipmentData equipmentData3 = Balance.getAttachEquipmentData(equipmentData, joker);
		if (tutorialManager.getGameTutorialState())
		{
			if (!tutorialManager.getTutorialFail())
			{
				equipmentData3 = new EquipmentData(EquipmentType.TYPE_WEAPON, EquipmentRank.TYPE_NORMAL, EquipmentGrade.TYPE_D, 2, 0, 85, 6f, 0, 0, 0f, 0, 0f, 0, 0, 0f);
			}
			else
			{
				equipmentData3 = new EquipmentData(EquipmentType.TYPE_WEAPON, EquipmentRank.TYPE_NORMAL, EquipmentGrade.TYPE_D, 1, 0, 75, 2f, 0, 0, 0f, 0, 0f, 0, 0, 0f);
				tutorialManager.failClear();
			}
		}
		if ((int)equipmentData3.imageIndex != (int)equipmentData.imageIndex)
		{
			particleAttachUpgrade.transform.localPosition = localPosition;
			particleAttachUpgrade.gameObject.SetActive(value: true);
			particleAttachUpgrade.Play();
			soundManager.playSound("EquipmentAttachSuccessed");
			if (equipmentData3.rank == EquipmentRank.TYPE_UNIQUE && !dataManager.firstUnique)
			{
				Singleton<AppCustomEventManager>.Instance.pushEvent("getPurple");
				dataManager.firstUnique = true;
			}
		}
		else
		{
			soundManager.playSound("EquipmentAttachFailed");
		}
		localPosition.y += 100f;
		switch (equipmentData3.rank)
		{
		case EquipmentRank.TYPE_NORMAL:
			particleNormal.transform.localPosition = localPosition;
			particleNormal.gameObject.SetActive(value: true);
			particleNormal.Play();
			break;
		case EquipmentRank.TYPE_UNIQUE:
			particleUnique.transform.localPosition = localPosition;
			particleUnique.gameObject.SetActive(value: true);
			particleUnique.Play();
			break;
		case EquipmentRank.TYPE_LEGENDARY:
			particleLegendary.transform.localPosition = localPosition;
			particleLegendary.gameObject.SetActive(value: true);
			particleLegendary.Play();
			break;
		case EquipmentRank.TYPE_SUPERLEGENDARY:
			particleSuperLegendary.transform.localPosition = localPosition;
			particleSuperLegendary.gameObject.SetActive(value: true);
			particleSuperLegendary.Play();
			break;
		}
		listInventoryIndexs[standIndex] = equipmentData3.objectIndex;
		listInventoryData[num2] = equipmentData3;
		listInventoryIndexs[touchIndex] = -1;
		listInventoryData.RemoveAt(num);
		activeInventoryChange();
		dataManager.saveDataAsync();
		selectIndex = -1;
		refreshinventoryList();
		refreshSelectEquipmentImage();
		offSelectStatus();
		offSelectButtons();
		tutorialManager.onInventoryAttach();
	}

	private int inventoryLocationToData(int index)
	{
		int count = listInventoryData.Count;
		int num = listInventoryIndexs[index];
		if (num == -1)
		{
			return -1;
		}
		for (int i = 0; i < count; i++)
		{
			if ((int)listInventoryData[i].objectIndex == num)
			{
				return i;
			}
		}
		return -1;
	}

	private int searchInventoryLocation(int objectIndex)
	{
		int result = -1;
		int count = listInventoryIndexs.Count;
		for (int i = 0; i < count; i++)
		{
			if ((int)listInventoryIndexs[i] == objectIndex)
			{
				result = i;
				break;
			}
		}
		return result;
	}

	private int settingInventoryLocation(int objectIndex)
	{
		int result = -1;
		int count = listInventoryIndexs.Count;
		for (int i = 0; i < count; i++)
		{
			if ((int)listInventoryIndexs[i] == -1)
			{
				result = i;
				break;
			}
		}
		return result;
	}

	private void removeInventoryLocationIndex(int index)
	{
		listInventoryIndexs[index] = -1;
	}

	public override void onStart()
	{
		base.onStart();
		selectIndex = -1;
		base.gameObject.SetActive(value: true);
		if (dataManager == null)
		{
			dataManager = Singleton<DataManager>.Instance;
			playerManager = Singleton<PlayerManager>.Instance;
			soundManager = Singleton<SoundManager>.Instance;
			tutorialManager = Singleton<TutorialManager>.Instance;
		}
		tutorialManager.onInventoryAttach();
		if (playerComponent == null)
		{
			playerComponent = playerObject.GetComponent<Player>();
		}
		inventoryListSetting(EquipmentType.TYPE_WEAPON);
		refreshinventoryList();
		refreshStatus();
		refreshPlayer();
		firstSelectEquipment();
		backgroundTransform.localScale = new Vector3(0f, 0f, 0f);
		backgroundTransform.DOScale(1f, 0.5f).SetEase(Ease.OutBack);
		panel.DOFade(0.6f, 0.5f);
	}

	public override void onExit()
	{
		base.onExit();
		particleNormal.gameObject.SetActive(value: false);
		particleUnique.gameObject.SetActive(value: false);
		particleLegendary.gameObject.SetActive(value: false);
		particleSuperLegendary.gameObject.SetActive(value: false);
		particleAttachUpgrade.gameObject.SetActive(value: false);
		onDelegate();
		backgroundTransform.DOScale(0f, 0.5f).SetEase(Ease.InBack).OnComplete(delegate
		{
			base.gameObject.SetActive(value: false);
		});
		panel.DOFade(0f, 0.5f);
		Singleton<SoundManager>.Instance.playSound("uiClick");
	}

	public void onUpgrade()
	{
		Singleton<UIControlManager>.Instance.onPopupYesNo("Do you want to upgrade?", delegate
		{
			if (selectIndex != -1)
			{
				int num = inventoryLocationToData(selectIndex);
				if (num != -1)
				{
					EquipmentData equipmentData = listInventoryData[num];
					int upgradeEquipment = Price.getUpgradeEquipment(equipmentData.rank, equipmentData.level);
					if ((int)dataManager.coinCount < upgradeEquipment)
					{
						Singleton<UIControlManager>.Instance.onPopupYesNo("Not enough money!");
					}
					else if (equipmentData.rank != EquipmentRank.TYPE_SUPERLEGENDARY)
					{
						Singleton<UIControlManager>.Instance.onPopupYesNo("Only SUPER LEGENDARY can be upgraded");
					}
					else
					{
						StartCoroutine(upgradeAction());
					}
				}
			}
		}, delegate
		{
		});
		Singleton<SoundManager>.Instance.playSound("uiClick");
	}

	private IEnumerator upgradeAction()
	{
		upgradePanel.gameObject.SetActive(value: true);
		selectEquipmentImage.material = spriteWhiteMaterial;
		float amountValue = 0f;
		while (true)
		{
			selectEquipmentImage.material.SetFloat("_FlashAmount", amountValue);
			amountValue += Time.deltaTime * 1.5f;
			if (amountValue > 1f)
			{
				break;
			}
			yield return null;
		}
		selectEquipmentImage.material.SetFloat("_FlashAmount", 1f);
		yield return new WaitForSeconds(0.5f);
		upgradePanel.DOFade(1f, 0.2f);
		yield return new WaitForSeconds(0.5f);
		upgradeActive();
		upgradePanel.DOKill();
		upgradePanel.DOFade(0f, 0.2f);
		selectEquipmentImage.material = spriteDefaultMaterial;
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
		if (selectIndex == -1)
		{
			return;
		}
		int num = inventoryLocationToData(selectIndex);
		if (num != -1)
		{
			EquipmentData equipmentData = listInventoryData[num];
			int upgradeEquipment = Price.getUpgradeEquipment(equipmentData.rank, equipmentData.level);
			++equipmentData.level;
			if ((int)equipmentData.power > 0)
			{
				ref ObscuredInt power = ref equipmentData.power;
				power = (int)power + (int)Price.upgradePower(equipmentData.rank, equipmentData.level, (int)equipmentData.power);
			}
			if ((int)equipmentData.shield > 0)
			{
				ref ObscuredInt shield = ref equipmentData.shield;
				shield = (int)shield + (int)Price.upgradeShield(equipmentData.rank, equipmentData.level, (int)equipmentData.shield);
			}
			if ((int)equipmentData.hp > 0)
			{
				ref ObscuredInt hp = ref equipmentData.hp;
				hp = (int)hp + (int)Price.upgradeHP(equipmentData.rank, equipmentData.level, (int)equipmentData.hp);
			}
			if ((float)equipmentData.critical > 0.1f)
			{
				ref ObscuredFloat critical = ref equipmentData.critical;
				critical = (float)critical + Price.upgradeCritical(equipmentData.rank, equipmentData.level, equipmentData.critical);
			}
			if ((float)equipmentData.speed > 0.1f)
			{
				ref ObscuredFloat speed = ref equipmentData.speed;
				speed = (float)speed + Price.upgradeSpeed(equipmentData.rank, equipmentData.level, equipmentData.speed);
			}
			listInventoryData[num] = equipmentData;
			if ((int)equipmentData.objectIndex == (int)dataManager.selectArmor.objectIndex)
			{
				dataManager.selectArmor = listInventoryData[num];
			}
			else if ((int)equipmentData.objectIndex == (int)dataManager.selectHelmet.objectIndex)
			{
				dataManager.selectHelmet = listInventoryData[num];
			}
			else if ((int)equipmentData.objectIndex == (int)dataManager.selectWeapon.objectIndex)
			{
				dataManager.selectWeapon = listInventoryData[num];
			}
			else if ((int)equipmentData.objectIndex == (int)dataManager.selectHorse.objectIndex)
			{
				dataManager.selectHorse = listInventoryData[num];
			}
			DataManager obj = dataManager;
			obj.coinCount = (int)obj.coinCount - upgradeEquipment;
			Singleton<UIControlManager>.Instance.setCoinUI(dataManager.coinCount);
			activeInventoryChange();
			dataManager.saveDataAsync();
			refreshPlayer();
			onSelectStatus();
			onSelectButtons();
			refreshinventoryList();
			refreshSelectEquipmentImage();
		}
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
					int count3 = listSellSelectIndexs.Count;
					for (int k = 0; k < count3; k++)
					{
						int index = listSellSelectIndexs[k];
						int num = inventoryLocationToData(index);
						if (num != -1)
						{
							EquipmentData equipmentData = listInventoryData[num];
							if ((int)dataManager.selectArmor.objectIndex != (int)equipmentData.objectIndex && (int)dataManager.selectHelmet.objectIndex != (int)equipmentData.objectIndex && (int)dataManager.selectWeapon.objectIndex != (int)equipmentData.objectIndex && (int)dataManager.selectHorse.objectIndex != (int)equipmentData.objectIndex)
							{
								int saleEquipment = Price.getSaleEquipment(equipmentData.rank, equipmentData.level, equipmentData.imageIndex);
								listInventoryData.Remove(equipmentData);
								listInventoryIndexs[index] = -1;
								DataManager obj = dataManager;
								obj.coinCount = (int)obj.coinCount + saleEquipment;
								obj.maxCoinCount = (int)obj.maxCoinCount + saleEquipment;
							}
						}
					}
					selectIndex = -1;
					Singleton<UIControlManager>.Instance.setCoinUI(dataManager.coinCount);
					offSelectStatus();
					refreshinventoryList();
					refreshSelectEquipmentImage();
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

	public void onSelect()
	{
		offSelectButtons();
		activeSelectEquipment();
		dataManager.saveDataAsync();
		selectIndex = -1;
		refreshStatus();
		refreshPlayer();
		refreshinventoryList();
		soundManager.playSound("equipmentChange");
	}

	public void onWeapon()
	{
		offSelectStatus();
		offSelectButtons();
		selectIndex = -1;
		refreshStatus();
		refreshSelectEquipmentImage();
		inventoryListSetting(EquipmentType.TYPE_WEAPON);
		refreshinventoryList();
		refreshPlayer();
		firstSelectEquipment();
		Singleton<SoundManager>.Instance.playSound("uiClick");
	}

	public void onArmor()
	{
		offSelectStatus();
		offSelectButtons();
		selectIndex = -1;
		refreshStatus();
		refreshSelectEquipmentImage();
		inventoryListSetting(EquipmentType.TYPE_ARMOR);
		refreshinventoryList();
		refreshPlayer();
		firstSelectEquipment();
		Singleton<SoundManager>.Instance.playSound("uiClick");
	}

	public void onHelmet()
	{
		offSelectStatus();
		offSelectButtons();
		selectIndex = -1;
		refreshStatus();
		refreshSelectEquipmentImage();
		inventoryListSetting(EquipmentType.TYPE_HELMET);
		refreshinventoryList();
		refreshPlayer();
		firstSelectEquipment();
		Singleton<SoundManager>.Instance.playSound("uiClick");
	}

	public void onHorse()
	{
		offSelectStatus();
		offSelectButtons();
		selectIndex = -1;
		refreshStatus();
		refreshSelectEquipmentImage();
		inventoryListSetting(EquipmentType.TYPE_HORSE);
		refreshinventoryList();
		refreshPlayer();
		firstSelectEquipment();
		Singleton<SoundManager>.Instance.playSound("uiClick");
	}


/*	,{\\\"objectIndex\\\":680,\\\"type\\\":2,\\\"rank\\\":0,\\\"grade\\\":0,
	\\\"level\\\":0,\\\"index\\\":10000,\\\"power\\\":0,\\\"critical\\\":0,\\\"hp\\\":0,
	\\\"shield\\\":0,\\\"speed\\\":0,\\\"addPower\\\":0,\\\"addCritical\\\":0,\\\"addHp
	\\\":0,\\\"addShield\\\":0,\\\"addSpeed\\\":0}*/

	public void onChoice(int index)
	{
		Debug.Log("=====================1");
		selectIndex = index;
		int index2 = inventoryLocationToData(selectIndex);
		EquipmentData equipmentData = listInventoryData[index2];
		if ((int)equipmentData.imageIndex >= 10000)
		{
			selectIndex = -1;
			return;
		}
		EquipmentData weapon = dataManager.selectWeapon;
		EquipmentData armor = dataManager.selectArmor;
		EquipmentData helmet = dataManager.selectHelmet;
		EquipmentData horse = dataManager.selectHorse;
		if ((int)equipmentData.imageIndex > 10)
		{
			switch (selectInventoryType)
			{
			case EquipmentType.TYPE_ARMOR:
				iconLegendaryArmor.gameObject.SetActive(value: true);
				iconLegendaryArmor.sprite = listSkillIcons[(int)equipmentData.imageIndex - 11];
				break;
			case EquipmentType.TYPE_HELMET:
				iconLegendaryHelmet.gameObject.SetActive(value: true);
				iconLegendaryHelmet.sprite = listSkillIcons[(int)equipmentData.imageIndex - 11];
				break;
			case EquipmentType.TYPE_WEAPON:
				iconLegendaryWeapon.gameObject.SetActive(value: true);
				iconLegendaryWeapon.sprite = listSkillIcons[(int)equipmentData.imageIndex - 11];
				break;
			case EquipmentType.TYPE_HORSE:
				iconLegendaryHorse.gameObject.SetActive(value: true);
				iconLegendaryHorse.sprite = listSkillIcons[(int)equipmentData.imageIndex - 11];
				break;
			}
		}
		else
		{
			switch (selectInventoryType)
			{
			case EquipmentType.TYPE_ARMOR:
				iconLegendaryArmor.gameObject.SetActive(value: false);
				break;
			case EquipmentType.TYPE_HELMET:
				iconLegendaryHelmet.gameObject.SetActive(value: false);
				break;
			case EquipmentType.TYPE_WEAPON:
				iconLegendaryWeapon.gameObject.SetActive(value: false);
				break;
			case EquipmentType.TYPE_HORSE:
				iconLegendaryHorse.gameObject.SetActive(value: false);
				break;
			}
		}
		switch (selectInventoryType)
		{
		case EquipmentType.TYPE_ARMOR:
			armor = equipmentData;
			break;
		case EquipmentType.TYPE_HELMET:
			helmet = equipmentData;
			break;
		case EquipmentType.TYPE_WEAPON:
			weapon = equipmentData;
			break;
		case EquipmentType.TYPE_HORSE:
			horse = equipmentData;
			break;
		}
		onSelectStatus();
		onSelectButtons();
		refreshStatus();
		refreshSelectEquipmentImage();
		refreshPlayer();
		refreshLegendaryCount(weapon, armor, helmet, horse);
		soundManager.playSound("uiEquipmentSelect");
	}

	public void onSkillData()
	{
		Singleton<SoundManager>.Instance.playSound("uiClick");
		Singleton<UIControlManager>.Instance.onEquipmentSkillPopup();
	}

	public void onInventorySorting()
	{
		listInventoryData.Sort(delegate(EquipmentData a, EquipmentData b)
		{
			if ((int)a.imageIndex == 10000)
			{
				return 1;
			}
			if ((int)b.imageIndex == 10000)
			{
				return -1;
			}
			if (a.rank > b.rank)
			{
				return -1;
			}
			if (a.rank == b.rank)
			{
				if ((int)a.level > (int)b.level)
				{
					return -1;
				}
				if ((int)a.imageIndex > (int)b.imageIndex)
				{
					return -1;
				}
			}
			return 1;
		});
		int count = listInventoryData.Count;
		int count2 = listInventoryIndexs.Count;
		for (int i = 0; i < count2; i++)
		{
			if (i >= count)
			{
				listInventoryIndexs[i] = -1;
			}
			else
			{
				listInventoryIndexs[i] = listInventoryData[i].objectIndex;
			}
		}
		selectIndex = -1;
		refreshinventoryList();
		refreshSelectEquipmentImage();
		offSelectStatus();
		offSelectButtons();
		dataManager.saveDataAsync();
		soundManager.playSound("itemSorting");
	}
}
