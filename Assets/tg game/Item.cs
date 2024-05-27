using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
	[Header("OBJECTS")]
	public SpriteRenderer mainSprite;

	public SpriteRenderer shadow;

	public GameObject dropParticleNormal;

	private ParticleStartColorChange dropNormalParticleColor;

	private ParticleSystem dropNormalParticleSystem;

	public GameObject dropParticleLegendary;

	private ParticleStartColorChange dropLegendaryParticleColor;

	private ParticleSystem dropLegendaryParticleSystem;

	[Header("SPRITES")]
	public List<Sprite> listItemImages = new List<Sprite>();

	public List<Sprite> listEquipmentShadows = new List<Sprite>();

	public Sprite itemShadow;

	public Sprite eggShadow;

	private Transform cameraTransform;

	private Vector3 addPosition;

	private ItemState state;

	private EquipmentData equipmentData;

	private bool life;

	private SoundManager soundManager;

	public void settingItem()
	{
		dropNormalParticleColor = dropParticleNormal.GetComponent<ParticleStartColorChange>();
		dropNormalParticleSystem = dropParticleNormal.GetComponent<ParticleSystem>();
		dropLegendaryParticleColor = dropParticleLegendary.GetComponent<ParticleStartColorChange>();
		dropLegendaryParticleSystem = dropParticleLegendary.GetComponent<ParticleSystem>();
		cameraTransform = Camera.main.transform;
		addPosition = new Vector3(4.5f, 8.7f, 0f);
		soundManager = Singleton<SoundManager>.Instance;
	}

	public void onItem(ItemState s, EquipmentData data)
	{
		state = s;
		equipmentData = data;
		shadow.gameObject.SetActive(value: true);
		if (state == ItemState.TYPE_EQUIPMENT)
		{
			initEquipment();
		}
		else
		{
			initItem();
		}
		StartCoroutine(startLife());
	}

	private IEnumerator startLife()
	{
		yield return new WaitForSeconds(0.7f);
		life = true;
	}

	private void initItem()
	{
		mainSprite.sprite = listItemImages[(int)state];
		mainSprite.transform.localPosition = new Vector3(0f, 0.457f, 0f);
		mainSprite.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
		mainSprite.transform.localScale = new Vector3(1f, 1f, 1f);
		if (state == ItemState.TYPE_EGG)
		{
			shadow.sprite = eggShadow;
		}
		else
		{
			shadow.sprite = itemShadow;
		}
	}

	private void initEquipment()
	{
		string str = "Equipment/";
		float num = 1f;
		float z = 0f;
		switch (equipmentData.type)
		{
		case EquipmentType.TYPE_ARMOR:
			str += "Armor/";
			break;
		case EquipmentType.TYPE_HELMET:
			str += "Helmet/";
			break;
		case EquipmentType.TYPE_HORSE:
			str += "Horse/";
			num = 0.6f;
			break;
		case EquipmentType.TYPE_WEAPON:
			str += "Weapon/";
			z = 92f;
			break;
		}
		str += equipmentData.imageIndex.ToString();
		Sprite sprite = Singleton<AssetManager>.Instance.LoadSprite(str);
		mainSprite.sprite = sprite;
		shadow.sprite = listEquipmentShadows[(int)equipmentData.type];
		float pixelsPerUnit = sprite.pixelsPerUnit;
		float num2 = sprite.pivot.x / sprite.rect.size.x - 0.5f;
		float x = sprite.rect.size.x * (num2 * num) / pixelsPerUnit;
		if (equipmentData.type == EquipmentType.TYPE_WEAPON)
		{
			x = 0f;
		}
		Vector3 localPosition = new Vector3(x, 0.457f, 0f);
		mainSprite.transform.localPosition = localPosition;
		mainSprite.transform.localScale = new Vector3(num, num, num);
		mainSprite.transform.rotation = Quaternion.Euler(0f, 0f, z);
	}

	private Color rankByColor()
	{
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		switch (equipmentData.rank)
		{
		case EquipmentRank.TYPE_NORMAL:
			num = 255f;
			num2 = 255f;
			num3 = 255f;
			break;
		case EquipmentRank.TYPE_UNIQUE:
			num = 235f;
			num2 = 10f;
			num3 = 255f;
			break;
		case EquipmentRank.TYPE_LEGENDARY:
			num = 255f;
			num2 = 96f;
			num3 = 10f;
			break;
		case EquipmentRank.TYPE_SUPERLEGENDARY:
			num = 255f;
			num2 = 252f;
			num3 = 10f;
			break;
		}
		return new Color(num / 255f, num2 / 255f, num3 / 255f, 1f);
	}

	public void onActionJump(Vector3 target, float direction)
	{
		base.transform.position = target;
		Vector3 endValue = target;
		endValue.x += direction * UnityEngine.Random.Range(7f, 10f);
		base.transform.DOMove(endValue, 1.5f).SetEase(Ease.OutExpo);
		if (state == ItemState.TYPE_EQUIPMENT)
		{
			mainSprite.transform.DOLocalJump(new Vector3(0f, 0f, 0f), 0.9f, 1, 0.35f).SetRelative(isRelative: true).SetEase(Ease.Flash)
				.OnComplete(delegate
				{
					if (equipmentData.rank == EquipmentRank.TYPE_LEGENDARY || equipmentData.rank == EquipmentRank.TYPE_SUPERLEGENDARY)
					{
						dropParticleLegendary.gameObject.SetActive(value: true);
						dropLegendaryParticleSystem.Play();
						dropLegendaryParticleColor.settingColor(rankByColor());
						soundManager.playSound("equipmentDrop_Legendary");
					}
					else
					{
						dropParticleNormal.gameObject.SetActive(value: true);
						dropNormalParticleSystem.Play();
						dropNormalParticleColor.settingColor(rankByColor());
						soundManager.playSound("equipmentDrop_Normal");
					}
				});
			return;
		}
		Sequence sequence = DOTween.Sequence();
		sequence.Append(mainSprite.transform.DOLocalJump(new Vector3(0f, 0f, 0f), 0.9f, 1, 0.2f).SetRelative(isRelative: true).SetEase(Ease.Flash));
		sequence.Append(mainSprite.transform.DOLocalJump(new Vector3(0f, 0f, 0f), 0.5f, 1, 0.1f).SetRelative(isRelative: true).SetEase(Ease.Flash));
		sequence.Append(mainSprite.transform.DOLocalJump(new Vector3(0f, 0f, 0f), 0.2f, 1, 0.05f).SetRelative(isRelative: true).SetEase(Ease.Flash));
		sequence.Play();
	}

	public void removeItem()
	{
		life = false;
		base.transform.DOKill();
		if (state == ItemState.TYPE_EQUIPMENT)
		{
			StartCoroutine(updateEndedMove());
		}
		else
		{
			activeOff();
		}
	}

	private IEnumerator updateEndedMove()
	{
		shadow.gameObject.SetActive(value: false);
		while (true)
		{
			Vector3 a = cameraTransform.position + addPosition;
			Vector3 a2 = a - base.transform.position;
			a2.Normalize();
			base.transform.position += a2 * 50f * Time.deltaTime;
			if (Vector3.Distance(a, base.transform.position) <= 1f)
			{
				break;
			}
			yield return null;
		}
		shadow.gameObject.SetActive(value: false);
		activeOff();
	}

	private void activeOff()
	{
		dropParticleNormal.gameObject.SetActive(value: false);
		dropParticleLegendary.gameObject.SetActive(value: false);
		base.gameObject.SetActive(value: false);
	}

	public ItemState getItemState()
	{
		return state;
	}

	public EquipmentData getItemEquipmentData()
	{
		return equipmentData;
	}

	public bool isLife()
	{
		return life;
	}
}
