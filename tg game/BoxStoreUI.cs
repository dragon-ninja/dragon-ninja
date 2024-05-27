using CodeStage.AntiCheat.ObscuredTypes;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BoxStoreUI : BaseUI
{
	private ObscuredInt normalCoin = 900;

	private ObscuredInt uniqueCoin = 8000;

	private ObscuredInt randomCoin = 3000;

	public List<Sprite> listRankTextImage = new List<Sprite>();

	public Material materialWhite;

	public Material materialBase;

	public Image box1;

	public Image box2;

	public Image box3;

	public TextMeshProUGUI textMinus;

	public Image circle;

	public Image panelOpen;

	public Image getEquipmentImage;

	public Image getEquipmentRankImage;

	public GameObject petLight;

	private PetLight petLightComponent;

	public Transform background;

	public Image panel;

	private DataManager dataManager;

	private SoundManager soundManager;

	private GameObject openBox;

	private EquipmentData openData;

	private bool openEnded;

	public override void onStart()
	{
		base.onStart();
		if (dataManager == null)
		{
			dataManager = Singleton<DataManager>.Instance;
			soundManager = Singleton<SoundManager>.Instance;
		}
		if (petLightComponent == null)
		{
			petLightComponent = petLight.GetComponent<PetLight>();
		}
		base.gameObject.SetActive(value: true);
		background.localScale = new Vector3(0f, 0f, 0f);
		background.DOScale(1f, 0.5f).SetEase(Ease.OutBack);
		panel.DOFade(0.6f, 0.5f);
	}

	public override void onExit()
	{
		base.onExit();
		onDelegate();
		background.DOScale(0f, 0.5f).SetEase(Ease.InBack).OnComplete(delegate
		{
			base.gameObject.SetActive(value: false);
		});
		panel.DOFade(0f, 0.5f);
		Singleton<SoundManager>.Instance.playSound("uiClick");
	}

	private bool checkCoin(int coin)
	{
		if ((int)dataManager.coinCount < coin)
		{
			Singleton<UIControlManager>.Instance.onPopupYesNo("Not enough money!");
			return false;
		}
		return true;
	}

	private List<int> getItemType()
	{
		List<int> list = new List<int>();
		if (dataManager.listHelmets.Count < 16)
		{
			list.Add(0);
		}
		if (dataManager.listArmors.Count < 16)
		{
			list.Add(1);
		}
		if (dataManager.listWeapons.Count < 16)
		{
			list.Add(2);
		}
		if (dataManager.listHorses.Count < 16)
		{
			list.Add(3);
		}
		if (list.Count == 0)
		{
			Singleton<UIControlManager>.Instance.onPopupYesNo("Inventory is full. Empty and try again!");
			return null;
		}
		return list;
	}

	public void onNormalBox()
	{
		Singleton<SoundManager>.Instance.playSound("uiClick");
		int num = normalCoin;
		if (!checkCoin(normalCoin))
		{
			return;
		}
		List<int> itemType = getItemType();
		if (itemType != null)
		{
			openData = Balance.getBoxEquipmentData(itemType, EquipmentRank.TYPE_NORMAL);
			switch (openData.type)
			{
			case EquipmentType.TYPE_HELMET:
				dataManager.listHelmets.Add(openData);
				break;
			case EquipmentType.TYPE_ARMOR:
				dataManager.listArmors.Add(openData);
				break;
			case EquipmentType.TYPE_WEAPON:
				dataManager.listWeapons.Add(openData);
				break;
			case EquipmentType.TYPE_HORSE:
				dataManager.listHorses.Add(openData);
				break;
			}
			DataManager obj = dataManager;
			obj.coinCount = (int)obj.coinCount - num;
			Singleton<UIControlManager>.Instance.setCoinUI(dataManager.coinCount);
			openBox = box1.gameObject;
			StartCoroutine(startOpenAction());
		}
	}

	public void onUniqueBox()
	{
		Singleton<SoundManager>.Instance.playSound("uiClick");
		int num = uniqueCoin;
		if (!checkCoin(num))
		{
			return;
		}
		List<int> itemType = getItemType();
		if (itemType != null)
		{
			openData = Balance.getBoxEquipmentData(itemType, EquipmentRank.TYPE_UNIQUE);
			switch (openData.type)
			{
			case EquipmentType.TYPE_HELMET:
				dataManager.listHelmets.Add(openData);
				break;
			case EquipmentType.TYPE_ARMOR:
				dataManager.listArmors.Add(openData);
				break;
			case EquipmentType.TYPE_WEAPON:
				dataManager.listWeapons.Add(openData);
				break;
			case EquipmentType.TYPE_HORSE:
				dataManager.listHorses.Add(openData);
				break;
			}
			DataManager obj = dataManager;
			obj.coinCount = (int)obj.coinCount - num;
			Singleton<UIControlManager>.Instance.setCoinUI(dataManager.coinCount);
			openBox = box2.gameObject;
			if (!dataManager.firstUnique)
			{
				Singleton<AppCustomEventManager>.Instance.pushEvent("getPurple");
				dataManager.firstUnique = true;
				dataManager.saveDataAsync();
			}
			StartCoroutine(startOpenAction());
		}
	}

	public void onRandomBox()
	{
		Singleton<SoundManager>.Instance.playSound("uiClick");
		int num = randomCoin;
		if (!checkCoin(num))
		{
			return;
		}
		List<int> itemType = getItemType();
		if (itemType != null)
		{
			openData = Balance.getBoxAllRandomEquipmentData(itemType);
			switch (openData.type)
			{
			case EquipmentType.TYPE_HELMET:
				dataManager.listHelmets.Add(openData);
				break;
			case EquipmentType.TYPE_ARMOR:
				dataManager.listArmors.Add(openData);
				break;
			case EquipmentType.TYPE_WEAPON:
				dataManager.listWeapons.Add(openData);
				break;
			case EquipmentType.TYPE_HORSE:
				dataManager.listHorses.Add(openData);
				break;
			}
			DataManager obj = dataManager;
			obj.coinCount = (int)obj.coinCount - num;
			Singleton<UIControlManager>.Instance.setCoinUI(dataManager.coinCount);
			openBox = box3.gameObject;
			if (openData.rank == EquipmentRank.TYPE_UNIQUE && !dataManager.firstUnique)
			{
				Singleton<AppCustomEventManager>.Instance.pushEvent("getPurple");
				dataManager.firstUnique = true;
				dataManager.saveDataAsync();
			}
			StartCoroutine(startOpenAction());
		}
	}

	private IEnumerator startOpenAction()
	{
		dataManager.saveDataAsync();
		panelOpen.gameObject.SetActive(value: true);
		Vector3 boxPos = openBox.transform.localPosition;
		openBox.transform.DOShakePosition(2f, 30f, 20).SetEase(Ease.Flash);
		soundManager.playSound("ui_random_box_shake");
		yield return new WaitForSeconds(2f);
		openBox.transform.localPosition = boxPos;
		yield return new WaitForSeconds(0.5f);
		boxPos.y += 25f;
		circle.color = Color.white;
		circle.gameObject.SetActive(value: true);
		circle.transform.localPosition = boxPos;
		circle.transform.DOScale(new Vector3(200f, 200f, 200f), 0.2f);
		soundManager.playSound("ui_pet_gotcha_4");
		yield return new WaitForSeconds(1.5f);
		panelOpen.color = new Color(0f, 0f, 0f, 0.6f);
		string text = "";
		float z = 0f;
		switch (openData.type)
		{
		case EquipmentType.TYPE_ARMOR:
			text = "Armor";
			break;
		case EquipmentType.TYPE_WEAPON:
			text = "Weapon";
			z = 45f;
			break;
		case EquipmentType.TYPE_HELMET:
			text = "Helmet";
			break;
		case EquipmentType.TYPE_HORSE:
			text = "Horse";
			break;
		}
		getEquipmentImage.gameObject.SetActive(value: true);
		getEquipmentImage.sprite = Singleton<AssetManager>.Instance.LoadSprite("Equipment/" + text + "/" + openData.imageIndex);
		getEquipmentImage.transform.rotation = Quaternion.Euler(0f, 0f, z);
		getEquipmentImage.SetNativeSize();
		petLightComponent.gameObject.SetActive(value: true);
		petLightComponent.openLight(openData.rank);
		getEquipmentRankImage.gameObject.SetActive(value: true);
		getEquipmentRankImage.sprite = listRankTextImage[(int)openData.rank];
		getEquipmentRankImage.SetNativeSize();
		circle.DOFade(0f, 0.5f);
		switch (openData.rank)
		{
		}
		yield return new WaitForSeconds(0.5f);
		soundManager.playSound("ui_random_get");
		openEnded = true;
	}

	public void onCloseRandomEquipment()
	{
		if (openEnded)
		{
			openEnded = false;
			panelOpen.color = new Color(0f, 0f, 0f, 0f);
			panelOpen.gameObject.SetActive(value: false);
			circle.gameObject.SetActive(value: false);
			getEquipmentImage.gameObject.SetActive(value: false);
			petLightComponent.gameObject.SetActive(value: false);
			petLightComponent.closeLight();
			getEquipmentRankImage.gameObject.SetActive(value: false);
		}
	}
}
