using DG.Tweening;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
	public Image hpImage;

	public Image hpActionImage;

	public Image helmetUI;

	public Image armorUI;

	public Image weaponUI;

	public Image horseUI;

	public List<Sprite> helmetBoxImages = new List<Sprite>();

	public List<Sprite> armorBoxImages = new List<Sprite>();

	public List<Sprite> weaponBoxImages = new List<Sprite>();

	public List<Sprite> horseBoxImages = new List<Sprite>();

	public Image helmetBox;

	public Image armorBox;

	public Image weaponBox;

	public Image horseBox;

	public TextMeshProUGUI textPower;

	public TextMeshProUGUI textCritical;

	public TextMeshProUGUI textShield;

	public TextMeshProUGUI textSpeed;

	public TextMeshProUGUI textHP;

	public TextMeshProUGUI textCoin;

	public TextMeshProUGUI textEgg;

	public Image warningEffect;

	public RectTransform settingUI;

	public RectTransform cointBG;

	public RectTransform coinText;

	public RectTransform eggBG;

	public RectTransform eggText;

	public RectTransform facebookUI;

	public RectTransform rateusUI;

	private bool upState;

	private Sequence sequence;

	private float oldHPpercent = 1f;

	private void Start()
	{
		facebookUI.gameObject.SetActive(false);
		rateusUI.gameObject.SetActive(false);
	}

	public void setHpPercent(float per)
	{
		float num = (per < 0f) ? 0f : per;
		hpImage.fillAmount = num;
		hpActionImage.transform.DOKill();
		hpActionImage.transform.DOScaleX(num, 1f);
		checkWarningEffect(num);
		oldHPpercent = num;
	}

	public void setCoin(int coin)
	{
		textCoin.text = coin.ToString();
	}

	public void setEgg(int egg)
	{
		textEgg.text = egg.ToString();
	}

	public void refreshStatus(int power, float critical, int shield, float speed, int hp)
	{
		textPower.text = "+" + power.ToString();
		textCritical.text = "+" + critical.ToString("0.0") + "%";
		textShield.text = "+" + shield.ToString();
		textSpeed.text = "+" + speed.ToString("0.0");
		textHP.text = "+" + hp.ToString();
	}

	public void setHelmetIndex(EquipmentData data)
	{
		Sprite texture = Singleton<AssetManager>.Instance.LoadSprite("Equipment/Helmet/" + data.imageIndex.ToString());
		equipmentUIimageSetting(helmetUI, texture, 100f);
		helmetBox.sprite = helmetBoxImages[(int)data.rank];
		helmetBox.SetNativeSize();
	}

	public void setArmorIndex(EquipmentData data)
	{
		Sprite texture = Singleton<AssetManager>.Instance.LoadSprite("Equipment/Armor/" + data.imageIndex.ToString());
		equipmentUIimageSetting(armorUI, texture, 100f);
		armorBox.sprite = armorBoxImages[(int)data.rank];
		armorBox.SetNativeSize();
	}

	public void setWeaponIndex(EquipmentData data)
	{
		Sprite texture = Singleton<AssetManager>.Instance.LoadSprite("Equipment/Weapon/" + data.imageIndex.ToString());
		equipmentUIimageSetting(weaponUI, texture, 150f);
		weaponBox.sprite = weaponBoxImages[(int)data.rank];
		weaponBox.SetNativeSize();
	}

	public void setHorseIndex(EquipmentData data)
	{
		Sprite texture = Singleton<AssetManager>.Instance.LoadSprite("Equipment/Horse/" + data.imageIndex.ToString());
		equipmentUIimageSetting(horseUI, texture, 150f);
		horseBox.sprite = horseBoxImages[(int)data.rank];
		horseBox.SetNativeSize();
	}

	private void equipmentUIimageSetting(Image image, Sprite texture, float maxSize)
	{
		image.sprite = texture;
		image.SetNativeSize();
		RectTransform component = image.gameObject.GetComponent<RectTransform>();
		Rect rect = component.rect;
		float num = (rect.width > rect.height) ? rect.width : rect.height;
		float num2 = maxSize / num;
		component.localScale = new Vector3(num2, num2, 1f);
	}

	public void onSettingButton()
	{
		Singleton<UIControlManager>.Instance.onSettingUI(null);
		Singleton<SoundManager>.Instance.playSound("uiClick");
	}

	public void onFacebookButton()
	{
		//Application.OpenURL("https://www.facebook.com/111percent/");
		//GameObject.Find("VillageSceneObjects").transform.Find("RankUI").gameObject.SetActive(true);
	}

	public void onRateusButton()
	{
		//Application.OpenURL("market://details?id=com.percent.wilknight");
		//GameObject.Find("VillageSceneObjects").transform.Find("RankUI").GetComponent<RankUI>().onStart();
	}

	public void onOpenRanking() {
		GameObject.Find("VillageSceneObjects").transform.Find("RankUI").GetComponent<RankUI>().onStart();
	}
	[DllImport("__Internal")]
	private static extern void BindTezos();

	public void bindTezos() {
		BindTezos();
	}



	private void checkWarningEffect(float nowPercent)
	{
		if (oldHPpercent > 0.3f && nowPercent < 0.3f)
		{
			warningEffect.gameObject.SetActive(value: true);
			if (sequence == null)
			{
				sequence = DOTween.Sequence();
				sequence.Append(warningEffect.DOFade(1f, 0.5f));
				sequence.Append(warningEffect.DOFade(0f, 0.5f));
				sequence.SetLoops(-1);
				sequence.Play();
			}
		}
		else if (oldHPpercent < 0.3f && nowPercent > 0.3f)
		{
			warningEffect.gameObject.SetActive(value: false);
		}
	}

	public void settingUIup()
	{
		if (!upState)
		{
			upState = true;
			//settingUI.localPosition += new Vector3(0f, 127f, 0f);
			facebookUI.localPosition += new Vector3(0f, 127f, 0f);
			rateusUI.localPosition += new Vector3(0f, 127f, 0f);
			cointBG.localPosition += new Vector3(0f, 127f, 0f);
			coinText.localPosition += new Vector3(0f, 127f, 0f);
			eggBG.localPosition += new Vector3(0f, 127f, 0f);
			eggText.localPosition += new Vector3(0f, 127f, 0f);
		}
	}
}
